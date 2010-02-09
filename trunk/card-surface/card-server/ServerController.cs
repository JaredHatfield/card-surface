using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardWeb;
using CardCommunication;
using CardAccount;

namespace CardServer
{
    public class ServerController
    {
        private GameController gameController;
        private WebController webController;
        private ServerCommunicationController serverCommunicationController;
        private AccountController accountController;
    }
}
