using SpicyInvader.Models;
using SpicyInvader.States;
using System;
using System.Diagnostics;
using System.Threading;

namespace SpicyInvader
{
    public class Game
    {
        private State _currentState;
        private State _nextState;
        private Stopwatch _stopWatch;

        public static Random Random { get; private set; }
        public const int MARGIN_X = 5;
        public const int MARGIN_Y = 1;
        public static readonly int ScreenWidth = 100;
        public static readonly int ScreenHeight = 50;
        public static readonly int GameWidth = ScreenWidth - MARGIN_X * 2;
        public static readonly int GameHeight = ScreenHeight - MARGIN_Y * 2;
        public static int Ticks { get; private set; }
        public static SpicyKeys Key { get; private set; }
        public static bool Sound { get; set; }
        public static int Difficulty { get; set; }

        public Game()
        {
            Initialize();
        }

        private void Initialize()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            Random = new Random();

            Console.SetWindowSize(ScreenWidth, ScreenHeight);
            Console.SetBufferSize(ScreenWidth, ScreenHeight);

            Console.CursorVisible = false;

            _currentState = new MenuState(this);
            _currentState.LoadContent();

            _nextState = null;

            Ticks = 0;

            Key = SpicyKeys.Nothing;

            Sound = true;
            Difficulty = 1;

            _stopWatch = new Stopwatch();
        }

        public void Update()
        {
            while (true)
            {
                _stopWatch.Restart();

                // Actual Key
                Key = Input.GetKeyDown();

                if (_nextState != null)
                {
                    _currentState = _nextState;
                    _currentState.LoadContent();

                    _nextState = null;
                    Ticks = 1;
                }

                _currentState.Update();
                _currentState.PostUpdate();

                Ticks++;
                if (Ticks == int.MaxValue)
                {
                    Ticks = 0;
                }
                // Slow FPS
                int elapsed = (int)_stopWatch.ElapsedMilliseconds;
                elapsed = Utils.Clamp(elapsed, 0, 10);
                Thread.Sleep(10 - elapsed);
            }
        }

        public void ChangeState(State state)
        {
            _nextState = state;
        }
    }
}
