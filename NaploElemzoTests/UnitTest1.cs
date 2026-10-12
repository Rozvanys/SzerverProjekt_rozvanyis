using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SzerverProjekt.Tests
{
    [TestFixture]
    public class NaploElemzoTests
    {
        private string szerverFajlUtvonal;
        private string naploFajlUtvonal;
        private NaploElemzo elemzo;

        [SetUp]
        public void Setup()
        {
            // Ideiglenes tesztfájlok létrehozása a pontos ellenőrzéshez
            szerverFajlUtvonal = Path.GetTempFileName();
            naploFajlUtvonal = Path.GetTempFileName();

            // Teszt adatok szerverekhez
            var szerverSorok = new[]
            {
                "1;Alpha-Web;Budapest;Linux",
                "2;Beta-DB;Frankfurt;Windows",
                "3;Gamma-Auth;Budapest;Linux",
                "4;Delta-Cache;Tokyo;FreeBSD"
            };
            File.WriteAllLines(szerverFajlUtvonal, szerverSorok);

            // Teszt adatok naplókhoz
            var naploSorok = new[]
            {
                "101;1;2026-10-01 08:30:00;Info;Indulás",
                "102;2;2026-10-01 13:15:00;Hiba;Adatbázis timeout",
                "103;1;2026-10-01 14:00:00;Kritikus;Összeomlás",
                "104;2;2026-10-01 10:00:00;Figyelmeztetes;Magas memória"
            };
            File.WriteAllLines(naploFajlUtvonal, naploSorok);

            elemzo = new NaploElemzo(szerverFajlUtvonal, naploFajlUtvonal);
        }

        [TearDown]
        public void TearDown()
        {
            // Fájlok eltávolítása a teszt után
            if (File.Exists(szerverFajlUtvonal)) File.Delete(szerverFajlUtvonal);
            if (File.Exists(naploFajlUtvonal)) File.Delete(naploFajlUtvonal);
        }

        [Test]
        public void BudapestSzerverekNeve_VisszaadjaACsakBudapestiNeveket()
        {
            var eredmeny = elemzo.BudapestSzerverekNeve();

            Assert.That(eredmeny.Count, Is.EqualTo(2));
            Assert.That(eredmeny, Is.EquivalentTo(new[] { "Alpha-Web", "Gamma-Auth" }));
        }

        [Test]
        public void KritikusUzenetekLekerdezese_VisszaadjaAKritikusUzenetet()
        {
            var eredmeny = elemzo.KritikusUzenetekLekerdezese();

            Assert.That(eredmeny.Count, Is.EqualTo(1));
            Assert.That(eredmeny.First(), Is.EqualTo("Összeomlás"));
        }

        [Test]
        public void SzerverEsHibaLeirasok_HelyesenOsszekapcsol()
        {
            var eredmeny = elemzo.SzerverEsHibaLeirasok();

            Assert.That(eredmeny.Count, Is.EqualTo(4));
            Assert.That(eredmeny, Does.Contain("Alpha-Web: Indulás"));
        }

        [Test]
        public void SulyosHibakSzama_HelyesenSzamol()
        {
            var eredmeny = elemzo.SulyosHibakSzama();

            // 1 Hiba + 1 Kritikus = 2
            Assert.That(eredmeny, Is.EqualTo(2));
        }

        [Test]
        public void VanNaploNelkuliSzerver_IgazatAdHaVanUresSzerver()
        {
            // A Delta-Cache (ID: 4) szerverhez nincs napló a tesztben
            var eredmeny = elemzo.VanNaploNelkuliSzerver();

            Assert.That(eredmeny, Is.True);
        }

        [Test]
        public void NaplokSzintSzerintCsoportositva_HelyesDictionarytAd()
        {
            var eredmeny = elemzo.NaplokSzintSzerintCsoportositva();

            Assert.That(eredmeny[HibaSzint.Info], Is.EqualTo(1));
            Assert.That(eredmeny[HibaSzint.Kritikus], Is.EqualTo(1));
        }

        [Test]
        public void LegtobbHibasSzerverNeve_AlphaWebetAdVissza()
        {
            // Alpha-Web-nek van egy Kritikus hibája
            var eredmeny = elemzo.LegtobbHibasSzerverNeve();

            Assert.That(eredmeny, Is.EqualTo("Alpha-Web"));
        }

        [Test]
        public void DeliNaplokSzerverNevvel_CsakDeliUtaniakatVisszaad()
        {
            var eredmeny = elemzo.DeliNaplokSzerverNevvel();

            // 13:15 és 14:00 órás naplók vannak 12 után
            Assert.That(eredmeny.Count, Is.EqualTo(2));
        }

        [Test]
        public void LinuxSzerverekRendezve_RendezettenAdjaVissza()
        {
            var eredmeny = elemzo.LinuxSzerverekRendezve();

            Assert.That(eredmeny.Count, Is.EqualTo(2));
            Assert.That(eredmeny.First().Nev, Is.EqualTo("Alpha-Web"));
            Assert.That(eredmeny.Last().Nev, Is.EqualTo("Gamma-Auth"));
        }

        [Test]
        public void LegkorabbiNaploIdopont_HelyesIdotAd()
        {
            var eredmeny = elemzo.LegkorabbiNaploIdopont();

            Assert.That(eredmeny, Is.EqualTo(new DateTime(2026, 10, 1, 8, 30, 0)));
        }

        [Test]
        public void BudapestiSzerverekUzenetei_KigyujtiAUzeneteket()
        {
            var eredmeny = elemzo.BudapestiSzerverekUzenetei();

            Assert.That(eredmeny.Count, Is.EqualTo(2));
            Assert.That(eredmeny, Does.Contain("Indulás"));
            Assert.That(eredmeny, Does.Contain("Összeomlás"));
        }

        [Test]
        public void ElofordultHibaSzintek_DistinctErtekeketAd()
        {
            var eredmeny = elemzo.ElofordultHibaSzintek();

            Assert.That(eredmeny.Count, Is.EqualTo(4)); // Info, Hiba, Kritikus, Figyelmeztetes
        }

        [Test]
        public void SzerverekOsSzerintCsoportositva_Mukodik()
        {
            var eredmeny = elemzo.SzerverekOsSzerintCsoportositva();

            Assert.That(eredmeny.Count(), Is.EqualTo(3)); // Linux, Windows, FreeBSD
        }

        [Test]
        public void AzelsoKritikusNaplo_MegtalaljaAKritikust()
        {
            var eredmeny = elemzo.AzelsoKritikusNaplo();

            Assert.That(eredmeny, Is.Not.Null);
            Assert.That(eredmeny.Szint, Is.EqualTo(HibaSzint.Kritikus));
        }

        [Test]
        public void AtlagosNaploSzamSzerverenkenet_HelyesenSzamol()
        {
            // 4 napló / 4 szerver = 1.0
            var eredmeny = elemzo.AtlagosNaploSzamSzerverenkenet();

            Assert.That(eredmeny, Is.EqualTo(1.0));
        }

        [Test]
        public void MindenSzerverAzonositoPozitiv_IgazatAd()
        {
            var eredmeny = elemzo.MindenSzerverAzonositoPozitiv();

            Assert.That(eredmeny, Is.True);
        }
    }
}