using RetroConsole.Extented;
using UnityEngine;

namespace RetroConsole.Console.Commands
{
    [AddComponentMenu("RetroConsole/Terminal/Echo")]
    public class Echo : TerminalCommand, IOrder
    {
        #region API
        public override void Init()
        {
            buffer.PrintLine(input.Replace("echo", string.Empty));
            OnExit();
        }

        public override void OnExit()
        {
            buffer.SetOrder(master);

            buffer.SetFormat($"unity@{Application.productName}");
        }

        #endregion

    }
}
