using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

/// <summary>
/// Suceed this with a field where you specify the button's text as the variable name
/// </summary>
[System.AttributeUsage(System.AttributeTargets.All)]
public class InspectorButtonAttribute : PropertyAttribute
{
    public static float defaultButtonWidth = 80;

    public readonly string methodToBeCalled;

    public readonly string buttonText;
     
    public bool dynamicWidth = false;

    private float _buttonWidth = defaultButtonWidth;
    public float ButtonWidth
    {
        get { return _buttonWidth; }
        set { _buttonWidth = value; }
    }

    /// <summary>
    /// Create a inspector button with the button text being based on the suceeding variable's name.
    /// </summary>
    /// <param name="methodNamePassed">The name of the method to be called.</param>
    /// <returns>Creates a button in inspector.</returns>
    public InspectorButtonAttribute(string methodNamePassed)
    {
        this.methodToBeCalled = methodNamePassed;

        this.buttonText = "";
    }

    /// <summary>
    /// Create a inspector button with the button text being based on the suceeding variable's name.
    /// </summary>
    /// <param name="methodNamePassed">The name of the method to be called.</param>
    /// <param name="buttonWidth">The width of the button, keep it greater than 80 as below that becomes too small. Pass 0 for dynamic scaling. </param>
    /// <returns>Creates a button in inspector.</returns>
    public InspectorButtonAttribute(string methodNamePassed,float buttonWidth)
    {
        this.methodToBeCalled = methodNamePassed;
        this._buttonWidth = buttonWidth;

        this.buttonText = "";

        if (buttonWidth == 0)
        {
            dynamicWidth = true;
        }
    }

    /// <summary>
    /// Create a inspector button with the button text being based on the suceeding variable's name.
    /// </summary>
    /// <param name="methodNamePassed">The name of the method to be called.</param>
    /// <param name="buttonWidth">The width of the button, keep it greater than 80 as below that becomes too small. Pass 0 for dynamic scaling. </param>
    /// <param name="buttonText">The text inside the button [when using this, the variable name will be ignored]. </param>
    /// <returns>Creates a button in inspector.</returns>
    public InspectorButtonAttribute(string methodNamePassed, float buttonWidth, string buttonText)
    {
        this.methodToBeCalled = methodNamePassed;
        this._buttonWidth = buttonWidth;

        this.buttonText = buttonText;

        if (buttonWidth == 0)
        {
            dynamicWidth = true;
        }
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
public class InspectorButtonPropertyDrawer : PropertyDrawer
{
    private MethodInfo _eventMethodInfo = null;

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        InspectorButtonAttribute inspectorButtonAttribute = (InspectorButtonAttribute)attribute;

        string currentLabel = "";

        if (inspectorButtonAttribute.buttonText.Equals(""))
            currentLabel = label.text;
        else
        {
            currentLabel = inspectorButtonAttribute.buttonText;
        }

        if (inspectorButtonAttribute.dynamicWidth)
        {
            inspectorButtonAttribute.ButtonWidth = HelperFunctions.ScaleRange(currentLabel.Length, 1, currentLabel.Length, 80, currentLabel.Length * 8);
        }
        Rect buttonRect = new Rect(position.x + (position.width - inspectorButtonAttribute.ButtonWidth) * 0.5f, position.y, inspectorButtonAttribute.ButtonWidth, position.height);
        if (GUI.Button(buttonRect, currentLabel))
        {
            System.Type eventOwnerType = prop.serializedObject.targetObject.GetType();
            string eventName = inspectorButtonAttribute.methodToBeCalled;

            if (_eventMethodInfo == null)
                _eventMethodInfo = eventOwnerType.GetMethod(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            if (_eventMethodInfo != null)
                _eventMethodInfo.Invoke(prop.serializedObject.targetObject, null);
            else
                Debug.LogWarning(string.Format("InspectorButton: Unable to find method {0} in {1}", eventName, eventOwnerType));
        }
    }
}
#endif