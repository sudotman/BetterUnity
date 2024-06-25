using UnityEngine;
using UnityEditor;
using static UnityEditor.SceneView;

[CustomEditor(typeof(Transform)), CanEditMultipleObjects]
public class BetterTransform : Editor
{
    // Colors and all
    private GUIStyle white;

    // Serialized Properties
    private SerializedObject m_object;
    private SerializedProperty m_Position;
    private SerializedProperty m_Rotation;
    private SerializedProperty m_Scale;

    private SerializedProperty m_test;
    private Transform myClass;

    private Transform transform;

    private bool locked;

    // Scale Thingies
    private bool lockScaleInternal;

    private float xy;
    private float xz;

    GenericMenu menu_visibility;
    GenericMenu menu_reset;
    GenericMenu menu_normalVisual;

    float maximumSliderSetting = 1;

    static float customScale = 0.0f;

    private string currentLocGlob = "Local";
    private bool currentlyLocBool = true;

    bool fetchedOnce = false;

    private bool[] freezeArray = new bool[6];

    private Mesh mesh;
    private MeshFilter mf;
    private Vector3[] verts;
    private Vector3[] normals;
    private float normalsLength = 1f;

    bool visualizeNormals = false;

    static GUIStyle sceneButtonStyle = new GUIStyle();

    bool viewVerticesInformation;

    float currentTotalVerts = 0.0f;
    float currentTotalTriangles = 0.0f;


    private void OnEnable()
    {
        m_object = new SerializedObject(target);

        m_Position = m_object.FindProperty("m_LocalPosition");
        m_Rotation = m_object.FindProperty("m_LocalRotation");
        m_Scale = m_object.FindProperty("m_LocalScale");

        m_test = m_object.FindProperty("m_LocalScale");


        transform = Selection.activeTransform;

        locked = false;

        menu_reset = new GenericMenu();
        menu_reset.AddItem(new GUIContent("Reset Position"), false, ResetScale, 1);
        menu_reset.AddItem(new GUIContent("Reset Rotation"), false, ResetScale, 2);
        menu_reset.AddItem(new GUIContent("Reset Scale"), false, ResetScale, 3);

        menu_visibility = new GenericMenu();
        menu_visibility.AddItem(new GUIContent("Make Object Visible"), false, ObjectVisiblity, 1);
        menu_visibility.AddItem(new GUIContent("Make Object Invisible"), false, ObjectVisiblity, 2);

        maximumSliderSetting = 10;

        for (int i = 0; i < 6; i++)
        {
            freezeArray[i] = false;
        }

        Transform trans = (Transform)target;
        if (trans.TryGetComponent<MeshFilter>(out mf))
        {
            mesh = mf.sharedMesh;
        }
        normalsLength = 0.5f;

        currentTotalVerts = EditorPrefs.GetFloat("TotalSceneVerts");
        currentTotalTriangles = EditorPrefs.GetFloat("TotalSceneTris");
    }

    void ResetScale(object parameter)
    {
        switch (parameter)
        {
            case 1:
                if (currentlyLocBool)
                {
                    Undo.RecordObject(transform, "Local Transform Before Resetting");
                    transform.localPosition = new Vector3(0, 0, 0);
                }
                else
                {
                    Undo.RecordObject(transform, "Transform Before Resetting");
                    transform.position = new Vector3(0, 0, 0);
                }

                break;
            case 2:

                if (currentlyLocBool)
                {
                    Undo.RecordObject(transform, "Local Rot Before Resetting");
                    transform.localRotation = Quaternion.identity;
                }
                else
                {
                    Undo.RecordObject(transform, "Rot Before Resetting");
                    transform.rotation = Quaternion.identity;
                }

                break;
            case 3:
                locked = false;
                transform.localScale = new Vector3(1, 1, 1);
                break;
        }
    }

    void ObjectVisiblity(object parameter)
    {
        switch (parameter)
        {
            case 1:
                MeshRenderer[] temp = transform.GetComponentsInChildren<MeshRenderer>();

                foreach (MeshRenderer mr in temp)
                {
                    Undo.RecordObject(mr, "Mesh Renderer before Visible");
                    mr.enabled = true;
                }

                break;
            case 2:
                MeshRenderer[] temp2 = transform.GetComponentsInChildren<MeshRenderer>();

                foreach (MeshRenderer mr in temp2)
                {
                    Undo.RecordObject(mr, "Mesh Renderer before Invisible");
                    mr.enabled = false;
                }
                break;
        }
    }

