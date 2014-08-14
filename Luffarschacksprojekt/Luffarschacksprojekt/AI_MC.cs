using System;
using System.Collections.Generic;

namespace Luffarschacksprojekt
{
      // Trasig
      public class MonteCarloAI : AI
      {
            // Parametrarna används när två drag har lika bra statistik
            // INTE när motorn spelar mot sig själv
            public MonteCarloAI(int egetTeckenvärde,
                                double sekundärhotKonstant = 0,
                                double totalaHotKonstant = 0,
                                double motståndarhotKonstant = 0,
                                double motståndarsekundärKonstant = 0,
                                double motståndartotalKonstant = 0,
                                double motståndarminus = 0)
            {
                  this.egetTeckenvärde = egetTeckenvärde;
                  this.sekundärhotkonstant = sekundärhotKonstant;
                  this.totalaHotKonstant = totalaHotKonstant;
                  this.motståndarhotkonstant = motståndarhotKonstant;
                  this.motståndarsekundärkonstant = motståndarsekundärKonstant;
                  this.motståndartotalkonstant = motståndartotalKonstant;
                  this.motståndarminus = motståndarminus;
            }

            public override Bräde.Fält BestämDrag()
            {
                  List<Bräde.Fält> dragAttTaBort = new List<Bräde.Fält>();
                  Bräde.Fält bästaDrag = new Bräde.Fält();
                  int vinstdifferens = int.MinValue;
                  // De AI:er som används i de simulerade partierna
                  OriginalgangsterAI egnaSidan = new OriginalgangsterAI(egetTeckenvärde, tolerans: 0);
                  OriginalgangsterAI andraSidan = new OriginalgangsterAI(-egetTeckenvärde, tolerans: 0);

                  foreach (Bräde.Fält fält in Bräde.möjligaDrag) {
                        int xkoordinat = fält.X;
                        int ykoordinat = fält.Y;
                        Bräde.Fält testdrag = new Bräde.Fält(xkoordinat, ykoordinat);

                        if (Bräde.ÄrVinstdrag(testdrag, egetTeckenvärde)) {
                              return testdrag;
                        } else {
                              int antalVinster = 0;
                              int antalFörluster = 0;

                              // Sätter in draget i spelplanen tillfälligt
                              Bräde.ställning[xkoordinat, ykoordinat] = egetTeckenvärde;

                              for (int parti = 0; parti < 100; parti++) {
                                    Bräde.Fält drag;

                                    for (int dragNummer = 0; dragNummer < 25; dragNummer++) {
                                          drag = andraSidan.BestämDrag();

                                          // Spelplanen är full
                                          // Partiet är oavgjort
                                          if (drag == Bräde.nåtHarGåttFel) {
                                                taBortFrånSpelplanen(dragAttTaBort);
                                                dragAttTaBort.Clear();
                                                break;
                                          }
                                                // Förlust
                                          else if (Bräde.ÄrVinstdrag(drag, -egetTeckenvärde)) {
                                                antalFörluster++;
                                                taBortFrånSpelplanen(dragAttTaBort);
                                                dragAttTaBort.Clear();
                                                break;
                                          }
                                                // Partiet fortsätter
                                          else {
                                                Bräde.ställning[drag.X, drag.Y] = -egetTeckenvärde;
                                                dragAttTaBort.Add(drag);
                                          }

                                          drag = egnaSidan.BestämDrag();

                                          // Spelplanen är full
                                          // Partiet är oavgjort
                                          if (drag == Bräde.nåtHarGåttFel) {
                                                taBortFrånSpelplanen(dragAttTaBort);
                                                dragAttTaBort.Clear();
                                                break;
                                          }
                                                // Vinst
                                          else if (Bräde.ÄrVinstdrag(drag, egetTeckenvärde)) {
                                                antalVinster++;
                                                taBortFrånSpelplanen(dragAttTaBort);
                                                dragAttTaBort.Clear();
                                                break;
                                          }
                                                // Partiet fortsätter
                                          else {
                                                Bräde.ställning[drag.X, drag.Y] = egetTeckenvärde;
                                                dragAttTaBort.Add(drag);
                                          }
                                    }
                              }

                              taBortFrånSpelplanen(dragAttTaBort);
                              dragAttTaBort.Clear();
                              int testDifferens = antalVinster - antalFörluster;

                              if (testDifferens > vinstdifferens) {
                                    vinstdifferens = testDifferens;
                                    bästaDrag = testdrag;
                              } else if (testDifferens == vinstdifferens
                                        && Math.Max(BeräknaDragvärde(testdrag, egetTeckenvärde),
                                                    BeräknaDragvärde(testdrag, -egetTeckenvärde))
                                        > Math.Max(BeräknaDragvärde(bästaDrag, egetTeckenvärde),
                                                    BeräknaDragvärde(bästaDrag, -egetTeckenvärde))) {
                                    vinstdifferens = testDifferens;
                                    bästaDrag = testdrag;
                              }

                              // Tar bort draget ur spelplanen
                              Bräde.ställning[testdrag.X, testdrag.Y] = 0;
                        }
                  }

                  return bästaDrag;
            }

            private void taBortFrånSpelplanen(List<Bräde.Fält> dragAttTaBort)
            {
                  foreach (Bräde.Fält drag in dragAttTaBort) {
                        Bräde.ställning[drag.X, drag.Y] = 0;
                  }
            }
      }
}
