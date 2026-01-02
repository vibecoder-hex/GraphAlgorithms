using System;
using System.Text.Json;

using Src.GraphProcessor;
using Src.DataLoader;

namespace MainProgram
{
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
