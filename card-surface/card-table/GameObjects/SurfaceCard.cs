// <copyright file="SurfaceCard.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Implements the SurfaceCard.</summary>
namespace CardTable.GameObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Windows.Controls;
    using CardGame;
    using Microsoft.Surface;
    using Microsoft.Surface.Presentation;
    using Microsoft.Surface.Presentation.Controls;

    /// <summary>
    /// The surface card.
    /// </summary>
    internal class SurfaceCard : Card
    {
        /// <summary>
        /// The object image source.
        /// </summary>
        private string objectImageSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceCard"/> class.
        /// </summary>
        /// <param name="id">The card's id.</param>
        /// <param name="suit">The card's suit.</param>
        /// <param name="face">The card's face.</param>
        /// <param name="status">The card's status.</param>
        internal SurfaceCard(Guid id, CardSuit suit, CardFace face, CardStatus status)
            : base(id, suit, face, status)
        {
            this.objectImageSource = this.FindCardImageSource();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceCard"/> class.
        /// </summary>
        /// <param name="suit">The card's suit.</param>
        /// <param name="face">The card's face.</param>
        /// <param name="status">The card's status.</param>
        internal SurfaceCard(CardSuit suit, CardFace face, CardStatus status)
            : base(suit, face, status)
        {
            this.objectImageSource = this.FindCardImageSource();
        }

        /// <summary>
        /// Gets the object image source.
        /// </summary>
        /// <value>The object image source.</value>
        public string ObjectImageSource
        {
            get
            {
                return this.objectImageSource;
            }
        }

        /// <summary>
        /// Finds the card image source.
        /// </summary>
        /// <returns>The path to this image resource as a string</returns>
        private string FindCardImageSource()
        {
            return "Resources/CardBack.png";
        }
    }
}
