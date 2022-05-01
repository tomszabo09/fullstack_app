using BV3N92_HFT_2021221.Logic;
using BV3N92_HFT_2021221.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BV3N92_HFT_2021221.Endpoint.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StatController : ControllerBase
    {
        IParliamentLogic parliamentLogic;
        IPartyLogic partyLogic;

        public StatController(IParliamentLogic parliamentLogic, IPartyLogic partyLogic)
        {
            this.parliamentLogic = parliamentLogic;
            this.partyLogic = partyLogic;
        }

        // GET: stat/representativesperparliament
        [HttpGet]
        public IEnumerable<KeyValuePair<string, int>> RepresentativesPerParliament()
        {
            return parliamentLogic.RepresentativesPerParliament();
        }

        // GET: stat/shortnamedmembersperparty
        [HttpGet]
        public IEnumerable<KeyValuePair<string, int>> ShortNamedMembersPerParty()
        {
            return partyLogic.ShortNamedMembersPerParty();
        }

        // GET: stat/oldestmembersageperparty
        [HttpGet]
        public IEnumerable<KeyValuePair<string, int>> OldestMembersAgePerParty()
        {
            return partyLogic.OldestMembersAgePerParty();
        }

        // GET: stat/juniormembersperparty
        [HttpGet]
        public IEnumerable<KeyValuePair<string, int>> JuniorMembersPerParty()
        {
            return partyLogic.JuniorMembersPerParty();
        }

        // GET: stat/avgageofmembersperparty
        [HttpGet]
        public IEnumerable<KeyValuePair<string, double>> AVGAgeOfMembersPerParty()
        {
            return partyLogic.AVGAgeOfMembersPerParty();
        }
    }
}
