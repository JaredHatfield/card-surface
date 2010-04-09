// <copyright file="SurfaceCard.xaml.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Interaction logic for SurfaceCard.xaml</summary>
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
    using CardGame;
    using Microsoft.Surface;
    using Microsoft.Surface.Presentation;
    using Microsoft.Surface.Presentation.Controls;

    /// <summary>
    /// Interaction logic for SurfaceCard.xaml
    /// </summary>
    public partial class SurfaceCard : SurfaceUserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceCard"/> class.
        /// </summary>
        public SurfaceCard()
        {
            InitializeComponent();
        }
    }
}
