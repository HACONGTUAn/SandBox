using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ParticleSpawnerSandBox : MonoBehaviour
{
    [Header("tao khu vuc mo phong")]
    [Tooltip("dat vi tri cho khu vuc mo phong")]
    public Vector2 obstacleCentre;
    [Tooltip("dat kich thuoc ")]
    public Vector2 boundsSize;
    [Tooltip("cai dat van toc ban dau")]
    public Vector2 initialVelocity;
    [Header("khu vuc tuong tac")]
    public float interactionRadius;
    Simulation2DSandBox simlation;
    //  float2 bouderSize;
    ParticleSpawnData _data;
    public bool isPlay = false;

    private void Awake()
    {
        simlation = GetComponent<Simulation2DSandBox>();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.4f);
        Gizmos.DrawWireCube(obstacleCentre, boundsSize);

        Vector2 boundX = new Vector2(boundsSize.x / 2 + obstacleCentre.x, -boundsSize.x / 2 + obstacleCentre.x);
        Vector2 boundY = new Vector2(boundsSize.y / 2 + obstacleCentre.y, -boundsSize.y / 2 + obstacleCentre.y);

        if (Application.isPlaying)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bool isPullInteraction = Input.GetMouseButton(0);
            bool isPushInteraction = Input.GetMouseButton(1);
            bool isInteracting = isPullInteraction || isPushInteraction;
            if (isInteracting && CheckAreaBound(mousePos, boundX, boundY) && !isPlay)
            {
                Debug.Log("check");
                Gizmos.color = isPullInteraction ? Color.green : Color.red;
                Gizmos.DrawWireSphere(mousePos, interactionRadius);
                _data = GetSpawnData(mousePos);
                simlation.CreatParicolInSystem();
                isPlay = true;

            }
        }
    }
    bool CheckAreaBound(Vector2 mousePos, Vector2 x, Vector2 y)
    {
        if (x.y <= mousePos.x && mousePos.x <= x.x && y.y <= mousePos.y && mousePos.y <= y.x)
        {
           
            return true;
        }
        return false;
    }
    // xu ly thay doi theo kich thuoc man hinh 
    public ParticleSpawnData GetSpawnData(Vector2 mousePos)
    {
        ParticleSpawnData data = new ParticleSpawnData(1);
                data.positions[0] = mousePos;
                data.velocities[0] = initialVelocity;

        return data;
    }
    public ParticleSpawnData GetData()
    {
        return _data;
    }

    internal Vector2 GetBoundsSize()
    {
        return boundsSize;
    }

    public struct ParticleSpawnData
    {
        public float2[] positions;
        public float2[] velocities;

        public ParticleSpawnData(int num)
        {
            positions = new float2[num];
            velocities = new float2[num];
        }

    }
}

