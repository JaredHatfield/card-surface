// <copyright file="Blackjack.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The game of Blackjack.</summary>
namespace GameBlackjack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// The game of Blackjack.
    /// </summary>
    public class Blackjack : Game
    {
        /// <summary>
        /// An array that indicates for each player if they are in the game and if they finished their hand.
        /// </summary>
        private int[] handFinished;

        /// <summary>
        /// The minimum amount of money required to join the game.
        /// </summary>
        private int minimumStake;

        /// <summary>
        /// The minimum amount that must be bet each round.
        /// </summary>
        private int minimumBet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Blackjack"/> class.
        /// </summary>
        public Blackjack() : base()
        {
            this.minimumStake = 10;
            this.minimumBet = 1;
            this.handFinished = new int[this.Seats.Count];
            for (int i = 0; i < this.Seats.Count; i++)
            {
                this.handFinished[i] = -1;
            }

            // Subscribe all of the possible game actions
            this.SubscribeAction(new GameActionHit());
            this.SubscribeAction(new GameActionStand());
            this.SubscribeAction(new GameActionSplit());
            this.SubscribeAction(new GameActionDeal());
            this.SubscribeAction(new GameActionDouble());

            // Add a deck of cards to the game
            CardPile destinationDeck = (CardPile)this.GetPile(this.DeckPile);
            destinationDeck.Open = true;
            CardPile sourceDeck = Deck.StandardDeck();
            this.EmptySpecifiedCardPileTo(sourceDeck, destinationDeck);
        }

        /// <summary>
        /// Gets the name of the game.
        /// </summary>
        /// <value>The game's name.</value>
        public override string Name
        {
            get { return "Blackjack"; }
        }

        /// <summary>
        /// Gets a value indicating whether betting is enabled.
        /// </summary>
        /// <value><c>true</c> if betting is enabled; otherwise, <c>false</c>.</value>
        public override int MinimumStake
        {
            get { return this.minimumStake; }
        }

        /// <summary>
        /// Gets the minimum bet.
        /// </summary>
        /// <value>The minimum bet.</value>
        public int MinimumBet
        {
            get { return this.minimumBet; }
        }

        /// <summary>
        /// Gets a value indicating whether the game is in a hand.
        /// </summary>
        /// <value><c>true</c> if the game is in a hand; otherwise, <c>false</c>.</value>
        internal bool InHand
        {
            get
            {
                for (int i = 0; i < this.handFinished.Length; i++)
                {
                    if (this.handFinished[i] == 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets or sets the hand finished.
        /// </summary>
        /// <value>The hand finished.</value>
        internal int[] HandFinished
        {
            get { return this.handFinished; }
            set { this.handFinished = value; }
        }

        /// <summary>
        /// Gets or sets the deck pile.
        /// </summary>
        /// <value>The deck pile.</value>
        internal new Guid DeckPile
        {
            get { return base.DeckPile; }
            set { base.DeckPile = value; }
        }

        /// <summary>
        /// Attempt to have a user sit down at a table.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="amount">The amount of money the user will place on the table.</param>
        /// <returns>
        /// True if the user was able to sit down; otherwise false.
        /// </returns>
        public override bool SitDown(string username, string password, int amount)
        {
            if (base.SitDown(username, password, amount))
            {
                this.InitializePlayer(username);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Have a user sit down in one of the seats
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// True if the user was able to sit down; otherwise false.
        /// </returns>
        public override bool SitDown(string username, string password)
        {
            if (base.SitDown(username, password))
            {
                this.InitializePlayer(username);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Move the specified IPhysicalObject into the specified Pile.
        /// </summary>
        /// <param name="physicalObject">The physical object.</param>
        /// <param name="destinationPile">The destination pile.</param>
        /// <returns>True if the move was successful; otherwise false.</returns>
        public new bool MoveAction(Guid physicalObject, Guid destinationPile)
        {
            return base.MoveAction(physicalObject, destinationPile);
        }

        /// <summary>
        /// Gets the pile containing a physical object with the specified id.
        /// </summary>
        /// <param name="physicalObjectId">The physical object id.</param>
        /// <returns>
        /// The instance of the pile containing the specified physical object; otherwise null.
        /// </returns>
        internal new Pile GetPileContaining(Guid physicalObjectId)
        {
            return base.GetPileContaining(physicalObjectId);
        }

        /// <summary>
        /// Gets the pile with the specified id.
        /// </summary>
        /// <param name="pileId">The pile id.</param>
        /// <returns>
        /// The instance of the specified pile if it exists; otherwise null.
        /// </returns>
        internal new Pile GetPile(Guid pileId)
        {
            return base.GetPile(pileId);
        }

        /// <summary>
        /// Clears the game board.
        /// </summary>
        internal new void ClearGameBoard()
        {
            base.ClearGameBoard();
        }

        /// <summary>
        /// Gets the index of the player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The index of the player</returns>
        internal int GetPlayerIndex(string player)
        {
            for (int i = 0; i < this.Seats.Count; i++)
            {
                if (!this.Seats[i].IsEmpty && this.Seats[i].Username.Equals(player))
                {
                    return i;
                }
            }

            throw new Exception("Player index could not be found.");
        }

        /// <summary>
        /// Gets the index of the player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The index of the player</returns>
        internal int GetPlayerIndex(Player player)
        {
            for (int i = 0; i < this.Seats.Count; i++)
            {
                if (!this.Seats[i].IsEmpty && this.Seats[i].Player == player)
                {
                    return i;
                }
            }

            throw new Exception("Player index could not be found.");
        }

        /// <summary>
        /// Resets the player turn so that it is currently the first playesr turn.
        /// </summary>
        protected internal new void ResetPlayerTurn()
        {
            base.ResetPlayerTurn();
        }

        /// <summary>
        /// Moves the pointer that determines whoes turn it is to next player.
        /// </summary>
        protected internal new void MoveToNextPlayersTurn()
        {
            base.MoveToNextPlayersTurn();
        }

        /// <summary>
        /// Tests if a move of a PhysicalObject to a specified Pile is valid for the specific game.
        /// </summary>
        /// <param name="physicalObject">The physical object.</param>
        /// <param name="destinationPile">The destination pile.</param>
        /// <returns>
        /// True if the move if valid; otherwise false.
        /// </returns>
        protected override bool MoveTest(Guid physicalObject, Guid destinationPile)
        {
            return base.MoveTest(physicalObject, destinationPile);
        }

        /// <summary>
        /// Initializes the specified player.
        /// </summary>
        /// <param name="username">The username to initialize.</param>
        private void InitializePlayer(string username)
        {
            // Initialize the player's piles for Blackjack
            Player player = this.GetPlayer(username);

            // Initialize the player's card piles
            this.AddCardPileToUser(username);
            this.AddCardPileToUser(username);
            player.PlayerArea.Cards[0].Open = true;
            player.PlayerArea.Cards[1].Open = true;

            // Initialize the player's chip piles
            this.AddChipPileToUser(username);
            this.AddChipPileToUser(username);
            player.PlayerArea.Chips[0].Open = true;
            player.PlayerArea.Chips[1].Open = true;
        }
    }
}
