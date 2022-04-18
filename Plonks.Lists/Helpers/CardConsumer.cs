using MassTransit;
using Plonks.Lists.Services;
using Plonks.Shared.Entities;

namespace Plonks.Lists.Helpers
{
    public class CardConsumer : IConsumer<QueueMessage<SharedCard>>
    {
        private readonly ICardService _service;

        public CardConsumer(ICardService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<QueueMessage<SharedCard>> context)
        {
            try
            {
                switch (context.Message.Type)
                {
                    case QueueMessageType.Insert:
                        await _service.CreateCard(context.Message.Data);
                        break;

                    case QueueMessageType.Update:
                        await _service.UpdateCard(context.Message.Data);
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
