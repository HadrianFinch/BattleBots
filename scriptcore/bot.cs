using System;
using System.Reflection;
using System.Diagnostics;

namespace BattleBots
{
    abstract class Bot : GameObject
    {
        protected uint ticksRemaining {get; private set;} = 0;

        public uint cupcakesDelivered {get; internal set;} = 0;

        public Bot()
        {

        }

        protected static class Debug
        {
            public static void Log(string text, params object[] p)
            {
                Console.Write("[{0}] ", NameOfCallingBot());
                Console.WriteLine(text, p);
            }

            private static string NameOfCallingBot()
            {
                string fullName;
                Type declaringType;
                int skipFrames = 2;
                do
                {
                    MethodBase method = new StackFrame(skipFrames, false).GetMethod();
                    declaringType = method.DeclaringType;
                    if (declaringType == null)
                    {
                        return method.Name;
                    }
                    skipFrames++;
                    fullName = declaringType.FullName;
                }
                while ((declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase)) &&
                declaringType.IsSubclassOf(typeof(Bot)) &&
                (!declaringType.IsAbstract) &&
                (declaringType.IsClass));

                return fullName;
            }
        }

        internal uint TakeTurn(uint availibleTics)
        {
            ticksRemaining = availibleTics;
            Update();
            uint leftoverTicks = ticksRemaining;
            ticksRemaining = 0;
            return leftoverTicks;
        }

        private bool TrySpendTicks(uint count)
        {
            if (ticksRemaining >= count)
            {
                ticksRemaining -= count;
                ResolvePendingTicks(count);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ResolvePendingTicks(uint count)
        {
            
        }

        protected bool Move(Rotation direction, uint distance)
        {
            if (ticksRemaining >= distance)
            {
                Position destination = null;
                for (int d = 0; d < distance; d++)
                {
                    Position tempDest = Position.PointAtAngle(transform.position, direction, distance);
                    if (GameObject.GetObjectFromPoint(tempDest) == null)
                    {
                        TrySpendTicks(1);
                        tempDest = Position.Clamp(
                            tempDest,
                            0, Arena.size,
                            0, Arena.size);
                        destination = tempDest;
                    }
                    else
                    {
                        break;
                    }              
                }
                
                transform.position = destination;
                return true;
            }
            else
            {
                return false;
            }
        }

        public enum EquipableItem
        {
            CupcakeCannon,
            RapidFireCupcakeLauncher,
            CupcakeClusterCannon,      
            CupcakeLaser,
            CupcakeMine,
            SeekerCupcake,
        }

        public EquipableItem[] equipedItems {get; internal set;} = new EquipableItem[2];
        internal bool allowWeaponSwaping = true;
    
        protected bool EquipItem(EquipableItem newItem, int index)
        {
            if (!allowWeaponSwaping)
            {
                return false;
            }

            if (TrySpendTicks(8))
            {
                equipedItems[index] = newItem;
                return true;
            }
            else
            {
                return false;
            }
        }

        // Update is called each turn of the game
        protected abstract void Update();
    }
}