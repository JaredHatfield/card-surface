

# 1. Introduction #
> ## 1.1. Product Purpose ##
> > `1.1.1.` The product shall provide demonstrate a touch surface integrated with wireless mobile devices.<br />
> > `1.1.2.` This Software Requirements Specification document shall be intended for external customer approval.<br />

> ## 1.2. Product Scope ##
> > `1.2.1.` Card Surface shall provide an interactive touch based card game experience.<br />
> > `1.2.2.` Card Surface shall utilize a touch surface for game play.<br />
> > `1.2.3.` Card Surface shall allow mobile devices to connect to a game for viewing private information.<br />
> > `1.2.4.` Card Surface shall include a server for managing game communications and game state.<br />

> ## 1.3. Definitions ##
> > `1.3.1.` Client – the touch enabled application and hardware necessary for interacting with the game<br />
> > `1.3.2.` Server – the application that manages game play and user statistics<br />
> > `1.3.3.` Mobile Client – web based interface displayed on a portable device<br />
> > `1.3.4.` Seat Code – a randomly generated string that permits a user to join a table at a specific seat<br />

> ## 1.4. References ##
> > `1.4.1.` Communication Protocol – defines client\server communication<br />
> > `1.4.2.` XML User Data Schema – defines the data structure for the persistent user information


> ## 1.5. Overview ##
> This SRS contains all of the requirements needed to implement this system and its dependencies.  This document shall be formatted so that requirements are organized by system component.  Requirements listed below may necessitate additional debugging data outlined in an internal specification.
 
# 2. General Description #
The goal is to create an interactive touch based version of Blackjack that utilizes wireless mobile devices to display the user's face down card.  Any number of wireless devices could be used including: iPhone, iPod Touch, Droid, Palm Pre, BlackBerry, Zune HD, and other browser enabled portable devices.  The system should be able to support 7 players at a time.  Users will be able to log into the system with a username and password.  When playing a game, the interaction with the interface should be natural and mimic real life game play per technical limitations of the display interface.  Betting will be conducted in accordance with the standard game rules including a minimum bet and an optional maximum bet.
> ## 2.1. Product Context ##
> > `2.1.1.` The product shall be fully independent and self-contained.<br />
> > `2.1.2.` This product shall consist of two components: a server application and a client application.<br />
> > `2.1.3.` This product shall be licensed under version 3 of the GNU General Public License by the Free Software Foundation.<br />

> ## 2.2. Product Functions ##
> > `2.2.1.` This product shall, at a minimum, operate using the following three components.  Extensibility to these components as well as additional components not listed here will be allowed:<br />
> > > `2.2.1.1.` Client:  The client shall display the status of the game field, and send the server commands.<br />
> > > `2.2.1.2.` Server:  The server shall process all of the game logic and rules.<br />
> > > `2.2.1.3.` Mobile Client: The mobile client shall allow a user to manage their account settings and view private information about the game state.<br />

> > `2.2.2.` Users shall be able to connect to tables with their mobile devices.<br />
> > `2.2.3.` Tables shall be able to connect to the server.<br />
> > `2.2.4.` The product shall store user statistics, identifiers, and game history in an XML file on the server.<br />


> ## 2.3. User Characteristics ##
> > `2.3.1.` Card Surface shall expect users to be familiar with the rules of the game and standard betting procedure.<br />
> > `2.3.2.` A user's account shall include a profile picture.<br />
> > `2.3.3.` A user’s account shall include an account balance.<br />
> > `2.3.4.` A user’s game play history shall be tracked by the server.<br />
> > `2.3.5.` A user shall be able to manage their account from a mobile client.<br />
> > `2.3.6.` A user shall be identified by a unique username.<br />
> > `2.3.7.` A user shall authenticate with the server using a hashed password mapped to their username.<br />

> ## 2.4. Constraints ##
> > `2.4.1.` The Card Surface client application shall be constrained by the number of touch points supported by the screen.<br />
> > `2.4.2.` The Card Surface server application shall be running in order for the system to function.<br />
> > `2.4.3.` Mobile Clients shall include a browser in order to be compatible with Card Surface.<br />

> ## 2.5. Dependencies ##
> > `2.5.1.` This product shall require a TCP/IP network to function.<br />
> > `2.5.2.` ~~This product shall use XNA libraries for the client application.~~<br />
> > `2.5.3.` This product shall use the Microsoft Surface SDK.<br />
> > `2.5.4.` This product shall require Microsoft .NET 3.5.<br />
 
