﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

//test comment!

namespace EntryPoint
{
#if WINDOWS || LINUX
  public static class Program
  {
    [STAThread]
    static void Main()
    {
      var fullscreen = false;
      read_input:
      switch (Microsoft.VisualBasic.Interaction.InputBox("Which assignment shall run next? (1, 2, 3, 4, or q for quit)", "Choose assignment", VirtualCity.GetInitialValue()))
      {
        case "1":
          using (var game = VirtualCity.RunAssignment1(SortSpecialBuildingsByDistance, fullscreen))
            game.Run();
          break;
        case "2":
          using (var game = VirtualCity.RunAssignment2(FindSpecialBuildingsWithinDistanceFromHouse, fullscreen))
            game.Run();
          break;
        case "3":
          using (var game = VirtualCity.RunAssignment3(FindRoute, fullscreen))
            game.Run();
          break;
        case "4":
          using (var game = VirtualCity.RunAssignment4(FindRoutesToAll, fullscreen))
            game.Run();
          break;
        case "q":
          return;
      }
      goto read_input;
    }

    /*private static IEnumerable<Vector2> SortSpecialBuildingsByDistance(Vector2 house, IEnumerable<Vector2> specialBuildings)
    {
      return specialBuildings.OrderBy(v => Vector2.Distance(v, house));
    }*/

    private static Vector2 currentHouse;       //Make house a global variable for later use

    private static IEnumerable<Vector2> SortSpecialBuildingsByDistance(Vector2 house, IEnumerable<Vector2> specialBuildings)
    {
      currentHouse = house;
      Console.WriteLine("*R*: " + house);
      Console.WriteLine("*R*: " + specialBuildings);

      return MergeSort(specialBuildings.ToList(), 0, (specialBuildings.Count() - 1));

      //return specialBuildings.OrderBy(v => Vector2.Distance(v, house));
    }

    //Methods added by me for Excercise 1:

    private static double CalculateEuclideanDistance(Vector2 specialBuilding)
    {
      double distance = Math.Sqrt((Math.Pow((currentHouse.X - specialBuilding.X), 2) + (Math.Pow((currentHouse.Y - specialBuilding.Y), 2))));
      return distance;
     }
    
    private static List<Vector2> MergeSort(List<Vector2> A, int p, int r)
    {
      if(p < r)
        {
            int q = (p + r) / 2;
            MergeSort(A, p, q);
            MergeSort(A, q + 1, r);
            Merge(A, p, q, r);
        }
      return A;
    }

    private static List<Vector2> Merge(List<Vector2> A, int p, int q, int r)
    {
      int LeftN = q - p + 1;    //calculate how long the arrays are going to be
      int RightN = r - q;
      Vector2[] L = new Vector2[LeftN + 1];   //make arrays with the right lengths
      Vector2[] R = new Vector2[RightN + 1];
      
      for(var i = 0; i < LeftN; i++)    //fill the arrays with the right numbers
        {
            L[i] = A[p + i];
        }
      
      L[LeftN] = new Vector2(float.PositiveInfinity, float.PositiveInfinity);   //add a infinity to the end of the list for later use in comparing

      for(var j = 0; j < RightN; j++)
        {
            Console.WriteLine("*R*: " + A[j] + " 2: " + A[q + j]);
            R[j] = A[q + j + 1];
        }

      R[RightN] = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

      int LCounter = 0;
      int RCounter = 0;

      for(var k = p; k <= r; k++)
        {
            double distanceL = CalculateEuclideanDistance(L[LCounter]);
            double distanceR = CalculateEuclideanDistance(R[RCounter]);
            if (distanceL <= distanceR)
                {
                    A[k] = L[LCounter];
                    LCounter++;
                }
            else
                {
                    A[k] = R[RCounter];
                    RCounter++;
                }
        }

      return A.ToList<Vector2>();
    }

    //End methods excercise 1

    //Make a root global variable that will store the root of my tree, for later use
    private static Node Root;
    //private static List<List<Vector2>> HousesInRangeGlobal;

    private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(
      IEnumerable<Vector2> specialBuildings, 
      IEnumerable<Tuple<Vector2, float>> housesAndDistances)
    {

      Console.WriteLine("*R*: " + specialBuildings + " Length: " + specialBuildings.Count());
      Console.WriteLine("*R*: " + housesAndDistances + " Length: " + housesAndDistances.Count() + " 1: " + housesAndDistances.ToList()[0].Item2);

      CreateKdTree(specialBuildings.ToList());
      return TraverseTree(housesAndDistances.ToList());
      
      //return HousesInRangeGlobal;
      
      //return
      //    from h in housesAndDistances
      //    select
      //      from s in specialBuildings
      //      where Vector2.Distance(h.Item1, s) <= h.Item2
      //      select s;
    }

    //Methods added by me for exercise 2

