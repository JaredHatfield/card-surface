// <copyright file="CardAccountCreationException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by CardAccountCreationException.</summary>
namespace CardAccount.AccountException
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception that indicates a new account could not be created.
    /// </summary>
    public class CardAccountCreationException : CardAccountException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardAccountCreationException"/> class.
        /// </summary>
        public CardAccountCreationException()
            : base("CardAccount: New account could not be created.")
        {
        }
    }
}
