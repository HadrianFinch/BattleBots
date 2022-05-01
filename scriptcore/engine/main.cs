using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace BattleBots.Core
{
    static class Arena
    {
        public static readonly long size = 1000;
        public static readonly uint turnTickAmount = 10;
        public static readonly uint tickTime = 50;

        public static void UpdateFrame()
        {

        }

        public static void CreateViewportWindow()
        {

        }
    }
    static class Engine
    {
        internal static List<Bot> bots = new List<Bot>();

        #if COREBUILD
        static void Main()
        {
            InternalMain();
        }
        #endif

        private static void InternalMain()
        {
            Type[] botScripts = GetInheritedClasses(typeof(Bot));
            foreach (Type botScript in botScripts)
            {
                Bot bot = Activator.CreateInstance(botScript) as Bot;
                bot.transform.position = Position.Random(0, Arena.size);
                bots.Add(bot);
            }

            Console.WriteLine("[System] Bots initilized");

            Arena.CreateViewportWindow();
            Arena.UpdateFrame();

            bool hasWinner = false;
            while (!hasWinner)
            {
                for (int i = 0; i < bots.Count; i++)
                {
                    Bot bot = bots[i];
                    uint usedTicks = (Arena.turnTickAmount + bot.TakeTurn(Arena.turnTickAmount));
                    Thread.Sleep((int)(Arena.tickTime * usedTicks));
                }
            }
        }

        private static Type[] GetInheritedClasses(Type MyType) 
        {
            //if you want the abstract classes drop the !TheType.IsAbstract but it is probably to instance so its a good idea to keep it.
            return System.Reflection.Assembly.GetAssembly(MyType).GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(MyType)).ToArray();
        }
    }
}