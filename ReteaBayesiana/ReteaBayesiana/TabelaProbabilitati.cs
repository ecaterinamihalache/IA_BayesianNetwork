using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReteaBayesiana
{
    public class TabelaProbabilitati
    {
        private Nod _nod;
        private double[,] _tabela;

        public Nod NodCentral { set => _nod = value; get => _nod; }
        public double[,] Tabela {
            set => _tabela = value;
            get => _tabela; 
        }

        public TabelaProbabilitati(Nod nodCurent, double[,] tabelaProbabilitati)
        {
            NodCentral = nodCurent;
            Tabela = tabelaProbabilitati;
            // ValidareTabela();
        }

        public TabelaProbabilitati(Nod nodCurent)
        {
            NodCentral = nodCurent;

            // Numarul de linii
            int linii = 1;
            foreach (Nod parinte in NodCentral.Parinti)
                linii *= parinte.Domeniu.Count;

            Tabela = new double[linii, NodCentral.Domeniu.Count];
        }

        /// <summary>
        /// Seteaza in tabela de probabilitati valori pe pozitia potrivita
        /// </summary>
        /// <param name="stariParinti"> Starile cunoscute ale parintilor </param>
        /// <param name="stareNod"> Starea nodului pentru care se seteaza probabilitatea </param>
        /// <param name="probabilitate"> Valoarea probabilitatii </param>
        public void SetProbabilitateStari(String[] stariParinti, String stareNod, double probabilitate)
        {
            // Probabilitatea trebuie sa fie intre 0 si 1
            if (probabilitate < 0 || probabilitate > 1)
                throw new Exception("Probabilitatea trebuie sa fie o valoare intre 0 si 1.");

            // Verificare dimensiune vector stariParinti
            if (stariParinti.Length != NodCentral.Parinti.Count)
                throw new Exception("Lista de stari ale parintilor trebuie sa fie la fel de lunga ca lista parintilor.");

            // Starea in nod
            int indexInStareNod = _nod.Domeniu.IndexOf(stareNod);

            // Daca nu exista starea, aruncam eroare
            if(indexInStareNod < 0)
                throw new Exception("Stare \"" + stareNod + "\" inexistenta pentru nodul \"" + _nod.Nume + "\"");

            int linie = CalculLinieStari(stariParinti);

            // Am obtinut linia si coloana, asa ca modificiam pozitia din nod
            Tabela[linie, indexInStareNod] = probabilitate;
        }

        // Ia probabilitatea din tabela pe baza descrierii starilor parintilor si a starii nodului
        public double GetProbabilitateStari(string[] stariParinti, string stareNod)
        {
            // Verificare dimensiune vector stariParinti
            if (stariParinti.Length != NodCentral.Parinti.Count)
                throw new Exception("Lista de stari ale parintilor trebuie sa fie la fel de lunga ca lista parintilor.");

            // Starea in nod
            int indexInStareNod = _nod.Domeniu.IndexOf(stareNod);

            // Daca nu exista starea, aruncam eroare
            if (indexInStareNod < 0)
                throw new Exception("Stare \"" + stareNod + "\" inexistenta pentru nodul \"" + _nod.Nume + "\"");

            int linie = CalculLinieStari(stariParinti);

            // Am obtinut linia si coloana, asa ca modificiam pozitia din nod
            return _tabela[linie, indexInStareNod];
        }

        /// <summary>
        /// Metoda pentru setarea unei valori din tabele dupa linie si coloana
        /// </summary>
        /// <param name="linie"> Linia din tabel care trebuie modificata </param>
        /// <param name="coloana"> Coloana din tabel care trebuie modificata </param>
        /// <param name="probabilitate"> Probabilitatea setata </param>
        public void SetProbabilitate(int linie, int coloana, double probabilitate)
        {
            // Probabilitatea trebuie sa fie intre 0 si 1
            if (probabilitate < 0 || probabilitate > 1)
                throw new Exception("Probabilitatea trebuie sa fie o valoare intre 0 si 1.");

            // Am obtinut linia si coloana, asa ca modificiam pozitia din nod
            Tabela[linie, coloana] = probabilitate;
        }

        /// <summary>
        /// Metoda responsabila cu accesul la probabilitatile din tabela
        /// </summary>
        /// <param name="linie"> Linia la care se face accesul </param>
        /// <param name="coloana"> Coloana la care se face accesul </param>
        /// <returns> Probabilitatea aflata la indecsii oferiti </returns>
        public double GetProbabilitate(int linie, int coloana)
        {
            return Tabela[linie, coloana];
        }

        /// <summary>
        /// Metoda ce primeste o lista de stari si identifica linia aferenta acesteia
        /// </summary>
        /// <param name="stariParinti"></param>
        /// <returns></returns>
        public int CalculLinieStari(String[] stariParinti)
        {

            // Baza este utilizata pentru calculul pozitiei din matrice
            int baza = 1;
            // Linia unde va fi plasata probabilitatea
            int linie = 0;

            // Pentru parinte de la dreapta la stanga
            for (int indexParinte = NodCentral.Parinti.Count - 1; indexParinte >= 0; indexParinte--)
            {
                Nod parinteCurent = NodCentral.Parinti[indexParinte];
                String stare = stariParinti[indexParinte];

                // Indexul elementului in starile parintelui
                int indexInStari = parinteCurent.Domeniu.IndexOf(stare);

                // Daca starea nu se afla in lista de stari a parintelui: Eroare
                if (indexInStari < 0)
                    throw new Exception("Stare \"" + stare + "\" inexistenta pentru nodul \"" + parinteCurent.Nume + "\"");

                // Update linie
                linie += baza * indexInStari;

                // Update baza
                baza = baza * parinteCurent.Domeniu.Count;
            }

            // Daca nu are parinti, va returna 0
            return linie;
        }


        /// <summary>
        /// Metoda care extrage starile care sunt aferente unei linii din matricea de probabilitati
        /// </summary>
        /// <param name="linie"> Un numar intreg care reprezinta linia de unde se doreste extragerea </param>
        /// <returns> O lista de stari care descriu linia </returns>
        public String[] StariLinie(int linie)
        {

            // Validare input
            if (linie >= _tabela.GetLength(0) || linie < 0)
                throw new Exception("Linia din tabel trebuie sa fie pozitiva si mai mica decat " + _tabela.Length);

            // Rezultatul calculului
            String[] rezultat = new String[NodCentral.Parinti.Count];

            // Se parcurge lista de parinti de la coada la cap
            for(int indexString = NodCentral.Parinti.Count - 1; indexString >= 0; indexString--)
            {
                // Extragerea parintelui
                Nod parinte = NodCentral.Parinti[indexString];

                // Setarea rezultatului
                rezultat[indexString] = parinte.Domeniu[linie % parinte.Domeniu.Count];

                // Eliminarea elemntului
                linie = linie / parinte.Domeniu.Count;
            }


            return rezultat;
        }

        /// <summary>
        /// Metoda de validare a tabelei de probabilitati
        /// </summary>
        /// <param name="value"> Tabela de evaluat </param>
        public void ValidareTabela()
        {
            // Verificare tabela probabilitati
            // 1) Numarul de linii
            int linii = 1;
            foreach (Nod parinte in NodCentral.Parinti)
                linii *= parinte.Domeniu.Count;

            if (Tabela.GetLength(0) != linii)
                throw new Exception("Numarul de linii din tabela de probabilitati nu corespunde cu numarul de linii necesare pentru reprezentarea starilor parintilor nodului.");

            // 2) Numarul de coloane
            if (_nod.Domeniu.Count != Tabela.GetLength(1))
                throw new Exception("Numarul de coloane din tabela nu corespunde cu numarul de stari ale nodului.");

            // 3) Suma probabilitatilor pe linie trebuie sa dea 1
            for (int i = 0; i < Tabela.GetLength(0); i++)
            {
                double sum = 0;
                for (int j = 0; j < Tabela.GetLength(1); j++)
                    sum += Tabela[i, j];

                if (Math.Abs(sum - 1) > 0.00000001)
                    throw new Exception("Suma probabilitatilor de pe o linie din tabel trebuie sa fie 1.");
            }
        }
    }
}
