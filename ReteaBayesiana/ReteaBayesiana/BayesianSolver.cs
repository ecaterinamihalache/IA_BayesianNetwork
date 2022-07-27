using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReteaBayesiana
{
    public class BayesianSolver
    {

        /// <summary>
        /// Metoda statica responsabila cu rezolvarea unui query
        /// </summary>
        /// <param name="retea"> Reteaua asupra careia executa query-ul </param>
        /// <param name="targetNod"> Nodul asupra caruia se realizeaza calcule </param>
        /// <returns> Lista cu probabilitatile asignate fiecarei stari ale nodului </returns>
        public static double[] SolutioneazaRetea(Retea retea, Nod targetNod)
        {
            // Rezultatul care trebuie intors la final
            double[] rezultat = new double[targetNod.Domeniu.Count];

            // Cazul in care nodul este deja observat, se cunoaste deja raspunsul
            if (targetNod.Observatie != null)
            {
                for (int indexStare = 0; indexStare < rezultat.Length; indexStare++)
                {
                    if (targetNod.Domeniu[indexStare].Equals(targetNod.Observatie))
                    {
                        rezultat[indexStare] = 1.0;
                    }
                    else
                    {
                        rezultat[indexStare] = 0.0;
                    }
                }
                return rezultat;
            }

            // Lista de noduri care mai trebuie vizitate
            List<Nod> vars = new List<Nod>();

            // Pentru fiecare stare, calculez probabilitatea starii respective
            for(int indexStare = 0; indexStare < rezultat.Length; indexStare++)
            {
                // Setez observatie pentru nodul respectiv
                targetNod.Observatie = targetNod.Domeniu[indexStare];

                // Reimprospatez vars
                vars.Clear();
                vars.AddRange(retea.Noduri);

                // Calculez lista
                rezultat[indexStare] = EnumerateAll(vars);
            }

            targetNod.Observatie = null; 

            // Normalizarea raspunsului
            Normalizare(rezultat);

            // Intoarcerea raspunsului
            return rezultat;
        }

        /// <summary>
        /// Metoda recursiva pentru calcului unei probabilitati din reteaua Bayesiana
        /// </summary>
        /// <param name="vars"> Lista cu nodurile ramase de procesat (sortata Topologic) </param>
        /// <returns> Probabilitatea variabilei </returns>
        private static double EnumerateAll(List<Nod> vars)
        {
            // Daca lista este goala, returneaza 1
            if (vars.Count == 0) return 1.0;

            // Extrage nodul care trebuie procesat
            Nod firstNode = vars[0];
            vars.RemoveAt(0);

            // Construieste lista de Observatii ale parintilor
            string[] observatiiParinti = firstNode.ListaObservatiiParinti();

            // Daca starea lui firstNode exte observata
            if (firstNode.Observatie != null)
            {
                // Generez o lista noua de elemente
                List<Nod> rest = new List<Nod>(vars);

                // Returneaza raspuns
                double result = firstNode.Tabela.GetProbabilitateStari(observatiiParinti, firstNode.Observatie) * EnumerateAll(rest);

                return result;
            }

            // Nodul curent nu contine o stare cunoscuta
            // Evaluam sumele cand setam nodul in toate starile
            double sumProbs = 0;

            // Pentru fiecare stare pe care o poate avea nodul curent
            for (int indexStare = 0; indexStare < firstNode.Domeniu.Count; indexStare++)
            {
                // Generez o lista noua de elemente
                List<Nod> rest = new List<Nod>(vars);

                // Setez nodul ca avand starea ca observatie
                firstNode.Observatie = firstNode.Domeniu[indexStare];

                // Update suma
                sumProbs += firstNode.Tabela.GetProbabilitateStari(observatiiParinti, firstNode.Observatie) *
                    EnumerateAll(rest);
            }

            // Reseteaza observatia nodului curent la null
            firstNode.Observatie = null;

            // Returneaza rezultatul calculului
            return sumProbs;
        }

        private static void Normalizare(double[] prob)
        {
            double alpha = 0;

            // Realizez suma
            foreach(double p in prob)
            {
                alpha += p;
            }

            // Inversez
            alpha = 1 / alpha;

            // Update elemente lista
            for(int i = 0; i<prob.Length; i++)
            {
                prob[i] *= alpha;
            }

        }
    }
}