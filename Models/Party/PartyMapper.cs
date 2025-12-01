namespace MvcWithApi.Models.Party
{
    public class PartyMapper
    {
        public static PartyDto ToPartyDto(Party party)
        {
            if (party == null) return null;

            return new PartyDto 
            {
                partyId = party.partyId,
                code = party.code, 
                name = party.name,
                address = party.address
            };
        }

        public static Party ToParty(PartyDto dto)
        {
            if (dto == null) return null;

            return new Party 
            {
                partyId = dto.partyId,
                code = dto.code,
                name = dto.name,
                address = dto.address
            };
        }
    }
}
