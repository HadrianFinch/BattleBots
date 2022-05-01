using System.Collections.Generic;

namespace BattleBots
{
    public abstract class GameObject
    {
        internal static List<GameObject> allGameObjects = new List<GameObject>();

        // Returns null if there is nothing there
        internal static GameObject GetObjectFromPoint(Position point)
        {
            foreach (GameObject go in allGameObjects)
            {
                if (go.transform.position == point)
                {
                    return go;
                }
            }
            return null;
        }


        public GameObject gameObject => this;

        public Transform transform {get;} = new Transform();

        public GameObject()
        {
            allGameObjects.Add(this);
        }
    }

    public class Transform
    {
        public Position position {get; internal set;} = new Position();
        public Rotation rotation {get; internal set;} = new Rotation();
    }
}