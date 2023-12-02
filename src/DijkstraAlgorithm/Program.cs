//Read the input file GraphData.txt which has our graph
/*
 Format
{List of vertexs}
{
Edge connections with the weight
Each vertex is a number by its index

}
 */

using DijkstraAlgorithm;

Queue<string> lines = new Queue<string>();
string line = "";

//Grab all the lines from the text file
try
{
    StreamReader streamReader = new StreamReader("..\\..\\..\\..\\GraphData.txt");

    line = streamReader.ReadLine();

    while (line != null)
    {
        lines.Enqueue(line);
        line = streamReader.ReadLine();
    }

    streamReader.Close();
}
catch (Exception exception)
{
    Console.WriteLine("Execption: " + exception.Message);
    Environment.Exit(0);
}

//First line is the list of vertexs
string[] currentLine = lines.Dequeue().Split(',');

DirectedGraph<string> graph = new DirectedGraph<string>();

for (int i = 0; i < currentLine.Length; i++)
{
    graph.AddVertex(new DVertex<string>(currentLine[i]));
}

//Next lines are the edge connections between the vertexs
while (lines.Count > 0)
{
    currentLine = lines.Dequeue().Split(',');

    int startingPoint = int.Parse(currentLine[0]);
    int endingPoint = int.Parse(currentLine[1]);
    int weight = int.Parse(currentLine[2]);

    graph.AddEdge(graph[startingPoint], graph[endingPoint], weight);
}

Console.WriteLine("Graph Connections:" + graph.ToString());

//Get the shortest path
var path = (Stack<DVertex<string>>)graph.Dijkstra(graph[0], graph[graph.Count - 1]);
int count = path.Count;

Console.WriteLine($"Short test path from: {graph[0].Value} to {graph[graph.Count - 1].Value}");

//Display the returned path
for (int i = 0; i < count; i++)
{
    Console.Write($"{path.Pop().Value}{(i == count - 1 ? "\n" : "->")}");
}

Console.ReadKey();