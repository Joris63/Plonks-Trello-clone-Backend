using MassTransit;
using Plonks.Cards.Services;
using Plonks.Shared.Entities;

namespace Plonks.Cards.Helpers
{
    public class ListConsumer : IConsumer<SharedBoardList>
    {
        private readonly IListService _service;

        public ListConsumer(IListService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<SharedBoardList> context)
        {
            try
            {
                await _service.SaveList(context.Message);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
        }
    }
}
