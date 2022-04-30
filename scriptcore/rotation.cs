
namespace BattleBots
{
    sealed class Rotation
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
                angle = 360 - value;
            }
            else
            {
                m_angle = value % 360;
            }
        }}
        
    }
}