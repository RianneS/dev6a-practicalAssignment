using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

//Solution by Rianne

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

    private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(
      IEnumerable<Vector2> specialBuildings, 
      IEnumerable<Tuple<Vector2, float>> housesAndDistances)
    {

      Console.WriteLine("*R*: " + specialBuildings + " Length: " + specialBuildings.Count());
      Console.WriteLine("*R*: " + housesAndDistances + " Length: " + housesAndDistances.Count() + " 1: " + housesAndDistances.ToList()[0].Item2);

      CreateKdTree(specialBuildings.ToList());
      return TraverseTree(housesAndDistances.ToList());

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
      Console.WriteLine("*R* specialBuildings[0], 1: " + specialBuildings[0] + " Y: " + specialBuildings[0].Y);
      
      //This loop creates a  (one for every specialBuilding) and then adds it to the tree
      foreach (var specialBuilding in specialBuildings){
            Node newNode = new Node(specialBuilding);
            InsertNode(newNode, Root, "X");  
      }
    }

    private static Node InsertNode(Node newNode, Node currentRoot, string comparingOn)
    {
        Console.WriteLine("*R* new coords: " + newNode.getCoordinates());
        if (Root == null)
        {
            Root = newNode;     //If Root is empty, this method gives the Root the value of the new node (so this only happens the first time)
            return Root;
        }
        if (comparingOn == "Y")        //This method compares alternately on X and then on Y, this is the part that happens when it's comparing on Y
        {
            if (newNode.getY() < currentRoot.getY())    //This checks if the new node is smaller than the current one
            {
                if (currentRoot.getLeftChild() == null)     //If it is, if checks if the left child of the current node is null
                {
                    currentRoot.setLeftChild(newNode);      //If it isn't, it adds the new node to that spot, and returns it
                    return currentRoot;
                }
                InsertNode(newNode, currentRoot.getLeftChild(), "X");   //If the spot isn't empty, it calls the method again, with the next node down, and now comparing on X
            }
            else if (newNode.getY() >= currentRoot.getY())      //This checks if the new node is bigger than the current one
            {
                if (currentRoot.getRightChild() == null)
                {
                    currentRoot.setRightChild(newNode);
                    return currentRoot;
                }
                InsertNode(newNode, currentRoot.getRightChild(), "X");
            }
        }
        if (comparingOn == "X") //This is the part that happens when it's comparing on X, it's almost identical to the part that happens for Y
            {
            if (newNode.getX() < currentRoot.getX())
            {
                if (currentRoot.getLeftChild() == null)
                {
                    currentRoot.setLeftChild(newNode);
                    return currentRoot;
                }
                InsertNode(newNode, currentRoot.getLeftChild(), "Y");
            }
            else if (newNode.getX() >= currentRoot.getX())
            {
                if (currentRoot.getRightChild() == null)
                {
                    currentRoot.setRightChild(newNode);
                    return currentRoot;
                }
                InsertNode(newNode, currentRoot.getRightChild(), "Y");
            }
        }
        return Root;
    }

    private static List<List<Vector2>> TraverseTree(List<Tuple<Vector2, float>> houses)
    {
        List<Vector2> HousesInRange = new List<Vector2> { };
        List<List<Vector2>> HousesInRangeLists = new List<List<Vector2>> { };

        //This loop creates a list of buildings that are in range for every house
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

    //This method checks if the given specialbuilding is in the range of the given house
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
    private static Node2 firstNode;
    private static List<Node2> nodeList = new List<Node2>();

    private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding, 
      Vector2 destinationBuilding, IEnumerable<Tuple<Vector2, Vector2>> roads)
    {
      Console.WriteLine("*R*" + startingBuilding + " " + destinationBuilding);
      /*foreach(Tuple<Vector2, Vector2> road in roads)
            {
                Console.WriteLine("*R* Road: " + road);
            }*/
      //var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
      //List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
      //var prevRoad = startingRoad;
      //for (int i = 0; i < 30; i++)
      //{
      //  prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, destinationBuilding)).First());
      //  fakeBestPath.Add(prevRoad);
      //}
      //return fakeBestPath;
      createAdjacencyMatrix(roads);

      List<Tuple<Vector2, Vector2>> bestPath = findShortestRoute(roads, startingBuilding, destinationBuilding);
      return bestPath;  //is List<Tuple<Vector2, Vector2>>
    }
    
    private static void createAdjacencyMatrix(IEnumerable<Tuple<Vector2, Vector2>> roads)
    {
        foreach(Tuple<Vector2, Vector2> road in roads)
        {
            Node2 currentNode2 = checkFirstPoint(road);
            addNeighbour(currentNode2, road); 
        }
    }

    private static Node2 checkFirstPoint(Tuple<Vector2, Vector2> road)
    {
        Node2 currentNode2;
        Console.WriteLine("*R* points to add, 1: " + road.Item1 + " 2: " + road.Item2);
        //See if the first point of the road is in the list
        int index = nodeList.FindIndex(p => p != null && p.getX() == road.Item1.X && p.getY() == road.Item1.Y);
        if (index >= 0)
        {
            //if it is, get it from the list
            currentNode2 = nodeList[index];
            //Console.WriteLine("*R* 1st point exists; number" + index + " in the list");
        }
        else
        {
            //if it isn't, create the node and add it to the list
            Node2 newNode = new Node2(road.Item1);
            if (firstNode == null)                  //If first node is empty
            {
                firstNode = newNode;
            }
            nodeList.Add(newNode);
            currentNode2 = newNode;
            //Console.WriteLine("*R* 1st point doesn't exist, created");
        }
        return currentNode2;
    }

    private static void addNeighbour(Node2 currentNode2, Tuple<Vector2, Vector2> road)
    {
        //See if the second point of the road is in the list
        int index2 = nodeList.FindIndex(q => q != null && q.getX() == road.Item2.X && q.getY() == road.Item2.Y);
        if (index2 >= 0)
        {
            //if it is, get it from the list and add it as a neighbour of the first point
            currentNode2.addNeighbour(nodeList[index2]);
            //Console.WriteLine("*R* 2nd point exists; number" + index2 + " in the list, added as neighbour");
        }
        else
        {
            //if it isn't, create it and add it as neighbour of the first point
            Node2 newNode = new Node2(road.Item2);
            currentNode2.addNeighbour(newNode);
            nodeList.Add(newNode);
            //Console.WriteLine("*R* 2nd point doesn't exist, created, added as neighbour");
        }
    }

    private static List<Tuple<Vector2, Vector2>> findShortestRoute(IEnumerable<Tuple<Vector2, Vector2>> roads, Vector2 start, Vector2 destination)
    {
        List<Tuple<Vector2, Vector2>> shortestRoute = new List<Tuple<Vector2, Vector2>>();
        Node2 startingHouse = nodeList.Find(r => r.getX() == start.X && r.getY() == start.Y);
        Node2 destinationHouse = nodeList.Find(s => s.getX() == destination.X && s.getY() == destination.Y);
        Node2 currentHouse = startingHouse;
        List<Node2> unvisitedNodes = nodeList;

        startingHouse.setVisited();
        startingHouse.setDistance(0);

        Console.WriteLine("*R* New nodes for path thingy: " + startingHouse.getX() + ", " + startingHouse.getY() + " 2: " + destinationHouse.getX() + ", " + destinationHouse.getY());

        //check all neighbours of the node, and set to smallest distances,
        while (unvisitedNodes.Count() > 0) { 
            Console.WriteLine("*R* 1");
            foreach(Node2 neighbour in currentHouse.getNeighbours())
            {
                Console.WriteLine("*R* 2");
                if ((currentHouse.getDistance() + 1) < neighbour.getDistance())
                {
                    Console.WriteLine("*R* 3");
                    neighbour.setDistance(currentHouse.getDistance() + 1);
                    Console.WriteLine("*R* 4");
                }
                Console.WriteLine("*R* 5");
            }
            currentHouse.setVisited();
            
            Console.WriteLine("*R* 6");
            //check if there's any unvisited neighbours
            int visitedIndex = currentHouse.getNeighbours().FindIndex(k => k.getVisited() == false);
            if (visitedIndex >= 0) {
                Console.WriteLine("*R* 7");
                List<Node2> candidates = currentHouse.getNeighbours().FindAll(k => k.getVisited() == false);

                Console.WriteLine("*R*: " + candidates[0]);

                Node2 nextHouse = candidates.OrderBy(k => k.getDistance()).First();
                
                Console.WriteLine("*R* 8");

                unvisitedNodes.Remove(currentHouse);

                shortestRoute.Add(Tuple.Create(new Vector2(currentHouse.getX(), currentHouse.getY()), new Vector2(nextHouse.getX(), nextHouse.getY())));
                Console.WriteLine("*R*" + shortestRoute[0].Item1 + " | " + shortestRoute[0].Item2);
                currentHouse = nextHouse;
            } else
            {
                break;
            }
        }
        return shortestRoute.ToList();
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
