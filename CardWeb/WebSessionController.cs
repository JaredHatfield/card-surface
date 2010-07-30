// <copyright file="WebSessionController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A singleton class that manages authenticated WebSessions.</summary>
namespace CardWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using WebExceptions;

    /// <summary>
    /// Singleton class that controlls HTTP sessions
    /// </summary>
    public class WebSessionController
    {
        /// <summary>
        /// Singleton instance of the WebSessionController.
        /// </summary>
        private static readonly WebSessionController instance = new WebSessionController();

        /// <summary>
        /// List of authenticated sessions.
        /// </summary>
        private List<WebSession> sessions;

        /// <summary>
        /// Prevents a default instance of the WebSessionController class from being created.  Initializes a new instance of the <see cref="WebSessionController"/> class.
        /// </summary>
        private WebSessionController() 
        {
            this.sessions = new List<WebSession>();
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static WebSessionController Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Adds the session.
        /// </summary>
        /// <param name="newSession">The new session.</param>
        /// <exception cref="WebServerActiveSessionAlreadyExistsException"></exception>
        public void AddSession(WebSession newSession)
        {
            foreach (WebSession session in this.sessions)
            {
                if (session.Username.Equals(newSession.Username) && !session.HasExpired)
                {
                    throw new WebServerActiveSessionAlreadyExistsException(newSession);
                }
            }

            this.sessions.Add(newSession);
        }

        /// <summary>
        /// Removes the session.
        /// </summary>
        /// <param name="session">The session.</param>
        public void RemoveSession(WebSession session)
        {
            this.sessions.Remove(session);
        }

        /// <summary>
        /// Finds the session.
        /// </summary>
        /// <param name="csid">The CardSurface Session ID as a GUID.</param>
        /// <returns>The authenticated WebSession.</returns>
        /// <exception cref="WebServerSessionNotFoundException"></exception>
        public WebSession GetSession(Guid csid)
        {
            foreach (WebSession session in this.sessions)
            {
                if (session.SessionId.Equals(csid))
                {
                    return session;
                }
            }

            throw new WebServerSessionNotFoundException(csid);
        }

        /// <summary>
        /// Gets the active session.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The active WebSession for the specified user.</returns>
        /// <exception cref="WebServerActiveSessionNotFoundException"></exception>
        public WebSession GetActiveSession(string username)
        {
            foreach (WebSession session in this.sessions)
            {
                if (session.Username.Equals(username) && !session.HasExpired)
                {
                    /* WebSessionController guarantees that a user only has one active session. */
                    return session;
                }
            }

            throw new WebServerActiveSessionNotFoundException(username);
        }

        /// <summary>
        /// Determines whether a user still has an active session.
        /// Cannot be used to determine whether or not the user has ever had a session.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// <c>true</c> if the user's session is currently active; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUserSessionActive(string username)
        {
            foreach (WebSession session in this.sessions)
            {
                if (session.Username.Equals(username) && !session.HasExpired)
                {
                    /* If we're looking at the right user's session and it has not expired, the session is still active. */
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the given session id is active or not.
        /// </summary>
        /// <param name="id">The session id.</param>
        /// <returns>
        /// <c>true</c> if the session matching the specified id is active; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSessionActive(Guid id)
        {
            foreach (WebSession session in this.sessions)
            {
                if (session.SessionId == id && !session.HasExpired)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
