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
        /// The players location at the table.
        /// </summary>
        private SeatLocation location;

        /// <summary>
        /// The players hand.
        /// </summary>
        private CardPile hand;

        /// <summary>
        /// The players area.
        /// </summary>
        private PlayingArea playerArea;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        internal Player()
        {
            this.balance = 0;
            this.location = SeatLocation.North;
            this.hand = new CardPile();
            this.playerArea = new PlayingArea();
        }

        /// <summary>
        /// The locations at the table.
        /// </summary>
        public enum SeatLocation
        {
            /// <summary>
            /// The north position on the table.
            /// </summary>
            North,

            /// <summary>
            /// The east position on the table.
            /// </summary>
            East,

            /// <summary>
            /// The west position on the table.
            /// </summary>
            West,

            /// <summary>
            /// The south position on the table.
            /// </summary>
            South,

            /// <summary>
            /// The southeast position on the table.
            /// </summary>
            SouthEast,

            /// <summary>
            /// The northeast position on the table.
            /// </summary>
            NorthEast,

            /// <summary>
            /// The northwest position on the table.
            /// </summary>
            NorthWest,

            /// <summary>
            /// The southwest position on the table.
            /// </summary>
            SouthWest
        }

        /// <summary>
        /// Gets the balance.
        /// </summary>
        /// <value>The balance.</value>
        public int Balance
        {
            get { return this.balance; }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>The location.</value>
        public SeatLocation Location
        {
            get { return this.location; }
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
    }
}
