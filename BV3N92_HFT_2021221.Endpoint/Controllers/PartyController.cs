using BV3N92_HFT_2021221.Endpoint.Services;
using BV3N92_HFT_2021221.Logic;
using BV3N92_HFT_2021221.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BV3N92_HFT_2021221.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PartyController : ControllerBase
    {
        IPartyLogic partyLogic;
        IHubContext<SignalRHub> hub;

        public PartyController(IPartyLogic partyLogic, IHubContext<SignalRHub> hub)
        {
            this.partyLogic = partyLogic;
            this.hub = hub;
        }

        // GET: /party
        [HttpGet]
        public IEnumerable<Party> Get()
        {
            return partyLogic.GetAllParties();
        }

        // GET /party/2
        [HttpGet("{id}")]
        public Party Get(int id)
        {
            return partyLogic.GetPartyByID(id);
        }

        // POST /party
        [HttpPost]
        public void Post([FromBody] Party value)
        {
            try
            {
                partyLogic.AddNewParty(value);
                hub.Clients.All.SendAsync("PartyCreated", value);
            }
            catch (Exception)
            {
                ; //purposely left empty due to phantom items being added to gui listbox without defining foreign key
            }
        }

        // PUT /party
        [HttpPut]
        public void Put([FromBody] Party value)
        {
            partyLogic.UpdateParty(value);
            hub.Clients.All.SendAsync("PartyUpdated", value);
        }

        // DELETE /party/2
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var toDel = partyLogic.GetPartyByID(id);
            partyLogic.DeleteParty(id);

            hub.Clients.All.SendAsync("PartyDeleted", toDel);
            hub.Clients.All.SendAsync("PartyMemberDeleted", null);
        }
    }
}
