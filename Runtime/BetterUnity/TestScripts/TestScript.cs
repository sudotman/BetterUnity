using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Search;

public class TestScript : MonoBehaviour
{
    [BetterHeader("Score")]

    [Optional]
    public int additionalPoints;

    public float mainPoints;

    [Label("The field below is how subheadings could be used to define different portions.")]

    [BetterHeader("Layers", true,true)]

    [Optional]
    public float numberOfLayers = 5;

    [SerializeField, Layer]
    int layer;

    [Label("Assignment of the field below is essential.")]

    [NullCheck]
    public Transform myField;

    [BetterHeader("Alignment Settings")]

    [NullCheck]
    public Transform alignObject;

    [Optional]
    public Color test;

    public int index;

    [InspectorButton("TestFunc2", "Custom Button Call!")]
    public char c;

    [CallInEditor]
    public void TestFunc1()
    {
        Debug.Log("Called Test1!");
    }

    [CallInEditor]
    public void TestFunc2()
    {
        Debug.Log("Called Test2!");
    }
}
