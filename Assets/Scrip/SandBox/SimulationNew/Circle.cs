using Unity.Mathematics;

public struct Circle
{
    public int id;
    public float2 position;
    public float radius;

    public Circle(int id,float2 position, float radius)
    {
        this.id = id;
        this.position = position;
        this.radius = radius;
    }
}