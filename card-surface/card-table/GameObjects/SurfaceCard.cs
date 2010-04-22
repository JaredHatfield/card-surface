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
                if (this.Status.Equals(Card.CardStatus.FaceUp))
                {
                    return this.objectImageSource;
                }
                else
                {
                    return "Resources/CardBack.png";
                }
            }
        }

        /// <summary>
        /// Finds the card image source.
        /// </summary>
        /// <returns>The path to this image resource as a string</returns>
        private string FindCardImageSource()
        {
            switch (this.Suit)
            {
                case Card.CardSuit.Clubs:
                    switch (this.Face)
                    {
                        case Card.CardFace.Ace:
                            return "Resources/Cards/AceClubs.bmp";
                        case Card.CardFace.King:
                            return "Resources/Cards/KingClubs.bmp";
                        case Card.CardFace.Queen:
                            return "Resources/Cards/QueenClubs.bmp";
                        case Card.CardFace.Jack:
                            return "Resources/Cards/JackClubs.bmp";
                        case Card.CardFace.Ten:
                            return "Resources/Cards/TenClubs.bmp";
                        case Card.CardFace.Nine:
                            return "Resources/Cards/NineClubs.bmp";
                        case Card.CardFace.Eight:
                            return "Resources/Cards/EightClubs.bmp";
                        case Card.CardFace.Seven:
                            return "Resources/Cards/SevenClubs.bmp";
                        case Card.CardFace.Six:
                            return "Resources/Cards/SixClubs.bmp";
                        case Card.CardFace.Five:
                            return "Resources/Cards/FiveClubs.bmp";
                        case Card.CardFace.Four:
                            return "Resources/Cards/FourClubs.bmp";
                        case Card.CardFace.Three:
                            return "Resources/Cards/ThreeClubs.bmp";
                        case Card.CardFace.Two:
                            return "Resources/Cards/TwoClubs.bmp";
                    }

                    break;
                case Card.CardSuit.Diamonds:
                    switch (this.Face)
                    {
                        case Card.CardFace.Ace:
                            return "Resources/Cards/AceDiamonds.bmp";
                        case Card.CardFace.King:
                            return "Resources/Cards/KingDiamonds.bmp";
                        case Card.CardFace.Queen:
                            return "Resources/Cards/QueenDiamonds.bmp";
                        case Card.CardFace.Jack:
                            return "Resources/Cards/JackDiamonds.bmp";
                        case Card.CardFace.Ten:
                            return "Resources/Cards/TenDiamonds.bmp";
                        case Card.CardFace.Nine:
                            return "Resources/Cards/NineDiamonds.bmp";
                        case Card.CardFace.Eight:
                            return "Resources/Cards/EightDiamonds.bmp";
                        case Card.CardFace.Seven:
                            return "Resources/Cards/SevenDiamonds.bmp";
                        case Card.CardFace.Six:
                            return "Resources/Cards/SixDiamonds.bmp";
                        case Card.CardFace.Five:
                            return "Resources/Cards/FiveDiamonds.bmp";
                        case Card.CardFace.Four:
                            return "Resources/Cards/FourDiamonds.bmp";
                        case Card.CardFace.Three:
                            return "Resources/Cards/ThreeDiamonds.bmp";
                        case Card.CardFace.Two:
                            return "Resources/Cards/TwoDiamonds.bmp";
                    }

                    break;
                case Card.CardSuit.Hearts:
                    switch (this.Face)
                    {
                        case Card.CardFace.Ace:
                            return "Resources/Cards/AceHearts.bmp";
                        case Card.CardFace.King:
                            return "Resources/Cards/KingHearts.bmp";
                        case Card.CardFace.Queen:
                            return "Resources/Cards/QueenHearts.bmp";
                        case Card.CardFace.Jack:
                            return "Resources/Cards/JackHearts.bmp";
                        case Card.CardFace.Ten:
                            return "Resources/Cards/TenHearts.bmp";
                        case Card.CardFace.Nine:
                            return "Resources/Cards/NineHearts.bmp";
                        case Card.CardFace.Eight:
                            return "Resources/Cards/EightHearts.bmp";
                        case Card.CardFace.Seven:
                            return "Resources/Cards/SevenHearts.bmp";
                        case Card.CardFace.Six:
                            return "Resources/Cards/SixHearts.bmp";
                        case Card.CardFace.Five:
                            return "Resources/Cards/FiveHearts.bmp";
                        case Card.CardFace.Four:
                            return "Resources/Cards/FourHearts.bmp";
                        case Card.CardFace.Three:
                            return "Resources/Cards/ThreeHearts.bmp";
                        case Card.CardFace.Two:
                            return "Resources/Cards/TwoHearts.bmp";
                    }

                    break;
                case Card.CardSuit.Spades:
                    switch (this.Face)
                    {
                        case Card.CardFace.Ace:
                            return "Resources/Cards/AceSpades.bmp";
                        case Card.CardFace.King:
                            return "Resources/Cards/KingSpades.bmp";
                        case Card.CardFace.Queen:
                            return "Resources/Cards/QueenSpades.bmp";
                        case Card.CardFace.Jack:
                            return "Resources/Cards/JackSpades.bmp";
                        case Card.CardFace.Ten:
                            return "Resources/Cards/TenSpades.bmp";
                        case Card.CardFace.Nine:
                            return "Resources/Cards/NineSpades.bmp";
                        case Card.CardFace.Eight:
                            return "Resources/Cards/EightSpades.bmp";
                        case Card.CardFace.Seven:
                            return "Resources/Cards/SevenSpades.bmp";
                        case Card.CardFace.Six:
                            return "Resources/Cards/SixSpades.bmp";
                        case Card.CardFace.Five:
                            return "Resources/Cards/FiveSpades.bmp";
                        case Card.CardFace.Four:
                            return "Resources/Cards/FourSpades.bmp";
                        case Card.CardFace.Three:
                            return "Resources/Cards/ThreeSpades.bmp";
                        case Card.CardFace.Two:
                            return "Resources/Cards/TwoSpades.bmp";
                    }

                    break;
            }

            return "Resources/CardBack.png";
        }
    }
}
