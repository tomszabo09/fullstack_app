using BV3N92_HFT_2021221.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BV3N92_HFT_2021221.Repository
{
    public class PartyRepository : Repository<Party>, IPartyRepository
    {
        public PartyRepository(DbContext ctx) : base(ctx)
        {

        }

        public void CreateParty(int partyId, int parliamentId, string partyName, string ideology)
        {
            var _new = new Party() { PartyID = partyId, ParliamentID = parliamentId, PartyName = partyName, Ideology = ideology };
            ctx.Add(_new);
            ctx.SaveChanges();
        }
        public void ChangeIdeology(int partyId, string newIdeology)
        {
            var party = GetOne(partyId);
            party.Ideology = newIdeology;
            ctx.SaveChanges();
        }
        public void ChangePartyName(int partyId, string newName)
        {
            var party = GetOne(partyId);
            party.PartyName = newName;
            ctx.SaveChanges();
        }
        public void DeleteParty(int partyId)
        {
            var todel = GetOne(partyId);
            ctx.Remove(todel);
            ctx.SaveChanges();
        }

        public override Party GetOne(int id)
        {
            return GetAll().SingleOrDefault(x => x.PartyID.Equals(id));
        }

        public void AddNewParty(Party party)
        {
            ctx.Add(party);
            ctx.SaveChanges();
        }

        public void UpdateParty(Party party)
        {
            var toUpdate = GetOne(party.PartyID);

            toUpdate.ParliamentID = party.ParliamentID;
            toUpdate.PartyName = party.PartyName;
            toUpdate.Ideology = party.Ideology;

            ctx.SaveChanges();
        }
    }
}
