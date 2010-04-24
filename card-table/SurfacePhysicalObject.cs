// <copyright file="SurfacePhysicalObject.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The graphical representation of an object on the board.</summary>
namespace CardTable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The graphical representation of an object on the board.
    /// </summary>
    internal class SurfacePhysicalObject
    {
        /// <summary>
        /// The identifier.
        /// </summary>
        private Guid id;

        /// <summary>
        /// The path to the image.
        /// </summary>
        private string image;

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfacePhysicalObject"/> class.
        /// </summary>
        /// <param name="id">The id of the object.</param>
        /// <param name="image">The image to use.</param>
        internal SurfacePhysicalObject(Guid id, string image)
        {
            this.id = id;
            this.image = image;
        }

        /// <summary>
        /// Gets the object image source.
        /// </summary>
        /// <value>The object image source.</value>
        public string ObjectImageSource
        {
            get
            {
                return this.image;
            }
        }

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <value>The ID of the object.</value>
        public Guid ID
        {
            get { return this.id; }
        }
    }
}
