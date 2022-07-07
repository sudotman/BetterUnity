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
