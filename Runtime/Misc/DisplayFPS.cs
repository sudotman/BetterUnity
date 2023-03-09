using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayFPS : MonoBehaviour
{
    private int current;
    private Text fpsCounter;
    // Start is called before the first frame update
    private void Awake()
    {
        fpsCounter = GetComponent<Text>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(RefreshFPS), 1, 1);
    }

    void RefreshFPS()
    {
        current = (int)(1f / Time.unscaledDeltaTime);
        fpsCounter.text = current.ToString();
    }
}
