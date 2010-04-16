// <copyright file="TableManager.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The Singleton TableManager.</summary>
namespace CardTable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardCommunication;
    using CardGame.GameFactory;

    /// <summary>
    /// The Singleton TableManager.
    /// </summary>
    public class TableManager
    {
        /// <summary>
        /// The Singleton instance of TableManager.
        /// </summary>
        private static TableManager instance;

        /// <summary>
        /// The TableCommunicationController.
        /// </summary>
        private TableCommunicationController tableCommunicationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableManager"/> class.
        /// </summary>
        /// <param name="ip">The ip address to connect to.</param>
        private TableManager(string ip)
        {
            if (ip.Equals(string.Empty))
            {
                this.tableCommunicationController = new TableCommunicationController();
            }
            else
            {
                this.tableCommunicationController = new TableCommunicationController(ip);
            }

            // TODO: We need to create new chip and card factories that make surface elements!!!
            PhysicalObjectFactory.SubscribeCardFactory(CardFactory.Instance());
            PhysicalObjectFactory.SubscribeChipFactory(ChipFactory.Instance());
            PhysicalObjectFactory factory = PhysicalObjectFactory.Instance();
        }

        /// <summary>
        /// Gets the table communication controller.
        /// </summary>
        /// <value>The table communication controller.</value>
        internal TableCommunicationController TableCommunicationController
        {
            get { return this.tableCommunicationController; }
        }

        /// <summary>
        /// Initializes the specified ip.
        /// </summary>
        /// <param name="ip">The ip address to connect to.</param>
        /// <returns>The instance of TableManager.</returns>
        internal static TableManager Initialize(string ip)
        {
            if (TableManager.instance == null)
            {
                TableManager.instance = new TableManager(ip);
                return TableManager.instance;
            }

            throw new Exception("TableManager already Initialized!");
        }

        /// <summary>
        /// Instances this instance.
        /// </summary>
        /// <returns>The instance of TableManager.</returns>
        internal static TableManager Instance()
        {
            if (TableManager.instance == null)
            {
                throw new Exception("TableManager not Initialize!");
            }

            return TableManager.instance;
        }
    }
}
