using UnityEngine;

public class HeadLevel : MonoBehaviour
{
    float timeElapsed;
    
    float startValue = 0;
    float endValue = 10;
    float valueToLerp;

    [Tooltip("Center Eye Anchor (represents head) to follow.")]
    public Transform centerEyeToFollow;

    [Header("Attributes")]

    [Tooltip("Lerp Duration of the follow (Smoothing).")]
    public float lerpDuration = 3;

    [Tooltip("Offset from the center eye.")]
    public float offset = -0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(transform.position.y, centerEyeToFollow.position.y, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
        }
        else { timeElapsed = 0; }

        transform.SetPositionAndRotation(new Vector3(transform.position.x, centerEyeToFollow.position.y + offset, transform.position.z), transform.rotation);

    }
}