# 3. Specific Requirements #

> ## 3.1. External Interface Requirements ##
> > `3.1.1.` Card Surface shall require a web interface on the Mobile Client.<br />
> > `3.1.2.` Card Surface shall require a touch surface interface for the client application.<br />

> ## 3.2. Functional Requirements ##
> > `3.2.1.` Client Application<br />
> > > `3.2.1.1.` The client application shall receive the state of the game from the server.<br />
> > > `3.2.1.2.` The client application shall submit a new game state to the server for approval.<br />
> > > `3.2.1.3.` The client application shall allow the users to play any game for which the server supports game rules.<br />
> > > `3.2.1.4.` The client application shall require the user to login with a mobile client to the server before joining a game.<br />
> > > `3.2.1.5.` Users shall enter a randomly generated seat code using their mobile client upon login to join a table.<br />
> > > `3.2.1.6.` Seat codes shall only be valid for a finite period of time after being displayed on the table.<br />
> > > `3.2.1.7.` The client application shall allow a user to select the server-supported game to be played on the table.<br />
> > > `3.2.1.8.` The client application shall transmit a player’s proposed actions to the server for validation.<br />
> > > `3.2.1.9.` The client application shall support seven players and a dealer.<br />
> > > `3.2.1.10.` The client application shall contain default graphics needed for standard game play.<br />
> > > `3.2.1.11.` Based on Microsoft’s Surface SDK SP1 minimum requirements, the client application shall require Microsoft Windows Vista or newer to run.<br />

> > `3.2.2.` Server Application<br />
> > > `3.2.2.1.` The server application shall perform all game logic.<br />
> > > `3.2.2.2.` The server application shall contain all components required by a client to play a game (excluding mobile clients).<br />
> > > `3.2.2.3.` The server application shall authenticate login credentials for users initiating game play.<br />
> > > `3.2.2.4.` The server application shall reject a user with invalid login credentials and deny that user from participating in a game.<br />
> > > `3.2.2.5.` Login credentials shall consist of a username, password, and seat code.<br />
> > > `3.2.2.6.` The server application shall validate all proposed actions against the game state and game rules.<br />
> > > `3.2.2.7.` The server application shall provide all game graphics to be downloaded by the client application.<br />
> > > `3.2.2.8.` The server application shall provide a list of all available games to the client application.<br />
> > > `3.2.2.9.` The server application shall determine the game state.<br />
> > > `3.2.2.10.` The server application shall save user game history to disk.<br />
> > > `3.2.2.11.` The server application shall provide the mobile client with the most current game state.<br />
> > > `3.2.2.12.` The server application shall require Microsoft Windows XP or newer to run.<br />

> ## 3.3. Performance Requirements ##
> > `3.3.1.` Card Surface interaction shall be natural and mimic real life game play per technical limitations of the display interface.<br />
> > `3.3.2.` The server application shall respond to all protocol-defined communications from its authenticated client applications.<br />
> > `3.3.3.` The client application shall respond to all protocol-defined communications from its connected server application.<br />
> > `3.3.4.` The server application shall handle all protocol-defined communications in the order they were received from its authenticated client applications.<br />
> > `3.3.5.` The server application shall accommodate up to 32 simultaneous mobile devices.<br />
> > `3.3.6.` The server application shall accommodate up to 4 table instances.<br />

> ## 3.4. Design Constraints ##
> > `3.4.1.` The system shall be constrained by the hardware capabilities of the client table.<br />

> ## 3.5. Quality Requirements ##
> > `3.5.1.` The server application shall maintain a synchronized game state with each of its authenticated client applications.<br />
> > `3.5.2.` The server application shall remain connected to its client applications for the duration of the games.<br />
> > `3.5.3.` The server application and the client application shall ignore any messages not defined by the system protocol.<br />
> > `3.5.4.` Any errors reported by the client or server shall be in plain English.<br />
> > `3.5.5.` Any errors reported by the client or server shall be understood by the user.<br />

> ## 3.6. Other Requirements ##
> > `3.6.1.` Additional requirements may be added as time permits.<br />
 
# 4. Appendices #
[Sample Communication Schema](SampleCommunicationSchema.md)