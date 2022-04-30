using System;

namespace BattleBots
{
    public sealed class Position
    {
        public int x {get; set;}
        public int y {get; set;}

        public Position()
        {
            x = 0;
            y = 0;
        }

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        // OPERATORS
        public static Position operator +(Position a, Position b) => new Position(a.x + b.x, a.y + b.y);
        public static Position operator -(Position a, Position b) => new Position(a.x - b.x, a.y - b.y);
        public static Position operator *(Position a, int m) => new Position(a.x * m, a.y * m);
        public static Position operator /(Position a, int m) => new Position(a.x / m, a.y / m);
        public static Position operator ==(Position a, Position b) => ((a.x == b.x) && (a.y == b.y));

        // STATIC
        public static uint Distance(Position p1, Position p2)
        {
            return Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2));
        }
    }
}