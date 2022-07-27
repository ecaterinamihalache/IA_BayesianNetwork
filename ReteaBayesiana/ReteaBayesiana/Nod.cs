using System;
using System.Collections.Generic;

namespace ReteaBayesiana
{
    public class Nod
    {
        private String _observatie;
        private int _pozitieX, _pozitieY;
        private List<String> _domeniu;
        private List<Nod> _parinti;
        private TabelaProbabilitati _tabelaProbabilitati;

        public Nod(String nume, List<String> domeniu)
        {
            Observatie = null;
            Domeniu = domeniu;
            Parinti = new List<Nod>();
            Nume = nume;
        }
        public Nod(String nume, List<String> domeniu, List<Nod> parinti)
        {
            Observatie = null;
            Domeniu = domeniu;
            Parinti = parinti;
            Nume = nume;
        }
        public Nod(String nume, List<String> domeniu, int pozX, int pozY)
        {
            Observatie = null;
            Domeniu = domeniu;
            Nume = nume;
            Parinti = new List<Nod>();
            PozitieX = pozX;
            PozitieY = pozY;
        }
        public Nod(String nume, List<String> domeniu, int pozX, int pozY, List<Nod> parinti)
        {
            Observatie = null;
            Domeniu = domeniu;
            Parinti = parinti;
            Nume = nume;
            PozitieX = pozX;
            PozitieY = pozY;
        }

        public String Nume { set; get; }
        public List<String> Domeniu { set => _domeniu = value; get => _domeniu; }
        public int PozitieX { set => _pozitieX = value; get => _pozitieX; }
        public int PozitieY { set => _pozitieY = value; get => _pozitieY; }

        public String Observatie
        {
            set
            {
                if (value == null || _domeniu.Contains(value))
                    _observatie = value;
                else
                    throw new Exception("Observatia nu face parte din domeniul nodului!");
            }
            get => _observatie;          
        }
        public List<Nod> Parinti { set => _parinti = value; get => _parinti; }
        public TabelaProbabilitati Tabela { get => _tabelaProbabilitati; }

        public void CreazaTabela(double[,] tabela)
        {
            _tabelaProbabilitati = new TabelaProbabilitati(this, tabela);
        }

        public string[] ListaObservatiiParinti()
        {
            // Construieste lista de Observatii ale parintilor
            string[] observatiiParinti = new string[Parinti.Count];
            for (int indexParinte = 0; indexParinte < Parinti.Count; indexParinte++)
            {
                // Extragem parintele
                Nod parinte = Parinti[indexParinte];

                // Adaugam in lista observatia sa
                observatiiParinti[indexParinte] = parinte.Observatie;
            }
            return observatiiParinti;
        }
    }
}
