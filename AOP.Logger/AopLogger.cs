using System;
using System.IO;
using System.Xml.Serialization;

namespace AOP.Logger
{
    [Serializable]
    public class AopLogger : IAopLogger
    {
        public void LogMethod(MethodLogModel model)
        {
            var xmlFormatter = new XmlSerializer(typeof(MethodLogModel));

            using (FileStream stream = new FileStream($"{model.Name}_{model.DateTime}.xml", FileMode.OpenOrCreate))
            {
                xmlFormatter.Serialize(stream, model);
            }
        }
    }
}
