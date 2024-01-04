// Custom editor script for FlowController
using UnityEditor;

[CustomEditor(typeof(FlowController))]
public class FlowControllerGUI : Editor
{
    SerializedProperty dataArrayProp;
    SerializedProperty selectedStringProp;
    SerializedProperty selectedIndexProp;
    SerializedProperty dontStartAutomatically;
    SerializedProperty startWithDelay;

    void OnEnable()
    {
        dataArrayProp = serializedObject.FindProperty("eventsDividedWithStoryboard");
        selectedStringProp = serializedObject.FindProperty("selectedString");
        selectedIndexProp = serializedObject.FindProperty("selectedIndex");


        dontStartAutomatically = serializedObject.FindProperty("dontStartAutomatically");
        startWithDelay = serializedObject.FindProperty("startWithDelay");

        UpdateSelectedIndex();
    }

    void UpdateSelectedIndex()
    {
        if (dataArrayProp.arraySize > 0)
        {
            string selectedString = selectedStringProp.stringValue;
            for (int i = 0; i < dataArrayProp.arraySize; i++)
            {
                SerializedProperty elementProp = dataArrayProp.GetArrayElementAtIndex(i);
                SerializedProperty dataStringProp = elementProp.FindPropertyRelative("phaseName");
                if (dataStringProp.stringValue == selectedString)
                {
                    selectedIndexProp.intValue = i;
                    return;
                }
            }
        }
        selectedIndexProp.intValue = 0;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.HelpBox("This helps you schedule events - the events associated[0] is called instantly while the rest are called with a delay of 2 seconds.", MessageType.Info);

        EditorGUILayout.PropertyField(dataArrayProp, true);

        if (dataArrayProp.arraySize > 0)
        {
            string[] dataStrings = new string[dataArrayProp.arraySize];
            for (int i = 0; i < dataArrayProp.arraySize; i++)
            {
                SerializedProperty elementProp = dataArrayProp.GetArrayElementAtIndex(i);
                SerializedProperty dataStringProp = elementProp.FindPropertyRelative("phaseName");
                dataStrings[i] = dataStringProp.stringValue;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Begin the flow from:");

            selectedIndexProp.intValue = EditorGUILayout.Popup(selectedIndexProp.intValue, dataStrings);

            dontStartAutomatically.boolValue = EditorGUILayout.Toggle("Don't Start", dontStartAutomatically.boolValue);

            startWithDelay.floatValue = EditorGUILayout.FloatField("Start with delay?", startWithDelay.floatValue);

            if (selectedIndexProp.intValue >= 0 && selectedIndexProp.intValue < dataStrings.Length)
            {
                selectedStringProp.stringValue = dataStrings[selectedIndexProp.intValue];
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}