using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [BetterHeader("Score")]
    public char c;

    [Optional]
    public int additionalPoints;

    public float mainPoints;

    [Label("This is a normal text")]
    public char normalText;

    [BetterHeader("Layers", true)]
    public char focusText;

    [SerializeField, Layer]
    int layer;

    public float testfloat;

    [BetterHeader("Alignment Settings", true)]
    public char leftAlignedFocusText;

    [NullCheck]
    public Transform myField;

    public Color test;
}
