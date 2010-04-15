// <copyright file="ChipFactory.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Class that is used to create new Chips.</summary>
namespace CardGame.GameFactory
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using CardGame.GameException;

    /// <summary>
    /// Class that is used to create new Chips.
    /// </summary>
    public class ChipFactory
    {
        /// <summary>
        /// The singleton instance of the ChipFactory.
        /// </summary>
        private static ChipFactory instance;

        /// <summary>
        /// Prevents a default instance of the ChipFactory class from being created.
        /// </summary>
        private ChipFactory()
        {
            // TODO: Implement Me!
        }

        /// <summary>
        /// Returns an instance of the ChipFactory
        /// </summary>
        /// <returns>The singleton instance of ChipFactory.</returns>
        public static ChipFactory Instance()
        {
            if (ChipFactory.instance == null)
            {
                ChipFactory.instance = new ChipFactory();
            }

            return ChipFactory.instance;
        }

        /// <summary>
        /// Creates a new IChip.
        /// </summary>
        /// <param name="id">The chip's id.</param>
        /// <param name="amount">The chip's amount.</param>
        /// <returns>An IChip with a specified Guid.</returns>
        protected internal virtual IChip MakeChip(Guid id, int amount)
        {
            switch (amount)
            {
                case 1:
                    return new Chip(id, 1, Color.White);
                case 5:
                    return new Chip(id, 5, Color.Red);
                case 10:
                    return new Chip(id, 10, Color.Blue);
                case 25:
                    return new Chip(id, 25, Color.Green);
                case 100:
                    return new Chip(id, 100, Color.Black);
                default:
                    throw new CardGameException("Invalid chip value requested");
            }
        }

        /// <summary>
        /// Creates a new IChip.
        /// </summary>
        /// <param name="amount">The chip's amount.</param>
        /// <returns>An IChip with a new Guid.</returns>
        protected internal virtual IChip MakeChip(int amount)
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
                    throw new CardGameException("Invalid chip value requested");
            }
        }
    }
}
