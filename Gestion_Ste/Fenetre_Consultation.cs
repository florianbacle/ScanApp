using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;



namespace Gestion_Ste
{
    public partial class Fenetre_Consultation : Form
    {
        //Gestion des champs à taille variable
        private int cptChamp;

        private Label txtSte;
        private TextBox ste;

        private Label txtArt;
        private TextBox codeArt;


        private String lot;
        private String datePer;

        private TextBox console;

        private Button articleSaisi;
        private Button articleManquant;

        private Button retour;
        private Button quitter;

        public Fenetre_Consultation()
        {
            this.cptChamp = 0;
            this.BackColor = Color.FromArgb(255, 186, 117);

            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.KeyPreview = true;
            this.Name = "Gestion des commandes";
            this.ResumeLayout(false);

            txtSte = new Label();
            txtSte.Text = "num sté :";
            txtSte.Location = new Point(5, 10);
            txtSte.Size = new System.Drawing.Size(80, 20);
            txtSte.Font = new Font("Verdana", 9, System.Drawing.FontStyle.Bold);

            ste = new TextBox();
            ste.Text = null;
            ste.Location = new Point(95, 10);
            ste.Size = new System.Drawing.Size(100, 20);
            ste.MaxLength = 8;
            ste.KeyDown += new System.Windows.Forms.KeyEventHandler(Ste_KeyDown);

            txtArt = new Label();
            txtArt.Text = "Article :";
            txtArt.Location = new Point(5, 35);
            txtArt.Size = new System.Drawing.Size(50, 20);
            txtArt.Font = new Font("Verdana", 9, System.Drawing.FontStyle.Bold);

            codeArt = new TextBox();
            codeArt.Text = null;
            codeArt.Location = new Point(95, 35);
            codeArt.KeyDown += new System.Windows.Forms.KeyEventHandler(codeArt_KeyDown);
            codeArt.MaxLength = 100;
            codeArt.Size = new System.Drawing.Size(140, 20);

            // Le bouton "articleSaisi"
            articleSaisi = new Button();
            articleSaisi.Text = "Articles Saisis";
            articleSaisi.Location = new Point(20, 60);
            articleSaisi.Click += new System.EventHandler(articleSaisi_Click);
            articleSaisi.Size = new System.Drawing.Size(100, 25);

            // Le bouton "articleManquant"
            articleManquant = new Button();
            articleManquant.Text = "Articles manquants";
            articleManquant.Location = new Point(130, 60);
            articleManquant.Click += new System.EventHandler(articleManquant_Click);
            articleManquant.Size = new System.Drawing.Size(100, 25);
           
            lot = null;
            datePer =null;

            console = new TextBox();
            console.Text = null;
            console.Location = new Point(5, 95);
            console.Multiline = true;
            console.ScrollBars = ScrollBars.Vertical;
            console.Size = new System.Drawing.Size(230, 160);

            // Le bouton "envoyer"
            retour = new Button();
            retour.Text = "Retour";
            retour.Location = new Point(40, 260);
            retour.Click += new System.EventHandler(Retour_Click);

            // Le bouton "quitter"
            quitter = new Button();
            quitter.Text = "Quitter";
            quitter.Location = new Point(130, 260);
            quitter.Click += new System.EventHandler(Quitter_Click);

         
            // Ajout des composants à la fenêtre 
            Controls.Add(txtSte);
            Controls.Add(ste);

            Controls.Add(txtArt);
            Controls.Add(codeArt);

            Controls.Add(articleManquant);
            Controls.Add(articleSaisi);

            Controls.Add(console);
            Controls.Add(retour);
            Controls.Add(quitter);

        }

