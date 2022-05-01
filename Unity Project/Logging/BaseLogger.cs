using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BaseLogger : MonoBehaviour, Bundle.ILoggableBundle
{
    public List<Bundle.ILoggableBundle> loggables = new List<Bundle.ILoggableBundle>();

    protected bool loggingStarted = false;

    public virtual void StartLogging()
    {
        loggingStarted = true;
    }

    public virtual void StopLogging()
    {
        loggingStarted = false;
    }


    public virtual void OnApplicationQuit()
    {
        if (loggingStarted)
            StopLogging();
    }

    //public BaseLogger(string filename)
    //{
    //    this.filename = filename;
    //}

    public void AddLoggable(Bundle.ILoggableBundle loggable)
    {
        loggables.Add(loggable);
    }

    public Bundle GetLogBundle(string prefix = "")
    {
        Bundle b = new Bundle(prefix);

        foreach (Bundle.ILoggableBundle loggable in loggables)
        {
            b.Append(loggable.GetLogBundle());
        }

        return b;
    }

    public void LoggablesToFile(LogWriter file, bool ignoreLogStatus = false)
    {
        foreach (Bundle.ILoggableBundle loggable in loggables)
        {
            LogToFile(file, loggable.GetLogBundle(), ignoreLogStatus);
        }
    }

    public void LogToFile(string filename, List<Bundle> bundles, bool ignoreLogStatus = false)
    {
        LogWriter writer = new LogWriter(filename);
        foreach (Bundle b in bundles)
        {
            LogToFile(writer, b, ignoreLogStatus);
        }
        writer.Close();
    }

    public void LogToFile(string filename, Bundle b, bool ignoreLogStatus = false)
    {
        LogWriter writer = new LogWriter(filename);
        LogToFile(writer, b, ignoreLogStatus);
        writer.Close();
    }

    public void LogToFile(LogWriter file, List<Bundle> bundles, bool ignoreLogStatus = false)
    {
        foreach (Bundle b in bundles)
        {
            LogToFile(file, b, ignoreLogStatus);
        }
    }

    public void LogToFile(LogWriter file, Bundle b, bool ignoreLogStatus = false)
    {
        bool shouldLog = file != null;
        shouldLog &= ignoreLogStatus ? shouldLog : loggingStarted;

        if (shouldLog)
        {
            file.WriteBundle(b);
        }
    }
}
