using System;
using System.IO;
using System.Runtime.Serialization;

namespace SharkSpirit.Editor.Core.Utilities.Serialization
{
    public static class Serializer
    {
        public static void Serialize<T>(T obj, string path)
        {
            try
            {
                using var fileStream = new FileStream(path, FileMode.Create);
                var dataContractSerializer = new DataContractSerializer(typeof(T));

                dataContractSerializer.WriteObject(fileStream, obj);
            }
            catch (Exception e)
            {
                //todo log
                throw;
            }
        }

        public static T Deserialize<T>(string path)
        {
            try
            {
                using var fileStream = new FileStream(path, FileMode.Open);
                var dataContractSerializer = new DataContractSerializer(typeof(T));

                var obj = (T)dataContractSerializer.ReadObject(fileStream);

                return obj;
            }
            catch (Exception e)
            {
                //todo log
                throw;
            }
        }
    }
}
