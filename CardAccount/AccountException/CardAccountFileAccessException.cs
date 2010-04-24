// <copyright file="CardAccountFileAccessException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by CardAccountFileAccessException.</summary>
namespace CardAccount.AccountException
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception that indicates a new account could not be created.
    /// </summary>
    public class CardAccountFileAccessException : CardAccountException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardAccountFileAccessException"/> class.
        /// </summary>
        public CardAccountFileAccessException()
            : base("CardAccount: File access failed.")
        {
        }
    }
}
