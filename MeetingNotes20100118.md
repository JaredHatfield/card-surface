

# Introduction #

This wiki outlines the first brainstorming discussion for the card-surface Capstone Design project.  System components and architecture for a surface-based card game engine were discussed.  Potential requirements have been "highlighted" in red for easy reference.

Digital scan of meeting notes available: <a href='http://h202131.dreamsparkhosting.com/docs/Meeting%20Notes%20-%2020100118.pdf'>Meeting Notes - 20100118.pdf</a>

# Server Application #

Server Application tracks game state, provides a mobile web server, facilitates network communication with tables, broadcasts debug information to a text-based monitor, and saves user game data (i.e. registered users, user stats, etc.) to a file.

  * Server Components
    * Game State Manager - handles all game state activity via event dispatching; (table-to-game mapping, users-to-table mapping, users-to-mobile mapping); one "Game State" instance per game?  How many games can be handled before performance degredation?
    * Web Server - HTML communication with mobile devices
    * Communication\Network Manager - used to send update requests and retrieve data from table.  XML?
    * Disk Thread - periodically saves user sensitive data to file
    * Debugging Thread - manages debugging data; broadcasts to file or text-based terminal?

  * Game Server Libraries
    * CardGame (C# Library) - abstract; <font color='red'>Requirement:</font>**Game State Manager only accesses CardGame library; Table displays CardGame.
    * Blackjack (C# Library) - implements CardGame
    * Network?
    * Web?
  * Game Table Libraries - ?
  * Shared Libraries - ?**

<font color='red'>Requirement:</font>**Server Application runs as background process on server machine.**


# Table Application #

Table Application will be a "dummy" application that requires update communication with server for display.  Table downloads graphic information from the game server as part of game server library.  Each table may create a new game (from list of availble games provided by managing server) or join an existing game.

<font color='red'>Requirement:</font>**Users cannot join more than one table.**

  * Create A New Game
    * <font color='red'>Requirement:</font>**Limit 8 seats per game (Blackjack assumption?).  1 seat = dealer.  7 players max.
    ***<font color='red'>Requirement:</font>**Interface on table must be relative to user. (Cards face proper direction)
  * Join An Existing Game
    * Seats Available - Available seats around the table show a "Join Game" button.  Users may select the "Join Game" button and be given a randomly generated "seat code" that allows the user to connect their mobile device with their game seat.  Users that join a game are identified across all tables participating in a game (see [Working Assumptions](MeetingNotes20100118#Working_Assumptions.md)).  (ID user by link to Facebook picture?)  On joining table, players pick starting amount from bank.**<font color='red'>Requirement:</font>**Players are required to be physically present before joining a game.
    * Seats**Not**Available - Table "watches" game.  ESPN watch mode?  Spectate rooms?**

<font color='red'>Requirement:</font>**Table Application must know only the objects that belong to the particular table on which the Application is running.
  * How do we see a sync movement of an object on Table B with Table A?  Instant update?  Motion?  Final state differential?  Don't update until bet placed?
  * Changes across areas? (Ex. user area to public betting "pot")
  * No position tracking?
  * Prevent users from moving objects not belonging to players at that table.**

Position does not determine game state.

**Dealer** - Auto deal option or "Real Delaer".
  * Dealer drags cards to a user pile ("area") and "drops" card.
  * Moving circle highlights user who gets next card dealt.  Prevents dealer from misdealing cards.
  * Grid defined user area?

**Player** - Each player maintains a bank account outside of games.  "Game bank" starting amount determined by player on joining table.  During game, player pulls chips (by graphical selection) out of "game bank".  Chips not shown individually until pulled out of "game bank" for bet.  Disable chip selection if not enough cash available in game bank.

<font color='red'>Requirement:</font>**Table application will have easy installation process.**

# Mobile Application #

Mobile Application will connect to a web server provided by the [Server Application](MeetingNotes20100118#Server_Application.md) to show private game information for each user.
  * Interactivity?
    * Login (username, password, seat code)
    * Confirm betting amount?
    * Transfer money to table from account
  * Refresh method?
    * Constant rate
    * Reverse AJAX

Should the mobile application interact with betting to confirm bet amount placed on table?  Security issue?

  * Possible Usage
    * Player Banking Information
    * Cards
    * Chip Info
    * Game Summary
    * Tabbed website format?

**Not enough touch points for everyone to place bets simultaneously on the table.**

<font color='red'>Requirement:</font>**Mobile application will have easy installation process.**

# Working Assumptions #

  * Tables join Games.
    * Multiple tables may be participating in a game.
  * Users join Tables.
    * The number of users in a game may not exceed the maximum number of seats allowed for a game (8 including dealer).
    * Game seats may be occupied by users from different tables.
  * Server Installation Process will require some technical expertise.  Not simple like mobile or table application installations.