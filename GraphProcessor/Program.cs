using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace GraphProcessor
{
    // JSON data series
    public record DistanceData(Dictionary<string, Dictionary<string, int>> Distances); 


    // Loading JSON from file and deserializing class
    public static class DistanceDataLoader 
    {
        public static DistanceData JsonLoadFromFile(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("File not found");
            string jsonText = File.ReadAllText(filename);
            DistanceData? distanceData = JsonSerializer.Deserialize<DistanceData>(jsonText);
            if (distanceData == null)
                throw new Exception("Deserialization Error");
            return distanceData;
        }
    }

    // Basic graph algorithms implementations
    // 1. Depth-First-search
    // 2. Breadth-First-search
    // 3. Dijktra
    public static class DistanceGraphProcessing 
    {
        // Reconstruct shortest path
        private static List<string> reconstructPath(string start, string target, Dictionary<string, string> parents) 
        {
            List<string> output = new List<string>();
            string current = target;
            while (current != start)
            {
                output.Add(current);
                current = parents[current];
            }
            output.Add(current);
            output.Reverse();
            return output;
        }

        // Recursive Depth fisrt search function
        private static void dfsRecursive(Dictionary<string, Dictionary<string, int>> graph, string currentVertex, List<string> output, HashSet<string> visited)
        {
            visited.Add(currentVertex);
            output.Add(currentVertex);
            foreach (var neighbour in graph[currentVertex])
            {
                if (!visited.Contains(neighbour.Key))
                {
                    dfsRecursive(graph, neighbour.Key, output, visited);
                }
            }
        }

        public static List<string> BfsTraversal(Dictionary<string, Dictionary<string, int>> graph, string start, string target)
        {
            if (start == target) return new List<string>(){start};

            Queue<string> queue = new Queue<string>();
            HashSet<string> visited = new HashSet<string>();
            Dictionary<string, string> parents = new Dictionary<string, string>();

            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count != 0) 
            {
                string currentVertex = queue.Dequeue();

                if (currentVertex == target)
                {
                    List<string> output = reconstructPath(start, target, parents);
                    return output;
                }

                foreach (var neighbour in graph[currentVertex])
                {
                    if (!visited.Contains(neighbour.Key))
                    {
                        parents[neighbour.Key] = currentVertex;
                        visited.Add(neighbour.Key);
                        queue.Enqueue(neighbour.Key);
                    }
                }
            }

            return new List<string>();
        }

        public static List<string> DfsTraversal(Dictionary<string, Dictionary<string, int>> graph, string start)
        {
            List<string> output = new List<string>();
            HashSet<string> visited = new HashSet<string>();
            dfsRecursive(graph, start, output, visited);
            return output;
        }

        public static List<string> DijkstraShortestPath(Dictionary<string, Dictionary<string, int>> graph, string start, string target)
        {
            PriorityQueue<string, int> priorityQueue = new PriorityQueue<string, int>();
            Dictionary<string, string> parent = new Dictionary<string, string>();

            var distances = graph.Keys.ToDictionary(vertexVal => vertexVal, weight => int.MaxValue);

            priorityQueue.Enqueue(start, 0);
            distances[start] = 0;

            while (priorityQueue.TryDequeue(out var currentValue, out var currentDist))
            {
                if (currentDist > distances[currentValue]) continue;

                foreach(var (neighbour, weight) in graph[currentValue])
                {
                    int newDistance = currentDist + weight;
                    if (newDistance < distances[neighbour])
                    {
                        distances[neighbour] = newDistance;
                        priorityQueue.Enqueue(neighbour, newDistance);
                        parent[neighbour] = currentValue;
                    }
                }
            }
            List<string> resultPath = reconstructPath(start, target, parent);
            return resultPath;
        }
    }

    class Program
    {

        // Graph algorithm selection by command line argument
        public static void GraphTraversal(DistanceData jsonData, string algorithm)
        {
            var graph = jsonData.Distances;
            string? startVertex = Console.ReadLine();

            switch (algorithm)
            {
                case "bfs":
                    {
                        string? targetVertex = Console.ReadLine();
                        DistanceGraphProcessing.BfsTraversal(graph, startVertex, targetVertex).ForEach(vertex => Console.Write($"{vertex} "));
                        break;
                    }
                case "dfs":
                    DistanceGraphProcessing.DfsTraversal(graph, startVertex).ForEach(vertex => Console.Write($"{vertex} "));
                    break;
                case "dijkstra":
                    {
                        string? targetVertex = Console.ReadLine();
                        DistanceGraphProcessing.DijkstraShortestPath(graph, startVertex, targetVertex).ForEach(vertex => Console.Write($"{vertex} "));
                        break;
                    }
            }
        }

        public static void Main(string[] args)
        {
            try
            {
                // Reading filename and algorithm name strings
                // Loading data from JSON and deserializing
                string? filename = args[0], algorithm = args[1];
                var jsonData = DistanceDataLoader.JsonLoadFromFile(filename);
                GraphTraversal(jsonData, algorithm);
                Console.WriteLine();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found");
                return;
            }
            catch (JsonException)
            {
                Console.WriteLine("JSON Deserialization error");
                return;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Command is incorrect");
                return;
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Vertex not found in dictionary");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}

