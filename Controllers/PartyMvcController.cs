using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MvcWithApi.Data;
using MvcWithApi.Models.Party;

namespace MvcWithApi.Controllers
{
    public class PartyMvcController : Controller
    {
        private readonly PartyRepository _repo;
        public PartyMvcController(PartyRepository repo)
        {
            _repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            var parties = await _repo.GetPartiesAsync();
            if (parties == null) parties = new List<Party>(); // prevent null
            return View(parties);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Party party)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.InsertPartyAsync(party);
                    TempData["SuccessMessage"] = "Party Inserted successfully!";
                    return RedirectToAction("Index");
                }
                catch (SqlException ex) when (ex.Number == 2627) // Unique constraint violation
                {
                    TempData["ErrorMessage"] = "Duplicate Party Code. Please choose another.";
                    return View("Create"); 
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "An unexpected database error occurred.";
                    return View("Create");
                }
            }

            return View(party);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var party = await _repo.GetPartyByIdAsync(id);
            if (party == null) return NotFound();
            return View(party);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Party party)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.UpdatePartyAsync(party);
                    TempData["SuccessMessage"] = "Party updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) // 2627 = Unique constraint violation
                    {
                        // ✅ Bind SQL exception into TempData
                        TempData["ErrorMessage"] = "Duplicate Party Code. Please choose another.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "An unexpected database error occurred.";
                    }

                    // Redirect back to Edit page with TempData message
                    return RedirectToAction("Edit", new { id = party.partyId });
                }
            }

            return View(party);
        }


        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeletePartyAsync(id);
            TempData["SuccessMessage"] = "Party Deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
