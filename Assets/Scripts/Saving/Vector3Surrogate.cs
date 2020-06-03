using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Vector3Surrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        Vector3 vect = (Vector3)obj;
        info.AddValue("x", vect.x);
        info.AddValue("y", vect.y);
        info.AddValue("z", vect.z);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Vector3 vect = (Vector3)obj;
        vect.x = (float)info.GetValue("x", typeof(float));
        vect.y = (float)info.GetValue("y", typeof(float));
        vect.z = (float)info.GetValue("z", typeof(float));
        obj = vect;
        return obj;
    }
}