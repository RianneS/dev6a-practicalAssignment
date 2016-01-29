using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

public class Node
{
    private Vector2 Coordinates;
    private Node LeftChild;
    private Node RightChild;

    public Node(Vector2 Coords)
	{
        Coordinates = Coords;
	}

    public Vector2 getCoordinates()
    {
        return Coordinates;
    }

    public float getY()
    {
        float Y = Coordinates.Y;
        return Y;
    }

    public float getX()
    {
        float X = Coordinates.X;
        return X;
    }

    public void setLeftChild(Node Child)
    {
        LeftChild = Child;
    }

    public Node getLeftChild()
    {
        return LeftChild;
    }

    public void setRightChild(Node Child)
    {
        RightChild = Child;
    }

    public Node getRightChild()
    {
        return RightChild;
    }
}
