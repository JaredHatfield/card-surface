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
        public enum SeatLocation { North, East, West, South, SouthEast, NorthEast, NorthWest, SouthWest };
        private int balance;
        private SeatLocation location;
        private CardPile hand;
        private PlayingArea playerArea;


        public int Balance
        {
            get { return this.balance; }
        }

        public SeatLocation Location
        {
            get { return this.location; }
        }

        public CardPile Hand
        {
            get { return this.hand; }
        }

        public PlayingArea PlayerArea
        {
            get { return this.playerArea; }
        }
    }
}