    void VisualizeNormals(object parameter)
    {
        visualizeNormals = !visualizeNormals;
    }

    void SampleFunction()
    {
        Debug.Log("Clicked sample function.");
    }

    public int FetchSceneYPosition(int buttonPosition)
    {
        return 5 * buttonPosition;
    }

    string individualMetrics;
    string entireSceneMetrics;


    private void OnSceneGUI()
    {
        if (!(Event.current.type == EventType.Repaint))
            return;

        if (viewVerticesInformation)
        {
            RepaintAll();
            // Calculate current zoom level
            if (mesh)
                individualMetrics = "Selected Object: \nVertices: " + mesh.vertexCount + ", Triangles: " + mesh.triangles.Length;
            else
                individualMetrics = "";

            entireSceneMetrics = "Entire Scene: \nVertices: " + currentTotalVerts + ", Triangles: " + currentTotalTriangles; 

            sceneButtonStyle.normal.textColor = Color.yellow;

            Handles.BeginGUI();
            if (GUI.Button(new Rect(5, FetchSceneYPosition(1), 100, sceneButtonStyle.lineHeight), entireSceneMetrics + "\n\n" + individualMetrics, sceneButtonStyle))
                SampleFunction();
            Handles.EndGUI();

        }

        if (visualizeNormals)
        {
            if (mesh == null)
            {
                Debug.LogWarning("Visualize Normals: There is no mesh attached to this game object.");
                return;
            }

            //the matrix calculation inspired by @mandarinx's gist
            Handles.matrix = mf.transform.localToWorldMatrix;
            Handles.color = Color.yellow;
            verts = mesh.vertices;
            normals = mesh.normals;
            int len = mesh.vertexCount;

            for (int i = 0; i < len; i++)
            {
                Handles.color = Color.yellow;

                if (normalsLength < 0)
                    Handles.color = Color.red;

                Handles.DrawLine(verts[i], verts[i] + normals[i] * normalsLength);

                if (i % 2 == 0)
                    Handles.color = Color.white;
                else
                    Handles.color = Color.red;

                Handles.DrawSolidDisc(verts[i], normals[i], normalsLength / 25);
            }
        }


    }

