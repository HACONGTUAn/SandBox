using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class QuadtreeDemo : MonoBehaviour
{
    public static QuadtreeDemo instant;
    public Rect worldBounds = new Rect(0, 0, 100, 100);
    public Quadtree quadtree;
    public List<Circle> circles;
    public List<Circle> circles2;
    public int count = 100;

    private void Awake()
    {
        instant = this;
        circles = new List<Circle>();
        circles2 = new List<Circle>();  
    }
    void Start()
    {
        quadtree = new Quadtree(worldBounds);
 
    }

    void Update()
    {
        if (quadtree.checkQuadtree() && circles.Count > 0 && circles2.Count > 0)
        {
          

            // Check for collisions using the quadtree
            for (int i = 0; i < circles.Count; i++)
            {
                var circle = circles[i];
                List<Circle> possibleCollisions = quadtree.Retrieve(new List<Circle>(), circle);

                foreach (var other in possibleCollisions)
                {
                    if (!circle.Equals(other) && AreCirclesColliding(circle, other) && circle.id != other.id)
                    {
                        Debug.Log($"Circle {i} collides with another circle");
                        // Handle collision logic here
                    }
                }
            }
        }
       
    }

    bool AreCirclesColliding(Circle a, Circle b)
    {
        float distance = Vector2.Distance(a.position, b.position);
        float radiusSum = a.radius + b.radius;
        return distance < radiusSum;
    }

    void OnDrawGizmos()
    {
        if (quadtree != null)
        {
            Gizmos.color = Color.white;
            quadtree.DrawGizmos();

            // Draw circles
            Gizmos.color = Color.red;
            foreach (var circle in circles)
            {
                Vector2 myVector2 = new Vector2(circle.position.x, circle.position.y);
                Gizmos.DrawWireSphere(myVector2, circle.radius);
            }
            Gizmos.color = Color.blue;
            foreach (var circle in circles2)
            {
                Vector2 myVector2 = new Vector2(circle.position.x, circle.position.y);
                Gizmos.DrawWireSphere(myVector2, circle.radius);
            }


        }
    }
}
