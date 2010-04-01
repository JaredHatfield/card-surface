// <copyright file="ServerCommunicationController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The controller that the server uses for communication with the table.</summary>
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
    /// The controller that the server uses for communication with the table.
    /// </summary>
    public class ServerCommunicationController : CommunicationController
    {
        /// <summary>
        /// list of all clients that have communicated with the server.
        /// </summary>
        private IPEndPointCollection clientList = new IPEndPointCollection();

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerCommunicationController"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        public ServerCommunicationController(IGameController gameController)
            : base(gameController)
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
        /// Processes the client communication.
        /// </summary>
        /// <param name="asyncResult">The async result.</param>
        protected override void ProcessClientCommunication(IAsyncResult asyncResult)
        {
            try
            {
                Socket socketWorker = (Socket)asyncResult.AsyncState;
                Socket socketProcessor = socketWorker.EndAccept(asyncResult);
                bool found = false;

                foreach (IPEndPoint ep in this.clientList)
                {
                    if (socketProcessor.RemoteEndPoint == ep)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    this.clientList.Add((IPEndPoint)socketProcessor.RemoteEndPoint);
                }

                try
                {
                    CommunicationObject commObject = new CommunicationObject();
                    byte[] data = { };
                    
                    commObject.WorkSocket = socketProcessor;
                    commObject.Data = data;

                    ////socketProcessor.Receive(data, 0, CommunicationObject.BufferSize, SocketFlags.None);
                    ////this.ProcessCommunicationData(data);
                    ////this.ProcessCommunicationData(asyncResult);
                    socketProcessor.BeginReceive(
                        commObject.Buffer,
                        0,
                        CommunicationObject.BufferSize,
                        SocketFlags.None,
                        new AsyncCallback(this.ProcessCommunicationData),
                        commObject);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error Receiving Data from client", e);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Receiving Data from client", e);
            }
        }        
    }
}
