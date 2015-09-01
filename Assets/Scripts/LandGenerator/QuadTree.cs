using UnityEngine;
using System.Collections.Generic;

public class QuadTreeNode
{
    public List<object> Objs;

    public Vector2 position;
    public float size;

    public QuadTreeNode node1;
    public QuadTreeNode node2;
    public QuadTreeNode node3;
    public QuadTreeNode node4;

    public QuadTreeNode(Vector2 pos, float newSize)
    {
        position = pos;
        size = newSize;
    }

    public bool Subdivide(float minNodeSize)
    {
        float half = size / 2.0f;

        if (half < minNodeSize)
            return false;

        float quart = size / 4.0f;

        node1 = new QuadTreeNode(position + new Vector2(-quart, -quart), half);
        node2 = new QuadTreeNode(position + new Vector2(quart, -quart), half);
        node3 = new QuadTreeNode(position + new Vector2(-quart, quart), half);
        node4 = new QuadTreeNode(position + new Vector2(quart, quart), half);

        return true;
    }

    public void SubdivideRecursively(float minNodeSize)
    {
        if (Subdivide(minNodeSize) == false)
            return;

        node1.SubdivideRecursively(minNodeSize);
        node2.SubdivideRecursively(minNodeSize);
        node3.SubdivideRecursively(minNodeSize);
        node4.SubdivideRecursively(minNodeSize);
    }

    public bool Contains(Vector2 point)
    {
        float halfSize = size * 0.5f;
        return Mathf.Abs(point.x - position.x) <= halfSize && Mathf.Abs(point.y - position.y) <= halfSize;
    }

    public QuadTreeNode GetNodeNearPoint(ref Vector2 point)
    {
        if (Contains(point) == false)
            return null;

        if (node1 == null)
            return this;

        QuadTreeNode n;

        n = node1.GetNodeNearPoint(ref point);
        if (n != null) return n;

        n = node2.GetNodeNearPoint(ref point);
        if (n != null) return n;

        n = node3.GetNodeNearPoint(ref point);
        if (n != null) return n;

        n = node4.GetNodeNearPoint(ref point);
        if (n != null) return n;

        return null;
    }
}
