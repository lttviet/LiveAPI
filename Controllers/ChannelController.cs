using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LiveAPI.Models;

namespace LiveAPI.Controllers
{
    [Route("api/channels")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly ChannelContext _context;

        public ChannelController(ChannelContext context)
        {
            _context = context;
        }

        // GET: api/Channel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChannelDTO>>> GetChannels()
        {
            return await _context.Channels
                .Select(ch => ChannelToDTO(ch))
                .ToListAsync();
        }

        // GET: api/Channel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChannelDTO>> GetChannel(long id)
        {
            var channel = await _context.Channels.FindAsync(id);

            if (channel == null)
            {
                return NotFound();
            }

            return ChannelToDTO(channel);
        }

        // PUT: api/Channel/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChannel(long id, ChannelDTO channelDTO)
        {
            if (id != channelDTO.Id)
            {
                return BadRequest();
            }

            var channel = await _context.Channels.FindAsync(id);
            if (channel == null)
            {
                return NotFound();
            }

            channel.Name = channelDTO.Name;
            channel.IsLive = channelDTO.IsLive;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ChannelExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Channel
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ChannelDTO>> PostChannel(ChannelDTO channelDTO)
        {
            var channel = new Channel
            {
                Name = channelDTO.Name,
                IsLive = channelDTO.IsLive
            };

            _context.Channels.Add(channel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetChannel),
                new { id = channel.Id },
                ChannelToDTO(channel)
            );
        }

        // DELETE: api/Channel/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ChannelDTO>> DeleteChannel(long id)
        {
            var channel = await _context.Channels.FindAsync(id);
            if (channel == null)
            {
                return NotFound();
            }

            _context.Channels.Remove(channel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChannelExists(long id)
        {
            return _context.Channels.Any(e => e.Id == id);
        }

        private static ChannelDTO ChannelToDTO(Channel ch) =>
            new ChannelDTO
            {
                Id = ch.Id,
                Name = ch.Name,
                IsLive = ch.IsLive
            };
    }
}
