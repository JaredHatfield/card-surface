// <copyright file="GameSelection.xaml.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The source code responsible for managing the GameSelection.xaml SurfaceWindow</summary>
namespace CardTable
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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

            this.tcc = new TableCommunicationController();

            this.tcc.OnUpdateGameList += new TableCommunicationController.UpdateGameListHandler(this.OnUpdateGameList);

            this.tcc.SendRequestGameListMessage();

            this.tcc.SendRequestGameListMessage();

            // Add handlers for Application activation events
            this.AddActivationHandlers();
        }

        /// <summary>
        /// Called when [update game list].
        /// </summary>
        /// <param name="gameList">The game list.</param>
        protected void OnUpdateGameList(Collection<string> gameList)
        {
            ////This function updates the game list.
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
        /// Updates the game list.
        /// </summary>
        /// <param name="gameList">The game list.</param>
        private void UpdateGameList(System.Collections.ObjectModel.Collection<string> gameList)
        {
            for (int i = 0; i < gameList.Count; i++)
            {
                SurfaceButton sb = new SurfaceButton();
                sb.Content = gameList.ElementAt(i);
                this.Games.Items.Add(sb);
            }
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