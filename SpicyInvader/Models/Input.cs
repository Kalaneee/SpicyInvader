using System;

namespace SpicyInvader.Models
{
    public enum SpicyKeys
    {
        Left,
        Right,
        Top,
        Down,
        Shoot,
        Menu,
        Select,
        EasterEgg,
        Nothing
    }

    public static class Input
    {
        public static SpicyKeys GetKeyDown()
        {
            if (Console.KeyAvailable)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.LeftArrow:
                        return SpicyKeys.Left;
                    case ConsoleKey.RightArrow:
                        return SpicyKeys.Right;
                    case ConsoleKey.UpArrow:
                        return SpicyKeys.Top;
                    case ConsoleKey.DownArrow:
                        return SpicyKeys.Down;
                    case ConsoleKey.Spacebar:
                        return SpicyKeys.Shoot;
                    case ConsoleKey.Escape:
                        return SpicyKeys.Menu;
                    case ConsoleKey.Enter:
                        return SpicyKeys.Select;
                    case ConsoleKey.P:
                        return SpicyKeys.EasterEgg;
                    default:
                        return SpicyKeys.Nothing;
                }
            }
            else
            {
                return SpicyKeys.Nothing;
            }
        }
    }
}
