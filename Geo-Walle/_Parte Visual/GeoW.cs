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

namespace Geo_Walle
{
    public partial class GeoW : Form
    {
        //Codigo de entrada del usuario.
        string impud;
        //errores del evaluador que debo imprimir en la pantalla.
        List<Errors> errores;
        // dividi la lista de figuras en cutro segun el tipo de figura que era cada objeto

        // Listas de las figuras que ya estan dibujadas en el lienzo.
        List<Linea> Lineas_Dibujadas;
        List<Rayo> Rayos_Dibujados;
        List<Segmento> Segmentos_Dibujados;
        List<PointP> Points_Dibujados;
        List<Circulo> Circulos_Dibujados;
        List<Arco> Arcos_Dibujados;

        public GeoW()
        {
            InitializeComponent();
            Walle.Drawing += Pintar_Figura;

            Lineas_Dibujadas = new List<Linea>();
            Circulos_Dibujados = new List<Circulo>();
            Rayos_Dibujados = new List<Rayo>();
            Segmentos_Dibujados = new List<Segmento>();
            Points_Dibujados = new List<PointP>();
            Arcos_Dibujados = new List<Arco>();
            errores = new List<Errors>();
        }

        private void btn_volver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void PaintPoint(PointP point, bool esta)
        {
            Graphics lienzo = this.Lienzo.CreateGraphics();

            if (!esta)
                Points_Dibujados.Add(new PointP("", point.x, point.y));

            lienzo.FillEllipse(Brushes.Black, point.x, point.y, 5, 5);
        }
        public void PaintLine(PointP point1, PointP point2, string color, bool esta)
        {
            Color FigColor = Color_de_la_Figura(color);
            Graphics lienzo = Lienzo.CreateGraphics();
            if (!esta)
            {
                Lineas_Dibujadas.Add(new Linea(new PointP("", point1.x, point1.y), new PointP("", point2.x, point2.y)));
                esta = true;
            }

            PaintPoint(point1, esta);
            PaintPoint(point2, esta);

            float pendiente_m = (float)(point1.y - point2.y) / (float)(point1.x - point2.x);
            PointP interseccion_izq = new PointP("p1", 0, (int)(point2.y + (0 - point2.x) * pendiente_m));
            PointP interseccion_der = new PointP("p2", this.Width - 1, (int)(point2.y + ((this.Width - 1) - point2.x) * pendiente_m));
            PointP interseccion_Top = new PointP("p1", (int)(point2.x + (0 - point2.y) / pendiente_m), 0);
            PointP interseccion_Dwn = new PointP("p2", (int)(point2.x + ((this.Height - 1) - point2.y) / pendiente_m), this.Height - 1);

            lienzo.DrawLine(new Pen(FigColor), interseccion_izq.x + 2, interseccion_izq.y + 2, interseccion_der.x + 2, interseccion_der.y + 2);
            lienzo.DrawLine(new Pen(FigColor), interseccion_Top.x + 2, interseccion_Top.y + 2, interseccion_Dwn.x + 2, interseccion_Dwn.y + 2);

        }
        public void PaintRay(PointP point1, PointP point2, string color, bool esta)
        {
            Color FigColor = Color_de_la_Figura("black");
            Graphics lienzo = Lienzo.CreateGraphics();
            if (!esta)
            {
                Rayos_Dibujados.Add(new Rayo(new PointP("", point1.x, point1.y), new PointP("", point2.x, point2.y)));
                esta = true;
            }

            PaintPoint(point1, esta);
            PaintPoint(point2, esta);

            //y = mx + b
            float pendiente_m = (float)(point1.y - point2.y) / (float)(point1.x - point2.x);
            float b = point2.y - pendiente_m * point2.x;

            float interseccionX_izq = 0;
            float interseccionY_izq = pendiente_m;
            PointP interseccion_Izq = new PointP("id", (int)interseccionX_izq, (int)interseccionY_izq);

            float interseccionX_der = this.Width - 1;
            float interseccionY_der = pendiente_m * interseccionX_der + b;
            PointP interseccion_Der = new PointP("id", (int)interseccionX_der, (int)interseccionY_der);

            lienzo.DrawLine(new Pen(FigColor), point1.x + 2, point1.y + 2, point2.x + 2, point2.y + 2);
            lienzo.DrawLine(new Pen(FigColor), point2.x + 2, point2.y + 2, interseccion_Der.x, interseccion_Der.y);
        }
        public void PaintSegment(PointP point1, PointP point2, string color, bool esta)
        {
            Color FigColor = Color_de_la_Figura("black");
            Graphics lienzo = Lienzo.CreateGraphics();
            if (!esta)
            {
                Segmentos_Dibujados.Add(new Segmento(new PointP("", point1.x, point1.y), new PointP("", point2.x, point2.y)));
                esta = true;
            }

            PaintPoint(point1, esta);
            PaintPoint(point2, esta);

            lienzo.DrawLine(new Pen(FigColor), point1.x + 2, point1.y + 2, point2.x + 2, point2.y + 2);
        }
        public void PaintCirc(PointP point1, PointP point2, string color, int media, bool esta)
        {
            Color FigColor = Color_de_la_Figura("black");
            Graphics lienzo = Lienzo.CreateGraphics();
            if (!esta)
            {
                Circulos_Dibujados.Add(new Circulo(new PointP("", point1.x, point1.y), new PointP("", point2.x, point2.y), media));
                esta = true;
            }
            PaintPoint(point1, esta);
            //PaintPoint(point2, esta);
            int radio = media;

            Rectangle circle = new Rectangle(point1.x - radio, point1.y - radio, radio * 2, radio * 2);
            lienzo.DrawEllipse(new Pen(FigColor), circle);
        }
        public void PaintArc(PointP centro, PointP point1, PointP point2, string color, int media, bool esta)
        {
            Color FigColor = Color_de_la_Figura("black");
            Graphics lienzo = Lienzo.CreateGraphics();
            if (!esta)
            {
                Arcos_Dibujados.Add(new Arco(new PointP("", centro.x, centro.y), new PointP("", point1.x, point1.y), new PointP("", point2.x, point2.y), media));
                esta = true;
            }
            PaintPoint(point1, esta);
            PaintPoint(point2, esta);
            PaintPoint(centro, esta);

            int radio = media;
            float pendiente_AC = (float)(point2.y - centro.y) / (float)(point2.x - centro.x);
            float pendiente_AB = (float)(point1.y - centro.y) / (float)(point1.x - centro.x);

            float angulo_AC = Angulo_de_la_Pendiente(centro, point2, pendiente_AC);
            float angulo_AB = Angulo_de_la_Pendiente(centro, point1, pendiente_AB);
            float angula_AC_AB = 0;

            if (angulo_AC > 0 && angulo_AB > 0)
                angula_AC_AB = angulo_AB > angulo_AC ? 360 - (angulo_AB - angulo_AC) : angulo_AC - angulo_AB;

            if (angulo_AC < 0 && angulo_AB < 0)
                angula_AC_AB = angulo_AB > angulo_AC ? angulo_AB - angulo_AC : 360 - (angulo_AC - angulo_AB);

            if (angulo_AB < 0 && angulo_AC > 0) angula_AC_AB = 360 - (angulo_AC - angulo_AB);

            if (angulo_AB > 0 && angulo_AC < 0) angula_AC_AB = angulo_AB - angulo_AC;

            Rectangle arc = new Rectangle(centro.x - radio, centro.y - radio, radio * 2, radio * 2);
            lienzo.DrawArc(new Pen(FigColor), arc, angulo_AC, angula_AC_AB);
        }

