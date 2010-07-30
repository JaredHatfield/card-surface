// <copyright file="BlackjackException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by Blackjack.</summary>
namespace GameBlackjack.BlackjackExceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame.GameException;

    /// <summary>
    /// An exception thrown by Blackjack.
    /// </summary>
    public class BlackjackException : CardGameException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlackjackException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public BlackjackException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlackjackException"/> class.
        /// </summary>
        public BlackjackException() :
            base("Blackjack: An error occured in the game.")
        {
        }
    }
}
