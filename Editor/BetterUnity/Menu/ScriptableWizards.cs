using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEngine.Application;
using static UnityEditor.AssetDatabase;
using System.IO;
using UnityEngine.Rendering;



public class RenameChildren : EditorWindow
{
    private static new readonly Vector2Int minSize = new Vector2Int(300, 100);
    private static new readonly Vector2Int maxSize = new Vector2Int(1200, 150);
    private string childrenPrefix;
    private int startIndex;
    [MenuItem("BetterUnity/Rename Suite", false, 11)]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<RenameChildren>();
        window.minSize = minSize;
        window.maxSize = maxSize;
        window.Repaint();
    }
    private void OnGUI()
    {
        childrenPrefix = EditorGUILayout.TextField("Children Prefix", childrenPrefix);
        startIndex = EditorGUILayout.IntField("Start Index", startIndex);
        if (GUILayout.Button("Rename Selection"))
        {
            //Debug.Log("being called");
            GameObject[] selectedObjects = Selection.gameObjects;
            for (int objectI = 0, i = startIndex; objectI < selectedObjects.Length; objectI++)
            {
                selectedObjects[objectI].name = $"{childrenPrefix}{i++}";
            }

        }
        if (GUILayout.Button("Rename Children of Selection"))
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            for (int objectI = 0; objectI < selectedObjects.Length; objectI++)
            {
                Transform selectedObjectT = selectedObjects[objectI].transform;
                for (int childI = 0, i = startIndex; childI < selectedObjectT.childCount; childI++) selectedObjectT.GetChild(childI).name = $"{childrenPrefix}{i++}";
            }
        }
    }
}

public class SelectAllGeneric : EditorWindow
{
    private static new readonly Vector2Int minSize = new Vector2Int(450, 150);
    private static new readonly Vector2Int maxSize = new Vector2Int(1200, 150);

    public GameObject selectedGameObject;
    public Component selectedComponent;

    private int selectedComponentIndex = 0;
    private List<Component> components;

    bool automaticallyGetComponent = true;
    bool selectGenericObjects = false;

    [MenuItem("BetterUnity/select all generic", false, 0)]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<SelectAllGeneric>("Automatic Selection Helper");
        window.minSize = minSize;
        window.maxSize = maxSize;
    }

    private void OnGUI()
    {

        GUILayout.Label("Select the GameObject that has the Component you wish to select all from.");

        GameObject parentToRecurseBelow = Selection.activeGameObject;
        selectedGameObject = EditorGUILayout.ObjectField("Component GameObject", selectedGameObject, typeof(GameObject), true) as GameObject;

        automaticallyGetComponent = EditorGUILayout.Toggle("Component from Heirarchy ", automaticallyGetComponent);
        if (automaticallyGetComponent)
        {
            selectedGameObject = Selection.activeGameObject;
        }


        if (selectedGameObject != null)
        {
            components = new List<Component>(selectedGameObject.GetComponents<Component>());
            List<string> componentNames = new List<string>();
            foreach (Component component in components)
            {
                componentNames.Add(component.GetType().Name);
            }
            EditorGUI.BeginChangeCheck();
            selectedComponentIndex = EditorGUILayout.Popup("Component", selectedComponentIndex, componentNames.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                selectedComponent = components[selectedComponentIndex];
            }
        }

        selectGenericObjects = EditorGUILayout.Toggle("Select Generic Objects", selectGenericObjects);

        if (GUILayout.Button("Select all under selected"))
        {
            if (selectedComponent != null && parentToRecurseBelow != null)
            {
                List<GameObject> selectedGameObjects = new List<GameObject>();
                List<Object> selectedObjects = new List<Object>();

                AddChildrenWithComponent(parentToRecurseBelow.transform, selectedGameObjects, selectedObjects, selectedComponent.GetType());

                if (selectGenericObjects)
                    Selection.objects = selectedObjects.ToArray();
                else
                    Selection.objects = selectedGameObjects.ToArray();
            }
            else
            {
                Debug.LogWarning("Please select a valid component and a valid gameobject in the heirarchy.");
            }
        }

    }

    // Recursive method to add all children with the specified component
    private void AddChildrenWithComponent(Transform parent, List<GameObject> selectedGameObjects, List<Object> selectedObjects, System.Type componentType)
    {
        foreach (Transform child in parent)
        {
            // Check if the child has the specified component attached
            Component childComponent = child.GetComponent(componentType);
            if (childComponent != null)
            {
                selectedGameObjects.Add(child.gameObject);
                selectedObjects.Add(child);
            }

            // Recursively call this method for all children of the current child
            AddChildrenWithComponent(child, selectedGameObjects, selectedObjects, componentType);
        }
    }
}

