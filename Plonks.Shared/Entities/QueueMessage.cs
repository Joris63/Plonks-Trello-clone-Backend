using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
