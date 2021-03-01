using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Logger : MonoBehaviour {

    public GameObject[] toTrack;

    private string statefilename;
    private string eventFilename;
    private bool loggingActive = false;
    private FileStream f;

	// Use this for initialization
	void Start () {
        Directory.CreateDirectory(Application.dataPath + "/logs/");
        statefilename = Application.dataPath + "/logs/statelog-" + System.DateTime.Now.Ticks + ".csv";
        FileStream f = File.Create(statefilename);
        f.Close();

        eventFilename = Application.dataPath + "/logs/eventlog-" + System.DateTime.Now.Ticks + ".csv";
        f = File.Create(eventFilename);
        f.Close();

        string s = "TIME(s),";
        foreach(GameObject go in toTrack)
        {
            s += go.name + "-POSX," + go.name + "-POSY," + go.name + "-POSZ," + go.name + "-ROTX," + go.name + "-ROTY," + go.name + "-ROTZ,";
        }
        s += "\n";
        File.WriteAllText(statefilename, s);
        loggingActive = true;

        s = "TIME(s),EVENT_DATA,";
        s += "\n";
        File.WriteAllText(eventFilename, s);
        loggingActive = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (loggingActive)
        {
            string s = Time.time + ",";
            foreach (GameObject go in toTrack)
            {
                s +=  + go.transform.position.x + "," + go.transform.position.y + "," + go.transform.position.z + "," + go.transform.localEulerAngles.x + "," + go.transform.localEulerAngles.y + "," + go.transform.localEulerAngles.z + ",";
            }
            s += "\n";
            File.AppendAllText(statefilename, s);
        }
	}

    public void logEvent(string eventData)
    {
        if (loggingActive)
        {
            string s = Time.time + "," + eventData;
            s += "\n";
            File.AppendAllText(eventFilename, s);
        }
    }

    public string getPositionFileName()
    {
        return statefilename;
    }
}
