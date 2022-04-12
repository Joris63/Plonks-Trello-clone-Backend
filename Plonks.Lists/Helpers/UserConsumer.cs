using MassTransit;
using Plonks.Lists;
using Plonks.Shared.Entities;

namespace Plonks.Boards.Helpers
{
    public class UserConsumer : IConsumer<SharedUser>
    {
        private readonly IUserService _service;

        public UserConsumer(IUserService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<SharedUser> context)
        {
            try
            {
                await _service.SaveUser(context.Message);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
        }
    }
}
