using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;


public class DisplayGame : MonoBehaviour
{
    public ComputeShader computeShader;
    public Mesh mesh;
    public Material material;
    public int instanceCount = 1;
    private ComputeBuffer positionBuffer;
    private ComputeBuffer velocityBuffer;
    ComputeBuffer predictedPositionBuffer;
    ComputeBuffer spatialOffsetsBuffer;
    ComputeBuffer SpatialIndicesBuffer;
    ComputeBuffer densityBuffer;
    private Matrix4x4[] matrices;
    public RenderParams renderParams;
    private int pressureKernel;
    private int externalForcesKernel;
    private int updatePositionKernel;
    private int spatialHashKernel;
    private int densityKernel;
    private int viscosityKernel;
    public ClickGameScene click;
    public int iterationsPerFrame = 2;
    public float timeScale = 1;

    public float gravity = -0.04f;
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
    GPUSort gpuSort;
  
    private void Start()
    {
    }

    void StartOne( Vector2 mousPostion)
    {
        khuvuc = click.boundsSize;
        //  vitrichinhSua = click.obstacleCentre;
        // Khởi tạo mảng matrices
        matrices = new Matrix4x4[instanceCount];

        // Khởi tạo buffer vị trí
        //positionBuffer = new ComputeBuffer(instanceCount, 2 * sizeof(float));
        //velocityBuffer = new ComputeBuffer(instanceCount, 2 * sizeof(float));
        //predictedPositionBuffer = new ComputeBuffer(instanceCount, 2 * sizeof(float));

        positionBuffer = ComputeHelper.CreateStructuredBuffer<float2>(instanceCount);
        predictedPositionBuffer = ComputeHelper.CreateStructuredBuffer<float2>(instanceCount);
        velocityBuffer = ComputeHelper.CreateStructuredBuffer<float2>(instanceCount);

        spatialOffsetsBuffer = new ComputeBuffer(instanceCount, sizeof(uint));
        densityBuffer = new ComputeBuffer(instanceCount, 2 * sizeof(float));
        SpatialIndicesBuffer = new ComputeBuffer(instanceCount, 3 * sizeof(float));
        // Khởi tạo các vị trí ban đầu
        Vector2[] positions = new Vector2[instanceCount];
        Vector2[] _velocityBuffer = new Vector2[instanceCount];
        Vector2[] _predictedPositionBuffer = new Vector2[instanceCount];

        //  uint[] _spatialOffsets = new uint[instanceCount];
        //  Vector2[] _densityBuffer = new Vector2[instanceCount];
        //  Vector3[] _SpatialIndicesBuffer = new Vector3[instanceCount];
        for (int i = 0; i < instanceCount; i++)
        {
            positions[i] = mousPostion;
            _velocityBuffer[i] = Vector2.zero;
            _predictedPositionBuffer[i] = positions[i];

            //  matrices[i] = Matrix4x4.TRS(new Vector3(positions[i].x, positions[i].y,0f) , Quaternion.identity, Vector3.one * 0.1f);
        }
        // SpatialIndicesBuffer.SetData(_SpatialIndicesBuffer);
        // densityBuffer.SetData(_densityBuffer);
        //  spatialOffsetsBuffer.SetData(_spatialOffsets);
        positionBuffer.SetData(positions);
        velocityBuffer.SetData(_velocityBuffer);
        predictedPositionBuffer.SetData(_predictedPositionBuffer);

        // Lấy kernel ID từ ComputeShader
        pressureKernel = computeShader.FindKernel("CalculatePressureForce");
        externalForcesKernel = computeShader.FindKernel("ExternalForces");
        updatePositionKernel = computeShader.FindKernel("UpdatePositions");
        spatialHashKernel = computeShader.FindKernel("UpdateSpatialHash");
        densityKernel = computeShader.FindKernel("CalculateDensities");
        viscosityKernel = computeShader.FindKernel("CalculateViscosity");
        // Gán buffer vào ComputeShader
        //computeShader.SetBuffer(kernelID3, "Densities", densityBuffer);
        //computeShader.SetBuffer(kernelID3, "PredictedPositions", predictedPositionBuffer);
        //computeShader.SetBuffer(kernelID3, "SpatialOffsets", spatialOffsetsBuffer);
        //computeShader.SetBuffer(kernelID3, "SpatialIndices", SpatialIndicesBuffer);
        //computeShader.SetBuffer(kernelID3, "Velocities", velocityBuffer);


        //computeShader.SetBuffer(kernelID2, "Positions", predictedPositionBuffer);
        //computeShader.SetBuffer(kernelID2, "PredictedPositions", predictedPositionBuffer);
        //computeShader.SetBuffer(kernelID2, "Velocities", velocityBuffer);

        //computeShader.SetBuffer(kernelID,"Positions", positionBuffer);
        //computeShader.SetBuffer(kernelID, "Velocities", velocityBuffer);

        ComputeHelper.SetBuffer(computeShader, positionBuffer, "Positions", externalForcesKernel, updatePositionKernel);
        ComputeHelper.SetBuffer(computeShader, predictedPositionBuffer, "PredictedPositions", externalForcesKernel, spatialHashKernel, densityKernel, pressureKernel, viscosityKernel);
        ComputeHelper.SetBuffer(computeShader, SpatialIndicesBuffer, "SpatialIndices", spatialHashKernel, densityKernel, pressureKernel, viscosityKernel);
        ComputeHelper.SetBuffer(computeShader, spatialOffsetsBuffer, "SpatialOffsets", spatialHashKernel, densityKernel, pressureKernel, viscosityKernel);
        ComputeHelper.SetBuffer(computeShader, densityBuffer, "Densities", densityKernel, pressureKernel, viscosityKernel);
        ComputeHelper.SetBuffer(computeShader, velocityBuffer, "Velocities", externalForcesKernel, pressureKernel, viscosityKernel, updatePositionKernel);
        // Khởi tạo RenderParams
        renderParams = new RenderParams(material);

        gpuSort = new();
        gpuSort.SetBuffers(SpatialIndicesBuffer, spatialOffsetsBuffer);
    
    }