    public override void OnInspectorGUI()
    {
        if (m_object != null)
        {
            m_object.Update();

            GUILayout.BeginHorizontal();

            EditorGUILayout.HelpBox(new GUIContent(currentLocGlob));


            if (GUILayout.Button("Toggle"))
            {
                if (currentlyLocBool == true)
                {
                    currentLocGlob = "Global Transform";

                    EditorGUILayout.PropertyField(m_test, new GUIContent("test"));

                    currentlyLocBool = false;
                }
                else
                {
                    currentLocGlob = "Local Transform";

                    EditorGUILayout.PropertyField(m_Position, new GUIContent("Position", "Position of the GameObject"));
                    currentlyLocBool = true;
                }
            }

            GUILayout.EndHorizontal();

            if (currentlyLocBool == true)
            {
                currentLocGlob = "Local Transform";

                EditorGUILayout.PropertyField(m_Position, new GUIContent("Position", "Local Position of the GameObject"));

            }
            else
            {
                currentLocGlob = "Global Transform";

                myClass = target as Transform;

                myClass.position = EditorGUILayout.Vector3Field("Position", myClass.position);

            }

            //EditorGUILayout.InspectorTitlebar(true, target);

            //EditorGUILayout.DropdownButton()
            // EditorGUILayout.Popup(0,new string[3] { " lolol", "safdsf", "sds" });
            //EditorGUILayout.GradientField(new Gradient()); // make a script to quickly modify the gradient of a new gameobject


            //Fixing Gimbal Lock issues with the rotation
            EditorGUI.BeginChangeCheck();
            Quaternion currentRotation = ((Transform)target).localRotation;
            Vector3 currentEulerAngles = currentRotation.eulerAngles;

            Vector3 newEulerAngles = EditorGUILayout.Vector3Field("Rotation", currentEulerAngles);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(targets, "Change Rotation");

                // Calculate the delta change in rotation
                Vector3 deltaEulerAngles = newEulerAngles - currentEulerAngles;

                // Apply the change to the rotation
                Quaternion deltaRotation = Quaternion.Euler(deltaEulerAngles);
                Quaternion newRotation = deltaRotation * currentRotation;

                // Assign the new rotation
                foreach (var obj in targets)
                {
                    Transform transform = (Transform)obj;
                    transform.localRotation = newRotation;
                }
            }

            //Display global rotation data
            if (!currentlyLocBool)
            {
                Vector4 test = new Vector4(myClass.rotation.w, myClass.rotation.x, myClass.rotation.y, myClass.rotation.z);

                GUI.color = Color.yellow;
                EditorGUILayout.LabelField("Q (w,x,y,z) : " + test.ToString());

                GUI.color = Color.white;
            }

            EditorGUILayout.PropertyField(m_Scale, new GUIContent("Scale"));

            //Display global lossy scale data
            if (!currentlyLocBool)
            {
                Vector3 scaleLossy = transform.lossyScale;

                GUI.color = Color.yellow;
                EditorGUILayout.LabelField("Lossy Scale (x,y,z) : " + scaleLossy.ToString());

                GUI.color = Color.white;
            }

            SessionState.SetBool("scaleUniform", EditorGUILayout.ToggleLeft("Lock Scale Ratio", SessionState.GetBool("scaleUniform", false)));
            locked = SessionState.GetBool("scaleUniform", false);

            if (locked)
            {
                maximumSliderSetting = EditorGUILayout.FloatField("Upper Scale Limit:", maximumSliderSetting);

                float fetchValue = FetchOnce();

                if (fetchValue > maximumSliderSetting)
                {
                    Debug.LogWarning("Attempted to set scale's upper limit lower than the scale slider.");
                    maximumSliderSetting = fetchValue;
                }

                customScale = EditorGUILayout.Slider("Scale:", fetchValue, 0, maximumSliderSetting);
                Undo.RecordObject(transform, "Scaling undo");
            }

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
                GameObject newChild = new GameObject(transform.gameObject.name + "Parent");

                Undo.SetTransformParent(transform, newChild.transform, "Set new parent");
            }

            EditorGUILayout.Separator();

            EditorGUILayout.HelpBox(new GUIContent("Visualizers"));

            GUIStyle yellowButtonStyle = new GUIStyle("Button");
            yellowButtonStyle.normal.textColor = Color.yellow;

            if (visualizeNormals)
            {
                GUIStyle centeredHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
                centeredHelpBoxStyle.alignment = TextAnchor.MiddleCenter;
                GUI.color = Color.white;
                EditorGUILayout.HelpBox(new GUIContent("Visualizing Normals"));
                GUI.color = Color.yellow;

                if (EditorGUILayout.DropdownButton(new GUIContent("Stop Visualizing Normals"), FocusType.Keyboard,yellowButtonStyle))
                {
                    visualizeNormals = false;

                    //Changes camera mode to textured automatically - commented out since it might be useful to some.

                    //DrawCameraMode cameraMode = DrawCameraMode.Textured;
                    //SceneView.lastActiveSceneView.cameraMode = SceneView.GetBuiltinCameraMode(cameraMode);

                    SceneView.RepaintAll();
                }

                EditorGUILayout.LabelField("Visualizing normals right now.");

            }
            else
            {
                if (EditorGUILayout.DropdownButton(new GUIContent("Visualize Normals"), FocusType.Keyboard))
                {
                    //Switching to wireframe mode automatically when visualizing normals - commented for usefulness

                    //DrawCameraMode cameraMode = DrawCameraMode.Wireframe;
                    //SceneView.lastActiveSceneView.cameraMode = SceneView.GetBuiltinCameraMode(cameraMode);

                    visualizeNormals = true;
                    SceneView.RepaintAll();
                }
            }


            if (visualizeNormals)
            {
                GUI.color = Color.yellow;
                normalsLength = EditorGUILayout.FloatField(new GUIContent("Normals Length"), normalsLength);
                GUI.color = Color.white;
                EditorGUILayout.Separator();
            }

            viewVerticesInformation = SessionState.GetBool("viewVertices", false);

