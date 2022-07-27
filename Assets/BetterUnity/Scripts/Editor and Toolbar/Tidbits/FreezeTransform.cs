using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FreezeTransform : MonoBehaviour
{
    public enum TransformType { GlobalTransform, LocalTransform }
    public enum ToFreeze { FreezeTransform, FreezePosition, FreezeRotation }

    public TransformType transformType;
    public ToFreeze toFreeze;

    private TransformType transformTracker;
    private ToFreeze toFreezeTracker;

    private Vector3 freezePos;
    private Quaternion freezeRot;

    private Vector3 freezeLocalPos;
    private Quaternion freezeLocalRot;

    
    // Start is called before the first frame update
    void Start()
    {
        AssignFreezeValues();
    }

    void AssignFreezeValues()
    {
        freezePos = transform.position;
        freezeRot = transform.rotation;

        freezeLocalPos = transform.localPosition;
        freezeLocalRot = transform.localRotation;

        transformTracker = transformType;
        toFreezeTracker = toFreeze;
    }

    // Update is called once per frame
    void Update()
    {
        if(transformType != transformTracker || toFreeze != toFreezeTracker)
        {
            AssignFreezeValues();
        }

        if (transformType.ToString().Equals("GlobalTransform"))
        {
            if (toFreeze.ToString().Equals("FreezeTransform"))
            {
                transform.SetPositionAndRotation(freezePos, freezeRot);
            }
            else if (toFreeze.ToString().Equals("FreezePosition"))
            {
                transform.position = freezePos;
            }
            else
            {
                transform.rotation = freezeRot;
            }         
        }
        else
        {
            if (toFreeze.ToString().Equals("FreezeTransform"))
            {
                transform.localPosition = freezeLocalPos;
                transform.localRotation = freezeLocalRot;
            }
            else if (toFreeze.ToString().Equals("FreezePosition"))
            {
                transform.localPosition = freezeLocalPos;
            }
            else
            {
                transform.localRotation = freezeLocalRot;
            }
        }

    }
}
