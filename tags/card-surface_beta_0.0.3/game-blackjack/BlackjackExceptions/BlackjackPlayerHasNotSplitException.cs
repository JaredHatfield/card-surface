// <copyright file="BlackjackPlayerHasNotSplitException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by Blackjack when a the second had is attempted to be accessed and the player has not split.</summary>
namespace GameBlackjack.BlackjackExceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception thrown by Blackjack when a the second had is attempted to be accessed and the player has not split.
    /// </summary>
    public class BlackjackPlayerHasNotSplitException : BlackjackException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlackjackPlayerHasNotSplitException"/> class.
        /// </summary>
        public BlackjackPlayerHasNotSplitException()
            : base("Blackjack: The player has not split.")
        {
        }
    }
}
