using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ControlsTestApp
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
//            Application.Run(new Form2());
//            Application.Run(new ImageMapForm());
            Application.Run(new MultiRegionsSelectForm());
        }
    }
}
