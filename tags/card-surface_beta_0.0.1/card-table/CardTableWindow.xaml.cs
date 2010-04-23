// <copyright file="CardTableWindow.xaml.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>CardTable Window where the game is played.</summary>
namespace CardTable
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
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
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class CardTableWindow : SurfaceWindow
    {
        /// <summary>
        /// The local game that is being played.
        /// </summary>
        private GameSurface game;

        /// <summary>
        /// The SurfacePlayingArea that represents the Game's GamingArea
        /// </summary>
        private SurfacePlayingArea surfaceGamingArea;

        /// <summary>
        /// Initializes a new instance of the CardTableWindow class.
        /// </summary>
        /// <param name="game">The game that we are playing.</param>
        public CardTableWindow(GameSurface game)
        {
            InitializeComponent();
            
            // Set up the game!
            this.game = game;

            // Set up the gaming area
            this.surfaceGamingArea = new SurfacePlayingArea(this.game.GamingArea);
            this.surfaceGamingArea.HorizontalAlignment = HorizontalAlignment.Center;
            this.surfaceGamingArea.VerticalAlignment = VerticalAlignment.Center;
            this.GameGrid.Children.Add(this.surfaceGamingArea);

            // Bind the update for the gaming area
            TableManager.Instance().CurrentGame.GameStateDidFinishUpdate += new CardCommunication.GameNetworkClient.GameStateDidFinishUpdateDelegate(this.surfaceGamingArea.UpdatePlayingAreaPiles);

            // Bind all of the seats!
            this.PlayerEast.BindSeat(game.GetSeat(Seat.SeatLocation.East));
            this.PlayerNorth.BindSeat(game.GetSeat(Seat.SeatLocation.North));

            // this.PlayerNorthEast.BindSeat(game.GetSeat(Seat.SeatLocation.NorthEast));
            // this.PlayerNorthWest.BindSeat(game.GetSeat(Seat.SeatLocation.NorthWest));
            this.PlayerSouth.BindSeat(game.GetSeat(Seat.SeatLocation.South));

            // this.PlayerSouthEast.BindSeat(game.GetSeat(Seat.SeatLocation.SouthEast));
            // this.PlayerSouthWest.BindSeat(game.GetSeat(Seat.SeatLocation.SouthWest));
            this.PlayerWest.BindSeat(game.GetSeat(Seat.SeatLocation.West));

            // Add handlers for Application activation events
            this.AddActivationHandlers();
        }

        /// <summary>
        /// Generic delegate utilized by Dispatcher invocations for methods containing no arugments and returning void
        /// </summary>
        private delegate void NoArgDelegate();

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for Application activation events
            this.RemoveActivationHandlers();
        }

        /// <summary>
        /// Updates the gaming area.
        /// </summary>
        private void UpdateGamingArea()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NoArgDelegate(this.UpdateGamingAreaDispatched));
        }

        /// <summary>
        /// Updates the gaming area dispatched.
        /// </summary>
        private void UpdateGamingAreaDispatched()
        {
            this.surfaceGamingArea.UpdatePlayingAreaPiles();
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
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void OnApplicationActivated(object sender, EventArgs e)
        {
            // TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when application is in preview mode.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void OnApplicationPreviewed(object sender, EventArgs e)
        {
            // TODO: Disable audio here if it is enabled

            // TODO: optionally enable animations here
        }

        /// <summary>
        ///  This is called when application has been deactivated.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void OnApplicationDeactivated(object sender, EventArgs e)
        {
            // TODO: disable audio, animations here
        }
    }
}