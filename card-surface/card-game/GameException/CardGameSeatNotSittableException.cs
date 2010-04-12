// <copyright file="CardGameSeatNotSittableException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by CardGame when someone tries to sit in a Seat they are not allowed to.</summary>
namespace CardGame.GameException
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception thrown by CardGame when someone tries to sit in a Seat they are not allowed to.
    /// </summary>
    public class CardGameSeatNotSittableException : CardGameException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardGameSeatNotSittableException"/> class.
        /// </summary>
        public CardGameSeatNotSittableException()
            : base("Players are not allowed to sit in this Seat.")
        {
        }
    }
}
