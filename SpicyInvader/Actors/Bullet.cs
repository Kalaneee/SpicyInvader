namespace SpicyInvader.Actors
{
    public class Bullet : Actor, ICollidable
    {
        private int _direction;

        public Bullet(string texture, int yPos, int xPos, int direction)
            : base(texture, yPos, xPos)
        {
            _direction = direction;
            _speed = 4;
        }

        public Bullet(string texture, int yPos, int xPos)
            : this(texture, yPos, xPos, -1) // -1 : go down
        {
            _speed = 4;
        }

        public override void Update()
        {
            if (Game.Ticks % _speed == 0) // Slow down Bullets
            {
                BulletMove();
            }
        }

        private void BulletMove()
        {
            if (YPos > 0 && YPos < Game.GameHeight - 1)
            {
                YPos += 1 * _direction;
            }
            else
            {
                IsRemoved = true;
            }
        }

        public void OnCollide(Actor actor)
        {
            // Bullets do not collide with each other
            if (actor is Bullet)
                return;

            // Enemies can't shoot each other
            if (actor.Parent is Enemy && this.Parent is Enemy)
                return;

            // A Player shoots an Enemy or an Enemy shoots a Player. Or a boss is hit.
            if ((actor is Enemy && this.Parent is Player) ||
                actor is Player && this.Parent is Enemy ||
                actor is Boss)
            {
                IsRemoved = true;
            }
        }
    }
}
