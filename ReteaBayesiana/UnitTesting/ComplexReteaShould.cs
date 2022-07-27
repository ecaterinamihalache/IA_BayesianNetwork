using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReteaBayesiana;

namespace UnitTesting
{
    [TestClass]
    public class ComplexReteaShould
    {
        private Retea _retea;

        [TestInitialize]
        public void TestInitialize()
        {
            List<string> domeniuSectiune = new List<string> { "B1", "B2", "B3", "B4" };
            List<string> domeniuDirectie = new List<string> { "Inainte", "Dreapta", "Stanga" };

            // Se face initializarea retelei
            // Nod:
            Nod s1 = new Nod("S1", domeniuSectiune);
            s1.CreazaTabela(
                new double[,]
                {
                    { 0.25, 0.25, 0.25, 0.25 }
                }
            );

            // Nod:
            Nod s2 = new Nod("S2", domeniuSectiune, new List<Nod> { s1 });
            s2.CreazaTabela(new double[,]
            {
                { 0.75, 0.25, 0.0, 0.0 },
                { 0.25, 0.50, 0.25, 0.0 },
                { 0.0, 0.25, 0.5, 0.25 },
                { 0.0, 0.0, 0.25, 0.75 },
            });

            // Nod:
            Nod s3 = new Nod("S3", domeniuSectiune, new List<Nod> { s2 });
            s3.CreazaTabela(new double[,]
            {
                { 0.75, 0.25, 0.0, 0.0 },
                { 0.25, 0.50, 0.25, 0.0 },
                { 0.0, 0.25, 0.5, 0.25 },
                { 0.0, 0.0, 0.25, 0.75 },
            });

            // Nod:
            Nod s4 = new Nod("S4", domeniuSectiune, new List<Nod> { s3 });
            s4.CreazaTabela(new double[,]
            {
                { 0.75, 0.25, 0.0, 0.0 },
                { 0.25, 0.50, 0.25, 0.0 },
                { 0.0, 0.25, 0.5, 0.25 },
                { 0.0, 0.0, 0.25, 0.75 },
            });

            // Nod:
            Nod directie = new Nod("Directie", domeniuDirectie, new List<Nod> { s4 });
            directie.CreazaTabela(new double[,]
            {
                { 0.25, 0.0, 0.75 },
                { 0.75, 0.0, 0.25 },
                { 0.75, 0.25, 0.0 },
                { 0.25, 0.75, 0.0 }
            });


            // Aici pot aparea probleme
            _retea = new Retea(new List<Nod> { s1, s2, s3, s4, directie });

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
            _retea.GetElementRetea("S1").Tabela.SetProbabilitateStari(new string[]{ }, "B1", 0);
            foreach (Nod nod in _retea.Noduri)
            {
                nod.Tabela.ValidareTabela();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void NotHaveNegativeProbabilities()
        {
            _retea.GetElementRetea("S1").Tabela.SetProbabilitateStari(new string[] { }, "B1", -1);
            foreach (Nod nod in _retea.Noduri)
            {
                nod.Tabela.ValidareTabela();
            }
        }

        [TestMethod]
        public void PerformTopologicalSorting()
        {
            String[] expected = new string[] { "S1", "S2", "S3", "S4", "Directie"};

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
            double prtobabilitateInvalida = _retea.GetElementRetea("Directie").Tabela.GetProbabilitate(500, 1);
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
            Nod s1 = _retea.GetElementRetea("S1");
            s1.Observatie = "Nu exista aceasta observatie";
        }

        [TestMethod]
        public void SetTheValidObservation()
        {
            Nod gripa = _retea.GetElementRetea("S1");
            gripa.Observatie = "B1";
        }
    }
}
