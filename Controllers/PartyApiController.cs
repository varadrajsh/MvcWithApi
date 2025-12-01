using Microsoft.AspNetCore.Mvc;
using MvcWithApi.Data;
using MvcWithApi.Models.Party;

namespace MvcWithApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartyController : ControllerBase
    {
        private readonly PartyRepository _repo;

        public PartyController(PartyRepository repo)
        {
            _repo = repo;
        }

        // GET: api/party
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Party>>> GetAll()
        {
            var parties = await _repo.GetPartiesAsync();
            return Ok(parties);
        }

        // GET: api/party/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Party>> GetById(int id)
        {
            var party = await _repo.GetPartyByIdAsync(id);
            if (party == null) return NotFound();
            return Ok(party);
        }

        // POST: api/party
        [HttpPost]
        public async Task<ActionResult<Party>> Create([FromBody] Party party)
        {
            if (party == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var rows = await _repo.InsertPartyAsync(party);
            if (rows == 0) return BadRequest("Insert failed");

            // ✅ Return 201 Created with Location header
            return CreatedAtAction(nameof(GetById), new { id = party.partyId }, party);
        }

        // PUT: api/party/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Party party)
        {
            if (party == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _repo.GetPartyByIdAsync(id);
            if (existing == null) return NotFound();

            party.partyId = id;
            var rows = await _repo.UpdatePartyAsync(party);
            if (rows == 0) return BadRequest("Update failed");

            return NoContent(); // ✅ 204 No Content
        }

        // DELETE: api/party/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.GetPartyByIdAsync(id);
            if (existing == null) return NotFound();

            var rows = await _repo.DeletePartyAsync(id);
            if (rows == 0) return BadRequest("Delete failed");

            return NoContent(); // ✅ 204 No Content
        }
    }
}
