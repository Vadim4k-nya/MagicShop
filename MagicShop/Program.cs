namespace MagicShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Magical Artifact Shop Management System!");

            string dataDir = "Data";
            Directory.CreateDirectory(dataDir);
            string antiqueFile = Path.Combine(dataDir, "antique.xml");
            string modernFile = Path.Combine(dataDir, "modern.json");
            string legendaryFile = Path.Combine(dataDir, "legends.txt");

            CreateSampleFiles(antiqueFile, modernFile, legendaryFile);

            ShopManager manager = new ShopManager();
            manager.LoadAllData(antiqueFile, modernFile, legendaryFile);

            bool exit = false;
            while (!exit)
            {
                PrintMenu();
                string choice = Console.ReadLine()?.Trim() ?? "";

                switch (choice)
                {
                    case "1":
                        DisplayAllArtifacts(manager);
                        break;
                    case "2":
                        FindAndDisplayCursedArtifacts(manager);
                        break;
                    case "3":
                        DisplayArtifactsByRarity(manager);
                        break;
                    case "4":
                        FindAndDisplayTopPowerArtifacts(manager);
                        break;
                    case "5":
                        manager.GenerateReport();
                        break;
                    case "6":
                        ExportCursedArtifacts(manager);
                        break;
                    case "7":
                        ExportTop5Artifacts(manager);
                        break;
                    case "0":
                        exit = true;
                        Console.WriteLine("Выход из системы. До свидания!");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
                if (!exit)
                {
                    Console.WriteLine("\nНажмите Enter, чтобы продолжить...");
                    Console.ReadLine();
                }
            }
        }


        static void PrintMenu()
        {
            Console.Clear(); // Clear console for better readability
            Console.WriteLine("\n--- Меню магазина магических артефактов ---");
            Console.WriteLine("1. Список всех артефактов");
            Console.WriteLine("2. Найдите проклятые артефакты (Power > 50)");
            Console.WriteLine("3. Показать артефакты по редкости");
            Console.WriteLine("4. Найти Топ N Артефактов по мощности");
            Console.WriteLine("5. Создать полный отчет (shop_report.txt)");
            Console.WriteLine("6. Экспорт проклятых артефактов (JSON/XML)");
            Console.WriteLine("7. Экспорт 5 лучших артефактов (JSON/XML)");
            Console.WriteLine("0. Выход");
            Console.Write("Введите ваш выбор: ");
        }


        static void DisplayAllArtifacts(ShopManager manager)
        {
            Console.WriteLine("\n--- Все артефакты ---");
            var artifacts = manager.GetAllArtifacts();
            if (!artifacts.Any())
            {
                Console.WriteLine("Артефакты не загружены.");
                return;
            }
            foreach (var artifact in artifacts)
            {
                Console.WriteLine(artifact.ToString());
            }
        }


        static void FindAndDisplayCursedArtifacts(ShopManager manager)
        {
            Console.WriteLine("\n--- Проклятые артефакты (Power > 50) ---");
            var cursedArtifacts = manager.FindCursedArtifacts(); // Uses default Power > 50
            if (!cursedArtifacts.Any())
            {
                Console.WriteLine("Проклятых артефактов, соответствующих критериям, не найдено.");
                return;
            }
            foreach (var artifact in cursedArtifacts)
            {
                Console.WriteLine(artifact.ToString());
            }
            Console.WriteLine($"\nНайдено {cursedArtifacts.Count} проклятые артефакты.");
        }


        static void DisplayArtifactsByRarity(ShopManager manager)
        {
            Console.WriteLine("\n--- Количество артефактов по редкости ---");
            var rarityGroups = manager.GroupByRarity();
            if (!rarityGroups.Any())
            {
                Console.WriteLine("Нет артефактов для группировки.");
                return;
            }
            foreach (var kvp in rarityGroups)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }


        static void FindAndDisplayTopPowerArtifacts(ShopManager manager)
        {
            Console.Write("Введите количество лучших артефактов для поиска (N): ");
            if (int.TryParse(Console.ReadLine(), out int count) && count > 0)
            {
                Console.WriteLine($"\n--- Топ {count} артефактов по силе ---");
                var topArtifacts = manager.TopByPower(count);
                if (!topArtifacts.Any())
                {
                    Console.WriteLine("Артефактов не обнаружено..");
                    return;
                }
                foreach (var artifact in topArtifacts)
                {
                    Console.WriteLine(artifact.ToString());
                }
            }
            else
            {
                Console.WriteLine("Введен неверный номер.");
            }
        }


        static void ExportCursedArtifacts(ShopManager manager)
        {
            var cursed = manager.FindCursedArtifacts();
            if (!cursed.Any())
            {
                Console.WriteLine("Нет проклятых артефактов для экспорта.");
                return;
            }


            Console.Write("Экспортировать проклятые артефакты в (1) JSON или (2) XML? Введите выбор: ");
            string choice = Console.ReadLine()?.Trim();


            if (choice == "1")
            {
                manager.ExportListToJson(cursed, "cursed_artifacts_export.json");
            }
            else if (choice == "2")
            {
                manager.ExportListToXml(cursed, "cursed_artifacts_export.xml");
            }
            else
            {
                Console.WriteLine("Неверный выбор формата экспорта.");
            }
        }


        static void ExportTop5Artifacts(ShopManager manager)
        {
            var top5 = manager.TopByPower(5);
            if (!top5.Any())
            {
                Console.WriteLine("Артефактов для экспорта не найдено.");
                return;
            }


            Console.Write("Экспортировать 5 лучших артефактов в (1) JSON или (2) XML? Введите выбор: ");
            string choice = Console.ReadLine()?.Trim();


            if (choice == "1")
            {
                manager.ExportListToJson(top5, "top5_artifacts_export.json");
            }
            else if (choice == "2")
            {
                manager.ExportListToXml(top5, "top5_artifacts_export.xml");
            }
            else
            {
                Console.WriteLine("Invalid export format choice.");
            }
        }

        static void CreateSampleFiles(string antiquePath, string modernPath, string legendaryPath)
        {
            string antiqueXmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                            <ArrayOfAntiqueArtifact>
                                              <AntiqueArtifact Id=""101"">
                                                <Name>Amulet of Yendor</Name>
                                                <PowerLevel>95</PowerLevel>
                                                <Rarity>Legendary</Rarity>
                                                <Age>1200</Age>
                                                <OriginRealm>Arcadia</OriginRealm>
                                              </AntiqueArtifact>
                                              <AntiqueArtifact Id=""102"">
                                                <Name>Goblet of Fire</Name>
                                                <PowerLevel>70</PowerLevel>
                                                <Rarity>Epic</Rarity>
                                                <Age>800</Age>
                                                <OriginRealm>Hogwarts Realm</OriginRealm>
                                              </AntiqueArtifact>
                                            </ArrayOfAntiqueArtifact>";

            string modernJsonContent = @"[
                                          {
                                            ""Id"": 201,
                                            ""Name"": ""Hyper Phase Blaster"",
                                            ""PowerLevel"": 88,
                                            ""Rarity"": ""Epic"",
                                            ""TechLevel"": 9.5,
                                            ""Manufacturer"": ""TechMage Inc.""
                                          },
                                          {
                                            ""Id"": 202,
                                            ""Name"": ""Stealth Cloak v3"",
                                            ""PowerLevel"": 45,
                                            ""Rarity"": ""Rare"",
                                            ""TechLevel"": 7.2,
                                            ""Manufacturer"": ""ShadowWorks""
                                          }
                                        ]";

            string legendaryTextContent = @"Sword of Destiny|100|Legendary|Drains life from the wielder|true
                                            Shield of Ages|85|Legendary|Attracts unwanted attention|true
                                            Staff of Minor Annoyance|15|Common|Slightly itchy handle|false
                                            Orb of Confusion|60|Rare|User occasionally forgets their name|true";


            if (!File.Exists(antiquePath))
            {
                File.WriteAllText(antiquePath, antiqueXmlContent);
                Console.WriteLine($"Созданный файл-образец: {antiquePath}");
            }
            if (!File.Exists(modernPath))
            {
                File.WriteAllText(modernPath, modernJsonContent);
                Console.WriteLine($"Созданный файл-образец: {modernPath}");
            }
            if (!File.Exists(legendaryPath))
            {
                File.WriteAllText(legendaryPath, legendaryTextContent);
                Console.WriteLine($"Созданный файл-образец: {legendaryPath}");
            }
        }
    }
}
