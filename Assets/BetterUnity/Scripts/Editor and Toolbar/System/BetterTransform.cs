using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Transform)), CanEditMultipleObjects]
public class BetterTransform : Editor
{
    // Colors and all
    private GUIStyle white;
    
    private SerializedObject m_object;
    private SerializedProperty m_Position;
    private SerializedProperty m_Rotation;
    private SerializedProperty m_Scale;

    private Transform transform;

    private int startIndex;

    private bool locked;

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
        //GUI Style gets multiplied by the GUI content color so lets do this
        //white = new GUIStyle(EditorStyles.label);
        //white.normal.textColor = Color.white;

        m_object = new SerializedObject(target);

        m_Position = m_object.FindProperty("m_LocalPosition");
        m_Rotation  = m_object.FindProperty("m_LocalRotation");
        m_Scale  = m_object.FindProperty("m_LocalScale");

        transform = Selection.activeTransform;

        locked = false;

        menu_reset = new GenericMenu();
        menu_reset.AddItem(new GUIContent("Reset Position"), false, ResetScale, 1);
        menu_reset.AddItem(new GUIContent("Reset Rotation"), false, ResetScale, 2);
        menu_reset.AddItem(new GUIContent("Reset Scale"), false, ResetScale, 3);

        menu_visibility = new GenericMenu();
        menu_visibility.AddItem(new GUIContent("Make Object Visible"), false, ObjectVisiblity, 1);
        menu_visibility.AddItem(new GUIContent("Make Object Invisible"), false, ObjectVisiblity, 2);

        menu_freeze = new GenericMenu();

        for(int i = 0; i < 6; i++)
        {
            freezeArray[i] = false;
        }
    }

    void ResetScale(object parameter)
    {
        switch (parameter)
        {
            case 1:
                transform.localPosition = new Vector3(0, 0, 0);
                break;
            case 2:
                transform.localRotation = Quaternion.identity;
                break;
            case 3:
                locked = false;
                transform.localScale = new Vector3(1,1,1);
                break;
        }   
    }

    void ObjectVisiblity(object parameter)
    {
        switch (parameter)
        {
            case 1:
                MeshRenderer[] temp = transform.GetComponentsInChildren<MeshRenderer>();

                foreach(MeshRenderer mr in temp)
                {
                    mr.enabled = true;
                }

                break;
            case 2:
                MeshRenderer[] temp2 = transform.GetComponentsInChildren<MeshRenderer>();

                foreach (MeshRenderer mr in temp2)
                {
                    mr.enabled = false;
                }
                break;
        }
    }

    public override void OnInspectorGUI()
    {
        if (m_object != null)
        {
            m_object.Update();

            EditorGUILayout.HelpBox(new GUIContent("Local Transform"));
            // EditorGUILayout.Popup(0,new string[3] { " lolol", "safdsf", "sds" });
            //EditorGUILayout.GradientField(new Gradient()); // make a script to quickly modify the gradient of a new gameobject


            EditorGUILayout.PropertyField(m_Position, new GUIContent("Position", "Position of the GameObject"));

            EditorGUILayout.PropertyField(m_Rotation, new GUIContent("Rotation"));

            EditorGUILayout.PropertyField(m_Scale, new GUIContent("Scale"));

            //GUI.contentColor = Color.yellow;
            locked = EditorGUILayout.ToggleLeft("Lock Scale Ratio", locked);

            EditorGUILayout.Separator();

            EditorGUILayout.HelpBox(new GUIContent("Tools"));

            if (EditorGUILayout.DropdownButton(new GUIContent("Reset Transform"), FocusType.Keyboard))
            {
                menu_reset.ShowAsContext();
            }

            if (EditorGUILayout.DropdownButton(new GUIContent("Visbility"), FocusType.Keyboard))
            {
                menu_visibility.ShowAsContext();
            }

            if (EditorGUILayout.DropdownButton(new GUIContent("Make Unit Scale Parent"), FocusType.Keyboard))
            {
                Debug.Log("pressed");

                GameObject newChild = new GameObject("test");
                //newChild.transform.loca

                //newChild.transform.parent = transform;

                transform.parent = newChild.transform;

                newChild.name = transform.gameObject.name + "Parent";
            }

          
            // set the GUI to use the color stored in m_Color
            //GUI.color = m_Color;

            m_object.ApplyModifiedProperties();

            //Debug.Log(Selection.activeContext);


            if (transform != null)
            {
                if (locked && !lockScaleInternal)
                {

                    xy = 1 / (transform.localScale.x / transform.localScale.y);
                    xz = 1 / (transform.localScale.x / transform.localScale.z);

                    lockScaleInternal = true;


                }
                else if (locked && lockScaleInternal)
                {

                    if (transform.localScale.x / transform.localScale.y != 1 / xy)
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x * xy, transform.localScale.x * xz);


                }
                else if (!locked)
                {
                    lockScaleInternal = false;
                }
            }

        }      

    }

  



 

}