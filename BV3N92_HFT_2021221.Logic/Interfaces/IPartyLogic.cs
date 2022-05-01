using BV3N92_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BV3N92_HFT_2021221.Logic
{
    public interface IPartyLogic
    {
        Party GetPartyByID(int id);

        void CreateParty(int partyId, int parliamentId, string partyName, string ideology);

        void ChangePartyName(int partyId, string newName);

        void ChangeIdeology(int partyId, string newIdeology);

        void DeleteParty(int partyId);

        IEnumerable<KeyValuePair<string, int>> ShortNamedMembersPerParty();

        IEnumerable<KeyValuePair<string, int>> OldestMembersAgePerParty();

        IEnumerable<KeyValuePair<string, int>> JuniorMembersPerParty();

        IEnumerable<KeyValuePair<string, double>> AVGAgeOfMembersPerParty();

        IQueryable<Party> GetAllParties();

        void AddNewParty(Party party);

        void UpdateParty(Party party);
    }
}
