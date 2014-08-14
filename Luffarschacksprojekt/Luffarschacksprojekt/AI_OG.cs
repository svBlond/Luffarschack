using System;
using System.Collections.Generic;

namespace Luffarschacksprojekt
{
      public class OriginalgangsterAI : AI
      {
            private double tolerans;

            public OriginalgangsterAI(int egetTeckenvärde,
                                        double tolerans = 0,
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
                  this.tolerans = tolerans;
            }

            public List<Bräde.Fält> HittaKandidatdrag()
            {
                  double hittillsBästaHot = int.MinValue;
                  List<Bräde.Fält> kandidatdrag = new List<Bräde.Fält>();

                  foreach (var item in Bräde.möjligaDrag) {
                        Bräde.Fält drag = new Bräde.Fält(item.X, item.Y);

                        double potentielltEgetVärde = BeräknaDragvärde(drag, egetTeckenvärde);
                        double potentielltMotståndarvärde = BeräknaDragvärde(drag, -egetTeckenvärde);
                        double störstaHotPåFältet = Math.Max(potentielltEgetVärde, potentielltMotståndarvärde);

                        if (störstaHotPåFältet - hittillsBästaHot > tolerans) {
                              hittillsBästaHot = störstaHotPåFältet;
                              kandidatdrag.Clear();
                              kandidatdrag.Add(drag);
                        } else if (störstaHotPåFältet - hittillsBästaHot >= -tolerans) {
                              kandidatdrag.Add(drag);
                        }
                  }

                  return kandidatdrag;
            }

            public override Bräde.Fält BestämDrag()
            {
                  List<Bräde.Fält> kandidatdrag = HittaKandidatdrag();

                  // Om den hittar minst ett drag
                  if (kandidatdrag.Count > 0) {
                        return kandidatdrag[new Random(DateTime.Now.Millisecond).Next(kandidatdrag.Count)];
                  } else {
                        return Bräde.nåtHarGåttFel; // Om den inte hittar några kandidatdrag returneras ett omöjligt drag
                  }
            }

      }
}
