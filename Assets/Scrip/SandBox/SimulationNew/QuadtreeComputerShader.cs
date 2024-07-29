using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class QuadtreeComputerShader : MonoBehaviour
{
    static public QuadtreeComputerShader instant;
    public ComputeShader quadtreeComputeShader;
    public int numCircles = 10000;
    public Rect quadtreeBounds;
    public ClickGameScene click;
    public bool checkData = false;

    private ComputeBuffer circleBuffer;
    private ComputeBuffer nodeBuffer;
    private ComputeBuffer collisionBuffer;
    private Circle[] circles ;
    private Node[] nodes;
    int index = 0;
    struct Circle
    {
        public float2 position;
        public float radius;
        public int id;
    }

    struct Node
    {
        public float4 bounds;
        public int count;
        public int firstChild;
        public int level;
    }

    public void SetCircles(float2 position, float radius, int id)
    {
        if (index < numCircles)
        {
            circles[index] = new Circle
            {
                position = position,
                radius = radius,
                id = id
            };
        }
        index++;
    }
    private void Awake()
    {
        instant = this;
    }
    void Start()
    {
        quadtreeBounds = new Rect(click.obstacleCentre.x, click.obstacleCentre.y,click.boundsSize.x,click.boundsSize.y);
        circles = new Circle[numCircles];
        nodes = new Node[1024];

        nodes[0] = new Node
        {
            bounds = new float4(quadtreeBounds.x, quadtreeBounds.y, quadtreeBounds.width, quadtreeBounds.height),
            count = 0,
            firstChild = -1,
            level = 0
        };

        circleBuffer = new ComputeBuffer(numCircles, sizeof(float) * 3 + sizeof(int));
        nodeBuffer = new ComputeBuffer(nodes.Length, sizeof(float) * 4 + sizeof(int) * 3);
        collisionBuffer = new ComputeBuffer(numCircles, sizeof(int));

    }


    private void Update()
    {

        if (checkData)
        {
            CreateBuffer();
        }
           
        
    }
    void CreateBuffer() { 
       
        
        circleBuffer.SetData(circles);
        nodeBuffer.SetData(nodes);

        quadtreeComputeShader.SetBuffer(0, "circles", circleBuffer);
        quadtreeComputeShader.SetBuffer(0, "nodes", nodeBuffer);
        quadtreeComputeShader.SetBuffer(0, "collisions", collisionBuffer);

        int threadGroups = Mathf.CeilToInt(numCircles / 64f);
        quadtreeComputeShader.Dispatch(0, threadGroups, 1, 1);

        quadtreeComputeShader.SetBuffer(1, "circles", circleBuffer);
        quadtreeComputeShader.SetBuffer(1, "nodes", nodeBuffer);
        quadtreeComputeShader.SetBuffer(1, "collisions", collisionBuffer);

        quadtreeComputeShader.Dispatch(1, threadGroups, 1, 1);

        int[] collisionResults = new int[numCircles];
        collisionBuffer.GetData(collisionResults);

       // Debug.Log(collisionResults.Length);
        for (int i = 0; i < numCircles; i++)
        {
            if (collisionResults[i] > 0)
            {
                Debug.Log($"Circle {i} has {collisionResults[i]} collisions");
            }
        }
    }

    void OnDestroy()
    {
        circleBuffer.Release();
        nodeBuffer.Release();
        collisionBuffer.Release();
    }

    void OnDrawGizmos()
    {

        //Gizmos.color = Color.red;
        //foreach (var circle in circles)
        //{
        //    if (circle.id == 1)
        //    {
        //        Gizmos.color = Color.red;
        //        Vector2 myVector2 = new Vector2(circle.position.x, circle.position.y);
        //        Gizmos.DrawWireSphere(myVector2, circle.radius);
        //    }
        //    else
        //    {

        //        Gizmos.color = Color.blue;
        //        Vector2 myVector2 = new Vector2(circle.position.x, circle.position.y);
        //        Gizmos.DrawWireSphere(myVector2, circle.radius);
        //    }
        //}
      
        Gizmos.DrawWireCube(click.obstacleCentre, click.boundsSize * 2);
    }
}
