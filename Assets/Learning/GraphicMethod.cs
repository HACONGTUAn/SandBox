using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GraphicMethod : MonoBehaviour
{
    public Mesh mesh;
    public Material material1;
    public Material material2;
    public int instanceCountGroup1 = 500;
    public int instanceCountGroup2 = 500;
    public float rotationSpeed = 20.0f;
    public float speed = 1f;

    private Matrix4x4[] matricesGroup1;
    private Matrix4x4[] matricesGroup2;



    void Start()
    {
        // Khởi tạo mảng chứa các matrix cho nhóm 1
        matricesGroup1 = new Matrix4x4[instanceCountGroup1];
        for (int i = 0; i < instanceCountGroup1; i++)
        {
            matricesGroup1[i] = Matrix4x4.TRS(
                new Vector3(UnityEngine.Random.Range(-10.0f, 0.0f), UnityEngine.Random.Range(-10.0f, 0.0f), UnityEngine.Random.Range(-10.0f, 0.0f)),
                Quaternion.Euler(UnityEngine.Random.Range(0.0f, 360.0f), UnityEngine.Random.Range(0.0f, 360.0f), UnityEngine.Random.Range(0.0f, 360.0f)),
                Vector3.one
            );
        }

        // Khởi tạo mảng chứa các matrix cho nhóm 2
        matricesGroup2 = new Matrix4x4[instanceCountGroup2];
        for (int i = 0; i < instanceCountGroup2; i++)
        {
            matricesGroup2[i] = Matrix4x4.TRS(
                new Vector3(UnityEngine.Random.Range(0.0f, 10.0f), UnityEngine.Random.Range(0.0f, 10.0f), UnityEngine.Random.Range(0.0f, 10.0f)),
                Quaternion.Euler(UnityEngine.Random.Range(0.0f, 360.0f), UnityEngine.Random.Range(0.0f, 360.0f), UnityEngine.Random.Range(0.0f, 360.0f)),
                Vector3.one
            );
        }


    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            MoveGameObject();


        }

        RenderParams rp = new RenderParams(material1);
        // Vẽ nhóm 1
        Graphics.RenderMeshInstanced(rp, mesh, 0, matricesGroup1, instanceCountGroup1);

        RenderParams rp2 = new RenderParams(material2);
        // Vẽ nhóm 2
        Graphics.RenderMeshInstanced(rp2, mesh, 0, matricesGroup2, instanceCountGroup2);

        UpdateGameObject1();
    }

    private void UpdateGameObject1()
    {
        for (int i = 0; i < instanceCountGroup1; i++)
        {
            // Tính toán vị trí xoay mới
            float angle = Time.time * rotationSpeed;
            float radius = 5.0f; // Bán kính xoay
            float x = Mathf.Cos(angle + i) * radius;
            float z = Mathf.Sin(angle + i) * radius;

            matricesGroup1[i] = Matrix4x4.TRS(
                new Vector3(matricesGroup1[i].m03, matricesGroup1[i].m13, matricesGroup1[i].m23), // Giữ nguyên tọa độ y ban đầu
                Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0), // Xoay quanh trục y
                Vector3.one
            );
        }
    }
    void MoveGameObject()
    {
        for (int i = 0; i < instanceCountGroup2; i++)
        {
            float2 _speed = Time.time * speed;
            matricesGroup1[i] = Matrix4x4.TRS(
                new Vector3(matricesGroup1[i].m03, matricesGroup1[i].m13, matricesGroup1[i].m23), // Giữ nguyên tọa độ y ban đầu
                Quaternion.Euler(0, 0, 0), // Xoay quanh trục y
                Vector3.one
            );
        }
    }
    public void AddInstancesToGroup2(int count)
    {
        int newSize = instanceCountGroup2 + count;
        Matrix4x4[] newMatricesGroup2 = new Matrix4x4[newSize];

        // Sao chép các ma trận cũ vào mảng mới
        for (int i = 0; i < instanceCountGroup2; i++)
        {
            newMatricesGroup2[i] = matricesGroup2[i];
        }

        // Tạo các ma trận mới và thêm vào mảng mới
        for (int i = instanceCountGroup2; i < newSize; i++)
        {
            newMatricesGroup2[i] = Matrix4x4.TRS(
                new Vector3(UnityEngine.Random.Range(0.0f, 10.0f), UnityEngine.Random.Range(0.0f, 10.0f), UnityEngine.Random.Range(0.0f, 10.0f)),
                Quaternion.Euler(UnityEngine.Random.Range(0.0f, 360.0f), UnityEngine.Random.Range(0.0f, 360.0f), UnityEngine.Random.Range(0.0f, 360.0f)),
                Vector3.one
            );
        }

        // Cập nhật mảng và số lượng instance
        matricesGroup2 = newMatricesGroup2;
        instanceCountGroup2 = newSize;
    }
}
