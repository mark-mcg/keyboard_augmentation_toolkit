using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Diagnostics;

/// <summary>
/// Polls the current active application in Windows, and can notify listeners when the
/// context has changed.
/// </summary>
public class ActiveApplicationMonitor : MonoBehaviour {

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    public string CurrentWindowTitle;
    public string CurrentWindowProcessName;

    public delegate void ContextChanged();
    public event ContextChanged OnContextChanged;

    public void Start()
    {
        PollWindow();
        InvokeRepeating("PollWindow", 0.0f, 0.2f);
        //Invoke("ChromeSite", 4.0f);
    }

    private string newProcess, newTitle;
    public void PollWindow()
    {
        //UnityEngine.Debug.Log("Active window is: " + GetActiveWindowTitle());
        //UnityEngine.Debug.Log("Active process is: " + GetActiveWindowProcess());
        newProcess = GetActiveWindowProcess();
        newTitle = GetActiveWindowTitle();

        if (CurrentWindowTitle != newTitle || CurrentWindowProcessName != newProcess)
        {
            CurrentWindowTitle = newTitle;
            CurrentWindowProcessName = newProcess;

            if (OnContextChanged != null)
                OnContextChanged();
        }
    }

    private string GetActiveWindowTitle()
    {
        const int nChars = 256;
        StringBuilder Buff = new StringBuilder(nChars);
        IntPtr handle = GetForegroundWindow();

        if (GetWindowText(handle, Buff, nChars) > 0)
        {
            return Buff.ToString();
        }
        return null;
    }

    private string GetActiveWindowProcess()
    {
        IntPtr handle = GetForegroundWindow();
        uint procId = 0;

        GetWindowThreadProcessId(handle, out procId);
        Process currentProc = Process.GetProcessById((int)procId);
        return currentProc.ProcessName;
    }
}
