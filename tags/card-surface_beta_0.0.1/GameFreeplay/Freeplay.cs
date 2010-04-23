// <copyright file="Freeplay.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A freeplay mode that does not restrict game movements and provides no actions.</summary>
namespace GameFreeplay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// A freeplay mode that does not restrict game movements and provides no actions.
    /// </summary>
    [Serializable] public class Freeplay : Game
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Freeplay"/> class.
        /// </summary>
        public Freeplay() : base()
        {
            // Add a deck of cards to the game
            CardPile destinationDeck = (CardPile)this.GetPile(this.DeckPile);
            destinationDeck.Open = true;
            CardPile sourceDeck = Deck.StandardDeck();
            this.EmptySpecifiedCardPileTo(sourceDeck, destinationDeck);
        }

        /// <summary>
        /// Gets a value indicating the minimum amount of money required to join a game.
        /// </summary>
        /// <value>The minimum stake for the game.</value>
        public override int MinimumStake
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the name of the game.
        /// </summary>
        /// <value>The game's name.</value>
        public override string Name
        {
            get { return "Freeplay"; }
        }
    }
}
