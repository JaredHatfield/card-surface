// <copyright file="BlackjackInvalidPlayerStateException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by Blackjack when a PlayerState attempts to go into an invalid state.</summary>
namespace GameBlackjack.BlackjackExceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception thrown by Blackjack when a PlayerState attempts to go into an invalid state.
    /// </summary>
    public class BlackjackInvalidPlayerStateException : BlackjackException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlackjackInvalidPlayerStateException"/> class.
        /// </summary>
        public BlackjackInvalidPlayerStateException()
            : base("Blackjack: The PlayerState has attempted to go into an invalid state.")
        {
        }
    }
}
