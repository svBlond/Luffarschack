
namespace Luffarschacksprojekt
{
      public abstract class AI
      {
            // Hur mycket olika positionella faktorer prioriteras
            // Orden beskrivs i dokumentation.html
            // TODO skapa dokumentation.html
            protected double sekundärhotkonstant;
            protected double totalaHotKonstant;
            protected double motståndarhotkonstant;
            protected double motståndarsekundärkonstant;
            protected double motståndartotalkonstant;
            protected double motståndarminus;
            protected int egetTeckenvärde;

            // Ska finnas i varje arvklass; returnerar vilket drag motorn gör.
            public virtual Bräde.Fält BestämDrag()
            {
                  return Bräde.nåtHarGåttFel;
            }

            static void GörSkit(ref double största, ref double nästStörsta, ref double summa, double[] vektor)
            {
                  for (int i = 0; i < 4; i++) {
                        summa += vektor[i];
                        if (vektor[i] >= största) {
                              nästStörsta = största;
                              största = vektor[i];
                        } else if (vektor[i] > nästStörsta) {
                              nästStörsta = vektor[i];
                        }
                  }
            }

            // Dragvärde = bästa hotet + näst bästa hotet * sekundärhotskonstant + summan av hoten * totalaHotKonstant
            protected double BeräknaDragvärde(Bräde.Fält drag, int teckenvärde)
            {
                  double största = double.MinValue;
                  double nästStörsta = double.MinValue;
                  double summa = 0;
                  double[] vektor = new double[] { HotLängsRad(drag, teckenvärde), HotLängsLinje(drag, teckenvärde),
                                                   HotLängsDiagonalUppHöger(drag, teckenvärde),
                                                   HotLängsDiagonalUppVänster(drag, teckenvärde) };

                  GörSkit(ref största, ref nästStörsta, ref summa, vektor);

                  if (teckenvärde == egetTeckenvärde) {
                        return största + nästStörsta * sekundärhotkonstant + summa * totalaHotKonstant;
                  } else {
                        return största + nästStörsta * sekundärhotkonstant + summa * totalaHotKonstant - motståndarminus;
                  }
            }

            #region Räknar ut hot i en viss rikning

            // Kollar höger-vänster
            internal double HotLängsRad(Bräde.Fält drag, int påDrag /*<- Vilken spelares drag den ska leta efter */)
            {
                  double antal = 0;
                  bool blockÅtEnaHållet = false; // Ifall serien gränsar till fält med ett motståndardrag ellet spelplanens kant

                  // Går igenom de fyra fälten närmast till höger
                  for (int indexökning = drag.X + 1; indexökning <= drag.X + 4; indexökning++) {
                        // Om den hamnat utanför spelanen
                        if (indexökning >= Bräde.sida) {
                              blockÅtEnaHållet = true;
                              break;
                        }
                        // Om det finns ett eget drag på fältet
                        if (Bräde.ställning[indexökning, drag.Y] == påDrag) {
                              antal++;
                        }
                              // Om det finns ett motståndardrag på fältet
                        else {
                              if (Bräde.ställning[indexökning, drag.Y] == -påDrag)
                                    blockÅtEnaHållet = true;
                              // Annars är den öppen på höger sida 
                              break;
                        }
                  }

                  // Går igenom de fyra fälten närmast till vänster
                  for (int indexminskning = drag.X - 1; indexminskning >= drag.X - 4; indexminskning--) {
                        if (indexminskning < 0) // Om den hamnat utanför spelanen
                {
                              if (blockÅtEnaHållet)
                                    // Är den helt blockerad är den värd 0,
                                    // om den inte är en femma i vilket fall den får värdet 4
                                    return antal == 4 ? 4 : 0;
                              else
                                    blockÅtEnaHållet = true;
                              break;
                        }
                        // Om det finns ett eget drag på fältet
                        if (Bräde.ställning[indexminskning, drag.Y] == påDrag)
                              antal++;
                        // Om det finns ett motståndardrag på fältet
                        else {
                              if (Bräde.ställning[indexminskning, drag.Y] == -påDrag) {
                                    if (blockÅtEnaHållet)
                                          // Är den helt blockerad är den värd 0, 
                                          // om den inte är en femma  i vilket fall den får värdet 4
                                          return antal == 4 ? 4 : 0;
                                    else
                                          blockÅtEnaHållet = true;
                              }
                              break;
                        }
                  }

                  antal += blockÅtEnaHållet ? 0 : 0.5; // Är serien helt öppen får den en bonus på 0,5
                  return antal;
            }

