using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [InspectorButton("myFunc")]
    public char myButton;

    [InspectorButton("myFunc",100)]
    public char myButtonWide;

    [InspectorButton("myFunc", 40)]
    public char smol;

    [InspectorButton("myFunc", 200)]
    public char big;

    void myFunc()
    {
        Debug.Log("log from my func!");

        Vector3 test = new Vector3();
        test.MinusFloatVector3(2);

        float test2 = 2;
        test2.ScaleRange(0, 100, 30, 40);

        Vector3.Lerp(test,test,0);

        
        
    }
  
}
