using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SzerverProjekt
{
    public class NaploElemzo
    {
        private List<Szerver> szerverek;
        private List<Hibanaplo> naplok;

        public List<Szerver> Szerverek
        {
            get => szerverek;
            set => szerverek = value;
        }

        public List<Hibanaplo> Naplok
        {
            get => naplok;
            set => naplok = value;
        }

        // Konstruktor: beolvassa a fájlokat és feltölti a listákat
        public NaploElemzo(string szerverFajlUtvonal, string naploFajlUtvonal)
        {
            szerverek = File.ReadAllLines(szerverFajlUtvonal)
                .Select(sor => sor.Split(';'))
                .Select(adatok => new Szerver(
                    int.Parse(adatok[0]),
                    adatok[1],
                    adatok[2],
                    adatok[3]))
                .ToList();

            naplok = File.ReadAllLines(naploFajlUtvonal)
                .Select(sor => sor.Split(';'))
                .Select(adatok => new Hibanaplo(
                    int.Parse(adatok[0]),
                    int.Parse(adatok[1]),
                    DateTime.Parse(adatok[2]),
                    Enum.Parse<HibaSzint>(adatok[3]),
                    adatok[4]))
                .ToList();
        }

        // 1. Add vissza a Budapesten található szerverek nevét egy List<string>-ben!
        public List<string> BudapestSzerverekNeve()
        {
            return szerverek
				.Where(s => s.Helyszin == "Budapest") //objektumokat gyűjtök ki
				.Select(s => s.Nev) //csak a nevüket adom vissza
				.ToList();
		}

        // 2. Add vissza az összes "Kritikus" szintű hibanapló szövegét egy listában!
        public List<string> KritikusUzenetekLekerdezese()
        {
            return naplok
				.Where(n => n.Szint == HibaSzint.Kritikus)
				.Select(n => n.Uzenet)
				.ToList();
		}

        // 3. JOIN feladat: Készíts egy listát, amely formázott szövegként tartalmazza a szerver nevét és a hibaüzenetet (pl. "SzerverNev: Üzenet")!
        public List<string> SzerverEsHibaLeirasok()
        {
            return szerverek.Join(
				naplok,
				szerver => szerver.Azonosito,
				naplo => naplo.SzerverAzonosito,
				(szerver, naplo) => $"{szerver.Nev}: {naplo.Uzenet}")
				.ToList();
		}

        // 4. Számold meg, hogy hány darab "Hiba" vagy "Kritikus" szintű bejegyzés érkezett összesen!
        public int SulyosHibakSzama()
        {
            return naplok.Count(n => n.Szint == HibaSzint.Hiba || n.Szint == HibaSzint.Kritikus);
        }

        // 5. Van-e olyan szerver, amelyhez egyáltalán nem tartozik hibanapló? (True/False)
        public bool VanNaploNelkuliSzerver()
        {
            return szerverek.Any(szerver => !naplok.Any(naplo => naplo.SzerverAzonosito == szerver.Azonosito));
		}

        // 6. Csoportosítsd a naplókat hiba szint szerint, és add vissza a darabszámokat egy Dictionary<HibaSzint, int>-ben!
        public Dictionary<HibaSzint, int> NaplokSzintSzerintCsoportositva()
        {
            return naplok.GroupBy(g => g.Szint)
				.ToDictionary(g => g.Key, g => g.Count());

		}

        // 7. JOIN + Aggregáció: Add vissza annak a szervernek a nevét, amelyiken a legtöbb súlyos (Hiba vagy Kritikus) hiba történt!
        public string LegtobbHibasSzerverNeve()
        {
            
        }

        // 8. Szűrés időpont alapján: Add vissza a 12:00 óra után történt naplóbejegyzéseket "SzerverNev (Óra:Perc) - Üzenet" formátumban!
        public List<string> DeliNaplokSzerverNevvel()
        {
            throw new NotImplementedException();
        }

        // 9. Add vissza az összes Linux operációs rendszerű szervert név szerint növekvő sorrendbe rendezve!
        public List<Szerver> LinuxSzerverekRendezve()
        {
            throw new NotImplementedException();
        }

        // 10. Add vissza a legelső (időben legkorábbi) hibanapló időpontját!
        public DateTime LegkorabbiNaploIdopont()
        {
            throw new NotImplementedException();
        }

        // 11. Kapcsold össze a budapesti szervereket a naplóikkal, és gyűjtsd ki az ott történt összes hibaüzenetet!
        public List<string> BudapestiSzerverekUzenetei()
        {
            throw new NotImplementedException();
        }

        // 12. Válaszd ki a naplókból a hiba szinteket, és Distinct segítségével add vissza az egyedi előfordulásaikat listában!
        public List<HibaSzint> ElofordultHibaSzintek()
        {
            throw new NotImplementedException();
        }

        // 13. Csoportosítsd a szervereket az OperaciosRendszer tulajdonságuk alapján!
        public IEnumerable<IGrouping<string, Szerver>> SzerverekOsSzerintCsoportositva()
        {
            throw new NotImplementedException();
        }

        // 14. Keresd meg és add vissza a legelső "Kritikus" szintű hibanapló objektumot! Ha nincs, adj vissza null-t.
        public Hibanaplo AzelsoKritikusNaplo()
        {
            throw new NotImplementedException();
        }

        // 15. Számítsd ki, hogy átlagosan hány hibanapló jut egy szerverre (double értékként)!
        public double AtlagosNaploSzamSzerverenkenet()
        {
            throw new NotImplementedException();
        }

        // 16. Ellenőrizd, hogy a flotta minden egyes szerverének az azonosítója pozitív szám-e (> 0)!
        public bool MindenSzerverAzonositoPozitiv()
        {
            throw new NotImplementedException();
        }
    }
}