using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Elo_Tracker.Utilities
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

        public static void SaveXML(IEnumerable<T> things, string filePath)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            FileStream outFile = File.Create(filePath);
            XmlSerializer formatter = new XmlSerializer(typeof(List<T>));
            formatter.Serialize(outFile, things);
        }

        public static List<T> LoadXML(string filePath)
        {
            List<T> things = new List<T>();
            XmlSerializer formatter = new XmlSerializer(typeof(List<T>));
            FileStream inFile = new FileStream(filePath, FileMode.Open);
            byte[] buffer = new byte[inFile.Length];
            inFile.Read(buffer, 0, (int)inFile.Length);
            MemoryStream stream = new MemoryStream(buffer);
            return (List<T>)formatter.Deserialize(stream);
        }


    }
}
