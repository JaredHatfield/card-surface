﻿// <copyright file="SurfacePlayer.xaml.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Interaction logic for SurfacePlayer.xaml</summary>
namespace CardTable
{
    using System;
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
        /// Initializes a new instance of the <see cref="SurfacePlayer"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        public SurfacePlayer(Player player)
        {
            InitializeComponent();

            this.player = player;

            /* TODO: Implement dynamic pile bindings. */
            /*this.surfacePlayingArea = new SurfacePlayingArea(this.player.PlayerArea);
            this.PlayerGrid.Children.Add(this.surfacePlayingArea);*/

            // We want to always assume objects are being moved
            // This insures that when we move out of a LibraryBar we do not get an inactive object left behind
            SurfaceDragDrop.AddPreviewDropHandler(this.bank, this.OnPreviewDrop);
            SurfaceDragDrop.AddPreviewDropHandler(this.hand, this.OnPreviewDrop);
            SurfaceDragDrop.AddPreviewDropHandler(this.chip0, this.OnPreviewDrop);
            SurfaceDragDrop.AddPreviewDropHandler(this.chip1, this.OnPreviewDrop);
            SurfaceDragDrop.AddPreviewDropHandler(this.card0, this.OnPreviewDrop);
            SurfaceDragDrop.AddPreviewDropHandler(this.card1, this.OnPreviewDrop);
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
            this.UpdatePlayerPiles();
        }

        /// <summary>
        /// Updates the player piles.
        /// </summary>
        private void UpdatePlayerPiles()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NoArgDelegate(this.UpdatePlayerPilesDispatched));
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
                this.bank.Items.Clear();
                for (int i = 0; i < currentBank.Chips.Count; i++)
                {
                    SurfaceChip surfaceChip = currentBank.Chips[i] as SurfaceChip;
                    SurfacePhysicalObject spo = new SurfacePhysicalObject(currentBank.Chips[i].Id, surfaceChip.ObjectImageSource);
                    this.bank.Items.Add(spo);
                }

                CardPile currentHand = this.player.Hand;
                this.hand.Items.Clear();
                for (int i = 0; i < currentHand.Cards.Count; i++)
                {
                    SurfacePhysicalObject spo = new SurfacePhysicalObject(currentHand.Cards[i].Id, "Resources/CardBack.png");
                    this.hand.Items.Add(spo);
                }

                CardPile currentCard0 = this.player.PlayerArea.Cards[0];
                this.card0.Items.Clear();
                for (int i = 0; i < currentCard0.Cards.Count; i++)
                {
                    SurfacePhysicalObject spo = new SurfacePhysicalObject(currentCard0.Cards[i].Id, "Resources/CardBack.png");
                    this.card0.Items.Add(spo);
                }

                CardPile currentCard1 = this.player.PlayerArea.Cards[1];
                this.card1.Items.Clear();
                for (int i = 0; i < currentCard1.Cards.Count; i++)
                {
                    SurfacePhysicalObject spo = new SurfacePhysicalObject(currentCard1.Cards[i].Id, "Resources/CardBack.png");
                    this.card1.Items.Add(spo);
                }

                ChipPile currentChip0 = this.player.PlayerArea.Chips[0];
                this.chip0.Items.Clear();
                for (int i = 0; i < currentChip0.Chips.Count; i++)
                {
                    SurfaceChip surfaceChip = currentChip0.Chips[i] as SurfaceChip;
                    SurfacePhysicalObject spo = new SurfacePhysicalObject(surfaceChip.Id, surfaceChip.ObjectImageSource);
                    this.chip0.Items.Add(spo);
                }

                ChipPile currentChip1 = this.player.PlayerArea.Chips[1];
                this.chip1.Items.Clear();
                for (int i = 0; i < currentChip1.Chips.Count; i++)
                {
                    SurfaceChip surfaceChip = currentChip1.Chips[i] as SurfaceChip;
                    SurfacePhysicalObject spo = new SurfacePhysicalObject(surfaceChip.Id, surfaceChip.ObjectImageSource);
                    this.chip0.Items.Add(spo);
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
            e.Effects = DragDropEffects.Move;
        }
    }
}
