using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RetroConsole.Extented;
using RetroConsole.Windows;
using static RetroConsole.Utility.ConstantsLibrary;

namespace RetroConsole.Console
{
    [AddComponentMenu("RetroConsole/Application/Terminal/Terminal")]
    public class TerminalMaster : MonoBehaviour, IOrder
    {
        #region Variables
        public ExternalCommands commands;

        [Space(5)]
        public string initCommand = null;

        //[HideInInspector]
        public string currentUser = "nouser";

        private TerminalBuffer _buffer;

        private string historyrcPath = $"{logPath}{rsFiles[0]}";
        private List<string> history = new();

        private int currentHistoryCommandIndex;
        private bool isInitCommand = false;

        #endregion

        #region Unity functions
        private void Awake() =>
            Init();

        private void Start() =>
            _buffer.BufferInit($"{currentUser}@{Application.productName}", $"Current login time - <color=green>{DateTime.Now}</color> on <color=yellow>{SystemInfo.deviceName}</color>");

        private void OnDestroy() =>
            System.IO.File.WriteAllLines(historyrcPath, history.ToArray());

        #endregion

        #region IOrder overides
        public void Init()
        {
            string[] _history = null;
            try
            {
                _history = System.IO.File.ReadAllLines(historyrcPath);
                foreach (string line in _history)
                    history.Add(line);
            }
            catch
            {
                Debug.Log("At first startup there is can be troubles with gettin history from data file. Ignore it, it will be fix in the next startup");
            }

            _buffer = GetComponentInChildren<TerminalBuffer>();
            _buffer.SetOrder(this);

            if (initCommand != string.Empty)
            {
                isInitCommand = true;
                _buffer.ExecuteWithoutVisual(initCommand);
            }
        }

        public void OnInputEnter(string input)
        {
            if (!isInitCommand)
            {
                history.Add(input);
                currentHistoryCommandIndex = history.Count;
            }

            if (CheckClassicCommands(input))
                return;
            else if (CheckExternalCommands(input))
                return;
            else
                _buffer.PrintLine($"<color=red>Invalid command - {input}</color>");
        }

        public void OnExit()
        {
            //In master terminal script this functios never calls
        }

        public void OnArrowUp()
        {
            if (currentHistoryCommandIndex > 0)
            {
                currentHistoryCommandIndex--;
                _buffer.InsertInput(history[currentHistoryCommandIndex]);
            }
            else
                _buffer.OnEnd();
        }

        public void OnArrowDown()
        {
            if (currentHistoryCommandIndex < history.Count-1)
            {
                currentHistoryCommandIndex++;
                _buffer.InsertInput(history[currentHistoryCommandIndex]);
            }
            else
                _buffer.OnEnd();
        }

        public void UserInit()
        {
            historyrcPath = $"{logPath}{rsFiles[0]}";
            history = System.IO.File.ReadAllLines(historyrcPath).ToList();

            currentHistoryCommandIndex = history.Count;
        }

        #endregion

        #region Commands cheking
        private bool CheckClassicCommands(string command)
        {
            switch (command)
            {
                case "clear":
                    _buffer.ClearBuffer();
                    return true;
                case "exit":
                    Application.Quit();
                    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                    #endif
                    return true;
                case "gamedata":
                    ShowGameData();
                    return true;
                case "close":
                    CloseWindow();
                    return true;
                case "fullscreen":
                    FullscreenWindow();
                    return true;
                case "history":
                    PrintHistroy();
                    return true;
                case "help":
                    PrintHelp();
                    return true;
                case "lock":
                    _buffer.SetReadOnly();
                    return true;
                default: return false;
            }
        }

        private bool CheckExternalCommands(string command)
        {
            string[] words = command.Split(' ');

            for (int i = 0; i < commands.commands.Length; i++)
            {
                if (commands.commands[i].command == words[0])
                {
                    ChangeOrder(commands.commands[i].execuble.GetComponent<TerminalCommand>(), command, words);
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Internal functions
        private void ChangeOrder(TerminalCommand command, string input, string[] separatedInput)
        {
            command.SetTerminalReference(this);
            command.SetBufferReference(_buffer);

            _buffer.SetOrder(command);

            command.input = input;
            command.separatedinput = separatedInput;

            command.Init();
        }

        private void ShowGameData() =>
            _buffer.PrintLine($"Game Name - <color=yellow>{Application.productName}</color>\nCompany Name - <color=yellow>{Application.companyName}</color>\nBuild ver - <color=green>{Application.version}</color>\nEngine Version (Unity ver) - <color=blue>{Application.unityVersion}</color>");
        
        private void CloseWindow() =>
            GetComponent<Window>().Close();

        private void FullscreenWindow() =>
            GetComponent<Window>().Fullscreen();

        private void PrintHistroy()
        {
            for (int i = 0; i < history.Count; i++)
                _buffer.PrintLine($"{i} {history[i]}");
        }

        private void PrintHelp()
        {
            ExternalCommands.CommandAndExecutes[] _commands = commands.commands;

            _buffer.PrintLine("<color=yellow>clear</color> - Clears current teminal buffer\n" +
                "<color=yellow>exit</color> - Kill game window\n" +
                "<color=yellow>gamedata</color> - Standart output with information about developer, game name engine version and etc.\n" +
                "<color=yellow>close</color> - Closues current terminal window\n" +
                "<color=yellow>fullscreen</color> - Resizes current terminal window on fullsreen and back\n" +
                "<color=yellow>history</color> - Standart output from \"historyrs.rdk\"\n" +
                "<color=yellow>lock</color> - sets current buffer as read only");

            foreach (ExternalCommands.CommandAndExecutes com in _commands)
                _buffer.PrintLine($"<color=yellow>{com.command}</color> - {com.description}");
        }

        #endregion
    }
}
