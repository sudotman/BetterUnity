//Derived from Microsoft's MRTK follow script.

using UnityEngine;

public enum FollowType
{
    FollowHeadBasic, FollowAndFace, FaceHeadBasic
}

public enum FollowType_FaceCustomization
{
    FullFace, OnlyX, OnlyY
}

public class HeadLevel : MonoBehaviour
{

    [SerializeField]
    FollowType followType;

    float timeElapsed;

    float valueToLerp;

    [Tooltip("Center Eye Anchor (represents head) to follow.")]
    public Transform centerEyeToFollow;

    [Header("Attributes")]

    [Tooltip("Lerp Duration of the follow (Smoothing).")]
    public float lerpDuration = 3;

    [Tooltip("Offset from the center eye.")]
    public float offset = -0.5f;

    [Header("Follow Settings")]
    [SerializeField]
    bool ignoreNearFarDistanceMovement;

    [SerializeField]
    [Tooltip("Min distance from eye to position element around, i.e. the sphere radius")]
    private float minDistance = 1f;

    [SerializeField]
    [Tooltip("Max distance from eye to element")]
    private float maxDistance = 2f;

    [SerializeField]
    [Tooltip("Move head as is required.")]
    bool doHeadYInstantMovement = true;

    [Header("Face Settings")]

    //If follow is set to follow and face
    [SerializeField]
    FollowType_FaceCustomization faceCustomization;

    [SerializeField]
    bool ignoreAngleClamp;

    [SerializeField]
    [Tooltip("The element will stay at least this far away from the center of view")]
    private float minViewDegrees = 0f;

    [SerializeField]
    [Tooltip("The element will stay at least this close to the center of view")]
    private float maxViewDegrees = 30f;

    [SerializeField]
    [Tooltip("Apply a different clamp to vertical FOV than horizontal. Vertical = Horizontal * aspectV")]
    private float aspectV = 1f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    // Update is called once per frame
    void Update()
    {
        if (timeElapsed < lerpDuration)
        {
            valueToLerp = timeElapsed / lerpDuration;
            timeElapsed += Time.deltaTime;
        }
        else
        {
            timeElapsed = 0;
        }

        // Lerp for position
        Vector3 lerpedPosition = Vector3.Lerp(transform.position, targetPosition, valueToLerp);

        // Lerp for rotation
        Quaternion lerpedRotation = Quaternion.Lerp(transform.rotation, targetRotation, valueToLerp);

        transform.SetPositionAndRotation(lerpedPosition, lerpedRotation);

        Debug.Log(valueToLerp);
        //transform.SetPositionAndRotation(new Vector3(transform.position.x, centerEyeToFollow.position.y + offset, transform.position.z), transform.rotation);

        SolverUpdate();
    }

    public void SolverUpdate()
    {
        Vector3 goalPosition = transform.position;

        if (ignoreAngleClamp)
        {
            if (ignoreNearFarDistanceMovement)
            {
                goalPosition = transform.position;
            }
            else
            {
                GetDesiredOrientation_DistanceOnly(ref goalPosition);
            }
        }
        else
        {
            GetDesiredOrientation(ref goalPosition);
        }

        // Element orientation
        Vector3 refDirUp = Vector3.up;
        Quaternion goalRotation = transform.rotation;



        if (doHeadYInstantMovement == true && (followType == FollowType.FollowHeadBasic || followType == FollowType.FollowAndFace))
        {
            goalPosition = new Vector3(transform.position.x, centerEyeToFollow.position.y, transform.position.z);
        }

        if (followType == FollowType.FollowAndFace || followType == FollowType.FaceHeadBasic)
        {
            GetFaceCustomizationGoalRotation(ref goalRotation, refDirUp);

            if (followType == FollowType.FollowAndFace)
                goalPosition = new Vector3(transform.position.x, centerEyeToFollow.position.y, transform.position.z);
        }

        targetPosition = goalPosition;
        targetRotation = goalRotation;

        //transform.SetPositionAndRotation(goalPosition, goalRotation);
    }

