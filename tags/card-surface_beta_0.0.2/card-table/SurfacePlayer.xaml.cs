// <copyright file="SurfacePlayer.xaml.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Interaction logic for SurfacePlayer.xaml</summary>
namespace CardTable
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    using CardGame;
    using CardTable.GameObjects;
    using Microsoft.Surface;
    using Microsoft.Surface.Presentation;
    using Microsoft.Surface.Presentation.Controls;

    /// <summary>
    /// Interaction logic for SurfacePlayer.xaml
    /// </summary>
    public partial class SurfacePlayer : SurfaceUserControl
    {
        /// <summary>
        /// The Player that this SurfacePlayer represents
        /// </summary>
        private Player player;

        /// <summary>
        /// This player's username.
        /// </summary>
        private string username;

        /// <summary>
        /// The SurfacePlaying area that is contained within the player.
        /// </summary>
        private SurfacePlayingArea surfacePlayingArea;

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfacePlayer"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="username">The username.</param>
        public SurfacePlayer(Player player, string username)
        {
            InitializeComponent();

            this.player = player;
            this.username = username;

            SurfacePlayingArea spa = new SurfacePlayingArea(player.PlayerArea, 70);
            spa.HorizontalAlignment = HorizontalAlignment.Center;
            spa.VerticalAlignment = VerticalAlignment.Top;
            this.PlayerGrid.Children.Add(spa);
            this.surfacePlayingArea = spa;

            this.PlayerNameLabel.Content = username;

            /* TODO: Implement dynamic pile bindings. */
            /*this.surfacePlayingArea = new SurfacePlayingArea(this.player.PlayerArea);
            this.PlayerGrid.Children.Add(this.surfacePlayingArea);*/

            // We want to always assume objects are being moved
            // This insures that when we move out of a LibraryBar we do not get an inactive object left behind
            SurfaceDragDrop.AddPreviewDropHandler(this.bank, this.OnPreviewDrop);
            SurfaceDragDrop.AddPreviewDropHandler(this.hand, this.OnPreviewDrop);
        }

        /// <summary>
        /// Generic delegate utilized by Dispatcher invocations for methods containing no arugments and returning void
        /// </summary>
        private delegate void NoArgDelegate();

        /// <summary>
        /// Binds the piles.
        /// </summary>
        internal void BindPiles()
        {
            TableManager.Instance().CurrentGame.GameStateDidFinishUpdate += new CardCommunication.GameNetworkClient.GameStateDidFinishUpdateDelegate(this.UpdatePlayerPiles);
            TableManager.Instance().CurrentGame.GameStateDidFinishUpdate += new CardCommunication.GameNetworkClient.GameStateDidFinishUpdateDelegate(this.surfacePlayingArea.UpdatePlayingAreaPiles);
            this.UpdatePlayerPiles();
        }

        /// <summary>
        /// Updates the player piles.
        /// </summary>
        private void UpdatePlayerPiles()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NoArgDelegate(this.UpdatePlayerPilesDispatched));
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NoArgDelegate(this.UpdateActions));
        }

        /// <summary>
        /// Updates the actions.
        /// </summary>
        private void UpdateActions()
        {
            if (this.player != null)
            {
                if (this.player.Actions.Count == 0)
                {
                    this.action.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.action.Visibility = Visibility.Visible;
                }

                this.action.Items.Clear();

                foreach (string s in this.player.Actions)
                {
                    ElementMenuItem e = new ElementMenuItem();
                    e.Command = new SurfaceAction(this.username);
                    e.CommandParameter = s;
                    e.Header = s;
                    e.Visibility = Visibility.Visible;               

                    this.action.Items.Add(e);
                }
            }
        }

        /// <summary>
        /// Updates the player piles used by the dispatcher.
        /// </summary>
        private void UpdatePlayerPilesDispatched()
        {
            if (this.player != null)
            {
                // Update the players piles
                BankPile currentBank = this.player.BankPile;
                this.bank.Uid = currentBank.Id.ToString();
                this.BankBalanceLabel.Content = "$" + this.player.Money.ToString();

                // Add all of the missing objects
                for (int i = 0; i < currentBank.Chips.Count; i++)
                {
                    SurfaceChip surfaceChip = currentBank.Chips[i] as SurfaceChip;
                    if (!this.bank.Items.Contains(surfaceChip))
                    {
                        this.bank.Items.Add(surfaceChip);
                    }
                }

                // Remove all of the unnecessary objects
                Collection<SurfaceChip> chipsToRemove = new Collection<SurfaceChip>();
                for (int i = 0; i < this.bank.Items.Count; i++)
                {
                    SurfaceChip surfaceChip = this.bank.Items[i] as SurfaceChip;
                    if (!currentBank.Chips.Contains(surfaceChip))
                    {
                        chipsToRemove.Add(surfaceChip);
                    }
                }

                for (int i = 0; i < chipsToRemove.Count; i++)
                {
                    this.bank.Items.Remove(chipsToRemove[i]);
                }

                // Update the hand
                CardPile currentHand = this.player.Hand;
                this.hand.Uid = currentHand.Id.ToString();

                // Add all of the missing objects
                for (int i = 0; i < currentHand.Cards.Count; i++)
                {
                    SurfaceCard surfaceCard = currentHand.Cards[i] as SurfaceCard;
                    if (!this.hand.Items.Contains(surfaceCard))
                    {
                        this.hand.Items.Add(surfaceCard);
                    }
                }

                // Remove all of the unnecessary objects
                Collection<SurfaceCard> cardsToRemove = new Collection<SurfaceCard>();
                for (int i = 0; i < this.hand.Items.Count; i++)
                {
                    SurfaceCard surfaceCard = this.hand.Items[i] as SurfaceCard;
                    if (!currentHand.Cards.Contains(surfaceCard))
                    {
                        cardsToRemove.Add(surfaceCard);
                    }
                }

                for (int i = 0; i < cardsToRemove.Count; i++)
                {
                    this.hand.Items.Remove(cardsToRemove[i]);
                }
            }
        }

        /// <summary>
        /// Called when PreviewDropHandler is called.
        /// This method forces every drop to be a move.  This specifically prevents a LibraryBar from leaving behind inactive objects.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Microsoft.Surface.Presentation.SurfaceDragDropEventArgs"/> instance containing the event data.</param>
        private void OnPreviewDrop(object sender, SurfaceDragDropEventArgs e)
        {
            // We don't want to duplicate object when we move out of a LibraryBar
            e.Effects = DragDropEffects.Move;

            // Get the information about the object that is being moved and the destination
            Guid physicalObject = (e.Cursor.Data as IPhysicalObject).Id;
            Guid destination = new Guid(e.Cursor.CurrentTarget.Uid);

            // Perform the move
            bool actionSuccess = TableManager.Instance().TableCommunicationController.SendMoveActionMessage(physicalObject.ToString(), destination.ToString()).ActionSuccessful;

            // If the move failed we don't want it to actually move the piece
            if (!actionSuccess)
            {
                e.Handled = true;
            }
        }
    }
}
