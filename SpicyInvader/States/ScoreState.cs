using SpicyInvader.Controllers;
using SpicyInvader.Models;
using System;

namespace SpicyInvader.States
{
    public class ScoreState : State
    {
        private ScoreController _scoreController;

        public ScoreState(Game game)
            : base(game)
        {
        }

        public override void LoadContent()
        {
            _scoreController = ScoreController.Load();

            DisplayHeader("* 10 HighScores *");

            // Display HighScores
            foreach (Score score in _scoreController.HighScores)
            {
                Console.WriteLine(PAD_LEFT_TEXT +  score.PlayerName + " : " + score.Value + "\n");
            }

            Console.CursorTop += 3;
            DisplayCentered(CURSOR + " Back to Menu");
        }

        public override void Update()
        {
            if(Game.Key == SpicyKeys.Select)
            {
                _soundPlayerSelect.Play(false);
                _game.ChangeState(new MenuState(_game));
            }
        }

        public override void PostUpdate()
        {
        }
    }
}