            if (viewVerticesInformation)
            {
                EditorGUILayout.Separator();

                EditorGUILayout.HelpBox(new GUIContent("Vertices Information"));
                GUI.color = Color.yellow;
                EditorGUILayout.LabelField("Watching vertices right now.");


                if (EditorGUILayout.DropdownButton(new GUIContent("Stop Viewing Vertices Information"), FocusType.Keyboard, yellowButtonStyle))
                {
                    viewVerticesInformation = false;
                    SessionState.SetBool("viewVertices", false);
                    SceneView.RepaintAll();   
                }
                if (EditorGUILayout.DropdownButton(new GUIContent("Refresh Scene Vertices"), FocusType.Keyboard, yellowButtonStyle))
                {
                    Renderer[] allRenderers = FindObjectsOfType<Renderer>();

                    foreach(Renderer renderer in allRenderers)
                    {
                        MeshFilter mr = renderer.GetComponent<MeshFilter>();

                        if (mr){
                            currentTotalVerts += mr.sharedMesh.vertexCount;
                            currentTotalTriangles += mr.sharedMesh.triangles.Length;
                        }
                        else
                        {
                            SkinnedMeshRenderer sr = (SkinnedMeshRenderer)renderer;
                            currentTotalVerts += sr.sharedMesh.vertexCount;
                            currentTotalTriangles += sr.sharedMesh.triangles.Length;
                        }
                    }

                    EditorPrefs.SetFloat("TotalSceneVerts", currentTotalVerts);
                    EditorPrefs.SetFloat("TotalSceneTris", currentTotalTriangles);

                    SceneView.RepaintAll();
                }

                GUI.color = Color.white; 
            }
            else
            {
                if (EditorGUILayout.DropdownButton(new GUIContent("View Vertices Information"), FocusType.Keyboard))
                {

                    SessionState.SetBool("viewVertices", true);

                    SceneView.RepaintAll();

                }
            }

            //EditorGUILayout.HelpBox(new GUIContent("Debug"));
            //if (EditorGUILayout.DropdownButton(new GUIContent("Force Refresh Scene"), FocusType.Keyboard))
            //{
            //    SceneView.RepaintAll();
            //}

            m_object.ApplyModifiedProperties();


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
                    {

                        xy = transform.localScale.y == 0 ? 1 : xy;
                        xz = transform.localScale.z == 0 ? 1 : xz;

                        
                        transform.localScale = new Vector3(customScale, transform.localScale.x * xy, transform.localScale.x * xz);

                    }
                    else
                    {
                        if (customScale != 0)
                            transform.localScale = new Vector3(customScale, transform.localScale.x * xy, transform.localScale.x * xz);
                        else
                            customScale = transform.localScale.x;
                    }

                }
                else if (!locked)
                {
                    lockScaleInternal = false;
                }
            }

        }

    }

    float FetchOnce()
    {
        if (!fetchedOnce)
        {
            fetchedOnce = true;
            return transform.localScale.x;
        }
        else
        {
            return customScale;
        }
    }

}

// This class stores all the scene overlays which are not toggled through the Inspector.
// Display a zoom indicator overlay on the 2D scene view.  Click it to reset to 100% zoom
[InitializeOnLoad]
public static class AutomaticSceneOveralys
{
    static GUIStyle buttonStyle = new GUIStyle();

    static AutomaticSceneOveralys()
    {
        //SceneView.duringSceneGui += OnSceneGUI;
        buttonStyle.normal.textColor = Color.white;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        if (sceneView.in2DMode)
        {
            // Calculate current zoom level
            float zoom = GetSceneViewHeight(sceneView) / (sceneView.camera.orthographicSize * 2f);

            // Display zoom indicator in top left corner; clicking the indicator resets to 100%
            Handles.BeginGUI();
            if (GUI.Button(new Rect(5, 5, 50, buttonStyle.lineHeight), $"{zoom * 100:N0}%", buttonStyle))
                SetSceneViewZoom(sceneView, 1f);
            Handles.EndGUI();
        }
    }

    static float GetSceneViewHeight(SceneView sceneView)
    {
        return 1;
    }

    static void SetSceneViewZoom(SceneView sceneView, float zoom)
    {
        
    }
}
