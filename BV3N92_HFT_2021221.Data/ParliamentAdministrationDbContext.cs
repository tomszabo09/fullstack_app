using BV3N92_HFT_2021221.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BV3N92_HFT_2021221.Data
{
    public class ParliamentAdministrationDbContext : DbContext
    {
        public virtual DbSet<Parliament> Parliament { get; set; }
        public virtual DbSet<Party> Parties { get; set; }
        public virtual DbSet<PartyMember> PartyMembers { get; set; }
        public ParliamentAdministrationDbContext()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\LocalDatabase.mdf;Integrated Security=True;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Party>(entity =>
            {
                entity.HasOne(party => party.Parliament)
                    .WithMany(parliament => parliament.Parties)
                    .HasForeignKey(party => party.ParliamentID);

                entity.HasMany(partymember => partymember.PartyMembers)
                .WithOne(party => party.Party)
                .HasForeignKey(partymember => partymember.PartyID);
            });

            Parliament ger = new Parliament() { ParliamentID = 1, ParliamentName = "Reichstag", RulingParty = "Deutschkonservative Partei" };
            Parliament eng = new Parliament() { ParliamentID = 2, ParliamentName = "House of Commons", RulingParty = "English Socialist Party" };
            Parliament hun = new Parliament() { ParliamentID = 3, ParliamentName = "Országház", RulingParty = "Magyar Nemzeti Párt" };


            Party gerleft = new Party() { PartyID = 1, ParliamentID = ger.ParliamentID, PartyName = "Sozialdemokratische Partei Deutschlands", Ideology = Ideologies.Socialist.ToString() };
            Party gercenter = new Party() { PartyID = 2, ParliamentID = ger.ParliamentID, PartyName = "Deutschkonservative Partei", Ideology = Ideologies.Conservative.ToString() };
            Party gerright = new Party() { PartyID = 3, ParliamentID = ger.ParliamentID, PartyName = "Deutsche Vaterlandspartei", Ideology = Ideologies.Nationalist.ToString() };

            Party engleft = new Party() { PartyID = 4, ParliamentID = eng.ParliamentID, PartyName = "English Socialist Party", Ideology = Ideologies.Socialist.ToString() };
            Party engcenter = new Party() { PartyID = 5, ParliamentID = eng.ParliamentID, PartyName = "English Conservative Party", Ideology = Ideologies.Conservative.ToString() };
            Party engright = new Party() { PartyID = 6, ParliamentID = eng.ParliamentID, PartyName = "English National Party", Ideology = Ideologies.Nationalist.ToString() };

            Party hunleft = new Party() { PartyID = 7, ParliamentID = hun.ParliamentID, PartyName = "Magyar Szociáldemokrata Párt", Ideology = Ideologies.Socialist.ToString() };
            Party huncenter = new Party() { PartyID = 8, ParliamentID = hun.ParliamentID, PartyName = "Magyar Konzervatív Párt", Ideology = Ideologies.Conservative.ToString() };
            Party hunright = new Party() { PartyID = 9, ParliamentID = hun.ParliamentID, PartyName = "Magyar Nemzeti Párt", Ideology = Ideologies.Nationalist.ToString() };

            var members = new List<PartyMember>()
            {
                // ger
                new PartyMember() { MemberID = 1, LastName = "Mayer", Age = 51, PartyID = gerleft.PartyID },
                new PartyMember() { MemberID = 2, LastName = "Schulze", Age = 39, PartyID = gerleft.PartyID },
                new PartyMember() { MemberID = 3, LastName = "Beck", Age = 67, PartyID = gerleft.PartyID },
                new PartyMember() { MemberID = 4, LastName = "Dreyer", Age = 23, PartyID = gerleft.PartyID },
                new PartyMember() { MemberID = 5, LastName = "Fuchs", Age = 45, PartyID = gerleft.PartyID },

                new PartyMember() { MemberID = 6, LastName = "Schacht", Age = 48, PartyID = gercenter.PartyID },
                new PartyMember() { MemberID = 7, LastName = "von Tirpitz", Age = 49, PartyID = gercenter.PartyID },
                new PartyMember() { MemberID = 8, LastName = "Arnold", Age = 52, PartyID = gercenter.PartyID },
                new PartyMember() { MemberID = 9, LastName = "von Hohenzoller", Age = 44, PartyID = gercenter.PartyID },
                new PartyMember() { MemberID = 10, LastName = "Plank", Age = 68, PartyID = gercenter.PartyID },

                new PartyMember() { MemberID = 11, LastName = "Weber", Age = 29, PartyID = gerright.PartyID },
                new PartyMember() { MemberID = 12, LastName = "Dirksen", Age = 30, PartyID = gerright.PartyID },
                new PartyMember() { MemberID = 13, LastName = "Taube", Age = 21, PartyID = gerright.PartyID },
                new PartyMember() { MemberID = 14, LastName = "Möller", Age = 45, PartyID = gerright.PartyID },
                new PartyMember() { MemberID = 15, LastName = "von Reuter", Age = 37, PartyID = gerright.PartyID },

                // eng
                new PartyMember() { MemberID = 16, LastName = "Blanton", Age = 64, PartyID = engleft.PartyID },
                new PartyMember() { MemberID = 17, LastName = "Richardson", Age = 25, PartyID = engleft.PartyID },
                new PartyMember() { MemberID = 18, LastName = "Padilla", Age = 67, PartyID = engleft.PartyID },
                new PartyMember() { MemberID = 19, LastName = "Cobbett", Age = 26, PartyID = engleft.PartyID },
                new PartyMember() { MemberID = 20, LastName = "Glover", Age = 69, PartyID = engleft.PartyID },

                new PartyMember() { MemberID = 21, LastName = "Stewart", Age = 22, PartyID = engcenter.PartyID },
                new PartyMember() { MemberID = 22, LastName = "Myers", Age = 20, PartyID = engcenter.PartyID },
                new PartyMember() { MemberID = 23, LastName = "Norman", Age = 27, PartyID = engcenter.PartyID },
                new PartyMember() { MemberID = 24, LastName = "Stephenson", Age = 38, PartyID = engcenter.PartyID },
                new PartyMember() { MemberID = 25, LastName = "Horton", Age = 32, PartyID = engcenter.PartyID },

                new PartyMember() { MemberID = 26, LastName = "Salvage", Age = 39, PartyID = engright.PartyID },
                new PartyMember() { MemberID = 27, LastName = "Ryan", Age = 65, PartyID = engright.PartyID },
                new PartyMember() { MemberID = 28, LastName = "Thornton", Age = 24, PartyID = engright.PartyID },
                new PartyMember() { MemberID = 29, LastName = "Howell", Age = 36, PartyID = engright.PartyID },
                new PartyMember() { MemberID = 30, LastName = "Nelson", Age = 18, PartyID = engright.PartyID },

                // hun
                new PartyMember() { MemberID = 31, LastName = "Vörös", Age = 58, PartyID = hunleft.PartyID },
                new PartyMember() { MemberID = 32, LastName = "Tóth", Age = 56, PartyID = hunleft.PartyID },
                new PartyMember() { MemberID = 33, LastName = "Hajdú", Age = 50, PartyID = hunleft.PartyID },
                new PartyMember() { MemberID = 34, LastName = "Kocsis", Age = 55, PartyID = hunleft.PartyID },
                new PartyMember() { MemberID = 35, LastName = "Biró", Age = 31, PartyID = hunleft.PartyID },

                new PartyMember() { MemberID = 36, LastName = "Budai", Age = 42, PartyID = huncenter.PartyID },
                new PartyMember() { MemberID = 37, LastName = "Magyar", Age = 23, PartyID = huncenter.PartyID },
                new PartyMember() { MemberID = 38, LastName = "Molnár", Age = 61, PartyID = huncenter.PartyID },
                new PartyMember() { MemberID = 39, LastName = "Bács", Age = 47, PartyID = huncenter.PartyID },
                new PartyMember() { MemberID = 40, LastName = "Horváth", Age = 46, PartyID = huncenter.PartyID },

                new PartyMember() { MemberID = 41, LastName = "Váradi", Age = 51, PartyID = hunright.PartyID },
                new PartyMember() { MemberID = 42, LastName = "Dudás", Age = 57, PartyID = hunright.PartyID },
                new PartyMember() { MemberID = 43, LastName = "Csatár", Age = 28, PartyID = hunright.PartyID },
                new PartyMember() { MemberID = 44, LastName = "Hegedüs", Age = 41, PartyID = hunright.PartyID },
                new PartyMember() { MemberID = 45, LastName = "Németh", Age = 54, PartyID = hunright.PartyID }
            };

            modelBuilder.Entity<Parliament>().HasData(ger, eng, hun);
            modelBuilder.Entity<Party>().HasData(gerleft, gercenter, gerright, engleft, engcenter, engright, hunleft, huncenter, hunright);
            modelBuilder.Entity<PartyMember>().HasData(members);
        }
    }
}
