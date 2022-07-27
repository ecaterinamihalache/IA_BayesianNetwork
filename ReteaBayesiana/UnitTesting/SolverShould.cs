using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReteaBayesiana;

namespace UnitTesting
{
    [TestClass]
    public class SolverShould
    {
        private Retea _retea;

        private void ReteaDefault()
        {
            List<string> domeniu = new List<string> { "Da", "Nu" };

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

            //Assert.IsTrue(_retea.IsValid());
        }

        private void ReteaSimpla()
        {
            List<string> domeniu = new List<string> { "T", "F" };
            Nod n1 = new Nod("N1", domeniu);
            Nod n2 = new Nod("N2", domeniu, new List<Nod> { n1 });

            n1.CreazaTabela(new double[,]
            {
                { 0.1, 0.9 }
            });

            n2.CreazaTabela(new double[,]
            {
                { 0.4, 0.6 },
                { 0.3, 0.7 }
            });

            _retea = new Retea(new List<Nod> { n1, n2 });

            //Assert.IsTrue(_retea.IsValid());
        }

        private void ReteaComplexa()
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

        private void TestRetea(string targetName, double[] expected)
        {
            // Rezultatul actual
            double[] actual = BayesianSolver.SolutioneazaRetea(_retea, _retea.GetElementRetea(targetName));

            // Sa fie aceasi dimensiune
            Assert.AreEqual(actual.Length, expected.Length);

            // Verificare rezultat
            for (int index = 0; index < expected.Length; index++)
            {
                Assert.AreEqual(expected[index], actual[index], 0.00001);
            }
        }

        [TestMethod]
        public void EvaluateObosealaWhenGripaAbcesAndAnorexieAreFalse()
        {
            ReteaDefault();

            // Setez observatii
            _retea.ObservatieNod("Gripa", "Nu");
            _retea.ObservatieNod("Abces", "Nu");
            _retea.ObservatieNod("Anorexie", "Nu");

            // Rezultatul asteptat
            double[] expected = new double[] { 0.21136, 0.78864 };

            // Testam reteaua
            TestRetea("Oboseala", expected);
        }

        [TestMethod]
        public void EvaluateFebraWhenGripaAndAbcesTrue()
        {
            ReteaDefault();

            // Setez observatii
            _retea.ObservatieNod("Gripa", "Da");
            _retea.ObservatieNod("Abces", "Da");

            // Rezultatul asteptat
            double[] expected = new double[] { 0.8, 0.2 };

            // Testam reteaua
            TestRetea("Febra", expected);
        }

        [TestMethod]
        public void NotChangeTheInitialObservations()
        {
            ReteaDefault();

            // Setez observatii
            _retea.ObservatieNod("Gripa", "Nu");
            _retea.ObservatieNod("Abces", "Nu");
            _retea.ObservatieNod("Anorexie", "Nu");

            BayesianSolver.SolutioneazaRetea(_retea, _retea.GetElementRetea("Febra"));

            // Verificam daca observatiile au ramas la fel
            Assert.AreEqual("Nu", _retea.GetElementRetea("Gripa").Observatie);
            Assert.AreEqual("Nu", _retea.GetElementRetea("Abces").Observatie);
            Assert.AreEqual("Nu", _retea.GetElementRetea("Anorexie").Observatie);
            Assert.AreEqual(null, _retea.GetElementRetea("Febra").Observatie);
            Assert.AreEqual(null, _retea.GetElementRetea("Oboseala").Observatie);
        }

        [TestMethod]
        public void EvaluateWhenEverythingIsUnknown()
        {
            ReteaDefault();

            // Rezultatul asteptat
            double[] expected;

            expected = new double[] { 0.1245, 0.8755 };

            // Testam reteaua
            TestRetea("Febra", expected);

            // Rezultatul asteptat
            expected = new double[] { 0.2498, 0.7502 };

            // Testam reteaua
            TestRetea("Oboseala", expected);

            // Rezultatul asteptat
            expected = new double[] { 0.1498, 0.8502 };

            // Testam reteaua
            TestRetea("Anorexie", expected);
        }



        [TestMethod]
        public void EvaluateAQueryOnAnObservedNode()
        {
            ReteaDefault();

            // Setez observatii
            _retea.ObservatieNod("Febra", "Nu");

            // Rezultatul asteptat
            double[] expected = new double[] { 0.0, 1.0 };

            // Testam reteaua
            TestRetea("Febra", expected);
        }


        [TestMethod]
        public void EvaluateASimpleQuery()
        {
            ReteaSimpla();

            // Setez observatie
            _retea.ObservatieNod("N1", "T");

            // Expected
            double[] expected = new double[] { 0.4, 0.6 };

            // Testez reteaua
            TestRetea("N2", expected);

        }

        [TestMethod]
        public void EvaluateATwoStepQueryWithAllStatesUnobserved()
        {
            ReteaSimpla();

            // Expected
            double[] expected = new double[] { 0.31, 0.69 };

            // Testez reteaua
            TestRetea("N2", expected);

        }

        [TestMethod]
        public void EvaluateASimpleQueryWithAllStatesUnobserved()
        {
            ReteaSimpla();

            // Expected
            double[] expected = new double[] { 0.1, 0.9 };

            // Testez reteaua
            TestRetea("N1", expected);
        }

        [TestMethod]
        public void EvaluateAComplexQuery()
        {
            ReteaComplexa();
            Assert.IsTrue(_retea.IsValid());
            double[] expected;

            // Observations
            _retea.ObservatieNod("S1", "B1");
            _retea.ObservatieNod("Directie", "Dreapta");

            // Expected
            expected = new double[] { 0.0, 0.5, 0.5, 0.0 };
            // Testez reteaua
            TestRetea("S3", expected);

            // Expected
            expected = new double[] { 0.0, 0.0, 0.7, 0.3 };
            // Testez reteaua
            TestRetea("S4", expected);


            // Observations
            _retea.ObservatieNod("S1", null);

            // Expected
            expected = new double[] { 0.03906, 0.14063, 0.32813, 0.49219 };
            // Testez reteaua
            TestRetea("S1", expected);

            // Expected
            expected = new double[] { 0.01563, 0.10938, 0.32813, 0.54688 };
            // Testez reteaua
            TestRetea("S2", expected);

            // Expected
            expected = new double[] { 0.0, 0.0625, 0.3125, 0.625 };
            // Testez reteaua
            TestRetea("S3", expected);

            // Expected
            expected = new double[] { 0.0, 0.0, 0.25, 0.75 };
            // Testez reteaua
            TestRetea("S4", expected);

        }

        [TestMethod]
        public void EvaluateAComplexQueryCaseNotWorkingOnInterface()
        {
            ReteaComplexa();
            Assert.IsTrue(_retea.IsValid());
            double[] expected;

            // Observations
            _retea.ObservatieNod("S1", "B1");

            // Expected
            expected = new double[] { 0.46875, 0.03906, 0.49219 };
            // Testez reteaua
            TestRetea("Directie", expected);
        }
    }
}
