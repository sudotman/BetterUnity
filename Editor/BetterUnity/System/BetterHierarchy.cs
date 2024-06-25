using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class BetterHierarchy
{
    private const float ToggleWidth = 14f;
    private const float ToggleHeight = 14f;
    private const float ToggleOffsetX = 26.6f;
    private const float defaultToggles = 40f;

    private static GameObject hoveredGameObject;

    static GameObject selectedGameObject;

    static BetterHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        EditorApplication.hierarchyChanged += HierarchyWindowChanged;
        Selection.selectionChanged += OnSelectionChanged;
    }

    private static void HierarchyWindowChanged()
    {
        hoveredGameObject = null;
    }

    private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        selectionRect = DrawToggle(instanceID, selectionRect);

        DrawParentSelector(instanceID, selectionRect);

        DrawFolders(instanceID, selectionRect);
    }

    static void OnSelectionChanged()
    {
        selectedGameObject = Selection.activeGameObject;
    }

    private static Rect DrawToggle(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (gameObject != null)
        {
            Event e = Event.current;
            if (e != null && e.type == EventType.Repaint)
            {
                selectionRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.height);
                if (new Rect(selectionRect.x - ToggleWidth - defaultToggles, selectionRect.y, selectionRect.width, selectionRect.height).Contains(e.mousePosition))
                {
                    hoveredGameObject = gameObject;
                }
                else
                {
                    gameObject = null;
                }
            }

            if (hoveredGameObject == gameObject)
            {
                // Calculate the position for the toggle box
                float togglePosX = selectionRect.x - ToggleOffsetX;
                Rect toggleRect = new Rect(togglePosX, selectionRect.y + 1, ToggleWidth, ToggleHeight);

                // Draw a toggle box for the hovered GameObject
                Color backgroundColor = GUI.backgroundColor;
                GUI.backgroundColor = new Color(1f, 1f, 1f, 0.3f);

                if (toggleRect.Contains(e.mousePosition))
                    GUI.backgroundColor = new Color(1f, 1f, 1f, 1f);


                if (gameObject)
                    gameObject.SetActive(GUI.Toggle(toggleRect, gameObject.activeSelf, ""));

                GUI.backgroundColor = backgroundColor;
            }

        }

        return selectionRect;
    }

    private static void DrawParentSelector(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject != null && selectedGameObject != null && selectedGameObject.transform.parent != null)
        {
            if (gameObject == selectedGameObject)
            {
                Rect buttonRect = new Rect(selectionRect);
                buttonRect.x = buttonRect.xMax - 60; // Draw the button at the far right

                // Select Parent
                int lengthOfTextVisible = 8;
                float textOpacity = 0.7f;

                //Included just in case the appearance of the parent select button is to be changed.
                GUIStyle selectParentStyle = new GUIStyle(EditorStyles.foldout);

                string elementName = selectedGameObject.transform.parent.name;
                elementName = elementName.Length > lengthOfTextVisible ? elementName.Substring(0, lengthOfTextVisible) + "..." : elementName;

                Color backgroundColor = GUI.backgroundColor;
                GUI.backgroundColor = new Color(1, 1, 1, textOpacity);

                if (GUI.Button(buttonRect, elementName, selectParentStyle))
                {
                    Selection.activeGameObject = selectedGameObject.transform.parent.gameObject;
                }

                GUI.backgroundColor = backgroundColor;
            }
        }
    }

    private static void DrawFolders(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (gameObject != null && gameObject.name.StartsWith("="))
        {
            bool currentlyActive = gameObject.activeInHierarchy;
            EditorGUI.DrawRect(selectionRect, currentlyActive ? new Color(0.15f,0.15f,0.15f,1f) : new Color(0.2f, 0.2f, 0.2f, 1f));
            GUIStyle style = new GUIStyle(EditorStyles.whiteLabel);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 12; // Adjust font size as needed
            
            // Remove the preceding "= " from the name
            string displayName = gameObject.name.Substring(2);

            if (!currentlyActive)
            {
                displayName = "[~] " + displayName;
                style.normal.textColor = new Color(0.7f, 0.7f, 0.7f, 0.7f);
            }
             
            EditorGUI.LabelField(selectionRect, displayName, style);
        }
    }

}

