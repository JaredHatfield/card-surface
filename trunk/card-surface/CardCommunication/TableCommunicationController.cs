// <copyright file="TableCommunicationController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The controller that the table uses for communication with the server.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Xml;
    using CardGame;
    using Messages;

    /// <summary>
    /// The controller that the table uses for communication with the server.
    /// </summary>
    public class TableCommunicationController : CommunicationController
    {
        /// <summary>
        /// IPAddress of the server
        /// </summary>
        private IPAddress serverIPAddress = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableCommunicationController"/> class.
        /// </summary>
        public TableCommunicationController()
            : base()
        {
        }

        /////// <summary>
        /////// Sends the state of the game.
        /////// </summary>
        /////// <param name="game">The game.</param>
        /////// <returns></returns>
        ////public bool SendMessage(Game game)
        ////{
        ////    bool success = true;

        ////    try
        ////    {
        ////        success = this.SendMessage(game, Message.MessageType.GameState);
        ////    }
        ////    catch (Exception e)
        ////    {
        ////        Console.WriteLine("Error sending message", e);
        ////        success = false;
        ////    }

        ////    return success;
        ////}

        /// <summary>
        /// Sends the action message.
        /// </summary>
        /// <param name="game">The game to be sent.</param>
        /// <returns>whether the Action Message was sent.</returns>
        public bool SendActionMessage(Game game)
        {
            bool success = true;

            try
            {
                MessageAction message = new MessageAction();

                success = message.BuildMessage(game);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error sending message", e);
                success = false;
            }

            return success;
        }

        ////public bool ProcessMessage()
        ////{
        ////    bool success = true;

        ////    try
        ////    {
        ////        MessageAction message = new MessageAction();
        ////        GameMessage game = new GameMessage();

        ////        success = message.ProcessMessage(xmldo);
        ////    }
        ////    catch (Exception e)
        ////    {
        ////        Console.WriteLine("Error sending message", e);
        ////        success = false;
        ////    }

        ////    return success;
        ////}

        /// <summary>
        /// Processes the client communication.
        /// </summary>
        /// <param name="asyncResult">The async result.</param>
        protected override void ProcessClientCommunication(IAsyncResult asyncResult)
        {
            try
            {
                Socket socketProcessor = this.SocketListener.EndAccept(asyncResult);
                byte[] data = { 0 };

                socketProcessor.BeginReceive(
                    data,
                    0,
                    data.Length,
                    SocketFlags.None,
                    new AsyncCallback(this.ProcessCommunicationData),
                    data);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Receiving Data from client", e);
            }
        }
    }
}
