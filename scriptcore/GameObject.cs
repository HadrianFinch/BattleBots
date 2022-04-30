
namespace BattleBots
{
    public abstract class GameObject
    {
        public GameObject gameObject => this;

        public Transform transform {get;} = new Transform();
    }

    public class Transform
    {
        Position position = new Position();
        
    }
}