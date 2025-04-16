using MagicShop.Artifacts;
using MagicShop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MagicShop.Processors
{
    public class XmlProcessor<T> : IDataProcessor<T> where T : Artifact
    {
        public List<T> LoadData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"xml не найден {filePath}");
                return new List<T>();
            }
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    return (List<T>)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ашибка xlm загрузки из {filePath}: {ex.Message}");
                return new List<T>();
            }
        }

        public void SaveData(List<T> data, string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    var settings = new System.Xml.XmlWriterSettings { Indent = true };
                    using (var writer = System.Xml.XmlWriter.Create(fs, settings))
                    {
                        serializer.Serialize(writer, data);
                    }
                }
                Console.WriteLine($"xml сохранен: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ашибка сохранения xml {filePath}: {ex.Message}");
            }
        }
    }
}
