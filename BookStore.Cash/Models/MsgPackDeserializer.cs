using Confluent.Kafka;
using MessagePack;

namespace DB2Kafka.Producer
{
    public class MsgPackDeserializer<TValue> : IDeserializer<TValue>
    {
        public TValue Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return MessagePackSerializer.Deserialize<TValue>(data.ToArray());
        }
    }
}