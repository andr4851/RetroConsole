using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using RetroConsole.Extented;
using RetroConsole.Utility;
using RetroConsole.Console;

public class Mk: TerminalCommand, IOrder
{
    #region Variables
    private string path;

    private bool directory = false, verbose = false;

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
                case "-d":
                    directory = true;
                    break;
                case "-v":
                    verbose = true;
                    break;
            }
        }

        try
        {
            if(directory != true)
            {
                File.Create(path);
                if (verbose != false)
                    buffer.PrintLine($"File on path: {path} was created!");
            }
            else
            {
                Directory.CreateDirectory(path);
                if (verbose != false)
                    buffer.PrintLine($"Cataloge on path: {path} was created!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            buffer.PrintLine($"Error was accured!\nError messege: {e}");
            OnExit();
            return;
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

    private void ResetVars()
    {
        path = string.Empty;
        directory = false;
    }

    #endregion
}
