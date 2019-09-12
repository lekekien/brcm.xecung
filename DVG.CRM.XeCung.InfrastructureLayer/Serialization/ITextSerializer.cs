using System.IO;

namespace DVG.CRM.XeCung.InfrastructureLayer.Serialization
{
    public interface ITextSerializer
    {
        void Serialize<T>(TextWriter writer, T objectGraph);

        T Deserialize<T>(TextReader reader);
    }
}