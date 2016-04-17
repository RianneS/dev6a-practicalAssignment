using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

public class Node2
{
    private Vector2 Coordinates;
    private List<Node2> Neighbours;
    private bool Visited = false;

    public Node2(Vector2 Coords)
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

    public void addNeigbour(Node2 Neighbour)
    {
        Neighbours.Add(Neighbour);
    }

    public List<Node2> getNeighbours()
    {
        return Neighbours;
    }

    public void setVisited()
    {
        Visited = true;
    }
}
