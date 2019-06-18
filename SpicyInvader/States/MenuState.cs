using SpicyInvader.Models;
using System;
using System.Collections.Generic;

namespace SpicyInvader.States
{
    public class MenuState : State
    {
        private List<string> _textsMainMenu;
        private List<string> _textsOptionsMenu;
        private int _cursorPosition;
        private int _yPosFirstText;
        private int _currentMenu;

        private const int SPACE_BETWEEN_TEXT = 3;
        private const char SPACE = ' ';

        public MenuState(Game game)
            : base(game)
        {
            _cursorPosition = 0;
            _currentMenu = 0;
        }

        public override void LoadContent()
        {
            _textsMainMenu = new List<string>()
            {
                "Play",
                "HighScores",
                "Options",
                "About",
                "Quit"
            };

            _textsOptionsMenu = new List<string>()
            {
                "Sound",
                "Difficulty",
                "Back to Menu"
            };

            DisplayMainOrOptionsMenu(_textsMainMenu);
        }

        public override void Update()
        {
            switch (_currentMenu)
            {
                case 0:
                case 1:
                    UpdateMainOrOptionsMenu();
                    break;
                default:
                    UpdateAboutMenu();
                    break;
            }
        }

        private void UpdateMainOrOptionsMenu()
        {
            switch (Game.Key)
            {
                case SpicyKeys.Top:
                    if (_cursorPosition != 0)
                    {
                        _soundPlayerSwitch.Play(false);
                        _cursorPosition--;
                        DrawCursor(_cursorPosition, _cursorPosition + 1);
                    }
                    break;
                case SpicyKeys.Down:
                    if (_cursorPosition != (_currentMenu == 0 ? _textsMainMenu : _textsOptionsMenu).Count - 1)
                    {
                        _soundPlayerSwitch.Play(false);
                        _cursorPosition++;
                        DrawCursor(_cursorPosition, _cursorPosition - 1);
                    }
                    break;
                case SpicyKeys.Select:
                    if (_currentMenu == 0)
                    {
                        _soundPlayerSelect.Play(false);
                        SelectMainMenu();
                    } 
                    else {
                        _soundPlayerSelect.Play(false);
                        SelectOptionsMenu();
                    }
                    break;
            }
        }

        private void UpdateAboutMenu()
        {
            if (Game.Key == SpicyKeys.Select)
            {
                _soundPlayerSelect.Play(false);
                _currentMenu = 0; // back to main menu
                DisplayMainOrOptionsMenu(_textsMainMenu);
            }
        }

        public override void PostUpdate()
        {

        }

        private void SelectMainMenu()
        {
            switch (_cursorPosition)
            {
                case 0:
                    Console.Clear();
                    _game.ChangeState(new GameState(_game));
                    break;
                case 1:
                    Console.Clear();
                    _game.ChangeState(new ScoreState(_game));
                    break;
                case 2:
                    _currentMenu = 1;
                    DisplayMainOrOptionsMenu(_textsOptionsMenu);
                    break;
                case 3:
                    _currentMenu = 2;
                    DisplayAboutMenu();
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
            }
        }

        private void SelectOptionsMenu()
        {
            // Cursor position
            Console.SetCursorPosition(PAD_LEFT_CURSOR.Length + 2, _yPosFirstText + _cursorPosition * SPACE_BETWEEN_TEXT);

            switch (_cursorPosition)
            {
                case 0: // Sound
                    Game.Sound = Game.Sound ? false : true; 
                    Console.Write("Sound: " + (Game.Sound ? "ON " : "OFF"));
                    break;
                case 1: // Difficulty
                    Game.Difficulty = Game.Difficulty == 1 ? 2 : 1;
                    Console.Write("Difficulty: " + (Game.Difficulty == 1 ? "Easy" : "Hard"));
                    break;
                case 2: // Back to menu
                    _currentMenu = 0;
                    DisplayMainOrOptionsMenu(_textsMainMenu);
                    break;
            }
        }

        private void DrawCursor(int pos, int oldPos = -1)
        {
            // Delete the old cursor if there was one
            if (oldPos != -1)
            {
                Console.SetCursorPosition(PAD_LEFT_CURSOR.Length, _yPosFirstText + oldPos * SPACE_BETWEEN_TEXT);
                Console.Write(SPACE);
            }

            Console.SetCursorPosition(PAD_LEFT_CURSOR.Length, _yPosFirstText + pos * SPACE_BETWEEN_TEXT);
            Console.Write(CURSOR);
        }


        private void DisplayMainOrOptionsMenu(List<string> texts)
        {
            _cursorPosition = 0; // cursor at the top

            if (texts.Contains("Play"))
            {
                DisplayHeader("* Main Menu *");
            }
            else
            {
                DisplayHeader("* Options *");
            }

            for (int i = 0; i < texts.Count; i++)
            {
                if (i == 0)
                {
                    _yPosFirstText = Console.CursorTop;
                    DrawCursor(_cursorPosition);
                }
                Console.CursorLeft = PAD_LEFT_TEXT.Length;
                Console.WriteLine(texts[i] + "\n\n");
            }
        }

        private void DisplayAboutMenu()
        {
            DisplayHeader("* About *");

            DisplayCentered("Game made by Pierre Morand, Thomas Wenger and Valentin Kaelin");
            Console.CursorTop += 2;
            DisplayCentered("Controls", ConsoleColor.Yellow, true);
            Console.CursorTop += 1;
            DisplayCentered("Left: Go left");
            DisplayCentered("Right: Go right");
            DisplayCentered("Space: Shoot");
            DisplayCentered("Enter: Select in the menus");
            DisplayCentered("Escape: Go back to menu");

            Console.CursorTop += 5;
            DisplayCentered(CURSOR + " Go back to menu");
        }
    }
}
