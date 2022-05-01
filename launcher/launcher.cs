// #define LAUNCHERBUILD
using Build;
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;


namespace BattleBots.Launcher
{
    internal static class LauncherApp
    {
        public static string battleBotsFolder {get; private set;} = "";
        internal static string buildInfoPath {get 
        {
            return battleBotsFolder + "data\\buildinfo";
        } set {}}
        static ulong launcherInfo;

        #if LAUNCHERBUILD
        private static void Main(string[] param)
        {
            battleBotsFolder = Assembly.GetEntryAssembly().Location; 
            battleBotsFolder = battleBotsFolder.Substring(0, battleBotsFolder.Length - 26);

            launcherInfo = LoadLauncherInfo();
            if (launcherInfo == 0)
            {
                Console.WriteLine("An error occured while trying to load the file {0}. Please make sure it exists. If not create a empty file with no extension and realunch the launcher.", buildInfoPath);
                // return;
            }
            else
            {
                Console.WriteLine("BattleBots Launcher v1.0\nCurrent bot build is {0}\nType 'build' to rebuild the bot code, type 'play' to launch the arena.", launcherInfo);
            }

            bool exit = false;

            if (param[0] == "-build")
            {
                Build();
                exit = true;
            }

            while (!exit)
            {
                Console.Write(">");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "build":
                    {
                        Build();
                    }
                    break;

                    // case "play":
                    // {
                    //     bool ret = Compiler.CompileScriptcore();
                    //     launcherInfo++;
                    //     SaveLauncherInfo(launcherInfo);
                    // }
                    // break;
                }
            }
        }
        #endif

        private static void Build()
        {
            bool ret = Compiler.CompileScriptcore();
            launcherInfo++;
            SaveLauncherInfo(launcherInfo);
        }

        private static ulong LoadLauncherInfo()
        {
            string text = "";
            try
            {
                text = File.ReadAllText(buildInfoPath);
            }
            catch (System.Exception)
            {
                return 0;
            }
            ulong data = Convert.ToUInt64(text);
            return data;
        }

        private static void SaveLauncherInfo(ulong buildInfo)
        {
            string text = buildInfo.ToString();
            File.WriteAllText(buildInfoPath, text);
        }
    }

    internal static class Compiler
    {
        internal static bool CompileScriptcore()
        {
            Builder.XMLBuildTargets buildTargets = Builder.XMLBuildTargets.ByName; 
            List<string> name = new List<string>(); name.Add("Core");

            BuildCommand buildCommand = Builder.GenerateBuildCommandsFromXML("buildscript.bml", buildTargets, name)[0];

            // buildCommand = new BuildCommand(buildCommand.buildName, ("bin\\" + buildCommand.buildCommand));

            Console.WriteLine("[Build] Building bot executibles");
            Console.ForegroundColor = ConsoleColor.Red;

            int retcode = buildCommand.Build();

            if (retcode != 0)
            {
                return false;
            }

            Console.ResetColor();
            Console.WriteLine("[Build] Build completed");
            return true;
        }
    }
}