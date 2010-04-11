// <copyright file="BlackjackActionNotAllowedException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by Blackjack when an unauthroized action is attempted.</summary>
namespace GameBlackjack.BlackjackExceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception thrown by Blackjack when an unauthroized action is attempted.
    /// </summary>
    public class BlackjackActionNotAllowedException : BlackjackException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlackjackActionNotAllowedException"/> class.
        /// </summary>
        public BlackjackActionNotAllowedException()
            : base("Blackjack: User is not allowed to perform that action.")
        {
        }
    }
}
