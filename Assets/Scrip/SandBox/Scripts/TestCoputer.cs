using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCoputer : MonoBehaviour
{
    public ComputeShader computeShader;

    struct Data {
        public float value;
    }

    private void Start()
    {
        int count = 10;
        ComputeBuffer buffe = new ComputeBuffer(count, sizeof(float));
        ComputeBuffer buffe2 = new ComputeBuffer(count, sizeof(float));
        Data[] data = new Data[count];
        Data[] data2 = new Data[count];

        for (int i = 0; i < count; i++)
        {
            data[i].value = i;
        }
        for (int i = 0; i < count; i++)
        {
            data2[i].value = i;
        }
        buffe.SetData(data);
        buffe2.SetData(data2);

        int kernelHandle = computeShader.FindKernel("CSMain");

        computeShader.SetBuffer(kernelHandle, "buffer", buffe);
        computeShader.Dispatch(kernelHandle, count, 1, 1);

        computeShader.SetBuffer(kernelHandle, "buffer", buffe2);
        computeShader.Dispatch(kernelHandle, count, 1, 1);

        buffe.GetData(data);
        buffe2.GetData(data2);

        for (int i = 0; i < count; i++)
        {
            Debug.Log("value" + ":" + data[i].value);
        }
        for (int i = 0; i < count; i++)
        {
            Debug.Log("value2" + ":" + data2[i].value);
        }
        buffe.Release();
        buffe2.Release();
    }
    // public Material displayMaterial;
    // Start is called before the first frame update
    void Update()
    {
      
    }

   
}
