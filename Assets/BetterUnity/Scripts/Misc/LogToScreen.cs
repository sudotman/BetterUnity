using UnityEngine;
using System.Collections;
using UnityEditor;

public class LogToScreen : MonoBehaviour
{
    string tempLogString;
    Queue logQueue = new Queue();   

    private GUIStyle defaultGUIStyle;

    void Start()
    {
        //Set the default style for our GUI elements
        defaultGUIStyle = new GUIStyle(EditorStyles.label);
        defaultGUIStyle.normal.textColor = Color.white; //We can multiply our elements with any color later to get our desired color
    }

  

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        tempLogString = logString;
        string newString = "\n [" + type + "] " + tempLogString;
        logQueue.Enqueue(newString);
        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            logQueue.Enqueue(newString);
        }
        tempLogString = string.Empty;
        foreach (string mylog in logQueue)
        {
            tempLogString += mylog;
        }

        if (logQueue.Count > 21)
        {
            logQueue.Dequeue();
        }
    }

    void LogTest()
    {
        Debug.Log("Current time: " + Time.time);
    }

    void OnGUI()
    {
        GUI.contentColor = Color.yellow;
        GUILayout.Label(tempLogString);
    }

    void RemoveInTime()
    {
        //Debug.Log("remove in time");
        if (logQueue.Count > 0)
        {
            logQueue.Dequeue();
        }
    }

    //IEnumerator removeInTime(string dequeue)
    //{
    //    yield return new WaitForSeconds(4f);
    //    myLogQueue.Dequeue();
    //}
    void RandomLogsForTesting()
    {
        Debug.Log("Log1");
        Debug.Log("Log2");
        Debug.Log("Log3");
        Debug.Log("Log4");

        Debug.LogError("This is an error plus time: " + Time.time);

        Debug.LogWarning("Warning plus position: " + transform.position);

        Debug.LogAssertion("Assertion plus time: " + Time.deltaTime);

        //a repeating function to test logging
        InvokeRepeating("LogTest", 1, 2);

        //remove debug elements after a certain time
        InvokeRepeating("RemoveInTime", 2, 5);

    }

}