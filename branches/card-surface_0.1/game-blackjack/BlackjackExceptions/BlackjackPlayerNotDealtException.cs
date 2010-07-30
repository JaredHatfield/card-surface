// <copyright file="BlackjackPlayerNotDealtException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by Blackjack when a player is not dealt cards and is accessed.</summary>
namespace GameBlackjack.BlackjackExceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception thrown by Blackjack when a player is not dealt cards and is accessed.
    /// </summary>
    public class BlackjackPlayerNotDealtException : BlackjackException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlackjackPlayerNotDealtException"/> class.
        /// </summary>
        public BlackjackPlayerNotDealtException()
            : base("Blackjack: Attempted to access/modify PlayerState for non-dealt player.")
        {
        }
    }
}
