using Mirror;

namespace MirrorExtension
{
    public class SyncListShort : SyncList<short>
    {
        protected override void SerializeItem(NetworkWriter writer, short item)
        {
            writer.Write(item);
        }

        protected override short DeserializeItem(NetworkReader reader)
        {
            return reader.ReadInt16();
        }
    }
}