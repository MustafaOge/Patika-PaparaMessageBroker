using MassTransit;
using PaparaMessageBroker.Entity;

namespace PaparaMessageBroker.Messaging
{
    public class OrderMessageConsumer : IConsumer<Order>
    {
        public async Task Consume(ConsumeContext<Order> context)
        {
            Console.WriteLine($"Dinlenen order mesajı: {context.Message.Id}, {context.Message.Name}, {context.Message.Description}");
        }
    }
}
