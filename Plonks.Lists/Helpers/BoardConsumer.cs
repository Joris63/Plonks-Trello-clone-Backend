using MassTransit;
using Plonks.Lists.Services;
using Plonks.Shared.Entities;

namespace Plonks.Lists.Helpers
{
    public class BoardConsumer : IConsumer<SharedBoard>
    {
        private readonly IBoardService _service;

        public BoardConsumer(IBoardService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<SharedBoard> context)
        {
            try
            {
                await _service.SaveBoard(context.Message);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
        }
    }
}