        private Color Color_de_la_Figura(string color)
        {
            Color color1 = new Color();

            switch (color)
            {
                case "green":
                    color1 = Color.Green;
                    break;
                case "blue":
                    color1 = Color.Blue;
                    break;
                case "red":
                    color1 = Color.Red;
                    break;
                case "yellow":
                    color1 = Color.Yellow;
                    break;
                case "cyan":
                    color1 = Color.Cyan;
                    break;
                case "magenta":
                    color1 = Color.Magenta;
                    break;
                case "white":
                    color1 = Color.White;
                    break;
                case "gray":
                    color1 = Color.Gray;
                    break;
                case "black":
                    color1 = Color.Black;
                    break;
                default:
                    System.Console.WriteLine("opcion no valida");
                    break;
            }

            return color1;
        }
        private float Angulo_de_la_Pendiente(PointP centro, PointP point, float pendiente_m)
        {
            float angulo1 = (float)Math.Atan(pendiente_m);
            float angulo_grados = (float)(angulo1 * 180 / Math.PI);
            float alpha = 0;

            if (pendiente_m >= 0)
                alpha = (centro.y > point.y) ? -(180 - angulo_grados) : angulo_grados;

            if (pendiente_m < 0)
                alpha = (centro.y > point.y) ? angulo_grados : 180 + angulo_grados;
            

            return alpha;
        }
        private void Btn_reset_Click(object sender, EventArgs e)
        {
            Reset(sender, e);
            txtBox_codigo.Clear();
            Error_Box.Clear();
        }
        private void Reset(object sender, EventArgs e)
        {
            Graphics lienzo = Lienzo.CreateGraphics();
            Segmentos_Dibujados = new List<Segmento>();
            Lineas_Dibujadas = new List<Linea>();
            Rayos_Dibujados = new List<Rayo>();
            Points_Dibujados = new List<PointP>();
            Circulos_Dibujados = new List<Circulo>();
            Arcos_Dibujados = new List<Arco>();
            lienzo.Clear(Color.Gray);
        }
        private void btn_arriba_Click(object sender, EventArgs e)
        {
            Limpiar_Lianzo();
            Traslate(0, 10);
        }
        private void btn_abajo_Click(object sender, EventArgs e)
        {
            Limpiar_Lianzo();
            Traslate(0, -10);
        }
        private void btn_derecha_Click(object sender, EventArgs e)
        {
            Limpiar_Lianzo();
            Traslate(-10, 0);
        }
        private void btn_izquierda_Click(object sender, EventArgs e)
        {
            Limpiar_Lianzo();
            Traslate(10, 0);
        }
        private void Traslate(int x, int y)
        {
            for (int i = 0; i < Lineas_Dibujadas.Count; i++)
            {
                Lineas_Dibujadas[i].Traslate(x, y);
                PaintLine(Lineas_Dibujadas[i].point1, Lineas_Dibujadas[i].point2, Lineas_Dibujadas[i].color, true);
            }
            for (int i = 0; i < Rayos_Dibujados.Count; i++)
            {
                Rayos_Dibujados[i].Traslate(x, y);
                PaintRay(Rayos_Dibujados[i].point1, Rayos_Dibujados[i].point2, Rayos_Dibujados[i].color, true);
            }
            for (int i = 0; i < Segmentos_Dibujados.Count; i++)
            {
                Segmentos_Dibujados[i].Traslate(x, y);
                PaintSegment(Segmentos_Dibujados[i].point1, Segmentos_Dibujados[i].point2, Segmentos_Dibujados[i].color, true);
            }
            for (int i = 0; i < Arcos_Dibujados.Count; i++)
            {
                Arcos_Dibujados[i].Traslate(x, y);
                PaintArc(Arcos_Dibujados[i].point1, Arcos_Dibujados[i].point2, Arcos_Dibujados[i].point3, Arcos_Dibujados[i].color, Arcos_Dibujados[i].media, true);
            }
            for (int i = 0; i < Circulos_Dibujados.Count; i++)
            {
                Circulos_Dibujados[i].Traslate(x, y);
                PaintCirc(Circulos_Dibujados[i].point1, Circulos_Dibujados[i].point2, Circulos_Dibujados[i].color, Circulos_Dibujados[i].media, true);
            }
            for (int i = 0; i < Points_Dibujados.Count; i++)
            {
                Points_Dibujados[i].Traslate(x, y);
                PaintPoint(Points_Dibujados[i], true);
            }
        }
        private void Limpiar_Lianzo()
        {
            Graphics lienzo = Lienzo.CreateGraphics();
            lienzo.Clear(Color.Gray);
        }

