using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ConsoleApp1
{
    public record DistanceData(Dictionary<string, Dictionary<string, int>> Distances);


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

    public class DistanceGraphProcessing
    {
        private Dictionary<string, Dictionary<string, int>> _graph = new Dictionary<string, Dictionary<string, int>>();

        public DistanceGraphProcessing(Dictionary<string, Dictionary<string, int>> GraphStructure) => _graph = GraphStructure;

        private List<string> reconstructPath(string start, string target, Dictionary<string, string> parents)
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

        private void dfsRecursive(string currentVertex, List<string> output, HashSet<string> visited)
        {
            visited.Add(currentVertex);
            output.Add(currentVertex);
            foreach (var neighbour in _graph[currentVertex])
            {
                if (!visited.Contains(neighbour.Key))
                {
                    dfsRecursive(neighbour.Key, output, visited);
                }
            }
        }

        public List<string> BfsTraversal(string start, string target)
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

                foreach (var neighbour in _graph[currentVertex])
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

        public List<string> DfsTraversal(string start)
        {
            List<string> output = new List<string>();
            HashSet<string> visited = new HashSet<string>();
            dfsRecursive(start, output, visited);
            return output;
        }

        public List<string> DijkstraShortestPath(string start, string target)
        {
            PriorityQueue<string, int> priorityQueue = new PriorityQueue<string, int>();
            Dictionary<string, string> parent = new Dictionary<string, string>();

            var distances = _graph.Keys.ToDictionary(vertexVal => vertexVal, weight => int.MaxValue);

            priorityQueue.Enqueue(start, 0);
            distances[start] = 0;

            while (priorityQueue.TryDequeue(out var currentValue, out var currentDist))
            {
                if (currentDist > distances[currentValue]) continue;

                foreach(var (neighbour, weight) in _graph[currentValue])
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

        public static void GraphTraversal(DistanceData jsonData, string algorithm)
        {
            DistanceGraphProcessing graphProcessing = new DistanceGraphProcessing(jsonData.Distances);
            string? startVertex = Console.ReadLine();
            switch (algorithm)
            {
                case "bfs":
                    {
                        string? targetVertex = Console.ReadLine();
                        graphProcessing.BfsTraversal(startVertex, targetVertex).ForEach(vertex => Console.Write($"{vertex} "));
                        break;
                    }
                case "dfs":
                    graphProcessing.DfsTraversal(startVertex).ForEach(vertex => Console.Write($"{vertex} "));
                    break;
                case "dijkstra":
                    {
                        string? targetVertex = Console.ReadLine();
                        graphProcessing.DijkstraShortestPath(startVertex, targetVertex).ForEach(vertex => Console.Write($"{vertex} "));
                        break;
                    }

            }
        }

        public static void Main(string[] args)
        {
            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            } 
        }
    }
}