    private static void CreateKdTree(List<Vector2> specialBuildings)
    {
      List<Vector2> specialBuildingsTest = new List<Vector2>(){};
      specialBuildingsTest.Add(specialBuildings[19]);
      specialBuildingsTest.Add(specialBuildings[2]);
      specialBuildingsTest.Add(specialBuildings[35]);
      specialBuildingsTest.Add(specialBuildings[37]);
      specialBuildingsTest.Add(specialBuildings[1]);

      Console.WriteLine("*R* specialBuildings[0], 1: " + specialBuildings[0] + " Y: " + specialBuildings[0].Y);
      
      //This loop creates a node and then adds it to the tree
      foreach (var specialBuilding in specialBuildings){
            Node newNode = new Node(specialBuilding);
            InsertNode(newNode, Root, "X");  
      }

      Console.WriteLine("*R* Root coordinates: " + Root.getCoordinates() + " LeftChild: " + Root.getLeftChild() + " Right: " + Root.getRightChild());
    }

    private static Node InsertNode(Node newNode, Node currentRoot, string comparingOn)
    {
        Console.WriteLine("*R* new coords: " + newNode.getCoordinates());
        if (Root == null)
        {
            Console.WriteLine("Root is null");
            Root = newNode;
            return Root;
        }
        Console.WriteLine("*R*: currentRoot " + currentRoot.getCoordinates() + " C: " + comparingOn);
        if (comparingOn == "Y")
        {
            Console.WriteLine("*R*: Y");
            if (newNode.getY() < currentRoot.getY())
            {
                Console.WriteLine("*R*: smaller");
                if (currentRoot.getLeftChild() == null)
                {
                    Console.WriteLine("*R*: add");
                    currentRoot.setLeftChild(newNode);
                    return currentRoot;
                }
                InsertNode(newNode, currentRoot.getLeftChild(), "X");
            }
            else if (newNode.getY() >= currentRoot.getY())
            {
                Console.WriteLine("*R*: bigger");
                if (currentRoot.getRightChild() == null)
                {
                    Console.WriteLine("*R*: add");
                    currentRoot.setRightChild(newNode);
                    return currentRoot;
                }
                InsertNode(newNode, currentRoot.getRightChild(), "X");
            }
        }
        if (comparingOn == "X")
        {
            Console.WriteLine("*R*: X");
            if (newNode.getX() < currentRoot.getX())
            {
                Console.WriteLine("*R*: smaller");
                if (currentRoot.getLeftChild() == null)
                {
                    Console.WriteLine("*R*: add");
                    currentRoot.setLeftChild(newNode);
                    return currentRoot;
                }
                InsertNode(newNode, currentRoot.getLeftChild(), "Y");
            }
            else if (newNode.getX() >= currentRoot.getX())
            {
                Console.WriteLine("*R*: bigger");
                if (currentRoot.getRightChild() == null)
                {
                    Console.WriteLine("*R*: add");
                    currentRoot.setRightChild(newNode);
                    return currentRoot;
                }
                InsertNode(newNode, currentRoot.getRightChild(), "Y");
            }
        }
        Console.WriteLine("*R*: Why is it here?");
        return Root;
    }

    private static List<List<Vector2>> TraverseTree(List<Tuple<Vector2, float>> houses)
    {
        List<Vector2> HousesInRange = new List<Vector2> { };
        List<List<Vector2>> HousesInRangeLists = new List<List<Vector2>> { };
        foreach (Tuple<Vector2,float> house in houses)
        {
                HousesInRangeLists.Add(Traverse(house, Root, HousesInRange));
        }
        return HousesInRangeLists;
    }

    private static List<Vector2> Traverse(Tuple<Vector2, float> house, Node currentNode, List<Vector2> HousesInRange)
    {
        if (IsInRange(currentNode.getCoordinates(), house.Item1, house.Item2))
            {
                Console.WriteLine("*R* Traverse add to list: " + currentNode.getCoordinates());
                HousesInRange.Add(currentNode.getCoordinates());
            }
        if (currentNode.getLeftChild() != null)
        {
            Traverse(house, currentNode.getLeftChild(), HousesInRange);
        }
        if (currentNode.getRightChild() != null)
        {
            Traverse(house, currentNode.getRightChild(), HousesInRange);
        }
        return HousesInRange;
    }

    private static bool IsInRange(Vector2 specialBuilding, Vector2 house, float distance)
    {
        if ((Math.Sqrt((Math.Pow((house.X - specialBuilding.X), 2) + (Math.Pow((house.Y - specialBuilding.Y), 2))))) < distance)
        {
            return true;
        }
        else{
            return false;
        }
     }

    //End methods excercise 2

    private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding, 
      Vector2 destinationBuilding, IEnumerable<Tuple<Vector2, Vector2>> roads)
    {
      var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
      List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
      var prevRoad = startingRoad;
      for (int i = 0; i < 30; i++)
      {
        prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, destinationBuilding)).First());
        fakeBestPath.Add(prevRoad);
      }
      return fakeBestPath;
    }

    private static IEnumerable<IEnumerable<Tuple<Vector2, Vector2>>> FindRoutesToAll(Vector2 startingBuilding, 
      IEnumerable<Vector2> destinationBuildings, IEnumerable<Tuple<Vector2, Vector2>> roads)
    {
      List<List<Tuple<Vector2, Vector2>>> result = new List<List<Tuple<Vector2, Vector2>>>();
      foreach (var d in destinationBuildings)
      {
        var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
        List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
        var prevRoad = startingRoad;
        for (int i = 0; i < 30; i++)
        {
          prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, d)).First());
          fakeBestPath.Add(prevRoad);
        }
        result.Add(fakeBestPath);
      }
      return result;
    }
  }
#endif
}
