using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Luffarschacksprojekt
{
    public abstract class AI
    {
        protected int[,] spelplan = Form1.spelplan;
        //protected double[,] dragVärden = new double[Form1.rotenUrAntalRutor, Form1.rotenUrAntalRutor];
        protected int längstTillVänster = Form1.rotenUrAntalRutor / 2;
        protected int längstTillHöger = Form1.rotenUrAntalRutor / 2;
        protected int längstUpp = Form1.rotenUrAntalRutor / 2;
        protected int längstNer = Form1.rotenUrAntalRutor / 2;
        public List<Form1.Koordinat> likvärdigaDrag = new List<Form1.Koordinat>();
        protected int egetDragvärde = 1; // Det värde som som AI:ns drag har i spelplan. Motsåndarens drag har värdet -egetDragvärde
        protected double sekundärhotskonstant; // Ett värde, 10 ^ -n (där n är ett  positivt heltal), som änvänds för att avgöra hur viktiga positionella faktorer är i jämförelse med varandra.
        protected double totalaHotKonstant; // Ett värde, 10 ^ -n (där n är ett positivt heltal), som änvänds för att avgöra hur viktiga positionella faktorer är i jämförelse med varandra.
        protected double moståndarensHotKonstant; // Ett värde, 10 ^ -n (där n är ett positivt heltal), som änvänds för att avgöra hur viktiga positionella faktorer är i jämförelse med varandra.
        protected double motståndarenSekundärsKonstant; // Ett värde, 10 ^ -n (där n är ett positivt heltal), som änvänds för att avgöra hur viktiga positionella faktorer är i jämförelse med varandra.
        protected double totalaMotståndarhotsKonstant; // Ett värde, 10 ^ -n (där n är ett positivt heltal), som änvänds för att avgöra hur viktiga positionella faktorer är i jämförelse med varandra.

        //private List<hotSorter> prioritetsLista = new List<hotSorter>();

        public double BeräknaDragvärde(Form1.Koordinat koordinater)
        {
            double sammaRad = PotentialSammaRad(koordinater.x, koordinater.y, egetDragvärde);
            double sammaLinje = PotentialSammaLinje(koordinater.x, koordinater.y, egetDragvärde);
            double snettUppHöger = PotentialSnettUppHöger(koordinater.x, koordinater.y, egetDragvärde);
            double snettUppVänster = PotentialSnettUppVänster(koordinater.x, koordinater.y, egetDragvärde);
            double totalaEgnaHot = sammaRad + sammaLinje + snettUppHöger + snettUppVänster; // Summan av alla serier som ett eget drag vid koordinaterna vore en del av.

            double sammaRadMotståndaren = PotentialSammaRad(koordinater.x, koordinater.y, -egetDragvärde);
            double sammaLinjeMotståndaren = PotentialSammaLinje(koordinater.x, koordinater.y, -egetDragvärde);
            double snettUppHögerMotståndaren = PotentialSnettUppHöger(koordinater.x, koordinater.y, -egetDragvärde);
            double snettUppVänsterMotståndaren = PotentialSnettUppVänster(koordinater.x, koordinater.y, -egetDragvärde);
            double totalaMotståndarhot = sammaRadMotståndaren + sammaLinjeMotståndaren + snettUppHögerMotståndaren + snettUppVänsterMotståndaren; // Summan av alla serier som ett motståndardrag vid koordinaterna vore en del a

            double dragVärde = 0;
            double motståndarensDragVärde = 0;
            double egetHot = 0; // Den längsta serien som ett eget drag vid koordinaterna vore en del av.
            double egetSekundärhot = 0; // Den näst längsta serien som ett eget drag vid koordinaterna vore en del av.
            double motståndarensHot = 0; // Den längsta serien som ett motståndardrag vid koordinaterna vore en del av.
            double motståndarensSekundärhot = 0; // Den näst längsta serien som ett motståndardrag vid koordinaterna vore en del av.

            BeräknaHotvärde(sammaRad, sammaLinje, snettUppHöger, snettUppVänster, ref egetHot, ref egetSekundärhot);
            BeräknaHotvärde(sammaRadMotståndaren, sammaLinjeMotståndaren, snettUppHögerMotståndaren, snettUppVänsterMotståndaren, ref motståndarensHot, ref motståndarensSekundärhot);

            dragVärde += egetHot;
            dragVärde += motståndarensHot * moståndarensHotKonstant;
            dragVärde += egetSekundärhot * sekundärhotskonstant;
            dragVärde += motståndarensSekundärhot * motståndarenSekundärsKonstant;
            double grejAttPlussaMed = totalaEgnaHot * totalaHotKonstant;
            double summa = dragVärde + grejAttPlussaMed;
            dragVärde += totalaEgnaHot * totalaHotKonstant;
            dragVärde += totalaMotståndarhot * totalaMotståndarhotsKonstant;

            motståndarensDragVärde += motståndarensHot;
            motståndarensDragVärde += motståndarensSekundärhot * sekundärhotskonstant;
            motståndarensDragVärde += egetSekundärhot * motståndarenSekundärsKonstant;
            motståndarensDragVärde += totalaMotståndarhot * totalaHotKonstant;
            motståndarensDragVärde += totalaEgnaHot * totalaMotståndarhotsKonstant;


            return dragVärde >= motståndarensDragVärde ? dragVärde : motståndarensDragVärde;
        }

        public void BeräknaHotvärde(double sammaRad, double sammaLinje, double snettUppHöger, double snettUppVänster, ref double störstaHot, ref double sekundärHot)
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
                if (Form1.spelplan[indexökning, y] == påDrag)
                {
                    antal++;
                }
                else
                {
                    if (Form1.spelplan[indexökning, y] == -påDrag)
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
                if (Form1.spelplan[indexminskning, y] == påDrag)
                    antal++;
                else
                {
                    if (Form1.spelplan[indexminskning, y] == -påDrag)
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
                if (Form1.spelplan[x, indexökning] == påDrag)
                    antal++;
                else
                {
                    if (Form1.spelplan[x, indexökning] == -påDrag)
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
                if (Form1.spelplan[x, indexminskning] == påDrag)
                    antal++;
                else
                {
                    if (Form1.spelplan[x, indexminskning] == -påDrag)
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
                if (Form1.spelplan[x + indexökning, y + indexökning] == påDrag)
                    antal++;
                else
                {
                    if (Form1.spelplan[x + indexökning, y + indexökning] == -påDrag)
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
                if (Form1.spelplan[x - indexökning, y - indexökning] == påDrag)
                    antal++;
                else
                {
                    if (Form1.spelplan[x - indexökning, y - indexökning] == -påDrag)
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
                else if (Form1.spelplan[x + indexökning, y - indexökning] == påDrag)
                    antal++;
                else
                {
                    if (Form1.spelplan[x + indexökning, y - indexökning] == -påDrag)
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
                if (Form1.spelplan[x - indexökning, y + indexökning] == påDrag)
                    antal++;
                else
                {
                    if (Form1.spelplan[x - indexökning, y + indexökning] == -påDrag)
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

        protected bool Grannlös(int xkoordinat, int ykoordinat)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (xkoordinat + x >= 0 && xkoordinat + x < 25)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (!(y == 0 && x == 0) && ykoordinat + y >= 0 && ykoordinat + y < 25 && Form1.spelplan[xkoordinat + x, ykoordinat + y] != 0)
                            return false;
                    }
                }
            }

            return true;
        }

    }

    public class Koordinat
    {
        public Koordinat(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Int32 X { get; set; }
        public Int32 Y { get; set; }
    }

    public class RekursivAI : AI
    {
        protected int antalKandidatdrag;
        protected int sökdjup;

        public RekursivAI(int antalKandidatdrag, int sökdjup)
        {
            this.antalKandidatdrag = antalKandidatdrag;
            this.sökdjup = sökdjup;
        }
    }

    public class OriginalgangsterAI : AI
    {
        double senasteDragvärde = 0;

        public OriginalgangsterAI(double sekundärhotskonstant = 0, double totalaHotKonstant = 0, double moståndarensHotsKonstant = 0, double motståndarenSekundärsKonstant = 0, double totalaMotståndarhotsKonstant = 0)
        {
            this.sekundärhotskonstant = sekundärhotskonstant;
            this.totalaHotKonstant = totalaHotKonstant;
            this.moståndarensHotKonstant = moståndarensHotsKonstant;
            this.motståndarenSekundärsKonstant = motståndarenSekundärsKonstant;
            this.totalaMotståndarhotsKonstant = totalaMotståndarhotsKonstant;
        }

        public void VäljdDrag(Form1.Koordinat koordinater)
        {
            double nuvarandeDragvärde = senasteDragvärde;
            int ursprungsx = koordinater.x;
            int ursprungsy = koordinater.y;

            #region Räkna ut dragvärdet för fälten som angränsar det fält där ett drag nyss gjordes.

            #region Höger

            for (int xkoordinat = koordinater.x + 1; xkoordinat >= 0 && xkoordinat <= Form1.spelplansgräns && spelplan[xkoordinat, ursprungsy] != -egetDragvärde; xkoordinat++)
            {
                if (spelplan[xkoordinat, ursprungsy] == 0)
                {
                    double dragvärde = BeräknaDragvärde(new Form1.Koordinat(xkoordinat, ursprungsy));

                    if (dragvärde > nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Clear();
                        likvärdigaDrag.Add(new Form1.Koordinat(xkoordinat, ursprungsy));
                        nuvarandeDragvärde = dragvärde;
                    }
                    else if (dragvärde == nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Add(new Form1.Koordinat(xkoordinat, ursprungsy));
                    }

                    break;
                }
            }

            #endregion

            #region Vänster

            for (int xkoordinat = ursprungsx - 1; xkoordinat >= 0 && xkoordinat <= Form1.spelplansgräns && spelplan[xkoordinat, ursprungsy] != -egetDragvärde; xkoordinat--)
            {
                if (spelplan[xkoordinat, ursprungsy] == 0)
                {
                    double dragvärde = BeräknaDragvärde(new Form1.Koordinat(xkoordinat, ursprungsy));

                    if (dragvärde > nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Clear();
                        likvärdigaDrag.Add(new Form1.Koordinat(xkoordinat, ursprungsy));
                        nuvarandeDragvärde = dragvärde;
                    }
                    else if (dragvärde == nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Add(new Form1.Koordinat(xkoordinat, ursprungsy));
                    }

                    break;
                }
            }

            #endregion

            #region upp

            for (int ykoordinat = ursprungsy - 1; ykoordinat >= 0 && ykoordinat <= Form1.spelplansgräns && spelplan[ursprungsx, ykoordinat] != -egetDragvärde; ykoordinat--)
            {
                if (spelplan[ursprungsx, ykoordinat] == 0)
                {
                    double dragvärde = BeräknaDragvärde(new Form1.Koordinat(ursprungsx, ykoordinat));

                    if (dragvärde > nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Clear();
                        likvärdigaDrag.Add(new Form1.Koordinat(ursprungsx, ykoordinat));
                        nuvarandeDragvärde = dragvärde;
                    }
                    else if (dragvärde == nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Add(new Form1.Koordinat(ursprungsx, ykoordinat));
                    }

                    break;
                }
            }

            #endregion

            #region ned

            for (int ykoordinat = ursprungsy + 1; ykoordinat >= 0 && ykoordinat <= Form1.spelplansgräns && spelplan[ursprungsx, ykoordinat] != -egetDragvärde; ykoordinat++)
            {
                if (spelplan[ursprungsx, ykoordinat] == 0)
                {
                    double dragvärde = BeräknaDragvärde(new Form1.Koordinat(ursprungsx, ykoordinat));

                    if (dragvärde > nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Clear();
                        likvärdigaDrag.Add(new Form1.Koordinat(ursprungsx, ykoordinat));
                        nuvarandeDragvärde = dragvärde;
                    }
                    else if (dragvärde == nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Add(new Form1.Koordinat(ursprungsx, ykoordinat));
                    }

                    break;
                }
            }

            #endregion

            #region snett ned höger

            for (int koordinatökning = 1; ursprungsx + koordinatökning >= 0 && ursprungsy + koordinatökning >= 0 && ursprungsx + koordinatökning <= Form1.spelplansgräns && ursprungsy + koordinatökning <= Form1.spelplansgräns && spelplan[ursprungsx + koordinatökning, ursprungsy + koordinatökning] != -egetDragvärde; koordinatökning++)
            {
                if (spelplan[ursprungsx + koordinatökning, ursprungsy + koordinatökning] == 0)
                {
                    double dragvärde = BeräknaDragvärde(new Form1.Koordinat(ursprungsx + koordinatökning, ursprungsy + koordinatökning));

                    if (dragvärde > nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Clear();
                        likvärdigaDrag.Add(new Form1.Koordinat(ursprungsx + koordinatökning, ursprungsy + koordinatökning));
                        nuvarandeDragvärde = dragvärde;
                    }
                    else if (dragvärde == nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Add(new Form1.Koordinat(ursprungsx + koordinatökning, ursprungsy + koordinatökning));
                    }

                    break;
                }
            }

            #endregion

            #region snett ned vänster

            for (int koordinatminskning = -1; ursprungsx + koordinatminskning >= 0 && ursprungsy + koordinatminskning >= 0 && ursprungsx + koordinatminskning <= Form1.spelplansgräns && ursprungsy + koordinatminskning <= Form1.spelplansgräns
                && spelplan[ursprungsx + koordinatminskning, ursprungsy + koordinatminskning] != -egetDragvärde; koordinatminskning--)
            {
                if (spelplan[ursprungsx + koordinatminskning, ursprungsy + koordinatminskning] == 0)
                {
                    double dragvärde = BeräknaDragvärde(new Form1.Koordinat(ursprungsx + koordinatminskning, ursprungsy + koordinatminskning));

                    if (dragvärde > nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Clear();
                        likvärdigaDrag.Add(new Form1.Koordinat(ursprungsx + koordinatminskning, ursprungsy + koordinatminskning));
                        nuvarandeDragvärde = dragvärde;
                    }
                    else if (dragvärde == nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Add(new Form1.Koordinat(ursprungsx + koordinatminskning, ursprungsy + koordinatminskning));
                    }

                    break;
                }
            }

            #endregion

            #region snett upp höger

            for (int koordinatföränding = 1; ursprungsy - koordinatföränding >= 0 && ursprungsx + koordinatföränding <= Form1.spelplansgräns &&
                spelplan[ursprungsx + koordinatföränding, ursprungsy - koordinatföränding] != -egetDragvärde; koordinatföränding++)
            {
                if (spelplan[ursprungsx + koordinatföränding, ursprungsy - koordinatföränding] == 0)
                {
                    double dragvärde = BeräknaDragvärde(new Form1.Koordinat(ursprungsx + koordinatföränding, ursprungsy - koordinatföränding));

                    if (dragvärde > nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Clear();
                        likvärdigaDrag.Add(new Form1.Koordinat(ursprungsx + koordinatföränding, ursprungsy - koordinatföränding));
                        nuvarandeDragvärde = dragvärde;
                    }
                    else if (dragvärde == nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Add(new Form1.Koordinat(ursprungsx + koordinatföränding, ursprungsy - koordinatföränding));
                    }

                    break;
                }
            }

            #endregion

            #region snett upp vänster

            for (int koordinatföränding = 1; ursprungsx - koordinatföränding >= 0 && ursprungsy + koordinatföränding <= Form1.spelplansgräns &&
               spelplan[ursprungsx - koordinatföränding, ursprungsy + koordinatföränding] != -egetDragvärde; koordinatföränding++)
            {
                if (spelplan[ursprungsx - koordinatföränding, ursprungsy + koordinatföränding] == 0)
                {
                    double dragvärde = BeräknaDragvärde(new Form1.Koordinat(ursprungsx - koordinatföränding, ursprungsy + koordinatföränding));

                    if (dragvärde > nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Clear();
                        likvärdigaDrag.Add(new Form1.Koordinat(ursprungsx - koordinatföränding, ursprungsy + koordinatföränding));
                        nuvarandeDragvärde = dragvärde;
                    }
                    else if (dragvärde == nuvarandeDragvärde)
                    {
                        likvärdigaDrag.Add(new Form1.Koordinat(ursprungsx - koordinatföränding, ursprungsy + koordinatföränding));
                    }

                    break;
                }
            }

            #endregion

            #endregion

            senasteDragvärde = BeräknaDragvärde(likvärdigaDrag[0]);
        }
    }
}
