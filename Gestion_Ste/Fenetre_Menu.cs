using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Gestion_Ste
{
    partial class Fenetre_Menu : Form
    {
        private Label txtTitre;
        private Button verificationSTE;
        private Button ajoutArticle;
        private Button supprimeArticle;
        private Button quitter;

        public Fenetre_Menu()
        {
            this.BackColor = Color.FromArgb(255, 186, 117);


            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.KeyPreview = false;
            this.Name = "Menu";
            this.ResumeLayout(false);

            //Texte "gestion des commandes"
            txtTitre = new Label();
            txtTitre.Text = "GESTION DES RETOUR STE";
            txtTitre.Location = new Point(15, 15);
            txtTitre.Size = new System.Drawing.Size(220, 20);
            txtTitre.Font = new Font("Verdana", 11, System.Drawing.FontStyle.Bold);
            // txtTitre.BackColor = Color.FromArgb(241, 243, 175);

            // Le bouton "VerificationCDE"
            verificationSTE = new Button();
            verificationSTE.Text = "Verification sté";
            verificationSTE.Location = new Point(50, 55);
            verificationSTE.Click += new System.EventHandler(VerificationCDE_Click);
            verificationSTE.Size = new System.Drawing.Size(145, 40);

            // Le bouton "AjoutArticle"
            ajoutArticle = new Button();
            ajoutArticle.Text = "Saisie d'article";
            ajoutArticle.Location = new Point(50, 115);
            ajoutArticle.Click += new System.EventHandler(AjoutArticle_Click);
            ajoutArticle.Size = new System.Drawing.Size(145, 40);

            // Le bouton "AjoutArticle"
            supprimeArticle = new Button();
            supprimeArticle.Text = "Suppression d'article";
            supprimeArticle.Location = new Point(50, 170);
            supprimeArticle.Click += new System.EventHandler(SupprimeArticle_Click);
            supprimeArticle.Size = new System.Drawing.Size(145, 40);


            //Le bouton quitter
            quitter = new Button();
            quitter.Text = "Quitter";
            quitter.Location = new Point(50, 230);
            quitter.Click += new System.EventHandler(Quitter_Click);
            quitter.Size = new System.Drawing.Size(145, 40);

            // Ajout des composants à la fenêtre 
            Controls.Add(txtTitre);
            Controls.Add(verificationSTE);
            Controls.Add(ajoutArticle); 
            Controls.Add(supprimeArticle);
            Controls.Add(quitter);




        }

        // Gestionnaire d'événements
        private void VerificationCDE_Click(object sender, EventArgs e)
        {



            Fenetre_Consultation fen = new Fenetre_Consultation();
            fen.Show();



        }

        private void AjoutArticle_Click(object sender, EventArgs e)
        {


            FenetreArticle fen = new FenetreArticle(1);
            fen.Show();


        }
        private void SupprimeArticle_Click(object sender, EventArgs e)
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