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
    using GameBlackjack;
    using Microsoft.Surface;
    using Microsoft.Surface.Presentation;
    using Microsoft.Surface.Presentation.Controls;

    /// <summary>
    /// Interaction logic for GameSelection.xaml
    /// </summary>
    public partial class GameSelection : SurfaceWindow
    {
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

            this.tcc = GameTableInstance.Instance.CommunicationController;

            this.tcc.OnUpdateGameList += new TableCommunicationController.UpdateGameListHandler(this.OnUpdateGameList);

            this.tcc.SendRequestGameListMessage();

            // Add handlers for Application activation events
            this.AddActivationHandlers();
        }

        /// <summary>
        /// Generic delegate utilized by Dispatcher invocations for single string argument methods
        /// </summary>
        /// <param name="param">The single string parameter</param>
        private delegate void OneArgDelegate(string param);

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
        /// Games the selection click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void GameSelectionClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Do you really want to create a new " + sender.ToString() + " game? (Click Event)");
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