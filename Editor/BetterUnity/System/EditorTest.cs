using UnityEngine;
using UnityEditor;
public static class EditorTest
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
        Pivots[index] = scene.pivot;
        Rotations[index] = scene.rotation;
        Sizes[index] = scene.size;
    }
    static void RestoreCamera(int index)
    {
        var scene = SceneView.lastActiveSceneView;
        scene.pivot = Pivots[index];
        scene.rotation = Rotations[index];
        scene.size = Sizes[index];
        scene.Repaint();
    }
}