using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimCapiConsole
{
    static Text _text;

    public static void setConsoleText(Text text)
    {
        _text = text;
    }

    public static void log(string text)
    {
        if (_text == null)
            return;

        _text.text += text + "\n\n";
    }

    public static void log(string text, string prefix)
    {
        if (_text == null)
            return;

        _text.text += prefix + " " + text + "\n\n";
    }

    public static void clear()
    {
        _text.text = "";
    }
}