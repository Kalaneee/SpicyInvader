namespace SpicyInvader.Models
{
    public class Score
    {
        public string PlayerName { get; set; }
        public int Value { get; set; }

        public Score()
        {

        }

        public Score(string playerName, int value)
        {
            PlayerName = playerName;
            Value = value;
        }
    }
}
