// <copyright file="BankPile.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A pile of cards.</summary>
namespace CardGame
{
    using System;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using CardGame.GameException;

    /// <summary>
    /// A pile of chips that the user can access money they have placed on the table.
    /// </summary>
    public class BankPile : ChipPile
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
            this.RefreshChipPile();
            this.Open = true;
        }

        /// <summary>
        /// Prevents a default instance of the BankPile class from being created.
        /// </summary>
        private BankPile()
            : base()
        {
            throw new CardGameException("BankPile should not be initialized without referencing a player.");
        }

        /// <summary>
        /// Gets the chips.
        /// </summary>
        /// <value>The chips.</value>
        public ReadOnlyObservableCollection<IPhysicalObject> Chips
        {
            get { return new ReadOnlyObservableCollection<IPhysicalObject>(this.Items); }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Refreshes the chip pile so that the pile only contains valid chips that can be removed by the player.
        /// </summary>
        internal void RefreshChipPile()
        {
            int[] amounts = { 1, 5, 10, 25, 100 };

            // 1) Remove any duplicate value chips and add the money to the users account
            for (int i = 0; i < amounts.Length; i++)
            {
                if (this.ChipAmountCount(amounts[i]) > 1)
                {
                    // We need to get all of the IDs of these chips
                    Collection<Guid> chips = new Collection<Guid>();
                    for (int j = 0; j < this.Items.Count; j++)
                    {
                        if ((this.Items[j] as IChip).Amount.Equals(amounts[i]))
                        {
                            chips.Add(this.Items[j].Id);
                        }
                    }

                    // Now we remove all except the first item from the list and credit the players account
                    for (int j = 1; j < chips.Count; j++)
                    {
                        if (this.RemoveItem(chips[j]))
                        {
                            this.player.Balance += amounts[i];
                        }
                    }
                }
            }

            // 2) Add all of the chips of each value to the pile if they are missing
            for (int i = 0; i < amounts.Length; i++)
            {
                this.AddChipToPile(amounts[i]);
            }
        }

        /// <summary>
        /// Returns the number of chips in the pile with the specified value.
        /// </summary>
        /// <param name="value">The value to look for.</param>
        /// <returns>The number of chips.</returns>
        private int ChipAmountCount(int value)
        {
            int count = 0;
            for (int i = 0; i < this.Items.Count; i++)
            {
                if ((this.Items[i] as IChip).Amount.Equals(value))
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Adds the chip with the specified value to the pile if necessary.
        /// The amount will be deducted from the players balance.
        /// </summary>
        /// <param name="value">The value of the chip.</param>
        private void AddChipToPile(int value)
        {
            if (this.player.Balance > value && !this.PileContainsChip(value))
            {
                this.Items.Add(this.GetNewChip(value));
                this.player.Balance -= value;
            }
        }

        /// <summary>
        /// Test if the bank already includes a chip with the specified value.
        /// </summary>
        /// <param name="value">The value to look for.</param>
        /// <returns>True if a chip with that value is contained in the pile; otherwise false.</returns>
        private bool PileContainsChip(int value)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if ((this.Items[i] as IChip).Amount.Equals(value))
                {
                    return true;
                }
            }

            return false;
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
                    throw new CardGameException("Invalid chip value requested");
            }
        }
    }
}
