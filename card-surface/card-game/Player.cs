// <copyright file="Player.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A player in the game.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A player in the game.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The players balance on the table.
        /// </summary>
        private int balance;

        /// <summary>
        /// The players hand.
        /// </summary>
        private CardPile hand;

        /// <summary>
        /// The players area.
        /// </summary>
        private PlayingArea playerArea;

        /// <summary>
        /// A flag to indicate if it is this players turn.
        /// </summary>
        private bool turn;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        internal Player()
        {
            this.balance = 0;
            this.hand = new CardPile();
            this.playerArea = new PlayingArea();
        }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        /// <value>The balance.</value>
        public int Balance
        {
            get { return this.balance; }
            set { this.balance = value; }
        }

        /// <summary>
        /// Gets the hand.
        /// </summary>
        /// <value>The players hand.</value>
        public CardPile Hand
        {
            get { return this.hand; }
        }

        /// <summary>
        /// Gets the player area.
        /// </summary>
        /// <value>The player area.</value>
        public PlayingArea PlayerArea
        {
            get { return this.playerArea; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether it is this players turn.
        /// </summary>
        /// <value><c>true</c> if it is this players turn; otherwise, <c>false</c>.</value>
        internal bool Turn
        {
            get { return this.turn; }
            set { this.turn = value; }
        }

        /// <summary>
        /// Determines whether the specified card id is held by the player.
        /// </summary>
        /// <param name="cardId">The unique card id.</param>
        /// <returns>
        ///     <c>true</c> if the specified card id is held by the player; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsCard(Guid cardId)
        {
            if (this.hand.ContainsPhysicalObject(cardId))
            {
                return true;
            }
            else if (this.playerArea.ContainsCard(cardId))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified chip id is held by the player.
        /// </summary>
        /// <param name="chipId">The unique chip id.</param>
        /// <returns>
        ///     <c>true</c> if the specified chip id is held by the player; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsChip(Guid chipId)
        {
            if (this.playerArea.ContainsChip(chipId))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified pile id held by the player.
        /// </summary>
        /// <param name="pileId">The unique pile id.</param>
        /// <returns>
        ///     <c>true</c> if the specified pile id is held by the player; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsPile(Guid pileId)
        {
            if (this.hand.Id.Equals(pileId))
            {
                return true;
            }
            else if (this.playerArea.ContainsPile(pileId))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the physical object by unique id.
        /// </summary>
        /// <param name="id">The unique id.</param>
        /// <returns>The IPhysicalObject specified or null if it is not possesed by the Player.</returns>
        internal IPhysicalObject GetPhysicalObject(Guid id)
        {
            if (this.PlayerArea.ContainsCard(id) || this.PlayerArea.ContainsChip(id))
            {
                return this.PlayerArea.GetPhysicalObject(id);
            }
            else
            {
                return this.Hand.GetPhysicalObject(id);
            }
        }

        /// <summary>
        /// Gets the pile with the specified id.
        /// </summary>
        /// <param name="pileId">The pile id.</param>
        /// <returns>The instance of the specified pile if it exists; otherwise null.</returns>
        internal Pile GetPile(Guid pileId)
        {
            if (this.hand.Id.Equals(pileId))
            {
                return this.hand;
            }
            else
            {
                return this.PlayerArea.GetPile(pileId);
            }
        }

        /// <summary>
        /// Gets the pile containing a physical object with the specified id.
        /// </summary>
        /// <param name="physicalObjectId">The physical object id.</param>
        /// <returns>The instance of the pile containing the specified physical object; otherwise null.</returns>
        internal Pile GetPileContaining(Guid physicalObjectId)
        {
            if (this.hand.ContainsPhysicalObject(physicalObjectId))
            {
                return this.hand;
            }
            else
            {
                return this.PlayerArea.GetPileContaining(physicalObjectId);
            }
        }
    }
}
