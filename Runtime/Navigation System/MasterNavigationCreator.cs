using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class MasterNavigationCreator : MonoBehaviour
{
    [Header("General Settings // Creation | Non-Variables")]
    [SerializeField] int amountOfPoints = 6;

    [SerializeField] int amountOfPeople = 6;

   
    [InspectorFocusText("Variables (can be updated)",true)]

    [Tooltip("The number of lanes in our outlined path to walk on.")]
    [SerializeField] int numberOfLanes = 1;
    [SerializeField] float distanceBetweenLanes = 0.5f;
    [Tooltip("Randomly choose a lane everytime you play.")]

    [SerializeField] bool randomlyAssignLane = false;

    [Header("The startpoint for the characters")]
    [SerializeField] WalkingEnums.StartPoint startPoint;

    [SerializeField] GameObject[] characters;

    public enum AnimationState  { Walking, Running }

    [SerializeField]
    private AnimationState currentAnimationState;

    [Header("Will start looking at the new target when they are how much through the current two points' journey (0.0-1.0)")]
    [Range(0.2f, 1f)]
    public float startTurningBodyAt = 0.7f;

    [Header("Make sure the gameobject is empty because it will destroy everything below it.")]
    [InspectorButton("CreateNavigationSystem", 0)]
    public char createNavigationSystem;

    [InspectorButton("RandomizePoints", 0)]
    public char randomizePoints;

    [Header("Press again, if the previous existing system doesnt all clear at once")]
    [InspectorButton("CleanUp", 0)]
    public char cleanUpObject;

    [Header("Use this to update any variable changes without disturbing already placed paths/people")]
    [InspectorButton("UpdateVariables", 0)]
    public char updateVariables;

    [Tooltip("All variable changes will be reflected automatically.")]
    [SerializeField] private bool updateAutomatically = false;

    GameObject pointsParent;
    GameObject peopleParent;

    private Transform[] points;


    [Header("editor prettiness")]
    [SerializeField] private Color pathColor = Color.red;
    [SerializeField] private Color individualPointsColor = Color.white;

    //34

    // Start is called before the first frame update
    void Start()
    {
        
    }


    void CreateNavigationSystem()
    {
        transform.DestroyChildren();

        pointsParent = new GameObject("PointsParent");
        peopleParent = new GameObject("PeopleParent");

        pointsParent.transform.parent = transform;
        peopleParent.transform.parent = transform;

        pointsParent.transform.ResetLocalTransform();
        peopleParent.transform.ResetLocalTransform();

        pointsParent.AddComponent<BetterRename>();
        pointsParent.GetComponent<BetterRename>().prefix = "p";
        pointsParent.GetComponent<BetterRename>().startNumberingFrom = 1;

        for(int i = 0; i < amountOfPoints; i++)
        {
            GameObject gb = new GameObject();

            gb.transform.parent = pointsParent.transform;

            gb.transform.ResetLocalTransform();

            gb.transform.localPosition = new Vector3(gb.transform.localPosition.x + Random.Range(-25, 25f),gb.transform.localPosition.y,gb.transform.localPosition.z + Random.Range(-25, 25f));
        }

        //spawn people
        for (int i = 0; i < amountOfPeople; i++)
        {

            GameObject gb = Instantiate(characters[Random.Range(0, characters.Length - 1)]);

            gb.transform.parent = peopleParent.transform;

            gb.transform.ResetLocalTransform();

            gb.transform.localPosition = new Vector3(gb.transform.localPosition.x + Random.Range(-25, 25f), gb.transform.localPosition.y, gb.transform.localPosition.z + Random.Range(-25, 25f));

            gb.AddComponent<CustomWalking>();

            CustomWalking currentCW = gb.GetComponent<CustomWalking>();

            currentCW.startAt = startPoint;

            //Animation thingies / beautiful code btw satyam, whoa

            Animator animator = gb.GetComponent<Animator>();

            if (animator == null)
                Debug.LogError("Please make sure your prefab has an animator attached to it with proper animation states.");

            var controller = (AnimatorController)animator.runtimeAnimatorController;

            var newState = controller.layers[0].stateMachine.defaultState;  

            AnimationClip currentClip = null;

            foreach (AnimationClip clip in controller.animationClips)
            {
                if (clip.name == currentAnimationState.ToString())
                {
                    currentClip = clip;
                    controller.SetStateEffectiveMotion(newState, currentClip);
                }
            }

            currentCW.speed = currentAnimationState == AnimationState.Walking ? 1 : 2;

            if (currentClip == null)
            {
                Debug.LogError("Animation: " + currentAnimationState.ToString() + " doesnt exist in your people Animator. Please make sure it exists as a state [Remember, the clip name matters. Not the state name].");
            }

            //Animation Thingies end here

            //--------------------

            //Lane thingies start here
            currentCW.numberOfLanes = numberOfLanes;

            if(randomlyAssignLane)
                currentCW.currentLaneWalkingOn = Random.Range(0, numberOfLanes);
            currentCW.distanceBetweenLanes = distanceBetweenLanes;

            currentCW.startTurningBodyAt = startTurningBodyAt;

            currentCW.pointParent = pointsParent.transform;
        }
    }

    void UpdateVariables()
    {
        if(transform.childCount > 0)
        {
            pointsParent = transform.GetChild(0).gameObject;
            peopleParent = transform.GetChild(1).gameObject; 
            //spawn people
            for (int i = 0; i < peopleParent.transform.childCount; i++)
            {
                GameObject gb = peopleParent.transform.GetChild(i).gameObject;

                CustomWalking currentCW = gb.GetComponent<CustomWalking>();

                currentCW.startAt = startPoint;

                //Animation thingies / beautiful code btw satyam, whoa
                Animator animator = gb.GetComponent<Animator>();

                var controller = (AnimatorController)animator.runtimeAnimatorController;

                var newState = controller.layers[0].stateMachine.defaultState;

                AnimationClip currentClip = null;

                foreach (AnimationClip clip in controller.animationClips)
                {
                    if (clip.name == currentAnimationState.ToString())
                    {
                        currentClip = clip;
                        controller.SetStateEffectiveMotion(newState, currentClip);
                    }
                }

                currentCW.speed = currentAnimationState == AnimationState.Walking ? 1 : 2;

                if (currentClip == null)
                {
                    Debug.LogError("Animation: " + currentAnimationState.ToString() + " doesnt exist in your people Animator. Please make sure it exists as a state [Remember, the clip name matters. Not the state name].");
                }

                currentCW.numberOfLanes = numberOfLanes;

                if (randomlyAssignLane)
                    currentCW.currentLaneWalkingOn = Random.Range(0, numberOfLanes);

                currentCW.distanceBetweenLanes = distanceBetweenLanes;

                currentCW.startTurningBodyAt = startTurningBodyAt;

                currentCW.pointParent = pointsParent.transform;
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = individualPointsColor;

        if (pointsParent != null)
        {
            foreach (Transform child in pointsParent.transform)
            {
                Handles.Label(child.position, child.gameObject.name);
                Gizmos.DrawWireSphere(child.position, 0.6f);
            }

            Gizmos.color = pathColor;
            points = pointsParent.GetComponentsInChildren<Transform>();
       

            for (int i = 0; i < points.Length; i++)
            {
                if ((i + 1) < points.Length && i != 0)
                {
                    if (numberOfLanes > 1)
                    {
                        Gizmos.DrawLine(points[i].position, points[i + 1].position);

                        for(int j = 1; j < numberOfLanes; j++)
                        {
                            Gizmos.DrawLine(points[i].position.PlusFloatX(distanceBetweenLanes * j), points[i + 1].position.PlusFloatX(distanceBetweenLanes * j));
                        }
                        
                    }
                    else
                    {
                        Gizmos.DrawLine(points[i].position, points[i + 1].position);
                    }
                }
                    
            }

        }

    }

    private void OnValidate()
    {
        if (updateAutomatically)
        {
            UpdateVariables();
        } 
    }

    private void OnDrawGizmosSelected()
    {
        if(pointsParent == null || peopleParent == null)
        {
            if (transform.childCount > 1)
            {
                pointsParent = transform.GetChild(0).gameObject;
                peopleParent = transform.GetChild(1).gameObject;
            }
        }
    }

    void CleanUp()
    {
        transform.DestroyChildren();
    }

    void RandomizePoints()
    {
        if(transform.childCount > 0)
        {
            if(pointsParent == null)
            {
                pointsParent = transform.GetChild(0).gameObject;
            }

            foreach (Transform child in pointsParent.transform)
            {
                child.transform.localPosition = new Vector3(Random.Range(-25, 25f), child.transform.localPosition.y, Random.Range(-25, 25f));
            }
        }
        else
        {
            Debug.LogError("Make sure the points and people exist as children");
        }
    }
}
