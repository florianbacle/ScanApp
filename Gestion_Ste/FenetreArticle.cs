using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;


namespace Gestion_Ste
{

    partial class FenetreArticle : Form
    {

        //Gestion des champs à taille variable
        private int cptChamp;
        //Stockage du mode
        private int mode;

        private Label txtDepot;
        private ComboBox depot;

        private Label txtArt;
        private TextBox codeArt;

        private String lot;
        private String datePer;

        private Label txtQuantite;
        private TextBox quantite;

        private TextBox console;
        private Button retour;
        private Button quitter;

        public FenetreArticle(int mode)
        {

            this.cptChamp = 0;
            this.BackColor = Color.FromArgb(255, 186, 117);

            this.mode = mode;

            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.KeyPreview = true;
            this.Name = "Gestion des commandes";
            this.ResumeLayout(false);

            txtDepot = new Label();
            txtDepot.Text = "Depot :";
            txtDepot.Location = new Point(5, 10);
            txtDepot.Size = new System.Drawing.Size(80, 20);
            txtDepot.Font = new Font("Verdana", 9, System.Drawing.FontStyle.Bold);

            depot = new ComboBox();
            depot.Text = null;
            depot.Items.Add("001");
            depot.Items.Add("903");
            depot.Items.Add("003");
            depot.Items.Add("006");
            depot.Items.Add("901");
            depot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            depot.Location = new Point(95, 10);
            depot.Size = new System.Drawing.Size(100, 20);
            depot.KeyDown += new System.Windows.Forms.KeyEventHandler(depot_KeyDown);
           
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

            lot = null;
            datePer =null;
           
            txtQuantite = new Label();
            txtQuantite.Text = "Quantité :";
            txtQuantite.Location = new Point(5, 60);
            txtQuantite.Size = new System.Drawing.Size(65, 20);
            txtQuantite.Font = new Font("Verdana", 9, System.Drawing.FontStyle.Bold);

            quantite = new TextBox();
            quantite.Text = null;
            quantite.Location = new Point(95, 60);
            quantite.MaxLength = 6;
            quantite.Size = new System.Drawing.Size(100, 20);


            console = new TextBox();
            console.Text = null;
            console.Location = new Point(5, 85);
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
            Controls.Add(txtDepot);
            Controls.Add(depot);

            Controls.Add(txtArt);
            Controls.Add(codeArt);

            Controls.Add(txtQuantite);
            Controls.Add(quantite);

            Controls.Add(console);
            Controls.Add(retour);
            Controls.Add(quitter);

            //Affichage mode
            if (mode == 1)
            {
                this.console.Text += "-----------AJOUT D'ARTICLES-----------\r\n";
            }
            else
            {
                this.console.Text += "-------SUPPRESSION D'ARTICLES--------\r\n";
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

        //Event sur la boite commande pour passer le focus
        private void depot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                this.codeArt.Focus();
            }
        }

