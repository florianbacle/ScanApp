using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using datalogic.device;
using datalogic.datacapture;
using datalogic.pdc;
using System.IO;

namespace SmartDeviceProject3{

    class FenetreArticle : Form
    {

        //Gestion des champs à taille variable
        private int cptChamp;
        //Stockage du mode
        private int mode;

        private Label l_Ref;
        private TextBox tb_NumRef;

        private Label l_Art;
        private TextBox tb_CodeArt;

        private String s_Lot;
        private String s_DatePer;

        private Label l_Quantite;
        private TextBox tb_Quantite;

        private TextBox tb_Console;
        private Button b_Retour;
        private Button b_Quitter;

       
        
        public FenetreArticle(int mode)
        {
            
            this.cptChamp = 0;
            this.mode = mode;
            this.BackColor = Color.FromArgb(255, 255, 255);

            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.KeyPreview = true;
            this.Name = "Gestion des commandes";
            this.ResumeLayout(false);

            this.l_Ref = new Label();
            this.l_Ref.Text = "Référence :";
            this.l_Ref.Location = new Point(5, 10);
            this.l_Ref.Size = new System.Drawing.Size(80, 20);
            this.l_Ref.Font = new Font("Verdana", 9, System.Drawing.FontStyle.Bold);

            this.tb_NumRef = new TextBox();
            this.tb_NumRef.Text = null;
            this.tb_NumRef.Location = new Point(95, 10);
            this.tb_NumRef.Size = new System.Drawing.Size(100, 20);
            this.tb_NumRef.MaxLength = 8;
            this.tb_NumRef.KeyDown += new System.Windows.Forms.KeyEventHandler(tb_NumRef_KeyDown);
            this.tb_NumRef.BackColor = Color.FromArgb(150, 218, 177);

            this.l_Art = new Label();
            this.l_Art.Text = "Article :";
            this.l_Art.Location = new Point(5, 35);
            this.l_Art.Size = new System.Drawing.Size(50, 20);
            this.l_Art.Font = new Font("Verdana", 9, System.Drawing.FontStyle.Bold);

            this.tb_CodeArt = new TextBox();
            this.tb_CodeArt.Text = null;
            this.tb_CodeArt.Location = new Point(95, 35);
            this.tb_CodeArt.KeyDown += new System.Windows.Forms.KeyEventHandler(tb_CodeArt_KeyDown);
            this.tb_CodeArt.MaxLength = 100;
            this.tb_CodeArt.Size = new System.Drawing.Size(140, 20);
            this.tb_CodeArt.BackColor = Color.FromArgb(150, 218, 177);

            this.s_Lot = null;
            this.s_DatePer =null;
          
            this.l_Quantite = new Label();
            this.l_Quantite.Text = "Quantité :";
            this.l_Quantite.Location = new Point(5, 60);
            this.l_Quantite.Size = new System.Drawing.Size(65, 20);
            this.l_Quantite.Font = new Font("Verdana", 9, System.Drawing.FontStyle.Bold);

            this.tb_Quantite = new TextBox();
            this.tb_Quantite.Text = null;
            this.tb_Quantite.Location = new Point(95, 60);
            this.tb_Quantite.MaxLength = 6;
            this.tb_Quantite.Size = new System.Drawing.Size(100, 20);
            this.tb_Quantite.BackColor = Color.FromArgb(150, 218, 177);

            this.tb_Console = new TextBox();
            this.tb_Console.Text = null;
            this.tb_Console.Location = new Point(5, 85);
            this.tb_Console.Multiline = true;
            this.tb_Console.ScrollBars = ScrollBars.Vertical;
            this.tb_Console.Size = new System.Drawing.Size(230, 160);
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
            this.Controls.Add(l_Ref);
            this.Controls.Add(tb_NumRef);

            this.Controls.Add(l_Art);
            this.Controls.Add(tb_CodeArt);

            this.Controls.Add(l_Quantite);
            this.Controls.Add(tb_Quantite);

            this.Controls.Add(tb_Console);
            this.Controls.Add(b_Retour);
            this.Controls.Add(b_Quitter);

       
            //Affichage mode
            if (mode == 1)
            {
                this.tb_Console.Text += "-----------AJOUT D'ARTICLES-----------\r\n";
            }
            else
            {
                this.tb_Console.Text += "-------SUPPRESSION D'ARTICLES--------\r\n";
            }


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

        //Event sur la boite commande pour passer le focus
        private void tb_NumRef_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                   this.tb_CodeArt.Focus();
             
            }
        }

