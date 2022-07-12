using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

/// <summary>
/// Suceed this with a field where you specify the button's text as the variable name
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field)]
public class InspectorButtonAttribute : PropertyAttribute
{
    public static float defaultButtonWidth = 80;

    public readonly string methodToBeCalled;

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
    }

    /// <summary>
    /// Create a inspector button with the button text being based on the suceeding variable's name.
    /// </summary>
    /// <param name="methodNamePassed">The name of the method to be called.</param>
    /// <param name="buttonWidth">The width of the button, keep it greater than 80 as below that becomes too small.</param>
    /// <returns>Creates a button in inspector.</returns>
    public InspectorButtonAttribute(string methodNamePassed,float buttonWidth)
    {
        this.methodToBeCalled = methodNamePassed;
        this._buttonWidth = buttonWidth;
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
        Rect buttonRect = new Rect(position.x + (position.width - inspectorButtonAttribute.ButtonWidth) * 0.5f, position.y, inspectorButtonAttribute.ButtonWidth, position.height);
        if (GUI.Button(buttonRect, label.text))
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