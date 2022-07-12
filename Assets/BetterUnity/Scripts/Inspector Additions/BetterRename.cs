using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BetterRename : MonoBehaviour
{
    [Tooltip("Prefix to attach to every child's name.")]
    [SerializeField] private string prefix;
    Transform[] list;

    [Tooltip("Children's suffix numbering will start from this number.")]
    [SerializeField] private int startNumberingFrom = 0;

    private int currentChildCount;

    [InspectorButton("Reset")]
    public char reset;

    private void Awake()
    {
        FetchList();
    }

    // Update is called once per frame
    void Update()
    {
        if (list == null || currentChildCount != transform.childCount)
        {
            FetchList();
        }

        for (int i = 1; i < list.Length; i++)
        {
            list[i].gameObject.name = prefix + (i+startNumberingFrom-1);
        }
    }

    void FetchList()
    {
        list = GetComponentsInChildren<Transform>();
        currentChildCount = transform.childCount;
    }

    void Reset()
    {
        FetchList();
    }
}
