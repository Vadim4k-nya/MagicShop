using MagicShop.Artifacts;
using MagicShop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MagicShop.Processors
{
    public class JsonProcessor<T> : IDataProcessor<T> where T : Artifact
    {
        public List<T> LoadData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"ашибка: json не найден {filePath}");
                return new List<T>();
            }
            try
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ашибка загрузки json из {filePath}: {ex.Message}");
                return new List<T>();
            }
        }

        public void SaveData(List<T> data, string filePath)
        {
            try
            {
                string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, json);
                Console.WriteLine($"Данные сохранены в json: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ашибка сохранения json {filePath}: {ex.Message}");
            }
        }
    }
}
