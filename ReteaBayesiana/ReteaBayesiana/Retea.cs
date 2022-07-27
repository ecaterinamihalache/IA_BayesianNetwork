using System;
using System.Collections.Generic;

namespace ReteaBayesiana
{
    public class Retea
    {
        public List<Nod> Noduri { set; get; }

        public Retea(List<Nod> noduri)
        {
            Noduri = noduri;
            SortareTopologicaNoduri();
        }

        /// <summary>
        /// Functie utilizata pentru a schimba starea de observare a unui nod din retea.
        /// </summary>
        /// <param name="numeNod"> Numele nodului </param>
        /// <param name="stare"> Starea in care se schimba nodul </param>
        public void ObservatieNod(String numeNod, String stare)
        {
            Nod nod = GetElementRetea(numeNod);

            if (nod == null)
                throw new Exception("Nu exista nod cu acest nume in retea.");

            nod.Observatie = stare;
        }


        /// <summary>
        /// Metoda care returneaza un element de retea pe baza numelui acestuia
        /// </summary>
        /// <param name="numeNod"> Numele elementului de retea </param>
        /// <returns> Elementul de retea cu numele dat sau null in caz contrar </returns>
        public Nod GetElementRetea(String numeNod)
        {
            foreach (Nod element in Noduri)
            {
                if (element.Nume.Equals(numeNod))
                {
                    return element;
                }
            }
            return null;
        }

        /// <summary>
        /// Metoda care intoarce parintii unui nod
        /// </summary>
        /// <param name="nodTarget"> Nodul caruia i se doreste aflarea parintilor </param>
        /// <returns> O lista de noduri care reprezinta parintii nodului target </returns>
        public List<Nod> Parinti(Nod nodTarget)
        {
            return nodTarget.Parinti;
        }

        /// <summary>
        /// Pentru un nod dat, returneaza o lista cu fii acestuia
        /// </summary>
        /// <param name="numeNod"> Numele nodului </param>
        /// <returns> O lista cu numele fiilor acestuia </returns>
        public List<Nod> Fii(Nod nodTarget)
        {
            List<Nod> fii = new List<Nod>();

            foreach (Nod nod in Noduri)
            {
                if (nod.Parinti.Contains(nodTarget))
                    fii.Add(nod);
            }

            return fii;
        }

        /// <summary>
        /// Verifica daca reteaua este una valida:
        /// 1) Nu are tabele de probabilitati invalide
        /// 2) Nu prezinta cicluri
        /// </summary>
        /// <returns> O variabila booleana care atesta validitatea retelei sau nu. </returns>
        public bool IsValid()
        {

            // 1) Nu contine tabele de probabilitati invalide
            foreach(Nod element in Noduri)
            {
                try
                {
                    element.Tabela.ValidareTabela();
                }
                catch
                {
                    return false;
                }
            }

            // 2) Nu contine cicluri
            try
            {
                // Se sorteaza
                SortareTopologicaNoduri();
            }
            catch
            {
                // Contine un ciclu
                return false;
            }

          
            return true;
        }

        /// <summary>
        /// Metoda recursiva utilizata pentru identificarea unui ciclu
        /// daca un element care nu e radacina (Nu are 0 parinti) 
        /// este vizitat de mai multe ori
        /// decat are parinti, atunci contine cicluri
        /// </summary>
        /// <param name="first"> Nodul curent in care se face evaluarea </param>
        /// <param name="vizite"> Dictionarul cu elemente vizitate </param>
        /// <returns> O variabila booleana care atesta sau nu existenta ciclului </returns>
        private bool EsteCiclu(Nod first, Dictionary<String, int> vizite)
        {
            // Daca este nod de start, nu interogam numarul de vizite
            // Altfel
            if (first.Parinti.Count != 0)
            {
                // Detectez un ciclu
                if (vizite[first.Nume] + 1 > first.Parinti.Count)
                    return false;
            }

            // Adaug la numarul de vizite pentru nodul respectiv
            vizite[first.Nume]++;

            // Pentru fiecare copil al sau, verific daca e valid
            foreach (Nod copil in Fii(first))
            {
                // Daca pentru un copil s-a detectat ciclu, atunci returnam false
                if (EsteCiclu(copil, vizite) == false)
                    return false;
            }

            // Returnez ca sectiunea este valida
            return true;
        }

        public void SortareTopologicaNoduri()
        {
            // The resulting list
            List<Nod> sorted = new List<Nod>();

            // The nod - numar parinti asociere
            List<Tuple<Nod, int>> pairList = new List<Tuple<Nod, int>>();
            foreach(Nod currentNod in Noduri)
            {
                pairList.Add(new Tuple<Nod, int>(currentNod, currentNod.Parinti.Count));
            }

            // Face sortarea dictionarului dupa numarul de parinti
            int NodParintiComparator(Tuple<Nod, int> nod1, Tuple<Nod, int> nod2) =>
                nod1.Item2 < nod2.Item2 ? -1 : nod1.Item2 == nod2.Item2 ? 0 : 1;

            // Numarul anterior de elemente in lista
            int lastCount = pairList.Count;
            while (pairList.Count != 0)
            {
                // Se sorteaza lista de noduri dupa numarul de parinti
                pairList.Sort(NodParintiComparator);

                // Se extrag elementele care au 0 parinti
                while(pairList.Count != 0 && pairList[0].Item2 == 0)
                {

                    // Adaugam nod in lista
                    Nod adaugat = pairList[0].Item1;
                    sorted.Add(adaugat);

                    // Eliminam nodul din lista de perechi
                    pairList.RemoveAt(0);

                    // Scadem din parintii nodurilor ramase daca adaugat era parintele acestuia
                    for(int i = 0; i < pairList.Count; i++)
                    {
                        // Alegem nodul curent
                        Tuple<Nod, int> other = pairList[i];

                        // Daca contine, scadem din numarul parintilor
                        if (other.Item1.Parinti.Contains(adaugat))
                        {
                            pairList[i] = new Tuple<Nod, int>(other.Item1, other.Item2 - 1);
                        }
                    }
                }

                // Daca in acest moment, numarul de elemente a ramas neschimbat, atunci este un ciclu
                if (lastCount == pairList.Count)
                    throw new Exception("Reteaua contine ciclu!");

                lastCount = pairList.Count;
            }

            // La final, in sorted se va regasi lista de noduri sortata
            Noduri = sorted;
        }
    }
}
