using BV3N92_HFT_2021221.Logic;
using BV3N92_HFT_2021221.Models;
using BV3N92_HFT_2021221.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BV3N92_HFT_2021221.Test
{
    [TestFixture]
    public class Tests
    {
        private ParliamentLogic parliamentLogic { get; set; }
        private PartyLogic partyLogic { get; set; }
        private PartyMemberLogic partyMemberLogic { get; set; }
        
        [OneTimeSetUp]
        public void Setup()
        {
            Mock<IParliamentRepository> mockedParliamentRepo = new Mock<IParliamentRepository>();
            Mock<IPartyRepository> mockedPartyRepo = new Mock<IPartyRepository>();
            Mock<IPartyMemberRepository> mockedPartyMemberRepo = new Mock<IPartyMemberRepository>();

            mockedParliamentRepo.Setup(x => x.GetOne(It.IsAny<int>())).Returns(new Parliament()
            {
                ParliamentID = 5,
                ParliamentName = "TestParliament5",
                RulingParty = "TestParty9"
            });

            mockedPartyRepo.Setup(x => x.GetOne(It.IsAny<int>())).Returns(new Party()
            {
                PartyID = 9,
                ParliamentID = 5,
                PartyName = "TestParty9",
                Ideology = Ideologies.Conservative.ToString()
            });

            mockedPartyMemberRepo.Setup(x => x.GetOne(It.IsAny<int>())).Returns(new PartyMember()
            {
                MemberID = 17,
                LastName = "TestMember17",
                Age = 44,
                PartyID = 9
            });

            mockedParliamentRepo.Setup(x => x.GetAll()).Returns(this.FakeParliamentObjects);
            mockedPartyRepo.Setup(x => x.GetAll()).Returns(this.FakePartyObjects);
            mockedPartyMemberRepo.Setup(x => x.GetAll()).Returns(this.FakePartyMemberObjects);

            this.parliamentLogic = new ParliamentLogic(mockedParliamentRepo.Object, mockedPartyRepo.Object, mockedPartyMemberRepo.Object);
            this.partyLogic = new PartyLogic(mockedPartyRepo.Object, mockedPartyMemberRepo.Object, mockedParliamentRepo.Object);
            this.partyMemberLogic = new PartyMemberLogic(mockedPartyMemberRepo.Object, mockedPartyRepo.Object);
        }

        #region Basic Tests
        [Test]
        public void GetOneParliamentReturnsCorrectInstance()
        {
            Assert.That(this.parliamentLogic.GetParliamentByID(1).ParliamentName, Is.EqualTo("TestParliament5"));
        }

        [Test]
        public void GetOnePartyReturnsCorrectInstance()
        {
            Assert.That(this.partyLogic.GetPartyByID(1).Ideology, Is.EqualTo(Ideologies.Conservative.ToString()));
        }

        [Test]
        public void GetOnePartyMemberReturnsCorrectInstance()
        {
            Assert.That(this.partyMemberLogic.GetMemberByID(1).Age, Is.EqualTo(44));
        }
        #endregion

        #region Non-CRUD Tests
        [Test]
        public void RepresentativesPerParliamentIsCorrect()
        {
            Assert.That(this.parliamentLogic.RepresentativesPerParliament().Count().Equals(4));
        }

        [Test]
        public void ShortNamedMembersPerPartyIsCorrect()
        {
            Assert.That(this.partyLogic.ShortNamedMembersPerParty().ToList()[7].Value.Equals(1));
        }

        [Test]
        public void OldestMembersAgePerPartyIsCorrect()
        {
            Assert.That(this.partyLogic.OldestMembersAgePerParty().ToList()[5].Value.Equals(70));
        }

        [Test]
        public void JuniorMembersPerPartyIsCorrect()
        {
            Assert.That(this.partyLogic.JuniorMembersPerParty().ToList()[0].Value.Equals(2));
        }

        [Test]
        public void AVGAgeOfMembersPerPartyIsCorrect()
        {
            Assert.That(this.partyLogic.AVGAgeOfMembersPerParty().ToList()[1].Value.Equals(32.5));
        }
        #endregion

        #region Exception Tests
        [TestCase(1, "Test", "Test")] // id occupied
        [TestCase(6, "TestParliament2", "Test")] // parliament name occupied
        [TestCase(-1, "Test", "Test")] // invalid id
        [TestCase(6, "", "Test")] // no name
        [TestCase(6, "TestParliament6", "")] // no ruling party
        public void CreateParliamentWithIncorrectDataThrowsException(int parliamentId, string name, string rulingParty)
        {
            Assert.That(() => this.parliamentLogic.CreateParliament(parliamentId, name, rulingParty), Throws.TypeOf<Exception>());
        }

        [TestCase(1, 6, "Test", "Socialist")] // party id occupied
        [TestCase(-1, 5, "TestParty9", "Socialist")] // invalid party id
        [TestCase(9, -1, "TestParty9", "Socialist")] // invalid parliament id
        [TestCase(9, 5, "TestParty3", "Socialist")] // party name occupied
        [TestCase(9, 5, "", "Socialist")] // no party name
        [TestCase(9, 5, "TestParty9", "")] // no ideology
        [TestCase(9, 5, "TestParty9", "Ideology")] // ideology value not in enum pool
        public void CreatePartyWithIncorrectDataThrowsException(int partyId, int parliamentId, string partyName, string ideology)
        {
            Assert.That(() => this.partyLogic.CreateParty(partyId, parliamentId, partyName, ideology), Throws.TypeOf<Exception>());
        }

        [TestCase(1, "Test", 32, 7)] // member id occupied
        [TestCase(-1, "Test", 32, 7)] // invalid member id
        [TestCase(20, "", 32, 7)] // no name
        [TestCase(20, "Test", 15, 7)] // age less than 18
        [TestCase(20, "Test", 72, 7)] // age more than 70
        [TestCase(20, "Test", -1, 7)] // invalid age
        [TestCase(20, "Test", 32, -1)] // invalid party id
        public void CreatePartyMemberWithIncorrectDataThrowsException(int memberId, string lastName, int age, int partyId)
        {
            Assert.That(() => this.partyMemberLogic.CreateMember(memberId, lastName, age, partyId), Throws.TypeOf<Exception>());
        }
        #endregion

        private IQueryable<Parliament> FakeParliamentObjects()
        {
            Parliament p1 = new Parliament() { ParliamentID = 1, ParliamentName = "TestParliament1", RulingParty = "TestParty1" };
            Parliament p2 = new Parliament() { ParliamentID = 2, ParliamentName = "TestParliament2", RulingParty = "TestParty4" };
            Parliament p3 = new Parliament() { ParliamentID = 3, ParliamentName = "TestParliament3", RulingParty = "TestParty7" };
            Parliament p4 = new Parliament() { ParliamentID = 4, ParliamentName = "TestParliament4", RulingParty = "TestParty8" };

            p1.Parties = new List<Party>();
            p2.Parties = new List<Party>();
            p3.Parties = new List<Party>();
            p4.Parties = new List<Party>();

            Party pt1 = new Party() { PartyID = 1, ParliamentID = p1.ParliamentID, PartyName = "TestParty1", Ideology = Ideologies.Socialist.ToString() };
            Party pt2 = new Party() { PartyID = 2, ParliamentID = p1.ParliamentID, PartyName = "TestParty2", Ideology = Ideologies.Nationalist.ToString() };
            Party pt3 = new Party() { PartyID = 3, ParliamentID = p1.ParliamentID, PartyName = "TestParty3", Ideology = Ideologies.Conservative.ToString() };
            Party pt4 = new Party() { PartyID = 4, ParliamentID = p2.ParliamentID, PartyName = "TestParty4", Ideology = Ideologies.Nationalist.ToString() };
            Party pt5 = new Party() { PartyID = 5, ParliamentID = p2.ParliamentID, PartyName = "TestParty5", Ideology = Ideologies.Socialist.ToString() };
            Party pt6 = new Party() { PartyID = 6, ParliamentID = p3.ParliamentID, PartyName = "TestParty6", Ideology = Ideologies.Nationalist.ToString() };
            Party pt7 = new Party() { PartyID = 7, ParliamentID = p3.ParliamentID, PartyName = "TestParty7", Ideology = Ideologies.Conservative.ToString() };
            Party pt8 = new Party() { PartyID = 8, ParliamentID = p4.ParliamentID, PartyName = "TestParty8", Ideology = Ideologies.Socialist.ToString() };

            p1.Parties.Add(pt1);
            p1.Parties.Add(pt2);
            p1.Parties.Add(pt3);
            p2.Parties.Add(pt4);
            p2.Parties.Add(pt5);
            p3.Parties.Add(pt6);
            p3.Parties.Add(pt7);
            p4.Parties.Add(pt8);

            pt1.PartyMembers = new List<PartyMember>();
            pt2.PartyMembers = new List<PartyMember>();
            pt3.PartyMembers = new List<PartyMember>();
            pt4.PartyMembers = new List<PartyMember>();
            pt5.PartyMembers = new List<PartyMember>();
            pt6.PartyMembers = new List<PartyMember>();
            pt7.PartyMembers = new List<PartyMember>();
            pt8.PartyMembers = new List<PartyMember>();

            PartyMember pm1 = new PartyMember() { MemberID = 1, LastName = "TestMember1", Age = 20, PartyID = pt1.PartyID, Party = pt1 };
            PartyMember pm2 = new PartyMember() { MemberID = 2, LastName = "TestMember2", Age = 25, PartyID = pt1.PartyID, Party = pt1 };
            PartyMember pm3 = new PartyMember() { MemberID = 3, LastName = "TestMember3", Age = 30, PartyID = pt2.PartyID, Party = pt2 };
            PartyMember pm4 = new PartyMember() { MemberID = 4, LastName = "TestMember4", Age = 35, PartyID = pt2.PartyID, Party = pt2 };
            PartyMember pm5 = new PartyMember() { MemberID = 5, LastName = "TestMember5", Age = 40, PartyID = pt3.PartyID, Party = pt3 };
            PartyMember pm6 = new PartyMember() { MemberID = 6, LastName = "TestMember6", Age = 45, PartyID = pt3.PartyID, Party = pt3 };
            PartyMember pm7 = new PartyMember() { MemberID = 7, LastName = "TestMember7", Age = 50, PartyID = pt4.PartyID, Party = pt4 };
            PartyMember pm8 = new PartyMember() { MemberID = 8, LastName = "TestMember8", Age = 55, PartyID = pt4.PartyID, Party = pt4 };
            PartyMember pm9 = new PartyMember() { MemberID = 9, LastName = "TestMember9", Age = 60, PartyID = pt5.PartyID, Party = pt5 };
            PartyMember pm10 = new PartyMember() { MemberID = 10, LastName = "TestMember10", Age = 65, PartyID = pt5.PartyID, Party = pt5 };
            PartyMember pm11 = new PartyMember() { MemberID = 11, LastName = "TestMember11", Age = 70, PartyID = pt6.PartyID, Party = pt6 };
            PartyMember pm12 = new PartyMember() { MemberID = 12, LastName = "TestMember12", Age = 23, PartyID = pt6.PartyID, Party = pt6 };
            PartyMember pm13 = new PartyMember() { MemberID = 13, LastName = "TestMember13", Age = 33, PartyID = pt7.PartyID, Party = pt7 };
            PartyMember pm14 = new PartyMember() { MemberID = 14, LastName = "TestMember14", Age = 43, PartyID = pt7.PartyID, Party = pt7 };
            PartyMember pm15 = new PartyMember() { MemberID = 15, LastName = "TestMember15", Age = 53, PartyID = pt8.PartyID, Party = pt8 };
            PartyMember pm16 = new PartyMember() { MemberID = 16, LastName = "TM16", Age = 63, PartyID = pt8.PartyID, Party = pt8 };

            pt1.PartyMembers.Add(pm1);
            pt1.PartyMembers.Add(pm2);
            pt2.PartyMembers.Add(pm3);
            pt2.PartyMembers.Add(pm4);
            pt3.PartyMembers.Add(pm5);
            pt3.PartyMembers.Add(pm6);
            pt4.PartyMembers.Add(pm7);
            pt4.PartyMembers.Add(pm8);
            pt5.PartyMembers.Add(pm9);
            pt5.PartyMembers.Add(pm10);
            pt6.PartyMembers.Add(pm11);
            pt6.PartyMembers.Add(pm12);
            pt7.PartyMembers.Add(pm13);
            pt7.PartyMembers.Add(pm14);
            pt8.PartyMembers.Add(pm15);
            pt8.PartyMembers.Add(pm16);

            List<Parliament> items = new List<Parliament>();

            items.Add(p1);
            items.Add(p2);
            items.Add(p3);
            items.Add(p4);

            return items.AsQueryable();
        }

        private IQueryable<Party> FakePartyObjects()
        {
            Parliament p1 = new Parliament() { ParliamentID = 1, ParliamentName = "TestParliament1", RulingParty = "TestParty1" };
            Parliament p2 = new Parliament() { ParliamentID = 2, ParliamentName = "TestParliament2", RulingParty = "TestParty4" };
            Parliament p3 = new Parliament() { ParliamentID = 3, ParliamentName = "TestParliament3", RulingParty = "TestParty7" };
            Parliament p4 = new Parliament() { ParliamentID = 4, ParliamentName = "TestParliament4", RulingParty = "TestParty8" };

            p1.Parties = new List<Party>();
            p2.Parties = new List<Party>();
            p3.Parties = new List<Party>();
            p4.Parties = new List<Party>();

            Party pt1 = new Party() { PartyID = 1, ParliamentID = p1.ParliamentID, PartyName = "TestParty1", Ideology = Ideologies.Socialist.ToString() };
            Party pt2 = new Party() { PartyID = 2, ParliamentID = p1.ParliamentID, PartyName = "TestParty2", Ideology = Ideologies.Nationalist.ToString() };
            Party pt3 = new Party() { PartyID = 3, ParliamentID = p1.ParliamentID, PartyName = "TestParty3", Ideology = Ideologies.Conservative.ToString() };
            Party pt4 = new Party() { PartyID = 4, ParliamentID = p2.ParliamentID, PartyName = "TestParty4", Ideology = Ideologies.Nationalist.ToString() };
            Party pt5 = new Party() { PartyID = 5, ParliamentID = p2.ParliamentID, PartyName = "TestParty5", Ideology = Ideologies.Socialist.ToString() };
            Party pt6 = new Party() { PartyID = 6, ParliamentID = p3.ParliamentID, PartyName = "TestParty6", Ideology = Ideologies.Nationalist.ToString() };
            Party pt7 = new Party() { PartyID = 7, ParliamentID = p3.ParliamentID, PartyName = "TestParty7", Ideology = Ideologies.Conservative.ToString() };
            Party pt8 = new Party() { PartyID = 8, ParliamentID = p4.ParliamentID, PartyName = "TestParty8", Ideology = Ideologies.Socialist.ToString() };

            p1.Parties.Add(pt1);
            p1.Parties.Add(pt2);
            p1.Parties.Add(pt3);
            p2.Parties.Add(pt4);
            p2.Parties.Add(pt5);
            p3.Parties.Add(pt6);
            p3.Parties.Add(pt7);
            p4.Parties.Add(pt8);

            pt1.PartyMembers = new List<PartyMember>();
            pt2.PartyMembers = new List<PartyMember>();
            pt3.PartyMembers = new List<PartyMember>();
            pt4.PartyMembers = new List<PartyMember>();
            pt5.PartyMembers = new List<PartyMember>();
            pt6.PartyMembers = new List<PartyMember>();
            pt7.PartyMembers = new List<PartyMember>();
            pt8.PartyMembers = new List<PartyMember>();

            PartyMember pm1 = new PartyMember() { MemberID = 1, LastName = "TestMember1", Age = 20, PartyID = pt1.PartyID, Party = pt1 };
            PartyMember pm2 = new PartyMember() { MemberID = 2, LastName = "TestMember2", Age = 25, PartyID = pt1.PartyID, Party = pt1 };
            PartyMember pm3 = new PartyMember() { MemberID = 3, LastName = "TestMember3", Age = 30, PartyID = pt2.PartyID, Party = pt2 };
            PartyMember pm4 = new PartyMember() { MemberID = 4, LastName = "TestMember4", Age = 35, PartyID = pt2.PartyID, Party = pt2 };
            PartyMember pm5 = new PartyMember() { MemberID = 5, LastName = "TestMember5", Age = 40, PartyID = pt3.PartyID, Party = pt3 };
            PartyMember pm6 = new PartyMember() { MemberID = 6, LastName = "TestMember6", Age = 45, PartyID = pt3.PartyID, Party = pt3 };
            PartyMember pm7 = new PartyMember() { MemberID = 7, LastName = "TestMember7", Age = 50, PartyID = pt4.PartyID, Party = pt4 };
            PartyMember pm8 = new PartyMember() { MemberID = 8, LastName = "TestMember8", Age = 55, PartyID = pt4.PartyID, Party = pt4 };
            PartyMember pm9 = new PartyMember() { MemberID = 9, LastName = "TestMember9", Age = 60, PartyID = pt5.PartyID, Party = pt5 };
            PartyMember pm10 = new PartyMember() { MemberID = 10, LastName = "TestMember10", Age = 65, PartyID = pt5.PartyID, Party = pt5 };
            PartyMember pm11 = new PartyMember() { MemberID = 11, LastName = "TestMember11", Age = 70, PartyID = pt6.PartyID, Party = pt6 };
            PartyMember pm12 = new PartyMember() { MemberID = 12, LastName = "TestMember12", Age = 23, PartyID = pt6.PartyID, Party = pt6 };
            PartyMember pm13 = new PartyMember() { MemberID = 13, LastName = "TestMember13", Age = 33, PartyID = pt7.PartyID, Party = pt7 };
            PartyMember pm14 = new PartyMember() { MemberID = 14, LastName = "TestMember14", Age = 43, PartyID = pt7.PartyID, Party = pt7 };
            PartyMember pm15 = new PartyMember() { MemberID = 15, LastName = "TestMember15", Age = 53, PartyID = pt8.PartyID, Party = pt8 };
            PartyMember pm16 = new PartyMember() { MemberID = 16, LastName = "TM16", Age = 63, PartyID = pt8.PartyID, Party = pt8 };

            pt1.PartyMembers.Add(pm1);
            pt1.PartyMembers.Add(pm2);
            pt2.PartyMembers.Add(pm3);
            pt2.PartyMembers.Add(pm4);
            pt3.PartyMembers.Add(pm5);
            pt3.PartyMembers.Add(pm6);
            pt4.PartyMembers.Add(pm7);
            pt4.PartyMembers.Add(pm8);
            pt5.PartyMembers.Add(pm9);
            pt5.PartyMembers.Add(pm10);
            pt6.PartyMembers.Add(pm11);
            pt6.PartyMembers.Add(pm12);
            pt7.PartyMembers.Add(pm13);
            pt7.PartyMembers.Add(pm14);
            pt8.PartyMembers.Add(pm15);
            pt8.PartyMembers.Add(pm16);

            List<Party> items = new List<Party>();

            items.Add(pt1);
            items.Add(pt2);
            items.Add(pt3);
            items.Add(pt4);
            items.Add(pt5);
            items.Add(pt6);
            items.Add(pt7);
            items.Add(pt8);

            return items.AsQueryable();
        }

        private IQueryable<PartyMember> FakePartyMemberObjects()
        {
            Parliament p1 = new Parliament() { ParliamentID = 1, ParliamentName = "TestParliament1", RulingParty = "TestParty1" };
            Parliament p2 = new Parliament() { ParliamentID = 2, ParliamentName = "TestParliament2", RulingParty = "TestParty4" };
            Parliament p3 = new Parliament() { ParliamentID = 3, ParliamentName = "TestParliament3", RulingParty = "TestParty7" };
            Parliament p4 = new Parliament() { ParliamentID = 4, ParliamentName = "TestParliament4", RulingParty = "TestParty8" };

            p1.Parties = new List<Party>();
            p2.Parties = new List<Party>();
            p3.Parties = new List<Party>();
            p4.Parties = new List<Party>();

            Party pt1 = new Party() { PartyID = 1, ParliamentID = p1.ParliamentID, PartyName = "TestParty1", Ideology = Ideologies.Socialist.ToString() };
            Party pt2 = new Party() { PartyID = 2, ParliamentID = p1.ParliamentID, PartyName = "TestParty2", Ideology = Ideologies.Nationalist.ToString() };
            Party pt3 = new Party() { PartyID = 3, ParliamentID = p1.ParliamentID, PartyName = "TestParty3", Ideology = Ideologies.Conservative.ToString() };
            Party pt4 = new Party() { PartyID = 4, ParliamentID = p2.ParliamentID, PartyName = "TestParty4", Ideology = Ideologies.Nationalist.ToString() };
            Party pt5 = new Party() { PartyID = 5, ParliamentID = p2.ParliamentID, PartyName = "TestParty5", Ideology = Ideologies.Socialist.ToString() };
            Party pt6 = new Party() { PartyID = 6, ParliamentID = p3.ParliamentID, PartyName = "TestParty6", Ideology = Ideologies.Nationalist.ToString() };
            Party pt7 = new Party() { PartyID = 7, ParliamentID = p3.ParliamentID, PartyName = "TestParty7", Ideology = Ideologies.Conservative.ToString() };
            Party pt8 = new Party() { PartyID = 8, ParliamentID = p4.ParliamentID, PartyName = "TestParty8", Ideology = Ideologies.Socialist.ToString() };

            p1.Parties.Add(pt1);
            p1.Parties.Add(pt2);
            p1.Parties.Add(pt3);
            p2.Parties.Add(pt4);
            p2.Parties.Add(pt5);
            p3.Parties.Add(pt6);
            p3.Parties.Add(pt7);
            p4.Parties.Add(pt8);

            pt1.PartyMembers = new List<PartyMember>();
            pt2.PartyMembers = new List<PartyMember>();
            pt3.PartyMembers = new List<PartyMember>();
            pt4.PartyMembers = new List<PartyMember>();
            pt5.PartyMembers = new List<PartyMember>();
            pt6.PartyMembers = new List<PartyMember>();
            pt7.PartyMembers = new List<PartyMember>();
            pt8.PartyMembers = new List<PartyMember>();

            PartyMember pm1 = new PartyMember() { MemberID = 1, LastName = "TestMember1", Age = 20, PartyID = pt1.PartyID, Party = pt1 };
            PartyMember pm2 = new PartyMember() { MemberID = 2, LastName = "TestMember2", Age = 25, PartyID = pt1.PartyID, Party = pt1 };
            PartyMember pm3 = new PartyMember() { MemberID = 3, LastName = "TestMember3", Age = 30, PartyID = pt2.PartyID, Party = pt2 };
            PartyMember pm4 = new PartyMember() { MemberID = 4, LastName = "TestMember4", Age = 35, PartyID = pt2.PartyID, Party = pt2 };
            PartyMember pm5 = new PartyMember() { MemberID = 5, LastName = "TestMember5", Age = 40, PartyID = pt3.PartyID, Party = pt3 };
            PartyMember pm6 = new PartyMember() { MemberID = 6, LastName = "TestMember6", Age = 45, PartyID = pt3.PartyID, Party = pt3 };
            PartyMember pm7 = new PartyMember() { MemberID = 7, LastName = "TestMember7", Age = 50, PartyID = pt4.PartyID, Party = pt4 };
            PartyMember pm8 = new PartyMember() { MemberID = 8, LastName = "TestMember8", Age = 55, PartyID = pt4.PartyID, Party = pt4 };
            PartyMember pm9 = new PartyMember() { MemberID = 9, LastName = "TestMember9", Age = 60, PartyID = pt5.PartyID, Party = pt5 };
            PartyMember pm10 = new PartyMember() { MemberID = 10, LastName = "TestMember10", Age = 65, PartyID = pt5.PartyID, Party = pt5 };
            PartyMember pm11 = new PartyMember() { MemberID = 11, LastName = "TestMember11", Age = 70, PartyID = pt6.PartyID, Party = pt6 };
            PartyMember pm12 = new PartyMember() { MemberID = 12, LastName = "TestMember12", Age = 23, PartyID = pt6.PartyID, Party = pt6 };
            PartyMember pm13 = new PartyMember() { MemberID = 13, LastName = "TestMember13", Age = 33, PartyID = pt7.PartyID, Party = pt7 };
            PartyMember pm14 = new PartyMember() { MemberID = 14, LastName = "TestMember14", Age = 43, PartyID = pt7.PartyID, Party = pt7 };
            PartyMember pm15 = new PartyMember() { MemberID = 15, LastName = "TestMember15", Age = 53, PartyID = pt8.PartyID, Party = pt8 };
            PartyMember pm16 = new PartyMember() { MemberID = 16, LastName = "TM16", Age = 63, PartyID = pt8.PartyID, Party = pt8 };

            pt1.PartyMembers.Add(pm1);
            pt1.PartyMembers.Add(pm2);
            pt2.PartyMembers.Add(pm3);
            pt2.PartyMembers.Add(pm4);
            pt3.PartyMembers.Add(pm5);
            pt3.PartyMembers.Add(pm6);
            pt4.PartyMembers.Add(pm7);
            pt4.PartyMembers.Add(pm8);
            pt5.PartyMembers.Add(pm9);
            pt5.PartyMembers.Add(pm10);
            pt6.PartyMembers.Add(pm11);
            pt6.PartyMembers.Add(pm12);
            pt7.PartyMembers.Add(pm13);
            pt7.PartyMembers.Add(pm14);
            pt8.PartyMembers.Add(pm15);
            pt8.PartyMembers.Add(pm16);

            List<PartyMember> items = new List<PartyMember>();

            items.Add(pm1);
            items.Add(pm2);
            items.Add(pm3);
            items.Add(pm4);
            items.Add(pm5);
            items.Add(pm6);
            items.Add(pm7);
            items.Add(pm8);
            items.Add(pm9);
            items.Add(pm10);
            items.Add(pm11);
            items.Add(pm12);
            items.Add(pm13);
            items.Add(pm14);
            items.Add(pm15);
            items.Add(pm16);

            return items.AsQueryable();
        }
    }
}
