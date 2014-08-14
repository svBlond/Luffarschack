using System.Drawing;
using System.Windows.Forms;

namespace Luffarschacksprojekt
{
    public partial class Form1 : Form
    {
        private void RitaFönstret(Graphics g)
        {
            // Ritar rektanglarna där tiderna skrivs
            g.FillRectangle(new SolidBrush(Color.GhostWhite), spelareEttsKlockposition);
            g.FillRectangle(new SolidBrush(Color.GhostWhite), spelareTvåsKlockposition);

            // Ritar spelplanen
            for (int grid = 0; grid <= Bräde.sida; grid++)
            {
                g.DrawLine(new Pen(new SolidBrush(Color.Silver)),
                            new Point(0, fältstorlek * grid + menuStrip1.Size.Height),
                            new Point(spelplansstorlekX, grid * fältstorlek + menuStrip1.Size.Height));
                g.DrawLine(new Pen(new SolidBrush(Color.Silver)),
                            new Point(fältstorlek * grid, menuStrip1.Size.Height),
                            new Point(grid * fältstorlek, spelplansstorlekY));
            }

            // Ritar dragen
            for (int y = 0; y < Bräde.sida; y++)
            {
                for (int x = 0; x < Bräde.sida; x++)
                {
                    if (Bräde.ställning[x, y] == (int)Spelare.Tecken.Kryss)
                    {
                        g.DrawString("X", new Font(FontFamily.GenericSerif, teckenstorlek),
                                    new SolidBrush(spelareEtt.Teckenfärg), 
                                    new PointF(x * fältstorlek, y * fältstorlek + menuStrip1.Height));
                    }
                    else if (Bräde.ställning[x, y] == (int)Spelare.Tecken.Cirkel)
                    {
                        g.DrawString("O",
                                    new Font(FontFamily.GenericSerif, teckenstorlek),
                                    new SolidBrush(spelareEtt.Teckenfärg),
                                    new PointF(x * fältstorlek, y * fältstorlek + menuStrip1.Height));
                    }
                }
            }

            // Skriver spelarnas tider
            g.DrawString(spelareEtt.tid.Minute + ":" + spelareEtt.tid.Second, 
                        new Font(FontFamily.GenericSerif, teckenstorlek),
                        new SolidBrush(Color.Black),
                        new PointF(spelareEttsKlockposition.X, spelareEttsKlockposition.Y));
            g.DrawString(spelareTvå.tid.Minute + ":" + spelareTvå.tid.Second,
                        new Font(FontFamily.GenericSerif, teckenstorlek),
                        new SolidBrush(Color.Black), 
                        new PointF(spelareTvåsKlockposition.X, spelareTvåsKlockposition.Y));
        }


        public void RitaSenasteMotorDrag(Bräde.Fält drag)
        {
            Graphics g = CreateGraphics();
            g.DrawString(spelarePåDrag.EgetTecken == Spelare.Tecken.Kryss ? "X" : "O",
                        new Font(FontFamily.GenericSerif, teckenstorlek),
                        new SolidBrush(Color.HotPink),
                        new PointF(drag.X * fältstorlek, drag.Y * fältstorlek + menuStrip1.Height));
        }

        void RitaKryss()
        {
        }
        void RitaCirkel()
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            RitaFönstret(e.Graphics);
        }
    }
}
