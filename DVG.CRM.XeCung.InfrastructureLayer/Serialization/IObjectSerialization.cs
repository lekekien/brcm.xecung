namespace DVG.CRM.XeCung.InfrastructureLayer.Serialization
{
    public interface IObjectSerialization<T> where T : class
    {
        byte[] Serialize(T objectGraph);

        T DeSerialize(byte[] data);
    }
}