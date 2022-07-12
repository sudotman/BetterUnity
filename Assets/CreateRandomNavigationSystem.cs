using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateRandomNavigationSystem : MonoBehaviour
{
    [SerializeField] int amountOfPoints = 6;

    [SerializeField] int amountOfPeople = 6;

    [Header("Randomize spawn point or start from the closest point")]
    [SerializeField] bool randomizeStartPoint;

    [SerializeField] GameObject[] characters;

    [Header("Make sure the gameobject is empty because it will destroy everything below it.")]
    [InspectorButton("CreateNavigationSystem", 0)]
    public char createNavigationSystem;

    [InspectorButton("RandomizePoints", 0)]
    public char randomizePoints;

    [Header("Press again, if this doesnt clear at once")]
    [InspectorButton("CleanUp", 0)]
    public char cleanUpObject;

    GameObject pointsParent;
    GameObject peopleParent;

    private Transform[] points;


    [Header("prettiness")]
    [SerializeField] private Color pathColor = Color.red;
    [SerializeField] private Color individualPointsColor = Color.white;

    //34

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateNavigationSystem()
    {
        transform.DestroyChildrenEditor();

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
            gb.GetComponent<CustomWalking>().randomizeSpawnPoints = randomizeStartPoint;

            gb.GetComponent<CustomWalking>().pointParent = pointsParent.transform;
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
                    Gizmos.DrawLine(points[i].position, points[i + 1].position);
            }
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
        transform.DestroyChildrenEditor();
    }

    void RandomizePoints()
    {
        foreach (Transform child in pointsParent.transform)
        {
            child.transform.localPosition = new Vector3(Random.Range(-25, 25f), child.transform.localPosition.y, Random.Range(-25, 25f));    
        }
    }
}
