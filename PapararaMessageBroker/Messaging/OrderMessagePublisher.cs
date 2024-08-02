using MassTransit;
using PaparaMessageBroker.Entity;
using PapararaMessageBroker;

namespace PaparaMessageBroker.Messaging
{
    public class OrderMessagePublisher(ISendEndpointProvider sendEndpointProvider, AppDbContext context)
    {

        public async Task SendOrderMessage(Order order)
        {

            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:order-created"));

            await endpoint.Send(order, pipe =>
            {
                pipe.Durable = true;
                pipe.SetAwaitAck(true);
                pipe.CorrelationId = Guid.NewGuid();
            }, tokenSource.Token);



            await context.SaveChangesAsync();
        }
    }
}
