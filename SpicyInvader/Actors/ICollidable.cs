namespace SpicyInvader.Actors
{
    /// <summary>
    ///  All Actors implementing this interface can collide with each other
    /// </summary>
    public interface ICollidable
    {
        void OnCollide(Actor actor);
    }
}
