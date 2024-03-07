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

    private static new readonly Vector2Int minSize = new Vector2Int(450, 200);
    private static new readonly Vector2Int maxSize = new Vector2Int(1200, 200);

    [MenuItem("BetterUnity/Mesh Combine Wizard")]
    static void CreateWizard()
    {
        var wizard = DisplayWizard<MeshCombineWizard>("Mesh Combine Wizard");

        EditorWindow window = GetWindow<MeshCombineWizard>();
        window.minSize = minSize;
        window.maxSize = maxSize;

        // If there is selection, and the selection of one Scene object, auto-assign it
        var selectionObjects = Selection.objects;
        if (selectionObjects != null && selectionObjects.Length == 1)
        {
            var firstSelection = selectionObjects[0] as GameObject;
            if (firstSelection != null)
            {
                wizard.combineParent = firstSelection;
            }
        }
    }

    void OnWizardCreate()
    {
        // Verify there is existing object root, ptherwise bail.
        if (combineParent == null)
        {
            Debug.LogError("Mesh Combine Wizard: Parent of objects to combne not assigned. Operation cancelled.");
            return;
        }

        var assetFolderResultPath = Path.Combine("Assets/", resultPath ?? "");
        if (!Directory.Exists(assetFolderResultPath))
        {
            Debug.LogError("Mesh Combine Wizard: 'Result Path' does not exist. Specified path must exist as relative to Assets folder. Operation cancelled.");
            return;
        }

        // Remember the original position of the object. 
        // For the operation to work, the position must be temporarily set to (0,0,0).
        Vector3 originalPosition = combineParent.transform.position;
        combineParent.transform.position = Vector3.zero;

        // Locals
        Dictionary<Material, List<MeshFilter>> materialToMeshFilterList = new Dictionary<Material, List<MeshFilter>>();
        List<GameObject> combinedObjects = new List<GameObject>();

        MeshFilter[] meshFilters = combineParent.GetComponentsInChildren<MeshFilter>();

        // Go through all mesh filters and establish the mapping between the materials and all mesh filters using it.
        foreach (var meshFilter in meshFilters)
        {
            var meshRenderer = meshFilter.GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.LogWarning("Mesh Combine Wizard: The Mesh Filter on object " + meshFilter.name + " has no Mesh Renderer component attached. Skipping.");
                continue;
            }

            var materials = meshRenderer.sharedMaterials;
            if (materials == null)
            {
                Debug.LogWarning("Mesh Combine Wizard: The Mesh Renderer on object " + meshFilter.name + " has no material assigned. Skipping.");
                continue;
            }

            // If there are multiple materials on a single mesh, cancel.
            if (materials.Length > 1)
            {
                // Rollback: return the object to original position
                combineParent.transform.position = originalPosition;
                Debug.LogError("Mesh Combine Wizard: Objects with multiple materials on the same mesh are not supported. Create multiple meshes from this object's sub-meshes in an external 3D tool and assign separate materials to each. Operation cancelled.");
                return;
            }
            var material = materials[0];

            // Add material to mesh filter mapping to dictionary
            if (materialToMeshFilterList.ContainsKey(material)) materialToMeshFilterList[material].Add(meshFilter);
            else materialToMeshFilterList.Add(material, new List<MeshFilter>() { meshFilter });
        }

        // For each material, create a new merged object, in the scene and in the assets.
        foreach (var entry in materialToMeshFilterList)
        {
            List<MeshFilter> meshesWithSameMaterial = entry.Value;
            // Create a convenient material name
            string materialName = entry.Key.ToString().Split(' ')[0];

            CombineInstance[] combine = new CombineInstance[meshesWithSameMaterial.Count];
            for (int i = 0; i < meshesWithSameMaterial.Count; i++)
            {
                combine[i].mesh = meshesWithSameMaterial[i].sharedMesh;
                combine[i].transform = meshesWithSameMaterial[i].transform.localToWorldMatrix;
            }

            // Create a new mesh using the combined properties
            var format = is32bit ? IndexFormat.UInt32 : IndexFormat.UInt16;
            Mesh combinedMesh = new Mesh { indexFormat = format };
            combinedMesh.CombineMeshes(combine);


            // Create asset
            materialName += "_" + combinedMesh.GetInstanceID();
            AssetDatabase.CreateAsset(combinedMesh, Path.Combine(assetFolderResultPath, "CombinedMeshes_" + materialName + ".asset"));

            // Create game object
            string goName = (materialToMeshFilterList.Count > 1) ? "CombinedMeshes_" + materialName : "CombinedMeshes_" + combineParent.name;
            GameObject combinedObject = new GameObject(goName);
            var filter = combinedObject.AddComponent<MeshFilter>();
            filter.sharedMesh = combinedMesh;
            var renderer = combinedObject.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = entry.Key;
            combinedObjects.Add(combinedObject);
        }

        // If there was more than one material, and thus multiple GOs created, parent them and work with result
        GameObject resultGO = null;
        if (combinedObjects.Count > 1)
        {
            resultGO = new GameObject("CombinedMeshes_" + combineParent.name);
            foreach (var combinedObject in combinedObjects) combinedObject.transform.parent = resultGO.transform;
        }
        else
        {
            resultGO = combinedObjects[0];
        }

        // Create prefab
        var prefabPath = Path.Combine(assetFolderResultPath, resultGO.name + ".prefab");
        PrefabUtility.SaveAsPrefabAssetAndConnect(resultGO, prefabPath, InteractionMode.UserAction);

        // Disable the original and return both to original positions
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