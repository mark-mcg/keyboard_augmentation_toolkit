using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class BaseLoggerToFile : BaseLogger
{
    private LogWriter logToFile;
    public string filename;

    public override void StartLogging()
    {
        if (GetFileName() != null && GetFileName().Length > 0)
            logToFile = new LogWriter(GetFileName());
        else
            Debug.LogError("Filename not set");

        base.StartLogging();
    }

    public override void StopLogging()
    {
        base.StopLogging();

        if (logToFile != null)
        {
            logToFile.Close();
        }
    }

    public LogWriter GetLogWriter(bool autostart = false)
    {
        if (!loggingStarted && autostart)
            StartLogging();

        return logToFile;
    }

    public virtual string GetFileName()
    {
        return filename;
    }

    public void LogToFile(Bundle b)
    {
        LogToFile(GetLogWriter(), b);
    }

    public void LoggablesToFile()
    {
        LoggablesToFile(GetLogWriter());
    }
}