        private void btn_run_Click_1(object sender, EventArgs e)
        {
            Reset(sender, e);
            Graphics lienzo = Lienzo.CreateGraphics();
            this.impud = txtBox_codigo.Text;
            try
            {
                impud = txtBox_codigo.Text;
                List<Token> tokens = lexer.TokenizeString(impud);
                Parser arbol = new Parser(tokens);
                List<Errors> errores = new List<Errors>();
                List<Expression> codigo = arbol.ParseOne();
                Scope General = new Scope(Context.General, new Dictionary<Token, object>(), new Dictionary<Token, ExpressionType>());

                foreach (Expression exp in codigo)
                    exp.Scope(General);

                foreach (Expression exp in codigo)
                    exp.CheckSemantic(errores);

                if (errores.Count > 0)
                {
                    for (int i = 0; i < errores.Count; i++)
                        Console.WriteLine(errores[i]);
                    throw new Exception(errores[0].ToString());
                }
                foreach (Expression exp in codigo)
                    exp.Evaluate();
            }
            catch (Exception ex)
            {
                Error_Box.Text = ex.Message;
                MessageBox.Show(ex.Message);
            }
        }
        public void Pintar_Figura(object figura,string color)
        {
            if (figura is PointP) PaintPoint((PointP)figura, false);
            else if (figura is Rayo) PaintRay(((Rayo)figura).point1, ((Rayo)figura).point2, ((Rayo)figura).color, false);
            else if (figura is Linea) PaintLine(((Linea)figura).point1, ((Linea)figura).point2, color, false);
            else if (figura is Segmento) PaintSegment(((Segmento)figura).point1, ((Segmento)figura).point2, ((Segmento)figura).color, false);
            else if (figura is Circulo) PaintCirc(((Circulo)figura).point1, ((Circulo)figura).point2, ((Circulo)figura).color, ((Circulo)figura).media, false);
            else if (figura is Arco) PaintArc(((Arco)figura).point1, ((Arco)figura).point2, ((Arco)figura).point3, ((Arco)figura).color, ((Arco)figura).media, false);
        }
    }
}
