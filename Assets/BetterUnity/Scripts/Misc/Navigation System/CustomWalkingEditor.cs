using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(CustomWalking)), CanEditMultipleObjects]
public class CustomWalkingEditor : Editor
{
    // Colors and all
    private GUIStyle white;

    private SerializedObject m_object;
    private SerializedProperty pointParent;

    private SerializedProperty randomizeSpeed;
    private SerializedProperty randomizeSpawnPoints;

    private Transform transform;

    private int startIndex;


#pragma warning disable IDE0052 // Remove unread private members
    private bool randomizeMasterBool;
#pragma warning restore IDE0052 // Remove unread private members

    // Scale Thingies
    private bool lockScaleInternal;

    private float xy;
    private float xz;

    GenericMenu menu_visibility;
    GenericMenu menu_reset;
    GenericMenu menu_freeze;

    private bool[] freezeArray = new bool[6];

    private void OnEnable()
    {

        m_object = new SerializedObject(target);

        pointParent = m_object.FindProperty("pointParent");
        randomizeSpeed = m_object.FindProperty("randomizeSpeed");
        randomizeSpawnPoints = m_object.FindProperty("randomizeSpawnPoints");

        transform = Selection.activeTransform;

        randomizeMasterBool = false;

        menu_reset = new GenericMenu();
        menu_reset.AddItem(new GUIContent("Randomize Everything"), false, RandomizeToggles, 1);
        menu_reset.AddItem(new GUIContent("Randomize Speed"), false, RandomizeToggles, 2);
        menu_reset.AddItem(new GUIContent("Randomize Spawn Points"), false, RandomizeToggles, 3);

        menu_freeze = new GenericMenu();

        for (int i = 0; i < 6; i++)
        {
            freezeArray[i] = false;
        }
    }

    void RandomizeToggles(object parameter)
    {
        switch (parameter)
        {
            case 1:
                //
                break;
            case 2:
                transform.localRotation = Quaternion.identity;
                break;
            case 3:
                randomizeMasterBool = false;
                transform.localScale = new Vector3(1, 1, 1);
                break;
        }
    }


    public override void OnInspectorGUI()
    {
        if (m_object != null)
        {
            m_object.Update();

            EditorGUILayout.HelpBox(new GUIContent("The parent containing the path of points to follow in order"));
            // EditorGUILayout.Popup(0,new string[3] { " lolol", "safdsf", "sds" });
            //EditorGUILayout.GradientField(new Gradient()); // make a script to quickly modify the gradient of a new gameobject


            EditorGUILayout.PropertyField(pointParent, new GUIContent("Points Parent", "Position of the GameObject"));

            EditorGUILayout.PropertyField(randomizeSpeed, new GUIContent("Randomize Speed"));

            EditorGUILayout.PropertyField(randomizeSpawnPoints, new GUIContent("Randomize Spawn Points"));


            EditorGUILayout.Separator();

            EditorGUILayout.HelpBox(new GUIContent("Tools"));

            if (EditorGUILayout.DropdownButton(new GUIContent("Randomization Settings"), FocusType.Keyboard))
            {
                menu_reset.ShowAsContext();
            }

            m_object.ApplyModifiedProperties();

        }

    }
}
