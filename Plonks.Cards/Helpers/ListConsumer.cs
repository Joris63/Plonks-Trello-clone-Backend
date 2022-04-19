using MassTransit;
using Plonks.Cards.Services;
using Plonks.Shared.Entities;

namespace Plonks.Cards.Helpers
{
    public class ListConsumer : IConsumer<QueueMessage<SharedBoardList>>
    {
        private readonly IListService _service;

        public ListConsumer(IListService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<QueueMessage<SharedBoardList>> context)
        {
            try
            {
                switch (context.Message.Type)
                {
                    case QueueMessageType.Insert:
                        await _service.CreateList(context.Message.Data);
                        break;

                    case QueueMessageType.Update:
                        await _service.UpdateList(context.Message.Data);
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
