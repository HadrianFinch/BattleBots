
namespace BattleBots
{
    public sealed class Rotation
    {
        private int m_angle = 0;
        public int angle {
        get
        {
            return m_angle;
        }
        set
        {
            if (value < 0)
            {
                angle = 360 + value;
            }
            else
            {
                m_angle = value % 360;
            }
        }}

        public double radians 
        {
            get
            {
                return ((double)m_angle / 360.0) * (2.0 * System.Math.PI);
            }
        }

        public Rotation()
        {
            angle = 0;
        }

        public Rotation(int a)
        {
            angle = a;
        }
        
        public static Rotation up {get {return new Rotation(0);}}
        public static Rotation down {get {return new Rotation(180);}}
        public static Rotation left {get {return new Rotation(270);}}
        public static Rotation right {get {return new Rotation(90);}}
    }
}