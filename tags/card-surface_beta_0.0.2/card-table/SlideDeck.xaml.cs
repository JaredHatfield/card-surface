// <copyright file="SlideDeck.xaml.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The deck popup.</summary>
namespace CardTable
{
    using System;
    using System.Collections.Generic;
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
    using Microsoft.Surface;
    using Microsoft.Surface.Presentation;
    using Microsoft.Surface.Presentation.Controls;

    /// <summary>
    /// Interaction logic for SlideDeck.xaml
    /// </summary>
    public partial class SlideDeck : SurfaceUserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SlideDeck"/> class.
        /// </summary>
        public SlideDeck()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the MouseDown event of the ClosePopup control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void ClosePopup_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Remove this popup from its partent's grid
            Grid cw = this.Parent as Grid;
            cw.Children.Remove(this);
        }
    }
}
