using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace SmartDeviceProject3
{
    public class DataAccess
    {



    }
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [MTAThread]
        static void Main()
        {

            Application.Run(new FenetreMenu(""));


        }
    }
}