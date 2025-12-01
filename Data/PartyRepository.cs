using MvcWithApi.Models.Party;
using System.Data;

namespace MvcWithApi.Data
{
    public class PartyRepository
    {
        private readonly DbHelper _db;
        public PartyRepository(DbHelper db)
        {
            _db = db;
        }

        //Get all parties
        public async Task<List<Party>> GetPartiesAsync()
        {
            using var conn = await _db.GetConnectionAsync();
            using var cmd = _db.CreateCommand(conn, "spParty_GetAll");

            var table = await _db.ExecuteDataTableAsync(cmd);
            var parties = new List<Party>();
            foreach (DataRow row in table.Rows)
            {
                parties.Add(new Party
                {
                    partyId = Convert.ToInt32(row["partyId"]),
                    code = row["code"].ToString() ?? string.Empty,
                    name = row["name"].ToString() ?? string.Empty,
                    address = row["address"].ToString() ?? string.Empty
                });
            }
            return parties;
        }

        // Get Party By ID
        public async Task<Party?> GetPartyByIdAsync(int id)
        {
            using var conn = await _db.GetConnectionAsync();
            using var cmd = _db.CreateCommand(conn, "spParty_GetById");

            _db.AddParameter(cmd, "@partyId", SqlDbType.Int, id);

            var table = await _db.ExecuteDataTableAsync(cmd);
            if (table.Rows.Count == 0) return null;

            var row = table.Rows[0];
            return new Party
            {
                partyId = Convert.ToInt32(row["partyid"]),
                code = row["code"].ToString() ?? string.Empty,
                name = row["name"].ToString() ?? string.Empty,
                address = row["address"].ToString() ?? string.Empty
            };
        }

        // Insert Party
        public async Task<int> InsertPartyAsync(Party party)
        {
            using var conn = await _db.GetConnectionAsync();
            using var cmd = _db.CreateCommand(conn, "spParty_Insert");

            _db.AddParameter(cmd, "@code", SqlDbType.NVarChar, party.code, 15);
            _db.AddParameter(cmd, "@name", SqlDbType.NVarChar, party.name, 100);
            _db.AddParameter(cmd, "@address", SqlDbType.NVarChar, party.address, 200);

            return await _db.ExecuteNonQueryAsync(cmd);
        }

        //Update Party
        public async Task<int> UpdatePartyAsync(Party party)
        {
            using var conn = await _db.GetConnectionAsync();
            using var cmd = _db.CreateCommand(conn, "spParty_Update");

            _db.AddParameter(cmd, "@partyId", SqlDbType.Int, party.partyId);
            _db.AddParameter(cmd, "@code", SqlDbType.NVarChar, party.code, 15);
            _db.AddParameter(cmd, "@name", SqlDbType.NVarChar, party.name, 100);
            _db.AddParameter(cmd, "@address", SqlDbType.NVarChar, party.address, 200);

            return await _db.ExecuteNonQueryAsync(cmd);
        }

        //Delete Party
        public async Task<int> DeletePartyAsync(int id)
        {
            using var conn = await _db.GetConnectionAsync();
            using var cmd = _db.CreateCommand(conn, "spParty_Delete");

            _db.AddParameter(cmd, "@partyId", SqlDbType.Int, id);

            return await _db.ExecuteNonQueryAsync(cmd);
        }
    }
}
