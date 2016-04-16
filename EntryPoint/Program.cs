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

      Console.WriteLine("*R*: Contents of A: ");
        foreach (Vector2 thing in specialBuildings.ToList())
        {
            Console.WriteLine(thing);
        }

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
            Merge2(A, p, q, r);
        }
      return A;
    }

    public static List<Vector2> Merge(List<Vector2> A, int p, int q, int r)
    {
            int LeftN = q;              //set lengths
            int RightN = r - q;

            Console.WriteLine("*R*: LeftN, RightN: " + LeftN + ", " + RightN);

            Vector2[] L = new Vector2[LeftN + 1];       //make arrays with right lengths
            Vector2[] R = new Vector2[RightN + 1];

            for (int i = 0; i < LeftN; i++)             //fill array
            {
                L[i] = A[i + 1];
            }

            L[LeftN].X = float.PositiveInfinity;        //make the last index of left/right array inifnity
            L[LeftN].Y = float.PositiveInfinity;

            for (int j = 0; j < RightN; j++)
            {
                R[j] = A[j];
            }

            R[RightN].X = float.PositiveInfinity;
            R[RightN].Y = float.PositiveInfinity;

            Console.WriteLine("*R*: Contents of R: ");
            foreach (Vector2 thing in R)
            {
                Console.Write(thing);
            }
            Console.WriteLine("");

            Console.WriteLine("*R*: Contents of L: ");
            foreach (Vector2 thing in L)
            {
                Console.Write(thing);
            }
            Console.WriteLine("");

            int RightCount = 0;
            int LeftCount = 0;

            for (int n = 0; n < LeftN + RightN - 2; n++) {      //does this as many times as the lengths of L and R together
                Console.WriteLine("*R*: RightCount, LeftCount: " + RightCount + ", " + LeftCount);
                Console.WriteLine("*R*: left: " + CalculateEuclideanDistance(L[LeftCount]) + ", right: " + CalculateEuclideanDistance(R[RightCount]));
                if (CalculateEuclideanDistance(L[LeftCount]) < CalculateEuclideanDistance(R[RightCount]))   //Checks distance
                {
                    A[n] = L[LeftCount];          //adds the smallest one
                    LeftCount++;
                    Console.WriteLine("*R*: Add Left");
                }
                else if (CalculateEuclideanDistance(L[LeftCount]) >= CalculateEuclideanDistance(R[RightCount]))
                {
                    A[n] = R[RightCount];
                    RightCount++;
                    Console.WriteLine("*R*: Add Right");
                }
            }

            return A;
    }

    public static List<Vector2> Merge2(List<Vector2> A, int p, int q, int r)
    {
            Console.WriteLine("*R*: p: " + p + " q: " + q + " r: " + r);
            int LeftN = q;
            int RightN = r - q;

            Vector2[] L = new Vector2[LeftN + 1];       //make arrays with right lengths
            Vector2[] R = new Vector2[RightN + 1];

            for (int i = 0; i < LeftN; i++)
            {
                L[i] = A[i];
            }

            for (int j = 0; j < RightN; j++)
            {
                R[j] = A[j + q];
            }

            L[LeftN].X = float.PositiveInfinity;        //make the last index of left/right array inifnity
            L[LeftN].Y = float.PositiveInfinity;
            R[RightN].X = float.PositiveInfinity;
            R[RightN].Y = float.PositiveInfinity;

            int LeftCount = 0;
            int RightCount = 0;

            for (int m = 0; m < LeftN + RightN - 2; m++ )
            {
                Console.Write(LeftCount + " " + RightCount + "; ");
                Console.WriteLine(CalculateEuclideanDistance(L[LeftCount]) + ", " + CalculateEuclideanDistance(R[RightCount]));
                if (CalculateEuclideanDistance(L[LeftCount]) < CalculateEuclideanDistance(R[RightCount]))
                {
                    Console.WriteLine("left");
                    A[m] = L[LeftCount];
                    LeftCount += 1;
                }
                else if (CalculateEuclideanDistance(L[LeftCount]) >= CalculateEuclideanDistance(R[RightCount]))
                {
                    Console.WriteLine("right");
                    A[m] = R[RightCount];
                    RightCount += 1;
                }
            }
            Console.WriteLine("");

            Console.WriteLine("Contents of R:");
            foreach (var thing in R)
            {
                Console.Write(thing);
            }
            Console.WriteLine("");

            Console.WriteLine("Contents of L:");
            foreach (var thing in L)
            {
                Console.Write(thing);
            }
            Console.WriteLine("");

            Console.WriteLine("Contents of A:");
            foreach (var thing in A)
            {
                Console.Write(thing);
            }

            return A;
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
            if (Root == null)
            {
                Root = newNode;
                return Root;
            }
            if (comparingOn == "Y")
            {
                if (newNode.getY() < currentRoot.getY())
                {
                    if (currentRoot.getLeftChild() == null)
                    {
                        currentRoot.setLeftChild(newNode);
                        Console.WriteLine("*R*: Added");
                        return newNode;
                    }
                    InsertNode(newNode, currentRoot.getLeftChild(), "X");
                }
                if (newNode.getY() >= currentRoot.getY())
                {
                    if (currentRoot.getRightChild() == null)
                    {
                        currentRoot.setRightChild(newNode);
                        Console.WriteLine("*R*: Added");
                        return newNode;
                    }
                    InsertNode(newNode, currentRoot.getRightChild(), "X");
                }
            }
            if (comparingOn == "X")
            {
                if (newNode.getX() < currentRoot.getX())
                {
                    if (currentRoot.getLeftChild() == null)
                    {
                        currentRoot.setLeftChild(newNode);
                        Console.WriteLine("*R*: Added");
                        return newNode;
                    }
                    InsertNode(newNode, currentRoot.getLeftChild(), "Y");
                }
                if (newNode.getX() >= currentRoot.getX())
                {
                    if (currentRoot.getRightChild() == null)
                    {
                        currentRoot.setRightChild(newNode);
                        Console.WriteLine("*R*: Added");
                        return newNode;
                    }
                    InsertNode(newNode, currentRoot.getRightChild(), "Y");
                }
            }
            return newNode;
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
