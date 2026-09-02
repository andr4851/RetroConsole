using System;
using UnityEngine;

namespace RetroConsole.Console
{
    [CreateAssetMenu(fileName = "ExternalCommands", menuName = "RetroConsole/Terminal/ExternalCommands")]
    public class ExternalCommands : ScriptableObject
    {
        public CommandAndExecutes[] commands;

        [Serializable]
        public struct CommandAndExecutes
        {
            public string command;
            public GameObject execuble;

            [TextArea(4, 10)]
            public string description;
        }
    }
}
