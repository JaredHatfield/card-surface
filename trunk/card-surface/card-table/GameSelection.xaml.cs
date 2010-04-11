// <copyright file="GameSelection.xaml.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The source code responsible for managing the GameSelection.xaml SurfaceWindow</summary>
namespace CardTable
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    using CardCommunication;
    using CardGame;
    using Microsoft.Surface;
    using Microsoft.Surface.Presentation;
    using Microsoft.Surface.Presentation.Controls;

    /// <summary>
    /// Interaction logic for GameSelection.xaml
    /// </summary>
    public partial class GameSelection : SurfaceWindow
    {
        /// <summary>
        /// Semaphore for waiting and notifying this window that a Game object has been received via communication
        /// </summary>
        private object gameReceivedSemaphore;

        /// <summary>
        /// String holding the temporary value for the selected Game type.  Used to determine the submenu New Game selection type.
        /// This property should NOT be relied upon after a game has been selected.
        /// </summary>
        private string gameTypeSelection;

        /// <summary>
        /// The latest game object to be updated.  Should only be accessed when in control of gameReceivedSemaphore!
        /// </summary>
        private Game gameToUpdate;

        /// <summary>
        /// The TableCommunicationController for this GameTableInstance
        /// </summary>
        private TableCommunicationController tcc;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSelection"/> class.
        /// </summary>
        public GameSelection()
        {
            InitializeComponent();

            this.gameReceivedSemaphore = new object();
            this.gameTypeSelection = String.Empty;
            this.gameToUpdate = null;
            this.tcc = GameTableInstance.Instance.CommunicationController;

            this.tcc.OnUpdateGameList += new TableCommunicationController.UpdateGameListHandler(this.OnUpdateGameList);
            this.tcc.OnUpdateGameState += new TableCommunicationController.UpdateGameStateHandler(this.OnUpdateGameState);
            this.tcc.OnUpdateExistingGames += new TableCommunicationController.UpdateExistingGamesHandler(this.OnUpdateExistingGames);

            this.tcc.SendRequestGameListMessage();

            // Add handlers for Application activation events
            this.AddActivationHandlers();
        }

        /// <summary>
        /// Generic delegate utilized by Dispatcher invocations for single string argument methods returning void
        /// </summary>
        /// <param name="param">The single string parameter</param>
        private delegate void OneArgDelegate(string param);

        /// <summary>
        /// Generic delegate utilized by Dispatcher invocations for methods containing no arugments and returning void
        /// </summary>
        private delegate void NoArgDelegate();

        /// <summary>
        /// Called when [update game list].
        /// Responsible for invoking the dispatcher for creating new SurfaceButtons on the window.
        /// </summary>
        /// <param name="gameList">The game list.</param>
        protected void OnUpdateGameList(Collection<string> gameList)
        {
            for (int i = 0; i < gameList.Count; i++)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new OneArgDelegate(this.AddNewGameOption), gameList.ElementAt(i));
            }
        }

        /// <summary>
        /// Called when [update game state].
        /// Responsible for updating the game state.
        /// </summary>
        /// <param name="game">The game update.</param>
        protected void OnUpdateGameState(Game game)
        {
            /* When GameSelection.xaml receives this event, it means a new Game has been created.
             * We need to send it to GameTableInstance to create the new CardTableWindow. */
            lock (this.gameReceivedSemaphore)
            {
                /* The user has selected a game.  Reset the gameTypeSelection "menu" tracker. */
                this.gameTypeSelection = String.Empty;
                /* Install the game state for this GameTableInstance. */
                this.gameToUpdate = game;
                /* Release the GameSelection window from its responsibility of processing GameState updates. 
                 * GameTableInstance is now in charge. */
                this.tcc.OnUpdateGameState -= this.OnUpdateGameState;
                /* Notify the window UI that we've attached a new game for play. */
                Monitor.Pulse(this.gameReceivedSemaphore);
                /* GameSelection's responsibilitiy is done. The game has been installed! */
            }            
        }

        /// <summary>
        /// Called when [update existing games].
        /// </summary>
        /// <param name="existingGames">The existing games.</param>
        protected void OnUpdateExistingGames(Collection<string> existingGames)
        {
            /* This is the cryptic string format which existing game information will be received in.
             * TODO: CardTable and whoever sends this need to agree on a format programmatically. */
            /* TODO: We need to determine 8 from the max players option in the CardGame library! */
            string newGame = "New Game%" + Guid.Empty + "%0/8";

            /* Remove the Game Type selection buttons so we can show the games we have available. */
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NoArgDelegate(this.Games.Items.Clear));

            /* Add the new game option first.  If lots of games are currently available, players shouldn't have to scroll
             * to arrive at the default option each time. */
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new OneArgDelegate(this.AddExistingGameOption), newGame);
      
            for (int i = 0; i < existingGames.Count; i++)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new OneArgDelegate(this.AddExistingGameOption), existingGames.ElementAt(i));
            }
        }

        /// <summary>
        /// Occurs when the window is about to close.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for Application activation events
            this.RemoveActivationHandlers();
        }

        /// <summary>
        /// Adds the new game option.  
        /// This method should only be called via Dispatcher invocation.
        /// </summary>
        /// <param name="gameName">Name of the game.</param>
        private void AddNewGameOption(string gameName)
        {
            Debug.WriteLine("GameSelection.xaml.cs: Adding new " + gameName + " Game Option to the GameSelection SurfaceWindow.");
            SurfaceButton sb = new SurfaceButton();
            sb.Content = gameName;
            /* The Surface, Surface Emulator, or Touch Screen may require additional event handlers to perform the needed action. */
            sb.Click += new RoutedEventHandler(this.GameSelectionClick);
            this.Games.Items.Add(sb);
        }

        /// <summary>
        /// Adds the existing game option.
        /// </summary>
        /// <param name="gameName">String composite of the game.</param>
        private void AddExistingGameOption(string gameName)
        {
            //// Logic for this function
            char[] splitter = { '%' };
            string[] game = gameName.Split(splitter);
            Debug.WriteLine("GameSelection.xaml.cs: Adding new " + game[0] + " Game Option to the GameSelection SurfaceWindow.");
            SurfaceButton sb = new SurfaceButton();
            sb.Content = game[0] + " (Players: " + game[2] + ")";
            sb.Tag = this.gameTypeSelection;
            sb.Uid = game[1];
            /* The Surface, Surface Emulator, or Touch Screen may require additional event handlers to perform the needed action. */
            sb.Click += new RoutedEventHandler(this.ActiveGameClick);
            this.Games.Items.Add(sb);
        }

        /// <summary>
        /// Games the selection click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void GameSelectionClick(object sender, RoutedEventArgs e)
        {
            this.gameTypeSelection = ((SurfaceButton)sender).Content.ToString();
            Debug.WriteLine("GameSelection.xaml.cs: Requesting existing " + this.gameTypeSelection + " games... (Click Event)");
            this.tcc.SendRequestExistingGames(this.gameTypeSelection);
        }

        /// <summary>
        /// Games the selection click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void ActiveGameClick(object sender, RoutedEventArgs e)
        {
            SurfaceButton clickedButton = (SurfaceButton)sender;

            if ((new Guid(clickedButton.Uid)).Equals(Guid.Empty))
            {
                Debug.WriteLine("GameSelection.xaml.cs: Creating a new " + this.gameTypeSelection + " game... (Click Event)");
                this.tcc.SendRequestGameMessage(this.gameTypeSelection);
            }
            else
            {
                Debug.WriteLine("GameSelection.xaml.cs: Joining an existing " + this.gameTypeSelection + " game... (Click Event)");
                this.tcc.SendRequestGameMessage(clickedButton.Uid);
            }
            
            lock (this.gameReceivedSemaphore)
            {
                while (GameTableInstance.Instance.CurrentGame == null)
                {
                    Debug.WriteLine("GameSelection.xaml.cs: Waiting for GameState response...");
                    Monitor.Wait(this.gameReceivedSemaphore);
                    if (this.gameToUpdate != null)
                    {
                        Debug.WriteLine("GameSelection.xaml.cs: New Game Received!");
                        GameTableInstance.Instance.CreateNewGame(this.gameToUpdate);
                        GameTableInstance.Instance.GameWindow.Show();
                    }
                    else
                    {
                        Debug.WriteLine("GameSelection.xaml.cs: Uh oh!  This is definitely not good...  Someone tricked us into thinking we received a new game state.");
                    }
                }
            }

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NoArgDelegate(this.Hide));            
        }

        /// <summary>
        /// Adds handlers for Application activation events.
        /// </summary>
        private void AddActivationHandlers()
        {
            // Subscribe to surface application activation events
            ApplicationLauncher.ApplicationActivated += this.OnApplicationActivated;
            ApplicationLauncher.ApplicationPreviewed += this.OnApplicationPreviewed;
            ApplicationLauncher.ApplicationDeactivated += this.OnApplicationDeactivated;
        }

        /// <summary>
        /// Removes handlers for Application activation events.
        /// </summary>
        private void RemoveActivationHandlers()
        {
            // Unsubscribe from surface application activation events
            ApplicationLauncher.ApplicationActivated -= this.OnApplicationActivated;
            ApplicationLauncher.ApplicationPreviewed -= this.OnApplicationPreviewed;
            ApplicationLauncher.ApplicationDeactivated -= this.OnApplicationDeactivated;
        }

        /// <summary>
        /// This is called when application has been activated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnApplicationActivated(object sender, EventArgs e)
        {
            // TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when application is in preview mode.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnApplicationPreviewed(object sender, EventArgs e)
        {
            // TODO: Disable audio here if it is enabled

            // TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when application has been deactivated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnApplicationDeactivated(object sender, EventArgs e)
        {
            // TODO: disable audio, animations here
        }
    }
}