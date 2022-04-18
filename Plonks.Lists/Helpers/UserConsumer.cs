using MassTransit;
using Plonks.Lists.Services;
using Plonks.Shared.Entities;

namespace Plonks.Lists.Helpers
{
    public class UserConsumer : IConsumer<QueueMessage<SharedUser>>
    {
        private readonly IUserService _service;

        public UserConsumer(IUserService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<QueueMessage<SharedUser>> context)
        {
            try
            {
                switch (context.Message.Type)
                {
                    case QueueMessageType.Insert:
                        await _service.RegisterUser(context.Message.Data);
                        break;

                    case QueueMessageType.Update:
                        await _service.UpdateUser(context.Message.Data);
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
