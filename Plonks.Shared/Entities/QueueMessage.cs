namespace Plonks.Shared.Entities
{
    public enum QueueMessageType
    {
        Insert,
        Update,
        Delete,
    }

    public class QueueMessage<T>
    {
        public T? Data { get; set; }

        public QueueMessageType Type { get; set; }
    }
}
