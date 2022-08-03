using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;
using UnityEngine.UI;

/// <summary>
/// Suceed this with a field where you specify the button's text as the variable name
/// </summary>
[System.AttributeUsage(System.AttributeTargets.All)]
public class InspectorTextAttribute : PropertyAttribute
{

    public static float defaultButtonWidth = 80;

    public float textWidth = defaultButtonWidth;

    public string text = "";

    public bool leftAlign = false;

    /// <summary>
    /// Create a inspector button with the button text being based on the suceeding variable's name.
    /// </summary>
    /// <param name="methodNamePassed">The name of the method to be called.</param>
    /// <returns>Creates a button in inspector.</returns>
    public InspectorTextAttribute(string methodNamePassed)
    {
        this.text = methodNamePassed;
        this.textWidth = text.Length;

        this.leftAlign = false;
    }

    /// <summary>
    /// Create a inspector button with the button text being based on the suceeding variable's name.
    /// </summary>
    /// <param name="methodNamePassed">The name of the method to be called.</param>
    /// <param name="leftAlign">Set true for the text to align to left.</param>
    /// <returns>Creates a button in inspector.</returns>
    public InspectorTextAttribute(string methodNamePassed, bool leftAlign)
    {
        this.text = methodNamePassed;
        this.textWidth = text.Length;

        this.leftAlign = leftAlign;
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

    public bool leftAlign = false;

    TextElement te = new TextElement();

    /// <summary>
    /// Create a inspector button with the button text being based on the suceeding variable's name.
    /// </summary>
    /// <param name="methodNamePassed">The name of the method to be called.</param>
    /// <returns>Creates a button in inspector.</returns>
    public InspectorFocusTextAttribute(string methodNamePassed)
    {
        this.text = methodNamePassed;

        this.textWidth = CalculateSizeAndReturn();

        this.leftAlign = false;
    }


    /// <summary>
    /// Create a inspector button with the button text being based on the suceeding variable's name.
    /// </summary>
    /// <param name="methodNamePassed">The name of the method to be called.</param>
    /// <param name="leftAlign">Set true for the text to align to left.</param>
    /// <returns>Creates a button in inspector.</returns>
    public InspectorFocusTextAttribute(string methodNamePassed, bool leftAlign)
    {
        this.text = methodNamePassed;

        this.textWidth = CalculateSizeAndReturn();

        this.leftAlign = leftAlign;
    }

    /// <summary>
    /// Create a inspector button with the button text being based on the suceeding variable's name.
    /// </summary>
    /// <param name="methodNamePassed">The name of the method to be called.</param>
    /// <param name="textWidth">Specify text width (if dynamic sizing breaks).</param>
    /// <returns>Creates a button in inspector.</returns>
    public InspectorFocusTextAttribute(string methodNamePassed, bool leftAlign, int textWidth)
    {
        this.text = methodNamePassed;

        this.textWidth = textWidth;

        this.leftAlign = leftAlign;
    }

    float CalculateSizeAndReturn()
    {
        // a very bandaidy fix / spent a lot of time trying to look for solutions and alternatives to this.
        // I tried using TextElement, TextGenerator, VisualElements but nothing worked sadly. I dont like doing this but this works accurately,
        // and I have spent the past few hours on such a menial problem so please enlighten me if possible.

        // doesn't work past 130 length of a string but how wide can an inspector really be? (i did bandaid fix previously but it looks cleaner
        // without it so again, please enlighten me.

        float add;
        if (text.Length >= 100)
            add = HelperFunctions.ScaleRange((float)text.Length, 100, 160, 0.9f, 0.85f);
        else if (text.Length >= 25)
            add = HelperFunctions.ScaleRange((float)text.Length, 25, 100, 0.95f, 0.9f);
        else if (text.Length < 9)
            add = 1.25f;
        else
            add = 1;

        float temp = text.Length * 7 * add;
        return temp;
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

        Rect buttonRect;

        if (InspectorTextAttribute.leftAlign)
        {
            buttonRect = new Rect(position.x, position.y, InspectorTextAttribute.textWidth, position.height + 5f);
        }
        else
        {
            buttonRect = new Rect(position.x, position.y, position.width,position.height);
        }

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

        Rect buttonRect;

        if (InspectorFocusTextAttribute.leftAlign)
        {
            buttonRect = new Rect(position.x, position.y, InspectorFocusTextAttribute.textWidth, position.height + 3f);

        }
        else
        {
            buttonRect = new Rect(position.x, position.y, position.width, position.height);
        }

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
