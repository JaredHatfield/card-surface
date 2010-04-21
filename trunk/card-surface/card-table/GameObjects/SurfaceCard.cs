// <copyright file="SurfaceCard.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Implements the SurfaceCard.</summary>
namespace CardTable.GameObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Text;
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
        /// The object image.
        /// </summary>
        private Image objectImage;

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
            // TODO: This should load the image
            // this.objectImage = Image.FromFile("pack://application:,,,/Resources/CardBack.png");
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
            // TODO: This should load the image
            // this.objectImage = Image.FromFile("pack://application:,,,/Resources/CardBack.png");
        }

        /// <summary>
        /// Gets the object image.
        /// </summary>
        /// <value>The object image.</value>
        internal Image ObjectImage
        {
            get
            {
                return this.objectImage;
            }
        }
    }
}
