using Src.GraphProcessor;
using Xunit.Abstractions;

namespace GraphProcessorTest
{
    public class AlgorithmTestClass
    {
        // Статический метод с 20 тестовыми графами
        private readonly ITestOutputHelper _output;

        public AlgorithmTestClass(ITestOutputHelper output)
        {
            _output = output;
        }
        public static IEnumerable<object[]> GetGraphTestData()
        {
            // 1. Простой граф
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string,int>>
                {
                    { "A", new Dictionary<string,int>{{"B",1}} },
                    { "B", new Dictionary<string,int>{{"C",2}} },
                    { "C", new Dictionary<string,int>() }
                }
            };

            // 2. Циклический граф
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string,int>>
                {
                    { "A", new Dictionary<string,int>{{"B",2}} },
                    { "B", new Dictionary<string,int>{{"C",3}} },
                    { "C", new Dictionary<string,int>{{"A",4}} }
                }
            };

            // 3. Граф с петлей
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string,int>>
                {
                    { "A", new Dictionary<string,int>{{"A",5}} },
                }
            };

            // 4. Два несвязанных подграфа
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string,int>>
                {
                    { "A", new Dictionary<string,int>{{"B",3}} },
                    { "B", new Dictionary<string,int>() },
                    { "X", new Dictionary<string,int>{{"Y",7}} },
                    { "Y", new Dictionary<string,int>() }
                }
            };

            // 5. Полносвязанный маленький граф
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string,int>>
                {
                    { "A", new Dictionary<string,int>{{"B",1},{"C",1}} },
                    { "B", new Dictionary<string,int>{{"A",1},{"C",1}} },
                    { "C", new Dictionary<string,int>{{"A",1},{"B",1}} }
                }
            };

            // 6. Большие веса
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string,int>>
                {
                    { "A", new Dictionary<string,int>{{"B",9999}} },
                    { "B", new Dictionary<string,int>{{"C",8888}} },
                    { "C", new Dictionary<string,int>{{"A",7777}} }
                }
            };

            // 7. Граф с нулевыми весами
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string, int>>
                {
                    { "A", new Dictionary<string, int>{{"B",0},{"C",0}} },
                    { "B", new Dictionary<string, int>() },
                    { "C", new Dictionary<string, int>() }
                }
            };

            // 8. Отрицательные веса
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string, int>>
                {
                    { "A", new Dictionary<string, int>{{"B",-1}} },
                    { "B", new Dictionary<string, int>{{"C",-2}} },
                    { "C", new Dictionary<string, int>() }
                }
            };

            // 9. Один большой центр
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string, int>>
                {
                    { "Center", new Dictionary<string, int>{{"A",1},{"B",2},{"C",3},{"D",4},{"E",5}} },
                    { "A", new Dictionary<string, int>() },
                    { "B", new Dictionary<string, int>() },
                    { "C", new Dictionary<string, int>() },
                    { "D", new Dictionary<string, int>() },
                    { "E", new Dictionary<string, int>() }
                }
            };

            // 10. Линейная цепь из 6 узлов
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string, int>>
                {
                    { "1", new Dictionary<string, int>{{"2",1}} },
                    { "2", new Dictionary<string, int>{{"3",1}} },
                    { "3", new Dictionary<string, int>{{"4",1}} },
                    { "4", new Dictionary<string, int>{{"5",1}} },
                    { "5", new Dictionary<string, int>{{"6",1}} },
                    { "6", new Dictionary<string, int>() }
                }
            };

            // 11. Две вершины, ребро в одну сторону
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string, int>>
                {
                    { "X", new Dictionary<string, int>{{"Y",5}} },
                    { "Y", new Dictionary<string, int>() }
                }
            };

            // 12. Две вершины, двустороннее ребро
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string,int>>
                {
                    { "X", new Dictionary<string, int>{{"Y",2}} },
                    { "Y", new Dictionary<string, int>{{"X",2}} }
                }
            };

            // 13. Пустой граф
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string, int>>()
            };

            // 14. Один узел без рёбер
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string, int>>
                {
                    { "A", new Dictionary<string, int>() }
                }
            };

            // 15. Несколько узлов без рёбер
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string, int>>
                {
                    { "A", new Dictionary<string, int>() },
                    { "B", new Dictionary<string, int>() },
                    { "C", new Dictionary<string, int>() }
                }
            };

            // 16. Смешанные веса
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string,int>>
                {
                    { "A", new Dictionary<string, int>{{"B",5},{"C",-3}} },
                    { "B", new Dictionary<string, int>{{"C",2}} },
                    { "C", new Dictionary<string, int>() }
                }
            };

            // 17. Звезда с центром в A
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string, int>>
                {
                    { "A", new Dictionary<string, int>{{"B",1},{"C",1},{"D",1},{"E",1}} },
                    { "B", new Dictionary<string, int>() },
                    { "C", new Dictionary<string, int>() },
                    { "D", new Dictionary<string, int>() },
                    { "E", new Dictionary<string, int>() }
                }
            };

            // 18. Треугольник двусторонний
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string, int>>
                {
                    { "A", new Dictionary<string, int>{{"B",1},{"C",1}} },
                    { "B", new Dictionary<string, int>{{"A",1},{"C",1}} },
                    { "C", new Dictionary<string, int>{{"A",1},{"B",1}} }
                }
            };

            // 19. Случайные строковые ключи
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string, int>>
                {
                    { "node1", new Dictionary<string, int>{{"node2",10}} },
                    { "node2", new Dictionary<string, int>{{"node3",20}} },
                    { "node3", new Dictionary<string, int>{{"node1",30}} }
                }
            };

            // 20. Разреженный граф
            yield return new object[]
            {
                new Dictionary<string, Dictionary<string, int>>
                {
                    { "A", new Dictionary<string, int>() },
                    { "B", new Dictionary<string, int>{{"C",1}} },
                    { "C", new Dictionary<string, int>() },
                    { "D", new Dictionary<string, int>{{"A",2}} }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetGraphTestData))]
        public void AlgorithmTesting(Dictionary<string, Dictionary<string, int>> graph)
        {
            var vertexCount = graph.Count;
            var edgeCount = graph.Values.Sum(pair => pair.Count);
            _output.WriteLine($"Starting testing on graph with {vertexCount} vertexes and {edgeCount} edges");
            foreach (var (vertex, neighbours) in graph)
            {
                foreach (var (neighbour, weight) in neighbours)
                {
                    DistanceGraphProcessing.DijkstraShortestPath(graph, vertex, neighbour);
                    DistanceGraphProcessing.BfsTraversal(graph, vertex, neighbour);
                }
                DistanceGraphProcessing.DfsTraversal(graph, vertex);
            }
        }
    }
}