using System.Text.Json;

namespace Src.DataLoader
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
}