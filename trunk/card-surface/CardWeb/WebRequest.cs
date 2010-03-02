// <copyright file="WebRequest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Object that represents HTTP request.</summary>
namespace CardWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

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
        /// Initializes a new instance of the <see cref="WebRequest"/> class.
        /// </summary>
        /// <param name="requestData">The request data.</param>
        /// <param name="connection">The connection.</param>
        public WebRequest(byte[] requestData, Socket connection)
        {
            this.request = requestData;
            this.socket = connection;

            this.requestMethod = this.GetHttpRequestMethod(this.request);
            this.requestVersion = this.GetHttpRequestVersion(this.request);
            this.requestResource = this.GetHttpRequestResource(this.request);
            this.requestContent = this.GetHttpRequestContent(this.request);
        } /* WebRequest() */

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
            Console.WriteLine("GetHttpRequestContent@WebController: Copied " + bytesCopied + " bytes from the HTTP request content.");

            return content;
        } /* GetHttpRequestContent() */

        /// <summary>
        /// Gets the length of the HTTP request content.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The number of bytes specified in the Content-Length property of the request.</returns>
        private int GetHttpRequestContentLength(byte[] request)
        {
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

            /* Trim off leading '/' part of URI prefix */
            return resourceTokens[1];
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
    }
}
