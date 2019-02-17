using System;
using UnityEngine;

public class FloatToShortTest : MonoBehaviour
{
    void Start()
    {
        float f0 = 1.0e-3f;
        float f1 = 1.0e-4f;
        float f2 = 4.0e-5f;
        float f3 = 3.0e-5f;
        float f4 = 1.0e-5f;
        float f5 = 1.0e-6f;

        short s0 = SerializeFloat.FloatConverterExt.ToShort(f0);
        short s1 = SerializeFloat.FloatConverterExt.ToShort(f1);
        short s2 = SerializeFloat.FloatConverterExt.ToShort(f2);
        short s3 = SerializeFloat.FloatConverterExt.ToShort(f3);
        short s4 = SerializeFloat.FloatConverterExt.ToShort(f4);
        short s5 = SerializeFloat.FloatConverterExt.ToShort(f5);

        float h0 = SerializeFloat.FloatConverterExt.ToFloat(s0);
        float h1 = SerializeFloat.FloatConverterExt.ToFloat(s1);
        float h2 = SerializeFloat.FloatConverterExt.ToFloat(s2);
        float h3 = SerializeFloat.FloatConverterExt.ToFloat(s3);
        float h4 = SerializeFloat.FloatConverterExt.ToFloat(s4);
        float h5 = SerializeFloat.FloatConverterExt.ToFloat(s5);

        Debug.Log("float: " + f0 + ",  half to float: " + h0);
        Debug.Log("float: " + f1 + ",  half to float: " + h1);
        Debug.Log("float: " + f2 + ",  half to float: " + h2);
        Debug.Log("float: " + f3 + ",  half to float: " + h3);
        Debug.Log("float: " + f4 + ",  half to float: " + h4);
        Debug.Log("float: " + f5 + ",  half to float: " + h5);
    }
}
