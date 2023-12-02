using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraAlgorithm
{
    public class Edge<T> where T : IComparable<T>
    {
        //Edge is weighted and has a starting and end point

        public DVertex<T> StartingPoint;
        public DVertex<T> EndingPoint;
        public int Weight;
        public bool IsVisited = false;

        public Edge(DVertex<T> startingPoint, DVertex<T> endingPoint, int weight)
        {
            StartingPoint = startingPoint;
            EndingPoint = endingPoint;
            Weight = weight;
        }
    }
}
