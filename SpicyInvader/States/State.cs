using SpicyInvader.Controllers;
using System;

namespace SpicyInvader.States
{
    public abstract class State
    {
        protected Game _game;

        private readonly string PAD_LEFT_TITLE = "".PadLeft(17);
        protected readonly string PAD_LEFT_TEXT = "".PadLeft(40);
        protected readonly string PAD_LEFT_CURSOR = "".PadLeft(38);
        protected const char CURSOR = '►';
        protected SoundController _soundPlayerSwitch;
        protected SoundController _soundPlayerSelect;

        public State(Game game)
        {
            _game = game;

            _soundPlayerSwitch = new SoundController("switchMenu.wav");
            _soundPlayerSelect = new SoundController("selectMenu.wav");
        }

        public abstract void LoadContent();

        public abstract void Update();

        public abstract void PostUpdate();

        protected void DisplayTitle()
        {
            Console.WriteLine("\n\n\n\n" + PAD_LEFT_TITLE + "   _____       _            _____                     _           ");
            Console.WriteLine(PAD_LEFT_TITLE + "  / ____|     (_)          |_   _|                   | |          ");
            Console.WriteLine(PAD_LEFT_TITLE + " | (___  _ __  _  ___ _   _  | |  _ ____   ____ _  __| | ___ _ __ ");
            Console.WriteLine(PAD_LEFT_TITLE + "  \\___ \\| '_ \\| |/ __| | | | | | | '_ \\ \\ / / _` |/ _` |/ _ \\ '__|");
            Console.WriteLine(PAD_LEFT_TITLE + "  ____) | |_) | | (__| |_| |_| |_| | | \\ V / (_| | (_| |  __/ |   ");
            Console.WriteLine(PAD_LEFT_TITLE + " |_____/| .__/|_|\\___|\\__, |_____|_| |_|\\_/ \\__,_|\\__,_|\\___|_|   ");
            Console.WriteLine(PAD_LEFT_TITLE + "        | |            __/ |                                      ");
            Console.WriteLine(PAD_LEFT_TITLE + "        |_|           |___/                                       \n\n\n\n");
        }

        protected void DisplayHeader(string title)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            DisplayTitle();

            DisplayCentered(title, ConsoleColor.Red);
            Console.CursorTop += 3;
        }

        protected void DisplayCentered(string text, bool newLine = true)
        {
            Console.CursorLeft = Game.ScreenWidth / 2 - text.Length / 2;

            if (newLine)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.Write(text);
            }
        }

        protected void DisplayCentered(string text, ConsoleColor color, bool newLine = true)
        {
            Console.ForegroundColor = color;
            DisplayCentered(text, newLine);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
