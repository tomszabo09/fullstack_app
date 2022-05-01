using BV3N92_HFT_2021221.Models;
using BV3N92_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BV3N92_HFT_2021221.Logic
{
    public class PartyLogic : IPartyLogic
    {
        IPartyRepository partyRepo;
        IPartyMemberRepository partyMemberRepo;
        IParliamentRepository parliamentRepo;

        public PartyLogic(IPartyRepository repo, IPartyMemberRepository partyMemberRepository, IParliamentRepository parliamentRepo)
        {
            this.partyRepo = repo;
            this.partyMemberRepo = partyMemberRepository;
            this.parliamentRepo = parliamentRepo;
        }

        public void ChangeIdeology(int partyId, string newIdeology)
        {
            if (partyId < 0)
            {
                throw new Exception("Invalid ID!");
            }
            else if (newIdeology.Equals(string.Empty))
            {
                throw new Exception("Ideology has to be given!");
            }
            else if (!newIdeology.Equals(Ideologies.Socialist.ToString()) && !newIdeology.Equals(Ideologies.Conservative.ToString()) && !newIdeology.Equals(Ideologies.Nationalist.ToString()))
            {
                throw new Exception($"Non-existent ideology! Ideology pool: {Ideologies.Socialist}, {Ideologies.Conservative}, {Ideologies.Nationalist}");
            }
            else if (GetPartyByID(partyId).Ideology.Equals(newIdeology))
            {
                throw new Exception("New ideology cannot match old one!");
            }
            else
                partyRepo.ChangeIdeology(partyId, newIdeology);
        }

        public void ChangePartyName(int partyId, string newName)
        {
            if (partyId < 0)
            {
                throw new Exception("Invalid ID!");
            }
            else if (newName.Equals(string.Empty))
            {
                throw new Exception("Name has to be given!");
            }
            else if (GetPartyByID(partyId).PartyName.Equals(newName))
            {
                throw new Exception("New name cannot match old one!");
            }
            else
                partyRepo.ChangePartyName(partyId, newName);
        }

        public void CreateParty(int partyId, int parliamentId, string partyName, string ideology)
        {
            foreach (var item in GetAllParties())
            {
                if (item.PartyName.Equals(partyName))
                {
                    throw new Exception($"A party with the name '{partyName}' already exists!");
                }
                else if (item.PartyID.Equals(partyId))
                {
                    throw new Exception($"A party with ID '{partyId}' already exists!");
                }
            }

            if (partyId < 0)
            {
                throw new Exception("Invalid Party ID!");
            }
            else if (parliamentId < 0)
            {
                throw new Exception("Invalid Parliament ID!");
            }
            else if (partyName.Equals(string.Empty))
            {
                throw new Exception("Name has to be given!");
            }
            else if (ideology.Equals(string.Empty))
            {
                throw new Exception("Ideology has to be given!");
            }
            else if (!ideology.Equals(Ideologies.Socialist.ToString()) && !ideology.Equals(Ideologies.Conservative.ToString()) && !ideology.Equals(Ideologies.Nationalist.ToString()))
            {
                throw new Exception($"Non-existent ideology! Ideology pool: {Ideologies.Socialist}, {Ideologies.Conservative}, {Ideologies.Nationalist}");
            }
            else
                partyRepo.CreateParty(partyId, parliamentId, partyName, ideology);
        }

        public void DeleteParty(int partyId)
        {
            int i = 0;
            foreach (var item in GetAllParties())
            {
                if (item.PartyID.Equals(partyId))
                {
                    i++;
                }
            }

            if (partyId < 0)
            {
                throw new Exception("Invalid ID!");
            }
            else if (i == 0)
            {
                throw new Exception($"No such party with ID '{partyId}'!");
            }
            else
                partyRepo.DeleteParty(partyId);
        }

        public IQueryable<Party> GetAllParties()
        {
            return partyRepo.GetAll();
        }

        public Party GetPartyByID(int id)
        {
            int i = 0;
            foreach (var item in GetAllParties())
            {
                if (item.PartyID.Equals(id))
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
                throw new Exception($"No such party with ID '{id}'!");
            }
            else
                return partyRepo.GetOne(id);
        }

        public IEnumerable<KeyValuePair<string, int>> ShortNamedMembersPerParty()
        {
            var q = from x in partyMemberRepo.GetAll()
                    group x by x.Party.PartyName into g
                    select new KeyValuePair<string, int>(g.Key, g.Where(y => y.LastName.Length < 6).Count());

            return q;
        }

        public IEnumerable<KeyValuePair<string, int>> OldestMembersAgePerParty()
        {
            var q = from x in partyMemberRepo.GetAll()
                    group x by x.Party.PartyName into g
                    select new KeyValuePair<string, int>(g.Key, g.Max(y => y.Age));

            return q;
        }

        public IEnumerable<KeyValuePair<string, int>> JuniorMembersPerParty()
        {
            var q = from x in partyMemberRepo.GetAll()
                    group x by x.Party.PartyName into g
                    select new KeyValuePair<string, int>(g.Key, g.Where(y => y.Age < 30).Count());

            return q;
        }

        public IEnumerable<KeyValuePair<string, double>> AVGAgeOfMembersPerParty()
        {
            var q = from x in partyMemberRepo.GetAll()
                    group x by x.Party.PartyName into g
                    select new KeyValuePair<string, double>(g.Key, g.Average(y => y.Age));

            return q;
        }

        public void AddNewParty(Party party)
        {
            if (party.ParliamentID <= 0)
            {
                throw new Exception("Invalid Parliament ID!");
            }
            else if (party.PartyName.Equals(string.Empty))
            {
                throw new Exception("Name has to be given!");
            }
            else if (party.Ideology.Equals(string.Empty))
            {
                throw new Exception("Ideology has to be given!");
            }
            else if (!party.Ideology.Equals(Ideologies.Socialist.ToString()) && !party.Ideology.Equals(Ideologies.Conservative.ToString()) && !party.Ideology.Equals(Ideologies.Nationalist.ToString()))
            {
                throw new Exception($"Non-existent ideology! Ideology pool: {Ideologies.Socialist}, {Ideologies.Conservative}, {Ideologies.Nationalist}");
            }

            foreach (var item in parliamentRepo.GetAll().ToList())
            {
                if (item.ParliamentID.Equals(party.ParliamentID))
                {
                    partyRepo.AddNewParty(party);
                }
            }
        }

        public void UpdateParty(Party party)
        {
            if (party.PartyID < 0)
            {
                throw new Exception("Invalid ID!");
            }
            else if (party.Ideology.Equals(string.Empty))
            {
                throw new Exception("Ideology has to be given!");
            }
            else if (!party.Ideology.Equals(Ideologies.Socialist.ToString()) && !party.Ideology.Equals(Ideologies.Conservative.ToString()) && !party.Ideology.Equals(Ideologies.Nationalist.ToString()))
            {
                throw new Exception($"Non-existent ideology! Ideology pool: {Ideologies.Socialist}, {Ideologies.Conservative}, {Ideologies.Nationalist}");
            }
            else if (party.PartyName.Equals(string.Empty))
            {
                throw new Exception("Name has to be given!");
            }
            else if (GetAllParties().Any(x => x.PartyID == party.PartyID))
            {
                partyRepo.UpdateParty(party);
            }
            else
                throw new Exception($"No such party with ID '{party.PartyID}'!");
        }
    }
}
