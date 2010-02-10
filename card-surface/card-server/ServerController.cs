// <copyright file="ServerController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary></summary>
namespace CardServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardWeb;
    using CardCommunication;
    using CardAccount;

    /// <summary>
    /// 
    /// </summary>
    public class ServerController
    {
        /// <summary>
        /// 
        /// </summary>
        private GameController gameController;

        /// <summary>
        /// 
        /// </summary>
        private WebController webController;

        /// <summary>
        /// 
        /// </summary>
        private ServerCommunicationController serverCommunicationController;

        /// <summary>
        /// 
        /// </summary>
        private AccountController accountController;
    }
}
