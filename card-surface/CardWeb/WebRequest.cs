// <copyright file="WebRequest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Object that represents HTTP request.</summary>
namespace CardWeb
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using WebExceptions;

    /// <summary>
    /// Describes HTTP request
    /// </summary>
    public class WebRequest
    {
        /// <summary>
        /// Index for the HTTP Request type in a tokenized string
        /// </summary>
        private const int HttpRequestMethodIndex = 0;

        /// <summary>
        /// Index for the HTTP Request resource in a tokenized string
        /// </summary>
        private const int HttpRequestResourceIndex = 1;

        /// <summary>
        /// Index for the HTTP Request version in a tokenized string
        /// </summary>
        private const int HttpRequestVersionIndex = 2;

        /// <summary>
        /// Socket used for receiving request data
        /// </summary>
        private Socket socket;

        /// <summary>
        /// Raw request data received
        /// </summary>
        private byte[] request;

        /// <summary>
        /// HTTP Request method (POST, GET, etc.)
        /// </summary>
        private string requestMethod;

        /// <summary>
        /// HTTP Request version
        /// </summary>
        private string requestVersion;

        /// <summary>
        /// Requested WebComponent resource
        /// </summary>
        private string requestResource;

        /// <summary>
        /// Content or POST data contained in HTTP request
        /// </summary>
        private string requestContent;

        /// <summary>
        /// Dictionary that contains key\value pairs for HTTP GET and POST request data.
        /// </summary>
        private Dictionary<string, string> urlParameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebRequest"/> class.
        /// </summary>
        /// <param name="requestData">The request data.</param>
        /// <param name="connection">The connection.</param>
        public WebRequest(byte[] requestData, Socket connection)
        {
            this.urlParameters = new Dictionary<string, string>();

            this.request = requestData;
            this.socket = connection;

            /* Retrieve requestMethod before any other properties. */
            this.requestMethod = this.GetHttpRequestMethod(this.request);
            this.requestVersion = this.GetHttpRequestVersion(this.request);
            this.requestResource = this.GetHttpRequestResource(this.request);
            this.requestContent = this.GetHttpRequestContent(this.request);
        } /* WebRequest() */

        /// <summary>
        /// Initializes a new instance of the <see cref="WebRequest"/> class.
        /// </summary>
        internal WebRequest()
        {
            this.urlParameters = new Dictionary<string, string>();

            this.request = null;
            this.socket = null;
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        public Socket Connection
        {
            get { return this.socket; }
        }

        /// <summary>
        /// Gets the request data.
        /// </summary>
        /// <value>The request data.</value>
        public byte[] Data
        {
            get { return this.request; }
        }

        /// <summary>
        /// Gets the request method.
        /// </summary>
        /// <value>The request method.</value>
        public string RequestMethod
        {
            get { return this.requestMethod; }
        }

        /// <summary>
        /// Gets the request version.
        /// </summary>
        /// <value>The request version.</value>
        public string RequestVersion
        {
            get { return this.requestVersion; }
        }

        /// <summary>
        /// Gets the request resource.
        /// </summary>
        /// <value>The request resource.</value>
        public string RequestResource
        {
            get { return this.requestResource; }
        }

        /// <summary>
        /// Gets the content of the request.
        /// </summary>
        /// <value>The content of the request.</value>
        public string RequestContent
        {
            get { return this.requestContent; }
        }

        /// <summary>
        /// Determines whether this instance is authenticated.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAuthenticated()
        {
            if (this.ContainsCookie())
            {
                WebCookie requestCookie = this.ExtractCookie();

                foreach (WebSession session in WebSessionController.Instance.Sessions)
                {
                    if (session.SessionId == requestCookie.Csid)
                    {
                        return true;
                    }
                }
            }

            return false;
        } /* IsAuthenticated() */

        /// <summary>
        /// Gets the session id.
        /// </summary>
        /// <returns>A Guid representing the WebSession ID</returns>
        public Guid GetSessionId()
        {
            if (this.ContainsCookie())
            {
                WebCookie requestCookie = this.ExtractCookie();
                return requestCookie.Csid;
            }
            else
            {
                throw new WebServerException("No cookie found in HTTP request");
            }
        } /* GetSessionId() */

        /// <summary>
        /// Gets the URL parameter.
        /// </summary>
        /// <param name="key">The search key.</param>
        /// <returns>
        /// A string value that matches the key entry.
        /// </returns>
        public string GetUrlParameter(string key)
        {
            string value;

            if (this.urlParameters.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                throw new WebServerUrlParameterNotFoundException(key);
            }
        } /* GetUrlParameter() */

        /// <summary>
        /// Determines whether this instance contains a cookie.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance contains a cookie; otherwise, <c>false</c>.
        /// </returns>
        private bool ContainsCookie()
        {
            /* TODO: Add security checks.  Is the "Cookie:" line properly formatted?  Does it appear before the content? */
            bool patternFound = false;
            byte[] pattern = Encoding.ASCII.GetBytes(WebCookie.HttpCookieHeader);

            for (int i = 0; i < this.request.Length - pattern.Length; i++)
            {
                if (this.request[i] == pattern[0])
                {
                    /* Assume the pattern has been found. */
                    patternFound = true;
                    for (int j = 0; j < pattern.Length; j++)
                    {
                        if (this.request[i + j] != pattern[j])
                        {
                            /* If there's not a match, we didn't really find it. */
                            patternFound = false;
                            break;
                        }
                    }

                    if (patternFound)
                    {
                        /* If we really did find the pattern, we can stop searching. */
                        break;
                    }
                }
            }

            return patternFound;
        } /* ContainsCookie() */

        /// <summary>
        /// Extracts the cookie.
        /// </summary>
        /// <returns>A WebCookie containing the data present in the HTTP request.</returns>
        private WebCookie ExtractCookie()
        {
            /* TODO: Add security checks.  Is the "Cookie:" line properly formatted?  Don't pull something out of the content! */
            if (this.ContainsCookie())
            {
                WebCookie requestCookie;
                string cookieLine = String.Empty;

                /* Start extracting the cookie data after the HTTP cookie header */
                int cookieLinePosition = Encoding.ASCII.GetString(this.request).IndexOf(WebCookie.HttpCookieHeader) + WebCookie.HttpCookieHeader.Length;
                int cookieEndOfLinePosition = Encoding.ASCII.GetString(this.request).IndexOf(new string(new char[] { WebUtilities.CarriageReturn, WebUtilities.LineFeed }), cookieLinePosition);

                for (int i = cookieLinePosition; i < cookieEndOfLinePosition; i++)
                {
                    if ((char)this.request[i] != WebUtilities.CarriageReturn && (char)this.request[i] != WebUtilities.LineFeed)
                    {
                        cookieLine += (char)this.request[i];
                    }
                }

                string[] cookieLineTokens = cookieLine.Split(new char[] { ';' });

                for (int i = 0; i < cookieLineTokens.Length; i += 2)
                {
                    string[] cookieValuePairTokens = cookieLineTokens[i].Split(new char[] { '=' });
                    if (cookieValuePairTokens[0].Trim().Equals(WebCookie.CsidIdentifier))
                    {
                        requestCookie = new WebCookie(new Guid(cookieValuePairTokens[1]));
                        return requestCookie;
                    }
                }
            }
            else
            {
                throw new WebServerException("No cookie found");
            }

            /* If we didn't find our cookie identifier, return an WebCookie constructed with an empty Guid (which will never be authenticated). */
            return new WebCookie(Guid.Empty);
        } /* ExtractCookie() */

        /// <summary>
        /// Gets the content of the HTTP request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A byte array containing the request contents.</returns>
        private string GetHttpRequestContent(byte[] request)
        {
            int position = 0;
            int bytesCopied = 0;
            bool patternFound = false;
            byte[] pattern = { (byte)WebUtilities.CarriageReturn, (byte)WebUtilities.LineFeed, (byte)WebUtilities.CarriageReturn, (byte)WebUtilities.LineFeed };
            string content = String.Empty;

            for (int i = 0; i < request.Length - pattern.Length; i++)
            {
                if (request[i] == pattern[0])
                {
                    /* Assume the pattern has been found. */
                    patternFound = true;
                    for (int j = 0; j < pattern.Length; j++)
                    {
                        if (request[i + j] != pattern[j])
                        {
                            /* If there's not a match, we didn't really find it. */
                            patternFound = false;
                            break;
                        }
                    }

                    /* If all bytes in the pattern array matched, we really did find it. */
                    if (patternFound)
                    {
                        /* Start the copy position after the \r\n\r\n content initiation sequence. */
                        position = i + pattern.Length;
                        break;
                    }
                }
            }

            /* Copy the content portion of the HTTP request to position. */
            for (int i = position; i < request.Length; i++)
            {
                /* Skip null characters. */
                if (request[i] != 0x0)
                {
                    content += (char)request[i];
                    bytesCopied++;
                }
            }

            /* TODO: Verify that all the bytes specified in the Content-Length property have actually been captured from the port! */
            /* TODO: How should this request be handled if it is a partial request?  Check that all content bytes received before processing? */
            Debug.WriteLine("GetHttpRequestContent@WebController: Copied " + bytesCopied + " bytes from the HTTP request content.");

            if (this.requestMethod == WebRequestMethods.Http.Post)
            {
                this.ParseUrlParameters(content);
            }

            return content;
        } /* GetHttpRequestContent() */

        /// <summary>
        /// Gets the length of the HTTP request content.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The number of bytes specified in the Content-Length property of the request.</returns>
        private int GetHttpRequestContentLength(byte[] request)
        {
            /* TODO: Implement GetHttpRequestContentLength? */
            throw new NotImplementedException();
        } /* GetHttpRequestContentLength() */

        /// <summary>
        /// Gets the HTTP request method.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A string representation of the HTTP Request Method</returns>
        private string GetHttpRequestMethod(byte[] request)
        {
            byte requestLineTerminator = 0x0;
            string firstLineOfRequest = String.Empty;

            for (int i = 0; i < request.Length; i++)
            {
                if (request[i] != WebUtilities.CarriageReturn && request[i] != WebUtilities.LineFeed)
                {
                    firstLineOfRequest += (char)request[i];
                }
                else
                {
                    requestLineTerminator |= request[i];
                }

                if (requestLineTerminator == (WebUtilities.CarriageReturn | WebUtilities.LineFeed))
                {
                    /* We've captured the first line of the HTTP request. */
                    break;
                }
            }

            /* Tokenize first line of HTTP request. */
            string[] firstLineTokens = firstLineOfRequest.Split(new char[] { ' ' });

            if (firstLineTokens[HttpRequestMethodIndex].Equals(WebRequestMethods.Http.Get))
            {
                return WebRequestMethods.Http.Get;
            }
            else if (firstLineTokens[HttpRequestMethodIndex].Equals(WebRequestMethods.Http.Post))
            {
                return WebRequestMethods.Http.Post;
            }
            else
            {
                throw new InvalidOperationException("Unrecognized HTTP request method");
            }
        } /* GetHttpRequestMethod() */

        /// <summary>
        /// Gets the HTTP request resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A string representation of the requested HTTP Resource.</returns>
        private string GetHttpRequestResource(byte[] request)
        {
            byte requestLineTerminator = 0x0;
            string firstLineOfRequest = String.Empty;

            for (int i = 0; i < request.Length; i++)
            {
                if (request[i] != WebUtilities.CarriageReturn && request[i] != WebUtilities.LineFeed)
                {
                    firstLineOfRequest += (char)request[i];
                }
                else
                {
                    requestLineTerminator |= request[i];
                }

                if (requestLineTerminator == (WebUtilities.CarriageReturn | WebUtilities.LineFeed))
                {
                    /* We've captured the first line of the HTTP request. */
                    break;
                }
            }

            /* Tokenize first line of HTTP request. */
            string[] firstLineTokens = firstLineOfRequest.Split(new char[] { ' ' });
            string[] resourceTokens = firstLineTokens[HttpRequestResourceIndex].Split(new char[] { '/' });

            /* Trim off leading '/' part of URI prefix and any trailing ?key=value&key=value... pairs */
            /* TODO: What if the request terminator equals 0?  We shouldn't do the same thing.  Ex: http://localhost/?gid=aekjaka Do we still need to parse? */
            int resourceTerminator = resourceTokens[HttpRequestResourceIndex].IndexOf('?');
            if (resourceTerminator >= 0)
            {
                if (this.requestMethod == WebRequestMethods.Http.Get)
                {
                    /* Parse the URL for its key\value pairs.  Remove the actual resource name and the ? separator. 
                       But only parse the parameters that followed for GET requests.  POST requests should contain
                       all of their key\value pairs in the request content.  Ignore leftover URL parameters on POST
                       requests. */
                    this.ParseUrlParameters(resourceTokens[HttpRequestResourceIndex].Remove(0, resourceTerminator + 1));
                }

                /* If the resource name contained trailing URL parameters, remove them. */
                return resourceTokens[HttpRequestResourceIndex].Remove(resourceTerminator);
            }
            else
            {
                return resourceTokens[HttpRequestResourceIndex];
            }
        } /* GetHttpRequestResource() */

        /// <summary>
        /// Gets the HTTP request version.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A string representation of the HTTP request version.</returns>
        private string GetHttpRequestVersion(byte[] request)
        {
            byte requestLineTerminator = 0x0;
            string firstLineOfRequest = String.Empty;

            for (int i = 0; i < request.Length; i++)
            {
                if (request[i] != WebUtilities.CarriageReturn && request[i] != WebUtilities.LineFeed)
                {
                    firstLineOfRequest += (char)request[i];
                }
                else
                {
                    requestLineTerminator |= request[i];
                }

                if (requestLineTerminator == (WebUtilities.CarriageReturn | WebUtilities.LineFeed))
                {
                    /* We've captured the first line of the HTTP request. */
                    break;
                }
            }

            /* Tokenize first line of HTTP request. */
            string[] firstLineTokens = firstLineOfRequest.Split(new char[] { ' ' });

            return firstLineTokens[HttpRequestVersionIndex];
        } /* GetHttpRequestVersion() */

        /// <summary>
        /// Parses the URL parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        private void ParseUrlParameters(string parameters)
        {
            /* TODO: What if a parameter is entered twice?  How should that be handled? */
            string[] tokens = parameters.Split(new char[] { '&', '=' });

            for (int i = 0; i < tokens.Length; i += 2)
            {
                this.urlParameters.Add(tokens[i], tokens[i + 1]);
            }
        } /* ParseUrlParameters() */
    }
}
