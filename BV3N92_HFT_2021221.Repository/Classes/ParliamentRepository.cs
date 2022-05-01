using BV3N92_HFT_2021221.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BV3N92_HFT_2021221.Repository
{
    public class ParliamentRepository : Repository<Parliament>, IParliamentRepository
    {
        public ParliamentRepository(DbContext ctx) : base(ctx)
        {

        }

        public void CreateParliament(int parliamentId, string name, string rulingParty)
        {
            var _new = new Parliament() { ParliamentID = parliamentId, ParliamentName = name, RulingParty = rulingParty };
            ctx.Add(_new);
            ctx.SaveChanges();
        }
        public void ChangeName(int parliamentId, string newName)
        {
            var parliament = GetOne(parliamentId);
            parliament.ParliamentName = newName;
            ctx.SaveChanges();
        }
        public void ReplaceRulingParty(int parliamentId, string newParty)
        {
            var parliament = GetOne(parliamentId);
            parliament.RulingParty = newParty;
            ctx.SaveChanges();
        }
        public void DeleteParliament(int parliamentId)
        {
            var todel = GetOne(parliamentId);
            ctx.Remove(todel);
            ctx.SaveChanges();
        }

        public override Parliament GetOne(int id)
        {
            return GetAll().SingleOrDefault(x => x.ParliamentID.Equals(id));
        }

        public void AddNewParliament(Parliament parliament)
        {
            ctx.Add(parliament);
            ctx.SaveChanges();
        }

        public void UpdateParliament(Parliament parliament)
        {
            var toUpdate = GetOne(parliament.ParliamentID);

            toUpdate.ParliamentName = parliament.ParliamentName;
            toUpdate.RulingParty = parliament.RulingParty;

            ctx.SaveChanges();
        }
    }
}
