using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWalking : MonoBehaviour
{
    [SerializeField] public Transform pointParent;

    Transform[] points;

    [Header("If not randomizing")]
    public float speed = 1.0F;

    [Range(0.1f, 1)]
    public float startTurningBodyAt = 0.5f; 

    [Header("Deactivate when it reaches the end of the path [else it respawns]")]
    [SerializeField] private bool deactivateAtEnd = false;

    [Header("If not deactivating at end")]
    public int respawnStartPoint = 1;
    public int respawnEndPoint = 6;


    [Header("Randomizing Speed and settings")]

    [SerializeField]
    private bool randomizeSpeed;

    [SerializeField]
    private float randomSpeedMinimumRange = 1;

    [SerializeField]
    private float randomSpeedMaximumRange = 3;


    [Header("Randomizing Spawn Point [if disabled, will calculate the least distance to the nearest point]")]
    public bool randomizeSpawnPoints;
    public WalkingEnums.StartPoint startAt = WalkingEnums.StartPoint.ClosestPoint;

    private Transform currentStartMarker;
    private Transform currentEndMarker;


    float fractionOfJourney = 0;

    private Transform currentLookAtEnd;


    private float startTime;

    private float journeyLength;

    float distCovered;

    int pathIndex = 0;


    private Transform cachedEndForFace;

    private float fakeFraction = 0.5f;

    [Header("Lanes")]
    public int currentLaneWalkingOn = 1;

    public int numberOfLanes = 1;

    public float distanceBetweenLanes = 0.5f;


    void Start()
    {
        currentLookAtEnd = new GameObject("CurrentLookAt").transform;

        Transform dumper;

        if (GameObject.Find("Dumper") == null)
        {
            GameObject gb = new GameObject("Dumper");
            dumper = gb.transform;
        }
        else
        {
            dumper = GameObject.Find("Dumper").transform;
        }

        if(dumper!=null)
            currentLookAtEnd.parent = dumper;

        points = pointParent.GetComponentsInChildren<Transform>();

        if (numberOfLanes > 1)
        {

        }

        startTime = Time.time;

        pathIndex = 0;

        if (randomizeSpeed)
        {
            speed = Random.Range(randomSpeedMinimumRange, randomSpeedMaximumRange);
        }

        if (startAt == WalkingEnums.StartPoint.Randomize)
        {
            pathIndex = Random.Range(1, points.Length - 3);
            currentStartMarker = points[pathIndex];
            currentEndMarker = points[pathIndex + 1];
            cachedEndForFace = points[pathIndex + 2];
        }
        else if(startAt == WalkingEnums.StartPoint.ClosestPoint)
        {
            //Find the closest point to start from

            float leastDistanceYet = 10000;
            int pathIndexToAssign = 1;
            for(int i = 1; i < points.Length - 1; i++)
            {
                float temp = Vector3.Distance(transform.position, points[i].position);

                if(temp < leastDistanceYet)
                {
                    leastDistanceYet = Vector3.Distance(transform.position, points[i].position);
                    pathIndexToAssign = i;
                }
            }

            pathIndex = pathIndexToAssign;

            if(pathIndex > points.Length - 3)
            {
                pathIndex -= 3;
            }

            currentStartMarker = points[pathIndex]; 
            currentEndMarker = points[pathIndex + 1];
            cachedEndForFace = points[pathIndex + 2];
        }
        else if(startAt == WalkingEnums.StartPoint.WalkToClosest)
        {
            //Find the closest point to start from

            float leastDistanceYet = 10000;
            int pathIndexToAssign = 1;
            for (int i = 1; i < points.Length - 1; i++)
            {
                float temp = Vector3.Distance(transform.position, points[i].position);

                if (temp < leastDistanceYet)
                {
                    leastDistanceYet = Vector3.Distance(transform.position, points[i].position);
                    pathIndexToAssign = i;
                }
            }

            pathIndex = pathIndexToAssign - 1;

            GameObject gb = new GameObject("DummyWalkClosest" + gameObject.name);

            if (dumper != null)
            {
                gb.transform.parent = dumper;
            }

            gb.transform.position = transform.position;

            currentStartMarker = gb.transform;
            currentEndMarker = points[pathIndex + 1];
            cachedEndForFace = points[pathIndex + 2];

            if (pathIndex > points.Length - 3)
            {
                pathIndex -= 3;
            }
        }
                
        currentLookAtEnd.position = currentEndMarker.position;

        journeyLength = Vector3.Distance(currentStartMarker.position, currentEndMarker.position);
    }


    void Update()
    {
        if(distCovered >= journeyLength)
        {
            
            pathIndex++;
            if (pathIndex > (points.Length - 2))
            {
                if (deactivateAtEnd)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    if(respawnEndPoint+1 > points.Length - 1)
                    {
                        respawnEndPoint = points.Length - 1;
                    }
                    pathIndex = Random.Range(respawnStartPoint, respawnEndPoint);

                   

                    currentStartMarker = points[pathIndex];
                    currentEndMarker = points[pathIndex + 1];

                    currentLookAtEnd.position = currentEndMarker.position;
                }
            }
            else
            {
                currentStartMarker = points[pathIndex];
                currentEndMarker = points[pathIndex + 1];

                currentLookAtEnd.position = currentEndMarker.position;

            }

            journeyLength = Vector3.Distance(currentStartMarker.position, currentEndMarker.position);

            distCovered = 0;

            startTime = Time.time;    
        }

        distCovered = (Time.time - startTime) * speed;

        fractionOfJourney = distCovered / journeyLength;

        if(fractionOfJourney > startTurningBodyAt)
        {
            if(pathIndex + 2 < (points.Length - 1))
            {
                cachedEndForFace = points[pathIndex + 2];
            }
            else
            {
                cachedEndForFace = points[pathIndex + 1];
            }

            fakeFraction = fractionOfJourney.ScaleRange(0.5f, 1, 0, 1);
            //Debug.Log(fakeFraction);

            DoFaceLerp();
        }

        if(numberOfLanes > 1)
        {
            transform.position = Vector3.Lerp(currentStartMarker.position.PlusFloatX(distanceBetweenLanes * currentLaneWalkingOn), currentEndMarker.position.PlusFloatX(distanceBetweenLanes * currentLaneWalkingOn), fractionOfJourney);
            transform.LookAt(currentLookAtEnd.position.PlusFloatX(distanceBetweenLanes));
        }
        else
        {
            transform.position = Vector3.Lerp(currentStartMarker.position, currentEndMarker.position, fractionOfJourney);
            transform.LookAt(currentLookAtEnd);
        }

       
 
    }

    void DoFaceLerp()
    {
        currentLookAtEnd.position = HelperFunctions.EaseInLerp(currentLookAtEnd.position, cachedEndForFace.position, fakeFraction);
    }
}


