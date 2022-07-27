using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RotationFreeze : MonoBehaviour
{
    [SerializeField] bool freezeX;
    [SerializeField] bool freezeY;
    [SerializeField] bool freezeZ;

    Quaternion rot;

    private void Awake()
    {
        rot = transform.rotation;
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (freezeX)
        {
            transform.rotation = new Quaternion(rot.x,transform.rotation.y,transform.rotation.z,transform.rotation.w);
        }

        if (freezeY)
        {
            transform.rotation = new Quaternion(transform.rotation.x, rot.y, transform.rotation.z, transform.rotation.w);
        }

        if (freezeZ)
        {
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, rot.z, transform.rotation.w);
        }
        
    }
}
