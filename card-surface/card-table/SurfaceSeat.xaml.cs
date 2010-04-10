// <copyright file="SurfaceSeat.xaml.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>SurfaceSeat user control used for joining a game.</summary>
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
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using Microsoft.Surface;
    using Microsoft.Surface.Presentation;
    using Microsoft.Surface.Presentation.Controls;

    /// <summary>
    /// Interaction logic for SurfaceSeat.xaml
    /// </summary>
    public partial class SurfaceSeat : SurfaceUserControl
    {
        /// <summary>
        /// The string that represents the seat location
        /// </summary>
        private string seatLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceSeat"/> class.
        /// </summary>
        public SurfaceSeat()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the seat location.
        /// </summary>
        /// <value>The seat location.</value>
        public string SeatLocation
        {
            get { return this.seatLocation; }
            set { this.seatLocation = value; }
        }

        /// <summary>
        /// Handles the Click event of the SurfaceButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void SurfaceButton_Click(object sender, RoutedEventArgs e)
        {
            Binding seatBinding = new Binding("Password");
            seatBinding.Source = GameTableInstance.Instance.CurrentGame.GetSeat(CardGame.Seat.ParseSeatLocation(this.seatLocation));
            SeatPassword.SetBinding(Label.ContentProperty, seatBinding);

            JoinButton.Visibility = Visibility.Hidden;
            SeatPasswordGrid.Visibility = Visibility.Visible;
            /* Start timer so that the seat password disappears after timeout. */
        }
    }
}
