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
using TokensGeo;
using ParserGeo;
using Geo_Walle;

namespace Geo_Walle
{
    public partial class GeoW : Form
    {
        //Codigo de entrada del usuario.
        string impud;
        //Figuras que se encuentran dentro de un draw.
        List<token> Figuras_del_Parser;
        //errores del evaluador que debo imprimir en la pantalla.
        List<Errors> errores;
        // dividi la lista de figuras en cutro segun el tipo de figura que era cada objeto
        //Puntos
        List<PointP> Puntos_para_dibujar;
        // Lineas, rayos y segmentos.
        List<Figuras> Figuras_para_dibujar;
        //circunferencias
        List<Circulo> Circulos_para_dibujar;
        //Arcos
        List<Arco> Arcos_para_dibujar;
        // Listas de las figuras que ya estan dibujadas en el lienzo.
        List<Figuras> Figuras_Dibujadas;
        List<PointP> Points_Dibujados;
        List<Circulo> Circulos_Dibujados;
        List<Arco> Arcos_Dibujados;
        //Pila de los colores q va poniendo el usuario.
        Stack<Color> color = new Stack<Color>();
        // color actual de la figuras.
        Color Fig_Color;

        public GeoW()
        {
            InitializeComponent();

            Puntos_para_dibujar = new List<PointP>();
            Figuras_para_dibujar = new List<Figuras>();
            Circulos_para_dibujar = new List<Circulo>();
            Arcos_para_dibujar = new List<Arco>();

            Figuras_del_Parser = new List<token>();
            Figuras_Dibujadas = new List<Figuras>();
            Circulos_Dibujados = new List<Circulo>();
            Points_Dibujados = new List<PointP>();
            Arcos_Dibujados = new List<Arco>();
            errores = new List<Errors>();

            //if (color.Count <= 0) Fig_Color = Color.Black;
            //else Fig_Color = color.Pop();
        }

        private void btn_volver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            try
            {

                impud = txtBox_codigo.Text;
                List<token> m = Tokenizar.TokenizeString(impud,errores);
                Geometrico arbol = new Geometrico("", TokenTypes.Identifier, null);
                arbol.expression = m;
                arbol.Parser();

                //if (!arbol.CheckSemantic(errores))
                //{
                //    for (int i = 0; i < errores.Count; i++)
                //        Console.WriteLine(errores[i]);
                //}
                //else
                //arbol.Evaluate();
                Figuras_del_Parser = arbol.tokens;

                Transformar(Figuras_del_Parser);
            }
            catch (Exception ex)
            {
                Error_Box.Text = ex.Message;
                MessageBox.Show(ex.Message);
            }


            Reset(sender, e);
            Graphics lienzo = Lienzo.CreateGraphics();
            this.impud = txtBox_codigo.Text;

            foreach (var item in Puntos_para_dibujar)
            {
                PaintPoint(item, false);
            }
            foreach (var item in Figuras_para_dibujar)
            {
                Paint(item.point1, item.point2, item.figTye, false, Fig_Color);
            }
            foreach (var item in Circulos_para_dibujar)
            {
                PaintCirc(item.point1, item.point2, item.figTye, item.media, false, Fig_Color);
            }
            foreach (var item in Arcos_para_dibujar)
            {
                PaintArc(item.point1, item.point2, item.point3, item.figTye, item.media, false, Fig_Color);
            }
        }

        private void PaintCirc(PointP point1, PointP point2, FigTye figura, int media, bool esta, Color color)
        {
            Graphics lienzo = Lienzo.CreateGraphics();
            if (!esta)
            {
                Circulos_Dibujados.Add(new Circulo(point1, point2, figura, media));
                esta = true;
            }
            PaintPoint(point1, esta);
            PaintPoint(point2, esta);
            int radio = media;

            Rectangle circle = new Rectangle(point1.x - radio, point1.y - radio, radio * 2, radio * 2);
            lienzo.DrawEllipse(new Pen(color), circle);
        }

