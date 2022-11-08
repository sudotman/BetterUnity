using UnityEngine;
using System.Collections;
using UnityEditor;

public class LogToScreen : MonoBehaviour
{
    [SerializeField]
    Color logColor = Color.yellow;

    [SerializeField]
    private bool displayFps = true;

    [SerializeField]
    Color fpsColor = Color.blue;

    string tempLogString;
    Queue logQueue = new Queue();   

    private GUIStyle defaultGUIStyle;

    private int currentFps;

    [SerializeField]
    private int removeLogsAfterTime = 6;

    [InspectorFocusText("Amount of logs after which previous ones get overwritten (-1 for unlimited)", true)]
    public char t;

    [SerializeField]
    private int maximumLogsAllowed = 21;

    [Header("Debug")]
    [SerializeField] bool randomLogsForTesting = false;

    void Start()
    {
        //Set the default style for our GUI elements
        defaultGUIStyle = new GUIStyle(EditorStyles.label);
        defaultGUIStyle.normal.textColor = Color.white; //We can multiply our elements with any color later to get our desired color

        if(displayFps)
            InvokeRepeating(nameof(RefreshFPS), 1, 1);

        if(randomLogsForTesting)
            RandomLogsForTesting();

        //remove debug elements after a certain time
        InvokeRepeating("RemoveInTime", 2, removeLogsAfterTime);
    }

    void RefreshFPS()
    {
        currentFps = (int)(1f / Time.unscaledDeltaTime);
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

        if(maximumLogsAllowed > 0)
        {
            if (logQueue.Count > maximumLogsAllowed)
            {
                logQueue.Dequeue();
            }
        }  
    }

    void LogTest()
    {
        Debug.Log("Current time: " + Time.time);
    }

    void OnGUI()
    {
        if(displayFps)
            GUI.contentColor = fpsColor;    
            GUILayout.Label(" FPS: " + currentFps.ToString());


        GUI.contentColor = logColor;

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

        

    }

}