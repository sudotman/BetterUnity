using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class testui : MonoBehaviour
{
    public string text;

    [Range(0,160)]
    public int textLength;

    //[Range(0,400)]
    public float add = 10;

    const string glyphs = "abcdefghijklmnopqrstsdfasdfsadjfhsadfjkbhsagkjldsbgjdsfklgdsfjbgldsfgdsfgdsfgdfsgdsfgsfsdsdgklgdsfjbgldsfgdsfgdsfgdfsgdsfgdfsgsdfgsdfgsdfguvwxyz0123456789sdaknfksadnflsdasdfgsdfskdff";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text = glyphs.Substring(0, textLength);

        if(textLength>=100)
            add = HelperFunctions.ScaleRange((float)text.Length, 100, 160, 0.9f, 0.85f);
        else if (textLength >= 25)
            add = HelperFunctions.ScaleRange((float)text.Length, 25, 100, 0.95f, 0.9f);
        else if(textLength<9)
            add = 1.25f;
        else
            add = 1;
      
       
    }

    private void OnGUI()
    {
        Rect buttonRect = new Rect(30, 200, add * 7 * text.Length, 30);
        GUI.Box(buttonRect, text);
    }
}
