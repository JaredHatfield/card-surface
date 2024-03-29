﻿// <copyright file="Game.cs" company="University of Louisville Speed School of Engineering">
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
    using CardGame.GameException;

    /// <summary>
    /// A generic card game that is extended to implement a specific game.
    /// </summary>
    [Serializable] 
    public abstract class Game
    {
        /// <summary>
        /// A unique identifier for a game.
        /// </summary>
        private Guid id;

        /// <summary>
        /// Whether the last action was successful.
        /// </summary>
        private bool actionSuccess;

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
        [NonSerialized]
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
            this.UpdatePlayerState();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// This creates a copy of a Game and is used in converting a specific instance of a Game from one type to another.
        /// </summary>
        /// <param name="game">The game to mimic.</param>
        protected Game(Game game)
        {
            this.id = game.id;
            this.actionSuccess = game.actionSuccess;
            this.gamingArea = game.gamingArea;
            this.deckPile = game.deckPile;
            this.seats = game.seats;
            this.actions = game.actions;
        }

        /// <summary>
        /// The delegate for an event that is triggered when a player leaves a game.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        public delegate void PlayerWillLeaveGameEventHandler(object sender, PlayerLeaveGameEventArgs e);

        /// <summary>
        /// The delegate for an event that is triggered when a player has left a game
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="location">The location that was vacated.</param>
        public delegate void PlayerDidLeaveGameEventHandler(object sender, Seat.SeatLocation location);

        /// <summary>
        /// The delegate for an event that is triggered when a player joins a game.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        public delegate void PlayerJoinGameEventHandler(object sender, PlayerJoinGameEventArgs e);

        /// <summary>
        /// The delegate for an event that is triggered when the game state is updated.
        /// </summary>
        public delegate void GameStateUpdatedHandler();

        /// <summary>
        /// Occurs when a player leaves the game.
        /// </summary>
        public event PlayerWillLeaveGameEventHandler PlayerWillLeaveGame;

        /// <summary>
        /// Occurs when [player did leave game].
        /// </summary>
        public event PlayerDidLeaveGameEventHandler PlayerDidLeaveGame;

        /// <summary>
        /// Occurs when a player joins the game.
        /// </summary>
        public event PlayerJoinGameEventHandler PlayerJoinGame;

        /// <summary>
        /// Occurs when the game state is updated.
        /// </summary>
        public event GameStateUpdatedHandler GameStateUpdated;

        /// <summary>
        /// Gets the name of the game.
        /// </summary>
        /// <value>The game's name.</value>
        public abstract string Name
        {
            get;
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
        /// Gets a value indicating whether [action successful].
        /// </summary>
        /// <value><c>true</c> if [action successful]; otherwise, <c>false</c>.</value>
        public bool ActionSuccessful
        {
            get { return this.actionSuccess; }
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
        /// Gets the number of seats.
        /// This number only includes those seats that are sittable.
        /// </summary>
        /// <value>The number of seats.</value>
        public int NumberOfSeats
        {
            get
            {
                int number = 0;
                for (int i = 0; i < this.seats.Count; i++)
                {
                    if (this.seats[i].Sittable)
                    {
                        number++;
                    }
                }

                return number;
            }
        }

        /// <summary>
        /// Gets a value indicating the minimum amount of money required to join a game.
        /// </summary>
        /// <value>The minimum stake for the game.</value>
        public abstract int MinimumStake
        {
            get;
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
        /// Flips a card from face up to face down or face down to face up.
        /// </summary>
        /// <param name="id">The id of the card.</param>
        /// <returns>
        /// True if card was flipped; otherwise false.
        /// </returns>
        public bool FlipCard(Guid id)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (!this.seats[i].IsEmpty && this.seats[i].Player.ContainsCard(id))
                {
                    return this.seats[i].Player.FlipCard(id);
                }
            }

            return false;
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

            throw new CardGamePlayerNotFoundException();
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

            throw new CardGamePlayerNotFoundException();
        }

        /// <summary>
        /// Gets a Seat based off of a Guid.
        /// </summary>
        /// <param name="seatId">The seat id.</param>
        /// <returns>The instance of the Seat.</returns>
        public Seat GetSeat(Guid seatId)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (!this.seats[i].IsEmpty && this.seats[i].Id.Equals(seatId))
                {
                    return this.seats[i];
                }
            }

            throw new CardGameSeatNotFoundException();
        }

        /// <summary>
        /// Gets a Seat based off of a SeatLocation.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>The instance of the Seat.</returns>
        public Seat GetSeat(Seat.SeatLocation location)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (this.seats[i].Location.Equals(location))
                {
                    return this.seats[i];
                }
            }

            throw new CardGameSeatNotFoundException();
        }

        /// <summary>
        /// Gets a Seat based off of a Player's username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The instance of the Seat.</returns>
        public Seat GetSeat(string username)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (!this.seats[i].IsEmpty && this.seats[i].Username.Equals(username))
                {
                    return this.seats[i];
                }
            }

            throw new CardGameSeatNotFoundException();
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
        /// <param name="userImagePath">The user image path.</param>
        /// <returns>
        /// True if the user was able to sit down; otherwise false.
        /// </returns>
        public virtual bool SitDown(string username, string password, int amount, string userImagePath)
        {
            if (amount < this.MinimumStake)
            {
                // If the player did not provide the minimum stake, we thrown an exception
                throw new CardGameMinimumStakeException();
            }
            else if (this.SitDown(username, password, userImagePath))
            {
                // Get the player and add the money to their account
                Player player = this.GetPlayer(username);
                player.Balance += amount;
                player.BankPile.RefreshChipPile();
                this.OnJoinGame(new PlayerJoinGameEventArgs(this.id, username, amount));
                return true;
            }
            else
            {
                // TODO: Should this be an exception?
                return false;
            }
        }

        /// <summary>
        /// Attempt to have a user sit down at a table.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="userImagePath">The user image path.</param>
        /// <returns>
        /// True if the user was able to sit down; otherwise false.
        /// </returns>
        public virtual bool SitDown(string username, string password, string userImagePath)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (this.seats[i].PasswordPeek(password))
                {
                    bool result = this.seats[i].SitDown(username, password, userImagePath);
                    if (result)
                    {
                        this.UpdatePlayerState();
                    }

                    return result;
                }
            }

            return false;
        }

        /// <summary>
        /// Have a player leave the game.
        /// </summary>
        /// <param name="username">The username.</param>
        public void Leave(string username)
        {
            // Calculate the players monetary worth on the table
            Player p = this.GetPlayer(username);
            int total = p.Money;

            // Trigger the event that the player will leave the game.
            this.OnWillLeaveGame(new PlayerLeaveGameEventArgs(this.id, username, total));

            // Remove the reference of the player from their Seat
            Seat s = this.GetSeat(username);
            s.Leave();

            // Trigger the event that the player did leave the game.
            this.OnDidLeaveGame(s.Location);
        }

        /// <summary>
        /// Sets the action status.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        public void SetActionStatus(bool success)
        {
            this.actionSuccess = success;
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
        /// Regenerates the seat code.
        /// </summary>
        /// <param name="seatGuid">The seat GUID.</param>
        /// <returns>whether the seat code was changed.</returns>
        public bool RegenerateSeatCode(Guid seatGuid)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (this.seats[i].Id == seatGuid)
                {
                    this.GetSeat(seatGuid).RegenerateSeatCode();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Move the specified IPhysicalObject into the specified Pile.
        /// This method takes the Guid of the PhysicalObject to move and the Guid of the pile to move it to.
        /// If the move was successful, this function returns true.
        /// This method will throw a CardGamePhysicalObjectNotFoundException if the PhysicalObject is not part of the game.
        /// This method will throw a CardGamePileNotFoundException if the pile is not part of the game.
        /// </summary>
        /// <param name="physicalObject">The PhysicalObject's Guid.</param>
        /// <param name="destinationPile">The destination pile's Guid.</param>
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
                return this.MovePhysicalObject(physicalObject, destinationPile);
            }
        }

        /// <summary>
        /// Execute a GameAction.
        /// This method takes the string representation of a GameAction and the username of the player that triggered the action.
        /// This method returns true if the action was executed successfully.
        /// This method will throw a CardGameActionNotFoundException if the specified action does not exist.
        /// This method will throw a CardGameActionAccessDeniedException if the user is not allowed to execute the action.
        /// This method will throw a CardGamePlayerNotFoundExcepting if the username is in the game.
        /// Other exceptions may be thrown depending on the implementation of the GameAction.
        /// </summary>
        /// <param name="name">The name of the GameAction.</param>
        /// <param name="player">The Player's username.</param>
        /// <returns>True if the GameAction was successful; otherwise false.</returns>
        public bool ExecuteAction(string name, string player)
        {
            if (!this.ActionTest(name, player))
            {
                return false;
            }

            for (int i = 0; i < this.actions.Count; i++)
            {
                if (this.actions[i].Name.Equals(name))
                {
                    bool result = this.actions[i].Action(this, player);
                    if (result)
                    {
                        this.UpdatePlayerState();
                    }

                    return result;
                }
            }

            throw new CardGameActionNotFoundException();
        }

        /// <summary>
        /// Updates the specified game.
        /// </summary>
        /// <param name="game">The game to reflect.</param>
        internal void Update(Game game)
        {
            this.id = game.id;
            this.gamingArea.Update(game.gamingArea);
            this.deckPile = game.deckPile;
        }

        /// <summary>
        /// Moves the physical object.  This method provides no checks and should be used with great care!
        /// </summary>
        /// <param name="physicalObject">The physical object.</param>
        /// <param name="destinationPile">The destination pile.</param>
        /// <returns>True if the move was successful.</returns>
        internal bool MovePhysicalObject(Guid physicalObject, Guid destinationPile)
        {
            // Retreive the physical object's pile
            Pile locatedSourcePile = this.GetPileContaining(physicalObject);

            // Retreive the physical object
            IPhysicalObject locatedPhysicalObject = locatedSourcePile.GetPhysicalObject(physicalObject);

            // Retreive the pile
            Pile locatedDestinationPile = this.GetPile(destinationPile);
            if (!locatedDestinationPile.Open)
            {
                throw new CardGameMoveToNonOpenPileException();
            }

            // Removed the object from the source pile
            locatedSourcePile.RemoveItem(locatedPhysicalObject);

            // Add the physical object to the destination pile
            locatedDestinationPile.AddItem(locatedPhysicalObject);

            this.UpdatePlayerState();
            return true;
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

                throw new CardGameException("Player turn could not be rotated to the next player.");
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

            throw new CardGamePlayerNotFoundException();
        }

        /// <summary>
        /// Gets the active Player's Seat.
        /// </summary>
        /// <returns>The active Player's Seat</returns>
        protected internal Seat GetActivePlayerSeat()
        {
            Player player = this.GetActivePlayer();
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (this.seats[i].Player == player)
                {
                    return this.seats[i];
                }
            }

            throw new CardGameSeatNotFoundException();
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

            throw new CardGamePhysicalObjectNotFoundException();
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

            throw new CardGamePileNotFoundException();
        }

        /// <summary>
        /// Gets the pile containing a physical object with the specified id.
        /// </summary>
        /// <param name="physicalObjectId">The physical object id.</param>
        /// <returns>The instance of the pile containing the specified physical object; otherwise null.</returns>
        protected internal Pile GetPileContaining(Guid physicalObjectId)
        {
            if (this.GamingArea.ContainsCard(physicalObjectId) || this.GamingArea.ContainsChip(physicalObjectId))
            {
                return this.GamingArea.GetPileContaining(physicalObjectId);
            }
            else
            {
                for (int i = 0; i < this.Seats.Count; i++)
                {
                    Seat s = this.Seats[i];
                    if (!s.IsEmpty && (s.Player.ContainsCard(physicalObjectId) || s.Player.ContainsChip(physicalObjectId)))
                    {
                        return this.Seats[i].Player.GetPileContaining(physicalObjectId);
                    }
                }
            }

            throw new CardGamePileNotFoundException();
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
        /// Adds the chip pile to game.
        /// </summary>
        protected internal void AddChipPileToGame()
        {
            this.gamingArea.AddChipPile(new ChipPile());
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
        /// Adds the card pile to game.
        /// </summary>
        protected internal void AddCardPileToGame()
        {
            this.gamingArea.AddCardPile(new CardPile());
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
        /// Updates the all of the Player's state.
        /// Insures the players action list includes only valid options.
        /// Removes or add chips to the players bank.
        /// </summary>
        protected internal void UpdatePlayerState()
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (!this.seats[i].IsEmpty)
                {
                    Player p = this.seats[i].Player;

                    // Update the Player's Actions
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

                    // Update the Player's bank
                    p.BankPile.RefreshChipPile();
                }
            }
        }

        /// <summary>
        /// Sets the seat sittable property.
        /// </summary>
        /// <param name="location">The location to change.</param>
        /// <param name="sittable">if set to <c>true</c> [sittable].</param>
        protected void SetSeatSittable(Seat.SeatLocation location, bool sittable)
        {
            this.GetSeat(location).Sittable = sittable;
        }

        /// <summary>
        /// Raises the event that indicates a player is leaving the game.
        /// </summary>
        /// <param name="e">The <see cref="CardGame.PlayerLeaveGameEventArgs"/> instance containing the event data.</param>
        protected void OnWillLeaveGame(PlayerLeaveGameEventArgs e)
        {
            if (this.PlayerWillLeaveGame != null)
            {
                this.PlayerWillLeaveGame(this, e);
            }
        }

        /// <summary>
        /// Raises the event that indicates a player left the game.
        /// </summary>
        /// <param name="location">The location that the player left.</param>
        protected void OnDidLeaveGame(Seat.SeatLocation location)
        {
            if (this.PlayerDidLeaveGame != null)
            {
                this.PlayerDidLeaveGame(this, location);
            }

            if (this.GameStateUpdated != null)
            {
                this.GameStateUpdated();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:PlayerJoinGame"/> event.
        /// </summary>
        /// <param name="e">The <see cref="CardGame.PlayerJoinGameEventArgs"/> instance containing the event data.</param>
        protected void OnJoinGame(PlayerJoinGameEventArgs e)
        {
            if (this.PlayerJoinGame != null)
            {
                this.PlayerJoinGame(this, e);
            }

            if (this.GameStateUpdated != null)
            {
                this.GameStateUpdated();
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
        /// Test if an action can be performed.
        /// </summary>
        /// <param name="name">The name of the action.</param>
        /// <param name="player">The player's username.</param>
        /// <returns>True if the action can be executed.</returns>
        protected virtual bool ActionTest(string name, string player)
        {
            // This method should be overridden by the network client so that it can intercept the call and route it to the server.
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
            IPhysicalObject myPhysicalObject = this.GetPhysicalObject(physicalObject);
            Pile myPile = this.GetPile(destinationPile);

            if (myPhysicalObject is ICard && myPile is CardPile)
            {
                return true;
            }
            else if (myPhysicalObject is IChip && myPile is ChipPile)
            {
                return true;
            }
            else if (myPhysicalObject is IChip && myPile is BankPile)
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
