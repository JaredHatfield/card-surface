// <copyright file="BlackjackInactivePlayerException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by Blackjack when an inactive player is accessed.</summary>
namespace GameBlackjack.BlackjackExceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception thrown by Blackjack when an inactive player is accessed.
    /// </summary>
    public class BlackjackInactivePlayerException : BlackjackException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlackjackInactivePlayerException"/> class.
        /// </summary>
        public BlackjackInactivePlayerException()
            : base("Blackjack: The player that was attempted to be accessed is not playing the game.")
        {
        }
    }
}
