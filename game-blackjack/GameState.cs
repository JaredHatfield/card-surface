// <copyright file="GameState.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The current state of the game of Blackjack.</summary>
namespace GameBlackjack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The current state of the game of Blackjack.
    /// </summary>
    internal class GameState
    {
        /// <summary>
        /// The current state of the game that can be advanced.
        /// </summary>
        private State current;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameState"/> class.
        /// </summary>
        internal GameState()
        {
            this.current = State.NotInGame;
        }

        /// <summary>
        /// The possible states for the game.
        /// </summary>
        internal enum State
        {
            /// <summary>
            /// A game is not in progress.
            /// </summary>
            NotInGame,

            /// <summary>
            /// The cards have been dealt.
            /// </summary>
            Playing,

            /// <summary>
            /// The players have finished, now the dealer takes his turn.
            /// </summary>
            Dealer,

            /// <summary>
            /// We now award those who have one their chips.
            /// </summary>
            Award,

            /// <summary>
            /// We deposit the winnings into the players bank.
            /// </summary>
            Clear
        }

        /// <summary>
        /// Gets the current state.
        /// </summary>
        /// <value>The current state.</value>
        internal State Current
        {
            get { return this.current; }
        }

        /// <summary>
        /// Advances the game state.
        /// </summary>
        internal void Advance()
        {
            switch (this.current)
            {
                case State.NotInGame:
                    this.current = State.Playing;
                    break;
                case State.Playing:
                    this.current = State.Dealer;
                    break;
                case State.Dealer:
                    this.current = State.Award;
                    break;
                case State.Award:
                    this.current = State.Clear;
                    break;
                case State.Clear:
                    this.current = State.NotInGame;
                    break;
            }
        }
    }
}
