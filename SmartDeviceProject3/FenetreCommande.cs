using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;


namespace SmartDeviceProject3
{

    class FenetreCommande : Form
    {

        private Label l_Ref;
        private TextBox tb_NumRef;

        private Button b_ArticleSaisi;
        private Button b_ArticleManquant;
        private Button b_ArticleSurplus;

        private TextBox tb_Console;
        private Button b_Retour;
        private Button b_Quitter;

        public FenetreCommande()
        {
            this.BackColor = Color.FromArgb(255, 255, 255);

            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.KeyPreview = true;
            this.Name = "Gestion des commandes";
            this.ResumeLayout(false);

            this.l_Ref = new Label();
            this.l_Ref.Text = "Numéro ref :";
            this.l_Ref.Location = new Point(5, 10);
            this.l_Ref.Size = new System.Drawing.Size(80, 20);
            this.l_Ref.Font = new Font("Verdana", 9, System.Drawing.FontStyle.Bold);


            this.tb_NumRef = new TextBox();
            this.tb_NumRef.Text = null;
            this.tb_NumRef.Location = new Point(95, 10);
            this.tb_NumRef.Size = new System.Drawing.Size(100, 20);
            this.tb_NumRef.MaxLength = 8;
            this.tb_NumRef.BackColor = Color.FromArgb(150, 218, 177);

           
            // Le bouton "b_ArticleSaisi"
            this.b_ArticleSaisi = new Button();
            this.b_ArticleSaisi.Text = "Saisi(s)";
            this.b_ArticleSaisi.Location = new Point(10, 40);
            this.b_ArticleSaisi.Click += new System.EventHandler(b_ArticleSaisi_Click);
            this.b_ArticleSaisi.Size = new System.Drawing.Size(60, 25);
            this.b_ArticleSaisi.BackColor = Color.FromArgb(150, 218, 177);

            // Le bouton "articleManquant"
            this.b_ArticleManquant = new Button();
            this.b_ArticleManquant.Text = "Manquant(s)";
            this.b_ArticleManquant.Location = new Point(80, 40);
            this.b_ArticleManquant.Click += new System.EventHandler(b_ArticleManquant_Click);
            this.b_ArticleManquant.Size = new System.Drawing.Size(90, 25);
            this.b_ArticleManquant.BackColor = Color.FromArgb(150, 218, 177);

            // Le bouton "articleSurplus"
            this.b_ArticleSurplus = new Button();
            this.b_ArticleSurplus.Text = "Surplus";
            this.b_ArticleSurplus.Location = new Point(180, 40);
            this.b_ArticleSurplus.Click += new System.EventHandler(b_ArticleSurplus_Click);
            this.b_ArticleSurplus.Size = new System.Drawing.Size(60, 25);
            this.b_ArticleSurplus.BackColor = Color.FromArgb(150, 218, 177);

            this.tb_Console = new TextBox();
            this.tb_Console.Text = null;
            this.tb_Console.Location = new Point(5, 75);
            this.tb_Console.Multiline = true;
            this.tb_Console.ScrollBars = ScrollBars.Vertical;
            this.tb_Console.Size = new System.Drawing.Size(230, 180);
            this.tb_Console.BackColor = Color.FromArgb(150, 218, 177);

            // Le bouton "envoyer"
            this.b_Retour = new Button();
            this.b_Retour.Text = "Retour";
            this.b_Retour.Location = new Point(40, 260);
            this.b_Retour.Click += new System.EventHandler(b_Retour_Click);
            this.b_Retour.BackColor = Color.FromArgb(150, 218, 177);

            // Le bouton "quitter"
            this.b_Quitter = new Button();
            this.b_Quitter.Text = "Quitter";
            this.b_Quitter.Location = new Point(130, 260);
            this.b_Quitter.Click += new System.EventHandler(b_Quitter_Click);
            this.b_Quitter.BackColor = Color.FromArgb(150, 218, 177);
            

            // Ajout des composants à la fenêtre 
            Controls.Add(l_Ref);
            Controls.Add(tb_NumRef);

            Controls.Add(b_ArticleSaisi);
            Controls.Add(b_ArticleManquant);
            Controls.Add(b_ArticleSurplus);

            Controls.Add(tb_Console);

            Controls.Add(b_Retour);
            Controls.Add(b_Quitter);

            this.tb_Console.Text += "-----------CONSULTATION-----------\r\n";

            this.tb_NumRef.Text = FenetreMenu.s_NumRef;
            this.tb_Console.Text = FenetreMenu.s_Console;
        }

