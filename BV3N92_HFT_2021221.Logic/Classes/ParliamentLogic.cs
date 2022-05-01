using BV3N92_HFT_2021221.Models;
using BV3N92_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BV3N92_HFT_2021221.Logic
{
    public class ParliamentLogic : IParliamentLogic
    {
        IParliamentRepository parliamentRepo;
        IPartyRepository partyRepo;
        IPartyMemberRepository partyMemberRepo;

        public ParliamentLogic(IParliamentRepository repo, IPartyRepository prepo, IPartyMemberRepository pmrepo)
        {
            this.parliamentRepo = repo;
            this.partyRepo = prepo;
            this.partyMemberRepo = pmrepo;
        }

        public void ChangeName(int parliamentId, string newName)
        {
            if (parliamentId < 0)
            {
                throw new Exception("Invalid ID!");
            }
            else if (newName.Equals(string.Empty))
            {
                throw new Exception("Name has to be given!");
            }

            if (GetAllParliaments().Any(x => x.ParliamentName == newName))
            {
                throw new Exception($"A parliament with the name '{newName}' already exists!");
            }
            else if (GetAllParliaments().Any(x => x.ParliamentID == parliamentId))
            {
                parliamentRepo.ChangeName(parliamentId, newName);
            }
            else
                throw new Exception($"No such parliament with ID '{parliamentId}'!");
        }

        public void CreateParliament(int parliamentId, string name, string rulingParty)
        {
            foreach (var item in GetAllParliaments())
            {
                if (item.ParliamentName.Equals(name))
                {
                    throw new Exception($"A parliament with the name '{name}' already exists!");
                }
                else if (item.ParliamentID.Equals(parliamentId))
                {
                    throw new Exception($"A parliament with ID '{parliamentId}' already exists!");
                }
            }

            if (parliamentId < 0)
            {
                throw new Exception("Invalid ID!");
            }
            else if (name.Equals(string.Empty))
            {
                throw new Exception("Name has to be given!");
            }
            else if (rulingParty.Equals(string.Empty))
            {
                throw new Exception("Ruling party has to be given!");
            }
            else
                parliamentRepo.CreateParliament(parliamentId, name, rulingParty);
        }

        public void DeleteParliament(int parliamentId)
        {
            int i = 0;
            foreach (var item in GetAllParliaments())
            {
                if (item.ParliamentID.Equals(parliamentId))
                {
                    i++;
                }
            }
            
            if (parliamentId < 0)
            {
                throw new Exception("Invalid ID!");
            }
            else if (i == 0)
            {
                throw new Exception($"No such parliament with ID '{parliamentId}'!");
            }
            else
                parliamentRepo.DeleteParliament(parliamentId);
        }

        public IQueryable<Parliament> GetAllParliaments()
        {
            return parliamentRepo.GetAll();
        }

        public IEnumerable<KeyValuePair<string, int>> RepresentativesPerParliament()
        {
            var q = from x in partyMemberRepo.GetAll()
                    group x by x.PartyID into g
                    select new
                    {
                        PARTY_ID = g.Key,
                        MEMBER_COUNT = g.Count()
                    };

            var qx = from x in partyRepo.GetAll()
                     join z in q on x.PartyID equals z.PARTY_ID
                     let joinedItem = new { x.PartyID, x.PartyName, z.MEMBER_COUNT }
                     join y in parliamentRepo.GetAll() on x.ParliamentID equals y.ParliamentID
                     let a = new { y.ParliamentName, z.MEMBER_COUNT }
                     group a by a.ParliamentName into g
                     select new KeyValuePair<string, int>(g.Key, g.Sum(b => b.MEMBER_COUNT));

            return qx;
        }

        public Parliament GetParliamentByID(int id)
        {
            int i = 0;
            foreach (var item in GetAllParliaments())
            {
                if (item.ParliamentID.Equals(id))
                {
                    i++;
                }
            }
            if (id < 0)
            {
                throw new Exception("Invalid ID!");
            }
            else if (i == 0)
            {
                throw new Exception($"No such parliament with ID '{id}'");
            }
            else
                return parliamentRepo.GetOne(id);
        }

        public void ReplaceRulingParty(int parliamentId, string newParty)
        {
            if (GetParliamentByID(parliamentId).RulingParty.Equals(newParty))
            {
                throw new Exception("New ruling party cannot have the same name as the old one!");
            }
            else if (newParty.Equals(string.Empty))
            {
                throw new Exception("New name has to be given!");
            }
            else
                parliamentRepo.ReplaceRulingParty(parliamentId, newParty);
        }

        public void AddNewParliament(Parliament parliament)
        {
            bool rulingPartyExists = false;
            foreach (var item in partyRepo.GetAll().ToList())
            {
                if (item.PartyName.Equals(parliament.RulingParty))
                {
                    rulingPartyExists = true;
                    if (parliament.ParliamentName.Equals(string.Empty))
                    {
                        throw new Exception("Name has to be given!");
                    }
                    else if (parliament.RulingParty.Equals(string.Empty))
                    {
                        throw new Exception("Ruling party has to be given!");
                    }
                    else
                        parliamentRepo.AddNewParliament(parliament);
                }
            }

            if (!rulingPartyExists)
            {
                throw new Exception("Ruling party has to be an existing one!");
            }
        }

        public void UpdateParliament(Parliament parliament)
        {
            bool rulingPartyExists = false;
            foreach (var item in partyRepo.GetAll().ToList())
            {
                if (item.PartyName.Equals(parliament.RulingParty))
                {
                    rulingPartyExists = true;
                    if (parliament.ParliamentID < 0)
                    {
                        throw new Exception("Invalid ID!");
                    }
                    else if (parliament.ParliamentName.Equals(string.Empty))
                    {
                        throw new Exception("Name has to be given!");
                    }
                    else if (parliament.RulingParty.Equals(string.Empty))
                    {
                        throw new Exception("New name has to be given!");
                    }
                    else if (GetAllParliaments().Any(x => x.ParliamentID == parliament.ParliamentID))
                    {
                        parliamentRepo.UpdateParliament(parliament);
                    }
                    else
                        throw new Exception($"No such parliament with ID '{parliament.ParliamentID}'!");
                }
            }

            if (!rulingPartyExists)
            {
                throw new Exception("Ruling party has to be an existing one!");
            }
        }
    }
}
