using System;
using System.IO;

namespace DVG.CRM.XeCung.InfrastructureLayer.Serialization
{
    public class TextSerializerForKeyCached : ITextSerializer
    {
        public T Deserialize<T>(TextReader reader)
        {
            throw new NotImplementedException();
        }

        public void Serialize<T>(TextWriter writer, T objectGraph)
        {
            throw new NotImplementedException();
        }
    }
}