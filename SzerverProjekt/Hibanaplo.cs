using System;

namespace SzerverProjekt
{
    public enum HibaSzint
    {
        Info,
        Figyelmeztetes,
        Hiba,
        Kritikus
    }

    public class Hibanaplo
    {
        private int naploAzonosito;
        private int szerverAzonosito;
        private DateTime idopont;
        private HibaSzint szint;
        private string uzenet;

        public int NaploAzonosito
        {
            get => naploAzonosito;
            set => naploAzonosito = value;
        }

        public int SzerverAzonosito
        {
            get => szerverAzonosito;
            set => szerverAzonosito = value;
        }

        public DateTime Idopont
        {
            get => idopont;
            set => idopont = value;
        }

        public HibaSzint Szint
        {
            get => szint;
            set => szint = value;
        }

        public string Uzenet
        {
            get => uzenet;
            set => uzenet = value;
        }

        public Hibanaplo(int naploAzonosito, int szerverAzonosito, DateTime idopont, HibaSzint szint, string uzenet)
        {
            this.naploAzonosito = naploAzonosito;
            this.szerverAzonosito = szerverAzonosito;
            this.idopont = idopont;
            this.szint = szint;
            this.uzenet = uzenet;
        }

        public override string ToString()
        {
            return $"{idopont:yyyy-MM-dd HH:mm} | [{szint}] Szerver ID: {szerverAzonosito} - {uzenet}";
        }
    }
}