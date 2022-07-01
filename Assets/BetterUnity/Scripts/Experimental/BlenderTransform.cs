using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlenderTest))]
public class BlenderTransform : Editor
{
    private Transform transform;
    //SerializedProperty lookAtPoint;


    void OnEnable()
    {
        //lookAtPoint = serializedObject.FindProperty("localPosition");
        transform = Selection.activeTransform;
    }
    void OnSceneGUI()
    {
        EditorGUILayout.LabelField("(Above this object)");

        Event e = Event.current;
        switch (e.type)
        {
            case EventType.KeyDown:
                {
                    if (Event.current.keyCode == (KeyCode.A))
                    {
                        //do something here
                        Debug.Log("test");

                        // EDIT  : this a the instruction to add. 
                        // You realy need it to avoid performance issues !
                        //e.Use();
                        // END EDIT
                    }
                    break;
                }
        }

        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
    }



    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //EditorGUILayout.PropertyField(lookAtPoint);
        serializedObject.ApplyModifiedProperties();
    }
}
