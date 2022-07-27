
namespace Interface
{
    partial class Interface
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.radioButtonObservație = new System.Windows.Forms.RadioButton();
            this.radioButtonQuery = new System.Windows.Forms.RadioButton();
            this.radioOptionsPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonIncarcare = new System.Windows.Forms.Button();
            this.buttonStergere = new System.Windows.Forms.Button();
            this.reteaPictureBox = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.openFileDialogIncarcare = new System.Windows.Forms.OpenFileDialog();
            this.radioOptionsPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reteaPictureBox)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonObservație
            // 
            this.radioButtonObservație.AutoSize = true;
            this.radioButtonObservație.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.radioButtonObservație.Location = new System.Drawing.Point(12, 10);
            this.radioButtonObservație.Name = "radioButtonObservație";
            this.radioButtonObservație.Size = new System.Drawing.Size(118, 28);
            this.radioButtonObservație.TabIndex = 2;
            this.radioButtonObservație.TabStop = true;
            this.radioButtonObservație.Text = "Observație";
            this.radioButtonObservație.UseVisualStyleBackColor = true;
            // 
            // radioButtonQuery
            // 
            this.radioButtonQuery.AutoSize = true;
            this.radioButtonQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.radioButtonQuery.Location = new System.Drawing.Point(138, 10);
            this.radioButtonQuery.Name = "radioButtonQuery";
            this.radioButtonQuery.Size = new System.Drawing.Size(80, 28);
            this.radioButtonQuery.TabIndex = 3;
            this.radioButtonQuery.TabStop = true;
            this.radioButtonQuery.Text = "Query";
            this.radioButtonQuery.UseVisualStyleBackColor = true;
            // 
            // radioOptionsPanel
            // 
            this.radioOptionsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.radioOptionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.radioOptionsPanel.Controls.Add(this.radioButtonObservație);
            this.radioOptionsPanel.Controls.Add(this.radioButtonQuery);
            this.radioOptionsPanel.Location = new System.Drawing.Point(16, 15);
            this.radioOptionsPanel.Name = "radioOptionsPanel";
            this.radioOptionsPanel.Size = new System.Drawing.Size(239, 45);
            this.radioOptionsPanel.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.buttonIncarcare);
            this.panel1.Controls.Add(this.buttonStergere);
            this.panel1.Location = new System.Drawing.Point(283, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(229, 45);
            this.panel1.TabIndex = 6;
            // 
            // buttonIncarcare
            // 
            this.buttonIncarcare.BackColor = System.Drawing.SystemColors.ControlLight;
            this.buttonIncarcare.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.buttonIncarcare.Location = new System.Drawing.Point(112, 7);
            this.buttonIncarcare.Name = "buttonIncarcare";
            this.buttonIncarcare.Size = new System.Drawing.Size(98, 31);
            this.buttonIncarcare.TabIndex = 6;
            this.buttonIncarcare.Text = "Încărcare";
            this.buttonIncarcare.UseVisualStyleBackColor = false;
            this.buttonIncarcare.Click += new System.EventHandler(this.buttonIncarcare_Click);
            // 
            // buttonStergere
            // 
            this.buttonStergere.BackColor = System.Drawing.SystemColors.ControlLight;
            this.buttonStergere.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.buttonStergere.Location = new System.Drawing.Point(14, 7);
            this.buttonStergere.Name = "buttonStergere";
            this.buttonStergere.Size = new System.Drawing.Size(78, 31);
            this.buttonStergere.TabIndex = 5;
            this.buttonStergere.Text = "Ștergere";
            this.buttonStergere.UseVisualStyleBackColor = false;
            this.buttonStergere.Click += new System.EventHandler(this.buttonStergere_Click);
            // 
            // reteaPictureBox
            // 
            this.reteaPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reteaPictureBox.BackColor = System.Drawing.Color.White;
            this.reteaPictureBox.Location = new System.Drawing.Point(37, 123);
            this.reteaPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.reteaPictureBox.Name = "reteaPictureBox";
            this.reteaPictureBox.Size = new System.Drawing.Size(996, 542);
            this.reteaPictureBox.TabIndex = 7;
            this.reteaPictureBox.TabStop = false;
            this.reteaPictureBox.Click += new System.EventHandler(this.reteaPictureBox_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.radioOptionsPanel);
            this.panel2.Location = new System.Drawing.Point(37, 23);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(527, 76);
            this.panel2.TabIndex = 8;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(583, 23);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(450, 76);
            this.richTextBox1.TabIndex = 10;
            this.richTextBox1.Text = "";
            // 
            // openFileDialogIncarcare
            // 
            this.openFileDialogIncarcare.FileName = "openFileDialogIncarcare";
            this.openFileDialogIncarcare.Title = "Incarcare Retea";
            // 
            // Interface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 694);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.reteaPictureBox);
            this.Name = "Interface";
            this.Text = "ReteaBayesiana";
            this.radioOptionsPanel.ResumeLayout(false);
            this.radioOptionsPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.reteaPictureBox)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonObservație;
        private System.Windows.Forms.RadioButton radioButtonQuery;
        private System.Windows.Forms.Panel radioOptionsPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonIncarcare;
        private System.Windows.Forms.Button buttonStergere;
        private System.Windows.Forms.PictureBox reteaPictureBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialogIncarcare;
    }
}