    void GetFaceCustomizationGoalRotation(ref Quaternion goalRot, Vector3 refDirUp)
    {
        if (faceCustomization == FollowType_FaceCustomization.FullFace)
            goalRot = Quaternion.LookRotation(centerEyeToFollow.forward, refDirUp);
        else if (faceCustomization == FollowType_FaceCustomization.OnlyX)
            goalRot = Quaternion.LookRotation(new Vector3(centerEyeToFollow.forward.x, centerEyeToFollow.forward.y, 0), refDirUp);
        else if (faceCustomization == FollowType_FaceCustomization.OnlyY)
            goalRot = Quaternion.LookRotation(new Vector3(centerEyeToFollow.forward.x, 0, centerEyeToFollow.forward.z), refDirUp);
    }

    /// <summary>
    /// Optimized version of GetDesiredOrientation.
    /// </summary>
    /// <param name="desiredPos"></param>
    private void GetDesiredOrientation_DistanceOnly(ref Vector3 desiredPos)
    {
        // TODO: There should be a different solver for distance constraint.
        // Determine reference locations and directions
        Vector3 refPoint = centerEyeToFollow.forward; // recheck between 
        Vector3 elementPoint = transform.position;
        Vector3 elementDelta = elementPoint - refPoint;
        float elementDist = elementDelta.magnitude;
        Vector3 elementDir = elementDist > 0 ? elementDelta / elementDist : Vector3.one;

        // Clamp distance too
        float clampedDistance = Mathf.Clamp(elementDist, minDistance, maxDistance);

        if (!clampedDistance.Equals(elementDist))
        {
            desiredPos = refPoint + clampedDistance * elementDir;
        }
    }

    private void GetDesiredOrientation(ref Vector3 desiredPos)
    {
        // Determine reference locations and directions
        Vector3 direction = centerEyeToFollow.forward; // recheck between this forward or the 'target'
        Vector3 upDirection = centerEyeToFollow.up;
        Vector3 referencePoint = centerEyeToFollow.position; // recheck between
        Vector3 elementPoint = transform.position;
        Vector3 elementDelta = elementPoint - referencePoint;
        float elementDist = elementDelta.magnitude;
        Vector3 elementDir = elementDist > 0 ? elementDelta / elementDist : Vector3.one;

        // Generate basis: First get axis perpendicular to reference direction pointing toward element
        Vector3 perpendicularDirection = (elementDir - direction);
        perpendicularDirection -= direction * Vector3.Dot(perpendicularDirection, direction);
        perpendicularDirection.Normalize();

        // Calculate the clamping angles, accounting for aspect (need the angle relative to view plane)
        float heightToViewAngle = Vector3.Angle(perpendicularDirection, upDirection);
        float verticalAspectScale = Mathf.Lerp(aspectV, 1f, Mathf.Abs(Mathf.Sin(heightToViewAngle * Mathf.Deg2Rad)));

        // Calculate the current angle
        float currentAngle = Vector3.Angle(elementDir, direction);
        float currentAngleClamped = Mathf.Clamp(currentAngle, minViewDegrees * verticalAspectScale, maxViewDegrees * verticalAspectScale);

        // Clamp distance too, if desired
        float clampedDistance = ignoreNearFarDistanceMovement ? elementDist : Mathf.Clamp(elementDist, minDistance, maxDistance);

        // If the angle was clamped, do some special update stuff
        if (currentAngle != currentAngleClamped)
        {
            float angRad = currentAngleClamped * Mathf.Deg2Rad;

            // Calculate new position
            desiredPos = referencePoint + clampedDistance * (direction * Mathf.Cos(angRad) + perpendicularDirection * Mathf.Sin(angRad));
        }
        else if (!clampedDistance.Equals(elementDist))
        {
            // Only need to apply distance
            desiredPos = referencePoint + clampedDistance * elementDir;
        }
    }

    public void ResetToZ()
    {
        Transform childGeneric = transform.GetChild(0);
        //childGeneric.parent = null;
        //childGeneric.rotation.Set(0, 0, 0, 0);
        Vector3 globalPosition = childGeneric.position;
        childGeneric.parent = null;
        childGeneric.SetPositionAndRotation(childGeneric.position, new Quaternion(0, 0, 0, childGeneric.rotation.w));
        //childGeneric.rotation.Set(childGeneric.rotation.x, childGeneric.rotation.y, 0, childGeneric.rotation.w);
    }
}
