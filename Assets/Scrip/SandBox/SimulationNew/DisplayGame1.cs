using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;


public class DisplayGame1 : MonoBehaviour
{
    public ComputeShader computeShader;
    public Mesh mesh;
    public Material material;
    public int instanceCount = 1;
   // private ComputeBuffer positionBuffer;
  //  private ComputeBuffer velocityBuffer;
    
    private Matrix4x4[] matrices;
    private RenderParams renderParams;
  
    public ClickGameScene click;
    public int iterationsPerFrame = 2;
    public float timeScale = 1;
   // float timeDestroy = 2f;
    public float gravity = 0.04f;
    public float collisionDamping;
    public float deltaTime;
    public Vector2 khuvuc;
    public Vector2 vitrichinhSua;
    
    public float interactionInputRadius;
    public float smoothingRadius;
    public float interactionInputStrength;
    public float nearPressureMultiplier;
    public float pressureMultiplier;
    public float targetDensity;
    public float viscosityStrength;
    public bool check = false;
    public bool checkGravity = false;
    private ComputeBuffer particleBuffer;
   
    //  private Particle[] listWaterParticle = new Particle[1000];
    int kernelID;
    int kernelID1;
    [System.Serializable]
    struct Particle
    {
        public Vector2 position;
        public Vector2 velocity;
        public float activeStatus;
    }

   

    void StartOne( Vector2 mousPostion)
    {
        particleBuffer = new ComputeBuffer(instanceCount, sizeof(float) * 5); 
        Particle[] _paticelBuffer = new Particle[instanceCount];

        for (int i = 0; i <  instanceCount; i++)
        {
            _paticelBuffer[i].position = mousPostion;
            _paticelBuffer[i].velocity = Vector2.zero;
            _paticelBuffer[i].activeStatus = 1f;
        }
         particleBuffer.SetData(_paticelBuffer);

         kernelID = computeShader.FindKernel("UpdatePositions");
        kernelID1 = computeShader.FindKernel("ExternalForces");

        computeShader.SetBuffer(kernelID1, "Particles", particleBuffer);
        computeShader.SetBuffer(kernelID, "Particles", particleBuffer);
        
    }

    public void OnUpdate()
    {
        if (check)
        {
            QuadtreeDemo.instant.quadtree.Clear();
            QuadtreeDemo.instant.circles2.Clear();
            // kiem tra buffer co du lieu hay khong
            if (particleBuffer == null)
            {
                Debug.LogError("particleBuffer is not initialized.");
                return;
            }
            float timeStep = Time.deltaTime / iterationsPerFrame * timeScale;
            // khoi chay GPU
            AddThongSo(timeStep);
            computeShader.Dispatch(kernelID1, Mathf.CeilToInt(instanceCount / 10f), 1, 1);
            computeShader.Dispatch(kernelID, Mathf.CeilToInt(instanceCount / 10f), 1, 1);
          
            // lay du lieu ra 
            Particle[] particlesNew = new Particle[instanceCount];
            particleBuffer.GetData(particlesNew);
            // dua du lieu vao quatree
            for (int i = 0; i < instanceCount; i++)
            {
                QuadtreeDemo.instant.circles2.Add(new Circle(2, particlesNew[i].position, 0.05f));
                QuadtreeDemo.instant.quadtree.Insert(QuadtreeDemo.instant.circles2[i]);
            }
            // doc du lieu ra 
            matrices = new Matrix4x4[instanceCount];
            for (int i = 0; i < instanceCount; i++)
            {
                if (particlesNew[i].activeStatus > -1)
                {
                    matrices[i].SetTRS(new Vector3(particlesNew[i].position.x, particlesNew[i].position.y, 0f), Quaternion.identity, Vector3.one * 0.1f);
                   
                }
                
            }
            Graphics.RenderMeshInstanced(renderParams, mesh, 0, matrices);

        }
        // dat thoi gian de giai phong buffer
        //if(timeDestroy <= 0)
        //{
        //    Destroy();
        //    timeDestroy = 2f;
        //}
        //timeDestroy -= Time.deltaTime;
    }
    void AddThongSo(float timeStep)
    {
        deltaTime = timeStep;
        computeShader.SetVector("boundsSize", khuvuc);
        computeShader.SetVector("obstacleCentre", click.obstacleCentre);
        computeShader.SetFloat("collisionDamping", collisionDamping);
        computeShader.SetFloat("deltaTime", deltaTime);
        computeShader.SetFloat("gravity",gravity);
        computeShader.SetInt("numParticles", instanceCount);
    }

    void OnDestroy()
    {
        //if (positionBuffer != null)
        //{
        //    positionBuffer.Release();
        //}

        //if (velocityBuffer != null)
        //{
        //    velocityBuffer.Release();
        //}
        if(particleBuffer != null)
        {
            particleBuffer.Release();
            particleBuffer = null;
        }
     
    }
    void Destroy()
    {
        //if (positionBuffer != null)
        //{
        //    positionBuffer.Release();
        //}

        //if (velocityBuffer != null)
        //{
        //    velocityBuffer.Release();
        //}
        Debug.Log("duoc goi");
    }
    public void AddInstances(int newInstancesCount, Vector2 mousPosition)
    {
        if (!check)
        {
            khuvuc = click.boundsSize;
            renderParams = new RenderParams(material);
            StartOne(mousPosition);
        }
        else
        {
            int newTotalInstanceCount = instanceCount + newInstancesCount;
            Particle[] newParticles = new Particle[newTotalInstanceCount];

            Particle[] existingParticles = new Particle[instanceCount];
            particleBuffer.GetData(existingParticles);

            System.Array.Copy(existingParticles, newParticles, instanceCount);

            for (int i = instanceCount; i < newTotalInstanceCount; i++)
            {
                newParticles[i].position = mousPosition;
                newParticles[i].velocity = Vector2.zero;
                newParticles[i].activeStatus = 1f;
            }

            particleBuffer.Release();
            particleBuffer = new ComputeBuffer(newTotalInstanceCount, sizeof(float) * 5);
            particleBuffer.SetData(newParticles);
            computeShader.SetBuffer(kernelID, "Particles", particleBuffer);
            computeShader.SetBuffer(kernelID1, "Particles", particleBuffer);
            matrices = new Matrix4x4[newTotalInstanceCount];
            for (int i = 0; i < newTotalInstanceCount; i++)
            {
                matrices[i] = Matrix4x4.TRS(new Vector3(newParticles[i].position.x, newParticles[i].position.y, 0f), Quaternion.identity, Vector3.one * 0.1f);
            }

            instanceCount = newTotalInstanceCount;
        }
        
        check = true;
    }
    
}
