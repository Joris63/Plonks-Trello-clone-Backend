using MassTransit;
using Plonks.Lists.Services;
using Plonks.Shared.Entities;

namespace Plonks.Lists.Helpers
{
    public class BoardConsumer : IConsumer<QueueMessage<SharedBoard>>
    {
        private readonly IBoardService _service;

        public BoardConsumer(IBoardService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<QueueMessage<SharedBoard>> context)
        {
            try
            {
                switch (context.Message.Type)
                {
                    case QueueMessageType.Insert:
                        await _service.CreateBoard(context.Message.Data);
                        break;

                    case QueueMessageType.Delete:
                        await _service.DeleteBoard(context.Message.Data);
                        break;
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
        }
    }
}
