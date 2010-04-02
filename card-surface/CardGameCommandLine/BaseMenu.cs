// <copyright file="BaseMenu.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The Base Menu provides common functions to the command line application.</summary>
namespace CardGameCommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The Base Menu provides common functions to the command line application.
    /// </summary>
    internal abstract class BaseMenu
    {
        /// <summary>
        /// Promps for enter before moving on.
        /// </summary>
        protected void PrompForEnter()
        {
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
    }
}
