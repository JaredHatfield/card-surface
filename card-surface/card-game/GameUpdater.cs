// <copyright file="GameUpdater.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Updates an instance of a game based on a GameMessage.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Updates an instance of a game based on a GameMessage.
    /// </summary>
    public class GameUpdater
    {
        /// <summary>
        /// The game that will be updated.
        /// </summary>
        private Game game;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameUpdater"/> class.
        /// </summary>
        /// <param name="game">The game that will be updated.</param>
        public GameUpdater(Game game)
        {
            this.game = game;
        }

        /// <summary>
        /// Updates the specified game message.
        /// </summary>
        /// <param name="gameMessage">The game message.</param>
        public void Update(Game gameMessage)
        {
            // 1) Make a constructive pass on all of the seats making sure all of the objects match.
            for (int i = 0; i < this.game.Seats.Count; i++)
            {
                Seat.SeatLocation loc = this.game.Seats[i].Location;
                this.game.Seats[i].Update(gameMessage.GetSeat(loc));
            }

            // TODO: 2) Add any missing piles

            // TODO: 3) Add any misssing physical objects to the appropriate pile

            // TODO: 4) For each physical object, find the destination pile
            //    If the object is in the same pile, done
            //    If the object is in a different pile, move it
            //    If the object is in no pile, delete it
            //    Special: Make sure all cards match their state

            // TODO: 5) Remove players that are no longer playing

            // TODO: 6) Make sure the piles are in the same order

            // TODO: 7) Make sure all of the physical objects are in the same order
        }
    }
}
