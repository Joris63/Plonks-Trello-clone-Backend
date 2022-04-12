using MassTransit;
using Plonks.Lists.Services;
using Plonks.Shared.Entities;

namespace Plonks.Lists.Helpers
{
    public class CardConsumer : IConsumer<SharedCard>
    {
        private readonly ICardService _service;

        public CardConsumer(ICardService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<SharedCard> context)
        {
            try
            {
                await _service.SaveCard(context.Message);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
        }
    }
}