        private void Paint(PointP point1, PointP point2, FigTye figura, bool esta, Color color)
        {
            Graphics lienzo = Lienzo.CreateGraphics();
            if (!esta)
            {
                Figuras_Dibujadas.Add(new Figuras(point1, point2, figura));
                esta = true;
            }

            PaintPoint(point1, esta);
            PaintPoint(point2, esta);

            if (figura == FigTye.segment)
            {
                lienzo.DrawLine(new Pen(color), point1.x + 2, point1.y + 2, point2.x + 2, point2.y + 2);
            }
            else if (figura == FigTye.line)
            {
                float pendiente_m = (float)(point1.y - point2.y) / (float)(point1.x - point2.x);
                PointP interseccion_izq = new PointP("p1", 0, (int)(point2.y + (0 - point2.x) * pendiente_m));
                PointP interseccion_der = new PointP("p2", this.Width - 1, (int)(point2.y + ((this.Width - 1) - point2.x) * pendiente_m));
                PointP interseccion_Top = new PointP("p1", (int)(point2.x + (0 - point2.y) / pendiente_m), 0);
                PointP interseccion_Dwn = new PointP("p2", (int)(point2.x + ((this.Height - 1) - point2.y) / pendiente_m), this.Height - 1);

                lienzo.DrawLine(new Pen(color), interseccion_izq.x + 2, interseccion_izq.y + 2, interseccion_der.x + 2, interseccion_der.y + 2);
                lienzo.DrawLine(new Pen(color), interseccion_Top.x + 2, interseccion_Top.y + 2, interseccion_Dwn.x + 2, interseccion_Dwn.y + 2);
            }
            else if (figura == FigTye.ray)
            {
                //y = mx + b
                float pendiente_m = (float)(point1.y - point2.y) / (float)(point1.x - point2.x);
                float b = point2.y - pendiente_m * point2.x;

                float interseccionX_izq = 0;
                float interseccionY_izq = pendiente_m;
                PointP interseccion_Izq = new PointP("id", (int)interseccionX_izq, (int)interseccionY_izq);

                float interseccionX_der = this.Width - 1;
                float interseccionY_der = pendiente_m * interseccionX_der + b;
                PointP interseccion_Der = new PointP("id", (int)interseccionX_der, (int)interseccionY_der);

                lienzo.DrawLine(new Pen(color), point1.x + 2, point1.y + 2, point2.x + 2, point2.y + 2);
                //lienzo.DrawLine(new Pen(Color.Brown), interseccion_Izq.x, interseccion_Izq.y, 0, interseccion_Izq.y);
                lienzo.DrawLine(new Pen(color), point2.x + 2, point2.y + 2, interseccion_Der.x, interseccion_Der.y);
            }
        }
        private void PaintArc(PointP point1, PointP point2, PointP centro, FigTye figura, int media, bool esta, Color color)
        {
            Graphics lienzo = Lienzo.CreateGraphics();
            if (!esta)
            {
                Arcos_Dibujados.Add(new Arco(point1, point2, centro, figura, media));
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

            if (angulo_AB < 0 && angulo_AC > 0) angula_AC_AB = 360 - (-angulo_AB + angulo_AC);

            if (angulo_AB > 0 && angulo_AC < 0) angula_AC_AB = angulo_AB - angulo_AC;

            Rectangle arc = new Rectangle(centro.x - radio, centro.y - radio, radio * 2, radio * 2);
            lienzo.DrawArc(new Pen(color), arc, angulo_AC, angula_AC_AB);
        }

        private void PaintPoint(PointP point, bool esta)
        {
            Graphics lienzo = this.Lienzo.CreateGraphics();

            if (!esta)
                Points_Dibujados.Add(point);

            lienzo.FillEllipse(Brushes.Black, point.x, point.y, 5, 5);
        }
        private float Angulo_de_la_Pendiente(PointP centro, PointP point2, float pendiente_m)
        {
            float angulo1 = (float)Math.Atan(pendiente_m);
            float angulo_grados = (float)(angulo1 * 180 / Math.PI);
            float alpha = 0;

            if (pendiente_m >= 0)
            {
                if (centro.y > point2.y) alpha = -(180 - angulo_grados);
                if (centro.y < point2.y) alpha = angulo_grados;
            }
            if (pendiente_m < 0)
            {
                if (centro.y > point2.y) alpha = angulo_grados;
                if (centro.y < point2.y) alpha = 180 - angulo_grados;
            }

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
            Figuras_Dibujadas = new List<Figuras>();
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
            for (int i = 0; i < Figuras_Dibujadas.Count; i++)
            {
                Figuras_Dibujadas[i].Traslate(x, y);
                Paint(Figuras_Dibujadas[i].point1, Figuras_Dibujadas[i].point2, Figuras_Dibujadas[i].figTye, true, Fig_Color);
            }
            for (int i = 0; i < Arcos_Dibujados.Count; i++)
            {
                Arcos_Dibujados[i].Traslate(x, y);
                PaintArc(Arcos_Dibujados[i].point1, Arcos_Dibujados[i].point2, Arcos_Dibujados[i].point3, Arcos_Dibujados[i].figTye, Arcos_Dibujados[i].media, true, Fig_Color);
            }
            for (int i = 0; i < Circulos_Dibujados.Count; i++)
            {
                Circulos_Dibujados[i].Traslate(x, y);
                PaintCirc(Circulos_Dibujados[i].point1, Circulos_Dibujados[i].point2, Circulos_Dibujados[i].figTye, Circulos_Dibujados[i].media, true, Fig_Color);
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

        private void GeoW_Load(object sender, EventArgs e)
        {

        }

        public void txtBox_codigo_TextChanged(object sender, EventArgs e)
        {

        }

        private void Transformar(List<token> figuras_del_Parser)
        {
            for (int i = 0; i < figuras_del_Parser.Count; i++)
            {
                if (figuras_del_Parser[i].Value == "draw")
                {
                    List<token> draw = figuras_del_Parser[i].tokens;
                    for (int j = 0;j< draw.Count; j++)
                    {
                        if (draw[j] is Point)
                        {

                            PointP p1 = new PointP(draw[j].Value, Convert.ToInt32(((Point)draw[j]).x),Convert.ToInt32(((Point)draw[j]).y));
                            Puntos_para_dibujar.Add(p1);
                        }
                        if (draw[j] is Segment)
                        {
                            PointP p1 = new PointP(draw[j].Value, Convert.ToInt32(((Point)draw[j]).x),Convert.ToInt32(((Point)draw[j]).y));
                            PointP p2 = new PointP(draw[j].Value, Convert.ToInt32(((Point)draw[j]).x),Convert.ToInt32(((Point)draw[j]).y));
                            Figuras_para_dibujar.Add(new Figuras(p1, p2, FigTye.segment));
                        }
                        if (draw[j] is Line)
                        {
                            PointP p1 = new PointP(draw[j].Value, Convert.ToInt32(((Point)draw[0]).x),Convert.ToInt32(((Point)draw[0]).y));
                            PointP p2 = new PointP(draw[j].Value, Convert.ToInt32(((Point)draw[1]).x),Convert.ToInt32(((Point)draw[1]).y)); 
                            Figuras_para_dibujar.Add(new Figuras(p1, p2, FigTye.line));
                        }
                        if (draw[j] is Ray)
                        {
                            PointP p1 = new PointP(draw[j].Value, Convert.ToInt32(((Point)draw[0]).x),Convert.ToInt32(((Point)draw[0]).y));
                            PointP p2 = new PointP(draw[j].Value, Convert.ToInt32(((Point)draw[1]).x),Convert.ToInt32(((Point)draw[1]).y)); 
                            Figuras_para_dibujar.Add(new Figuras(p1, p2, FigTye.ray));
                        }
                        if (draw[j] is Circunferencia)
                        {
                            PointP p1 = new PointP(draw[j].Value, Convert.ToInt32(((Point)draw[0]).x),Convert.ToInt32(((Point)draw[0]).y));
                            PointP p2 = new PointP(draw[j].Value, Convert.ToInt32(((Point)draw[1]).x),Convert.ToInt32(((Point)draw[1]).y)); 
                            int media = (int)Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2));
                            Circulos_para_dibujar.Add(new Circulo(p1, p2, FigTye.circle, media));
                        }
                        if (draw[j] is Arco)
                        {
                            PointP p1 = new PointP(draw[j].tokens[0].Value, Convert.ToInt32(((Point)draw[0]).x),Convert.ToInt32(((Point)draw[0]).y));
                            PointP p2 = new PointP(draw[1].Value, Convert.ToInt32(((Point)draw[1]).x),Convert.ToInt32(((Point)draw[1]).y));
                            PointP p3 = new PointP(draw[2].Value, Convert.ToInt32(((Point)draw[2]).x),Convert.ToInt32(((Point)draw[2]).y));
                            int media = (int)Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2));
                            Arcos_para_dibujar.Add(new Arco(p1, p2, p3, FigTye.circle, media));
                        }
                    }
                }       
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