        //Event sur la boite commande pour passer le focus
        private void Ste_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                this.codeArt.Focus();
            }
        }

        // Gestionnaire d'événement
        private void Retour_Click(object sender, EventArgs e)
        {

            this.Dispose();
            this.Close();

        }

        private void Quitter_Click(object sender, EventArgs e)
        {

            this.Dispose();
            this.Close();
            Application.Exit();

        }


        private void codeArt_KeyDown(object sender, KeyEventArgs e)
        {

            //Controle de fin de saisie (Nessecite le caractrère <CR> en fin de code barre)
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {

                this.codeArt.Text += "#";
                codeArt.SelectionStart = codeArt.TextLength;
                LectureCodeBarreEan128(this.codeArt.Text);

               

                //Récuperation des configurations
                string ip = SmartDeviceProject3.ConfigurationManager.GetKey<string>("Ip");
                string bd = SmartDeviceProject3.ConfigurationManager.GetKey<string>("Bd");
                string user = SmartDeviceProject3.ConfigurationManager.GetKey<string>("User");

                //This is your database connexion:
                string connectionString = "Data Source=" + ip + ";Initial Catalog=" + bd + ";User ID=" + user;
                SqlConnection cn = new SqlConnection(connectionString);


                try
                {

                    cn.Open();
               
                    

                }
                catch (SqlException ex)
                {

                    for (int i = 0; i < ex.Errors.Count; i++)
                    {
                        this.console.Text += "\r\n Error #" + i + ex.Errors[i].Message;
                    }
                   


                }
                finally
                {
                    if (cn.State != ConnectionState.Closed)
                    {
                        cn.Close();
                    }
                }

            }


            //Controle des champs à taille variable
            if (e.KeyCode == System.Windows.Forms.Keys.Menu)
            {
                this.cptChamp = 1;

            }
            else if (this.cptChamp == 1 && e.KeyCode == System.Windows.Forms.Keys.NumPad2)
            {
                this.cptChamp = 2;
            }
            else if (this.cptChamp == 2 && e.KeyCode == System.Windows.Forms.Keys.NumPad9)
            {

                this.codeArt.Text += "#";
                codeArt.SelectionStart = codeArt.TextLength;
                this.cptChamp = 0;
            }
            else
            {
                this.cptChamp = 0;
            }


        }

        private void LectureCodeBarreEan128(String code)
        {
            if (code.Length > 1)
            {

                String Aim = code.Substring(0, 2);

                switch (Aim)
                {
                    case "01": this.codeArt.Text = code.Substring(3, 13); LectureCodeBarreEan128(code.Substring(16, code.Length - 16)); break;
                    case "17": this.datePer = code.Substring(2, 6); LectureCodeBarreEan128(code.Substring(8, code.Length - 8)); break;

                    case "30": LectureCodeBarreEan128(code.Substring(code.IndexOf("#") + 1, code.Length - (code.IndexOf("#") + 1))); break;

                    case "10": this.lot = code.Substring(2, code.IndexOf("#") - 2); LectureCodeBarreEan128(code.Substring(code.IndexOf("#") + 1, code.Length - (code.IndexOf("#") + 1))); break;

                    default: this.console.Text += "\r\n" + "identifiant inconnu :" + code; LectureCodeBarreEan128(""); break;


                }
            }
            else if (code.Length == 1)
            {
                this.console.Text += "\r\n" + "symbole inconnu :" + code;
            }

        }

        private void articleSaisi_Click(object sender, EventArgs e)
        {


            //Récuperation des configurations
            string ip = SmartDeviceProject3.ConfigurationManager.GetKey<string>("Ip");
            string bd = SmartDeviceProject3.ConfigurationManager.GetKey<string>("Bd");
            string user = SmartDeviceProject3.ConfigurationManager.GetKey<string>("User");

            //This is your database connexion:
            string connectionString = "Data Source=" + ip + ";Initial Catalog=" + bd + ";User ID=" + user;

            SqlConnection cn = null;
            try
            {
                 cn = new SqlConnection(connectionString);
                cn.Open();
                if (Verif_NumSte(cn) == 1)
                {
                    Recup_Article_Saisi(cn);
                }


            }
            catch (SqlException ex)
            {

                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    this.console.Text += "\r\n Error #" + i + ex.Errors[i].Message;
                }



            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();
                }
            }

            
            


        }

        private void articleManquant_Click(object sender, EventArgs e)
        {


            //Récuperation des configurations
            string ip = SmartDeviceProject3.ConfigurationManager.GetKey<string>("Ip");
            string bd = SmartDeviceProject3.ConfigurationManager.GetKey<string>("Bd");
            string user = SmartDeviceProject3.ConfigurationManager.GetKey<string>("User");

            //This is your database connexion:
            string connectionString = "Data Source=" + ip + ";Initial Catalog=" + bd + ";User ID=" + user;
            SqlConnection cn = null;
            try
            {
                cn = new SqlConnection(connectionString);
                cn.Open();
                if (Verif_NumSte(cn) == 1)
                {
                    Recup_Article_Manquant(cn);
                }


            }
            catch (SqlException ex)
            {

                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    this.console.Text += "\r\n Error #" + i + ex.Errors[i].Message;
                }



            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();
                }
            }


        }



        private void Recup_Article_Manquant(SqlConnection cn)
        {

            //Paramètre sql, stock le numero de la commande/Bl saisi
            SqlParameter ParamNumSte = new SqlParameter("@ParamNumSte", SqlDbType.Char);
            ParamNumSte.Value = this.ste.Text;

            this.console.Text += "\r\n" + "_____________________________";
            this.console.Text += "\r\n" + "              ARTICLE(S) MANQUANT(S) : ";
            this.console.Text += "\r\n" + "              Ref : "+this.ste.Text+"\r\n";

            SqlCommand sCommand =new SqlCommand("select Artic.C_artic,lot.C_lot,depstd.qte_us from DEPOT,DEPSTD,ARTIC,LOT where DEPOT.T_DEPOT=DEPSTD.T_DEPOT and DEPSTD.T_ARTIC=ARTIC.T_ARTIC and DEPSTD.T_LOT=LOT.T_LOT  and DEPOT.C_DEPOT='002' and c_lot like '%'+@ParamNumSte", cn);
            sCommand.Parameters.Add(ParamNumSte);
            SqlDataReader sqlReader = sCommand.ExecuteReader();
           
            while (sqlReader.Read())
            {
                this.console.Text += "\r\n" + "Article :" + sqlReader.GetValue(0).ToString() + "\r\n" + "Lot :" + sqlReader.GetValue(1).ToString() + "\r\n" + "Quantité :" + sqlReader.GetValue(2).ToString() + "\r\n";
            }
            sqlReader.Close();
        }

        private int Verif_NumSte(SqlConnection cn)
        {

            //Paramètre sql, stock le numero de la commande/Bl saisi
            SqlParameter ParamNumSte = new SqlParameter("@ParamNumSte", SqlDbType.Char);
            ParamNumSte.Value = this.ste.Text;


            SqlCommand sCommand= new SqlCommand ("select C_lot from lot where c_lot like '%'+@ParamNumSte",cn);
            sCommand.Parameters.Add(ParamNumSte);


            SqlDataReader sqlReader = sCommand.ExecuteReader();
           
            if(!sqlReader.Read()){
                this.console.Text += "\r\n" + "Ce numéro de sté est invalide";
                sqlReader.Close();
                return 0;
            }
             this.console.Text += "\r\n" + "num sté valide";
             sqlReader.Close();
            return 1;
        }

        private void Recup_Article_Saisi(SqlConnection cn)
        {

            //Paramètre sql, stock le numero de la commande/Bl saisi
            SqlParameter ParamNumSte = new SqlParameter("@ParamNumSte", SqlDbType.Char);
            ParamNumSte.Value = this.ste.Text;

            this.console.Text += "\r\n" + "_____________________________";
            this.console.Text += "\r\n" + "              ARTICLE(S) SAISI(S) : ";
            this.console.Text += "\r\n" + "              Ref : "+this.ste.Text+"\r\n";

            SqlCommand sCommand = new SqlCommand("select artic.C_artic,C_lot,qte_us,c_depot from DEPOT,SOPHYSCANSTE,ARTIC where depot.T_depot=sophyscanste.T_depot and artic.t_artic=sophyscanste.t_artic and c_lot like '%'+@ParamNumSte", cn);
            sCommand.Parameters.Add(ParamNumSte);
            SqlDataReader sqlReader = sCommand.ExecuteReader();
           
            while (sqlReader.Read())
            {
                this.console.Text += "\r\n" + "Article :" + sqlReader.GetValue(0).ToString() + "\r\n" + "Lot :" + sqlReader.GetValue(1).ToString() + "\r\n" + "Quantité :" + sqlReader.GetValue(2).ToString() + "\r\n" + "Dépôt :" + sqlReader.GetValue(3).ToString() + "\r\n";
            }
            sqlReader.Close();
        }


    }
}