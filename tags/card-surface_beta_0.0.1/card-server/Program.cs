// <copyright file="Program.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Main application.</summary>
namespace CardServer
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using CardAccount;

    /// <summary>
    /// Main application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main application.
        /// </summary>
        /// <param name="args">The args for the application.</param>
        private static void Main(string[] args)
        {
            string nextCmd = String.Empty;

            // TODO: Implement CardServer.
            ServerController serverController = new ServerController();
            GameController gameController = GameController.Instance();
            System.Diagnostics.Process.GetCurrentProcess().Exited += new EventHandler(serverController.Close);
            Console.WriteLine();
            Console.WriteLine("Server IP: " + serverController.ServerCommunicationController.IP);
            Console.WriteLine();
            ReadOnlyCollection<string> s = gameController.GameTypes;
            Console.WriteLine("Available Game Types on this Server:");
            for (int i = 0; i < s.Count; i++)
            {
                Console.WriteLine(s[i]);
            }

            Console.WriteLine();

            do
            {
                string[] cmdTokens = nextCmd.Split(new char[] { ' ' });
                if (cmdTokens[0].Equals("createaccount", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (cmdTokens[1].Equals(String.Empty))
                    {
                        Console.WriteLine("Invalid username!");
                    }
                    else if (cmdTokens[2].Equals(String.Empty))
                    {
                        Console.WriteLine("Invalid password!");
                    }
                    else
                    {
                        if (AccountController.Instance.CreateAccount(cmdTokens[1], cmdTokens[2]))
                        {
                            Console.WriteLine("Account created successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Account creation failed!");
                        }
                    }
                }
                else if (cmdTokens[0].Equals("deposit", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (cmdTokens[1].Equals(String.Empty))
                    {
                        Console.WriteLine("Invalid username!");
                    }
                    else if (cmdTokens[2].Equals(String.Empty))
                    {
                        Console.WriteLine("Invalid deposit amount!");
                    }
                    else
                    {
                        try
                        {
                            int depositAmount = int.Parse(cmdTokens[2]);

                            if (!AccountController.Instance.Deposit(cmdTokens[1], depositAmount))
                            {
                                if (depositAmount < 0)
                                {
                                    Console.WriteLine("Deposit failed!  Deposits must be made in positive amounts!");
                                }
                                else
                                {
                                    Console.WriteLine("Deposit failed!");
                                }
                            }
                        }
                        catch (FormatException fe)
                        {
                            Console.WriteLine("Invalid deposit amount! (" + fe.Message + ")");
                        }
                    }
                }
                else if (cmdTokens[0].Equals("withdraw", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (cmdTokens[1].Equals(String.Empty))
                    {
                        Console.WriteLine("Invalid username!");
                    }
                    else if (cmdTokens[2].Equals(String.Empty))
                    {
                        Console.WriteLine("Invalid deposit amount!");
                    }
                    else
                    {
                        try
                        {
                            int withdrawalAmount = int.Parse(cmdTokens[2]);

                            if (!AccountController.Instance.Withdraw(cmdTokens[1], withdrawalAmount))
                            {
                                if (withdrawalAmount < 0)
                                {
                                    Console.WriteLine("Withdrawal failed!  Withdrawals must be made in positive amounts!");
                                }
                                else
                                {
                                    Console.WriteLine("Withdrawal failed!");
                                }
                            }
                        }
                        catch (FormatException fe)
                        {
                            Console.WriteLine("Invalid withdrawal amount! (" + fe.Message + ")");
                        }
                    }
                }
                else if (cmdTokens[0].Equals("listusers", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("Username\tBalance");

                    foreach (GameAccount account in AccountController.Instance.Accounts)
                    {
                        Console.WriteLine(account.Username + "\t\t" + account.Balance);
                    }
                }
                else if (cmdTokens[0].Equals("listgames", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (CardGame.Game game in gameController.Games)
                    {
                        Console.WriteLine(game.GetType() + "\t\t(Players: " + game.NumberOfPlayers + "/" + game.NumberOfSeats + ")");
                    }
                }
                else if (cmdTokens[0].Equals("help", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine(" createaccount <name> <password> - Creates a new account with the specified name and password");
                    Console.WriteLine(" deposit <name> <amount> - Deposits money in this user's account");
                    Console.WriteLine(" withdraw <name> <amount> - Withdraw money from this user's account");
                    Console.WriteLine(" listusers - Lists all the users with an account on this server");
                    Console.WriteLine(" listgames - Lists all the games currently being managed by this server");
                }

                Console.Write("CardServer> ");
            } 
            while (!(nextCmd = Console.ReadLine()).Equals("exit", StringComparison.CurrentCultureIgnoreCase));

            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }
    }
}
