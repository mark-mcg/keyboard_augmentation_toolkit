using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsetLoggingDemo : MonoBehaviour
{
    private Vector3 startPos, startEuler;
    private bool logging = false;
    private float waitTime = 0.05f; // time to wait between writes to log file

    public void StartLogging()
    {
        logging = true;
        StartCoroutine(LogCoroutine());
    }

    public void StopLogging()
    {
        logging = false;
    }

    public void OnDestroy()
    {
        StopLogging();
    }

    public IEnumerator LogCoroutine()
    {
        startPos = Camera.main.transform.position;
        startEuler = Camera.main.transform.eulerAngles;

        while (logging)
        {
            yield return new WaitForSeconds(waitTime);

            // log the raw world position and angle, and the position/angle relative to start position
            string toLog = string.Format("{0}, {1}, {2}, {3}",
                Camera.main.transform.position.ToString("F4"),
                 Camera.main.transform.eulerAngles.ToString("F4"),
                  Camera.main.transform.position - startPos,
                   Camera.main.transform.eulerAngles - startEuler);

            // now record toLog somewhere
        }
    }
}
