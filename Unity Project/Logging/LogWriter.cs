using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using UnityEngine;


/// <summary>
/// A Logging class implementing the Singleton pattern and an internal Queue to be flushed perdiodically
/// http://www.bondigeek.com/blog/2011/09/08/a-simple-c-thread-safe-logging-class/
/// </summary>
public class LogWriter
{
    public static bool LoggingEnabled = true;

    private Queue<String> logQueue;
    private string logDir = "";
    private string logFile = "";//DateTime.Today.ToString("d").Replace('/', '-');
    private int maxLogAge = 3;
    private int queueSize = 10;
    private DateTime LastFlushed = DateTime.Now;

    private FileStream fs;
    private StreamWriter log;
    private string lastFilename;
    Boolean openedFile = false;

    public static string GlobalLogFolder;

    /// <summary>
    /// Private constructor to prevent instance creation
    /// </summary>
    public LogWriter(string fileName)
    {
        lastFilename = fileName;
        int fileCount = 0;
        Boolean gotFile = false;

        if (GlobalLogFolder != null)
        {
            if (!Directory.Exists(GlobalLogFolder))
            {
                Directory.CreateDirectory(GlobalLogFolder);
            }
        }

        while (!gotFile)
        {
            try
            {
                if (GlobalLogFolder != null && GlobalLogFolder.Length > 0)
                    logFile += GlobalLogFolder + "//";
                logFile += DateTime.Now.ToString("yyyyMMdd--HH_mm_ss_ffftt") + "-" + fileName + "-" + fileCount + ".tsv";

                logFile = logFile.Replace(':', '-');

                if (File.Exists(logFile))
                    throw new Exception("File already exists");
                else
                {
                    fs = File.Open(logFile, FileMode.Append, FileAccess.Write);
                    log = new StreamWriter(fs);
                    log.AutoFlush = true;
                    logQueue = new Queue<String>();

                    gotFile = true;
                    openedFile = true;
                }

            }
            catch (Exception e)
            {
                Debug.LogError("Problem logging to file " + logFile + e.ToString());
                fileCount++;
                gotFile = false;
            }
        }
    }

    static Bundle toPrependGlobal;
    Bundle toPrepend;

    /// <summary>
    /// This bundle will be prepended to every row (e.g. condition data) for this log file
    /// </summary>
    /// <param name="prepend"></param>
    public void SetPrependBundle(Bundle prepend)
    {
        toPrepend = prepend;
    }

    /// <summary>
    /// This bundle will be prepended to every row (e.g. condition data) for all log files
    /// </summary>
    /// <param name="prepend"></param>
    public static void SetGlobalPrependBundle(Bundle prepend)
    {
        toPrependGlobal = prepend;
    }


    // TODO: Tidy this up, horribly hacky
    public void WriteBundle(Bundle b)
    {
        string headers = "";
        string towrite = "";

        if (toPrependGlobal != null)
        {
            headers += toPrependGlobal.GetHeaders();
            towrite += toPrependGlobal.GetContents();
        }

        if (toPrepend != null)
        {
            headers += toPrepend.GetHeaders();
            towrite += toPrepend.GetContents();
        }

        if (!hasHeaders)
            WriteHeaders(headers + b.GetHeaders());

        WriteToLog(towrite + b.GetContents());
    }

    public bool hasHeaders = false;
    public void WriteHeaders(string message)
    {
        hasHeaders = true;
        WriteToLog(message + "\tlogger.duration", false);
    }

    int lineCount = 0;
    float timeSinceLastRow = 0;

    private void WriteLastRowDuration()
    {
        if (lineCount > 0 && fs.CanWrite)
        {
            // write the duration of the last instance
            log.WriteLine("\t" + (Time.time - timeSinceLastRow));
        }
        lineCount++;
        timeSinceLastRow = Time.time;
    }

    /// <summary>
    /// The single instance method that writes to the log file
    /// </summary>
    /// <param name="message">The message to write to the log</param>
    public void WriteToLog(string message, bool rowduration = true)
    {
        // Lock the queue while writing to prevent contention for the log file
        lock (logQueue)
        {
            if (fs.CanWrite)
            {
                WriteLastRowDuration();

                // now write the new log row
                log.Write(string.Format("{0}\t{1}\t{2}", DateTime.Now.ToString("hh:mm:ss.fff tt"), Time.time, message));


                // Create the entry and push to the Queue
                //Log logEntry = new Log(message);
                //logQueue.Enqueue(logEntry);

                // If we have reached the Queue Size then flush the Queue
                //if (logQueue.Count >= queueSize || DoPeriodicFlush())
                //{
                //    FlushLog();
                //}
            }
        }
    }

    public void Close()
    {
        if (openedFile)
        {
            //Debug.Log("Close() called for " + lastFilename);
            WriteLastRowDuration();

            log.Flush();
            log.Close();
            fs.Close();
            fs.Dispose();
            openedFile = false;
        }
    }

    private bool DoPeriodicFlush()
    {
        TimeSpan logAge = DateTime.Now - LastFlushed;
        if (logAge.TotalSeconds >= maxLogAge)
        {
            LastFlushed = DateTime.Now;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Flushes the Queue to the physical log file
    /// </summary>
    public void FlushLog()
    {
        lock (logQueue)
        {
            while (logQueue.Count > 0)
            {
                //Log entry = logQueue.Dequeue();
                //log.WriteLine(string.Format("{0},\t{1}", entry.LogTime, entry.Message));
            }
        }
    }

    ~LogWriter()
    {
        FlushLog();
    }

}

/// <summary>
/// A Log class to store the message and the Date and Time the log entry was created
/// </summary>
//public class Log
//{
//    public string Message { get; set; }
//    public string LogTime { get; set; }
//    public string LogDate { get; set; }

//    public Log(string message)
//    {
//        Message = message;
//        LogDate = DateTime.Now.ToString("yyyy-MM-dd");
//        LogTime = DateTime.Now.ToString("hh:mm:ss.fff tt");
//    }
//}