using SpicyInvader.Actors;
using SpicyInvader.Controllers;
using SpicyInvader.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SpicyInvader.States
{
    public class GameState : State
    {
        private char[][] _content = new char[Game.GameHeight][];
        private List<Actor> _actors;
        private Player _player;
        private int _playerHealth;
        private int _nbEnemies;
        private int _bossDirection; // (1: go right / -1: go left)
        private EnemyController _enemyController;
        private string _scoreText;

        public GameState(Game game)
            : base(game)
        {

        }

        public override void LoadContent()
        {
            _nbEnemies = Game.Difficulty == 1 ? 7 : 9;
            _playerHealth = Game.Difficulty == 1 ? 5 : 3;
            _bossDirection = 1;

            _player = new Player("º¤º", Game.GameHeight - 4, Game.GameWidth / 2 - 2)
            {
                Health = _playerHealth,
                Score = new Score("User not saved", 0),
            };

            _actors = new List<Actor>()
            {
                _player,
            };

            _enemyController = new EnemyController("■■■", _nbEnemies, _nbEnemies, 0, 8);
            _enemyController.CreateEnemies(ref _actors);
        }

        public override void Update()
        {
            // Stop game
            if(Game.Key == SpicyKeys.Menu)
            {
                _game.ChangeState(new EndGameState(_game, _player.Score));
            }

            // Change color of the game
            if (Game.Key == SpicyKeys.EasterEgg)
            {
                int color = Game.Random.Next(10, 16);
                Console.ForegroundColor = (ConsoleColor)color;
            }

            // Boss appears
            if (Game.Ticks % (2000 / Game.Difficulty)  == 0)
            {
                int xPosBoss = _bossDirection == 1 ? 0 : Game.GameWidth - 4;
                _actors.Add(new Boss("<-O->", 5, xPosBoss, _bossDirection));
                _bossDirection = _bossDirection == 1 ? -1 : 1;
            }

            // Reset content array
            for (int i = 0; i < _content.Length; i++)
            {
                _content[i] = "".PadLeft(Game.ScreenWidth - 1).ToCharArray();
            }

            // Fill in the content array
            foreach (Actor a in _actors)
            {
                for (int i = 0; i < a.Texture.Length; i++)
                {
                    _content[a.YPos][a.XPos + i + Game.MARGIN_X] = a.Texture[i];
                }
            }

            // Display player's score
            _scoreText = "Score: " + _player.Score.Value;
            for (int i = 0; i < _scoreText.Length; i++)
            {
                _content[0][i + 5] = _scoreText[i];
            }

            // Display player's life
            _content[0][Game.GameWidth - 1] = '♥';
            for (int i = 0; i < _player.Health.ToString().Length; i++)
            {
                _content[0][Game.GameWidth - 3 + i] = _player.Health.ToString()[i];
            }

            // Display all content
            Console.CursorTop = Game.MARGIN_Y;
            string allContent = "";
            for (int i = 0; i < _content.Length; i++)
            {
                allContent += new string(_content[i]) + " ";
            }
            Console.Write(allContent);

            // Update all actors
            foreach (Actor a in _actors)
            {
                a.Update();
            }

            _enemyController.Update();
        }

        public override void PostUpdate()
        {
            // Collisions
            List<Actor> collidableActors = _actors.Where(c => c is ICollidable).ToList();
            foreach (Actor actorA in collidableActors)
            {
                foreach (Actor actorB in collidableActors)
                {
                    if (actorA == actorB)
                        continue;

                    if (Utils.Intersects(actorA, actorB))
                    {
                        ((ICollidable)actorA).OnCollide(actorB);
                    }
                }
            }

            // Add children to the list of actors
            for (int i = 0; i < _actors.Count; i++)
            {
                Actor a = _actors[i];
                foreach (Actor c in a.Children)
                {
                    _actors.Add(c);
                }
                a.Children = new List<Actor>();
            }

            // Delete all actors with isRemoved = true
            for (int i = 0; i < _actors.Count; i++)
            {
                if (_actors[i].IsRemoved)
                {
                    _actors.RemoveAt(i);
                    i--;
                }
            }

            // All enemies are dead: new wave
            if (!_actors.OfType<Enemy>().Any())
            {
                _enemyController.CreateEnemies(ref _actors);
                _player.Health++;
            }

            // Player is dead
            if(_player.IsDead)
            {
                Boss boss = _actors.OfType<Boss>().FirstOrDefault();
                if (boss != null)
                {
                    boss.StopSound();
                }

                _game.ChangeState(new EndGameState(_game, _player.Score));
            }
        }
    }
}
