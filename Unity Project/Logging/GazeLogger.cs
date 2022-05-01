using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VET.Interactables.Gaze;

public class GazeLogger : MonoBehaviour {

    public GazeObject GazeObject;
    public float TimeGazedAt = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
        if (GazeObject.isBeingGazedAt)
            TimeGazedAt += Time.deltaTime;
	}

    public void ResetTimer()
    {
        TimeGazedAt = 0;
    }

    public float GetDurationGazedAt()
    {
        return TimeGazedAt;
    }
}
