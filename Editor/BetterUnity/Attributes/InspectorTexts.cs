using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;
using UnityEngine.UI;

/// <summary>
/// Insert a simple text label in the inspector
/// </summary>
[System.AttributeUsage(System.AttributeTargets.All)]
public class LabelAttribute : PropertyAttribute
{

    public static float defaultButtonWidth = 80;

    public float textWidth = defaultButtonWidth;

    public string text = "";

    public bool leftAlign = false;

    /// <summary>
    /// Creates a simple non-editable text field in the inspector.
    /// </summary>
    /// <param name="label">The text to display.</param>
    public LabelAttribute(string label)
    {
        this.text = label;
        this.textWidth = text.Length;

        this.leftAlign = false;
    }

    /// <summary>
    /// Creates a simple non-editable text field in the inspector.
    /// </summary>
    /// <param name="label">The text to display.</param>
    /// <param name="leftAlign">Set true for the text to align to left.</param>
    public LabelAttribute(string methodNamePassed, bool leftAlign)
    {
        this.text = methodNamePassed;
        this.textWidth = text.Length;

        this.leftAlign = leftAlign;
    }

}

/// <summary>
/// Insert a better looking header into your inspector.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.All, Inherited = true, AllowMultiple = true)]
public class BetterHeaderAttribute : PropertyAttribute
{
    public static float defaultHeaderWidth = 80;
    public static float defaultHeaderHeight = 80;

    public float textWidth = defaultHeaderWidth;

    public float textHeight = defaultHeaderHeight;

    public string text = "";

    public bool leftAlign = false;

    public bool subHeading = false;

    TextElement te = new TextElement();

    /// <summary>
    /// Creates a header text with the string passed.
    /// </summary>
    /// <param name="heading">The name of the header.</param>
    public BetterHeaderAttribute(string heading)
    {
        this.text = heading;

        this.textWidth = CalculateSizeAndReturn();

        this.textHeight = defaultHeaderHeight;

        this.leftAlign = false;
    }

    /// <summary>
    /// Creates a header text with the string passed.
    /// </summary>
    /// <param name="heading">The name of the header.</param>
    /// <param name="leftAlign">Set true for the text to align to left.</param>
    public BetterHeaderAttribute(string heading, bool leftAlign)
    {
        this.text = heading;

        this.textWidth = CalculateSizeAndReturn();

        this.leftAlign = leftAlign;
    }

    /// <summary>
    /// Creates a header text with the string passed.
    /// </summary>
    /// <param name="heading">The name of the header.</param>
    /// <param name="leftAlign">Set true for the text to align to left.</param>
    /// <param name="subHeading"> To specify a sub-heading.</param>
    public BetterHeaderAttribute(string methodNamePassed, bool leftAlign, bool subHeading)
    {
        this.text = methodNamePassed;

        this.textWidth = CalculateSizeAndReturn();

        this.leftAlign = leftAlign;

        this.subHeading = subHeading;
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

//Declrations for Property Attributes:
#region PropertyAttributesDeclarations

/// <summary> Force a field to not be null by making it look red if it's somehow not assigned. </summary>
[System.AttributeUsage(System.AttributeTargets.Field)]
public class NullCheckAttribute : PropertyAttribute { }

/// <summary>
/// Used on a SerializedField surfaces the expectation that this field can remain empty.
/// </summary>
public class OptionalAttribute : PropertyAttribute
{
    [System.Flags]
    public enum Flag
    {
        /// <summary>
        /// Optional Initilization 
        /// </summary>
        Test = 0,
        /// <summary>
        ///test
        /// </summary>
        Test2 = 1 << 0,
        /// <summary>
        /// test
        /// </summary>
        Test3 = 1 << 1
    }

    public Flag Flags { get; private set; } = Flag.Test;

    public OptionalAttribute() { }

    public OptionalAttribute(Flag flags)
    {
        Flags = flags;
    }
}

#endregion

// Custom Drawers :
// ---------------
// ***************
// ---------------

//Implementations for Property Attributes:

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(LabelAttribute))]
public class InspectorTextPropertyDrawer : DecoratorDrawer
{
    float extraSpaceInternal = 1.4f;

    public override void OnGUI(Rect position)
    {
        GUI.color = Color.white;

        LabelAttribute InspectorTextAttribute = (LabelAttribute)attribute;

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

    public override float GetHeight()
    {
        return base.GetHeight() * extraSpaceInternal;
    }
}


[CustomPropertyDrawer(typeof(BetterHeaderAttribute))]
public class BetterHeadAttributeDecorator : DecoratorDrawer
{
    float supplementHeight = 2f;
    float extraSpaceInternal = 1.3f;
    public override void OnGUI(Rect position)
    {
        GUI.color = Color.white;

        BetterHeaderAttribute InspectorFocusTextAttribute = (BetterHeaderAttribute)attribute;

        Rect buttonRect;

        if (InspectorFocusTextAttribute.leftAlign)
        {
            buttonRect = new Rect(position.x, position.y, InspectorFocusTextAttribute.textWidth, position.height/ extraSpaceInternal + supplementHeight);

        }
        else
        {
            buttonRect = new Rect(position.x, position.y, position.width, position.height/extraSpaceInternal + supplementHeight);
        }

        GUIStyle gUIStyle = new GUIStyle(GUI.skin.box);

        if (InspectorFocusTextAttribute.subHeading)
            gUIStyle.fontStyle = FontStyle.Normal;
        else
            gUIStyle.fontStyle = FontStyle.Bold;

        GUI.Box(buttonRect, InspectorFocusTextAttribute.text,gUIStyle);
    }

    public override float GetHeight()
    {
        return base.GetHeight() * extraSpaceInternal;
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

//Null check // Paint the field red if not assigned
[CustomPropertyDrawer(typeof(OptionalAttribute))]
public class OptionalLabel : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, prop);

        if (prop.propertyType == SerializedPropertyType.ObjectReference)
        {
            if (prop.objectReferenceValue == null)
            {
                GUI.color = new Color(1, 1, 1, 0.45f);
            }
            else
            {
                GUI.color = new Color(1, 1, 1, 0.8f);
            }
        }
        else
        {
            GUI.color = new Color(1, 1, 1, 0.45f);
            
        }

        label.text = "(optional) " + label.text;

        EditorGUI.PropertyField(position, prop, label);

        GUI.color = Color.white;

        EditorGUI.EndProperty();
    }
}

#endif

