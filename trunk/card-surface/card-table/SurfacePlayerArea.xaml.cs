// <copyright file="SurfacePlayerArea.xaml.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Interaction logic for SurfacePlayerArea.xaml</summary>
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
    /// Interaction logic for SurfacePlayerArea.xaml
    /// </summary>
    public partial class SurfacePlayerArea : SurfaceUserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SurfacePlayerArea"/> class.
        /// </summary>
        public SurfacePlayerArea()
        {
            InitializeComponent();

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
        /// Called when PreviewDropHandler is called.
        /// This method forces every drop to be a move.  This specifically prevents a LibraryBar for leaving behind inactive objects.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Microsoft.Surface.Presentation.SurfaceDragDropEventArgs"/> instance containing the event data.</param>
        private void OnPreviewDrop(object sender, SurfaceDragDropEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }
    }
}
