using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Runtime.InteropServices;

namespace Build
{
    internal static class BuildApp
    {
        // Build with csc.exe build.cs -define:BUILDBUILD -out:CSBuild.exe
        #if BUILDBUILD

        static int Main(string[] paramaters)
        {
            bool anyFails = false;
            if ((paramaters.Length > 0) && 
                (paramaters[0] != "--help") && (paramaters[0] != "-h") &&
                (paramaters[0] != "/?") && (paramaters[0] != "/h"))
            {
                Builder.XMLBuildTargets buildInfo = Builder.XMLBuildTargets.Default;
                List<string> targetNames = new List<string>();

                if (paramaters.Length >= 2)
                {
                    buildInfo = GetXMLBuildTargetsFromString(paramaters[1]);
                    if (buildInfo == Builder.XMLBuildTargets.ByName)
                    {
                        if (paramaters.Length > 2)
                        {
                            for (int i = 1; i < paramaters.Length; i++)
                            {
                                if ((paramaters[i].ToCharArray()[0] != '-') &&
                                    (paramaters[i].ToCharArray()[0] != '/'))
                                {
                                    targetNames.Add(paramaters[i]);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            PreBuild(); Error();
                            Console.WriteLine("'name' Requires names to be passed afterwords");
                            Console.ResetColor();
                            return -1;
                        }
                    }
                }

                PreBuild();
                Console.WriteLine("Generating Build Commands...");
                List<BuildCommand> commands = Builder.GenerateBuildCommandsFromXML(paramaters[0], buildInfo, targetNames);
                
                if (commands == null)
                {
                    Console.ResetColor();
                    return -2;
                }


                foreach (BuildCommand buildTarget in commands)
                {
                    Build(); Console.WriteLine("Building {0}", buildTarget.buildName);

                    // In case it prints error text we want it red
                    Console.ForegroundColor = ConsoleColor.Red;


                    int retcode = 0;
                    try
                    {
                        retcode = buildTarget.Build();
                    }
                    catch (Exception e)
                    {
                        Build(); Error();
                        if (e.Message == "The system cannot find the file specified")
                        {
                            Console.WriteLine("Error runing build command: Could not find csc.exe, make sure it is installed, and either initilize vcvarsall.bat or run build.exe with the -auto switch");
                        }
                        else
                        {
                            Console.WriteLine("Error runing build command: {0}", e.Message);
                        }
                        Console.ResetColor();
                        anyFails = true;
                    }
                    
                    PostBuild();
                    string str = "";
                    if (retcode != 0)
                    {
                        str = " with errors";
                        anyFails = true;
                        Error();
                    }
                    Console.WriteLine("Build finished{1}. csc.exe returned {0}", retcode, str);
                }

                // Loop through ramaining paramaters
                // if (!anyFails)
                // {
                //     for (int i = 0; i < paramaters.Length; i++)
                //     {
                //         if (paramaters[i] == "-run")
                //         {
                //             // Builder.XMLBuildTargets buildtargets = buildInfo;
                //             // List<string> runNames = new List<string>(targetNames);

                //             // if (paramaters[i + 1].ToCharArray[0] != '-')
                //             // {
                //             //     buildtargets = GetXMLBuildTargetsFromString(paramaters[i + 1]);
                //             //     if (buildInfo == Builder.XMLBuildTargets.ByName)
                //             //     {
                //             //         if (i < paramaters.Length - 1)
                //             //         {
                //             //             for (int b = i + 1; b < paramaters.Length; b++)
                //             //             {
                //             //                 if ((paramaters[b].ToCharArray()[0] != '-') &&
                //             //                     (paramaters[b].ToCharArray()[0] != '/'))
                //             //                 {
                //             //                     runNames.Add(paramaters[b]);
                //             //                 }
                //             //                 else
                //             //                 {
                //             //                     if (b == (i + 1))
                //             //                     {
                //             //                         PostBuild(); Error();
                //             //                         Console.WriteLine("'name' Requires names to be passed afterwords");
                //             //                         Console.ResetColor();
                //             //                         return -3;
                //             //                     }

                //             //                     break;
                //             //                 }
                //             //             }
                //             //         }
                //             //         else
                //             //         {
                //             //             PostBuild(); Error();
                //             //             Console.WriteLine("'name' Requires names to be passed afterwords");
                //             //             Console.ResetColor();
                //             //             return -4;
                //             //         }
                //             //     }
                //             // }

                            
                //             foreach (BuildCommand buildTarget in commands)
                //             {
                //                 try
                //                 {
                //                     using (Process myProcess = new Process())
                //                     {
                //                         myProcess.StartInfo.UseShellExecute = false;
                //                         myProcess.StartInfo.FileName = "C:\\HelloWorld.exe";
                //                         myProcess.StartInfo.CreateNoWindow = true;
                //                         myProcess.Start();
                //                         // This code assumes the process you are starting will terminate itself.
                //                         // Given that it is started without a window so you cannot terminate it
                //                         // on the desktop, it must terminate itself or you can do it programmatically
                //                         // from this application using the Kill method.
                //                     }
                //                 }
                //                 catch (Exception e)
                //                 {
                //                     Console.WriteLine(e.Message);
                //                 }
                //             }
                //         }
                //     }                    
                // }
            }
            else
            {
                PrintHelp();
            }
            

            Console.ResetColor();
            if (anyFails)
            {
                return 1;
            }
            return 0;
        }

        private static Builder.XMLBuildTargets GetXMLBuildTargetsFromString(string str)
        {
            Builder.XMLBuildTargets buildInfo = Builder.XMLBuildTargets.Default;
            switch (str)
            {
                case "all":
                {
                    buildInfo = Builder.XMLBuildTargets.All;
                }
                break;

                case "default":
                {
                    buildInfo = Builder.XMLBuildTargets.Default;
                }
                break;

                case "name":
                {
                    buildInfo = Builder.XMLBuildTargets.ByName;
                }
                break;
            }
            return buildInfo;
        }

        private static void PreBuild()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[Pre Build] ");
        }
        private static void Build()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[Build] ");
        }
        private static void PostBuild()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("[Post Build] ");
        }
        private static void Error()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }

        private static void PrintHelp()
        {
            Console.ResetColor();
            Console.WriteLine("------------------------USAGE------------------------");
            Console.WriteLine("  build.exe [buildscript] [optional:build specifyer] [optional:...]");
            Console.WriteLine("    buildscript: the local or complete path to the buildscript file");
            Console.WriteLine("    build specifyer: which build targets to build. can be");
            Console.WriteLine("      default: builds the target marked with default=\"true\"");
            Console.WriteLine("      all: builds all targets in the buildscript");
            Console.WriteLine("      name [name][...]: builds the targets with the names specifyed");
        }
        #endif
    }

    public class BuildCommand
    {
        [DllImport("msvcrt.dll")]
        private static extern int system(string format);

        public BuildCommand(string name, string command, bool isDefault = false)
        {
            buildName = name;
            buildCommand = command;
            isDefaultBuild = isDefault;
        }

        public string buildCommand {get; private set;} = "";
        public string buildName {get; private set;} = "";

        public bool isDefaultBuild {get; private set;} = false;

        public int Build()
        {
            return system(buildCommand);
        }           
    }

    public static class Builder
    {
        public enum XMLBuildTargets
        {
            Default,
            All,
            ByName
        }

        public static List<BuildCommand> GenerateBuildCommandsFromXML(string filepath, XMLBuildTargets buildInfo, List<string> names = null)
        {
            List<BuildCommand> commands = new List<BuildCommand>();

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;

            try
            {
                doc.Load(filepath);
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("The file '" + filepath + "' could not be found. Please make sure you are running this command in the right directory and try again.");
                return null;
            }

            XmlNodeList buildNodes = doc.GetElementsByTagName("build");
            foreach (XmlNode node in buildNodes)
            {
                XmlElement buildElm = node as XmlElement;

                bool isDefault = false;
                if (buildElm.HasAttribute("default"))
                {
                    if (buildElm.GetAttribute("default") == "true")
                    {
                        isDefault = true;
                    }
                }
    
                if (((buildInfo == XMLBuildTargets.Default) && (isDefault)) ||
                    ((buildInfo == XMLBuildTargets.ByName) && (names.Contains(buildElm.GetAttribute("name")))) ||
                    (buildInfo == XMLBuildTargets.All))
                {
                    List<string> sources = new List<string>();
                    List<string> defines = new List<string>();
                    string output = "";

                    // Get from XML
                    XmlNodeList sourceNodes = buildElm.GetElementsByTagName("source");
                    foreach (XmlNode sourceNode in sourceNodes)
                    {
                        XmlElement sourceElement = sourceNode as XmlElement;
                        sources.Add(sourceElement.InnerText);
                    }
                    
                    XmlNodeList defineNodes = buildElm.GetElementsByTagName("define");
                    foreach (XmlNode defineNode in defineNodes)
                    {
                        XmlElement defineElement = defineNode as XmlElement;
                        defines.Add(defineElement.InnerText);
                    }

                    XmlNodeList outputNodes = buildElm.GetElementsByTagName("output");
                    if (outputNodes.Count == 1)
                    {
                        XmlElement outputElement = outputNodes[0] as XmlElement;
                        output = outputElement.InnerText;
                    }
                    
                    // Generate command
                    string command = GenerateBuildCommand(sources, defines, output);
                    commands.Add(new BuildCommand(buildElm.GetAttribute("name"), command, isDefault));
                }
                // else { dont build this target }
            }

            return commands;
        }

        public static string GenerateBuildCommand(List<string> sourceFiles, List<string> defines, string outputFile)
        {
            string buildstring = "csc.exe -nologo -debug:portable ";
            
            foreach (string file in sourceFiles)
            {
                buildstring += (file + " ");
            }

            foreach (string define in defines)
            {
                buildstring += ("-define:" + define + " ");
            }

            if (!string.IsNullOrEmpty(outputFile))
            {
                buildstring += ("-out:" + outputFile);
            }

            return buildstring;
        }
    }
}