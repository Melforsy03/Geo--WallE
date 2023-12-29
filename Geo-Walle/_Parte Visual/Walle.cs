using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexer;
using Tokenizador;
using Jerarquia;
using Arbol;
using Geo_Walle;
using System.Windows.Documents;

namespace Geo_Walle
{
    public static class Walle
    {
        public static event Action<Figura, string> Drawing;
        public static void InvoKEvent(Figura figura,string color)
        {
            Drawing.Invoke(figura, color);
        }

    }
}
