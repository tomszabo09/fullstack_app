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
    public class PartyMemberController : ControllerBase
    {
        IPartyMemberLogic memberLogic;
        IHubContext<SignalRHub> hub;

        public PartyMemberController(IPartyMemberLogic memberLogic, IHubContext<SignalRHub> hub)
        {
            this.memberLogic = memberLogic;
            this.hub = hub;
        }

        // GET: /partymember
        [HttpGet]
        public IEnumerable<PartyMember> Get()
        {
            return memberLogic.GetAllMembers();
        }

        // GET /partymember/2
        [HttpGet("{id}")]
        public PartyMember Get(int id)
        {
            return memberLogic.GetMemberByID(id);
        }

        // POST /partymember
        [HttpPost]
        public void Post([FromBody] PartyMember value)
        {
            try
            {
                memberLogic.AddNewMember(value);
                hub.Clients.All.SendAsync("PartyMemberCreated", value);
            }
            catch (Exception)
            {
                ; //purposely left empty due to phantom items being added to gui listbox without defining foreign key
            }
        }

        // PUT /partymember
        [HttpPut]
        public void Put([FromBody] PartyMember value)
        {
            memberLogic.UpdateMember(value);
            hub.Clients.All.SendAsync("PartyMemberUpdated", value);
        }

        // DELETE /partymember/2
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var toDel = memberLogic.GetMemberByID(id);
            memberLogic.DeleteMember(id);
            hub.Clients.All.SendAsync("PartyMemberDeleted", toDel);
        }
    }
}
