using UnityEngine;
using RetroConsole.Extented;
using Codice.Client.BaseCommands;

namespace RetroConsole.Console.Commands
{
    [AddComponentMenu("RetroConsole/Terminal Commands/Log")]
    public class Log: TerminalCommand, IOrder
    {
        #region Variables
        private GameObject currentTerminal;

        private bool inWork = false;

        private bool showLog, showWarn, showError, showAssert, showException, showStack; 

        #endregion

        #region Unity functions
        private void OnApplicationQuit() =>
            ResetVars();

        #endregion

        #region API
        public override void Init()
        {
            if (separatedinput.Length <= 1)
                SetThemAllTrue();
            else 
                SetThemAllFalse();

            for (int i = 1; i < separatedinput.Length; i++)
            {
                switch (separatedinput[i])
                {
                    case "-l":
                        showLog = true;
                        break;
                    case "-w":
                        showWarn = true;
                        break;
                    case "-e":
                        showError = true;
                        break;
                    case "-a":
                        showAssert = true;
                        break;
                    case "-x":
                        showException = true;
                        break;
                    case "-s":
                        showStack = true;
                        break;
                }
            }

            if (inWork != false || currentTerminal == null)
            {
                buffer.SetFormat(string.Empty);
                buffer.ClearBuffer();

                inWork = true;
                currentTerminal = master.gameObject;
                Application.logMessageReceived += Getter;
            }
            else
            {
                buffer.PrintLine("<color=red>There can't be more then one active logger!\nClose the window with active logger and try again</color>");
                OnExit();
            }
        }

        public override void OnInputEnter(string input)
        {
            //This void is plug
        }

        public override void OnExit()
        {
            buffer.PrintLine("The logger was Interrupted!");
            Application.logMessageReceived -= Getter;

            buffer.SetOrder(master);

            buffer.SetFormat($"unity@{Application.productName}");
        }

        public override void OnCtrlC()
        {
            ResetVars();
            OnExit();
        }

        #endregion

        #region Internal functions
        private void Getter (string _messege, string _stack, LogType _logType)
        {
            switch (_logType)
            {
                case LogType.Error:
                    if (showError)
                        if (showStack)
                            buffer.PrintLine($"<color=red>[ERROR]{_messege} in {_stack}</color>");
                        else
                            buffer.PrintLine($"<color=red>[ERROR]{_messege}</color>");
                    break;
                case LogType.Warning:
                    if (showWarn)
                        if (showStack)
                            buffer.PrintLine($"<color=yellow>[WARNING]{_messege} in {_stack}</color>");
                        else
                            buffer.PrintLine($"<color=yellow>[WARNING]{_messege}</color>");
                    break;
                case LogType.Log:
                    if (showLog)
                        if (showStack)
                            buffer.PrintLine($"<color=green>[LOG]{_messege} in {_stack}</color>");
                        else
                            buffer.PrintLine($"<color=green>[LOG]{_messege}</color>");
                    break;
                case LogType.Assert:
                    if (showAssert)
                        if (showStack)
                            buffer.PrintLine($"<color=red>[ASSERT]{_messege} in {_stack}</color>");
                        else
                            buffer.PrintLine($"<color=red>[ASSERT]{_messege}</color>");
                    break;
                case LogType.Exception:
                    if (showException)
                        if(showStack)
                            buffer.PrintLine($"<color=red>[EXCEPTION]{_messege} in {_stack}</color>");
                        else
                            buffer.PrintLine($"<color=red>[EXCEPTION]{_messege}</color>");
                    break;
            }

        }

        private void ResetVars()
        {
            currentTerminal = null;
            inWork = false;

            input = string.Empty;
            separatedinput = null;
        }

        private void SetThemAllTrue()
        {
            showLog = true;
            showError = true;
            showWarn = true;
            showException = true;
            showAssert = true;
            showStack = true;
        }

        private void SetThemAllFalse()
        {
            showLog = false;
            showError = false;
            showWarn = false;
            showException = false;
            showAssert = false;
            showStack = false;
        }

        #endregion
    }
}
