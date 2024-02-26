using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LinearController : MonoBehaviour
{
    public LinearEvents[] linearEventsToBeExecuted;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}


[System.Serializable]
public class LinearEvents
{
    [Tooltip("Name for the sake of reference and also for dictionary based searching/indexing.")]
    public string eventName;

    [Tooltip("Time to call the event. Can be implemented as absolute time but I prefer this as an additional way of indexing and calling the functions manually.")]
    public float timeToCallAt;

    [Tooltip("Actual events called when this event is hit.")]
    public UnityEvent eventsToBeCalled;

    LinearEvents()
    {

    }
}
