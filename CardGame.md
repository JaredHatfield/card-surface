# Game #
An instance of a game is used as the underlying logic to implement a specific card game.

A Game has an array of seats.  These seats can be empty or occupied and are tied to a specific location on the board.

# Seat #
A Seat is a position at the table specified by a SeatLocation.

It contains a Player, a seat password, a username, and a unique id.

The Player is an instance of the Player class for the individual sitting in this seat.

The seat password is used when joining the table and associates this seat with an individual username.  The username is used when accessing the game to properly link the individual accessing the game with their seat.

The unique id is used to distinguish this object from other seats.

# Player #
The participant in the game that contains all of the PhysicalObjects associated with the Player.  This includes the player's hand and the player's playing area.

The Player also has associated actions they can perform as part of the game.

# Playing Area #
A PlayingArea is the area on the board that belongs to a specific player.  This includes an array of CardPile's and an array of ChipPile's.  These are objects that are on the table.

There is also a PlayingArea associated with the game that is shared among all of the Players.

# Pile #
A pile is a collection of PhysicalObjects, either Chips or Cards.

## CardPile ##
A CardPile is an extension of a Pile that is used specifically for Cards.

## ChipPile ##
A ChipPile is an extension of a Pile that is used specifically for Chips.

# PhysicalObject #
A PhysicalObject is the abstract class that is used for dealing with objects on the game board.

## Chip ##
A Chip is an implementation of a PhysicalObject that includes a color and a value.

## Card ##
A Card is an implementation of a PhysicalObject that includes a face, a suit, and a status.  A card is also associated with an image that is displayed to the user.