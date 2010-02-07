// <copyright file="CardPile.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary></summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public class CardPile : Pile
    {
        /// <summary>
        /// A card pile can be visually expanded to see what is in the pile.
        /// </summary>
        private Boolean expandable;

        /// <summary>
        /// Gets a value indicating whether this <see cref="CardPile"/> is expandable.
        /// </summary>
        /// <value><c>true</c> if expandable; otherwise, <c>false</c>.</value>
        public Boolean Expandable
        {
            get { return this.expandable; }
        }
    }
}
