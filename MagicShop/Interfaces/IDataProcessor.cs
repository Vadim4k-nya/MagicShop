using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MagicShop.Artifacts;

namespace MagicShop.Interfaces
{
    public interface IDataProcessor<T> where T : Artifact
    {
        List<T> LoadData(string filePath);
        void SaveData(List<T> data, string filePath);
    }
}
