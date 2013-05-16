using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SmartDeviceProject3
{
      partial class FenetreMenu : Form
    {
          internal static String s_NumRef;
          internal static String s_Console;
        private Label txtTitre;  
        private Button verification;
        private Button ajoutArticle;
        private Button suppresionArticle;
        private Button quitter;

        private String numRef;

        public FenetreMenu(String numRef)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.numRef = numRef;

            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.KeyPreview = false;
            this.Name = "Menu";
            this.ResumeLayout(false);

            //Texte "gestion des commandes"
            txtTitre = new Label();
            txtTitre.Text = "GESTION DES COMMANDES";
            txtTitre.Location = new Point(15, 15);
            txtTitre.Size = new System.Drawing.Size(220, 20);
            txtTitre.Font = new Font( "Verdana",11,System.Drawing.FontStyle.Bold);
           // txtTitre.BackColor = Color.FromArgb(241, 243, 175);

            // Le bouton "VerificationCDE"
            verification = new Button();
            verification.Text = "Vérification";
            verification.Location = new Point(50, 55);
            verification.Click += new System.EventHandler(Verification_Click);
            verification.Size=new System.Drawing.Size(145, 40);
            this.verification.BackColor = Color.FromArgb(150, 218, 177);

            // Le bouton "AjoutArticle"
            ajoutArticle = new Button();
            ajoutArticle.Text = "Saisie";
            ajoutArticle.Location = new Point(50, 115);
            ajoutArticle.Click += new System.EventHandler(AjoutArticle_Click);
            ajoutArticle.Size = new System.Drawing.Size(145, 40);
            this.ajoutArticle.BackColor = Color.FromArgb(150, 218, 177);

            // Le bouton "SuppresionArticle"
            suppresionArticle = new Button();
            suppresionArticle.Text = "Suppression";
            suppresionArticle.Location = new Point(50, 175);
            suppresionArticle.Click += new System.EventHandler(SuppresionArticle_Click);
            suppresionArticle.Size = new System.Drawing.Size(145, 40);
            this.suppresionArticle.BackColor = Color.FromArgb(150, 218, 177);

            //Le bouton quitter
            quitter = new Button();
            quitter.Text = "Quitter";
            quitter.Location = new Point(50, 235);
            quitter.Click += new System.EventHandler(Quitter_Click);
            quitter.Size = new System.Drawing.Size(145, 40);
            this.quitter.BackColor = Color.FromArgb(150, 218, 177);

            // Ajout des composants à la fenêtre 
            Controls.Add(txtTitre);
            Controls.Add(verification);
            Controls.Add(suppresionArticle);
            Controls.Add(ajoutArticle);
            Controls.Add(quitter);
            
        }

        // Gestionnaire d'événements
        private void Verification_Click(object sender, EventArgs e)
        {
            FenetreCommande fen = new FenetreCommande();
            fen.Show();
        }

        private void AjoutArticle_Click(object sender, EventArgs e)
        {
            FenetreArticle fen = new FenetreArticle(1);
            fen.Show();
        }
        private void SuppresionArticle_Click(object sender, EventArgs e)
        {
            FenetreArticle fen = new FenetreArticle(2);
            fen.Show();
        }

        private void Quitter_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
    }
}