            // Kollar upp-ner
            internal double HotLängsLinje(Bräde.Fält drag, int påDrag /*<- Vilken spelares drag den ska leta efter */)
            {
                  double antal = 0;
                  bool blockÅtEnaHållet = false; // Ifall serien gränsar till fält med ett motståndardrag ellet spelplanens kant

                  for (int indexökning = drag.Y + 1; indexökning <= drag.Y + 4; indexökning++) // Kollar de fyra fälten närmast nedanför
                  {
                        if (indexökning >= Bräde.sida) // Om den hamnat utanför spelanen
                        {
                              blockÅtEnaHållet = true;
                              break;
                        }
                        if (Bräde.ställning[drag.X, indexökning] == påDrag) // Om det finns ett eget drag på fältet
                              antal++;
                        else {
                              if (Bräde.ställning[drag.X, indexökning] == -påDrag) // Om det finns ett motståndardrag på fältet
                                    blockÅtEnaHållet = true;
                              // Annars är den öppen på ovansidan
                              break;
                        }
                  }

                  for (int indexminskning = drag.Y - 1; indexminskning >= drag.Y - 4; indexminskning--) // Kollar de fyra fälten närmast ovanför
                  {
                        if (indexminskning < 0) // Om den hamnat utanför spelanen
                        {
                              if (blockÅtEnaHållet)
                                    return antal == 4 ? 4 : 0; // Är den helt blockerad är den värd noll, om den inte är en femma i vilket fall den får värdet 4
                              else
                                    blockÅtEnaHållet = true;
                              break;
                        }
                        if (Bräde.ställning[drag.X, indexminskning] == påDrag) // Om det finns ett eget drag på fältet
                              antal++;
                        else {
                              if (Bräde.ställning[drag.X, indexminskning] == -påDrag) // Om det finns ett motståndardrag på fältet
                              {
                                    if (blockÅtEnaHållet)
                                          return antal == 4 ? 4 : 0; // Är den helt blockerad är den värd noll, om den inte är en femma i vilket fall den får värdet 4
                                    else
                                          blockÅtEnaHållet = true;
                              }
                              break;
                        }
                  }

                  antal += blockÅtEnaHållet ? 0 : 0.5; // Är serien helt öppen får den en bonus på 0,5
                  return antal;
            }

