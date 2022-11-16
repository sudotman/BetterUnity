using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BetterScale : MonoBehaviour
{
    [Tooltip("'Lock' the scaling ratio")]
    [SerializeField] private bool lockScale = true;

    private bool lockScaleInternal;

    private float xy;
    private float xz;


    // Update is called once per frame
    void Update()
    {
        if (lockScale && !lockScaleInternal)
        {
            xy = 1/(transform.localScale.x / transform.localScale.y);
            xz = 1/(transform.localScale.x / transform.localScale.z);

            lockScaleInternal = true;
        }
        else if(lockScale && lockScaleInternal)
        {
            if(transform.localScale.x/transform.localScale.y != 1/xy)
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x * xy, transform.localScale.x * xz);
        }
        else if (!lockScale)
        {
            lockScaleInternal = false;
        }

    }
}
