// <copyright file="ConnectionWindow.xaml.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The startup ConnectionWindow that allows the user to connect to a CardServer.</summary>
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
    using System.Windows.Shapes;
    using System.Windows.Threading;
    using Microsoft.Surface;
    using Microsoft.Surface.Presentation;
    using Microsoft.Surface.Presentation.Controls;

    /// <summary>
    /// Interaction logic for ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : SurfaceWindow
    {
        /// <summary>
        /// The timer responsible for making the connection error label disappear
        /// </summary>
        private DispatcherTimer connectionErrorLabelDisplayTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionWindow"/> class.
        /// </summary>
        public ConnectionWindow()
        {
            InitializeComponent();

            // Add handlers for Application activation events
            this.AddActivationHandlers();
        }

        /// <summary>
        /// Occurs when the window is about to close.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for Application activation events
            this.RemoveActivationHandlers();
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

        /// <summary>
        /// Handles the Click event of the RemoteConnection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void RemoteConnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TableManager tm = TableManager.Initialize(ServerAddress.Text);
                tm.GameSelectionWindow.Show();
                this.Hide();
            }
            catch (Exception exception)
            {
                Debug.WriteLine("ConnectionWindow.xaml.cs: " + exception.Message);
                this.ConnectionErrorLabel.Visibility = Visibility.Visible;
                this.connectionErrorLabelDisplayTimer = new DispatcherTimer(new TimeSpan(0, 0, 5), DispatcherPriority.Normal, this.ConnectionErrorLabelDisplayTimeout, Dispatcher.CurrentDispatcher);
            }
        }

        /// <summary>
        /// Connections the error label display timeout.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ConnectionErrorLabelDisplayTimeout(object sender, EventArgs e)
        {
            this.ConnectionErrorLabel.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Handles the MouseDown event of the BlackChipImage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void BlackChipImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TableManager tm = TableManager.Initialize();
                tm.GameSelectionWindow.Show();
                this.Hide();
            }
            catch (Exception exception)
            {
                Debug.WriteLine("ConnectionWindow.xaml.cs: " + exception.Message);
                this.ConnectionErrorLabel.Visibility = Visibility.Visible;
                this.connectionErrorLabelDisplayTimer = new DispatcherTimer(new TimeSpan(0, 0, 5), DispatcherPriority.Normal, this.ConnectionErrorLabelDisplayTimeout, Dispatcher.CurrentDispatcher);
            }
        }
    }
}