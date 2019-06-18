using System.Collections.Generic;

namespace SpicyInvader.Actors
{
    public class Actor
    {
        protected int _speed;

        public string Texture { get; private set; }
        public int YPos { get; set; }
        public int XPos { get; set; }
        public List<Actor> Children { get; set; }
        public bool IsRemoved { get; set; }
        public Actor Parent { get; set; }

        public Actor(string texture, int yPos, int xPos)
        {
            Texture = texture;
            XPos = xPos;
            YPos = yPos;
            Children = new List<Actor>();
            IsRemoved = false;
        }

        public virtual void Update()
        {
        }
    }
}