            internal double HotLängsDiagonalUppVänster(Bräde.Fält drag, int påDrag /*<- Vilken spelares drag den ska leta efter */)
            {
                  double antal = 0;
                  bool blockÅtEnaHållet = false; // Ifall serien gränsar till fält med ett motståndardrag ellet spelplanens kant

                  for (int indexökning = 1; indexökning <= 4; indexökning++) // Kollar de fyra fälten närmast snett upp vänster
                  {
                        if (drag.X + indexökning >= Bräde.sida || drag.Y + indexökning >= Bräde.sida) // Om den hamnat utanför spelanen
                        {
                              blockÅtEnaHållet = true;
                              break;
                        }
                        if (Bräde.ställning[drag.X + indexökning, drag.Y + indexökning] == påDrag) // Om det finns ett eget drag på fältet
                              antal++;
                        else {
                              if (Bräde.ställning[drag.X + indexökning, drag.Y + indexökning] == -påDrag) // Om det finns ett motståndardrag på fältet
                                    blockÅtEnaHållet = true;
                              // Annars är den öppen på ovansidan 
                              break;
                        }
                  }

                  for (int indexökning = 1; indexökning <= 4; indexökning++) // Kollar de fyra fälten närmast snett ned höger
            {
                        if (drag.X - indexökning < 0 || drag.Y - indexökning < 0) // Om den hamnat utanför spelanen
                {
                              if (blockÅtEnaHållet)
                                    return antal == 4 ? 4 : 0; // Är den helt blockerad är den värd noll, om den inte är en femma i vilket fall den får värdet 4
                              else
                                    blockÅtEnaHållet = true;
                              break;
                        }
                        if (Bräde.ställning[drag.X - indexökning, drag.Y - indexökning] == påDrag) // Om det finns ett eget drag på fältet
                              antal++;
                        else {
                              if (Bräde.ställning[drag.X - indexökning, drag.Y - indexökning] == -påDrag) // Om det finns ett motståndardrag på fältet
                    {
                                    if (blockÅtEnaHållet)
                                          return antal == 4 ? 4 : 0; // Är den helt blockerad är den värd noll, om den inte är en femma i vilket fall den får värdet 4
                                    else
                                          blockÅtEnaHållet = true;
                              }
                              break;
                        }
                  }

                  antal += blockÅtEnaHållet ? 0 : 0.5; // Är serien helt öppen får den en bonus på 0,5
                  return antal;
            }

            internal double HotLängsDiagonalUppHöger(Bräde.Fält drag, int påDrag /*<- Vilken spelares drag den ska leta efter */)
            {
                  double antal = 0;
                  bool blockÅtEnaHållet = false; // Ifall serien gränsar till fält med ett motståndardrag ellet spelplanens kant

                  for (int indexökning = 1; indexökning <= 4; indexökning++) // Kollar de fyra fälten närmast snett upp höger
                  {
                        if (drag.X + indexökning >= Bräde.sida || drag.Y - indexökning < 0) // Om den hamnat utanför spelanen
                        {
                              blockÅtEnaHållet = true;
                              break;
                        } else if (Bräde.ställning[drag.X + indexökning, drag.Y - indexökning] == påDrag) // Om det finns ett eget drag på fältet
                              antal++;
                        else {
                              if (Bräde.ställning[drag.X + indexökning, drag.Y - indexökning] == -påDrag) // Om det finns ett motståndardrag på fältet
                                    blockÅtEnaHållet = true;
                              // Annars är den öppen på ovansidan 
                              break;
                        }
                  }

                  for (int indexökning = 1; indexökning <= 4; indexökning++) // Kollar de fyra fälten närmast snett ned vänster
                        {
                        if (drag.X - indexökning < 0 || drag.Y + indexökning >= Bräde.sida) // Om den hamnat utanför spelanen
                        {
                              if (blockÅtEnaHållet)
                                    return antal == 4 ? 4 : 0; // Är den helt blockerad är den värd noll, om den inte är en femma i vilket fall den får värdet 4
                              else
                                    blockÅtEnaHållet = true;
                              break;
                        }
                        if (Bräde.ställning[drag.X - indexökning, drag.Y + indexökning] == påDrag) // Om det finns ett eget drag på fältet
                              antal++;
                        else {
                              if (Bräde.ställning[drag.X - indexökning, drag.Y + indexökning] == -påDrag) // Om det finns ett motståndardrag på fältet
                              {
                                    if (blockÅtEnaHållet)
                                          return antal == 4 ? 4 : 0; // Är den helt blockerad är den värd noll, om den inte är en femma i vilket fall den får värdet 4
                                    else
                                          blockÅtEnaHållet = true;
                              }
                              break;
                        }
                  }

                  antal += blockÅtEnaHållet ? 0 : 0.5; // Är serien helt öppen får den en bonus på 0,5
                  return antal;
            }
            #endregion

      }
}

