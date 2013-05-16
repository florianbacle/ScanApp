using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gestion_Ste
{
    static class Program
    {
        /// <summary> 
        /// Point d'entrée principal de l'application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Application.Run(new Fenetre_Menu());
        }
    }
}