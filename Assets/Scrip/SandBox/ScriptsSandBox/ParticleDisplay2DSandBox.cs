using UnityEngine;

public class ParticleDisplay2DSandBox : MonoBehaviour
{
    public Mesh mesh;
    public float scale;
    public float velocityDisplayMax;

    public Material material;
    ComputeBuffer argsBuffer;
    Bounds bounds;
    bool needsUpdate;
    ParticleSpawnerSandBox simulation;

    private void Awake()
    {
        simulation = GetComponent<ParticleSpawnerSandBox>();
    }

    internal void Init(Simulation2DSandBox sim)
    {
        material.SetBuffer("Positions2D", sim.positionBuffer);
        material.SetBuffer("Velocities", sim.velocityBuffer);
        material.SetBuffer("DensityData", sim.densityBuffer);

        argsBuffer = ComputeHelper.CreateArgsBuffer(mesh, sim.positionBuffer.count);
        bounds = new Bounds(Vector3.zero, Vector3.one * 10000);
    }

    void LateUpdate()
    {
        if (material != null && simulation.isPlay)
        {
            UpdateSettings();
            Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, argsBuffer);
        }
    }

    void UpdateSettings()
    {
        if (needsUpdate)
        {
            needsUpdate = false;
            material.SetFloat("_Scale", scale);
            material.SetFloat("_VelocityMax", velocityDisplayMax);
        }
    }

    void OnValidate()
    {
        needsUpdate = true;
    }

    void OnDestroy()
    {
        ComputeHelper.Release(argsBuffer);
    }
}
