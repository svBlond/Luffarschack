using System.Collections.Generic;

namespace Luffarschacksprojekt
{

      public static class Bräde
      {
            public const int sida = 15; // Antal fält per sida
            public const int antalFält = sida * sida;
            static public int[,] ställning = new int[sida, sida]; // Där dragen lagras.
            static public List<Fält> gjordaDrag = new List<Fält>(); // Lagrar dragen i den ordningen de utförts så att man kan ångra dem
            static public List<Fält> möjligaDrag = new List<Fält>();// Alla fält som gränsar till ett gjort drag
            static public readonly Fält nåtHarGåttFel = new Fält(-13, -37);

            public struct Fält
            {
                  public int X;
                  public int Y;

                  public Fält(int x, int y)
                  {
                        this = new Fält();
                        this.X = x;
                        this.Y = y;
                  }

                  public static bool operator ==(Fält koordinat1, Fält koordinat2)
                  {
                        return koordinat1.X == koordinat2.X && koordinat1.Y == koordinat2.Y;
                  }

                  public static bool operator !=(Fält koordinat1, Fält koordinat2)
                  {
                        return koordinat1.X != koordinat2.X || koordinat1.Y != koordinat2.Y;
                  }
            }

            public static void skrivDebug()
            {

            }

            public static void uppdateraMöjligaDrag(Fält nyttDrag)
            {
                  möjligaDrag.Remove(nyttDrag);

                  for (int x = nyttDrag.X - 1; x <= nyttDrag.X + 1 && x < sida; x++) {
                        for (int y = nyttDrag.Y - 1; y <= nyttDrag.Y + 1 && y < sida; y++) {
                              if (x >= 0 && y >= 0 && ställning[x, y] == 0 && !möjligaDrag.Contains(new Fält(x, y))) {
                                    möjligaDrag.Add(new Fält(x, y));
                              }
                        }
                  }
            }

            public static bool ÄrVinstdrag(Bräde.Fält drag, int teckenvärde)
            {
                  if (SammaRad(drag, teckenvärde) >= 4 || SammaLinje(drag, teckenvärde) >= 4 ||
                      SnettUppHöger(drag, teckenvärde) >= 4 || SnettUppVänster(drag, teckenvärde) >= 4) {
                        return true;
                  }

                  return false;
            }

            // Använd inte ordet hot
            #region Räknar ut hot i en viss rikning

            // Kollar vänster-höger
            public static int SammaRad(Bräde.Fält drag, int påDrag /*<- Vilken spelares drag den ska leta efter */)
            {
                  int antal = 0;

                  // Går igenom de fyra fälten närmast till höger
                  for (int indexökning = drag.X + 1; indexökning <= drag.X + 4; indexökning++) {
                        // Om den hamnat utanför spelplanen
                        if (indexökning >= Bräde.sida) {
                              break;
                        }
                        // Om det finns ett eget drag på fältet
                        if (Bräde.ställning[indexökning, drag.Y] == påDrag) {
                              antal++;
                        } else {
                              break;
                        }
                  }

                  // Går igenom de fyra fälten närmast till vänster
                  for (int indexminskning = drag.X - 1; indexminskning >= drag.X - 4; indexminskning--) {
                        // Om den hamnat utanför spelanen
                        if (indexminskning < 0) {
                              break;
                        }
                        // Om det finns ett eget drag på fältet
                        if (Bräde.ställning[indexminskning, drag.Y] == påDrag)
                              antal++;
                        else {
                              break;
                        }
                  }

                  return antal;
            }

            // Kollar upp-ner
            public static int SammaLinje(Bräde.Fält drag, int påDrag /*<- Vilken spelares drag den ska leta efter */)
            {
                  int antal = 0;

                  for (int indexökning = drag.Y + 1; indexökning <= drag.Y + 4; indexökning++) // Kollar de fyra fälten närmast nedanför
            {
                        if (indexökning >= Bräde.sida) // Om den hamnat utanför spelanen
                {
                              break;
                        }
                        if (Bräde.ställning[drag.X, indexökning] == påDrag) // Om det finns ett eget drag på fältet
                              antal++;
                        else {
                              break;
                        }
                  }

                  for (int indexminskning = drag.Y - 1; indexminskning >= drag.Y - 4; indexminskning--) // Kollar de fyra fälten närmast ovanför
            {
                        if (indexminskning < 0) // Om den hamnat utanför spelanen
                {
                              break;
                        }
                        if (Bräde.ställning[drag.X, indexminskning] == påDrag) // Om det finns ett eget drag på fältet
                              antal++;
                        else {
                              break;
                        }
                  }

                  return antal;
            }

            public static int SnettUppVänster(Bräde.Fält drag, int påDrag /*<- Vilken spelares drag den ska leta efter */)
            {
                  int antal = 0;

                  for (int indexökning = 1; indexökning <= 4; indexökning++) // Kollar de fyra fälten närmast snett upp vänster
            {
                        if (drag.X + indexökning >= Bräde.sida || drag.Y + indexökning >= Bräde.sida) // Om den hamnat utanför spelanen
                {
                              break;
                        }
                        if (Bräde.ställning[drag.X + indexökning, drag.Y + indexökning] == påDrag) // Om det finns ett eget drag på fältet
                              antal++;
                        else {
                              break;
                        }
                  }

                  for (int indexökning = 1; indexökning <= 4; indexökning++) // Kollar de fyra fälten närmast snett ned höger
            {
                        if (drag.X - indexökning < 0 || drag.Y - indexökning < 0) // Om den hamnat utanför spelanen
                {
                              break;
                        }
                        if (Bräde.ställning[drag.X - indexökning, drag.Y - indexökning] == påDrag) // Om det finns ett eget drag på fältet
                              antal++;
                        else {
                              break;
                        }
                  }

                  return antal;
            }

            public static int SnettUppHöger(Bräde.Fält drag, int påDrag /*<- Vilken spelares drag den ska leta efter */)
            {
                  int antal = 0;

                  for (int indexökning = 1; indexökning <= 4; indexökning++) // Kollar de fyra fälten närmast snett upp höger
            {
                        if (drag.X + indexökning >= Bräde.sida || drag.Y - indexökning < 0) // Om den hamnat utanför spelanen
                {
                              break;
                        } else if (Bräde.ställning[drag.X + indexökning, drag.Y - indexökning] == påDrag) // Om det finns ett eget drag på fältet
                {
                              antal++;
                        } else {
                              break;
                        }
                  }

                  for (int indexökning = 1; indexökning <= 4; indexökning++) // Kollar de fyra fälten närmast snett ned vänster
            {
                        if (drag.X - indexökning < 0 || drag.Y + indexökning >= Bräde.sida) // Om den hamnat utanför spelanen
                {
                              break;
                        }
                        if (Bräde.ställning[drag.X - indexökning, drag.Y + indexökning] == påDrag) // Om det finns ett eget drag på fältet
                              antal++;
                        else {
                              break;
                        }
                  }

                  return antal;
            }
            #endregion
      }
}
