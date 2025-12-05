using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRLogOverlay : MonoBehaviour
{
    public TextWrapper logText; // TextMeshProUGUI ‚Å‚à OK
    private string buffer = "";
    private const int maxLines = 9;

    private void Awake()
    {
        logText.Initialize();
    }

    void OnEnable()
    {
        Application.logMessageReceived += OnLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= OnLog;
    }

    void OnLog(string condition, string stackTrace, LogType type)
    {
        // F•Ï‚¦‚½‚è‚µ‚Ä‚à‚¢‚¢
        string prefix = type switch
        {
            LogType.Warning => "<color=yellow>[W]</color> ",
            LogType.Error => "<color=red>[E]</color> ",
            _ => ""
        };

        buffer += $"{prefix}{condition}\n";

        // s”§ŒÀ
        var lines = buffer.Split('\n');
        if (lines.Length > maxLines)
        {
            buffer = string.Join("\n", lines, lines.Length - maxLines, maxLines);
        }

        logText.SetText(buffer);
    }
}

