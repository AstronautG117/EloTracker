using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EloTracker.Utilities
{
    public static class Serializer<T>
    {
        public static void Save(IEnumerable<T> things, string filePath)
        {
            List<T> listThings = new List<T>(things);
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            FileStream outFile = new FileStream(filePath, FileMode.Create);
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(outFile, listThings);
            outFile.Close();
        }

        public static List<T> Load(string filePath)
        {
            FileStream inFile = new FileStream(filePath, FileMode.Open);
            IFormatter formatter = new BinaryFormatter();
            List<T> things = (List<T>)formatter.Deserialize(inFile);
            return things;
        }
    }
}
