using SpicyInvader.Controllers;

namespace SpicyInvader.Actors
{
    public class Boss : Actor, ICollidable
    {
        private int _direction;

        private SoundController _soundPlayerMove;
        private SoundController _soundPlayerDead;

        public Boss(string texture, int yPos, int xPos, int direction)
            : base(texture, yPos, xPos)
        {
            _direction = direction;
            _speed = 7;

            _soundPlayerMove = new SoundController("bossMove.wav");
            _soundPlayerDead = new SoundController("bossDead.wav");

            _soundPlayerMove.Play(true);
        }

        public override void Update()
        {
            if (Game.Ticks % (_speed - (Game.Difficulty - 1)) == 0) // Slow down Boss
            {
                BossMove();
            }

            // Remove Boss Sound if we go back to menu
            if (Game.Key == Models.SpicyKeys.Menu)
            {
                _soundPlayerMove.StopPlaying();
            }
        }

        private void BossMove()
        {
            XPos += 1 * _direction;

            // If the boss touches one side of the game, it is removed
            if (XPos < 0 || XPos > Game.GameWidth - Texture.Length)
            {
                _soundPlayerMove.StopPlaying();
                IsRemoved = true;
            }
        }

        public void StopSound()
        {
            _soundPlayerMove.StopPlaying();
        }

        public void OnCollide(Actor actor)
        {
            // Hit by Bullet
            if (actor is Bullet && actor.Parent is Player)
            {
                _soundPlayerMove.StopPlaying();
                _soundPlayerDead.Play(false);

                IsRemoved = true;
                ((Player)actor.Parent).Score.Value += 10 * Game.Difficulty;
            }
        }
    }
}