//Small modifications to @sirgu's wizard
public class MeshCombineWizard : ScriptableWizard
{
    public GameObject combineParent;
    public string resultPath = "";
    public bool is32bit = true;
    public bool generateSecondaryUVs = false;

    private static readonly Vector2Int minSize = new Vector2Int(700, 430);
    private static readonly Vector2Int maxSize = new Vector2Int(1200, 430);

    [MenuItem("BetterUnity/Mesh Combine Wizard")]
    static void CreateWizard()
    {
        MeshCombineWizard wizard = DisplayWizard<MeshCombineWizard>("Mesh Combine Wizard");
        EditorWindow window = GetWindow<MeshCombineWizard>();
        window.minSize = minSize;
        window.maxSize = maxSize;

        // If there is a single selected object, auto-assign it as combineParent
        if (Selection.activeGameObject != null)
        {
            wizard.combineParent = Selection.activeGameObject;
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Settings while combining meshes.", EditorStyles.boldLabel);

        GUILayout.Space(20);

        GUILayout.Label("Parent Object", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("The parent object should have the meshes, which we want to combine, as its children.");
        EditorGUILayout.LabelField("By default, the currently selected object in heirarchy will be assigned.");
        combineParent = EditorGUILayout.ObjectField("Parent Object:", combineParent, typeof(GameObject), true) as GameObject;

        GUILayout.Space(20);

        GUILayout.Label("Path Settings", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Keeping the path empty will create the combined mesh prefab in the root 'Assets' folder.");
        resultPath = EditorGUILayout.TextField("Result Path:", resultPath);

        GUILayout.Space(20);

        GUILayout.Label("Indices Settings", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Enable 32-bit index if the combined mesh has more than 65535 vertices. (to avoid scrambled meshes)");
        is32bit = EditorGUILayout.Toggle("Use 32-bit Index:", is32bit);

        GUILayout.Space(20);

        GUILayout.Label("Secondary UVs Settings", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("When you import a model, you can compute a lightmap UV for it using [[ModelImporter-generateSecondaryUV]]");
        EditorGUILayout.LabelField("or the Model Import Settings Inspector. This allows you to do the same to procedural meshes.");
        generateSecondaryUVs = EditorGUILayout.Toggle("Generate Secondary UVs:", generateSecondaryUVs);

        GUILayout.Space(20);

        if (GUILayout.Button("Combine Meshes"))
        {
            CombineMeshes();
        }
    }

    void CombineMeshes()
    {
        if (combineParent == null)
        {
            Debug.LogError("Mesh Combine Wizard: Parent object not assigned. Operation cancelled.");
            return;
        }

        string assetFolderResultPath = Path.Combine("Assets/", resultPath ?? "");
        if (!Directory.Exists(assetFolderResultPath))
        {
            Debug.LogError("Mesh Combine Wizard: Result path does not exist or is invalid. Operation cancelled.");
            return;
        }

        Vector3 originalPosition = combineParent.transform.position;
        combineParent.transform.position = Vector3.zero;

        Dictionary<Material, List<MeshFilter>> materialToMeshFilterList = new Dictionary<Material, List<MeshFilter>>();
        List<GameObject> combinedObjects = new List<GameObject>();

        MeshFilter[] meshFilters = combineParent.GetComponentsInChildren<MeshFilter>();

        foreach (var meshFilter in meshFilters)
        {
            MeshRenderer meshRenderer = meshFilter.GetComponent<MeshRenderer>();
            if (meshRenderer == null || meshRenderer.sharedMaterial == null)
            {
                Debug.LogWarning("Mesh Combine Wizard: Skipping mesh without renderer or material.");
                continue;
            }

            Material material = meshRenderer.sharedMaterial;

            if (materialToMeshFilterList.ContainsKey(material))
                materialToMeshFilterList[material].Add(meshFilter);
            else
                materialToMeshFilterList.Add(material, new List<MeshFilter>() { meshFilter });
        }

        foreach (var entry in materialToMeshFilterList)
        {
            List<MeshFilter> meshesWithSameMaterial = entry.Value;
            string materialName = entry.Key.name + "_" + entry.Key.GetInstanceID();

            CombineInstance[] combine = new CombineInstance[meshesWithSameMaterial.Count];
            for (int i = 0; i < meshesWithSameMaterial.Count; i++)
            {
                combine[i].mesh = meshesWithSameMaterial[i].sharedMesh;
                combine[i].transform = meshesWithSameMaterial[i].transform.localToWorldMatrix;
            }

            Mesh combinedMesh = new Mesh { indexFormat = is32bit ? IndexFormat.UInt32 : IndexFormat.UInt16 };
            combinedMesh.CombineMeshes(combine);

            //Generate Secondary UVs
            if (generateSecondaryUVs)
            {
                //Unity 2022 or later has a return code for generating secondary UVs. Uncomment this for 2022 or later - use this as is otherwise.
                //if (!UnityEditor.Unwrapping.GenerateSecondaryUVSet(combinedMesh))
                //{
                //    Debug.LogWarning("Mesh Combine Wizard: Could not generate secondary UVs. See https://docs.unity3d.com/2022.2/Documentation/ScriptReference/Unwrapping.GenerateSecondaryUVSet.html");
                //}
                UnityEditor.Unwrapping.GenerateSecondaryUVSet(combinedMesh);
            }

            string assetName = "CombinedMeshes_" + materialName;
            string assetPath = Path.Combine(assetFolderResultPath, assetName + ".asset");
            AssetDatabase.CreateAsset(combinedMesh, assetPath);

            GameObject combinedObject = new GameObject(assetName);
            MeshFilter filter = combinedObject.AddComponent<MeshFilter>();
            filter.sharedMesh = combinedMesh;
            MeshRenderer renderer = combinedObject.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = entry.Key;
            combinedObjects.Add(combinedObject);
        }

        GameObject resultGO = (combinedObjects.Count > 1) ?
            new GameObject("CombinedMeshes_" + combineParent.name) :
            combinedObjects[0];

        foreach (var combinedObject in combinedObjects)
            combinedObject.transform.parent = resultGO.transform;

        string prefabPath = Path.Combine(assetFolderResultPath, resultGO.name + ".prefab");
        PrefabUtility.SaveAsPrefabAssetAndConnect(resultGO, prefabPath, InteractionMode.UserAction);

        combineParent.SetActive(false);
        combineParent.transform.position = originalPosition;
        resultGO.transform.position = originalPosition;
    }
}




//Inspired by Jason Storey

public class DefaultSetup : EditorWindow
{
    private static readonly Vector2Int size = new Vector2Int(250, 100);
    private string childrenPrefix;
    private int startIndex;
    [MenuItem("BetterUnity/Setup Default Project..", false, 22)]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<DefaultSetup>();
        window.minSize = size;
        window.maxSize = size;
    }
    private void OnGUI()
    {


        EditorGUILayout.HelpBox("If possible, use this straight when you create a new project to avoid confusion with existing folders. (I have been confused a lot)", MessageType.Warning);

        if (GUILayout.Button("Create Default Settings"))
        {

            CustomCreateDirectory("Scripts", "Editor", "FBX Imports", "Materials", "Audio Clips", "Prefabs", "Presets", "Animations");

            Refresh();
        }
    }
    public static void CustomCreateDirectory(bool differentRoot, string root, params string[] folders)
    {
        var fullPath = Combine(dataPath, root);
        foreach (var newDirectory in folders)
            CreateDirectory(Combine(fullPath, newDirectory));
    }

    public static void CustomCreateDirectory(params string[] folders)
    {
        foreach (var newDirectory in folders)
            CreateDirectory(Combine(dataPath, newDirectory));
    }
}

public class SelectAllTag : ScriptableWizard
{
    [SerializeField]
    string tagName = "Player";

    [MenuItem("BetterUnity/Select all with/tag", false, 0)]
    static void SelectAllOfTag()
    {
        ScriptableWizard.DisplayWizard("Select all with tag..", typeof(SelectAllTag), "Make selection");
    }

    private void OnWizardCreate()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(tagName);
        Selection.objects = gos;
    }

}

public class SelectAllLayer : ScriptableWizard
{

    [SerializeField, Layer] int layer;

    [MenuItem("BetterUnity/Select all with/layer", false, 1)]
    static void SelectAllOfLayer()
    {
        ScriptableWizard.DisplayWizard("Select all with layer..", typeof(SelectAllLayer), "Make selection");
    }

    private void OnWizardCreate()
    {

        GameObject[] gos = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> temp = new List<GameObject>();

        foreach (GameObject gb in gos)
        {
            if (gb.layer == layer)
            {
                temp.Add(gb);
            }
        }
        if (temp.Count < 1)
        {
            Debug.LogWarning("No objects with the specified layer found.");
        }
        else
        {
            Selection.objects = temp.ToArray();
        }
    }
}

public class SelectAllName : ScriptableWizard
{

    [SerializeField] string objectName = "Cube";

    [MenuItem("BetterUnity/Select all with/name", false, 2)]
    static void SelectAllOfName()
    {
        ScriptableWizard.DisplayWizard("Select all with name..", typeof(SelectAllName), "Make Selection");
    }

    private void OnWizardCreate()
    {
        GameObject[] gos = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> temp = new List<GameObject>();

        foreach (GameObject gb in gos)
        {
            if (gb.name.Contains(objectName))
            {
                temp.Add(gb);
            }
        }
        if (temp.Count < 1)
        {
            Debug.LogWarning("No objects with the specified name found.");
        }
        else
        {
            Selection.objects = temp.ToArray();
        }

    }
}
public class SelectAllMesh : ScriptableWizard
{

    [MenuItem("BetterUnity/Select all with/having mesh", false, 14)]
    static void SelectMeshes()
    {
        GameObject[] gos = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> temp = new List<GameObject>();

        foreach (GameObject gb in gos)
        {
            if (gb.GetComponent<MeshRenderer>() || gb.GetComponent<SkinnedMeshRenderer>())
            {
                temp.Add(gb);
            }
        }
        if (temp.Count < 1)
        {
            Debug.LogWarning("No objects with meshes found.");
        }
        else
        {
            Selection.objects = temp.ToArray();
        }

    }
}

public class SelectAllAudio : ScriptableWizard
{

    [MenuItem("BetterUnity/Select all with/having audio source", false, 15)]
    static void SelectAudios()
    {
        GameObject[] gos = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> temp = new List<GameObject>();

        foreach (GameObject gb in gos)
        {
            if (gb.GetComponent<AudioSource>())
            {
                temp.Add(gb);
            }
        }
        if (temp.Count < 1)
        {
            Debug.LogWarning("No objects with audio sources found.");
        }
        else
        {
            Selection.objects = temp.ToArray();
        }

    }
}

public class SelectAllCameras : ScriptableWizard
{

    [MenuItem("BetterUnity/Select all with/having cameras", false, 16)]
    static void SelectCameras()
    {
        GameObject[] gos = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> temp = new List<GameObject>();

        foreach (GameObject gb in gos)
        {
            if (gb.GetComponent<Camera>())
            {
                temp.Add(gb);
            }
        }
        if (temp.Count < 1)
        {
            Debug.LogWarning("No objects with cameras found.");
        }
        else
        {
            Selection.objects = temp.ToArray();
        }

    }
}

public class ReverseAnimation : Editor
{
    public static AnimationClip GetSelectedClip()
    {
        var clips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Assets);
        if (clips.Length > 0)
        {
            return clips[0] as AnimationClip;
        }
        return null;
    }

    [MenuItem("BetterUnity/Reverse Selected Animation")]
    public static void Reverse()
    {
        var clip = GetSelectedClip();
        if (clip == null)
            return;
        float clipLength = clip.length;

        List<AnimationCurve> curves = new List<AnimationCurve>();
        EditorCurveBinding[] editorCurveBindings = AnimationUtility.GetCurveBindings(clip);
        foreach (EditorCurveBinding i in editorCurveBindings)
        {
            var curve = AnimationUtility.GetEditorCurve(clip, i);
            curves.Add(curve);
        }

        clip.ClearCurves();
        for (int i = 0; i < curves.Count; i++)
        {
            var curve = curves[i];
            var binding = editorCurveBindings[i];

            var keys = curve.keys;
            int keyCount = keys.Length;
            var postWrapmode = curve.postWrapMode;
            curve.postWrapMode = curve.preWrapMode;
            curve.preWrapMode = postWrapmode;
            for (int j = 0; j < keyCount; j++)
            {
                Keyframe K = keys[j];
                K.time = clipLength - K.time;
                var tmp = -K.inTangent;
                K.inTangent = -K.outTangent;
                K.outTangent = tmp;
                keys[j] = K;
            }
            curve.keys = keys;
            clip.SetCurve(binding.path, binding.type, binding.propertyName, curve);
        }

        var events = AnimationUtility.GetAnimationEvents(clip);
        if (events.Length > 0)
        {
            for (int i = 0; i < events.Length; i++)
            {
                events[i].time = clipLength - events[i].time;
            }
            AnimationUtility.SetAnimationEvents(clip, events);
        }
        Debug.Log("Animation reversed!");
    }
}


//Deprecated Code - Preserved for a personal reason
/*
public class SelectAllGeneric : EditorWindow
{
    private static readonly Vector2Int size = new Vector2Int(350, 200);

    public MonoScript selectedType;

    public GameObject selectedGameObject;
    public Component selectedComponent;

    private int selectedComponentIndex = -1;
    private List<Component> components;

    [MenuItem("BetterUnity/select all generic", false, 0)]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<SelectAllGeneric>("Selector");
        window.minSize = size;
        window.maxSize = size;
    }

    private void OnEnable()
    {
    }

    private void OnGUI()
    {

        GUILayout.Label("Select the GameObject that has the Component you wish to select all from.");

        selectedGameObject = EditorGUILayout.ObjectField("GameObject", selectedGameObject, typeof(GameObject), true) as GameObject;

        if (selectedGameObject != null)
        {
            components = new List<Component>(selectedGameObject.GetComponents<Component>());
            List<string> componentNames = new List<string>();
            foreach (Component component in components)
            {
                componentNames.Add(component.GetType().Name);
            }
            EditorGUI.BeginChangeCheck();
            selectedComponentIndex = EditorGUILayout.Popup("Component", selectedComponentIndex, componentNames.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                selectedComponent = components[selectedComponentIndex];
            }
        }

        if (GUILayout.Button("Select GameObjects with Component"))
        {
            if (selectedComponent != null)
            {
                System.Type type = selectedComponent.GetType();

                Debug.Log(type);
                if (type != null && typeof(Component).IsAssignableFrom(type))
                {
                    Object[] objectArray = GameObject.FindObjectsByType(type, FindObjectsSortMode.None) as Object[];

                    List<GameObject> allGameObjects = new List<GameObject>();


                    foreach(Object obj in objectArray)
                    {
                        GameObject tempGO = ((Component)obj).gameObject;

                        Debug.Log(tempGO.GetInstanceID());

                        allGameObjects.Add(tempGO);
                    }

                    Selection.objects = allGameObjects.ToArray();
                }
                else
                {
                    Debug.LogWarning("Invalid component type selected.");
                }
            }
            else
            {
                Debug.LogWarning("Please select a valid component.");
            }
        }

    }

    private System.Type GetTypeFromMonoScript(MonoScript monoScript)
    {
        if (monoScript != null)
        {
            return monoScript.GetClass();
        }
        return null;
    }
}
*/