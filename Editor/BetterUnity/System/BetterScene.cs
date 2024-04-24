using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class BetterScene
{
    [MenuItem("BetterUnity/BetterScene/GetCameraPivot _1")]
    static void GetSceneCameraLocation()
    {
        Debug.Log("Teleported to position 1.");
        RestoreCamera(0);
    }
    [MenuItem("BetterUnity/BetterScene/SetCameraPivot %1")]
    static void SetSceneCameraLocation()
    {
        Debug.Log("You set the camera pivot to 1.");
        StoreCamera(0);
    }

    [MenuItem("BetterUnity/BetterScene/GetCameraPivot2 _2")]
    static void GetSceneCameraLocation2()
    {
        Debug.Log("Teleported to position 2.");
        RestoreCamera(1);
    }
    [MenuItem("BetterUnity/BetterScene/SetCameraPivot2 %2")]
    static void SetSceneCameraLocation2()
    {
        Debug.Log("You set the camera pivot to 2.");
        StoreCamera(1);
    }

    [MenuItem("BetterUnity/BetterScene/GetCameraPivot3 _3")]
    static void GetSceneCameraLocation3()
    {
        Debug.Log("Teleported to position 3.");
        RestoreCamera(2);
    }
    [MenuItem("BetterUnity/BetterScene/SetCameraPivot3 %3")]
    static void SetSceneCameraLocation3()
    {
        Debug.Log("You set the camera pivot to 3.");
        StoreCamera(2);
    }


    static Vector3[] Pivots = { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
    static Quaternion[] Rotations = { Quaternion.identity, Quaternion.identity, Quaternion.identity, Quaternion.identity };
    static float[] Sizes = { 1f, 1f, 1f, 1f };
    static void StoreCamera(int index)
    {
        var scene = SceneView.lastActiveSceneView;

        EditorPrefs.SetString("pivot" + index, scene.pivot.ToString());
        Pivots[index] = scene.pivot; 
        EditorPrefs.SetString("rotation" + index, scene.rotation.ToString());
        Rotations[index] = scene.rotation;
        EditorPrefs.SetString("size" + index, scene.size.ToString());
        Sizes[index] = scene.size;
    }

    static void RestoreCamera(int index)
    {
        var scene = SceneView.lastActiveSceneView;

        Pivots[index] = (EditorPrefs.GetString("pivot" + index,"(0.00,0.00,0.00)")).StringToVector3();
        Rotations[index] = (EditorPrefs.GetString("rotation" + index, "(0.00,0.00,0.00,1)")).StringToQuaternion();
        Sizes[index] = float.Parse(EditorPrefs.GetString("size" + index, "1f"));

        scene.pivot = Pivots[index];
        scene.rotation = Rotations[index];
        scene.size = Sizes[index];
        scene.Repaint();
    }
}