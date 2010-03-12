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
        /// Gets the sessions.
        /// </summary>
        /// <value>The sessions.</value>
        public List<WebSession> Sessions
        {
            get { return this.sessions; }
        }

        /// <summary>
        /// Finds the session.
        /// </summary>
        /// <param name="csid">The CardSurface Session ID as a GUID.</param>
        /// <returns>The authenticated WebSession.</returns>
        public WebSession GetSession(Guid csid)
        {
            foreach (WebSession session in this.sessions)
            {
                if (session.SessionId.Equals(csid))
                {
                    return session;
                }
            }

            throw new Exception("No matching session found");
        }
    }
}
