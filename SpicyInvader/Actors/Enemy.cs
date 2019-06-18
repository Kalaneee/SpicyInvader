using SpicyInvader.Controllers;
using System.Diagnostics;

namespace SpicyInvader.Actors
{
    public class Enemy : Actor, ICollidable
    {
        private int _direction;
        private SoundController _soundPlayer;

        public int XPosSwarm { get; set; }
        public int YPosSwarm { get; private set; }
        public int XSizeSwarm { get; set; }

        public Enemy(string texture, int yPos, int xPos, int yPosSwarn, int xPosSwarm, int xSizeSwarm)
            : base(texture, yPos, xPos)
        {
            YPosSwarm = yPosSwarn;
            XPosSwarm = xPosSwarm;
            XSizeSwarm = xSizeSwarm;

            _direction = 1;
            _speed = 5;

            _soundPlayer = new SoundController("enemyHit.wav");
        }

        public override void Update()
        {
            if (Game.Ticks % (_speed - (Game.Difficulty - 1)) == 0) // Slow down Enemy
            {
                EnemyMove();
            }

            int shoot = Game.Random.Next(1, 1000); // 1 in 1000 chance of shooting
            if (shoot == 42)
            {
                FireBullet();
            }
        }

        private void EnemyMove()
        {
            XPos += 1 * _direction;
            int tmp = XPos;
            XPos = Utils.Clamp(XPos, 0 + XPosSwarm * (Texture.Length + 1), Game.GameWidth - Texture.Length - (XSizeSwarm - 1 - XPosSwarm) * (Texture.Length + 1)); // Maintiens l'Enemy dans la zone de jeu

            // One side has been reached
            if (tmp != XPos)
            {
                YPos++; // Go down by 1
                _direction = _direction == 1 ? -1 : 1; // Changement direction
            }
        }

        private void FireBullet()
        {
            Bullet shoot = new Bullet(".", YPos + 1, XPos + Texture.Length / 2, 1)
            {
                Parent = this
            };
            Children.Add(shoot);
        }

        public void OnCollide(Actor actor)
        {
            // Crash against Player
            if (actor is Player)
            {
                IsRemoved = true;
                ((Player)actor).Score.Value += Game.Difficulty;
            }

            // Hit by Bullet
            if (actor is Bullet && actor.Parent is Player)
            {
                _soundPlayer.Play(false);
                IsRemoved = true;
                ((Player)actor.Parent).Score.Value += Game.Difficulty;
            }
        }
    }
}
