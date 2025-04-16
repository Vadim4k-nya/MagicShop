using MagicShop.Artifacts;
using MagicShop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop.Processors
{
    public class LegendaryProcessor : IDataProcessor<LegendaryArtifact>
    {
        public List<LegendaryArtifact> LoadData(string filePath)
        {
            var artifacts = new List<LegendaryArtifact>();
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"ашибка: файл не найден {filePath}");
                return artifacts;
            }

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                int lineNumber = 0;
                foreach (string line in lines)
                {
                    lineNumber++;
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split('|');
                    if (parts.Length != 5)
                    {
                        Console.WriteLine($"Внимание: инвалид формат в строке {lineNumber} в {filePath}. Ожидалось 5 частей, найдено {parts.Length}. Пропуск строки.");
                        continue;
                    }

                    bool idParsed = int.TryParse(parts[0].Trim(), out int id);
                    string name = parts[1].Trim();
                    bool powerParsed = int.TryParse(parts[2].Trim(), out int powerLevel);
                    bool rarityParsed = Enum.TryParse<Rarity>(parts[3].Trim(), true, out Rarity rarity);
                    string curseDesc = parts[4].Trim();
                    bool isCursedParsed = bool.TryParse(parts[5].Trim(), out bool isCursed);


                    if (!idParsed || !powerParsed || !rarityParsed || !isCursedParsed)
                    {
                        Console.WriteLine($"Внимание: неверный формат в строке {lineNumber} в {filePath}. Не удалось спарсить ID, PowerLevel, Rarity, или IsCursed status. Пропуск строки.");
                        continue;
                    }

                    string nameFromFile = parts[0].Trim();
                    bool powerParsedFile = int.TryParse(parts[1].Trim(), out int powerLevelFile);
                    bool rarityParsedFile = Enum.TryParse<Rarity>(parts[2].Trim(), true, out Rarity rarityFile);
                    string curseDescFile = parts[3].Trim();
                    bool isCursedParsedFile = bool.TryParse(parts[4].Trim(), out bool isCursedFile);


                    if (string.IsNullOrEmpty(nameFromFile) || !powerParsedFile || !rarityParsedFile || !isCursedParsedFile)
                    {
                        Console.WriteLine($"Внимание: неверные данные в строке {lineNumber} в {filePath}. пропуск.");
                        continue;
                    }


                    artifacts.Add(new LegendaryArtifact
                    {
                        Id = artifacts.Count + 1000, 
                        Name = nameFromFile,
                        PowerLevel = powerLevelFile,
                        Rarity = rarityFile,
                        CurseDescription = curseDescFile,
                        IsCursed = isCursedFile
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ашибка загрузки текста из {filePath}: {ex.Message}");
            }
            return artifacts;
        }


        public void SaveData(List<LegendaryArtifact> data, string filePath)
        {
            try
            {
                var lines = data.Select(a =>
                    $"{a.Name}|{a.PowerLevel}|{a.Rarity}|{a.CurseDescription}|{a.IsCursed}"
                );
                File.WriteAllLines(filePath, lines);
                Console.WriteLine($"Данные сохранены: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ашибка сохранения {filePath}: {ex.Message}");
            }
        }
    }
}