    public void OnUpdate()
    {
        if (check)
        {
            QuadtreeDemo.instant.quadtree.Clear();
            QuadtreeDemo.instant.circles.Clear();
            float timeStep = Time.deltaTime / iterationsPerFrame * timeScale;
            AddThongSo(timeStep);
            // Dispatch ComputeShader để cập nhật vị trí
            //  computeShader.Dispatch(kernelID2, Mathf.CeilToInt(instanceCount / 10f), 1, 1);
            //  computeShader.Dispatch(kernelID3, Mathf.CeilToInt(instanceCount / 10f), 1, 1);
            //   computeShader.Dispatch(kernelID, Mathf.CeilToInt(instanceCount / 10f), 1, 1);

            //     ComputeHelper.Dispatch(computeShader, instanceCount, kernelIndex: kernelID2);
            ComputeHelper.Dispatch(computeShader, instanceCount, kernelIndex: externalForcesKernel);
            ComputeHelper.Dispatch(computeShader, instanceCount, kernelIndex: spatialHashKernel);
            gpuSort.SortAndCalculateOffsets();
            ComputeHelper.Dispatch(computeShader, instanceCount, kernelIndex: densityKernel);
            ComputeHelper.Dispatch(computeShader, instanceCount, kernelIndex: pressureKernel);
            ComputeHelper.Dispatch(computeShader, instanceCount, kernelIndex: viscosityKernel);
            ComputeHelper.Dispatch(computeShader, instanceCount, kernelIndex: updatePositionKernel);
            // Nhận lại dữ liệu từ buffer
            Vector2[] positions = new Vector2[instanceCount];
            float2[] positonsFloat = new float2[instanceCount];
          
            positionBuffer.GetData(positions);
            for (int i = 0; i < instanceCount; i++)
            {
                positonsFloat[i] = new float2(positions[i].x, positions[i].y);
                QuadtreeDemo.instant.circles.Add(new Circle(1, positonsFloat[i], 0.05f));
                QuadtreeDemo.instant.quadtree.Insert(QuadtreeDemo.instant.circles[i]);
            }

           
            // Cập nhật matrices
            for (int i = 0; i < instanceCount; i++)
            {

                // Kiểm tra giá trị NaN hoặc vô hạn
                if (float.IsNaN(positions[i].x) || float.IsNaN(positions[i].y) || float.IsInfinity(positions[i].x) || float.IsInfinity(positions[i].y))
                {
                    Debug.LogError($"Invalid position at index {i}: {positions[i]}");
                    positions[i] = Vector2.zero; // Đặt giá trị mặc định hoặc xử lý lỗi theo cách của bạn
                }
                matrices[i].SetTRS(new Vector3(positions[i].x, positions[i].y, 0f), Quaternion.identity, Vector3.one * 0.1f);


            }
            //if (Input.GetKeyDown(KeyCode.N))
            //{
            //    AddInstances(100);

            //}
            // Vẽ các đối tượng bằng RenderMeshInstanced
            Graphics.RenderMeshInstanced(renderParams, mesh, 0, matrices);

        }
    }
    void AddThongSo(float timeStep)
    {
        
        deltaTime = timeStep;
        computeShader.SetFloat("gravity", gravity);
        computeShader.SetFloat("collisionDamping", collisionDamping);
        computeShader.SetFloat("deltaTime", deltaTime);
        computeShader.SetVector("boundsSize", khuvuc);
        computeShader.SetVector("obstacleCentre", click.obstacleCentre);

        computeShader.SetFloat("smoothingRadius", smoothingRadius);
        computeShader.SetFloat("interactionInputStrength", interactionInputStrength);

        computeShader.SetFloat("viscosityStrength", viscosityStrength);

        computeShader.SetFloat ("nearPressureMultiplier", nearPressureMultiplier);
        computeShader.SetFloat ("pressureMultiplier", pressureMultiplier);
        computeShader.SetFloat("targetDensity", targetDensity);


        computeShader.SetFloat("Poly6ScalingFactor", 4 / (Mathf.PI * Mathf.Pow(smoothingRadius, 8)));
        computeShader.SetFloat("SpikyPow3ScalingFactor", 10 / (Mathf.PI * Mathf.Pow(smoothingRadius, 5)));
        computeShader.SetFloat("SpikyPow2ScalingFactor", 6 / (Mathf.PI * Mathf.Pow(smoothingRadius, 4)));
        computeShader.SetFloat("SpikyPow3DerivativeScalingFactor", 30 / (Mathf.Pow(smoothingRadius, 5) * Mathf.PI));
        computeShader.SetFloat("SpikyPow2DerivativeScalingFactor", 12 / (Mathf.Pow(smoothingRadius, 4) * Mathf.PI));

        computeShader.SetInt("numParticles", instanceCount);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //bool isPullInteraction = Input.GetMouseButton(0);
        //bool isPushInteraction = Input.GetMouseButton(1);
        float currInteractStrength = 0;
        //if (isPushInteraction || isPullInteraction)
        //{
        //    currInteractStrength = isPushInteraction ? -interactionInputStrength : interactionInputStrength;
        //    //  spawnData = spawner.GetSpawnDataClick();
        //    //  display.Init(this);



        //}
       

        computeShader.SetVector("interactionInputPoint", mousePos);
        computeShader.SetFloat("interactionInputStrength", currInteractStrength);
        computeShader.SetFloat("interactionInputRadius", interactionInputRadius);
    }

