using System;

namespace BattleBots
{
    public sealed class Position
    {
        public long x {get; set;}
        public long y {get; set;}

        public Position()
        {
            x = 0;
            y = 0;
        }

        public Position(long x, long y)
        {
            this.x = x;
            this.y = y;
        }


        // OPERATORS
        public static Position operator +(Position a, Position b) => new Position(a.x + b.x, a.y + b.y);
        public static Position operator -(Position a, Position b) => new Position(a.x - b.x, a.y - b.y);
        public static Position operator *(Position a, long m) => new Position(a.x * m, a.y * m);
        public static Position operator /(Position a, int m) => new Position(a.x / m, a.y / m);
        public static bool operator ==(Position a, Position b) => ((a.x == b.x) && (a.y == b.y));
        public static bool operator !=(Position a, Position b) => ((a.x != b.x) || (a.y != b.y));
        
        public override bool Equals(object obj)
        {
            Position p = obj as Position;
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return (x == p.x) && (y == p.y);
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // STATIC
        public static uint Distance(Position p1, Position p2)
        {
            return (uint)Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2));
        }

        public static Position Clamp(Position origional, long xMin, long xMax, long yMin, long yMax)
        {
            Position p = new Position(origional.x, origional.y);
            
            if (p.x > xMax)
            {
                p.x = xMax;
            }
            else if (p.x < xMin)
            {
                p.x = xMin;
            }

            if (p.y > yMax)
            {
                p.y = yMax;
            }
            else if (p.y < yMin)
            {
                p.y = yMin;
            }

            return p;
        }

        public static Position Random(long min, long max)
        {
            Random r = new Random();
            return new Position(r.Next((int)min, (int)max), r.Next((int)min, (int)max));
        }

        public static Position PointAtAngle(Position start, Rotation direction, uint distance)
        {
            Position dest = new Position();
            dest.x = (start.x + (long)((double)distance * Math.Sin(direction.radians)));
            dest.y = (start.y + (long)((double)distance * Math.Cos(direction.radians)));
            return dest;
        }        
    }
}