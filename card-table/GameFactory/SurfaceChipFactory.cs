// <copyright file="SurfaceChipFactory.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Implements the SurfaceChipFactory.</summary>
namespace CardTable.GameFactory
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using CardGame;
    using CardGame.GameException;
    using CardGame.GameFactory;
    using CardTable.GameObjects;

    /// <summary>
    /// The factory for creating surface chips.
    /// </summary>
    internal class SurfaceChipFactory : ChipFactory
    {
        /// <summary>
        /// The singleton instance of the ChipFactory.
        /// </summary>
        private static SurfaceChipFactory instance;

        /// <summary>
        /// Returns an instance of the SurfaceChipFactory
        /// </summary>
        /// <returns>The singleton instance of SurfaceChipFactory.</returns>
        public static new SurfaceChipFactory Instance()
        {
            if (SurfaceChipFactory.instance == null)
            {
                SurfaceChipFactory.instance = new SurfaceChipFactory();
            }

            return SurfaceChipFactory.instance;
        }

        /// <summary>
        /// Creates a new IChip.
        /// </summary>
        /// <param name="id">The chip's id.</param>
        /// <param name="amount">The chip's amount.</param>
        /// <returns>An IChip with a specified Guid.</returns>
        protected override IChip MakeChip(Guid id, int amount)
        {
            switch (amount)
            {
                case 1:
                    return new SurfaceChip(id, 1, Color.White);
                case 5:
                    return new SurfaceChip(id, 5, Color.Red);
                case 10:
                    return new SurfaceChip(id, 10, Color.Blue);
                case 25:
                    return new SurfaceChip(id, 25, Color.Green);
                case 100:
                    return new SurfaceChip(id, 100, Color.Black);
                default:
                    throw new CardGameException("Invalid chip value requested");
            }
        }

        /// <summary>
        /// Creates a new IChip.
        /// </summary>
        /// <param name="amount">The chip's amount.</param>
        /// <returns>An IChip with a new Guid.</returns>
        protected override IChip MakeChip(int amount)
        {
            switch (amount)
            {
                case 1:
                    return new SurfaceChip(1, Color.White);
                case 5:
                    return new SurfaceChip(5, Color.Red);
                case 10:
                    return new SurfaceChip(10, Color.Blue);
                case 25:
                    return new SurfaceChip(25, Color.Green);
                case 100:
                    return new SurfaceChip(100, Color.Black);
                default:
                    throw new CardGameException("Invalid chip value requested");
            }
        }
    }
}
