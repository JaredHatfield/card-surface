// <copyright file="ParameterStruct.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A struct for an active game.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A struct for a parameter of a messaage.
    /// </summary>
    public struct ParameterStruct
    {
        /// <summary>
        /// The name of the parameter
        /// </summary>
        public string Name;

        /////// <summary>
        /////// The type of the parameter
        /////// </summary>
        ////public object type;

        /// <summary>
        /// The value of the parameter
        /// </summary>
        public string Value;
    }
}
