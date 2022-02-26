using KSP.UI.Screens.DebugToolbar;
using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace PlanetInfoPlus
{
    /// <summary>
    /// Adds custom console commands for PlanetInfoPlus.
    /// </summary>
    [KSPAddon(KSPAddon.Startup.AllGameScenes, false)]
    internal class DebugConsole : MonoBehaviour
    {
        internal const string COMMAND = "pip";
        private const string HELP = "Work with the PlanetInfoPlus mod";
        private static readonly Regex CONSECUTIVE_SPACE = new Regex(@"\s+");

        private static readonly ConsoleCommand[] COMMANDS =
        {
            new HelpCommand(),
            new ConsolePrecalculateCommand(),
            new ConsoleDumpCommand(),
        };

        public void Awake()
        {
            DebugScreenConsole.AddConsoleCommand(COMMAND, OnCommand, HELP);
        }

        /// <summary>
        /// Here when the PlanetInfoPlus command is triggered.
        /// </summary>
        /// <param name="arg"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnCommand(string arg)
        {
            string[] arguments = ParseArguments(arg);
            if (arguments.Length == 0) arguments = new string[] { HelpCommand.COMMAND };
            string command = arguments[0];
            string[] commandArgs = new string[arguments.Length - 1];
            for (int i = 0; i < commandArgs.Length; ++i)
            {
                commandArgs[i] = arguments[i + 1];
            }
            for (int i = 0; i < COMMANDS.Length; ++i)
            {
                if (COMMANDS[i].Command == command)
                {
                    if ((commandArgs.Length == 1) && (commandArgs[0] == HelpCommand.COMMAND))
                    {
                        Logging.Log(HelpCommand.HelpStringOf(COMMANDS[i]));
                        return;
                    }
                    try
                    {
                        COMMANDS[i].Call(commandArgs);
                    }
                    catch (CommandException e)
                    {
                        Logging.Error("/" + COMMAND + " " + command + ": " + e.Message);
                    }
                    return;
                }
            }
            Logging.Error("Unknown command: /" + COMMAND + " " + command);
        }

        /// <summary>
        /// Parses an argument string into an array of zero or more individual arguments.
        /// </summary>
        /// <param name="argString"></param>
        /// <returns></returns>
        private static string[] ParseArguments(string argString)
        {
            if (argString == null) return new string[0];

            argString = argString.Trim();
            if (string.Empty == argString) return new string[0];
            return CONSECUTIVE_SPACE.Replace(argString, (m) => " ").Split(' ');
        }

        /// <summary>
        /// Implements help for available console commands.
        /// </summary>
        private class HelpCommand : ConsoleCommand
        {
            public const string COMMAND = "help";

            public HelpCommand() : base(COMMAND, "Shows this message") { }

            public override void Call(string[] arguments)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("Commands available via /{0}:", DebugConsole.COMMAND);
                for (int i = 0; i < DebugConsole.COMMANDS.Length; ++i)
                {
                    builder.Append("\n").Append(HelpStringOf(DebugConsole.COMMANDS[i]));
                }
                Logging.Log(builder.ToString());
            }

            public static string HelpStringOf(DebugConsole.ConsoleCommand command)
            {
                if (command.Usage == null)
                {
                    return string.Format("/{0} {1}: {2}", DebugConsole.COMMAND, command.Command, command.Help);
                }
                else
                {
                    return string.Format("/{0} {1} {2}: {3}", DebugConsole.COMMAND, command.Command, command.Usage, command.Help);
                }
            }
        }

        /// <summary>
        /// Base class for commands defined here.
        /// </summary>
        internal abstract class ConsoleCommand
        {
            private readonly string command;
            private readonly string help;
            private readonly string usage;

            protected ConsoleCommand(string command, string help, string usage = null)
            {
                this.command = command;
                this.help = help;
                this.usage = usage;
            }

            public string Command { get { return command; } }
            public string Help { get { return help; } }
            public string Usage { get { return usage; } }

            public abstract void Call(string[] arguments);

            protected CommandException UsageException()
            {
                return new CommandException(usage);
            }
        }

        /// <summary>
        /// Thrown when a command encounters a problem while executing.
        /// </summary>
        internal class CommandException : Exception
        {
            public CommandException(string message) : base(message) { }

            public CommandException(string message, Exception cause) : base(message, cause) { }
        }
    }
}
