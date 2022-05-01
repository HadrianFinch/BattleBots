
namespace BattleBots
{
    public abstract class GameObject
    {
        public GameObject gameObject => this;

        public Transform transform {get;} = new Transform();
    }

    public class Transform
    {
        public Position position {get; internal set;} = new Position();
        public Rotation rotation {get; internal set;} = new Rotation();
    }
}