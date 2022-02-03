using System;
using System.Collections.Generic;
using System.IO;

namespace Melodi.IO
{
    public class SmartLogger
    {
        private static string DatetimeParser = "MM/dd/yyyy HH:mm:ss.fff";

        public List<string> Data;
        private readonly LogLevel MinLogLevel;
        public bool AllowInvert = true;
        public SmartLogger(LogLevel logLevel)
        {
            this.MinLogLevel = logLevel;
            Data = new List<string>();
        }
        public void Save(string filepath)
        {
            System.IO.File.WriteAllLines(filepath, Data);
        }
        public void Log(LogLevel logLevel, string text)
        {
            string time = DateTime.Now.ToString(DatetimeParser);
            string logLevelText;
            ConsoleColor logLevelColor;
            bool invert = false;

            switch (logLevel)
            {
                case LogLevel.None:
                    logLevelText = "null";
                    logLevelColor = ConsoleColor.DarkGray;
                    break;
                
                case LogLevel.Debug:
                    logLevelText = "deb ";
                    logLevelColor = ConsoleColor.DarkMagenta;
                    break;
                
                case LogLevel.Information:
                    logLevelText = "info";
                    logLevelColor = ConsoleColor.Blue;
                    break;
                
                case LogLevel.Trace:
                    logLevelText = "trc ";
                    logLevelColor = ConsoleColor.Green;
                    invert = true;
                    break;
                
                case LogLevel.Warning:
                    logLevelText = "warn";
                    logLevelColor = ConsoleColor.DarkYellow;
                    break;
                
                case LogLevel.Error:
                    logLevelText = "err ";
                    logLevelColor = ConsoleColor.Red;
                    break;
                
                case LogLevel.Critical:
                    logLevelText = "crit";
                    logLevelColor = ConsoleColor.DarkRed;
                    invert = true;
                    break;
                
                default:
                    logLevelText = "null";
                    logLevelColor = Console.ForegroundColor;
                    break;
            }
            
            Data.Add($"[{time}] [{logLevelText}]{new string(' ', 5)}{text}");
            
            if (MinLogLevel <= logLevel)
            {
                Console.ResetColor();
                (ConsoleColor, ConsoleColor) defColors = (Console.ForegroundColor, Console.BackgroundColor);

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"[{time}] ");
                Console.ResetColor();

                if (invert && AllowInvert)
                {
                    Console.ForegroundColor = defColors.Item2;
                    Console.BackgroundColor = logLevelColor;
                }
                else
                {
                    Console.ForegroundColor = logLevelColor;
                }
                Console.Write($"[{logLevelText}]");
                Console.ResetColor();
                
                Console.WriteLine($"{new string(' ', 5)}{text}");
                Console.ResetColor();
            }
        }
    }

    public enum LogLevel
    {
        Trace = 0,
        Debug = 1,
        Information = 2,
        Warning = 3,
        Error = 4,
        Critical = 5,
        None = 6
    }
}