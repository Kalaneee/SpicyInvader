using SpicyInvader.Actors;

namespace SpicyInvader
{
    public static class Utils
    {
        private static bool ValueInRange(int value, int min, int max)
        {
            return (value >= min) && (value <= max);
        }

        public static bool Intersects(Actor actorA, Actor actorB)
        {
            if (actorA.YPos != actorB.YPos)
                return false;

            bool xOverlap = ValueInRange(actorA.XPos, actorB.XPos, actorB.XPos + actorB.Texture.Length - 1) ||
                            ValueInRange(actorB.XPos, actorA.XPos, actorA.XPos + actorA.Texture.Length - 1);

            return xOverlap;
        }

        public static int Clamp(int value, int min, int max)
        {
            value = (value > max) ? max : value;

            value = (value < min) ? min : value;

            return value;
        }
    }
}
