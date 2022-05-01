using BV3N92_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BV3N92_HFT_2021221.Repository
{
    public interface IPartyRepository : IRepository<Party>
    {
        void CreateParty(int partyId, int parliamentId, string partyName, string ideology);

        void ChangePartyName(int partyId, string newName);

        void ChangeIdeology(int partyId, string newIdeology);

        void DeleteParty(int partyId);

        void AddNewParty(Party party);

        void UpdateParty(Party party);
    }
}
