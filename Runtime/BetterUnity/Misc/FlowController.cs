using System;
using UnityEngine;
using UnityEngine.Events;

public class FlowController : MonoBehaviour
{
    public StoryboardEvents[] eventsDividedWithStoryboard;

    public string selectedString;

    public int selectedIndex;

    public bool currentlyWaiting;
    public int currentlyWaitingForIndex;

    [Header("The event to start from - leave it blank to start from the beginning")]
    public string startFrom;

    public bool dontStartAutomatically;

    public bool[] genericSetOfBools = new bool[150];

    public float startWithDelay = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        genericSetOfBools = new bool[150];
        if (!dontStartAutomatically)
        {
            Invoke(nameof(BeginEvent),startWithDelay);
        }
    }

    void BeginEvent()
    {
        eventsDividedWithStoryboard[selectedIndex].eventsAssociated[0].Invoke();

        Debug.Log("Premature Playing: " + eventsDividedWithStoryboard[selectedIndex].phaseName);
        //Call waiting conditions
        if (eventsDividedWithStoryboard[selectedIndex].movingAheadRequirements.GetPersistentEventCount() > 0)
        {
            currentlyWaitingForIndex++;

            currentlyWaiting = true;

            eventsDividedWithStoryboard[selectedIndex].movingAheadRequirements.Invoke();

            Debug.Log("Playing: " + eventsDividedWithStoryboard[selectedIndex].phaseName);
        }

        //Call delayed events
        if (eventsDividedWithStoryboard[selectedIndex].eventsAssociated.Length > 1)
        {
            
            Invoke(nameof(ExecuteDelayedEventsAssociated), eventsDividedWithStoryboard[selectedIndex].lateEventDelay);
        }
            
    }
    
    public void ExecuteDelayedEventsAssociated()
    {
        Debug.Log("Delay executing.");

        eventsDividedWithStoryboard[selectedIndex].eventsAssociated[1].Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentlyWaiting)
        {
            if (genericSetOfBools[currentlyWaitingForIndex])
            {
                selectedIndex++;
                BeginEvent();
                currentlyWaiting = false;
                currentlyWaitingForIndex++;
            }
        }
    }

    //waiting for the condition to turn true for something or the other

    //setTheBoolToBeRelatedToPressingAButton - that button press is now utilized from the set of bools

    //currently waiting for index is checked repeatedly in update to see if the condition has turned true

    //if multiple conditions make up the condition to move ahead - instead use up logical gates inside the variables you're checking to keep the bool pipeline clean
    public void SampleWaitCondition()
    {
        genericSetOfBools[currentlyWaitingForIndex] = false;
    }
    public void MakeGenericBoolTrue()
    {
        currentlyWaiting = true;
        genericSetOfBools[currentlyWaitingForIndex] = true;
    }
}

[System.Serializable]
public class StoryboardEvents
{
    [Tooltip("Name for the sake of reference and also for dictionary based searching/indexing.")]
    public string phaseName;

    [Tooltip("For the sake of reference.")]
    public string phaseDescription;

    [Tooltip("Actual events called when this event is hit.")]
    public UnityEvent[] eventsAssociated = new UnityEvent[2];

    public UnityEvent movingAheadRequirements;

    public float lateEventDelay = 2f;

    StoryboardEvents()
    {
    }
}
