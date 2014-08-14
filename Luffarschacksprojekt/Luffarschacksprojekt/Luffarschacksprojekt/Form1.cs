using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Luffarschacksprojekt
{
    public partial class Form1 : Form
    {
        public const int rotenUrAntalRutor = 25;
        public const int spelplansgräns = rotenUrAntalRutor - 1;
        static public int[,] spelplan = new int[rotenUrAntalRutor, rotenUrAntalRutor];
        static public bool xPådrag = true;
        static public List<Koordinat> gjordaDrag = new List<Koordinat>();
        private int dragnummer = 0;
        private static int storlek = 32;
        private static int brädstorlek = rotenUrAntalRutor * storlek;
        private int teckenStorlek;
        private int[,] dragVärden = new int[Form1.rotenUrAntalRutor, Form1.rotenUrAntalRutor];
        private int längstTillVänster = Form1.rotenUrAntalRutor / 2;
        private int längstTillHöger = Form1.rotenUrAntalRutor / 2;
        private int längstUpp = Form1.rotenUrAntalRutor / 2;
        private int längstNer = Form1.rotenUrAntalRutor / 2;
        OriginalgangsterAI bot = new OriginalgangsterAI(/*sekundärhotskonstant: 0.01, totalaHotKonstant: 0.001, moståndarensHotsKonstant: 0.1*/);

        public Form1()
        {
            InitializeComponent();

            // Fixar font storleken
            Graphics g = CreateGraphics();

            for (int sizeIndex = 8; sizeIndex < 256; sizeIndex++)                                   //Tommys kod
            {
                SizeF theSize = g.MeasureString("X", new Font(FontFamily.GenericSerif, sizeIndex)); //Tommys kod
                if (theSize.Height >= storlek)                                                      //Tommys kod
                {
                    teckenStorlek = sizeIndex + 4;                                                  //Tommys kod
                    break;                                                                          //Tommys kod
                }
            }


            xPådrag = true;
            this.ClientSize = new System.Drawing.Size(brädstorlek, brädstorlek);
            spelplan[längstNer, längstNer] = 1;
            RitaDrag(längstNer, längstNer);
            dragnummer++;

            bot.VäljdDrag(new Koordinat(längstNer, längstTillHöger));
            //AI();
        }

        protected override void OnPaint(PaintEventArgs e) //Tommys kod
        {
            base.OnPaint(e);//Tommys kod

            Graphics g = e.Graphics;//Tommys kod

            for (int grid = 0; grid < 26; grid++)//Tommys kod
            {
                g.DrawLine(new Pen(new SolidBrush(Color.Silver)), new Point(0, storlek * grid), new Point(brädstorlek, grid * storlek));//Tommys kod
                g.DrawLine(new Pen(new SolidBrush(Color.Silver)), new Point(storlek * grid, 0), new Point(grid * storlek, brädstorlek));//Tommys kod

            }

            for (int y = 0; y < 25; y++)
            {
                for (int x = 0; x < 25; x++)
                {
                    if (spelplan[x, y] == 1)
                        g.DrawString("X", new Font(FontFamily.GenericSerif, teckenStorlek), new SolidBrush(Color.Black), new PointF(x * storlek, y * storlek));  //Tommys kod
                    else if (spelplan[x, y] == -1)
                        g.DrawString("O", new Font(FontFamily.GenericSerif, teckenStorlek), new SolidBrush(Color.Black), new PointF(x * storlek, y * storlek));  //Tommys kod

                }

            }
            //AI();
            //System.Threading.Thread.Sleep(500);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            bool vann = false;
            int x = e.X / storlek; //Tommys kod
            int y = e.Y / storlek; // Tommys kod
            int påDrag;

            if (spelplan[x, y] == 0)
            {
                xPådrag = false;
                påDrag = -1;

                spelplan[x, y] = påDrag;
                Refresh();
                Koordinat drag = new Koordinat(x, y);
                gjordaDrag.Add(drag);

                if (bot.likvärdigaDrag.Contains(drag))
                {
                    bot.likvärdigaDrag.Remove(drag);
                }

                if (x < längstTillVänster)
                    längstTillVänster = x - 1;
                else if (x > längstTillHöger)
                    längstTillHöger = x + 1;
                if (y > längstNer)
                    längstNer = y + 1;
                else if (y < längstUpp)
                    längstUpp = y - 1;

                if (SammaRad(x, y, påDrag) >= 4 || SammaLinje(x, y, påDrag) >= 4 || SnettUppVänster(x, y, påDrag) >= 4 || SnettUppHöger(x, y, påDrag) >= 4)
                {
                    vann = true;
                    MessageBox.Show("Du vann...");
                    spelplan = new int[25, 25];
                    xPådrag = true;
                    spelplan[12, 12] = 1;
                    gjordaDrag.Clear();
                    Refresh();
                    bot = new OriginalgangsterAI(/*sekundärhotskonstant: 0.01, totalaHotKonstant: 0.001, moståndarensHotsKonstant: 0.1*/);
                }
                if (!vann)
                {
                    xPådrag = !xPådrag;
                    //AI();
                    bot.VäljdDrag(drag);
                    GörDrag(bot.likvärdigaDrag);
                }
            }
        }

        public void AI()
        {
            /*
            Grundläggande AI som ska först kolla söka igenom hur lång oblockad serie den kan skapa, och sedan hur lång motståndaren kan göra.
            Om dens egen är lika lång, eller längre, än motsåndarens fortsätter den på den. Annars blockar den motståndarens längsta.
            */
            System.Diagnostics.Stopwatch stoppur = new System.Diagnostics.Stopwatch();
            stoppur.Start();
            double längstaEgnaHot = 0;
            double längstaMotståndareHot = 0;
            double längstaMotståndarSekundär = 0;
            double längstaEgnaSekundär = 0;
            double störstaEgnaTotal = 0;
            double störstaMotståndarTotal = 0;
            double egetHotVidBlock = 0;
            double egetSekundärHotVidBlock = 0;
            double egnaTotalaHotVidBlock = 0;
            List<Koordinat> bästaEgnaKoordinater = new List<Koordinat>();
            List<Koordinat> bästaMotståndareKoordinater = new List<Koordinat>();
            xPådrag = !xPådrag;
            int egetDragVärde = xPådrag ? 1 : -1;
            int motståndareDragVärde = -egetDragVärde;
            double motståndarhotVidAnfall = 0;

            for (int xkoordinat = längstTillVänster - 1; xkoordinat <= längstTillHöger + 1; xkoordinat++)
            {
                for (int ykoordinat = längstUpp - 1; ykoordinat <= längstNer + 1; ykoordinat++)
                {
                    if (xkoordinat >= 0 && ykoordinat >= 0 && xkoordinat <= 24 && ykoordinat <= 24 && spelplan[xkoordinat, ykoordinat] == 0 && !Grannlös(xkoordinat, ykoordinat))
                    {
                        spelplan[xkoordinat, ykoordinat] = egetDragVärde;
                        double potentielltEgnaVärde = 0;
                        double sammaRad = PotentialSammaRad(xkoordinat, ykoordinat, egetDragVärde);
                        double sammaLinje = PotentialSammaLinje(xkoordinat, ykoordinat, egetDragVärde);
                        double snettUppVänster = PotentialSnettUppVänster(xkoordinat, ykoordinat, egetDragVärde);
                        double snettUppHöger = PotentialSnettUppHöger(xkoordinat, ykoordinat, egetDragVärde);
                        double egetSekundärhot = 0;
                        double egnaTotalaHot = sammaRad + sammaLinje + snettUppHöger + snettUppVänster;

                        BeräknaDragvärde(sammaRad, sammaLinje, snettUppHöger, snettUppVänster, ref potentielltEgnaVärde, ref egetSekundärhot);

                        spelplan[xkoordinat, ykoordinat] = motståndareDragVärde;
                        double potentielltMotståndarVärde = 0;
                        sammaRad = PotentialSammaRad(xkoordinat, ykoordinat, motståndareDragVärde);
                        sammaLinje = PotentialSammaLinje(xkoordinat, ykoordinat, motståndareDragVärde);
                        snettUppVänster = PotentialSnettUppVänster(xkoordinat, ykoordinat, motståndareDragVärde);
                        snettUppHöger = PotentialSnettUppHöger(xkoordinat, ykoordinat, motståndareDragVärde);
                        double motståndarensSekundärhot = 0;
                        double motståndarensTotalaHot = sammaRad + sammaLinje + snettUppHöger + snettUppVänster;

                        BeräknaDragvärde(sammaRad, sammaLinje, snettUppHöger, snettUppVänster, ref potentielltMotståndarVärde, ref motståndarensSekundärhot);

                        if (potentielltMotståndarVärde > potentielltEgnaVärde)
                        {
                            VäljBlockeringsdrag(potentielltMotståndarVärde, ref längstaMotståndareHot, motståndarensSekundärhot, ref längstaMotståndarSekundär, motståndarensTotalaHot,
                                                ref störstaMotståndarTotal, potentielltEgnaVärde, ref egetHotVidBlock, egetSekundärhot, ref egetSekundärHotVidBlock, egnaTotalaHot, ref egnaTotalaHotVidBlock,
                                                xkoordinat, ykoordinat, bästaMotståndareKoordinater);
                        }

                        else
                        {
                            VäljAnfallsDrag(potentielltEgnaVärde, ref längstaEgnaHot, egetSekundärhot, ref längstaEgnaSekundär, egnaTotalaHot, ref störstaEgnaTotal,
                                             potentielltMotståndarVärde, ref motståndarhotVidAnfall, xkoordinat, ykoordinat, bästaEgnaKoordinater);
                        }

                        spelplan[xkoordinat, ykoordinat] = 0;
                    }
                }
            }

            GörDrag(längstaEgnaHot, längstaMotståndareHot, stoppur, bästaEgnaKoordinater, bästaMotståndareKoordinater, egetDragVärde);
            //System.Threading.Thread.Sleep(500);
            //AI();
        }


        public void BeräknaDragvärde(double sammaRad, double sammaLinje, double snettUppHöger, double snettUppVänster, ref double störstaHot, ref double sekundärHot)
        {
            if (sammaRad > störstaHot)
                störstaHot = sammaRad;
            else if (sammaRad == störstaHot && sammaRad == 2.5)
                störstaHot = 3.25;
            else if (sammaRad >= sekundärHot)
                sekundärHot = sammaRad;
            if (sammaLinje > störstaHot)
                störstaHot = sammaLinje;
            else if (sammaLinje == störstaHot && sammaLinje == 2.5)
                störstaHot = 3.25;
            else if (sammaLinje >= sekundärHot)
                sekundärHot = sammaLinje;
            if (snettUppVänster > störstaHot)
                störstaHot = snettUppVänster;
            else if (snettUppVänster == störstaHot && snettUppVänster == 2.5)
                störstaHot = 3.25;
            else if (snettUppVänster >= sekundärHot)
                sekundärHot = snettUppVänster;
            if (snettUppHöger > störstaHot)
                störstaHot = snettUppHöger;
            else if (snettUppHöger == störstaHot && snettUppHöger == 2.5)
                störstaHot = 3.25;
            else if (snettUppHöger >= sekundärHot)
                sekundärHot = snettUppHöger;
        }

        public void VäljBlockeringsdrag(double potentielltMotståndarVärde, ref double längstaMotståndareHot, double motståndarensSekundärhot, ref double längstaMotståndarSekundär, double motståndarensTotalaHot,
                                ref double störstaMotståndarTotal, double potentielltEgnaVärde, ref double egetHotVidBlock, double egetSekundärhot, ref double egetSekundärHotVidBlock, double egnaTotalaHot, ref double egnaTotalaHotVidBlock,
                                int xkoordinat, int ykoordinat, List<Koordinat> bästaMotståndareKoordinater)
        {
            if (potentielltMotståndarVärde > längstaMotståndareHot)
            {
                egetSekundärHotVidBlock = egetSekundärhot;
                egnaTotalaHotVidBlock = egnaTotalaHot;
                egetHotVidBlock = potentielltEgnaVärde;
                längstaMotståndareHot = potentielltMotståndarVärde;
                längstaMotståndarSekundär = motståndarensSekundärhot;
                störstaMotståndarTotal = motståndarensTotalaHot;
                bästaMotståndareKoordinater.Clear();
                bästaMotståndareKoordinater.Add(new Koordinat(xkoordinat, ykoordinat));
            }

            else if (potentielltMotståndarVärde == längstaMotståndareHot)
            {
                if (potentielltEgnaVärde >= egetHotVidBlock)
                {
                    if (potentielltEgnaVärde > egetHotVidBlock)
                    {
                        egetSekundärHotVidBlock = egetSekundärhot;
                        egnaTotalaHotVidBlock = egnaTotalaHot;
                        egetHotVidBlock = potentielltEgnaVärde;
                        längstaMotståndarSekundär = motståndarensSekundärhot;
                        störstaMotståndarTotal = motståndarensTotalaHot;
                        bästaMotståndareKoordinater.Clear();
                        bästaMotståndareKoordinater.Add(new Koordinat(xkoordinat, ykoordinat));
                    }

                    else if (motståndarensSekundärhot >= längstaMotståndarSekundär)
                    {
                        if (motståndarensSekundärhot > längstaMotståndarSekundär)
                        {
                            egetSekundärHotVidBlock = egetSekundärhot;
                            egnaTotalaHotVidBlock = egnaTotalaHot;
                            egetHotVidBlock = potentielltEgnaVärde;
                            längstaMotståndarSekundär = motståndarensSekundärhot;
                            störstaMotståndarTotal = motståndarensTotalaHot;
                            bästaMotståndareKoordinater.Clear();
                            bästaMotståndareKoordinater.Add(new Koordinat(xkoordinat, ykoordinat));
                        }

                        else if (motståndarensSekundärhot == längstaMotståndarSekundär)
                        {
                            if (egetSekundärhot > egetSekundärHotVidBlock)
                            {
                                egetSekundärHotVidBlock = egetSekundärhot;
                                egnaTotalaHotVidBlock = egnaTotalaHot;
                                egetHotVidBlock = potentielltEgnaVärde;
                                längstaMotståndarSekundär = motståndarensSekundärhot;
                                störstaMotståndarTotal = motståndarensTotalaHot;
                                bästaMotståndareKoordinater.Clear();
                                bästaMotståndareKoordinater.Add(new Koordinat(xkoordinat, ykoordinat));
                            }

                            else if (motståndarensTotalaHot > störstaMotståndarTotal && egetSekundärhot == egetSekundärHotVidBlock)
                            {
                                egetSekundärHotVidBlock = egetSekundärhot;
                                egnaTotalaHotVidBlock = egnaTotalaHot;
                                egetHotVidBlock = potentielltEgnaVärde;
                                längstaMotståndarSekundär = motståndarensSekundärhot;
                                störstaMotståndarTotal = motståndarensTotalaHot;
                                bästaMotståndareKoordinater.Clear();
                                bästaMotståndareKoordinater.Add(new Koordinat(xkoordinat, ykoordinat));
                            }

                            else if (egnaTotalaHot > egnaTotalaHotVidBlock && egetSekundärhot == egetSekundärHotVidBlock && motståndarensTotalaHot > störstaMotståndarTotal)
                            {
                                egetSekundärHotVidBlock = egetSekundärhot;
                                egnaTotalaHotVidBlock = egnaTotalaHot;
                                egetHotVidBlock = potentielltEgnaVärde;
                                längstaMotståndarSekundär = motståndarensSekundärhot;
                                störstaMotståndarTotal = motståndarensTotalaHot;
                                bästaMotståndareKoordinater.Clear();
                                bästaMotståndareKoordinater.Add(new Koordinat(xkoordinat, ykoordinat));
                            }
                            else if (egetSekundärhot == egetSekundärHotVidBlock && egnaTotalaHot == egnaTotalaHotVidBlock && motståndarensTotalaHot == störstaMotståndarTotal)
                                bästaMotståndareKoordinater.Add(new Koordinat(xkoordinat, ykoordinat));
                        }
                    }
                }
            }
        }

        public void VäljAnfallsDrag(double potentielltEgnaVärde, ref double längstaEgnaHot, double egetSekundärhot, ref double längstaEgnaSekundär, double egnaTotalaHot, ref double störstaEgnaTotal,
                                    double potentielltMotståndarVärde, ref double motståndarhotVidAnfall, int xkoordinat, int ykoordinat, List<Koordinat> bästaEgnaKoordinater)
        {
            if (potentielltEgnaVärde > längstaEgnaHot)
            {
                längstaEgnaSekundär = egetSekundärhot;
                längstaEgnaHot = potentielltEgnaVärde;
                störstaEgnaTotal = egnaTotalaHot;
                motståndarhotVidAnfall = potentielltMotståndarVärde;
                bästaEgnaKoordinater.Clear();
                bästaEgnaKoordinater.Add(new Koordinat(xkoordinat, ykoordinat));
            }

            else if (potentielltEgnaVärde == längstaEgnaHot)
            {
                if (potentielltMotståndarVärde > motståndarhotVidAnfall && motståndarhotVidAnfall >= egetSekundärhot)
                {
                    motståndarhotVidAnfall = potentielltMotståndarVärde;
                    längstaEgnaSekundär = egetSekundärhot;
                    störstaEgnaTotal = egnaTotalaHot;
                    bästaEgnaKoordinater.Clear();
                    bästaEgnaKoordinater.Add(new Koordinat(xkoordinat, ykoordinat));
                }

                else if (egetSekundärhot > längstaEgnaSekundär && egetSekundärhot > motståndarhotVidAnfall)
                {
                    motståndarhotVidAnfall = potentielltMotståndarVärde;
                    längstaEgnaSekundär = egetSekundärhot;
                    störstaEgnaTotal = egnaTotalaHot;
                    bästaEgnaKoordinater.Clear();
                    bästaEgnaKoordinater.Add(new Koordinat(xkoordinat, ykoordinat));
                }

                else if (egetSekundärhot == längstaEgnaSekundär && potentielltMotståndarVärde == motståndarhotVidAnfall)
                {
                    if (egnaTotalaHot > störstaEgnaTotal)
                    {
                        längstaEgnaSekundär = egetSekundärhot;
                        störstaEgnaTotal = egnaTotalaHot;
                        motståndarhotVidAnfall = potentielltMotståndarVärde;
                        bästaEgnaKoordinater.Clear();
                        bästaEgnaKoordinater.Add(new Koordinat(xkoordinat, ykoordinat)); ;
                    }

                    else if (egnaTotalaHot == störstaEgnaTotal)
                        bästaEgnaKoordinater.Add(new Koordinat(xkoordinat, ykoordinat));
                }
            }
        }

        public void GörDrag(double längstaEgnaHot, double längstaMotståndareHot, System.Diagnostics.Stopwatch stoppur, List<Koordinat> bästaEgnaKoordinater, List<Koordinat> bästaMotståndareKoordinater, int egetDragVärde)
        {
            Random slumpgenerator = new Random(DateTime.Now.Millisecond);

            if (längstaEgnaHot >= längstaMotståndareHot)
            {
                int slumpatIndex = slumpgenerator.Next(bästaEgnaKoordinater.Count);
                //stoppur.Stop();
                //MessageBox.Show(stoppur.ElapsedTicks.ToString());
                RitaDrag(bästaEgnaKoordinater[slumpatIndex].x, bästaEgnaKoordinater[slumpatIndex].y);
                spelplan[bästaEgnaKoordinater[slumpatIndex].x, bästaEgnaKoordinater[slumpatIndex].y] = egetDragVärde;
                gjordaDrag.Add(bästaEgnaKoordinater[slumpatIndex]);
                //MessageBox.Show(längstaEgnaHot.ToString());
                if (bästaEgnaKoordinater[slumpatIndex].x < längstTillVänster)
                    längstTillVänster = bästaEgnaKoordinater[slumpatIndex].x - 1;
                else if (bästaEgnaKoordinater[slumpatIndex].x > längstTillHöger)
                    längstTillHöger = bästaEgnaKoordinater[slumpatIndex].x + 1;
                if (bästaEgnaKoordinater[slumpatIndex].y > längstNer)
                    längstNer = bästaEgnaKoordinater[slumpatIndex].y + 1;
                else if (bästaEgnaKoordinater[slumpatIndex].y < längstUpp)
                    längstUpp = bästaEgnaKoordinater[slumpatIndex].y - 1;

                if (SammaRad(bästaEgnaKoordinater[slumpatIndex].x, bästaEgnaKoordinater[slumpatIndex].y, egetDragVärde) >= 4 || SammaLinje(bästaEgnaKoordinater[slumpatIndex].x, bästaEgnaKoordinater[slumpatIndex].y, egetDragVärde) >= 4 || SnettUppVänster(bästaEgnaKoordinater[slumpatIndex].x, bästaEgnaKoordinater[slumpatIndex].y, egetDragVärde) >= 4 || SnettUppHöger(bästaEgnaKoordinater[slumpatIndex].x, bästaEgnaKoordinater[slumpatIndex].y, egetDragVärde) >= 4)
                {
                    MessageBox.Show("Du blev ägd av en maskin.");
                    spelplan = new int[25, 25];
                    gjordaDrag.Clear();
                    Refresh();
                }
            }
            else
            {
                //MessageBox.Show(längstaMotståndareHot.ToString());
                int slumpatIndex = slumpgenerator.Next(bästaMotståndareKoordinater.Count);
                //MessageBox.Show(stoppur.ElapsedTicks.ToString());
                RitaDrag(bästaMotståndareKoordinater[slumpatIndex].x, bästaMotståndareKoordinater[slumpatIndex].y);
                spelplan[bästaMotståndareKoordinater[slumpatIndex].x, bästaMotståndareKoordinater[slumpatIndex].y] = egetDragVärde;
                gjordaDrag.Add(bästaMotståndareKoordinater[slumpatIndex]);

                if (bästaMotståndareKoordinater[slumpatIndex].x < längstTillVänster)
                    längstTillVänster = bästaMotståndareKoordinater[slumpatIndex].x - 1;
                else if (bästaMotståndareKoordinater[slumpatIndex].x > längstTillHöger)
                    längstTillHöger = bästaMotståndareKoordinater[slumpatIndex].x + 1;
                if (bästaMotståndareKoordinater[slumpatIndex].y > längstNer)
                    längstNer = bästaMotståndareKoordinater[slumpatIndex].y + 1;
                else if (bästaMotståndareKoordinater[slumpatIndex].y < längstUpp)
                    längstUpp = bästaMotståndareKoordinater[slumpatIndex].y - 1;
            }
        }

        public void GörDrag(List<Koordinat> likvärdigaDrag)
        {
            Random slumpgenerator = new Random();
            Koordinat drag = likvärdigaDrag[slumpgenerator.Next(likvärdigaDrag.Count)];
            RitaDrag(drag.x, drag.y);
            spelplan[drag.x, drag.y] = xPådrag ? 1 : -1;
            xPådrag = !xPådrag;
            likvärdigaDrag.Remove(drag);
            gjordaDrag.Add(drag);
            bot.VäljdDrag(drag);

            if (SammaRad(drag.x, drag.y, 1) >= 4 || SammaLinje(drag.x, drag.y, 1) >= 4 || SnettUppVänster(drag.x, drag.y, 1) >= 4 || SnettUppHöger(drag.x, drag.y, 1) >= 4)
            {
                MessageBox.Show("Du blev ägd av en maskin.");
                spelplan = new int[25, 25];
                xPådrag = true;
                spelplan[12, 12] = 1;
                gjordaDrag.Clear();
                bot = new OriginalgangsterAI(/*sekundärhotskonstant: 0.01, totalaHotKonstant: 0.001, moståndarensHotsKonstant: 0.1*/);
                Refresh();
            }
        }
        
        public void RitaDrag(int x, int y)
        {
            Graphics g = CreateGraphics();
            g.DrawString(xPådrag ? "X" : "O", new Font(FontFamily.GenericSerif, teckenStorlek), new SolidBrush(Color.HotPink), new PointF(x * storlek, y * storlek));  //Tommys kod förutom ternaryn
        }

        public bool Grannlös(int xkoordinat, int ykoordinat)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (xkoordinat + x >= 0 && xkoordinat + x < 25)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (!(y == 0 && x == 0) && ykoordinat + y >= 0 && ykoordinat + y < 25 && spelplan[xkoordinat + x, ykoordinat + y] != 0)
                            return false;
                    }
                }
            }

            return true;
        }

        public int SammaRad(int x, int y, int påDrag)
        {
            int antal = 0;
            for (int indexökning = x + 1; indexökning <= x + 4; indexökning++)
            {
                if (indexökning > 24)
                    break;
                if (spelplan[indexökning, y] == påDrag)
                    antal++;
                else
                    break;
            }

            for (int indexminskning = x - 1; indexminskning >= x - 4; indexminskning--)
            {
                if (indexminskning < 0)
                    break;
                if (spelplan[indexminskning, y] == påDrag)
                    antal++;
                else
                    break;
            }

            return antal;
        }

        public int SammaLinje(int x, int y, int påDrag)
        {
            int antal = 0;
            for (int indexökning = y + 1; indexökning <= y + 4; indexökning++)
            {
                if (indexökning > 24)
                    break;
                if (spelplan[x, indexökning] == påDrag)
                    antal++;
                else
                    break;
            }

            for (int indexminskning = y - 1; indexminskning >= y - 4; indexminskning--)
            {
                if (indexminskning < 0)
                    break;
                if (spelplan[x, indexminskning] == påDrag)
                    antal++;
                else
                    break;
            }

            return antal;
        }

        public int SnettUppVänster(int x, int y, int påDrag)
        {
            int antal = 0;

            for (int indexökning = 1; indexökning <= 4; indexökning++)
            {
                if (x + indexökning > 24 || y + indexökning > 24)
                    break;
                if (spelplan[x + indexökning, y + indexökning] == påDrag)
                    antal++;
                else
                    break;
            }

            for (int indexökning = 1; indexökning <= 4; indexökning++)
            {
                if (x - indexökning < 0 || y - indexökning < 0)
                    break;
                if (spelplan[x - indexökning, y - indexökning] == påDrag)
                    antal++;
                else
                    break;
            }

            return antal;
        }

        public int SnettUppHöger(int x, int y, int påDrag)
        {
            int antal = 0;

            for (int indexökning = 1; indexökning <= 4; indexökning++)
            {
                if (x + indexökning > 24 || y - indexökning < 0)
                    break;
                else if (spelplan[x + indexökning, y - indexökning] == påDrag)
                    antal++;
                else
                    break;
            }

            for (int indexökning = 1; indexökning <= 4; indexökning++)
            {
                if (x - indexökning < 0 || y + indexökning > 24)
                    break;
                if (spelplan[x - indexökning, y + indexökning] == påDrag)
                    antal++;
                else
                    break;
            }

            return antal;
        }

        public double PotentialSammaRad(int x, int y, int påDrag)
        {
            double antal = 0;
            bool blockÅtEnaHållet = false;

            for (int indexökning = x + 1; indexökning <= x + 4; indexökning++)
            {
                if (indexökning > 24)
                {
                    blockÅtEnaHållet = true;
                    break;
                }
                if (spelplan[indexökning, y] == påDrag)
                {
                    antal++;
                }
                else
                {
                    if (spelplan[indexökning, y] == -påDrag)
                        blockÅtEnaHållet = true;
                    break;
                }
            }

            for (int indexminskning = x - 1; indexminskning >= x - 4; indexminskning--)
            {
                if (indexminskning < 0)
                {
                    if (blockÅtEnaHållet)
                        return antal == 4 ? 5 : 0;
                    else
                        blockÅtEnaHållet = true;
                    break;
                }
                if (spelplan[indexminskning, y] == påDrag)
                    antal++;
                else
                {
                    if (spelplan[indexminskning, y] == -påDrag)
                    {
                        if (blockÅtEnaHållet)
                            return antal == 4 ? 5 : 0;
                        else
                            blockÅtEnaHållet = true;
                    }
                    break;
                }
            }

            antal += blockÅtEnaHållet ? 0 : 0.5;
            return antal;
        }

        public double PotentialSammaLinje(int x, int y, int påDrag)
        {
            double antal = 0;
            bool blockÅtEnaHållet = false;

            for (int indexökning = y + 1; indexökning <= y + 4; indexökning++)
            {
                if (indexökning > 24)
                {
                    blockÅtEnaHållet = true;
                    break;
                }
                if (spelplan[x, indexökning] == påDrag)
                    antal++;
                else
                {
                    if (spelplan[x, indexökning] == -påDrag)
                        blockÅtEnaHållet = true;
                    break;
                }
            }

            for (int indexminskning = y - 1; indexminskning >= y - 4; indexminskning--)
            {
                if (indexminskning < 0)
                {
                    if (blockÅtEnaHållet)
                        return antal == 4 ? 5 : 0;
                    else
                        blockÅtEnaHållet = true;
                    break;
                }
                if (spelplan[x, indexminskning] == påDrag)
                    antal++;
                else
                {
                    if (spelplan[x, indexminskning] == -påDrag)
                    {
                        if (blockÅtEnaHållet)
                            return antal == 4 ? 5 : 0;
                        else
                            blockÅtEnaHållet = true;
                    }
                    break;
                }
            }

            antal += blockÅtEnaHållet ? 0 : 0.5;
            return antal;
        }

        public double PotentialSnettUppVänster(int x, int y, int påDrag)
        {
            double antal = 0;
            bool blockÅtEnaHållet = false;

            for (int indexökning = 1; indexökning <= 4; indexökning++)
            {
                if (x + indexökning > 24 || y + indexökning > 24)
                {
                    blockÅtEnaHållet = true;
                    break;
                }
                if (spelplan[x + indexökning, y + indexökning] == påDrag)
                    antal++;
                else
                {
                    if (spelplan[x + indexökning, y + indexökning] == -påDrag)
                        blockÅtEnaHållet = true;
                    break;
                }
            }

            for (int indexökning = 1; indexökning <= 4; indexökning++)
            {
                if (x - indexökning < 0 || y - indexökning < 0)
                {
                    if (blockÅtEnaHållet)
                        return antal == 4 ? 5 : 0;
                    else
                        blockÅtEnaHållet = true;
                    break;
                }
                if (spelplan[x - indexökning, y - indexökning] == påDrag)
                    antal++;
                else
                {
                    if (spelplan[x - indexökning, y - indexökning] == -påDrag)
                    {
                        if (blockÅtEnaHållet)
                            return antal == 4 ? 5 : 0;
                        else
                            blockÅtEnaHållet = true;
                    }
                    break;
                }
            }

            antal += blockÅtEnaHållet ? 0 : 0.5;
            return antal;
        }

        public double PotentialSnettUppHöger(int x, int y, int påDrag)
        {
            double antal = 0;
            bool blockÅtEnaHållet = false;

            for (int indexökning = 1; indexökning <= 4; indexökning++)
            {
                if (x + indexökning > 24 || y - indexökning < 0)
                {
                    blockÅtEnaHållet = true;
                    break;
                }
                else if (spelplan[x + indexökning, y - indexökning] == påDrag)
                    antal++;
                else
                {
                    if (spelplan[x + indexökning, y - indexökning] == -påDrag)
                        blockÅtEnaHållet = true;
                    break;
                }
            }

            for (int indexökning = 1; indexökning <= 4; indexökning++)
            {
                if (x - indexökning < 0 || y + indexökning > 24)
                {
                    if (blockÅtEnaHållet)
                        return antal == 4 ? 5 : 0;
                    else
                        blockÅtEnaHållet = true;
                    break;
                }
                if (spelplan[x - indexökning, y + indexökning] == påDrag)
                    antal++;
                else
                {
                    if (spelplan[x - indexökning, y + indexökning] == -påDrag)
                    {
                        if (blockÅtEnaHållet)
                            return antal == 4 ? 5 : 0;
                        else
                            blockÅtEnaHållet = true;
                    }
                    break;
                }
            }

            antal += blockÅtEnaHållet ? 0 : 0.5;
            return antal;
        }

        public struct Koordinat
        {
            public int x;
            public int y;

            public Koordinat(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        //private void Form1_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Left/*<- Nilssons kod*/ && gjordaDrag.Count > 1)
        //    {
        //        for (int i = 0; i < 2; i++)
        //        {
        //            int gjordaDragIndex = gjordaDrag.Count - 1;
        //            Graphics g = CreateGraphics();
        //            g.DrawString(spelplan[gjordaDrag[gjordaDragIndex].x, gjordaDrag[gjordaDragIndex].y] == 1 ? "X" : "O",
        //                new Font(FontFamily.GenericSerif, teckenStorlek), new SolidBrush(Color.Silver), new PointF(gjordaDrag[gjordaDragIndex].x * storlek, gjordaDrag[gjordaDragIndex].y * storlek));
        //            spelplan[gjordaDrag[gjordaDragIndex].x, gjordaDrag[gjordaDragIndex].y] = 0;
        //            gjordaDrag.RemoveAt(gjordaDragIndex);
        //        }
        //    }
        //}

    }
}
