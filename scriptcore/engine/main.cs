using System;

namespace BattleBots.Core
{
    static class Engine
    {
        #if COREBUILD
        static void Main()
        {
            InternalMain();
        }
        #endif

        private static void InternalMain()
        {
            
        }

        private Type[] GetInheritedClasses(Type MyType) 
        {
            //if you want the abstract classes drop the !TheType.IsAbstract but it is probably to instance so its a good idea to keep it.
            return System.Reflection.Assembly.GetAssembly(MyType).GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(MyType));
        }
    }
}