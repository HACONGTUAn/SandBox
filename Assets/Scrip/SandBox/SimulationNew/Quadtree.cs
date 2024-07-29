using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


public class Quadtree
{
    private class Node
    {
        public Rect bounds;
        public List<Circle> objects;
        public Node[] children;

        public Node(Rect bounds)
        {
            this.bounds = bounds;
            this.objects = new List<Circle>();
            this.children = null;
        }
    }

    private Node root;
    private int maxObjects = 4;
    private int maxLevels = 5;

    public bool checkQuadtree()
    {
        if (root == null) return false;
        return true;
    }
    public Quadtree(Rect bounds)
    {
        root = new Node(bounds);
    }

    public void Insert(Circle obj)
    {
        Insert(root, obj, 0);
    }

    private void Insert(Node node, Circle obj, int level)
    {
        if (node.children != null)
        {
            int index = GetChildIndex(node, obj);
            if (index != -1)
            {
                Insert(node.children[index], obj, level + 1);
                return;
            }
        }

        node.objects.Add(obj);

        if (node.objects.Count > maxObjects && level < maxLevels)
        {
            if (node.children == null && ShouldSubdivide(node, obj))
            {
                Subdivide(node);
            }

            if (node.children != null)
            {
                int i = 0;
                while (i < node.objects.Count)
                {
                    int index = GetChildIndex(node, node.objects[i]);
                    if (index != -1)
                    {
                        Circle temp = node.objects[i];
                        node.objects.RemoveAt(i);
                        Insert(node.children[index], temp, level + 1);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }
    }

    private bool ShouldSubdivide(Node node, Circle newObject)
    {
        foreach (var obj in node.objects)
        {
            if (obj.id != newObject.id)
            {
                return true;
            }
        }
        return false;
    }

    private void Subdivide(Node node)
    {
        float subWidth = node.bounds.width / 2f;
        float subHeight = node.bounds.height / 2f;
        float x = node.bounds.x;
        float y = node.bounds.y;

        node.children = new Node[4];
        node.children[0] = new Node(new Rect(x, y, subWidth, subHeight));
        node.children[1] = new Node(new Rect(x + subWidth, y, subWidth, subHeight));
        node.children[2] = new Node(new Rect(x, y + subHeight, subWidth, subHeight));
        node.children[3] = new Node(new Rect(x + subWidth, y + subHeight, subWidth, subHeight));
    }

    private int GetChildIndex(Node node, Circle obj)
    {
        int index = -1;
        float verticalMidpoint = node.bounds.x + (node.bounds.width / 2f);
        float horizontalMidpoint = node.bounds.y + (node.bounds.height / 2f);

        bool topQuadrant = (obj.position.y - obj.radius) < horizontalMidpoint && (obj.position.y + obj.radius) < horizontalMidpoint;
        bool bottomQuadrant = (obj.position.y - obj.radius) > horizontalMidpoint;

        if ((obj.position.x - obj.radius) > verticalMidpoint)
        {
            if (topQuadrant)
            {
                index = 3;
            }
            else if (bottomQuadrant)
            {
                index = 1;
            }
        }
        else if ((obj.position.x + obj.radius) < verticalMidpoint)
        {
            if (topQuadrant)
            {
                index = 2;
            }
            else if (bottomQuadrant)
            {
                index = 0;
            }
        }

        return index;
    }

    public List<Circle> Retrieve(List<Circle> returnObjects, Circle obj)
    {
        return Retrieve(root, returnObjects, obj);
    }

    private List<Circle> Retrieve(Node node, List<Circle> returnObjects, Circle obj)
    {
        int index = GetChildIndex(node, obj);
        if (index != -1 && node.children != null)
        {
            Retrieve(node.children[index], returnObjects, obj);
        }

        returnObjects.AddRange(node.objects);

        return returnObjects;
    }

    public void Clear()
    {
        Clear(root);
    }

    private void Clear(Node node)
    {
        node.objects.Clear();
        if (node.children != null)
        {
            for (int i = 0; i < node.children.Length; i++)
            {
                Clear(node.children[i]);
                node.children[i] = null;
            }
            node.children = null;
        }
    }

    public void DrawGizmos()
    {
        DrawNode(root);
    }

    private void DrawNode(Node node)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(node.bounds.center, new Vector3(node.bounds.width, node.bounds.height, 0));

        if (node.children != null)
        {
            foreach (var child in node.children)
            {
                DrawNode(child);
            }
        }
    }
}