        // Gestionnaire d'événement
        private void b_Retour_Click(object sender, EventArgs e)
        {
        
            FenetreMenu.s_NumRef = this.tb_NumRef.Text;
            FenetreMenu.s_Console = this.tb_Console.Text;
            this.Dispose();
            this.Close();

        }

        private void b_Quitter_Click(object sender, EventArgs e)
        {

            this.Dispose();
            this.Close();
            Application.Exit();

        }

        private void b_ArticleSaisi_Click(object sender, EventArgs e)
        {


            //Récuperation des configurations
            string ip = ConfigurationManager.GetKey<string>("Ip");
            string bd = ConfigurationManager.GetKey<string>("Bd");
            string user = ConfigurationManager.GetKey<string>("User");

            //This is your database connexion:
            string connectionString = "Data Source=" + ip + ";Initial Catalog=" + bd + ";User ID=" + user;
            SqlConnection cn = null;
            try
            {
                int type = Recup_Format_Document(this.tb_NumRef.Text);
                cn = new SqlConnection(connectionString);
                if (type != 0)
                {
                   
                    cn.Open();
                    if (Verif_Document(cn, type) == 1)
                    {

                        Liste_Article_Saisi(cn, type);

                    }
                }
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    this.tb_Console.Text += "\r\n Error #" + i + ex.Errors[i].Message;
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

        private void b_ArticleManquant_Click(object sender, EventArgs e)
        {
            //Récuperation des configurations
            string ip = ConfigurationManager.GetKey<string>("Ip");
            string bd = ConfigurationManager.GetKey<string>("Bd");
            string user = ConfigurationManager.GetKey<string>("User");

            //This is your database connexion:
            string connectionString = "Data Source=" + ip + ";Initial Catalog=" + bd + ";User ID=" + user;
            SqlConnection cn = null;
            try
            {
               
                int type = Recup_Format_Document(this.tb_NumRef.Text);
                cn = new SqlConnection(connectionString);
               
                if (type != 0  && type!=2)
                {
                    cn.Open();
                    if (Verif_Document(cn, type) == 1)
                    {
                        Liste_Article_Manquant(cn, type);
                    }
                }
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    this.tb_Console.Text += "\r\n Error #" + i + ex.Errors[i].Message;
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

        private void b_ArticleSurplus_Click(object sender, EventArgs e)
        {


            //Récuperation des configurations
            string ip = ConfigurationManager.GetKey<string>("Ip");
            string bd = ConfigurationManager.GetKey<string>("Bd");
            string user = ConfigurationManager.GetKey<string>("User");

            //This is your database connexion:
            string connectionString = "Data Source=" + ip + ";Initial Catalog=" + bd + ";User ID=" + user;
            SqlConnection cn = null;
            try
            {
                
                int type = Recup_Format_Document(this.tb_NumRef.Text);
                cn = new SqlConnection(connectionString);
               
                if (type != 0 && type!=2)
                {
                    cn.Open();
                    if (Verif_Document(cn, type) == 1)
                    {

                        Liste_Article_Surplus(cn, type);

                    }
                }
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    this.tb_Console.Text += "\r\n Error #" + i + ex.Errors[i].Message;
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

        
        /* Fonction de recuperation type de document
           * @param numDoc, Chaine de caractère
           * @return 1, si la chaine correspond au format d'un BL
           * @return 2, si la chaine correspond au format d'une commande
           * @return 3, si la chaine correspond au format d'un depot
           * @reutrn 0, si la chaine ne correspond à aucun des deux format
           * */
        private int Recup_Format_Document(String numDoc)
        {
            int longueur = numDoc.Length;

            if (isDigit(numDoc))
            {

                //Cas bl/Commande
                if (longueur == 8)
                {
                    //Recuperation type de document
                    String type = numDoc.Substring(2, 2);
                    if (type.Equals("11"))
                    {
                        return 1;
                    }
                    else if (type.Equals("12"))
                    {
                        return 2;
                    }
                }
                //Case depot
                else if (longueur == 3)
                {
                    return 3;
                }
            }

            this.tb_Console.Text += "\r\n" + "Format document de reference invalide";
            this.tb_NumRef.Text = null;
            this.tb_NumRef.Focus();
            return 0;

        }
        //1 si le document existe (BL,commande ou depot, 0 sinon
        private int Verif_Document(SqlConnection cn, int type)
        {

            SqlCommand sCommand = null;
            String typeReference = null;

            switch (type)
            {
                case 1:

                    //Paramètre sql stock le numero BL saisi
                    SqlParameter ParamNumBl = new SqlParameter("@ParamNumBl", SqlDbType.Int);
                    ParamNumBl.Value = ParamNumBl.Value = Int32.Parse(this.tb_NumRef.Text);

                    // requete de recuperation du bl saisi
                    sCommand = new SqlCommand("select * from BLVET where NO_DOCET=@ParamNumBl", cn);
                    sCommand.Parameters.Add(ParamNumBl);

                    typeReference = "bl";

                    break;


                case 2:

                    //Paramètre sql stock le numero commande saisi
                    SqlParameter ParamNumCom = new SqlParameter("@ParamNumCom", SqlDbType.Int);
                    ParamNumCom.Value = ParamNumCom.Value = Int32.Parse(this.tb_NumRef.Text);

                    // requete de recuperation de la commande saisie
                    sCommand = new SqlCommand("select * from CDEET where NO_DOCET=@ParamNumCom", cn);
                    sCommand.Parameters.Add(ParamNumCom);

                    typeReference = "commande";

                    break;

                case 3:

                    //Paramètre sql stock le code depot saisi
                    SqlParameter ParamCodeDepot = new SqlParameter("@ParamCodeDepot", SqlDbType.Int);
                    ParamCodeDepot.Value = this.tb_NumRef.Text;

                    // requete de recuperation du bl saisi
                    sCommand = new SqlCommand("select C_DEPOT from DEPOT where C_DEPOT=@ParamCodeDepot", cn);
                    sCommand.Parameters.Add(ParamCodeDepot);

                    typeReference = "dépôt";

                    break;
            }



            //Test resultat de la requete
            SqlDataReader sqlReader = sCommand.ExecuteReader();

            if (!sqlReader.Read())
            {
                this.tb_Console.Text += "\r\n" + "Ce " + typeReference + " n'existe pas";
                sqlReader.Close();
                this.tb_NumRef.Text = null;
                this.tb_NumRef.Focus();
                return 0;
            }
            else
            {
                this.tb_Console.Text += "\r\n" + typeReference + " existe ";
                sqlReader.Close();
                return 1;
            }
        }

        private void Liste_Article_Saisi(SqlConnection cn,int type)
        {
            SqlCommand sCommand = null;


            //Paramètre sql stock le numero BL saisi
            SqlParameter ParamNumDoc = new SqlParameter("@ParamNumDoc", SqlDbType.Int);
            ParamNumDoc.Value = Int32.Parse(this.tb_NumRef.Text);

            this.tb_Console.Text += "\r\n" + "_____________________________";
            this.tb_Console.Text += "\r\n" + "              ARTICLE(S) SAISI(S) : ";
            this.tb_Console.Text += "\r\n" + "              Ref : "+this.tb_NumRef.Text+"\r\n";
            // requete de recuperation des articles saisis par rapport à la référence


            if (type == 3)
            {
                sCommand = new SqlCommand("select ARTIC.C_ARTIC, SOPHYSCANCDE.C_LOT, SOPHYSCANCDE.QTE_US from SOPHYSCANCDE,ARTIC where NO_DOCET=@ParamNumDoc and ARTIC.T_ARTIC=SOPHYSCANCDE.T_ARTIC and datediff (dd,getdate(),DATE)=0", cn);
            }
            else
            {
                sCommand = new SqlCommand("select ARTIC.C_ARTIC, SOPHYSCANCDE.C_LOT, SOPHYSCANCDE.QTE_US from SOPHYSCANCDE,ARTIC where NO_DOCET=@ParamNumDoc and ARTIC.T_ARTIC=SOPHYSCANCDE.T_ARTIC", cn);
            }

            sCommand.Parameters.Add(ParamNumDoc);
            
            SqlDataReader sqlReader = sCommand.ExecuteReader();

            //Descente de la scrollbarre
            this.tb_Console.SelectionStart = this.tb_Console.Text.Length;
            this.tb_Console.ScrollToCaret();
            int cpt = 0;
            while (sqlReader.Read())
            {
                cpt += 1;
                this.tb_Console.Text += "\r\n\r\n" + "Article :" + sqlReader.GetValue(0).ToString() + "\r\n" + "Lot :" + sqlReader.GetValue(1).ToString() + "\r\n" + "Quantité :" + sqlReader.GetValue(2).ToString();
            }

            if (cpt == 0)
            {
                this.tb_Console.Text += "\r\n\r\n" + "Vous n'avez saisi aucun article.";
            }

        }

        /*Fonction de recuperation des articles de la référence qui ne sont pas dans sophyscancde*/
        private int Liste_Article_Manquant(SqlConnection cn,int type)
        {
            SqlCommand sCommand = null;
     
            this.tb_Console.Text += "\r\n" + "_____________________________";
            this.tb_Console.Text += "\r\n" + "             ARTICLE(S) MANQUANT(S) : ";
            this.tb_Console.Text += "\r\n" + "              Ref : " + this.tb_NumRef.Text + "\r\n";
            // requete de recuperation des articles saisis par rapport à la référence

            //Paramètre sql stock le numero BL saisi
            SqlParameter ParamNumDoc = new SqlParameter("@ParamNumDoc", SqlDbType.Int);
            ParamNumDoc.Value = ParamNumDoc.Value = Int32.Parse(this.tb_NumRef.Text);

            switch (type)
            {
                case 1: sCommand = new SqlCommand("select ARTIC.T_ARTIC,ARTIC.C_ARTIC,BLVLG.QTE_US, LOT.C_LOT, depmvd.qte_us, ISNULL(SOPHYSCANCDE.QTE_US,0),DEPMVD.QTE_US- SOPHYSCANCDE.QTE_US as QTE_DIFF from BLVLG,ARTIC,BLVET,DEPMV,DEPMVD,LOT ,SOPHYSCANCDE  where BLVLG.T_ARTIC=ARTIC.T_ARTIC and BLVLG.T_DOCET=BLVET.T_DOCET  and BLVLG.T_DOCLG=DEPMV.T_DOCLG and DEPMV.T_DEPMV=DEPMVD.T_DEPMV and DEPMVD.T_LOT=LOT.T_LOT and DEPMV.C_DOINT=50 and BLVLG.C_TYPAR='DIV' and BLVET.NO_DOCET=@ParamNumDoc and sophyscancde.NO_DOCET =* BLVET.NO_DOCET and sophyscancde.t_artic =* artic.t_artic and sophyscancde.c_lot =* lot.c_lot", cn);
                    break;
                case 3: sCommand = new SqlCommand("select ARTIC.T_ARTIC,ARTIC.C_ARTIC,ARTIC.C_ARTIC,LOT.C_LOT,depmvd.QTE_US,ISNULL(SOPHYSCANCDE.QTE_US,0),DEPMVD.QTE_US- SOPHYSCANCDE.QTE_US as QTE_DIFF from artic, depmv, depmvd, lot, depot ,SOPHYSCANCDE  where artic.t_artic = depmv.t_artic  and depmv.t_depmv = depmvd.t_depmv and depmvd.t_lot = lot.t_lot  and depmv.t_depot = depot.t_depot and DEPMV.QTE_US > 0   and datediff (dd,getdate(),DEPMV.DT_MVT)=0 and DEPMV.NAT_MVT = 13 and DEPOT.C_DEPOT = @ParamNumDoc and sophyscancde.t_artic =* artic.t_artic and sophyscancde.c_lot =* lot.c_lot", cn);
                    break;
                default: return 0;
            }

            sCommand.Parameters.Add(ParamNumDoc);

            //Test resultat de la requete
            SqlDataReader sqlReader = sCommand.ExecuteReader();
            int cpt = 0;
            while (sqlReader.Read())
            {
                if (sqlReader.GetValue(5).ToString() != "0")
                {
                    if ((sqlReader.GetValue(6).ToString() != "0")&& (sqlReader.GetDouble(6)>0))
                    {
                        cpt += 1;
                        this.tb_Console.Text += "\r\n\r\n" + "Article :" + sqlReader.GetValue(1).ToString() + "\r\nLot :" + sqlReader.GetValue(3).ToString()+"\r\n" + "Quantité :" + sqlReader.GetValue(6).ToString();
                    }
                }
                else
                {
                    this.tb_Console.Text += "\r\n\r\n" + "Article :" + sqlReader.GetValue(1).ToString() + "\r\nLot :" + sqlReader.GetValue(3).ToString() +"\r\n" + "Quantité :" + sqlReader.GetValue(4).ToString();
                }
            }

            if (cpt == 0)
            {
                this.tb_Console.Text += "\r\n\r\n" + "il n'y a aucun article manquant.";
            }
            sqlReader.Close();
            return 1;
        }

        /*Fonction de recuperation des articles de sophyscancde qui ne sont pas dans la liste de la référence*/
        private int  Liste_Article_Surplus(SqlConnection cn, int type)
        {
            SqlCommand sCommand = null;

            this.tb_Console.Text += "\r\n" + "_____________________________";
            this.tb_Console.Text += "\r\n" + "             ARTICLE(S) SURPLUS : ";
            this.tb_Console.Text += "\r\n" + "              Ref : " + this.tb_NumRef.Text + "\r\n";
            // requete de recuperation des articles saisis par rapport à la référence

            //Paramètre sql stock le numero BL saisi
            SqlParameter ParamNumDoc = new SqlParameter("@ParamNumDoc", SqlDbType.Int);
            ParamNumDoc.Value = ParamNumDoc.Value = Int32.Parse(this.tb_NumRef.Text);

            switch (type)
            {
                case 1: sCommand = new SqlCommand("select ARTIC.T_ARTIC,ARTIC.C_ARTIC,BLVLG.QTE_US, LOT.C_LOT, depmvd.qte_us, ISNULL(SOPHYSCANCDE.QTE_US,0),DEPMVD.QTE_US- SOPHYSCANCDE.QTE_US as QTE_DIFF from BLVLG,ARTIC,BLVET,DEPMV,DEPMVD,LOT ,SOPHYSCANCDE  where BLVLG.T_ARTIC=ARTIC.T_ARTIC and BLVLG.T_DOCET=BLVET.T_DOCET  and BLVLG.T_DOCLG=DEPMV.T_DOCLG and DEPMV.T_DEPMV=DEPMVD.T_DEPMV and DEPMVD.T_LOT=LOT.T_LOT and DEPMV.C_DOINT=50 and BLVLG.C_TYPAR='DIV' and BLVET.NO_DOCET=@ParamNumDoc and sophyscancde.NO_DOCET =* BLVET.NO_DOCET and sophyscancde.t_artic =* artic.t_artic and sophyscancde.c_lot =* lot.c_lot", cn);
                    break;
                case 3: sCommand = new SqlCommand("select ARTIC.T_ARTIC,ARTIC.C_ARTIC,ARTIC.C_ARTIC,LOT.C_LOT,depmvd.QTE_US,ISNULL(SOPHYSCANCDE.QTE_US,0),DEPMVD.QTE_US- SOPHYSCANCDE.QTE_US as QTE_DIFF from artic, depmv, depmvd, lot, depot ,SOPHYSCANCDE  where artic.t_artic = depmv.t_artic  and depmv.t_depmv = depmvd.t_depmv and depmvd.t_lot = lot.t_lot  and depmv.t_depot = depot.t_depot and DEPMV.QTE_US > 0   and datediff (dd,getdate(),DEPMV.DT_MVT)=0 and DEPMV.NAT_MVT = 13 and DEPOT.C_DEPOT = @ParamNumDoc and sophyscancde.t_artic =* artic.t_artic and sophyscancde.c_lot =* lot.c_lot", cn);
                    break;
                default: return 0;
            }

            sCommand.Parameters.Add(ParamNumDoc);

            //Test resultat de la requete
            SqlDataReader sqlReader = sCommand.ExecuteReader();
            Double tmp_qte;
            int cpt = 0;
            while (sqlReader.Read())
            {
                if (sqlReader.GetValue(5).ToString() != "0")
                {
                    if (sqlReader.GetDouble(6) < 0)
                    {
                                           
                        tmp_qte = sqlReader.GetDouble(6) * (-1);
                        cpt += 1;
                        this.tb_Console.Text += "\r\n\r\n" + "Article :" + sqlReader.GetValue(1).ToString() + "\r\nLot :" + sqlReader.GetValue(3).ToString() +"\r\n" + "Quantité :" + tmp_qte.ToString();
                    }
                }
            }
            if (cpt == 0)
            {
                this.tb_Console.Text += "\r\n\r\n" + "Il n'y a aucun article en surplus.";
            }
            sqlReader.Close();
            return 1;
        }


        public bool isDigit(string chaine)
        {
            bool isDigit;
            isDigit = true;
            foreach (char tmp_char in chaine)
            {
                if (char.IsDigit(tmp_char) == false)
                {
                    isDigit = false;
                }
            }
            return isDigit;
        } 
    }
       

}

