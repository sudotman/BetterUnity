using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

/// <summary>
/// Suceed this with a field where you specify the button's text as the variable name
/// </summary>
[System.AttributeUsage(System.AttributeTargets.All)]
public class InspectorTextAttribute : PropertyAttribute
{
    public static float defaultButtonWidth = 80;

    public float textWidth = defaultButtonWidth;

    public string text = "";

    /// <summary>
    /// Create a inspector button with the button text being based on the suceeding variable's name.
    /// </summary>
    /// <param name="methodNamePassed">The name of the method to be called.</param>
    /// <returns>Creates a button in inspector.</returns>
    public InspectorTextAttribute(string methodNamePassed)
    {
        this.text = methodNamePassed;
        this.textWidth = text.Length;
    }

}

/// <summary>
/// Suceed this with a field where you specify the button's text as the variable name
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field)]
public class InspectorFocusTextAttribute : PropertyAttribute
{
    public static float defaultButtonWidth = 80;

    public float textWidth = defaultButtonWidth;

    public string text = "";

    /// <summary>
    /// Create a inspector button with the button text being based on the suceeding variable's name.
    /// </summary>
    /// <param name="methodNamePassed">The name of the method to be called.</param>
    /// <returns>Creates a button in inspector.</returns>
    public InspectorFocusTextAttribute(string methodNamePassed)
    {
        this.text = methodNamePassed;
        this.textWidth = text.Length;

        Debug.Log(text.Length);


        //Its late and I am crying that this was the implementation I came up with. A non-linear remapping would work better
        if (text.Length < 10)
        {
            this.textWidth = HelperFunctions.ScaleRange(this.textWidth, 1, 100, 40, this.textWidth * HelperFunctions.ScaleRange(this.textWidth, 1, 9, 13, 9));
        }
        else if(text.Length >= 10)
        {
            this.textWidth = HelperFunctions.ScaleRange(this.textWidth, 10, 100, 80, this.textWidth * HelperFunctions.ScaleRange(this.textWidth, 10, 60, 20, 11));
        }
        else if (text.Length > 100)
        {
            this.textWidth = HelperFunctions.ScaleRange(this.textWidth, 1, 100, 80, this.textWidth * HelperFunctions.ScaleRange(this.textWidth, 100, 200, 15, 10));

        }

    }
}

/// <summary> Force a field to not be null by making it look red if it's somehow not assigned. </summary>
[System.AttributeUsage(System.AttributeTargets.Field)]
public class NullCheckAttribute : PropertyAttribute { }

// Custom Drawers :
// ---------------
// ***************
// ---------------

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(InspectorTextAttribute))]
public class InspectorTextPropertyDrawer : PropertyDrawer
{
    
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        GUI.color = Color.white;

        InspectorTextAttribute InspectorTextAttribute = (InspectorTextAttribute)attribute;

        InspectorTextAttribute.textWidth = HelperFunctions.ScaleRange(InspectorTextAttribute.text.Length, 1, InspectorTextAttribute.text.Length, 80, InspectorTextAttribute.text.Length * 8);
 
        Rect buttonRect = new Rect(position.x, position.y, InspectorTextAttribute.textWidth, position.height + 5f);

        //GUI.Box(buttonRect, InspectorTextAttribute.text);

        GUI.Label(buttonRect, InspectorTextAttribute.text);

    }
}

[CustomPropertyDrawer(typeof(InspectorFocusTextAttribute))]
public class InspectorFocusTextPropertyDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        GUI.color = Color.white;

        InspectorFocusTextAttribute InspectorFocusTextAttribute = (InspectorFocusTextAttribute)attribute;

        //InspectorFocusTextAttribute.textWidth = HelperFunctions.ScaleRange(InspectorFocusTextAttribute.text.Length, 1, InspectorFocusTextAttribute.text.Length, 80, InspectorFocusTextAttribute.text.Length * 8);


        Rect buttonRect = new Rect(position.x, position.y, InspectorFocusTextAttribute.textWidth, position.height + 3f);

        GUI.Box(buttonRect, InspectorFocusTextAttribute.text);
    }
}


//Null check // Paint the field red if not assigned
[CustomPropertyDrawer(typeof(NullCheckAttribute))]
public class NullCheckDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, prop);

        if(prop.objectReferenceValue == null)
        {
            label.text = "[null] " + label.text;
            GUI.color = Color.red;
        }

        EditorGUI.PropertyField(position, prop, label);

        GUI.color = Color.white;

        EditorGUI.EndProperty();
    }
}

#endif
