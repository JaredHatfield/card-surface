// <copyright file="Game.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A generic card game that is extended to implement a specific game.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A generic card game that is extended to implement a specific game.
    /// </summary>
    public abstract class Game
    {
        /// <summary>
        /// A unique identifier for a game.
        /// </summary>
        private Guid id;

        /// <summary>
        /// The center shared area of the game for cards and chips.
        /// </summary>
        private PlayingArea gamingArea;

        /// <summary>
        /// The collection of players that are playing in the game.
        /// </summary>
        private ReadOnlyObservableCollection<Seat> seats;

        /// <summary>
        /// The actions that can be performed in this game.
        /// </summary>
        private Collection<GameAction> actions;

        /// <summary>
        /// The pile of cards that represents the deck.
        /// </summary>
        private Guid deckPile;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        protected internal Game()
        {
            this.id = Guid.NewGuid();
            this.gamingArea = new PlayingArea();
            CardPile theDeck = new CardPile(false, true);
            this.deckPile = theDeck.Id;
            this.gamingArea.AddCardPile(theDeck); // This first pile will be the deck
            ObservableCollection<Seat> s = new ObservableCollection<Seat>();
            s.Add(new Seat(Seat.SeatLocation.East));
            s.Add(new Seat(Seat.SeatLocation.North));
            s.Add(new Seat(Seat.SeatLocation.NorthEast));
            s.Add(new Seat(Seat.SeatLocation.NorthWest));
            s.Add(new Seat(Seat.SeatLocation.South));
            s.Add(new Seat(Seat.SeatLocation.SouthEast));
            s.Add(new Seat(Seat.SeatLocation.SouthWest));
            s.Add(new Seat(Seat.SeatLocation.West));
            this.seats = new ReadOnlyObservableCollection<Seat>(s);
            this.actions = new Collection<GameAction>();
            this.UpdatePlayerActions();
        }

        /// <summary>
        /// Gets the unique id for a game.
        /// </summary>
        /// <value>The unique id.</value>
        public Guid Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets the gaming area.
        /// </summary>
        /// <value>The gaming area.</value>
        public PlayingArea GamingArea
        {
            get { return this.gamingArea; }
        }

        /// <summary>
        /// Gets the seats.
        /// </summary>
        /// <value>The seats.</value>
        public ReadOnlyObservableCollection<Seat> Seats
        {
            get { return this.seats; }
        }

        /// <summary>
        /// Gets the number of players in the game.
        /// </summary>
        /// <value>The number of players in the game.</value>
        public int NumberOfPlayers
        {
            get
            {
                int number = 0;
                for (int i = 0; i < this.seats.Count; i++)
                {
                    if (!this.seats[i].IsEmpty)
                    {
                        number++;
                    }
                }

                return number;
            }
        }

        /// <summary>
        /// Gets or sets the deck pile.
        /// </summary>
        /// <value>The deck pile.</value>
        protected Guid DeckPile
        {
            get { return this.deckPile; }
            set { this.deckPile = value; }
        }

        /// <summary>
        /// Gets the player at a specific seat.
        /// </summary>
        /// <param name="location">The location of the player.</param>
        /// <returns>The player at that location.</returns>
        public Player GetPlayer(Seat.SeatLocation location)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (this.seats[i].Location == location)
                {
                    return this.seats[i].Player;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a player based off of a username
        /// </summary>
        /// <param name="username">The username to look up.</param>
        /// <returns>The instance of the player.</returns>
        public Player GetPlayer(string username)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (!this.seats[i].IsEmpty && this.seats[i].Username.Equals(username))
                {
                    return this.seats[i].Player;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a seat based off of a Guid.
        /// </summary>
        /// <param name="seatId">The seat id.</param>
        /// <returns>The instance of the seat.</returns>
        public Seat GetSeat(Guid seatId)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (!this.seats[i].IsEmpty && this.seats[i].Id.Equals(seatId))
                {
                    return this.seats[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified username is playing this game.
        /// </summary>
        /// <param name="username">The username to look up.</param>
        /// <returns>
        ///     <c>true</c> if the specified username is playing this game; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUserPlaying(string username)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (!this.seats[i].IsEmpty && this.seats[i].Username.Equals(username))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the seat at the specified location is empty.
        /// </summary>
        /// <param name="location">The location to check.</param>
        /// <returns>
        ///     <c>true</c> if the seat at the specified location is empty; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSeatEmpty(Seat.SeatLocation location)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (this.seats[i].Location == location)
                {
                    return this.seats[i].IsEmpty;
                }
            }

            return true;
        }

        /// <summary>
        /// Attempt to have a user sit down at a table.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="amount">The amount of money the user will place on the table.</param>
        /// <returns>True if the user was able to sit down; otherwise false.</returns>
        public virtual bool SitDown(string username, string password, int amount)
        {
            if (this.SitDown(username, password))
            {
                Player player = this.GetPlayer(username);
                player.Balance += amount;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Attempt to have a user sit down at a table.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>True if the user was able to sit down; otherwise false.</returns>
        public virtual bool SitDown(string username, string password)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (this.seats[i].PasswordPeek(password))
                {
                    bool result = this.seats[i].SitDown(username, password);
                    if (result)
                    {
                        this.UpdatePlayerActions();
                    }

                    return result;
                }
            }

            return false;
        }

        /// <summary>
        /// Test to see if the privided password is valid for an open seat in this game.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>True if the password is valid; otherwsie false.</returns>
        public bool PasswordPeek(string password)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (this.seats[i].PasswordPeek(password))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Move the specified IPhysicalObject into the specified Pile.
        /// </summary>
        /// <param name="physicalObject">The physical object.</param>
        /// <param name="destinationPile">The destination pile.</param>
        /// <returns>True if the move was successful; otherwise false.</returns>
        public bool MoveAction(Guid physicalObject, Guid destinationPile)
        {
            if (!this.MoveCompatible(physicalObject, destinationPile))
            {
                // We can not move this object to this pile because of compatibility
                return false;
            }
            else if (!this.MoveTest(physicalObject, destinationPile))
            {
                // Not a valid game move
                return false;
            }
            else
            {
                // Retreive the physical object's pile
                Pile locatedSourcePile = this.GetPileContaining(physicalObject);
                if (locatedSourcePile == null)
                {
                    return false;
                }

                // Retreive the physical object
                IPhysicalObject locatedPhysicalObject = locatedSourcePile.GetPhysicalObject(physicalObject);
                if (locatedPhysicalObject == null)
                {
                    return false;
                }

                // Retreive the pile
                Pile locatedDestinationPile = this.GetPile(destinationPile);
                if (locatedDestinationPile == null || !locatedDestinationPile.Open)
                {
                    return false;
                }
                
                // Removed the object from the source pile
                locatedSourcePile.RemoveItem(locatedPhysicalObject);

                // Add the physical object to the destination pile
                locatedDestinationPile.AddItem(locatedPhysicalObject);

                this.UpdatePlayerActions();
                return true;
            }
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="name">The name of the action.</param>
        /// <param name="player">The player.</param>
        /// <returns>True if the action was successful; otherwise false.</returns>
        public bool ExecuteAction(string name, string player)
        {
            for (int i = 0; i < this.actions.Count; i++)
            {
                if (this.actions[i].Name.Equals(name))
                {
                    bool result = this.actions[i].Action(this, player);
                    if (result)
                    {
                        this.UpdatePlayerActions();
                    }

                    return result;
                }
            }

            return false;
        }

        /// <summary>
        /// Resets the player turn so that it is currently the first playesr turn.
        /// </summary>
        protected internal void ResetPlayerTurn()
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (!this.seats[i].IsEmpty)
                {
                    this.seats[i].Player.Turn = false;
                }
            }

            for (int i = 0; i < this.seats.Count; i++)
            {
                if (!this.seats[i].IsEmpty)
                {
                    this.seats[i].Player.Turn = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Moves the pointer that determines whoes turn it is to next player.
        /// </summary>
        protected internal void MoveToNextPlayersTurn()
        {
            int active = -1;
            int playerCount = 0;

            // Determine whoes turn it currently is
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (!this.seats[i].IsEmpty)
                {
                    playerCount++;
                    if (this.seats[i].Player.Turn && active == -1)
                    {
                        active = i;
                    }
                }
            }

            // Special case where we have only one player
            if (playerCount == 1)
            {
                return;
            }

            // A quick check to make sure it is actually someones turn
            if (active >= 0)
            {
                // First check the bottom half of the player list
                for (int i = active + 1; i < this.seats.Count; i++)
                {
                    if (!this.seats[i].IsEmpty)
                    {
                        this.seats[active].Player.Turn = false;
                        this.seats[i].Player.Turn = true;
                        return;
                    }
                }

                // Next check the top half of the player list
                for (int i = 0; i < active; i++)
                {
                    if (!this.seats[i].IsEmpty)
                    {
                        this.seats[active].Player.Turn = false;
                        this.seats[i].Player.Turn = true;
                        return;
                    }
                }

                throw new Exception("Player turn could not be rotated to the next player.");
            }
        }

        /// <summary>
        /// Gets the active Player whose turn it currently is.
        /// </summary>
        /// <returns>The active Player</returns>
        protected internal Player GetActivePlayer()
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (!this.seats[i].IsEmpty)
                {
                    if (this.seats[i].Player.Turn)
                    {
                        return this.seats[i].Player;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the specified physical object.
        /// </summary>
        /// <param name="id">The unique id.</param>
        /// <returns>The instance of the IPhysicalObject if it exists; otherwise null.</returns>
        protected internal IPhysicalObject GetPhysicalObject(Guid id)
        {
            if (this.GamingArea.ContainsCard(id) || this.GamingArea.ContainsChip(id))
            {
                return this.GamingArea.GetPhysicalObject(id);
            }
            else
            {
                for (int i = 0; i < this.Seats.Count; i++)
                {
                    Seat s = this.Seats[i];
                    if (!s.IsEmpty && (s.Player.ContainsCard(id) || s.Player.ContainsChip(id)))
                    {
                        return this.Seats[i].Player.GetPhysicalObject(id);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the pile with the specified id.
        /// </summary>
        /// <param name="pileId">The pile id.</param>
        /// <returns>The instance of the specified pile if it exists; otherwise null.</returns>
        protected internal Pile GetPile(Guid pileId)
        {
            if (this.GamingArea.ContainsPile(pileId))
            {
                return this.GamingArea.GetPile(pileId);
            }
            else
            {
                for (int i = 0; i < this.Seats.Count; i++)
                {
                    Seat s = this.Seats[i];
                    if (!s.IsEmpty && s.Player.ContainsPile(pileId))
                    {
                        return this.Seats[i].Player.GetPile(pileId);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the pile containing a physical object with the specified id.
        /// </summary>
        /// <param name="physicalObjectId">The physical object id.</param>
        /// <returns>The instance of the pile containing the specified physical object; otherwise null.</returns>
        protected internal Pile GetPileContaining(Guid physicalObjectId)
        {
            if (this.GamingArea.GetPhysicalObject(physicalObjectId) != null)
            {
                return this.GamingArea.GetPileContaining(physicalObjectId);
            }
            else
            {
                for (int i = 0; i < this.Seats.Count; i++)
                {
                    Seat s = this.Seats[i];
                    if (!s.IsEmpty && s.Player.GetPhysicalObject(physicalObjectId) != null)
                    {
                        return this.Seats[i].Player.GetPileContaining(physicalObjectId);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Adds a ChipPile to a user.
        /// </summary>
        /// <param name="username">The username.</param>
        protected internal void AddChipPileToUser(string username)
        {
            Player p = this.GetPlayer(username);
            p.PlayerArea.AddChipPile(new ChipPile());
        }

        /// <summary>
        /// Adds a CardPile to a user.
        /// </summary>
        /// <param name="username">The username.</param>
        protected internal void AddCardPileToUser(string username)
        {
            Player p = this.GetPlayer(username);
            p.PlayerArea.AddCardPile(new CardPile());
        }

        /// <summary>
        /// Clears the game board.
        /// </summary>
        protected internal void ClearGameBoard()
        {
            CardPile deck = this.GetPile(this.deckPile) as CardPile;
            this.gamingArea.EmptyCardPileTo(deck);
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (!this.seats[i].IsEmpty)
                {
                    this.seats[i].Player.EmptyCardPileTo(deck);
                }
            }

            // Change all of the cards so they are face down
            for (int i = 0; i < deck.Cards.Count; i++)
            {
                ICard card = deck.Cards[i] as ICard;
                card.Status = Card.CardStatus.FaceDown;
            }
        }

        /// <summary>
        /// Updates the all of the players with valid actions.
        /// </summary>
        protected internal void UpdatePlayerActions()
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (!this.seats[i].IsEmpty)
                {
                    Player p = this.seats[i].Player;
                    for (int j = 0; j < this.actions.Count; j++)
                    {
                        if (this.actions[j].IsExecutableByPlayer(this, p))
                        {
                            p.AddAction(this.actions[j].Name);
                        }
                        else
                        {
                            p.RemoveAction(this.actions[j].Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Empties the specified card pile to the destination pile.
        /// </summary>
        /// <param name="source">The source pile.</param>
        /// <param name="destination">The destination pile.</param>
        protected void EmptySpecifiedCardPileTo(CardPile source, CardPile destination)
        {
            source.EmptyCardPileTo(destination);
        }

        /// <summary>
        /// Tests if a move of a IPhysicalObject to a specified Pile is valid for the specific game.
        /// </summary>
        /// <param name="physicalObject">The physical object.</param>
        /// <param name="destinationPile">The destination pile.</param>
        /// <returns>True if the move if valid; otherwise false.</returns>
        protected virtual bool MoveTest(Guid physicalObject, Guid destinationPile)
        {
            // This method should be overridden by a specific game to validate this specific move.
            return true;
        }

        /// <summary>
        /// Subscribes an action to the game.
        /// </summary>
        /// <param name="gameAction">The game action.</param>
        protected void SubscribeAction(GameAction gameAction)
        {
            this.actions.Add(gameAction);
        }

        /// <summary>
        /// Tests if a move of a IPhysicalObject to a specified Pile is valid.
        /// </summary>
        /// <param name="physicalObject">The physical object.</param>
        /// <param name="destinationPile">The destination pile.</param>
        /// <returns>True if the move is compatible; otherwise false.</returns>
        private bool MoveCompatible(Guid physicalObject, Guid destinationPile)
        {
            Type physicalObjectType = physicalObject.GetType();
            Type destinationPileType = destinationPile.GetType();

            if (physicalObjectType == typeof(ICard) && destinationPileType == typeof(CardPile))
            {
                return true;
            }
            else if (physicalObjectType == typeof(IChip) && destinationPileType == typeof(ChipPile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
