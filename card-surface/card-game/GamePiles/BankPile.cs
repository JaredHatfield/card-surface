// <copyright file="BankPile.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A pile of cards.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A pile of chips that the user can access money they have placed on the table.
    /// </summary>
    public class BankPile : CardPile
    {
        /// <summary>
        /// The player who the bank belongs to.
        /// </summary>
        private Player player;

        /// <summary>
        /// Initializes a new instance of the <see cref="BankPile"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        internal BankPile(Player player)
            : base()
        {
            this.player = player;
        }

        /// <summary>
        /// Prevents a default instance of the BankPile class from being created.
        /// </summary>
        private BankPile()
            : base()
        {
        }

        /// <summary>
        /// Refreshes the chip pile so that the pile only contains valid chips that can be removed by the player.
        /// </summary>
        internal void RefreshChipPile()
        {
            // TODO: Implement the RefreshChipPile function for the BankPile

            // 1) Remove any duplicate value chips and add the money to the users account
            // 2) Add a new chip with value 1 if it is not in the pile
            // 3) Add a new chip with value 5 if it is not in the pile
            // 4) Add a new chip with value 10 if it is not in the pile
            // 5) Add a new chip with value 25 if it is not in the pile
            // 6) Add a new chip with value 100 if it is not in the pile
        }

        /// <summary>
        /// Gets a new chip based on the value requested.
        /// </summary>
        /// <param name="amount">The amount of the chip.</param>
        /// <returns>A new chip of the requested value.</returns>
        private IChip GetNewChip(int amount)
        {
            switch (amount)
            {
                case 1:
                    return new Chip(1, Color.White);
                case 5:
                    return new Chip(5, Color.Red);
                case 10:
                    return new Chip(10, Color.Blue);
                case 25:
                    return new Chip(25, Color.Green);
                case 100:
                    return new Chip(100, Color.Black);
                default:
                    throw new Exception("Invalid chip value requested");
            }
        }
    }
}
