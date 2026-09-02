using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using RetroConsole.Extented;
using RetroConsole.Utility;

namespace RetroConsole.Console.Commands
{
    public class Rm: TerminalCommand, IOrder
    {
        #region Variables
        private string path;

        private bool verbose = false, recursive = false, force = false;

        private FileInfo[] fileInfos;
        private DirectoryInfo[] directoryInfos;

        #endregion

        #region API
        public override void Init()
        {
            path = GetPath();

            if (path == string.Empty)
            {
                Debug.LogError("Empty path");
                buffer.PrintLine("<color=red>No path given!</color>\nUse that syntaxis for read a file:\nrm <color=green>[path]</color> <color=purple>[argiments]</color>");
                OnExit();
                return;
            }

            for (int i = 1; i < separatedinput.Length; i++)
            {
                switch (separatedinput[i])
                {
                    case "-r":
                        recursive = true;
                        GetCatalogAndFiles();
                        break;
                    case "-v":
                        verbose = true;
                        break;
                    case "-f":
                        force = true;
                        break;
                    case "-rf":
                        force = true;
                        recursive = true;
                        break;
                }
            }

            if (recursive != true)
            {
                if (File.Exists(path) != true)
                {
                    if (Directory.Exists(path) != false && force != true)
                    {
                        buffer.PrintLine($"<color=red>{path} is cataloge</color>");
                        OnExit();
                        return;
                    }
                    else if (Directory.Exists(path) != true)
                    {
                        buffer.PrintLine($"<color=red>{path} file not founde</color>");
                        OnExit();
                        return;
                    }
                    else
                    {
                        Directory.Delete(path);
                        if (verbose != false)
                            buffer.PrintLine($"Cataloge on path: {path} was deleted!");
                        OnExit();
                        return;
                    }
                }
                else
                {
                    File.Delete(path);
                    if (verbose != false)
                        buffer.PrintLine($"File on path: {path} was deleted!");
                    OnExit();
                    return;
                }
            }
            else
            {
                foreach (FileInfo file in fileInfos)
                {
                    File.Delete(file.FullName);
                    if (verbose != false)
                        buffer.PrintLine($"File {file.Name} was deleted!");
                }

                foreach (DirectoryInfo dir in directoryInfos)
                {
                    Directory.Delete(dir.FullName);
                    if (verbose != false)
                        buffer.PrintLine($"Directory {dir.Name} was deleted!");
                }

                if (File.Exists(path) != true)
                {
                    if (Directory.Exists(path) != false && force != true)
                    {
                        buffer.PrintLine($"<color=red>{path} is cataloge</color>");
                        OnExit();
                        return;
                    }
                    else if (Directory.Exists(path) != true)
                    {
                        buffer.PrintLine($"<color=red>{path} file not founde</color>");
                        OnExit();
                        return;
                    }
                    else
                    {
                        Directory.Delete(path);
                        if (verbose != false)
                            buffer.PrintLine($"Cataloge on path: {path} was deleted!");
                        OnExit();
                        return;
                    }
                }
                else
                {
                    File.Delete(path);
                    if (verbose != false)
                        buffer.PrintLine($"File on path: {path} was deleted!");
                    OnExit();
                    return;
                }
            }
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
            foreach (Match m in Tokenizator.TokenRx.Matches(input))
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

        private void GetCatalogAndFiles()
        {
            DirectoryInfo pathInfo = new DirectoryInfo(path);

            fileInfos = pathInfo.GetFiles("*", SearchOption.AllDirectories);
            directoryInfos = pathInfo.GetDirectories("*", SearchOption.AllDirectories);
        }

        private void ResetVars()
        {
            path = string.Empty;
            recursive = false;
            force = false;
            verbose = false;
        }

        #endregion

    }
}
