using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Gomoku.Logic.Tests
{
    public class Tests
    {
        private GomokuGame? GomokuGame;
        private IList<Player> players;

        [SetUp]
        public void Setup()
        {
            players = new List<Player>()
                        {
                          new Player("Player 1", new Stone(Stones.X)),
                          new Player("Player 2", new Stone(Stones.O)),
                        };
        }

        [Test]
        public void Play_should_return_success()
        {
            // arrange
            GomokuGame = new GomokuGame(15, 15, players);
            PlayResult result = null;

            // act
            GomokuGame.PlaceStone(0, 1);
            result = GomokuGame.PlaceStone(1, 1);
            Assert.IsFalse(result.IsGameOver);

            GomokuGame.PlaceStone(0, 2);
            result = GomokuGame.PlaceStone(1, 2);
            Assert.IsFalse(result.IsGameOver);

            GomokuGame.PlaceStone(0, 3);
            result = GomokuGame.PlaceStone(1, 3);
            Assert.IsFalse(result.IsGameOver);

            GomokuGame.PlaceStone(0, 4);
            result = GomokuGame.PlaceStone(1, 4);
            Assert.IsFalse(result.IsGameOver);

            result = GomokuGame.PlaceStone(0, 5);
            Assert.IsTrue(result.IsGameOver);
        }


        [Test]
        public void Play_should_return_Player1_wins()
        {
            // arrange
            GomokuGame = new GomokuGame(15, 15, players);

            // act
            GomokuGame.PlaceStone(4, 4);
            GomokuGame.PlaceStone(5, 3);

            GomokuGame.PlaceStone(3, 5);
            GomokuGame.PlaceStone(4, 6);

            GomokuGame.PlaceStone(2, 6);
            GomokuGame.PlaceStone(3, 6);

            GomokuGame.PlaceStone(1, 7);
            GomokuGame.PlaceStone(6, 8);

            PlayResult result = GomokuGame.PlaceStone(0, 8);
            
            // assert
            Assert.IsTrue(result.IsGameOver);
            Assert.AreEqual($"Game is over. {players.First().Name} wins.", result.Message);
        }

        [Test]
        public void Play_should_return_Player2_wins()
        {
            // arrange
            GomokuGame = new GomokuGame(15, 15, players);

            // act
            GomokuGame.PlaceStone(6, 5);
            GomokuGame.PlaceStone(6, 3);

            GomokuGame.PlaceStone(7, 8);
            GomokuGame.PlaceStone(7, 4);

            GomokuGame.PlaceStone(8, 6);
            GomokuGame.PlaceStone(9, 6);

            GomokuGame.PlaceStone(7, 6);
            GomokuGame.PlaceStone(10, 7);

            GomokuGame.PlaceStone(5, 4);
            PlayResult result = GomokuGame.PlaceStone(8, 5);

            // assert
            Assert.IsTrue(result.IsGameOver);
            Assert.AreEqual($"Game is over. {players.Last().Name} wins.", result.Message);
            Assert.IsNotNull(result.ToString());
        }
    }
}