using MagicShop.Artifacts;
using MagicShop.Interfaces;
using MagicShop.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop
{
    public class ShopManager
    {
        private List<Artifact> allArtifacts;
        private readonly IDataProcessor<AntiqueArtifact> antiqueProcessor;
        private readonly IDataProcessor<ModernArtifact> modernProcessor;
        private readonly IDataProcessor<LegendaryArtifact> legendaryProcessor;

        public ShopManager()
        {
            allArtifacts = new List<Artifact>();
            antiqueProcessor = new XmlProcessor<AntiqueArtifact>();
            modernProcessor = new JsonProcessor<ModernArtifact>();
            legendaryProcessor = new LegendaryProcessor();
        }

        private void LoadData<T>(IDataProcessor<T> processor, string filePath) where T : Artifact
        {
            try
            {
                Console.WriteLine($"Загрузка {typeof(T).Name} данных из {filePath}...");
                List<T> loadedData = processor.LoadData(filePath);
                if (loadedData != null && loadedData.Any())
                {
                    allArtifacts.AddRange(loadedData);
                    Console.WriteLine($"Успешная загрузка {loadedData.Count} {typeof(T).Name}.");
                }
                else
                {
                    Console.WriteLine($"Данные {typeof(T).Name} не загружены или файл пуст/недействителен.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла непредвиденная ошибка при загрузке данных {typeof(T).Name} из {filePath}: {ex.Message}");
            }
        }

        public void LoadAllData(string antiqueXmlPath, string modernJsonPath, string legendaryTxtPath)
        {
            Console.WriteLine("--- Начало загрузки данных ---");
            allArtifacts.Clear();


            LoadData(antiqueProcessor, antiqueXmlPath);
            LoadData(modernProcessor, modernJsonPath);
            LoadData(legendaryProcessor, legendaryTxtPath);


            Console.WriteLine($"--- Загрузка данных завершена --- Всего загружено артефактов: {allArtifacts.Count} ---");
        }


        // --- Методы LINQ ---
        public List<Artifact> GetAllArtifacts()
        {
            return allArtifacts.ToList();
        }

        public List<LegendaryArtifact> FindCursedArtifacts(int minPowerLevel = 50)
        {
            return allArtifacts
                .OfType<LegendaryArtifact>()
                .Where(a => a.IsCursed && a.PowerLevel > minPowerLevel)
                .ToList();
        }

        public Dictionary<Rarity, int> GroupByRarity()
        {
            return allArtifacts
                .GroupBy(a => a.Rarity)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public List<Artifact> TopByPower(int count)
        {
            if (count <= 0) return new List<Artifact>();


            return allArtifacts
                .OrderByDescending(a => a.PowerLevel)
                .Take(count)
                .ToList();
        }


        // --- Reporting ---

        public void GenerateReport(string reportFileName = "shop_report.txt")
        {
            string filePath = Path.Combine(reportFileName);
            Console.WriteLine($"Формирование отчета в {filePath}...");


            try
            {
                var report = new StringBuilder();
                report.AppendLine("========== Отчет ==========");
                report.AppendLine($"Всего артефактов: {allArtifacts.Count}");
                report.AppendLine();


                report.AppendLine("--- Количество артефактов по редкости ---");
                var rarityCounts = GroupByRarity();
                foreach (var kvp in rarityCounts)
                {
                    report.AppendLine($"{kvp.Key}: {kvp.Value}");
                }
                report.AppendLine();


                report.AppendLine("--- Средний уровень мощности по редкости ---");
                var avgPowerByRarity = allArtifacts
                   .GroupBy(a => a.Rarity)
                   .OrderBy(g => g.Key)
                   .Select(g => new { Rarity = g.Key, AvgPower = g.Average(a => a.PowerLevel) });


                foreach (var item in avgPowerByRarity)
                {
                    report.AppendLine($"{item.Rarity}: {item.AvgPower:F2}");
                }
                report.AppendLine();


                report.AppendLine("--- Проклятые артефакты (Павер > 50) ---");
                var cursed = FindCursedArtifacts();
                if (cursed.Any())
                {
                    foreach (var artifact in cursed)
                    {
                        report.AppendLine($"- ID: {artifact.Id}, Name: {artifact.Name}, Power: {artifact.PowerLevel}");
                    }
                }
                else
                {
                    report.AppendLine("Проклятых артефактов с уровнем мощности > 50 не обнаружено..");
                }
                report.AppendLine();


                report.AppendLine("================ Конец отчета ================");


                File.WriteAllText(filePath, report.ToString());
                Console.WriteLine("Отчет успешно создан.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка создания отчета: {ex.Message}");
            }
        }


        // --- Экспорт результатов ---

        public void ExportListToJson<T>(List<T> data, string fileName) where T : Artifact
        {
            if (data == null || !data.Any())
            {
                Console.WriteLine("Нет данных для экспорта JSON.");
                return;
            }
            string filePath = Path.Combine(fileName);
            var exporter = new JsonProcessor<T>();
            exporter.SaveData(data, filePath);
        }

        public void ExportListToXml<T>(List<T> data, string fileName) where T : Artifact
        {
            if (data == null || !data.Any())
            {
                Console.WriteLine("Нет данных для экспорта XML.");
                return;
            }
            string filePath = Path.Combine(fileName);
            var exporter = new XmlProcessor<T>();
            exporter.SaveData(data, filePath);
        }
    }
}
