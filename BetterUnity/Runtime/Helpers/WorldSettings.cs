using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSettings : MonoBehaviour
{
    Transform[] all;

    [Header("Kill Y [kill z] Settings")]

    [Tooltip("The distance, below which, an object will automatically be destroyed. Akin to Unreal's KillZ")]
    [SerializeField] private float killY = -5000.0f;

    [Tooltip("The time/rate at which KillY is checked.")]
    [SerializeField] private int seconds = 5;

    [Header("Gravity Settings")]

    [Tooltip("Override World Gravity.")]
    [SerializeField] private bool overrideWorldGravity = false;
    [SerializeField] private float worldGravity = -1;

    [Header("Default Player/Camera")]
    [SerializeField] private bool setDefaultGameCamera = false;
    [SerializeField] private Camera defaultPlayerCam;


    public void Awake()
    {
        transform.position = Vector3.zero;

    }

    // Start is called before the first frame update
    void Start()
    {
        all = GameObject.FindObjectsOfType<Transform>();

        if(overrideWorldGravity)
            Physics.gravity = new Vector3(0, worldGravity, 0);

        if (defaultPlayerCam)
            defaultPlayerCam.depth = 10;

        InvokeRepeating("CheckForY", seconds, seconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckForY()
    {
        foreach(Transform obj in all)
        {
            if (obj != null)
            {
                if (obj.position.y < killY)
                {
                    Destroy(obj.gameObject);
                }
            }
            
        }
    }
}
