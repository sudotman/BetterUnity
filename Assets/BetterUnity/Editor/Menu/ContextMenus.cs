using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ContextMenus : EditorWindow
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [MenuItem("GameObject/Better Unity/Solve Import", false,-15)]
    static void SolveImport(MenuCommand command)
    {
        GameObject obj = (GameObject)command.context;

        //obj.SetActive(false);

        Transform[] tempArray = obj.transform.GetComponentsInChildren<Transform>();

        for(int i = 1; i < tempArray.Length; i++)
        {
            if (!tempArray[i].GetComponent<MeshRenderer>() && !tempArray[i].GetComponent<SkinnedMeshRenderer>())
            {
                DestroyImmediate(tempArray[i].gameObject);
            }
        }
    }

    [MenuItem("GameObject/Better Unity/Move ATB",false,0)]
    static void MoveToTo(MenuCommand command)
    {
        if (Selection.objects.Length != 2)
        {
            Debug.LogError("Select TWO objects which you would want to move to each other. You have selected " + Selection.objects.Length + " object(s) right now.");
        }
        else
        {
            GameObject obj1 = (GameObject) Selection.objects[0];
            GameObject obj2 = (GameObject) Selection.objects[1];

            obj1.transform.position = obj2.transform.position;
        }
    }

    [MenuItem("GameObject/Better Unity/Move BTA", false,0)]
    static void MoveToTwo(MenuCommand command)
    {
        if (Selection.objects.Length > 2)
        {
            Debug.LogError("select two objects");
        }
        else
        {
            GameObject obj1 = (GameObject)Selection.objects[0];
            GameObject obj2 = (GameObject)Selection.objects[1];

            obj2.transform.position = obj1.transform.position;
        }
    }

 


    // Add a menu item called "usual player settings" to a Rigidbody's context menu.
    [MenuItem("CONTEXT/Rigidbody/BetterUnity/Usual Player Settings")]
    static void DoubleMass(MenuCommand command)
    {
        Rigidbody body = (Rigidbody)command.context;

        body.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;

        body.interpolation = RigidbodyInterpolation.Interpolate;

        Debug.Log("Set usual player settings of Rigidbody.");
    }


    [MenuItem("CONTEXT/AudioSource/BetterUnity/Play Source")]
    static void PlayAudio(MenuCommand command)
    {
        AudioSource src = (AudioSource)command.context;
        src.Play();
        //Debug.Log("Doubled Rigidbody's Mass to " + body.mass + " from Context Menu.");
        Debug.Log("Playing audio now.");
    }
 
    [MenuItem("CONTEXT/AudioSource/BetterUnity/Stop Source")]
    static void StopAudio(MenuCommand command)
    {
        AudioSource src = (AudioSource)command.context;
        src.Stop();
        //Debug.Log("Doubled Rigidbody's Mass to " + body.mass + " from Context Menu.");
        Debug.Log("Stopping audio now.");
    }

    [MenuItem("CONTEXT/AudioSource/BetterUnity/Pause this source")]
    static void PauseAudio(MenuCommand command)
    {
        AudioSource src = (AudioSource)command.context;
        src.Pause();
        //Debug.Log("Doubled Rigidbody's Mass to " + body.mass + " from Context Menu.");
        Debug.Log("Pausing audio now.");
    }

}
