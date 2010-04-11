// <copyright file="GameActionHit.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The hit action for blackjack.</summary>
namespace GameBlackjack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;
    using GameBlackjack.BlackjackExceptions;

    /// <summary>
    /// The hit action for blackjack.
    /// </summary>
    internal class GameActionHit : GameAction
    {
        /// <summary>
        /// Gets this actions name.
        /// </summary>
        /// <value>The actions name.</value>
        public override string Name
        {
            get
            {
                return "Hit";
            }
        }

        /// <summary>
        /// Perform the action on the specified game.
        /// </summary>
        /// <param name="game">The game to modify.</param>
        /// <param name="player">The player that triggered this action.</param>
        /// <returns>True if the action was successful; otherwise false.</returns>
        public override bool Action(Game game, string player)
        {
            this.PlayerCanExecuteAction(game.GetPlayer(player));
            Blackjack blackjack = (Blackjack)game;

            // Get the player and add a card to their hand
            Player p = blackjack.GetPlayer(player);
            PlayerState playerState = blackjack.GetPlayerState(p); // Will this throw an exception if player is not found?

            if (p != null)
            {
                CardPile deck = blackjack.GetPile(blackjack.DeckPile) as CardPile;

                // The player has cards and they are not finished
                if (playerState.IsDealt && !playerState.IsFinished)
                {
                    if (playerState.HasSplit)
                    {
                        // The player has split so things are a bit more complicated
                        if (!playerState.HasHandOneStand)
                        {
                            // Player is still working on first hand
                            p.PlayerArea.Cards[0].Open = true;
                            (deck.TopItem as ICard).Status = Card.CardStatus.FaceUp;
                            blackjack.MoveAction(deck.TopItem.Id, p.PlayerArea.Cards[0].Id);
                            if (BlackjackRules.GetPileVale(p.PlayerArea.Cards[0]) >= 21)
                            {
                                blackjack.ExecuteAction("Stand", player);
                            }

                            return true;
                        }
                        else if (!playerState.HasHandTwoStand)
                        {
                            // Player has moved on to the second hand
                            p.PlayerArea.Cards[1].Open = true;
                            (deck.TopItem as ICard).Status = Card.CardStatus.FaceUp;
                            blackjack.MoveAction(deck.TopItem.Id, p.PlayerArea.Cards[1].Id);
                            if (BlackjackRules.GetPileVale(p.PlayerArea.Cards[1]) >= 21)
                            {
                                blackjack.ExecuteAction("Stand", player);
                            }

                            return true;
                        }
                        else
                        {
                            throw new BlackjackException("Hit not allowed.");
                        }
                    }
                    else
                    {
                        // The player has not split so things are easy
                        if (!playerState.HasHandOneStand)
                        {
                            p.Hand.Open = true;
                            (deck.TopItem as ICard).Status = Card.CardStatus.FaceUp;
                            blackjack.MoveAction(deck.TopItem.Id, p.Hand.Id);
                            if (BlackjackRules.GetPileVale(p.Hand) >= 21)
                            {
                                blackjack.ExecuteAction("Stand", player);
                            }

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is executable by player] [the specified game].
        /// </summary>
        /// <param name="game">The game to check.</param>
        /// <param name="player">The player to modify.</param>
        /// <returns>
        /// <c>true</c> if [is executable by player] [the specified game]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsExecutableByPlayer(Game game, Player player)
        {
            Blackjack blackjack = game as Blackjack;
            PlayerState playerState = blackjack.GetPlayerState(player);

            // It is the players turn, they have cards, and they are not finished
            if (player.IsTurn && playerState.IsDealt && !playerState.IsFinished)
            {
                if (playerState.HasSplit)
                {
                    // The player has split so things are a bit more complicated
                    if (!playerState.HasHandOneStand)
                    {
                        // Player is still working on first hand
                        return true;
                    }
                    else if (!playerState.HasHandTwoStand)
                    {
                        // Player has moved on to the second hand
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // The player has not split so things are easy
                    if (!playerState.HasHandOneStand)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }
}
