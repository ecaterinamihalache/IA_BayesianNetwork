using ReteaBayesiana;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Interface
{
    
    public partial class Interface : Form
    {
        private const int _nodWitdh = 80;
        private const int _nodHeight = 50;
        private const int _nodPadding = 10;
        private Retea _retea = null;
        private Dictionary<Nod, TextBox> NoduriTextboxes { get; set; }

        /// <summary>
        /// Constructorul clasei
        /// </summary>
        public Interface()
        {
            InitializeComponent();
            
            reteaPictureBox.Image = new Bitmap(reteaPictureBox.Height, reteaPictureBox.Width);
            NoduriTextboxes = new Dictionary<Nod, TextBox>();
        }

        /// <summary>
        /// Metoda pentru butonul de Stergere
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStergere_Click(object sender, EventArgs e)
        {
            StergeRetea();
        }

        /// <summary>
        /// Metoda responsabila pentru stergerea retelei din interfata
        /// </summary>
        private void StergeRetea()
        {
            reteaPictureBox.Image = new Bitmap(reteaPictureBox.Height, reteaPictureBox.Width);
            foreach (KeyValuePair<Nod, TextBox> nod in NoduriTextboxes)
            {
                reteaPictureBox.Controls.Remove(nod.Value);
            }
            NoduriTextboxes.Clear();
        }

        /// <summary>
        /// Metida responsabila cu incarcarea unei retele si desenarea acesteia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonIncarcare_Click(object sender, EventArgs e)
        {
            
            openFileDialogIncarcare.InitialDirectory = System.Environment.CurrentDirectory;
            openFileDialogIncarcare.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialogIncarcare.FilterIndex = 1;
            openFileDialogIncarcare.RestoreDirectory = true;

            //foreach (Nod nod1 in _retea.Noduri)
            //{
            //    richTextBox1.Text += nod1.PozitieX + " " + nod1.PozitieY + "\r\n";
            //}
            StergeRetea();
            DialogResult openFileDialogResult = openFileDialogIncarcare.ShowDialog();
            string path = "";
            if (openFileDialogResult == DialogResult.OK)
            {
                path = openFileDialogIncarcare.FileName;

                // Extract the network
                _retea = ReteaFactory.CreazaDinFisier(path);

                // Normalize node positions
                ReteaFactory.NormalizeazPuncte(_retea, reteaPictureBox.Height/2, reteaPictureBox.Width/2);

                NoduriTextboxes = new Dictionary<Nod, TextBox>();
                reteaPictureBox.Image = new Bitmap(reteaPictureBox.Width, reteaPictureBox.Height);
                reteaPictureBox.Refresh();

                richTextBox1.Text = "";
                _retea.Noduri.ForEach(n =>
                {
                    richTextBox1.Text += n.Nume + ": " + n.PozitieX + ", " + n.PozitieY + "\r\n";
                    DeseneazaNod(n);
                    if (_retea.Parinti(n).Count != 0)
                    {
                        DeseneazaLegatura(n);
                    }
                });
            }
        }

        /// <summary>
        /// Metoda responsabila cu desenarea unui nod
        /// </summary>
        /// <param name="nod"></param>
        private void DeseneazaNod(Nod nod)
        {
            using (Graphics G = Graphics.FromImage(reteaPictureBox.Image))
            {
                Pen pen = new Pen(Color.Black, 1);
                TextBox textBox = new TextBox();

                textBox.Location = new Point(nod.PozitieX + _nodPadding, nod.PozitieY + _nodPadding);
                textBox.Text = nod.Nume + "\r\nnull";
                textBox.Multiline = true;
                textBox.Width = _nodWitdh - 2 * _nodPadding;
                textBox.Height = _nodHeight - 2 * _nodPadding;
                textBox.BackColor = Color.White;
                textBox.ForeColor = Color.Black;
                textBox.Visible = true;
                textBox.BorderStyle = BorderStyle.None;
                textBox.TextAlign = HorizontalAlignment.Center;
                textBox.Enabled = false;
                reteaPictureBox.Controls.Add(textBox);
                NoduriTextboxes.Add(nod, textBox);
                textBox.BringToFront();

                G.DrawEllipse(pen, new Rectangle(nod.PozitieX, nod.PozitieY, _nodWitdh, _nodHeight));
            }
            reteaPictureBox.Refresh();
        }

        
        /// <summary>
        /// Metoda responsabila cu desenarea legaturii dintre doua noduri
        /// </summary>
        /// <param name="nod"></param>
        private void DeseneazaLegatura(Nod nod)
        {
            using (Graphics G = Graphics.FromImage(reteaPictureBox.Image))
            {
                Pen pen = new Pen(Color.Black, 1);
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.ArrowAnchor;
                int startX=0, startY=0, finalX=0, finalY=0;

                //Pentru nodul primit ca parametru trebuie sa ii vad prima data lista de parinti
                List<Nod> parinti = _retea.Parinti(nod);

                //Dupa sa ii parcurg lista de parinti si pentru fiecare nod parinte calculez distanta intre nod si nodul primit ca parametru adica copil
                foreach (Nod parinte in parinti)
                {
                    if (GenerareDirectie(parinte, nod) == "DREAPTA")
                    {
                        startX = parinte.PozitieX;
                        startY = parinte.PozitieY + _nodHeight / 2;
                        finalX = nod.PozitieX + _nodWitdh;
                        finalY = nod.PozitieY + _nodHeight / 2;
                    }
                    else if (GenerareDirectie(parinte, nod) == "STANGA")
                    {
                        startX = parinte.PozitieX + _nodWitdh;
                        startY = parinte.PozitieY + _nodHeight / 2;
                        finalX = nod.PozitieX;
                        finalY = nod.PozitieY + _nodHeight / 2;
                    }
                    else if(GenerareDirectie(parinte, nod) == "SUS")
                    {
                        startX = parinte.PozitieX + _nodWitdh / 2;
                        startY = parinte.PozitieY + _nodHeight;
                        finalX = nod.PozitieX + _nodWitdh / 2;
                        finalY = nod.PozitieY;
                    }
                    else if(GenerareDirectie(parinte, nod) == "JOS")
                    {
                        startX = parinte.PozitieX + _nodWitdh / 2;
                        startY = parinte.PozitieY;
                        finalX = nod.PozitieX + _nodWitdh / 2;
                        finalY = nod.PozitieY + _nodHeight;
                    }
                    else
                    {
                        MessageBox.Show("Eroare!", " Nodurile sunt foarte apropiate!", MessageBoxButtons.OK);
                    }
                    G.DrawLine(pen, startX, startY, finalX, finalY);
                }
            }
            reteaPictureBox.Refresh();
        }

        /// <summary>
        /// Metoda ce imi genereaza pozitia nodului copil fata de nodul parinte
        /// </summary>
        /// <param name="parinte"></param>
        /// <param name="copil"></param>
        /// <returns></returns>
        private String GenerareDirectie(Nod parinte, Nod copil)
        {
            if (parinte.PozitieX < copil.PozitieX && parinte.PozitieY >= copil.PozitieY && parinte.PozitieY <= copil.PozitieY + Height)
                return "STANGA";
            else if (parinte.PozitieX > copil.PozitieX + _nodWitdh && parinte.PozitieY >= copil.PozitieY && parinte.PozitieY <= copil.PozitieY + Height)
                return "DREAPTA";
            else if (parinte.PozitieY < copil.PozitieY)
                return "SUS";
            else if (parinte.PozitieY > copil.PozitieY + _nodHeight)
                return "JOS";
            else
                return "NONE";
        }

        /// <summary>
        /// Metoda responsabila de functionarea radioButton-ului
        /// </summary>
        /// <param name="nod"></param>
        private void radioOptionsPanel_Paint(Nod nod)
        {
            if (radioButtonObservație.Checked == true)
            {
                selectareValoareDomeniu(nod);
            }
            else if(radioButtonQuery.Checked == true)
            {
                vizualizareProbabilitati(nod);
            }
        }

        /// <summary>
        /// Metoda responsabila cu selectarea valorii din domeniu pentru un nod
        /// </summary>
        /// <param name="nod"></param>
        private void selectareValoareDomeniu(Nod nod)
        {
            Observatie form = new Observatie();
            form.Text = "Observația nodului: " + nod.Nume;

            int pozitieX = 80, pozitieY = 50;
            TextBox noduriTextBox = NoduriTextboxes[nod];
            RadioButton radioButton;

            foreach (String valuareD in nod.Domeniu)
            {
                radioButton = new RadioButton();
                radioButton.Location = new Point(pozitieX, pozitieY);
                radioButton.Text = valuareD;

                radioButton.CheckedChanged += (sender, e) => { noduriTextBox.Text = nod.Nume + "\r\n(" + valuareD + ")"; nod.Observatie = valuareD; };
                form.Controls.Add(radioButton);
                pozitieY += 20;
            }

            radioButton = new RadioButton();
            radioButton.Location = new Point(pozitieX, pozitieY);
            radioButton.Text = "null";
            radioButton.Checked = true;
            radioButton.CheckedChanged += (sender, e) =>
            {
                noduriTextBox.Text = nod.Nume + "\r\n(null)";
                nod.Observatie = null;
            };
            form.Controls.Add(radioButton);
            form.Show();
        }

        /// <summary>
        /// Metoda responsabila de vizualizarea probabilitatilor unui nod
        /// </summary>
        /// <param name="nod"></param>
        private void vizualizareProbabilitati(Nod nod)
        {
            Observatie form = new Observatie();
            form.Text = "Probabilitatile nodului: " + nod.Nume;
            int pozitieX = 80, pozitieY = 50;

            double[] probabilitati = BayesianSolver.SolutioneazaRetea(_retea, nod);
            int i = 0;
            foreach (double probabilitate in probabilitati)
            {
                Label label = new Label();
                label.Size = new Size(150, 25);
                label.AutoSize = true;
                label.Location = new Point(pozitieX, pozitieY);
                label.Text = "P (" + nod.Nume + " = " + nod.Domeniu[i] + ") = " + probabilitate.ToString() + "\r\n";

                form.Controls.Add(label);
                pozitieY += 30;
                i++;
            }
            form.Show();
        }

        /// <summary>
        /// Metoda responsabila cu vizualizarea datelor dand click pe un nod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reteaPictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                MouseEventArgs mouseClick = (MouseEventArgs)e;
                Point location = mouseClick.Location;
                Nod nodApasat;

                foreach (Nod nod in _retea.Noduri)
                {
                    if (location.X >= nod.PozitieX + _nodPadding && location.X <= nod.PozitieX + _nodWitdh - _nodPadding && location.Y >= nod.PozitieY + _nodPadding && location.Y <= nod.PozitieY + _nodHeight - _nodPadding)
                    {
                        nodApasat = nod;
                        radioOptionsPanel_Paint(nodApasat);
                    }
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Nu ați încărcat nicio rețea!");
            }
            
        }
    }
}