using SpicyInvader.Controllers;
using SpicyInvader.Models;
using System;

namespace SpicyInvader.States
{
    public class EndGameState : State
    {
        private Score _score;
        private ScoreController _scoreController;

        public EndGameState(Game game, Score score)
            : base(game)
        {
            _score = score;
        }

        public override void LoadContent()
        {
            _scoreController = ScoreController.Load();

            DisplayHeader("* End Game *");

            AskUsername();

            Console.CursorTop += 4;
            DisplayCentered("Your score was: " + _score.Value);
            Console.CursorTop += 10;
            DisplayCentered(CURSOR + " Go back to Menu");
        }

        public override void Update()
        {
            if (Game.Key == SpicyKeys.Menu || Game.Key == SpicyKeys.Select)
            {
                _soundPlayerSelect.Play(false);
                _game.ChangeState(new MenuState(_game));
            }
        }

        public override void PostUpdate()
        {

        }

        private void AskUsername()
        {
            string username = "";
            while (username.Length > 15 || username.Length == 0)
            {
                Console.CursorTop--;
                DisplayCentered(CURSOR + " Enter your name to save the score (max 15 char.): ", false);
                username = Console.ReadLine();
            }
            _score.PlayerName = username;

            _scoreController.Add(_score);
            ScoreController.Save(_scoreController);
        }
    }
}
