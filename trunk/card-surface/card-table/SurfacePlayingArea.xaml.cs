// <copyright file="SurfacePlayingArea.xaml.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Interaction logic for SurfacePlayingArea.xaml</summary>
namespace CardTable
{
    using System;
    using System.Collections.Generic;
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
    using CardGame;
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
            /*
            foreach (ChipPile chipPile in this.playingArea.Chips)
            {
                Debug.WriteLine("SurfacePlayingArea.xaml.cs: Created a new ChipPile (as a LibraryStack) in the SurfacePlayingArea for " + chipPile.Id);
                LibraryStack surfaceChipPile = new LibraryStack();
                surfaceChipPile.Height = 80;
                surfaceChipPile.Width = 80;
                Binding chipPileBinding = new Binding("BindableChips");
                chipPileBinding.Source = chipPile;
                surfaceChipPile.SetBinding(LibraryStack.ItemsSourceProperty, chipPileBinding);
                this.PlayingAreaGrid.Children.Add(surfaceChipPile);
            }

            foreach (CardPile cardPile in this.playingArea.Cards)
            {
                if (cardPile.NumberOfItems != 0)
                {
                    Debug.WriteLine("SurfacePlayingArea.xaml.cs: Created a new CardPile (as a LibraryStack) in the SurfacePlayingArea for " + cardPile.Id);
                    LibraryStack surfaceCardPile = new LibraryStack();
                    surfaceCardPile.Height = 80;
                    surfaceCardPile.Width = 80;

                    for (int i = 0; i < this.PlayingAreaGrid.Children.Count; i++)
                    {
                        Debug.WriteLine(this.PlayingAreaGrid.Children[i].ToString());
                    }

                    Binding cardPileBinding = new Binding("BindableCards");
                    cardPileBinding.Source = (CardPile)cardPile;

                    surfaceCardPile.SetBinding(LibraryStack.ItemsSourceProperty, cardPileBinding);
                    this.PlayingAreaGrid.Children.Add(surfaceCardPile);
                }
            }
             */
        }
    }
}
