using SpicyInvader.Controllers;
using SpicyInvader.Models;
using System.Threading;
using System.Windows.Input;

namespace SpicyInvader.Actors
{
    public class Player : Actor, ICollidable
    {
        private int _timerShoot;
        private int _timerMove;

        private SoundController _soundPlayerFire;
        private SoundController _soundPlayerHit;
        private SoundController _soundPlayerDead;
        private Thread _threadKeyBoard;

        public int Health { get; set; }
        public Score Score { get; set; }
        public bool IsDead
        {
            get { return Health <= 0; }
        }

        public Player(string texture, int yPos, int xPos)
            : base(texture, yPos, xPos)
        {
            _timerShoot = 50; // To be able to shoot directly
            _timerMove = 2; // To be able to move directly
            Health = 5;

            _soundPlayerFire = new SoundController("playerFire.wav");
            _soundPlayerHit = new SoundController("playerHit.wav");
            _soundPlayerDead = new SoundController("playerDead.wav");

            // Thread listening to user inputs
            _threadKeyBoard = new Thread(MovePlayer)
            {
                Name = "ReadKeyboard"
            };
            _threadKeyBoard.SetApartmentState(ApartmentState.STA);
            _threadKeyBoard.Start();
        }

        private void MovePlayer()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                _timerShoot++;
                _timerMove++;

                if (_timerMove > 2)
                {
                    if (Keyboard.IsKeyDown(Key.Left))
                    {

                        _timerMove = 0;
                        XPos--;
                    }
                    else if (Keyboard.IsKeyDown(Key.Right))
                    {
                         _timerMove = 0;
                         XPos++;
                    }
                }
                if (Keyboard.IsKeyDown(Key.Space))
                {
                    if (_timerShoot > 50)
                    {
                        _timerShoot = 0;
                        FireBullet();
                    }
                }

                XPos = Utils.Clamp(XPos, 0, Game.GameWidth - Texture.Length);
                Thread.Sleep(10);
            }
        }

        private void FireBullet()
        {
            _soundPlayerFire.Play(false);

            Bullet shoot = new Bullet("♦", YPos - 1, XPos + Texture.Length / 2)
            {
                Parent = this
            };
            Children.Add(shoot);
        }

        public void OnCollide(Actor actor)
        {
            if (actor is Bullet)
            {
                Health--;
                _soundPlayerHit.Play(false);
            }

            if (actor is Enemy)
            {
                Health = 0;
            }

            if (Health == 0)
            {
                _soundPlayerDead.Play(false);
            }
        }
    }
}
