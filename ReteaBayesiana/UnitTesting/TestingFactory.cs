using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReteaBayesiana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    [TestClass]
    public class TestingFactory
    {
        [TestMethod]
        public void TestAdaugareNod()
        {
            Retea retea = ReteaFactory.CreazaDinFisier(@"D:\Facultate\An4\IA Proiect\Proiect\Bayese_example.xml");
            Nod x = retea.GetElementRetea("Node 1");
            Assert.AreEqual("Node 1", x.Nume);
        }
        [TestMethod]
        public void TestAdaugareParinti()
        {
            Retea retea = ReteaFactory.CreazaDinFisier(@"D:\Facultate\An4\IA Proiect\Proiect\Bayese_example.xml");
            Assert.AreEqual(2, retea.GetElementRetea("Node 4").Parinti.Count);
        }

        [TestMethod]
        public void TestTabelaProbabilitati()
        {
            Retea retea = ReteaFactory.CreazaDinFisier(@"D:\Facultate\An4\IA Proiect\Proiect\Bayese_example.xml");
            Assert.AreEqual(0.5, retea.GetElementRetea("Node 2").Tabela.Tabela[0,0]);
        }

        [TestMethod]
        public void TestDomeniu()
        {
            Retea retea = ReteaFactory.CreazaDinFisier(@"D:\Facultate\An4\IA Proiect\Proiect\Bayese_example.xml");
            Assert.AreEqual(2, retea.Noduri[3].Domeniu.Count);
        }

        [TestMethod]
        public void TestPozitii()
        {
            Retea retea = ReteaFactory.CreazaDinFisier(@"D:\Facultate\An4\IA Proiect\Proiect\Bayese_example.xml");
            Assert.AreEqual(7165, retea.Noduri[3].PozitieX);
        }
        [TestMethod]
        public void TestIsValid()
        {
            Retea retea = ReteaFactory.CreazaDinFisier(@"D:\Facultate\An4\IA Proiect\Proiect\Bayese_example.xml");
            Assert.IsTrue(retea.IsValid());
        }

        

    }
}
       
