using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReteaBayesiana;

namespace UnitTesting
{
    [TestClass]
    public class DefaultReteaShould
    {
        private Retea _retea;

        [TestInitialize]
        public void TestInitialize()
        {
            List<String> domeniu = new List<string> { "Da", "Nu" };

            // Se face initializarea retelei
            // Nod gripa:
            Nod gripa = new Nod("Gripa", domeniu);
            gripa.CreazaTabela(
                new double[,]
                {
                    { 0.1, 0.9 }
                }
            );

            // Nod Abces:
            Nod abces = new Nod("Abces", domeniu);
            abces.CreazaTabela(new double[,]
            {
                { 0.05, 0.95 }
            });

            // Nod Febra
            Nod febra = new Nod("Febra", domeniu, new List<Nod> { gripa, abces });
            febra.CreazaTabela(new double[,] {
                { 0.8, 0.2 },
                { 0.7, 0.3 },
                { 0.25, 0.75},
                { 0.05, 0.95}
            });

            // Nod Oboseala
            Nod oboseala = new Nod("Oboseala", domeniu, new List<Nod> { febra });
            oboseala.CreazaTabela(new double[,] {
                { 0.6, 0.4 },
                { 0.2, 0.8 }
            });

            // Nod Anorexie
            Nod anorexie = new Nod("Anorexie", domeniu, new List<Nod> { febra });
            anorexie.CreazaTabela(new double[,] {
                { 0.5, 0.5 },
                { 0.1, 0.9 }
            });

            // Aici pot aparea probleme
            _retea = new Retea(new List<Nod> { gripa, febra, abces, oboseala, anorexie });

        }


        [TestMethod]
        public void HaveValidProbabilityTables()
        {
            foreach(Nod nod in _retea.Noduri)
            {
                nod.Tabela.ValidareTabela();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void NotHaveProbabilitiesThatNotSumTo1()
        {
            _retea.GetElementRetea("Gripa").Tabela.SetProbabilitateStari(new string[]{ }, "Da", 0);
            foreach (Nod nod in _retea.Noduri)
            {
                nod.Tabela.ValidareTabela();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void NotHaveNegativeProbabilities()
        {
            _retea.GetElementRetea("Gripa").Tabela.SetProbabilitateStari(new string[] { }, "Da", -1);
            foreach (Nod nod in _retea.Noduri)
            {
                nod.Tabela.ValidareTabela();
            }
        }

        [TestMethod]
        public void PerformTopologicalSorting()
        {
            String[] expected = new string[] { "Gripa", "Abces", "Febra", "Oboseala", "Anorexie"};

            _retea.SortareTopologicaNoduri();

            for(int index = 0; index < _retea.Noduri.Count; index++)
            {
                Assert.AreEqual(_retea.Noduri[index].Nume, expected[index]);
            }
        }

        [TestMethod]
        public void BeValid()
        {
            Assert.AreEqual(_retea.IsValid(), true);
        }

        [TestMethod]
        public void DetectCicles()
        {
            // Modificam reteaua
            Nod febra = _retea.GetElementRetea("Febra");
            Nod oboseala = _retea.GetElementRetea("Oboseala");

            febra.Parinti.Add(oboseala);
            febra.CreazaTabela( new double[,]
            {
                { 0.8, 0.2 },
                { 0.7, 0.3 },
                { 0.25, 0.75 },
                { 0.05, 0.95 },
                { 0.8, 0.2 },
                { 0.7, 0.3 },
                { 0.25, 0.75 },
                { 0.05, 0.95 }
            });

            Assert.AreNotEqual(_retea.IsValid(), true);
        }


        [TestMethod]
        public void ExtractTheStatesOfALine()
        {
            foreach (Nod nod in _retea.Noduri)
            {
                List<string> expectedStates = new List<string>();

                for (int linie = 0; linie < nod.Tabela.Tabela.GetLength(0); linie++)
                {
                    expectedStates.Clear();
                    for (int index = 0; index < nod.Parinti.Count; index++)
                    {
                        Nod parinte = nod.Parinti[index];
                        int shiftator = (1 << (nod.Parinti.Count - index - 1));
                        expectedStates.Add(parinte.Domeniu[(linie & shiftator) != 0 ? 1 : 0]);
                    }

                    Assert.AreEqual(expectedStates.Count, nod.Parinti.Count, nod.Nume + ": Nu are numarul de elemente egale.");

                    string[] stari = nod.Tabela.StariLinie(linie);
                    Assert.AreEqual(expectedStates.Count, stari.Length, nod.Nume + ": Nu are numarul de stari egale cu cele asteptate.");
                    for (int index = 0; index < stari.Length; index++)
                        Assert.AreEqual(expectedStates[index], stari[index]);
                }
            }
        }


        [TestMethod]
        public void GetTheProbabilityGivenAlistOfTheStates()
        {
            // Modificam reteaua
            foreach (Nod nod in _retea.Noduri)
            {
                double[,] tabela = nod.Tabela.Tabela;

                for (int i = 0; i < tabela.GetLength(0); i++)
                {
                    for (int j = 0; j < tabela.GetLength(1); j++)
                    {
                        Assert.AreEqual(tabela[i, j], nod.Tabela.GetProbabilitateStari(nod.Tabela.StariLinie(i), nod.Domeniu[j]));
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void ThrowExceptionWhenGettingAProbabilityFromAnInvalidLine()
        {
            double prtobabilitateInvalida = _retea.GetElementRetea("Gripa").Tabela.GetProbabilitate(500, 1);
        }

        [TestMethod]
        public void SetTheProbabilitiesUsingTheStates()
        {
            foreach(Nod nod in _retea.Noduri)
            {
                // pentru fiecare linie
                for(int linie = 0; linie < nod.Tabela.Tabela.GetLength(0); linie++)
                {
                    // Preiau lista de elemente de pe linie si o fac reverse
                    double[] expected = new double[nod.Tabela.Tabela.GetLength(1)];
                    for(int col = 0; col < nod.Tabela.Tabela.GetLength(1); col++)
                    {
                        expected[nod.Tabela.Tabela.GetLength(1) - col - 1] = nod.Tabela.Tabela[linie, col];
                    }

                    for (int col = 0; col < nod.Tabela.Tabela.GetLength(1); col++) {
                        // Schimb elementele de pe linie folosind functia
                        nod.Tabela.SetProbabilitateStari(nod.Tabela.StariLinie(linie), nod.Domeniu[col], expected[col]);
                    }

                    // Preiau setul actual de date
                    // Compar setul actual cu cel expected
                    double[] actual = new double[nod.Tabela.Tabela.GetLength(1)];
                    for (int col = 0; col < nod.Tabela.Tabela.GetLength(1); col++)
                    {
                        actual[col] = nod.Tabela.Tabela[linie, col];
                        Assert.AreEqual(expected[col], actual[col]);
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ThrowExceptionThenMakeAnObservationOnAnInvalidState()
        {
            Nod gripa = _retea.GetElementRetea("Gripa");
            gripa.Observatie = "Nu exista aceasta observatie";
        }

        [TestMethod]
        public void SetTheValidObservation()
        {
            Nod gripa = _retea.GetElementRetea("Gripa");
            gripa.Observatie = "Da";
        }
    }
}
