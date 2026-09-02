using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using RetroConsole.Utility;
using RetroConsole.Extented;

namespace RetroConsole.Console.Commands
{
    public class Cat : TerminalCommand, IOrder
    {
        #region Variables
        private string path;
        private string[] fileLines;
        
        public bool numerate = false;
        public bool numerateWithoutEmpty = false;

        #endregion

        #region API
        public override void Init()
        {
            path = GetPath();

            if (path == string.Empty)
            {
                Debug.LogError("Empty path");
                buffer.PrintLine("<color=red>No path given!</color>\nUse that syntaxis for read a file:\ncat <color=green>[path]</color> <color=purple>[argiments (-n or -b)]</color>");
                OnExit();
                return;
            }

            try
            {
                fileLines = File.ReadAllLines(path);
            }
            catch (FileNotFoundException)
            {
                Debug.LogError("File not found");
                buffer.PrintLine("<color=red>Invalid path! File not found</color>");
                OnExit();
                return;
            }

            for (int i = 1; i < separatedinput.Length; i++)
            {
                switch (separatedinput[i])
                {
                    case "-n":
                        if (numerateWithoutEmpty != true)
                            numerate = true;

                        break;
                    case "-b":
                        numerateWithoutEmpty = true;

                        if (numerate != false)
                            numerate = false;

                        break;
                }
            }

            if (!numerate && !numerateWithoutEmpty)
                buffer.PrintLine(string.Join(" ", fileLines));
            else
            {
                if (numerate != false)
                    for (int i = 0; i < fileLines.Length; i++)
                        buffer.PrintLine($"{i} {fileLines[i]}");

                if (numerateWithoutEmpty != false)
                {
                    for (int i = 0; i < fileLines.Length; i++)
                        if (fileLines[i] != string.Empty)
                            buffer.PrintLine($"{i} {fileLines[i]}");
                }
            }

            OnExit();
        }
        public override void OnExit()
        {
            ResetVars();

            buffer.SetOrder(master);

            buffer.SetFormat($"unity@{Application.productName}");
        }

        #endregion

        #region Internal functions

        private string GetPath()
        {
            List<string> _path = new List<string>();

            List<string> tokens = new List<string>();
            foreach(Match m in Tokenizator.TokenRx.Matches(input))
                tokens.Add(Tokenizator.Unescape(m.Groups["val"].Value));

            if (tokens.Count == 0)
                throw new ArgumentException("Null arguments");

            foreach (string t in tokens.Skip(1))
            {
                if (t.Length > 1 && t[0] == '-')
                    Debug.Log("Ignore");
                else
                    _path.Add(t);
            }

            return string.Join(' ', _path);
        }

        private void ResetVars()
        {
            path = string.Empty;
            numerate = false;
            numerateWithoutEmpty = false;
        }

        #endregion
    }
}