        private void codeArt_KeyDown(object sender, KeyEventArgs e)
        {

            //Controle de fin de saisie (Nessecite le caractrère <CR> en fin de code barre)
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {

                int type = 0;
                if(this.codeArt.Text.Contains(':')){
                     LectureCodeBarreSophyCode(this.codeArt.Text);
                    
                }else{
                     this.codeArt.Text += "#";
                     codeArt.SelectionStart = codeArt.TextLength;

                     LectureCodeBarreEan128(this.codeArt.Text);
                     if (this.quantite.Text == "")
                     {
                         this.quantite.Text = "1";

                     }
                     type = 1;
                        
                }

               

                
                //This is your database connexion:
                string connectionString = "Data Source=130.0.0.13;Initial Catalog=PYRA;User ID=consult";
                SqlConnection cn = new SqlConnection(connectionString);


                try
                {

                    cn.Open();
                   
                        if (Verif_Format_Depot())
                        {
                            String tokenArt=Verif_Article(cn,type);
                            if (tokenArt != null)
                            {
                                String tokenDepot = Verif_Depot(cn);
                                if (tokenDepot != null)
                                {
                                    int type_ajout=Verif_Saisie_Article(cn, tokenDepot, tokenArt);
                                    if (this.mode == 1)
                                    {
                                        switch (type_ajout)
                                        {
                                            case 1: Insert_Article(cn, tokenDepot, tokenArt); break;
                                            case 2: Update_Article(cn, tokenDepot, tokenArt); break;
                                        }
                                    }
                                    else
                                    {
                                        switch (type_ajout)
                                        {
                                            case 1: Supprime_Article(cn, tokenDepot, tokenArt); break;
                                            case 2: Update_Article(cn, tokenDepot, tokenArt); break;
                                        }

                                    }
                                }
                            }
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

                    case "30": this.quantite.Text = code.Substring(2, code.IndexOf("#") - 2); LectureCodeBarreEan128(code.Substring(code.IndexOf("#") + 1, code.Length - (code.IndexOf("#") + 1))); break;

                    case "10": this.lot = code.Substring(2, code.IndexOf("#") - 2); LectureCodeBarreEan128(code.Substring(code.IndexOf("#") + 1, code.Length - (code.IndexOf("#") + 1))); break;

                    default: this.console.Text += "\r\n" + "identifiant inconnu :" + code; LectureCodeBarreEan128(""); break;


                }
            }
            else if (code.Length == 1)
            {
                this.console.Text += "\r\n" + "symbole inconnu :" + code;
            }

        }

        private void LectureCodeBarreSophyCode(String code)
        {
            String reference = code.Substring(0, code.LastIndexOf(':'));
            String lot = code.Substring(code.LastIndexOf(':')+1);
            this.codeArt.Text = reference;
            this.lot = lot;
        }

      

        private void VideChamps()
        {
            this.codeArt.Text = null;
            this.datePer = null;
            this.lot = null;
            this.quantite.Text = null;

            //Descente de la scrollbarre
            this.console.SelectionStart = this.console.Text.Length;
            this.console.ScrollToCaret();

            //Retour sur codeArt pour une nouvelle saisie
            this.codeArt.Focus();


        }

        private Boolean Verif_Format_Depot()
        {
            if (this.depot.Text.Length != 3)
            {
                this.console.Text += "\r\n\r\n" + "Format saisie depot non valide";
                return false;

            }
          
            return true;

        }

        /* Fonction de Verification depot exist, token depot si ok, null sinn*/
        private String Verif_Depot(SqlConnection cn)
        {
            SqlCommand sCommand = null;

            //Paramètre sql, stock le numéro de depot saisie
            SqlParameter ParamDepot = new SqlParameter("@ParamDepot", SqlDbType.Int);
            ParamDepot.Value = Int32.Parse(this.depot.Text);

            //Selection du depot dans la base de donnée;
            sCommand = new SqlCommand("select T_DEPOT from DEPOT where C_DEPOT=@ParamDepot", cn);
            sCommand.Parameters.Add(ParamDepot);

            //Test resultat de la requete
            SqlDataReader sqlReader = sCommand.ExecuteReader();

            if (!sqlReader.Read())
            {

                this.console.Text += "\r\n" + "Ce depot n'existe pas";
                sqlReader.Close();
                this.VideChamps();
                this.depot.Text = null;
                this.depot.Focus();
                return null;
            }
            else
            {
                String tokenDepot = sqlReader.GetValue(0).ToString();
                this.console.Text += "\r\n" + "depot existe ";
                sqlReader.Close();
                return tokenDepot;
            }
            


        }


       


        /*Fonction de vérification article déjà saisi ou n'estp as en sTé, return 0 si oui, 1 si la qté saisie dans sophyscanste est 0 2 sinon*/
        private int Verif_Saisie_Article(SqlConnection cn, String tokenDepot, String tokenArt)
        {
            

            SqlCommand sCommandDepot = null;
            SqlCommand sCommandSophyScan = null;

            SqlParameter ParamTokenDepot = new SqlParameter("@ParamTokenDepot", SqlDbType.Int);
            ParamTokenDepot.Value = Int32.Parse(tokenDepot);

            //Paramètre sql, stock le gtin de l'article saisi
            SqlParameter ParamTokenArt = new SqlParameter("@ParamTokenArt", SqlDbType.Int);
            ParamTokenArt.Value = tokenArt;

            //Paramètre sql, stock le numéro de lot saisi
            SqlParameter ParamLot = new SqlParameter("@ParamLot", SqlDbType.Char, this.lot.Length);
            ParamLot.Value = this.lot;

          
            //Requete d'addition des quantités de tous les articles du lot et du BL désigné dans le depot 002
            sCommandDepot = new SqlCommand("select COALESCE(sum(qte_us),0) from DEPOT,DEPSTD,ARTIC,LOT where DEPOT.T_DEPOT=DEPSTD.T_DEPOT and DEPSTD.T_ARTIC=ARTIC.T_ARTIC and DEPSTD.T_LOT=LOT.T_LOT and ARTIC.T_ARTIC=@ParamTokenArt and LOT.C_LOT=@ParamLot and DEPOT.C_DEPOT='002'", cn);
            sCommandDepot.Parameters.Add(ParamTokenArt);
            sCommandDepot.Parameters.Add(ParamLot);
            sCommandDepot.Parameters.Add(ParamTokenDepot);

            double resDepot = (double)sCommandDepot.ExecuteScalar();
            if (resDepot == 0)
            {
                this.console.Text += "\r\n" + "Cet article n'est pas en désorption";
                this.VideChamps();
                return 0;
            }
            sCommandDepot.Parameters.Clear();

            //Requete d'addition des quantités de tous les articles du lot et du BL désigné dans SOPHYSCANste
            sCommandSophyScan = new SqlCommand("select COALESCE(sum(qte_us),0) from SOPHYSCANSTE where T_ARTIC=@ParamTokenArt and C_LOT=@ParamLot", cn);
            sCommandSophyScan.Parameters.Add(ParamLot);
            sCommandSophyScan.Parameters.Add(ParamTokenArt);
            sCommandSophyScan.Parameters.Add(ParamTokenDepot);
         
            int resSophyScan = (int)sCommandSophyScan.ExecuteScalar();

            switch(this.mode){

                case 1: if ((resSophyScan + Int32.Parse(this.quantite.Text)) > resDepot)
                    {
                        this.console.Text += "\r\n" + "quantité article depassé";
                        this.VideChamps();
                        return 0;
                    }
                    else
                    {
                        this.console.Text += "\r\n" + "quantité article ok";
                        
                        if (resSophyScan == 0)
                        {
                            return 1;
                        }
                        return 2;
                    }

                case 2: if ((resSophyScan - Int32.Parse(this.quantite.Text)) < 0)
                        {
                            this.console.Text += "\r\n" + "quantité article à supprimer invalide";
                            this.VideChamps();
                            return 0;
                        }
                       
                        else if (resSophyScan - Int32.Parse(this.quantite.Text) == 0)
                        {
                            return 1;
                        }
                       
                        return 2;
           }
            return 0;

        }


        //Insertion d'un article
        private void Insert_Article(SqlConnection cn, String tokenDepot, String tokenArt)
        {
            SqlCommand sCommand = null;
            //Paramètre sql, stock le numero de la commande/Bl saisi
            SqlParameter ParamTokenDepot = new SqlParameter("@ParamTokenDepot", SqlDbType.Int);
            ParamTokenDepot.Value = Int32.Parse(tokenDepot);

            //Paramètre sql, stock le numéro système de l'article saisi
            SqlParameter ParamTokenArt = new SqlParameter("@ParamTokenArt", SqlDbType.Int);
            ParamTokenArt.Value = Int32.Parse(tokenArt);

            //Paramètre sql, stock le numéro de lot saisi
            SqlParameter ParamLot = new SqlParameter("@ParamLot", SqlDbType.Char, this.lot.Length);
            ParamLot.Value = this.lot;

            //Paramètre sql, stock le numero de la commande/Bl saisi
            SqlParameter ParamQte = new SqlParameter("@ParamQte", SqlDbType.Int);
            ParamQte.Value = Int32.Parse(this.quantite.Text);


            sCommand = new SqlCommand("insert into SOPHYSCANSTE values (@ParamTokenArt,@paramLot,@paramQte,@ParamTokenDepot)", cn);
            sCommand.Parameters.Add(ParamTokenDepot);
            sCommand.Parameters.Add(ParamLot);
            sCommand.Parameters.Add(ParamQte);
            sCommand.Parameters.Add(ParamTokenArt);
            sCommand.ExecuteNonQuery();

            this.console.Text += "\r\n\r\n" + "Ajout Article effectué :" + "\r\n" + this.codeArt.Text + "\r\n" + this.lot + "\r\n" + this.datePer + "\r\n" + this.quantite.Text;
            this.VideChamps();



        }

        //Update d'une ligne, tyoe_update 1=ajout other=suppresion
        private void Update_Article(SqlConnection cn, String tokenDepot, String tokenArt)
        {
            SqlCommand sCommand = null;
            //Paramètre sql, stock le numero de la commande/Bl saisi
            SqlParameter ParamTokenDepot = new SqlParameter("@ParamTokenDepot", SqlDbType.Int);
            ParamTokenDepot.Value = Int32.Parse(tokenDepot);

            //Paramètre sql, stock le numéro système de l'article saisi
            SqlParameter ParamTokenArt = new SqlParameter("@ParamTokenArt", SqlDbType.Int);
            ParamTokenArt.Value = Int32.Parse(tokenArt);

            //Paramètre sql, stock le numéro de lot saisi
            SqlParameter ParamLot = new SqlParameter("@ParamLot", SqlDbType.Char, this.lot.Length);
            ParamLot.Value = this.lot;

            //Paramètre sql, stock le numero de la commande/Bl saisi
            SqlParameter ParamQte = new SqlParameter("@ParamQte", SqlDbType.Int);
            ParamQte.Value = Int32.Parse(this.quantite.Text);


            if (this.mode == 1)
            {
                sCommand = new SqlCommand("update SOPHYSCANSTE set QTE_US=QTE_US+@paramQte where C_LOT=@ParamLot and T_ARTIC=@ParamTokenArt and T_DEPOT=@ParamTokenDepot", cn);
            }
            else
            {
                this.console.Text += "bop";
                sCommand = new SqlCommand("update SOPHYSCANSTE set QTE_US=QTE_US-@paramQte where C_LOT=@ParamLot and T_ARTIC=@ParamTokenArt and T_DEPOT=@ParamTokenDepot", cn);
            }
           
            sCommand.Parameters.Add(ParamTokenDepot);
            sCommand.Parameters.Add(ParamLot);
            sCommand.Parameters.Add(ParamQte);
            sCommand.Parameters.Add(ParamTokenArt);
            sCommand.ExecuteNonQuery();

            this.console.Text += "\r\n\r\n" + "update Article effectué :" + "\r\n" + this.codeArt.Text + "\r\n" + this.lot + "\r\n" + this.datePer + "\r\n" + this.quantite.Text;
            this.VideChamps();

        }

        //token article si article existe, null sinon
        private String Verif_Article(SqlConnection cn,int type)
        {
            SqlCommand sCommand = null;

            switch (type)
            {
                case 1:
                    //Paramètre sql, stock le gtin de l'article saisi
                    SqlParameter ParamGtinArt = new SqlParameter("@ParamGtinArt", SqlDbType.Char, 13);
                    ParamGtinArt.Value = this.codeArt.Text;

                    //requete de recupération du code de l'article via son gtin
                    sCommand = new SqlCommand("select T_ARTIC,C_ARTIC from ARTIC where C_EANAR=@ParamGtinArt", cn);
                    sCommand.Parameters.Add(ParamGtinArt);
                    break;
                case 0:
                    if (this.quantite.Text == "")
                    {
                        this.console.Text += "\r\n" + "Veuillez preciser une quantité";
                        this.VideChamps();
                        return null;
                    }
                    //Paramètre sql, stock le gtin de l'article saisi
                    SqlParameter ParamCodeArt = new SqlParameter("@ParamCodeArt", SqlDbType.Char, 13);
                    ParamCodeArt.Value = this.codeArt.Text;

                    //requete de recupération du code de l'article via son gtin
                    sCommand = new SqlCommand("select T_ARTIC,C_ARTIC from ARTIC where C_ARTIC=@ParamCodeArt", cn);
                    sCommand.Parameters.Add(ParamCodeArt);
                    break;

            }

            SqlDataReader sqlReader = sCommand.ExecuteReader();

            if (sqlReader.Read())
            {
                
                String tokenArt = sqlReader.GetValue(0).ToString();
                this.codeArt.Text = sqlReader.GetValue(1).ToString();
                this.console.Text += "\r\n" + "Cet article existe";
               
                sqlReader.Close();
                return tokenArt;

            }
            else
            {
                this.console.Text += "\r\n" + "Cet article n'existe pas";
                sqlReader.Close();
                this.VideChamps();
                return null;
            }

        }

        //1 si l'élément article,lot, 0 sinon
        private void Supprime_Article(SqlConnection cn,String tokenDepot, String tokenArt)
        {

            SqlCommand sCommandDel = null;

            //Paramètre sql, stock le numero de la commande/Bl saisi
            SqlParameter ParamTokenDepot = new SqlParameter("@ParamTokenDepot", SqlDbType.Int);
            ParamTokenDepot.Value = Int32.Parse(tokenDepot);

            //Paramètre sql, stock le numéro système de l'article saisi
            SqlParameter ParamTokenArt = new SqlParameter("@ParamTokenArt", SqlDbType.Int);
            ParamTokenArt.Value = Int32.Parse(tokenArt);

            //Paramètre sql, stock le numéro de lot saisi
            SqlParameter ParamLot = new SqlParameter("@ParamLot", SqlDbType.Char, this.lot.Length);
            ParamLot.Value = this.lot;

            //Paramètre sql, stock la quantité saisie
            SqlParameter ParamQte = new SqlParameter("@ParamQte", SqlDbType.Int);
            ParamQte.Value = Int32.Parse(this.quantite.Text);


            sCommandDel = new SqlCommand("delete from SOPHYSCANSTE where QTE_US=@paramQte and C_LOT=@ParamLot and T_ARTIC=@ParamTokenArt and T_DEPOT=@ParamTokenDepot", cn);


            sCommandDel.Parameters.Add(ParamTokenDepot);
            sCommandDel.Parameters.Add(ParamLot);
            sCommandDel.Parameters.Add(ParamQte);
            sCommandDel.Parameters.Add(ParamTokenArt);
            sCommandDel.ExecuteNonQuery();

            this.console.Text += "\r\n\r\n" + "Suppression Article effectué :" + "\r\n" + this.depot.Text + "\r\n" + this.codeArt.Text + "\r\n" + this.lot + "\r\n" + this.datePer + "\r\n" + this.quantite.Text;
            this.VideChamps();
        }




        ////1 si nb article depassé, 0 sinon
        //private int Verif_Nb_Article_Table(SqlConnection cn, int type, String TokenArt)
        //{
        //    SqlCommand sCommandSophyScan = null;
        //    SqlCommand sCommandCdeBl = null;
        //    //Paramètre sql, stock le numero de la commande/Bl saisi
        //    /*SqlParameter ParamNumDoc = new SqlParameter("@ParamNumDoc", SqlDbType.Int);
        //    ParamNumDoc.Value = Int32.Parse(this.commande.Text);*/

        //    //Paramètre sql, stock le gtin de l'article saisi
        //    SqlParameter ParamTokenArt = new SqlParameter("@ParamTokenArt", SqlDbType.Int);
        //    ParamTokenArt.Value = TokenArt;

        //    //Paramètre sql, stock le numéro de lot saisi
        //    SqlParameter ParamLot = new SqlParameter("@ParamLot", SqlDbType.Char, this.lot.Text.Length);
        //    ParamLot.Value = this.lot.Text;

        //    switch (type)
        //    {
        //        case 1: //Requete d'addition des quantités de tous les articles du lot et du BL désigné dans SOPHYSCANCDE
        //            sCommandSophyScan = new SqlCommand("select COALESCE(sum(qte_us),0) from SOPHYSCANCDE where NO_DOCET=@ParamNumDoc and T_ARTIC=@ParamTokenArt and C_LOT=@paramLot", cn);
        //            sCommandSophyScan.Parameters.Add(ParamTokenArt);
        //            sCommandSophyScan.Parameters.Add(ParamNumDoc);
        //            sCommandSophyScan.Parameters.Add(ParamLot);
        //            break;

        //        case 2: //Requete de recherche de l'article désigné par son gtin dans les articles de la commande
        //            sCommandSophyScan = new SqlCommand("select COALESCE(sum(qte_us),0) from SOPHYSCANCDE where NO_DOCET=@ParamNumDoc and T_ARTIC=@ParamTokenArt", cn);
        //            sCommandSophyScan.Parameters.Add(ParamNumDoc);
        //            sCommandSophyScan.Parameters.Add(ParamTokenArt);
        //            break;
        //    }


        //    int resSophyScan = (int)sCommandSophyScan.ExecuteScalar();

        //    sCommandSophyScan.Parameters.Clear();

        //    switch (type)
        //    {
        //        case 1: //Requete d'addition des quantités de tous les articles du lot et du BL désigné dans SOPHYSCANCDE
        //            sCommandCdeBl = new SqlCommand("select COALESCE(sum(qte_us),0) from BLVLG,ARTIC,BLVET,DEPMV,DEPMVD,LOT where BLVLG.T_ARTIC=ARTIC.T_ARTIC and BLVLG.T_DOCET=BLVET.T_DOCET and BLVLG.T_DOCLG=DEPMV.T_DOCLG and DEPMV.T_DEPMV=DEPMVD.T_DEPMV and DEPMVD.T_LOT=LOT.T_LOT and DEPMV.C_DOINT=50 and BLVLG.C_TYPAR='DIV' and BLVET.NO_DOCET=@ParamNumDoc and ARTIC.T_ARTIC=@ParamTokenArt and LOT.C_LOT=@paramLot", cn);
        //            sCommandCdeBl.Parameters.Add(ParamNumDoc);
        //            sCommandCdeBl.Parameters.Add(ParamLot);
        //            sCommandCdeBl.Parameters.Add(ParamTokenArt);
        //            break;

        //        case 2: //Requete de recherche de l'article désigné par son gtin dans les articles de la commande
        //            sCommandCdeBl = new SqlCommand("select COALESCE(sum(qte_us),0) from CDEET,ARTIC,CDELG where CDELG.T_ARTIC=ARTIC.T_ARTIC and CDELG.T_DOCET=CDEET.T_DOCET and CDELG.C_TYPAR='DIV' and CDEET.NO_DOCET=@ParamNumDoc and ARTIC.T_ARTIC=@ParamTokenArt", cn);
        //            sCommandCdeBl.Parameters.Add(ParamTokenArt);
        //            sCommandCdeBl.Parameters.Add(ParamNumDoc);
        //            break;
        //    }

        //    double resCdeBl = (double)sCommandCdeBl.ExecuteScalar();


        //    if (resSophyScan == resCdeBl)
        //    {
        //        this.console.Text += "\r\n" + "nb article depassé";
        //        this.VideChamps();
        //        return 1;
        //    }
        //    else
        //    {
        //        this.console.Text += "\r\n" + "nb article ok";
        //        return 0;
        //    }

        //}

        ////Insertion d'un article
        //private void Insert_Article(SqlConnection cn, String tokenArt)
        //{
        //    SqlCommand sCommand = null;
        //    //Paramètre sql, stock le numero de la commande/Bl saisi
        //   /* SqlParameter ParamNumDoc = new SqlParameter("@ParamNumDoc", SqlDbType.Int);
        //    ParamNumDoc.Value = Int32.Parse(this.commande.Text);*/

        //    //Paramètre sql, stock le numéro système de l'article saisi
        //    SqlParameter ParamTokenArt = new SqlParameter("@ParamTokenArt", SqlDbType.Int);
        //    ParamTokenArt.Value = Int32.Parse(tokenArt);

        //    //Paramètre sql, stock le numéro de lot saisi
        //    SqlParameter ParamLot = new SqlParameter("@ParamLot", SqlDbType.Char, this.lot.Text.Length);
        //    ParamLot.Value = this.lot.Text;


        //    //Paramètre sql, stock la quantité saisie
        //    SqlParameter ParamQte = new SqlParameter("@ParamQte", SqlDbType.Int);
        //    ParamQte.Value = Int32.Parse(this.quantite.Text);
         
           


        //    sCommand = new SqlCommand("insert into SOPHYSCANCDE values (@ParamNumDoc,@ParamTokenArt,@paramLot,@paramQte)", cn);
        //    sCommand.Parameters.Add(ParamNumDoc);
        //    sCommand.Parameters.Add(ParamLot);
        //    sCommand.Parameters.Add(ParamQte);
        //    sCommand.Parameters.Add(ParamTokenArt);
        //    sCommand.ExecuteNonQuery();

        //    this.console.Text += "\r\n\r\n" + "Ajout Article effectué :" + "\r\n" + this.codeArt.Text + "\r\n" + this.lot.Text + "\r\n" + this.datePer.Text + "\r\n" + this.quantite.Text;
        //    this.VideChamps();



        //}

        ////1 si l'article appartient à la table sophyscancde,lot, 0 sinon
        //private int Verif_Article_Table(SqlConnection cn, String tokenArt)
        //{
        //    SqlCommand sCommand = null;
            

        //    //Paramètre sql, stock le numéro système de l'article saisi
        //    SqlParameter ParamTokenArt = new SqlParameter("@ParamTokenArt", SqlDbType.Int);
        //    ParamTokenArt.Value = Int32.Parse(tokenArt);

        //    //Paramètre sql, stock le numéro de lot saisi
        //    SqlParameter ParamLot = new SqlParameter("@ParamLot", SqlDbType.Char, this.lot.Text.Length);
        //    ParamLot.Value = this.lot.Text;


        //    //Paramètre sql, stock la quantité saisie
        //    SqlParameter ParamQte = new SqlParameter("@ParamQte", SqlDbType.Int);
        //    if (this.quantite.Text != "")
        //    {
        //        ParamQte.Value = Int32.Parse(this.quantite.Text);
        //    }
        //    else
        //    {
        //        ParamQte.Value = 1;
        //    }


        //    sCommand = new SqlCommand("select * from SOPHYSCANCDE where NO_DOCET=@ParamNumDoc and T_ARTIC=@ParamTokenArt and C_LOT=@paramLot and QTE_US=@paramQte", cn);
        //    sCommand.Parameters.Add(ParamNumDoc);
        //    sCommand.Parameters.Add(ParamLot);
        //    sCommand.Parameters.Add(ParamQte);
        //    sCommand.Parameters.Add(ParamTokenArt);

        //    SqlDataReader sqlReader = sCommand.ExecuteReader();


        //    if (sqlReader.Read())
        //    {

        //        this.console.Text += "\r\n" + "article enregistré";
        //        sqlReader.Close();
        //        return 1;

        //    }
        //    else
        //    {
        //        this.console.Text += "\r\n" + "Cet article n'est pas enregistré";
        //        sqlReader.Close();
        //        this.VideChamps();
        //        return 0;
        //    }

        //}

        ////1 si l'élément article,lot, 0 sinon
        //private void Supprime_Article_Table(SqlConnection cn, String tokenArt)
        //{
        //    SqlCommand sCommandTok = null;
        //    SqlCommand sCommandDel = null;
        //    //Paramètre sql, stock le numero de la commande/Bl saisi
        //    SqlParameter ParamNumDoc = new SqlParameter("@ParamNumDoc", SqlDbType.Int);
        //    ParamNumDoc.Value = Int32.Parse(this.commande.Text);

        //    //Paramètre sql, stock le numéro système de l'article saisi
        //    SqlParameter ParamTokenArt = new SqlParameter("@ParamTokenArt", SqlDbType.Int);
        //    ParamTokenArt.Value = Int32.Parse(tokenArt);

        //    //Paramètre sql, stock le numéro de lot saisi
        //    SqlParameter ParamLot = new SqlParameter("@ParamLot", SqlDbType.Char, this.lot.Text.Length);
        //    ParamLot.Value = this.lot.Text;


        //    //Paramètre sql, stock la quantité saisie
        //    SqlParameter ParamQte = new SqlParameter("@ParamQte", SqlDbType.Int);
        //    if (this.quantite.Text != "")
        //    {
        //        ParamQte.Value = Int32.Parse(this.quantite.Text);
        //    }
        //    else
        //    {
        //        ParamQte.Value = 1;
        //    }

        //    //Recuperation des lignes correspondant à la saisie commande/article/lot/qte
        //    sCommandTok = new SqlCommand("select TOK from SOPHYSCANCDE where NO_DOCET=@ParamNumDoc and T_ARTIC=@ParamTokenArt and C_LOT=@paramLot and QTE_US=@paramQte", cn);
        //    sCommandTok.Parameters.Add(ParamNumDoc);
        //    sCommandTok.Parameters.Add(ParamLot);
        //    sCommandTok.Parameters.Add(ParamQte);
        //    sCommandTok.Parameters.Add(ParamTokenArt);

        //    SqlDataReader sqlReader = sCommandTok.ExecuteReader();
        //    sqlReader.Read();
        //    String tok = sqlReader.GetValue(0).ToString();

        //    sCommandTok.Parameters.Clear();
        //    sqlReader.Close();

        //    SqlParameter ParamTokenLigne = new SqlParameter("@ParamTokenLigne", SqlDbType.Int);
        //    ParamTokenLigne.Value = Int32.Parse(tok);


        //    sCommandDel = new SqlCommand("delete from SOPHYSCANCDE where NO_DOCET=@ParamNumDoc and T_ARTIC=@ParamTokenArt and C_LOT=@paramLot and QTE_US=@paramQte and TOK=@ParamTokenLigne", cn);
        //    sCommandDel.Parameters.Add(ParamNumDoc);
        //    sCommandDel.Parameters.Add(ParamLot);
        //    sCommandDel.Parameters.Add(ParamQte);
        //    sCommandDel.Parameters.Add(ParamTokenArt);
        //    sCommandDel.Parameters.Add(ParamTokenLigne);
        //    sCommandDel.ExecuteNonQuery();

        //    this.console.Text += "\r\n\r\n" + "Suppression Article effectué :" + "\r\n" + this.commande.Text + "\r\n" + this.codeArt.Text + "\r\n" + this.lot.Text + "\r\n" + this.datePer.Text + "\r\n" + this.quantite.Text;
        //    this.VideChamps();


        //}


        //private String Recup_Token_Article(SqlConnection cn, String gtin)
        //{

        //    //Parametre sql correspondant au gtin saisi
        //    SqlParameter ParamGtin = new SqlParameter("@ParamGtin", SqlDbType.Char, 13);
        //    ParamGtin.Value = gtin;



        //    //requete de recupération du code de l'article via son gtin
        //    SqlCommand sCommand = new SqlCommand("select T_ARTIC from ARTIC where C_EANAR=@ParamGtin", cn);
        //    sCommand.Parameters.Add(ParamGtin);

        //    String tokenArt = sCommand.ExecuteScalar().ToString();
        //    return tokenArt;
        //}
        /*
                protected override void OnPaint(PaintEventArgs e)
        {
             Graphics gxOff = null;   //Offscreen graphics

             try
             {
                 if (bmpOffscreen == null)
                 {
                     bmpOffscreen = new Bitmap(this.Width, this.Height);
                 }

                 gxOff = Graphics.FromImage(bmpOffscreen);

                 gxOff.DrawImage(backgroundImage, 0, 0);

                 e.Graphics.DrawImage(bmpOffscreen, 0, 0);

                 base.OnPaint(e);
             }
             catch (Exception ex)
             {
                   throw ex;
             }
             finally
             {
                 gxOff.Dispose();
                 gxOff = null;
                 bmpOffscreen.Dispose();
                 bmpOffscreen = null;
             }
        }

        */
    }

}

  
 


// ------------------------------------------------------
// Override OnPaint() Event.
//------------------------------------------------------