        private void tb_CodeArt_KeyDown(object sender, KeyEventArgs e)
        {

            //Controle de fin de lecture (Nessecite le caractrère <CR> en fin de code barre)
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {

               

                int lectureOk=0;
                int typeCode = 0;
                Char separator = ConfigurationManager.GetKey<char>("SeparatorCodeSophysa");
                if (this.tb_CodeArt.Text.Contains(separator))
                {
                    typeCode = 0;
                    lectureOk = LectureCodeBarreSophyCode(this.tb_CodeArt.Text,separator);
                }else{
                    typeCode = 1;
                    this.tb_CodeArt.Text += "#";
                    tb_CodeArt.SelectionStart = tb_CodeArt.TextLength;
                    lectureOk = LectureCodeBarreEan128(this.tb_CodeArt.Text);
                   
                }

                if (lectureOk == 1)
                {
                    if (this.tb_Quantite.Text == "")
                    {
                        this.tb_Quantite.Text = "1";
                    }
                    //Récuperation des configurations
                    string ip = ConfigurationManager.GetKey<string>("Ip");
                    string bd = ConfigurationManager.GetKey<string>("Bd");
                    string user = ConfigurationManager.GetKey<string>("User");

                    //This is your database connexion:
                    string connectionString = "Data Source=" + ip + ";Initial Catalog=" + bd + ";User ID=" + user;
                    SqlConnection cn = new SqlConnection(connectionString);

                    try
                    {

                        
                        int typeDoc = Recup_Format_Document(this.tb_NumRef.Text);
                        if (typeDoc != 0)
                        {
                            cn.Open();
                            if (Verif_Document(cn, typeDoc) == 1)
                            {
                                String tokenArt = Recup_CodeArticle(cn,typeCode);
                                if (tokenArt != null)
                                {
                                    if (Verif_Article_Document(cn, typeDoc, tokenArt) != 0)
                                    {
                                        //Differentiation des cas, ajout ou suppression
                                        int resNb = Verif_Nb_Article_Table(cn, typeDoc, tokenArt);
                                        if (this.mode == 1)
                                        {
                                            switch (resNb)
                                            {
                                                case 1: Insert_Article(cn, tokenArt); break;
                                                case 2: Update_Article(cn, tokenArt); break;
                                            }
                                        }
                                        else
                                        {
                                            switch (resNb)
                                            {
                                                case 1: Supprime_Article_Table(cn, tokenArt); break;
                                                case 2: Update_Article(cn, tokenArt); break;
                                            }
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
                            this.tb_Console.Text += "\r\n Error #" + i + ex.Errors[i].Message;
                        }
                        this.VideChamps();


                    }
                    finally
                    {
                        if (cn.State != ConnectionState.Closed)
                        {
                            cn.Close();
                        }
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
                this.tb_CodeArt.Text += "#";
                tb_CodeArt.SelectionStart = tb_CodeArt.TextLength;
                this.cptChamp = 0;
            }
            else
            {
                this.cptChamp = 0;
            }


        }


        /*0 pas ok, 1 sinon*/
        private int LectureCodeBarreEan128(String code)
        {
            if (code.Length > 1)
            {

                String Aim = code.Substring(0, 2);

                switch (Aim)
                {
                    case "01": this.tb_CodeArt.Text = code.Substring(2, 14); LectureCodeBarreEan128(code.Substring(16, code.Length - 16)); break;
                    case "17": this.s_DatePer = code.Substring(2, 6); LectureCodeBarreEan128(code.Substring(8, code.Length - 8)); break;

                    case "30": if (this.tb_Quantite.Text == "")
                        {
                            this.tb_Quantite.Text = code.Substring(2, code.IndexOf("#") - 2);
                        }
                        LectureCodeBarreEan128(code.Substring(code.IndexOf("#") + 1, code.Length - (code.IndexOf("#") + 1))); break;

                    case "10": this.s_Lot = code.Substring(2, code.IndexOf("#") - 2); LectureCodeBarreEan128(code.Substring(code.IndexOf("#") + 1, code.Length - (code.IndexOf("#") + 1))); break;

                    default: this.tb_Console.Text += "\r\n" + "identifiant inconnu :" + code; LectureCodeBarreEan128(""); this.VideChamps(); return 0;


                }
            }
            else if (code.Length == 1)
            {
                this.tb_Console.Text += "\r\n" + "symbole inconnu :" + code; this.VideChamps(); return 0;
            }

            return 1;

        }

        private int LectureCodeBarreSophyCode(String code,Char separator)
        {
            String reference = code.Substring(0, code.LastIndexOf(separator));
            String lot = code.Substring(code.LastIndexOf(separator) + 1);
            this.tb_CodeArt.Text = reference;
            this.s_Lot = lot;
            return 1;
        }

        private void VideChamps()
        {
            this.tb_CodeArt.Text = null;
            this.s_DatePer = null;
            this.s_Lot = null;
            this.tb_Quantite.Text = null;

            //Descente de la scrollbarre
            this.tb_Console.SelectionStart = this.tb_Console.Text.Length;
            this.tb_Console.ScrollToCaret();
          
            //Retour sur codeArt pour une nouvelle saisie
            this.tb_CodeArt.Focus();

         
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
            if (isDigit(numDoc) && isDigit(this.tb_Quantite.Text))
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
            this.VideChamps();
            this.tb_NumRef.Text = null;
            this.tb_NumRef.Focus();
            Beep(0);
            return 0;

       }
    
        //1 si le document existe, 0 sinon
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
                this.VideChamps();
                this.tb_NumRef.Text = null;
                this.tb_NumRef.Focus();
                Beep(0);
                return 0;
            }
            else
            {
                this.tb_Console.Text += "\r\n" + typeReference + " existe ";
                sqlReader.Close();
                return 1;
            }



        }

        //tokenart si code trouvé, null sinon
        private String Recup_CodeArticle(SqlConnection cn,int typeCode)
        {

            SqlCommand sCommand = null;
            String error=null;
            switch (typeCode)
            {
                case 1:
                    //Paramètre sql, stock le gtin de l'article saisi
                    SqlParameter ParamGtinArt = new SqlParameter("@ParamGtinArt", SqlDbType.Char, 14);
                    ParamGtinArt.Value = this.tb_CodeArt.Text;

                    //requete de recupération du code de l'article via son gtin
                    sCommand = new SqlCommand("select C_ARTIC,T_ARTIC from dbo.SophyEAN where EAN=@ParamGtinArt", cn);
                    sCommand.Parameters.Add(ParamGtinArt);

                    error = "gtin";
                    break;
                case 0:
                   
                    //Paramètre sql, stock le gtin de l'article saisi
                    SqlParameter ParamCodeArt = new SqlParameter("@ParamCodeArt", SqlDbType.Char, this.tb_CodeArt.Text.Length);
                    ParamCodeArt.Value = this.tb_CodeArt.Text;

                    //requete de recupération du code de l'article via son gtin
                    sCommand = new SqlCommand("select C_ARTIC,T_ARTIC from ARTIC where C_ARTIC=@ParamCodeArt", cn);
                    sCommand.Parameters.Add(ParamCodeArt);

                    error = "code";
                    break;

            }

           

            SqlDataReader sqlReader = sCommand.ExecuteReader();


            if (sqlReader.Read())
            {
                this.tb_CodeArt.Text = sqlReader.GetValue(0).ToString();
                String tokenArt = sqlReader.GetValue(1).ToString();
                this.tb_Console.Text += "\r\n" + "Ce GTIN correspond à un article";
                sqlReader.Close();
                return tokenArt;

            }
            else
            {
                this.tb_Console.Text += "\r\n" + error+" : "+this.tb_CodeArt.Text+" ne correspond à aucun article";
                sqlReader.Close();
                this.VideChamps();
                Beep(0);
                return null;
            }

        }

        //1 si article appartient à la commande/BL/(depot,date), 0 sinon
        private int Verif_Article_Document(SqlConnection cn, int type, String tokenArt)
        {   
            String typeReference = null;
            SqlCommand sCommand = null;

            //Paramètre sql, stock le numero de la commande/Bl saisi
            SqlParameter ParamNumDoc = new SqlParameter("@ParamNumDoc", SqlDbType.Int);
            ParamNumDoc.Value = Int32.Parse(this.tb_NumRef.Text);

            //Paramètre sql, stock le gtin de l'article saisi
            SqlParameter ParamTokenArt = new SqlParameter("@ParamTokenArt", SqlDbType.Char, 13);
            ParamTokenArt.Value = tokenArt;

            //Paramètre sql, stock le numéro de s_Lot saisi
            SqlParameter ParamLot = new SqlParameter("@ParamLot", SqlDbType.Char, this.s_Lot.Length);
            ParamLot.Value = this.s_Lot;

            switch (type)
            {
                case 1: //Requete de recherche de l'article désigné par son gtin dans les article du BL
                    sCommand = new SqlCommand("select ARTIC.T_ARTIC from BLVLG,ARTIC,BLVET,DEPMV,DEPMVD,LOT where BLVLG.T_ARTIC=ARTIC.T_ARTIC and BLVLG.T_DOCET=BLVET.T_DOCET and BLVLG.T_DOCLG=DEPMV.T_DOCLG and DEPMV.T_DEPMV=DEPMVD.T_DEPMV and DEPMVD.T_LOT=LOT.T_LOT and DEPMV.C_DOINT=50 and BLVLG.C_TYPAR='DIV' and BLVET.NO_DOCET=@ParamNumDoc and ARTIC.T_ARTIC=@ParamTokenArt and LOT.C_LOT=@paramLot", cn);
                    sCommand.Parameters.Add(ParamTokenArt);
                    sCommand.Parameters.Add(ParamNumDoc);
                    sCommand.Parameters.Add(ParamLot);
                   
                    typeReference = "du bl";

                    break;

                case 2: //Requete de recherche de l'article désigné par son gtin dans les articles de la commande et du lot dans le dépôt
                    sCommand = new SqlCommand(" select ARTIC.T_ARTIC from CDEET,ARTIC,CDELG where CDELG.T_ARTIC=ARTIC.T_ARTIC and CDELG.T_DOCET=CDEET.T_DOCET and CDELG.C_TYPAR='DIV' and CDEET.NO_DOCET=@ParamNumDoc and ARTIC.T_ARTIC=@ParamTokenArt", cn);
                    sCommand.Parameters.Add(ParamNumDoc);
                    sCommand.Parameters.Add(ParamTokenArt);
                    typeReference = " de la commande";

                    break;

                case 3:
                    //Paramètre sql stock le code depot saisi
                    SqlParameter ParamCodeDepot = new SqlParameter("@ParamCodeDepot", SqlDbType.Char,3);
                    ParamCodeDepot.Value = this.tb_NumRef.Text;

                    //Requete de recherche de l'article saisi parmis les articles appartenant au mouvement effectué à la date donnée
                    sCommand = new SqlCommand("select ARTIC.T_ARTIC from artic, depmv, depmvd, lot, depot where artic.t_artic = depmv.t_artic and depmv.t_depmv = depmvd.t_depmv and depmvd.t_lot = lot.t_lot and depmv.t_depot = depot.t_depot and DEPMV.QTE_US > 0  and datediff (dd,getdate(),DEPMV.DT_MVT)=0 and DEPMV.NAT_MVT = 13 and DEPOT.C_DEPOT = @ParamCodeDepot and ARTIC.T_ARTIC=@ParamTokenArt and LOT.C_LOT=@paramLot", cn);
                    sCommand.Parameters.Add(ParamTokenArt);
                    sCommand.Parameters.Add(ParamLot);
                    sCommand.Parameters.Add(ParamCodeDepot);

                    typeReference = "du dépôt";

                    break;
            }
            SqlDataReader sqlReader = sCommand.ExecuteReader();


            if (sqlReader.Read())
            {

                this.tb_Console.Text += "\r\n" + "Cet article fait parti"+ typeReference;
                sqlReader.Close();
                return 1;

            }
            else
            {
                this.tb_Console.Text += "\r\n" + "Cet article ne fait pas parti" + typeReference;
                sqlReader.Close();
                this.VideChamps();
                Beep(0);
                return 0;
            }

        }



        //0 si nb article depassé, 1 si nb article dans sophyscancde =0 , 2 sinon
        private int Verif_Nb_Article_Table(SqlConnection cn, int type, String TokenArt)
        {
            SqlCommand sCommandSophyScan = null;
            SqlCommand sCommandCdeBl = null;
          
            //Paramètre sql, stock le numero de la commande/Bl saisi
            SqlParameter ParamNumDoc = new SqlParameter("@ParamNumDoc", SqlDbType.Int);
            ParamNumDoc.Value = Int32.Parse(this.tb_NumRef.Text);

            //Paramètre sql, stock le gtin de l'article saisi
            SqlParameter ParamTokenArt = new SqlParameter("@ParamTokenArt", SqlDbType.Int);
            ParamTokenArt.Value = TokenArt;

            //Paramètre sql, stock le numéro de lot saisi
            SqlParameter ParamLot = new SqlParameter("@ParamLot", SqlDbType.Char, this.s_Lot.Length);
            ParamLot.Value = this.s_Lot;

   
            sCommandSophyScan = new SqlCommand("select ISNULL(sum(qte_us),0) from SOPHYSCANCDE where NO_DOCET=@ParamNumDoc and T_ARTIC=@ParamTokenArt and C_LOT=@paramLot", cn);
            sCommandSophyScan.Parameters.Add(ParamTokenArt);
            sCommandSophyScan.Parameters.Add(ParamNumDoc);
            sCommandSophyScan.Parameters.Add(ParamLot);
            

            double resSophyScan = (double)sCommandSophyScan.ExecuteScalar();
            sCommandSophyScan.Parameters.Clear();

            switch (type)
            {
                case 1: //Requete d'addition des quantités de tous les articles du lot et du BL désigné dans SOPHYSCANCDE
                    sCommandCdeBl = new SqlCommand("select ISNULL(DEPMVD.qte_us,0) from BLVLG,ARTIC,BLVET,DEPMV,DEPMVD,LOT where BLVLG.T_ARTIC=ARTIC.T_ARTIC and BLVLG.T_DOCET=BLVET.T_DOCET and BLVLG.T_DOCLG=DEPMV.T_DOCLG and DEPMV.T_DEPMV=DEPMVD.T_DEPMV and DEPMVD.T_LOT=LOT.T_LOT and DEPMV.C_DOINT=50 and BLVLG.C_TYPAR='DIV' and BLVET.NO_DOCET=@ParamNumDoc and ARTIC.T_ARTIC=@ParamTokenArt and LOT.C_LOT=@paramLot", cn);
                    sCommandCdeBl.Parameters.Add(ParamNumDoc);
                    sCommandCdeBl.Parameters.Add(ParamLot);
                    sCommandCdeBl.Parameters.Add(ParamTokenArt);
                    break;

                case 2: //Requete de recherche de l'article désigné par son gtin dans les articles de la commande
                    sCommandCdeBl = new SqlCommand("select distinct ISNULL(depstd.qte_us,0) from cdelg,cdeet,depstd,lot where lot.t_lot=depstd.t_lot  and cdeet.t_depot_liv=depstd.t_depot and no_docet=@ParamNumDoc and depstd.t_artic=@ParamTokenArt and lot.c_lot=@ParamLot", cn);
                    sCommandCdeBl.Parameters.Add(ParamTokenArt);
                    sCommandCdeBl.Parameters.Add(ParamLot);
                    sCommandCdeBl.Parameters.Add(ParamNumDoc);
                    break;

                case 3:
                    //Paramètre sql stock le code depot saisi
                    SqlParameter ParamCodeDepot = new SqlParameter("@ParamCodeDepot", SqlDbType.Char, 3);
                    ParamCodeDepot.Value = this.tb_NumRef.Text;

                    sCommandCdeBl = new SqlCommand("select ISNULL(depmvd.qte_us),0)from artic, depmv, depmvd, lot, depot where artic.t_artic = depmv.t_artic and depmv.t_depmv = depmvd.t_depmv and depmvd.t_lot = lot.t_lot and depmv.t_depot = depot.t_depot and DEPMV.QTE_US > 0  and datediff(dd,getdate(),DEPMV.DT_MVT)=0 and DEPMV.NAT_MVT = 13 and DEPOT.C_DEPOT = @ParamCodeDepot and ARTIC.T_ARTIC=@ParamTokenArt and LOT.C_LOT=@paramLot", cn);
                    sCommandCdeBl.Parameters.Add(ParamCodeDepot);
                    sCommandCdeBl.Parameters.Add(ParamLot);
                    sCommandCdeBl.Parameters.Add(ParamTokenArt);
                    break;
            }

            double resCdeBl = (double)sCommandCdeBl.ExecuteScalar();
            sCommandCdeBl.Parameters.Clear();

            //Differenciation des cas ajout/suppresion
            switch (this.mode)
            {

                case 1: if ((resSophyScan + Int32.Parse(this.tb_Quantite.Text)) > resCdeBl)
                    {
                        this.tb_Console.Text += "\r\n" + "quantité depassée";
                        this.VideChamps();
                        Beep(0);
                        return 0;
                    }
                    else
                    {
                        this.tb_Console.Text += "\r\n" + "quantité commande ok";

                        if (resSophyScan == 0)
                        {
                           return 1;
                        }
                        return 2;
                    }
                  

                case 2: if ((resSophyScan - Int32.Parse(this.tb_Quantite.Text)) < 0)
                    {
                        this.tb_Console.Text += "\r\n" + "quantité article à supprimer invalide";
                        this.VideChamps();
                        return 0;
                    }
                    else
                    {
                        if (resSophyScan - Int32.Parse(this.tb_Quantite.Text) == 0)
                        {
                            return 1;
                        }
                        return 2;
                    }
            }
            return 0;

        }

        //Insertion d'un article
        private void Insert_Article(SqlConnection cn, String tokenArt)
        {
            SqlCommand sCommand = null;
          

            //Paramètre sql, stock le numéro système de l'article saisi
            SqlParameter ParamTokenArt = new SqlParameter("@ParamTokenArt", SqlDbType.Int);
            ParamTokenArt.Value = Int32.Parse(tokenArt);

            //Paramètre sql, stock le numéro de lot saisi
            SqlParameter ParamLot = new SqlParameter("@ParamLot", SqlDbType.Char, this.s_Lot.Length);
            ParamLot.Value = this.s_Lot;

            //Paramètre sql, stock la quantité saisie
            SqlParameter ParamQte = new SqlParameter("@ParamQte", SqlDbType.Int);
            ParamQte.Value = Int32.Parse(this.tb_Quantite.Text);

            SqlParameter ParamNumDoc = null;
            //Paramètre sql, stock le numero de la commande/Bl saisi
            ParamNumDoc = new SqlParameter("@ParamNumDoc", SqlDbType.Int);
            ParamNumDoc.Value = Int32.Parse(this.tb_NumRef.Text);
         
        

            sCommand = new SqlCommand("insert into SOPHYSCANCDE values (@ParamNumDoc,@ParamTokenArt,@paramLot,@paramQte,GETDATE())", cn);
            sCommand.Parameters.Add(ParamNumDoc);
            sCommand.Parameters.Add(ParamLot);
            sCommand.Parameters.Add(ParamQte);
            sCommand.Parameters.Add(ParamTokenArt);
            sCommand.ExecuteNonQuery();

            this.tb_Console.Text += "\r\n\r\n" + "Ajout Article effectué :" + "\r\n" + this.tb_CodeArt.Text + "\r\n" + this.s_Lot + "\r\n" + this.s_DatePer + "\r\n" + this.tb_Quantite.Text;
            this.VideChamps();
            //Beep(1);
        }

        //Update d'une ligne, tyoe_update 1=ajout other=suppresion
        private void Update_Article(SqlConnection cn, String tokenArt)
        {
            SqlCommand sCommand = null;
          
            //Paramètre sql, stock le numéro système de l'article saisi
            SqlParameter ParamTokenArt = new SqlParameter("@ParamTokenArt", SqlDbType.Int);
            ParamTokenArt.Value = Int32.Parse(tokenArt);

            //Paramètre sql, stock le numéro de lot saisi
            SqlParameter ParamLot = new SqlParameter("@ParamLot", SqlDbType.Char, this.s_Lot.Length);
            ParamLot.Value = this.s_Lot;

            //Paramètre sql, stock la quantité saisie
            SqlParameter ParamQte = new SqlParameter("@ParamQte", SqlDbType.Int);
            ParamQte.Value = Int32.Parse(this.tb_Quantite.Text);

           
            //Paramètre sql stock le code depot saisi
            //Paramètre sql, stock le numero de la commande/Bl saisi
            SqlParameter ParamNumDoc = new SqlParameter("@ParamNumDoc", SqlDbType.Int);
            ParamNumDoc.Value = Int32.Parse(this.tb_NumRef.Text);


            if (this.mode == 1)
            {
                sCommand = new SqlCommand("update SOPHYSCANCDE set QTE_US=QTE_US+@paramQte where NO_DOCET=@ParamNumDoc and T_ARTIC=@ParamTokenArt and C_LOT=@paramLot", cn);
            }
            else
            {
                sCommand = new SqlCommand("update SOPHYSCANCDE set QTE_US=QTE_US-@paramQte where NO_DOCET=@ParamNumDoc and T_ARTIC=@ParamTokenArt and C_LOT=@paramLot", cn);
            }
            sCommand.Parameters.Add(ParamNumDoc);
            sCommand.Parameters.Add(ParamLot);
            sCommand.Parameters.Add(ParamQte);
            sCommand.Parameters.Add(ParamTokenArt);
            sCommand.ExecuteNonQuery();

            this.tb_Console.Text += "\r\n\r\n" + "update Article effectué :" + "\r\n" + this.tb_CodeArt.Text + "\r\n" + this.s_Lot + "\r\n" + this.s_DatePer+ "\r\n" + this.tb_Quantite.Text;
            this.VideChamps();
           // Beep(1);
        }

        

        //1 si l'élément article,lot, 0 sinon
        private void Supprime_Article_Table(SqlConnection cn, String tokenArt)
        {

            SqlCommand sCommandDel=null;

            //Paramètre sql, stock le numero de la commande/Bl saisi
            SqlParameter ParamNumDoc = new SqlParameter("@ParamNumDoc", SqlDbType.Int);
            ParamNumDoc.Value = Int32.Parse(this.tb_NumRef.Text);

            //Paramètre sql, stock le numéro système de l'article saisi
            SqlParameter ParamTokenArt = new SqlParameter("@ParamTokenArt", SqlDbType.Int);
            ParamTokenArt.Value = Int32.Parse(tokenArt);

            //Paramètre sql, stock le numéro de lot saisi
            SqlParameter ParamLot = new SqlParameter("@ParamLot", SqlDbType.Char, this.s_Lot.Length);
            ParamLot.Value = this.s_Lot;

            //Paramètre sql, stock la quantité saisie
            SqlParameter ParamQte = new SqlParameter("@ParamQte", SqlDbType.Int);
            ParamQte.Value = Int32.Parse(this.tb_Quantite.Text);

      
            sCommandDel = new SqlCommand("delete from SOPHYSCANCDE where NO_DOCET=@ParamNumDoc and T_ARTIC=@ParamTokenArt and C_LOT=@paramLot and QTE_US=@paramQte", cn);
  
          
            sCommandDel.Parameters.Add(ParamNumDoc);
            sCommandDel.Parameters.Add(ParamLot);
            sCommandDel.Parameters.Add(ParamQte);
            sCommandDel.Parameters.Add(ParamTokenArt);
            sCommandDel.ExecuteNonQuery();

            this.tb_Console.Text += "\r\n\r\n" + "Suppression Article effectué :" + "\r\n" + this.tb_NumRef.Text + "\r\n" + this.tb_CodeArt.Text + "\r\n" + this.s_Lot + "\r\n" + this.s_DatePer + "\r\n" + this.tb_Quantite.Text;
            this.VideChamps();
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

        public void Beep(int type)
        {
            String path = null;
            if (type == 0)
            {
                path = ConfigurationManager.GetKey<String>("PathBeepError");
            }
            else
            {
                path = ConfigurationManager.GetKey<String>("PathBeepOk");
            }
            if (File.Exists(path))
            {
                Sound sound = new Sound(path);
                sound.Play();
            }
        }
    }

}

