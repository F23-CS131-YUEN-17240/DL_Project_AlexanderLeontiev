using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraAlgorithm
{
    public class DirectedGraph<T> : BaseGraph<T, DVertex<T>> where T : IComparable<T>
    {
        public List<DVertex<T>> Vertices { get; private set; }
        public List<Edge<T>> Edges { get; private set; }

        public DVertex<T> this[int index]
        {
            get { return Vertices[index]; }
            set { Vertices[index] = value; }
        }

        public int Count
        {
            get { return Vertices.Count; }
        }

        public DirectedGraph()
            : base()
        {
            Vertices = new List<DVertex<T>>();
            Edges = new List<Edge<T>>();
        }

        //Adding a vertex to the graph
        public override void AddVertex(DVertex<T> vertex)
        {
            if (Vertices.Contains(vertex))
            {
                throw new Exception("Vertex already exists");
            }

            Vertices.Add(vertex);
        }

        //Adding an edge to the graph
        public override bool AddEdge(DVertex<T> a, DVertex<T> b, int distance)
        {
            //If the edge already exists do not add it
            if (GetEdge(a, b) != null)
            {
                return false;
            }

            //Make b one of the neighbors to a and add it to overall list of edges
            a.Neighbors.Add(new Edge<T>(a, b, distance));
            Edges.Add(a.Neighbors[a.Count - 1]);
            return true;
        }

        //Removing a vertex from the graph
        public override bool RemoveVertex(DVertex<T> vertex)
        {
            //Check if it exists first
            if (!Vertices.Contains(vertex))
            {
                return false;
            }

            //Remove all the connections that the vertex has
            for (int i = 0; i < Edges.Count; i++)
            {
                //If the vertex is a starting point remove its connection
                if (Edges[i].StartingPoint == vertex)
                {
                    Edges.RemoveAt(i);
                    i--;
                }
                //If the vertex is an ending point to another vertex remove its connection as well
                else if (Edges[i].EndingPoint == vertex)
                {
                    Edges[i].StartingPoint.Neighbors.RemoveAt(Edges[i].StartingPoint.Neighbors.FindIndex((a) => { return a.EndingPoint == vertex; }));
                    Edges.RemoveAt(i);
                    i--;
                }
            }

            //Remove it from the list of vertices
            Vertices.Remove(vertex);

            return true;
        }

        //Removing an Edge from the list of Edges
        public override bool RemoveEdge(DVertex<T> a, DVertex<T> b)
        {
            //First check if an edge exists
            Edge<T> temp = GetEdge(a, b);
            if (temp == null)
            {
                return false;
            }

            //If it exists then remove the connection from the starting vertex
            a.Neighbors.Remove(temp);
            Edges.Remove(temp);
            return true;
        }

        //Find the vertex with a certain value
        public override DVertex<T> Search(T value)
        {
            return Vertices[Vertices.FindIndex((a) => { return a.Value.CompareTo(value) == 0; })];
        }

        //Find an edge with a certain starting and ending point
        public Edge<T> GetEdge(DVertex<T> a, DVertex<T> b)
        {
            //If the vertices exist see if there is a connection from the starting and ending point
            if ((Vertices.Contains(a) && Vertices.Contains(b)))
            {
                for (int i = 0; i < Edges.Count; i++)
                {
                    if (Edges[i].StartingPoint == a && Edges[i].EndingPoint == b)
                    {
                        return Edges[i];
                    }
                }
            }

            return null;
        }

        //Finding the shortest path between 2 points in a graph using Dijkstra
        public IEnumerable<DVertex<T>> Dijkstra(DVertex<T> start, DVertex<T> end)
        {
            //Check first if the starting and ending point exist
            if (!Vertices.Contains(start) && !Vertices.Contains(end))
            {
                return null;
            }

            //Keep track of how we got to a certain vertex by having a list of finding vertex with the weight
            var info = new Dictionary<DVertex<T>, (DVertex<T> founder, int weight)>();

            //Create a priority queue where we compare the weights of each vertex
            var queue = new PriorityQueue<DVertex<T>>(Comparer<DVertex<T>>.Create((a, b) => info[a].weight.CompareTo(info[b].weight)));

            //Add all the vertices to the dictionary and set them with the default values
            for (int i = 0; i < Count; i++)
            {
                this[i].IsVisited = false;
                info.Add(this[i], (null, int.MaxValue));
            }

            //Set the starting point to a null founder with a weight of 0
            info[start] = (null, 0);

            //Add the starting point to the queue
            queue.Enqueue(start);

            //Keep going through queue until it is empty
            while (!queue.IsEmpty())
            {
                //Grab the next smallest vertex from the priority queue and mark it as visisted
                var vertex = queue.Dequeue();
                vertex.IsVisited = true;
                
                //Go through all the neighbors that current vertex has
                foreach (var edge in vertex.Neighbors)
                {
                    //Grab the connecting vertex
                    var neighbor = edge.EndingPoint;

                    //Add the current total weight with the next weight (current weight on endpoint)
                    int tentative = edge.Weight + info[vertex].weight;

                    //If the new total weight is less than current total weight
                    //Then update the founder values with the new total weight
                    //Mark the neighbor as unvisited because it is not the shortest path to that vertex
                    if (tentative < info[neighbor].weight)
                    {
                        info[neighbor] = (vertex, tentative);
                        neighbor.IsVisited = false;
                    }

                    //If they are not in the queue AND have not been visited, add them to the queue
                    if (!queue.Contains(neighbor) && !neighbor.IsVisited)
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }

            //Start at the end and build a stack of founders back to beginning
            Stack<DVertex<T>> stack = new Stack<DVertex<T>>();

            var current = info.ElementAt(info.Count - 1);
            while (true)
            {
                stack.Push(current.Key);

                //Stop looping once we have reached the start
                if (current.Value.founder == null)
                {
                    break;
                }

                current = new KeyValuePair<DVertex<T>, (DVertex<T> founder, int distance)>(current.Value.founder, info[current.Value.founder]);
            }

            return stack;
        }

        public override string ToString()
        {
            string text = "";

            foreach (var edge in Edges)
            {
                text += $"{edge.StartingPoint.Value} __{edge.Weight}__> {edge.EndingPoint.Value}\n"; 
            }

            return text;
        }
    }
}
