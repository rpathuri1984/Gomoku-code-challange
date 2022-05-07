using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Gomoku.Logic.Tests
{
    public class Tests
    {
        private Game? GomokuGame;
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
            GomokuGame = new Game(15, 15, players);
            PlayResult isGaveOver = null;

            // act
            GomokuGame.Play(0, 1);
            isGaveOver = GomokuGame.Play(1, 1);
            Assert.IsFalse(isGaveOver.IsGameOver);

            GomokuGame.Play(0, 2);
            isGaveOver = GomokuGame.Play(1, 2);
            Assert.IsFalse(isGaveOver.IsGameOver);

            GomokuGame.Play(0, 3);
            isGaveOver = GomokuGame.Play(1, 3);
            Assert.IsFalse(isGaveOver.IsGameOver);

            GomokuGame.Play(0, 4);
            isGaveOver = GomokuGame.Play(1, 4);
            Assert.IsFalse(isGaveOver.IsGameOver);

            isGaveOver = GomokuGame.Play(0, 5);
            Assert.IsTrue(isGaveOver.IsGameOver);
        }


        [Test]
        public void Play_should_return_Player1_should_wins()
        {
            // arrange
            GomokuGame = new Game(15, 15, players);

            // act
            GomokuGame.Play(4, 4);
            GomokuGame.Play(5, 3);

            GomokuGame.Play(3, 5);
            GomokuGame.Play(4, 6);

            GomokuGame.Play(2, 6);
            GomokuGame.Play(3, 6);

            GomokuGame.Play(1, 7);
            GomokuGame.Play(6, 8);

            PlayResult isGaveOver = GomokuGame.Play(0, 8);
            
            // assert
            Assert.IsTrue(isGaveOver.IsGameOver);
            Assert.AreEqual(5, isGaveOver?.WinningLine?.Count());
        }

        [Test]
        public void Play_should_return_Player2_should_wins()
        {
            // arrange
            GomokuGame = new Game(15, 15, players);

            // act
            GomokuGame.Play(6, 5);
            GomokuGame.Play(6, 3);

            GomokuGame.Play(7, 8);
            GomokuGame.Play(7, 4);

            GomokuGame.Play(8, 6);
            GomokuGame.Play(9, 6);

            GomokuGame.Play(7, 6);
            GomokuGame.Play(10, 7);

            GomokuGame.Play(5, 4);
            PlayResult isGaveOver = GomokuGame.Play(8, 5);

            // assert
            Assert.IsTrue(isGaveOver.IsGameOver);
            Assert.AreEqual(5, isGaveOver?.WinningLine?.Count());
        }
    }
}