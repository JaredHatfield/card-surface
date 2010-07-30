// <copyright file="PlayerState.cs" company="University of Louisville Speed School of Engineering">
// GNU General internal License v3
// </copyright>
// <summary>The state of the player who is playing a game of Blackjack.</summary>
namespace GameBlackjack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GameBlackjack.BlackjackExceptions;

    /// <summary>
    /// The state of the player who is playing a game of Blackjack.
    /// </summary>
    internal class PlayerState
    {
        /// <summary>
        /// A flag to indicate this player is playing the game.
        /// </summary>
        private bool playing;

        /// <summary>
        /// A flag to indicate thist player has been delt cards.
        /// </summary>
        private bool dealt;

        /// <summary>
        /// A flag to indicate this player has split.
        /// </summary>
        private bool split;

        /// <summary>
        /// A flag to indicate this player has finished their first hand.
        /// </summary>
        private bool handOneStand;

        /// <summary>
        /// A flag to indicate this player has finished their second hand.
        /// </summary>
        private bool handTwoStand;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerState"/> class.
        /// </summary>
        internal PlayerState()
        {
            this.playing = false;
            this.dealt = false;
            this.split = false;
            this.handOneStand = false;
            this.handTwoStand = false;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is playing.
        /// </summary>
        /// <value><c>true</c> if this instance is playing; otherwise, <c>false</c>.</value>
        internal bool IsPlaying
        {
            get { return this.playing; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is finished.
        /// </summary>
        /// <value><c>true</c> if this instance is finished; otherwise, <c>false</c>.</value>
        internal bool IsFinished
        {
            get
            {
                this.TestInactive();
                if (!this.dealt)
                {
                    // We have not been dealt, so we are automatically finished
                    return true;
                }
                else if (!this.split)
                {
                    // The player has not split
                    return this.handOneStand;
                }
                else
                {
                    // The player has split, check both hands
                    return this.handOneStand && this.handTwoStand;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is dealt.
        /// </summary>
        /// <value><c>true</c> if this instance is dealt; otherwise, <c>false</c>.</value>
        internal bool IsDealt
        {
            get
            {
                this.TestInactive();
                return this.dealt;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has split.
        /// </summary>
        /// <value><c>true</c> if this instance has split; otherwise, <c>false</c>.</value>
        internal bool HasSplit
        {
            get
            {
                this.TestInactive();
                this.TestDealt();
                return this.split;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has hand one stand.
        /// </summary>
        /// <value><c>true</c> if this instance has hand one stand; otherwise, <c>false</c>.</value>
        internal bool HasHandOneStand
        {
            get
            {
                this.TestInactive();
                this.TestDealt();
                return this.handOneStand;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has hand two stand.
        /// </summary>
        /// <value><c>true</c> if this instance has hand two stand; otherwise, <c>false</c>.</value>
        internal bool HasHandTwoStand
        {
            get
            {
                this.TestInactive();
                this.TestDealt();
                this.TestNotSplit();
                return this.handTwoStand;
            }
        }

        /// <summary>
        /// Joins this instance.
        /// </summary>
        internal void Join()
        {
            if (this.playing)
            {
                throw new BlackjackInvalidPlayerStateException();
            }
            else
            {
                this.playing = true;
                this.Reset();
            }
        }

        /// <summary>
        /// Leaves this instance.
        /// </summary>
        internal void Leave()
        {
            if (this.playing)
            {
                this.playing = false;
                this.Reset();
            }
            else
            {
                throw new BlackjackInvalidPlayerStateException();
            }
        }

        /// <summary>
        /// Deals this instance.
        /// </summary>
        internal void Deal()
        {
            this.TestInactive();
            if (this.dealt)
            {
                throw new BlackjackInvalidPlayerStateException();
            }
            else
            {
                this.dealt = true;
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        internal void Reset()
        {
            this.dealt = false;
            this.split = false;
            this.handOneStand = false;
            this.handTwoStand = false;
        }

        /// <summary>
        /// Splits this instance.
        /// </summary>
        internal void Split()
        {
            this.TestInactive();
            this.TestDealt();
            if (this.split)
            {
                throw new BlackjackInvalidPlayerStateException();
            }

            this.split = true;
        }

        /// <summary>
        /// Stand hand one.
        /// </summary>
        internal void StandHandOne()
        {
            this.TestInactive();
            this.TestDealt();
            if (this.handOneStand)
            {
                throw new BlackjackInvalidPlayerStateException();
            }

            this.handOneStand = true;
        }

        /// <summary>
        /// Stand hand two.
        /// </summary>
        internal void StandHandTwo()
        {
            this.TestInactive();
            this.TestDealt();
            this.TestNotSplit();
            if (this.handTwoStand)
            {
                throw new BlackjackInvalidPlayerStateException();
            }

            this.handTwoStand = true;
        }

        /// <summary>
        /// Tests if the player is inactive and throws appropriate exception.
        /// </summary>
        private void TestInactive()
        {
            if (!this.playing)
            {
                throw new BlackjackInactivePlayerException();
            }
        }

        /// <summary>
        /// Tests if the player has not split and throws appropriate exception.
        /// </summary>
        private void TestNotSplit()
        {
            if (!this.split)
            {
                throw new BlackjackPlayerHasNotSplitException();
            }
        }

        /// <summary>
        /// Tests if the player has been dealt cards and throws and appropriate exception.
        /// </summary>
        private void TestDealt()
        {
            if (!this.dealt)
            {
                throw new BlackjackPlayerNotDealtException();
            }
        }
    }
}
