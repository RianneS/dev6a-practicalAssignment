using Microsoft.Xna.Framework;
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

      Console.WriteLine("*R*: " + specialBuildings.Count());

      return MergeSort(specialBuildings.ToList(), 0, specialBuildings.Count());

      //return specialBuildings.OrderBy(v => Vector2.Distance(v, house));
    }

    //Methods added by me for Excercise 1:

    private static double CalculateEuclideanDistance(Vector2 specialBuilding)
    {
      double distance = Math.Sqrt((Math.Pow((currentHouse.X - specialBuilding.X), 2) + (Math.Pow((currentHouse.Y - specialBuilding.Y), 2))));
      return distance;
     }  //TODO make this return 1 distance?
    
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
      //TODO

      int LeftN = q - p + 1;    //calculate how long the arrays are going to be
      int RightN = r - q;
      
      Vector2[] L = new Vector2[LeftN + 1];   //make arrays with the right lengths
      Vector2[] R = new Vector2[RightN + 1];
      
      for(var i = 0; i < LeftN; i++)    //fill the arrays with the right numbers
        {
            L[i] = A[p + i];
        }
      
      //Console.WriteLine("*R*: leftn: " + LeftN + ", L length: " + L.Length + ", rightn: " + RightN + ", R length: " + R.Length + ", A length" + A.Length);

      L[LeftN] = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
      
      Console.WriteLine("*R*: contents of L");
      foreach(var item in L)
        {
            Console.WriteLine(item);
        }   //TODO delete test

      for(var j = 0; j < RightN; j++)
        {
            R[j] = A[q + j];
        }

      R[RightN] = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

      Console.WriteLine("*R*: contents of R");
      foreach(var item in R)
        {
            Console.WriteLine(item);
        }   //TODO delete test

      Console.WriteLine("*R*: ");

      //TODO sort the things

      int LCounter = 0;
      int RCounter = 0;

      for(var k = p; k <= r; k++)
        {
            Console.WriteLine(L[LCounter] + R[RCounter]);
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

    private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(
      IEnumerable<Vector2> specialBuildings, 
      IEnumerable<Tuple<Vector2, float>> housesAndDistances)
    {

      Console.WriteLine("*R*: " + specialBuildings);
      Console.WriteLine("*R*: " + housesAndDistances);

      return
          from h in housesAndDistances
          select
            from s in specialBuildings
            where Vector2.Distance(h.Item1, s) <= h.Item2
            select s;
    }

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
