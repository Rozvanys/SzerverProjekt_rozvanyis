using System;
using System.Collections.Generic;
using System.Text;

namespace SzerverProjekt
{
    public class Szerver
    {
        private int azonosito;
        private string nev;
        private string helyszin;
        private string operaciosRendszer;

        public int Azonosito
        {
            get => azonosito;
            set => azonosito = value;
        }

        public string Nev
        {
            get => nev;
            set => nev = value;
        }

        public string Helyszin
        {
            get => helyszin;
            set => helyszin = value;
        }

        public string OperaciosRendszer
        {
            get => operaciosRendszer;
            set => operaciosRendszer = value;
        }

        public Szerver(int azonosito, string nev, string helyszin, string operaciosRendszer)
        {
            this.azonosito = azonosito;
            this.nev = nev;
            this.helyszin = helyszin;
            this.operaciosRendszer = operaciosRendszer;
        }

        public override string ToString()
        {
            return $"[Szerver #{azonosito}] {nev} ({helyszin} - {operaciosRendszer})";
        }
    }
}
