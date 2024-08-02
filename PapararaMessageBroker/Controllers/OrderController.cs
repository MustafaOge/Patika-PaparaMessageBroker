using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaparaMessageBroker.Entity;
using PaparaMessageBroker.Messaging;

namespace PapararaMessageBroker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController(AppDbContext context, OrderMessagePublisher messagePublisher) : ControllerBase
    {
    

        // GET: /product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await context.Orders.ToListAsync();
            return Ok(products);
        }

        // GET: /product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await context.Orders.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: /product
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            context.Orders.Add(order);
            await context.SaveChangesAsync();

           await messagePublisher.SendOrderMessage(order);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Order updatedOrder)
        {
            if (id != updatedOrder.Id) return BadRequest();

            var existingOrder = await context.Orders.FindAsync(id);
            if (existingOrder == null) return NotFound();

            context.Entry(existingOrder).CurrentValues.SetValues(updatedOrder);
            await context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: /product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await context.Orders.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            context.Orders.Remove(product);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
