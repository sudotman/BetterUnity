using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [Optional]
    public int additionalPoints;

    public float mainPoints;

    [InspectorText("This is a normal text")]
    public char normalText;

    [InspectorFocusText("This is a text with focus")]
    public char focusText;

    [InspectorFocusText("This is a text with focus aligned to the left", true)]
    public char leftAlignedFocusText;

    [SerializeField, Layer]
    int layer;

    [NullCheck]
    public Transform myField;
}
