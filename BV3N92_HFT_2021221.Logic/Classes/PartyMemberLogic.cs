using BV3N92_HFT_2021221.Models;
using BV3N92_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BV3N92_HFT_2021221.Logic
{
    public class PartyMemberLogic : IPartyMemberLogic
    {
        IPartyMemberRepository memberRepo;
        IPartyRepository partyRepo;

        public PartyMemberLogic(IPartyMemberRepository repo, IPartyRepository partyRepo)
        {
            this.memberRepo = repo;
            this.partyRepo = partyRepo;
        }

        public void ChangeMemberAge(int memberId, int newAge)
        {
            if (memberId < 0)
            {
                throw new Exception("Invalid ID!");
            }
            else if (GetMemberByID(memberId).Age.Equals(newAge))
            {
                throw new Exception("New age cannot be old one!");
            }
            else if (GetMemberByID(memberId).Age < 18 || GetMemberByID(memberId).Age > 70)
            {
                throw new Exception("The age pool is between 18 and 70 years!");
            }
            else
                memberRepo.ChangeMemberAge(memberId, newAge);
        }

        public void ChangeMemberAllegiance(int memberId, int newPartyId)
        {
            if (memberId < 0)
            {
                throw new Exception("Invalid ID!");
            }
            else if (GetMemberByID(memberId).PartyID.Equals(newPartyId))
            {
                throw new Exception("New party ID matches old one!");
            }
            else
                memberRepo.ChangeMemberAllegiance(memberId, newPartyId);
        }

        public void ChangeMemberName(int memberId, string newName)
        {
            if (memberId < 0)
            {
                throw new Exception("Invalid ID!");
            }
            else if (GetMemberByID(memberId).LastName.Equals(string.Empty))
            {
                throw new Exception("Name has to be given!");
            }
            else if (GetMemberByID(memberId).LastName.Equals(newName))
            {
                throw new Exception("New name matches old one!");
            }
            else
                memberRepo.ChangeMemberName(memberId, newName);
        }

        public void CreateMember(int memberId, string lastName, int age, int partyId)
        {
            foreach (var item in GetAllMembers())
            {
                if (item.MemberID.Equals(memberId))
                {
                    throw new Exception($"A member with the ID '{memberId}' already exists!");
                }
            }

            if (memberId < 0)
            {
                throw new Exception("Invalid Member ID!");
            }
            else if (lastName.Equals(string.Empty))
            {
                throw new Exception("Name has to be given!");
            }
            else if (age < 0)
            {
                throw new Exception("Invalid age value! Age pool: 18-70");
            }
            else if (age < 18 || age > 70)
            {
                throw new Exception("The age pool is between 18 and 70 years!");
            }
            else if (partyId < 0)
            {
                throw new Exception("Invalid Party ID!");
            }

            foreach (var item in partyRepo.GetAll().ToList())
            {
                if (item.PartyID.Equals(partyId))
                {
                    memberRepo.CreateMember(memberId, lastName, age, partyId);
                }
            }
        }

        public void DeleteMember(int memberId)
        {
            int i = 0;
            foreach (var item in GetAllMembers())
            {
                if (item.MemberID.Equals(memberId))
                {
                    i++;
                }
            }

            if (memberId < 0)
            {
                throw new Exception("Invalid ID!");
            }
            else if (i == 0)
            {
                throw new Exception($"No such party member with ID '{memberId}'!");
            }
            else
                memberRepo.DeleteMember(memberId);
        }

        public IQueryable<PartyMember> GetAllMembers()
        {
            return memberRepo.GetAll();
        }

        public PartyMember GetMemberByID(int id)
        {
            int i = 0;
            foreach (var item in GetAllMembers())
            {
                if (item.MemberID.Equals(id))
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
                throw new Exception($"No such member with ID '{id}'!");
            }
            else
                return memberRepo.GetOne(id);
        }

        public void AddNewMember(PartyMember member)
        {
            foreach (var item in GetAllMembers())
            {
                if (item.MemberID.Equals(member.MemberID))
                {
                    throw new Exception($"A member with the ID '{member.MemberID}' already exists!");
                }
            }

            if (member.LastName.Equals(string.Empty))
            {
                throw new Exception("Name has to be given!");
            }
            else if (member.Age < 0)
            {
                throw new Exception("Invalid age value! Age pool: 18-70");
            }
            else if (member.Age < 18 || member.Age > 70)
            {
                throw new Exception("The age pool is between 18 and 70 years!");
            }
            else if (member.PartyID <= 0)
            {
                throw new Exception("Invalid Party ID!");
            }

            foreach (var item in partyRepo.GetAll().ToList())
            {
                if (item.PartyID.Equals(member.PartyID))
                {
                    memberRepo.AddNewMember(member);
                }
            }
        }

        public void UpdateMember(PartyMember member)
        {
            if (member.MemberID < 0)
            {
                throw new Exception("Invalid ID!");
            }
            else if (GetMemberByID(member.MemberID).Age < 18 || GetMemberByID(member.MemberID).Age > 70)
            {
                throw new Exception("The age pool is between 18 and 70 years!");
            }
            else if (GetMemberByID(member.MemberID).LastName.Equals(string.Empty))
            {
                throw new Exception("Name has to be given!");
            }
            else if (GetAllMembers().Any(x => x.MemberID == member.MemberID))
            {
                memberRepo.UpdateMember(member);
            }
            else
                throw new Exception($"No such party member with ID '{member.MemberID}'!");
        }
    }
}
