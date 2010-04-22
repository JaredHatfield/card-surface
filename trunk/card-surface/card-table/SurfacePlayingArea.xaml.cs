// <copyright file="SurfacePlayingArea.xaml.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Interaction logic for SurfacePlayingArea.xaml</summary>
namespace CardTable
{
    using System;
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
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    using CardGame;
    using CardTable.GameObjects;
    using Microsoft.Surface;
    using Microsoft.Surface.Presentation;
    using Microsoft.Surface.Presentation.Controls;

    /// <summary>
    /// Interaction logic for SurfacePlayingArea.xaml
    /// </summary>
    public partial class SurfacePlayingArea : SurfaceUserControl
    {
        /// <summary>
        /// The PlayingArea that this SurfacePlayingArea binds to
        /// </summary>
        private PlayingArea playingArea;

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfacePlayingArea"/> class.
        /// </summary>
        /// <param name="playingArea">The playing area.</param>
        public SurfacePlayingArea(PlayingArea playingArea)
        {
            InitializeComponent();

            this.playingArea = playingArea;
            this.UpdatePlayingAreaPiles();
        }

        /// <summary>
        /// Generic delegate utilized by Dispatcher invocations for methods containing no arugments and returning void
        /// </summary>
        private delegate void NoArgDelegate();

        /// <summary>
        /// Updates the playing area piles.
        /// </summary>
        internal void UpdatePlayingAreaPiles()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NoArgDelegate(this.UpdatePlayingAreaPilesDispatched));
        }

        /// <summary>
        /// Updates the playing area piles from the dispatcher.
        /// </summary>
        private void UpdatePlayingAreaPilesDispatched()
        {
            // Add all of the missing card piles
            int cardDelta = this.playingArea.Cards.Count - this.CardPilesStack.Children.Count;

            while (cardDelta > 0)
            {
                this.CardPilesStack.Children.Add(this.NewLibraryStack());
                cardDelta--;
            }

            if (cardDelta < 0)
            {
                // TODO: REMOVE DEAD PILES!!!
            }

            // Add all of the missing chip piles
            int chipDelta = this.playingArea.Chips.Count - this.ChipPilesStack.Children.Count;

            while (chipDelta > 0)
            {
                this.ChipPilesStack.Children.Add(this.NewLibraryStack());
                chipDelta--;
            }

            if (chipDelta < 0)
            {
                // TODO: REMOVE DEAD PILES!!!
            }

            // Update all of the chip piles
            for (int n = 0; n < this.playingArea.Chips.Count; n++)
            {
                ChipPile chipPile = this.playingArea.Chips[n];
                LibraryStack chipStack = this.ChipPilesStack.Children[n] as LibraryStack;

                for (int i = 0; i < chipPile.Chips.Count; i++)
                {
                    SurfaceChip surfaceChip = chipPile.Chips[i] as SurfaceChip;
                    if (!chipStack.Items.Contains(surfaceChip))
                    {
                        chipStack.Items.Add(surfaceChip);
                    }
                }

                // Remove all of the unnecessary objects
                Collection<SurfaceChip> chipsToRemove = new Collection<SurfaceChip>();
                for (int i = 0; i < chipStack.Items.Count; i++)
                {
                    SurfaceChip surfaceChip = chipStack.Items[i] as SurfaceChip;
                    if (!chipPile.Chips.Contains(surfaceChip))
                    {
                        chipsToRemove.Add(surfaceChip);
                    }
                }

                for (int i = 0; i < chipsToRemove.Count; i++)
                {
                    chipStack.Items.Remove(chipsToRemove[i]);
                }
            }

            // Update all of the card piles
            for (int n = 0; n < this.playingArea.Cards.Count; n++)
            {
                CardPile cardPile = this.playingArea.Cards[n];
                LibraryStack cardStack = this.CardPilesStack.Children[n] as LibraryStack;

                for (int i = 0; i < cardPile.Cards.Count; i++)
                {
                    SurfaceCard surfaceCard = cardPile.Cards[i] as SurfaceCard;
                    if (!cardStack.Items.Contains(surfaceCard))
                    {
                        cardStack.Items.Add(surfaceCard);
                    }
                }

                // Remove all of the unnecessary objects
                Collection<SurfaceCard> cardsToRemove = new Collection<SurfaceCard>();
                for (int i = 0; i < cardStack.Items.Count; i++)
                {
                    SurfaceCard surfaceCard = cardStack.Items[i] as SurfaceCard;
                    if (!cardPile.Cards.Contains(surfaceCard))
                    {
                        cardsToRemove.Add(surfaceCard);
                    }
                }

                for (int i = 0; i < cardsToRemove.Count; i++)
                {
                    cardStack.Items.Remove(cardsToRemove[i]);
                }
            }
        }

        /// <summary>
        /// Creates a new library stack.
        /// </summary>
        /// <returns>A new LibraryStack to be added to the GUI.</returns>
        private LibraryStack NewLibraryStack()
        {
            LibraryStack libraryStack = new LibraryStack();
            libraryStack.Width = 70;
            libraryStack.Height = 70;
            SurfaceDragDrop.AddPreviewDropHandler(libraryStack, this.OnPreviewDrop);
            return libraryStack;
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
