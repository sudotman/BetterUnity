using System;
using System.Text;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class SerializedVector3
{
    public float x;
    public float y;
    public float z;

    public SerializedVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public SerializedVector3(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }
}

[Serializable]
public class SerializedQuaternion
{
    public float x;
    public float y;
    public float z;
    public float w;

    public SerializedQuaternion(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public SerializedQuaternion(Quaternion quaternion)
    {
        x = quaternion.x;
        y = quaternion.y;
        z = quaternion.z;
        w = quaternion.w;
    }
}

public static class Vector3Extensions
{
    public static Vector3 ToVector3(this SerializedVector3 serializedVector3)
    {
        return new Vector3(serializedVector3.x, serializedVector3.y, serializedVector3.z);
    }

    public static SerializedVector3 FromVector3(this Vector3 vector3)
    {
        return new SerializedVector3(vector3);
    }
}

public static class QuaternionExtensions
{
    public static Quaternion ToQuaternion(this SerializedQuaternion serializedRotation)
    {
        return new Quaternion(serializedRotation.x, serializedRotation.y, serializedRotation.z, serializedRotation.w);
    }

    public static SerializedQuaternion FromQuaternion(this Quaternion quaternion)
    {
        return new SerializedQuaternion(quaternion);
    }
}

// Sample code with a general pipeline of conversions

/*

// Converting a Vector 3 array into serialized JSON

List<SerializedVector3> anchorPositions = new List<SerializedVector3>();
List<SerializedQuaternion> anchorRotations = new List<SerializedQuaternion>();

foreach (var gameObjects in arrayOfGameObjects)
{
    Vector3 currentPositionData = anchorGameObject.transform.position;

    Quaternion currentRotData = anchorGameObject.transform.rotation;

    anchorPositions.Add(Vector3Extensions.FromVector3(currentPositionData));
    anchorRotations.Add(QuaternionExtensions.FromQuaternion(currentRotData));
}

// The conversion to array is from a weird bug (that I haven't been able to reproduce but I dont want to risk it.
SerializedVector3[] actualPositions = anchorPositions.ToArray();
SerializedQuaternion[] actualRotations = anchorRotations.ToArray();

string jsonPositions = JsonConvert.SerializeObject(actualPositions);
string jsonRotations = JsonConvert.SerializeObject(actualRotations);

byte[] dataPos = Encoding.ASCII.GetBytes(jsonPositions);
byte[] dataRot = Encoding.ASCII.GetBytes(jsonRotations);

UnityEngine.Windows.File.WriteAllBytes(localJSONPath, dataPos);
UnityEngine.Windows.File.WriteAllBytes(localJSONPath, dataRot);


// Converting a JSON stored array of Vector3 into a Serialized Vector3 array/list

byte[] dataPos = UnityEngine.Windows.File.ReadAllBytes(localJSONPath);
string jsonPos = Encoding.ASCII.GetString(dataPos);

Newtonsoft.Json.Linq.JArray genericObjPos = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(jsonPos);
Debug.Log("SLWLT: " + genericObjPos);

Debug.Log("SLWLT Type: " + genericObjPos.GetType());

Vector3[] positions = genericObjPos.ToObject<Vector3[]>();
*/