    void OnDestroy()
    {
        // Giải phóng buffer khi không cần thiết
        if (positionBuffer != null)
        {
            positionBuffer.Release();
        }
        if (velocityBuffer != null)
        {
            velocityBuffer.Release();
        }
        if(predictedPositionBuffer != null)
        {
            predictedPositionBuffer.Release();
        }
        if(spatialOffsetsBuffer != null)
        {
            spatialOffsetsBuffer.Release();
        }
        if(SpatialIndicesBuffer != null)
        {
            SpatialIndicesBuffer.Release(); 
        }
        if(densityBuffer != null)
        {
            densityBuffer.Release();
        }
    }

    public void AddInstances(int newInstancesCount, Vector2 mousPosition)
    {
        if (!check)
        {
            StartOne(mousPosition);
        }
        else
        {
            // Tính toán số lượng tổng thể mới
            int newTotalInstanceCount = instanceCount + newInstancesCount;

            // Tạo các mảng mới để chứa dữ liệu mở rộng
            Vector2[] newPositions = new Vector2[newTotalInstanceCount];
            Vector2[] newVelocities = new Vector2[newTotalInstanceCount];
            Vector2[] newPredictedPositions = new Vector2[newTotalInstanceCount];
            uint[] newSpatialOffsets = new uint[newTotalInstanceCount];
            Vector2[] newDensityBuffer = new Vector2[newTotalInstanceCount];
            Vector3[] newSpatialIndicesBuffer = new Vector3[newTotalInstanceCount];

            // Sao chép dữ liệu hiện tại vào các mảng mới
            Vector2[] existingPositions = new Vector2[instanceCount];
            Vector2[] existingVelocities = new Vector2[instanceCount];
            Vector2[] existingPredictedPositions = new Vector2[instanceCount];
            uint[] existingSpatialOffsets = new uint[instanceCount];
            Vector2[] existingDensityBuffer = new Vector2[instanceCount];
            Vector3[] existingSpatialIndicesBuffer = new Vector3[instanceCount];


            // Đọc dữ liệu từ các buffer hiện tại
            positionBuffer.GetData(existingPositions);
            velocityBuffer.GetData(existingVelocities);
            predictedPositionBuffer.GetData(existingPredictedPositions);
            spatialOffsetsBuffer.GetData(existingSpatialOffsets);
            densityBuffer.GetData(existingDensityBuffer);
            SpatialIndicesBuffer.GetData(existingSpatialIndicesBuffer);

            // Sao chép dữ liệu cũ vào mảng mới
            System.Array.Copy(existingPositions, newPositions, instanceCount);
            System.Array.Copy(existingVelocities, newVelocities, instanceCount);
            System.Array.Copy(existingPredictedPositions, newPredictedPositions, instanceCount);
            System.Array.Copy(existingSpatialOffsets, newSpatialOffsets, instanceCount);
            System.Array.Copy(existingDensityBuffer, newDensityBuffer, instanceCount);
            System.Array.Copy(existingSpatialIndicesBuffer, newSpatialIndicesBuffer, instanceCount);

            // Khởi tạo dữ liệu cho các đối tượng mới
            for (int i = instanceCount; i < newTotalInstanceCount; i++)
            {
                newPositions[i] = mousPosition;
                newVelocities[i] = Vector2.zero;
                newPredictedPositions[i] = newPositions[i];
                newSpatialOffsets[i] = 0;  // Giá trị mặc định cho spatialOffsets
                newDensityBuffer[i] = Vector2.zero;  // Giá trị mặc định cho densityBuffer
                newSpatialIndicesBuffer[i] = new Vector3(0, 0, 0);  // Giá trị mặc định cho SpatialIndicesBuffer
            }

            // Giải phóng các buffer cũ
            positionBuffer.Release();
            velocityBuffer.Release();
            predictedPositionBuffer.Release();
            spatialOffsetsBuffer.Release();
            SpatialIndicesBuffer.Release();
            densityBuffer.Release();

            // Tạo các buffer mới với kích thước đã cập nhật
            positionBuffer = new ComputeBuffer(newTotalInstanceCount, sizeof(float) * 2);
            velocityBuffer = new ComputeBuffer(newTotalInstanceCount, sizeof(float) * 2);
            predictedPositionBuffer = new ComputeBuffer(newTotalInstanceCount, sizeof(float) * 2);
            spatialOffsetsBuffer = new ComputeBuffer(newTotalInstanceCount, sizeof(uint));
            SpatialIndicesBuffer = new ComputeBuffer(newTotalInstanceCount, sizeof(float) * 3);
            densityBuffer = new ComputeBuffer(newTotalInstanceCount, sizeof(float) * 2);

            // Cập nhật dữ liệu mới vào các buffer
            positionBuffer.SetData(newPositions);
            velocityBuffer.SetData(newVelocities);
            predictedPositionBuffer.SetData(newPredictedPositions);
            spatialOffsetsBuffer.SetData(newSpatialOffsets);
            SpatialIndicesBuffer.SetData(newSpatialIndicesBuffer);
            densityBuffer.SetData(newDensityBuffer);

            // Cập nhật ComputeShader với các buffer mới
            ComputeHelper.SetBuffer(computeShader, positionBuffer, "Positions", externalForcesKernel, updatePositionKernel);
            ComputeHelper.SetBuffer(computeShader, predictedPositionBuffer, "PredictedPositions", externalForcesKernel, spatialHashKernel, densityKernel, pressureKernel, viscosityKernel);
            ComputeHelper.SetBuffer(computeShader, SpatialIndicesBuffer, "SpatialIndices", spatialHashKernel, densityKernel, pressureKernel, viscosityKernel);
            ComputeHelper.SetBuffer(computeShader, spatialOffsetsBuffer, "SpatialOffsets", spatialHashKernel, densityKernel, pressureKernel, viscosityKernel);
            ComputeHelper.SetBuffer(computeShader, densityBuffer, "Densities", densityKernel, pressureKernel, viscosityKernel);
            ComputeHelper.SetBuffer(computeShader, velocityBuffer, "Velocities", externalForcesKernel, pressureKernel, viscosityKernel, updatePositionKernel);

            gpuSort = new GPUSort();
            gpuSort.SetBuffers(SpatialIndicesBuffer, spatialOffsetsBuffer);
            // Cập nhật matrices để vẽ
            matrices = new Matrix4x4[newTotalInstanceCount];
            for (int i = 0; i < newTotalInstanceCount; i++)
            {
                matrices[i] = Matrix4x4.TRS(new Vector3(newPositions[i].x, newPositions[i].y, 0f), Quaternion.identity, Vector3.one * 0.1f);
            }

            // Cập nhật số lượng đối tượng
            instanceCount = newTotalInstanceCount;
        }
        
        check = true;
    }

}

