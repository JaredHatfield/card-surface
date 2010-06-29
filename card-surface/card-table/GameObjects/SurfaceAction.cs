// <copyright file="SurfaceAction.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Interaction logic for SurfaceAction.xaml</summary>
namespace CardTable.GameObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;
    using CardGame;

    /// <summary>
    /// Surface Action
    /// </summary>
    public class SurfaceAction : ICommand
    {
        /// <summary>
        /// the usernama
        /// </summary>
        private string username;

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceAction"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        public SurfaceAction(string username)
        {
            this.username = username;
            this.CanExecuteChanged += new EventHandler(this.DoNothing);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Determines whether this instance can execute the specified o.
        /// </summary>
        /// <param name="o">The object here.</param>
        /// <returns>
        /// <c>true</c> if this instance can execute the specified o; otherwise, <c>false</c>.
        /// </returns>
        public bool CanExecute(object o)
        {
            if (o is string)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Executes the specified o.
        /// </summary>
        /// <param name="o">The object here.</param>
        public void Execute(object o)
        {
            TableManager.Instance().TableCommunicationController.SendCustomActionMessage((string)o, this.username);
        }

        /// <summary>
        /// Does the nothing.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        private void DoNothing(object source, object message)
        {
        }
    }
}
