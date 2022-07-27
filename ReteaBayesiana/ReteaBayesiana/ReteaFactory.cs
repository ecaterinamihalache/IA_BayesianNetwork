using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ReteaBayesiana
{
    public class ReteaFactory
    {
        public static Retea CreazaDinFisier(String path)
        {
            List<Nod> noduri = new List<Nod>();
            //List<String> domeniu = new List<string>();

            XmlDocument document = new XmlDocument();
            
            try
            {
                document.Load(path);
            }
            catch (Exception ex)
            {
                throw new Exception("Cale incorecta,fisierul xml nu este gasit");
            }

            XmlNodeList xmlNodeList = document.SelectNodes("NETWORK/VARIABLE");
            XmlNodeList xmlDefinitionList = document.SelectNodes("NETWORK/DEFINITION");

            noduri = PopuleazaNoduri(xmlNodeList);
            
            Retea retea = new Retea(noduri);
            
            AtaseazaParinti(xmlDefinitionList, retea);
            //throw new Exception("Inainte de load");
            CreazaTabela(xmlDefinitionList, retea);
            // throw new Exception("Inainte de loasvdasdvasd");
            return retea;
        }
        /// <summary>
        /// Metodă ce populează rețeaua cu noduri. Totodată nodurile respective conțin și o poziție relativă pe interfață
        /// </summary>
        /// <param name="xmlNodeList"> lista cu elemente extrase din xml</param>
        /// <returns> lista noduri</returns>
        private static List<Nod> PopuleazaNoduri(XmlNodeList xmlNodeList)
        {
            List<Nod> noduri = new List<Nod>();
           
            // parcurgem lista de noduri din xml
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                List<string> domeniu = new List<string>();

                // lista noduri cu starile
                XmlNodeList xmlStari = xmlNode.SelectNodes("OUTCOME");

                //pentru fiecare nod se adauga starea in domeniu
                foreach (XmlNode xmlNode1 in xmlStari)
                {
                    string stare = xmlNode1.InnerText;
                    domeniu.Add(stare);
                }

                // un singur nod in xml ce contine pozitiile
                XmlNode xmlPozitii = xmlNode.SelectSingleNode("PROPERTY");

                //regex ce ajuta la extragerea pozitiilor
                string pattern = @"[0-9]+\.[0-9]+";
                Regex rg = new Regex(pattern);
                MatchCollection matches = rg.Matches(xmlPozitii.InnerText);

                // convertirea pozitiilor
                double pozx = Convert.ToDouble(matches[0].Value);
                double pozy = Convert.ToDouble(matches[1].Value);
                Nod nod = new Nod(xmlNode["NAME"].InnerText, domeniu,(int)pozx,(int)pozy);

                //adaugarea nod in lista
                noduri.Add(nod);
            }
            return noduri;
        }

        private static void AtaseazaParinti(XmlNodeList xmlNodeList, Retea retea)
        {

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                List<Nod> noduriParinti = new List<Nod>();
                XmlNode firstChild = xmlNode.FirstChild;
                Nod x = retea.GetElementRetea(firstChild.InnerText);
                XmlNodeList noduriParintiXml = xmlNode.SelectNodes("GIVEN");
                //throw new Exception("numar given= " + noduriParintiXml.Count);
                foreach (XmlNode parinte in noduriParintiXml)
                {
                    Nod y = retea.GetElementRetea(parinte.InnerText);
                    noduriParinti.Add(y);
                }
                x.Parinti = noduriParinti;
                //throw new Exception("nume="+x.Nume+ " numar given= " + noduriParintiXml.Count);
            }
        }

        private static void CreazaTabela(XmlNodeList xmlNodeList, Retea retea)
        {
            foreach (XmlNode xmlNode in xmlNodeList)
            {
               
                XmlNode firstChild = xmlNode.FirstChild;
                Nod x = retea.GetElementRetea(firstChild.InnerText);
                XmlNode probabilitatiXml = xmlNode.LastChild;
                string[] valori = probabilitatiXml.InnerText.Split(' ');
                var linii = valori.Length / x.Domeniu.Count;
                var coloane = x.Domeniu.Count;
                double[,] tabela = new double[linii, coloane];
                for (int i = 0; i < linii; ++i)
                {
                    for (int j = 0; j < coloane; ++j)
                    {
                        tabela[i, j] = Convert.ToDouble(valori[i * coloane + j]);
                    }
                }
                x.CreazaTabela(tabela);
                //throw new Exception("Am intrat in for");
            }
        }

        public static void NormalizeazPuncte(Retea retea, int maxHeight, int maxWidth)
        {
            const int oo = 999999999;

            // Extract minimum and maximum positions
            int xmin = oo, xmax = -oo, ymin = oo, ymax = -oo;
            foreach (Nod nod in retea.Noduri)
            {
                xmin = Math.Min(xmin, nod.PozitieX);
                ymin = Math.Min(ymin, nod.PozitieY);
                ymax = Math.Max(ymax, nod.PozitieY);
                xmax = Math.Max(xmax, nod.PozitieX);
            }

            // Translate to left of image with margin
            int margin = 10;
            foreach (Nod nod in retea.Noduri)
            {
                nod.PozitieX = nod.PozitieX - xmin + margin; 
                nod.PozitieY = nod.PozitieY - ymin + margin; 
            }

            // Recalibrate max values
            xmax = xmax - xmin + margin;
            ymax = ymax - ymin + margin;

            // Apply 3 simple rule
            double pozxReal, pozyReal;
            foreach (Nod nod in retea.Noduri)
            {
                pozxReal = (double)(maxWidth * nod.PozitieX) / (double)(xmax);
                pozyReal = (double)(maxHeight * nod.PozitieY) / (double)(ymax);
                nod.PozitieX = (int)pozxReal;
                nod.PozitieY = (int)pozyReal;
            }
        }
        
    }
}
