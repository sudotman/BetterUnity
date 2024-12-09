using UnityEngine;
using System.Collections;
using UnityEditor;

public class LogToScreen : MonoBehaviour
{
    [Header("Config")]

    [SerializeField]
    [Tooltip("Color of the logs")]
    Color logColor = Color.yellow;

    string tempLogString;
    Queue logQueue = new Queue();   

    private GUIStyle defaultGUIStyle;

    private int currentFps;

    [SerializeField]
    [Tooltip("TTL for any log")]
    private int removeLogsAfterTime = 6;

    [SerializeField]
    [Tooltip("Amount of logs after which previous ones get overwritten (-1 for unlimited)")]
    private int maximumLogsAllowed = 21;

    [Header("Key Toggle")]
    [Tooltip("If the logs should be toggleable with a key.")]
    public bool toggleWithKey = false;

    [Tooltip("Key to toggle with.")]
    public KeyCode keyCode = KeyCode.Tilde;

    private bool showRightNow = false;

    [Header("Skeletal FPS Counter")]
    [SerializeField]
    [Tooltip("If FPS should be displayed alongside the logs.")]
    private bool displayFps = true;

    [SerializeField]
    [Tooltip("Color of the FPS counter")]
    Color fpsColor = Color.blue;

    [Header("Debug")]
    [Tooltip("When the scene is started, a function will repeatedly throw logs out to test.")]
    [SerializeField] bool randomLogsForTesting = false;

    void Start()
    {
        //Set the default style for our GUI elements
        defaultGUIStyle = new GUIStyle(EditorStyles.label);
        defaultGUIStyle.normal.textColor = Color.white; // We can multiply our elements with any color later to get our desired color

        if(displayFps)
            InvokeRepeating(nameof(RefreshFPS), 1, 1);

        if(randomLogsForTesting)
            RandomLogsForTesting();

        // Remove debug elements after a certain time
        InvokeRepeating("RemoveInTime", 2, removeLogsAfterTime);

        // Start log disabled
        showRightNow = false;
    }

    void RefreshFPS()
    {
        currentFps = (int)(1f / Time.unscaledDeltaTime);
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            showRightNow = !showRightNow;
        }
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
        Debug.Log("Test 1:" + test1Counter + " and: " + timer);
    }

    void OnGUI()
    {
        if (toggleWithKey)
        {
            if(showRightNow)
                ProcessGUI();
        }
        else
        {
            ProcessGUI();
        }

        
    }

    void ProcessGUI()
    {
        if (displayFps)
        {
            GUI.contentColor = fpsColor;
            GUILayout.Label(" FPS: " + currentFps.ToString());
        }

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

    int test1Counter = 3;
    float timer = 4.0f;

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