using System;
using System.Drawing;

namespace Luffarschacksprojekt
{
      public class Spelare
      {
            public AI         användAI;
            public DateTime   tid;
            public Tecken     EgetTecken  { get; private set; }
            public int        Teckenvärde { get; private set; }
            public Color      Teckenfärg  { get; private set; }
            public Typ        EgenTyp     { get; private set; }

            public enum Tecken
            {
                  Cirkel = 1,
                  Kryss = -1,
            }

            public enum Typ
            {
                  Människa,
                  Originalgangster,
                  MonteCarlo
            }

            public Spelare(ArgumentFörSpelare parametrar)
            {
                  this.EgetTecken = parametrar.spelarTecken;
                  this.Teckenvärde = (int)parametrar.spelarTecken;
                  this.Teckenfärg = parametrar.spelarFärg;
                  this.EgenTyp = parametrar.typ;
                  tid = new DateTime().AddMinutes(parametrar.starttid);

                  if (parametrar.typ == Typ.MonteCarlo) {
                        // Parametrarna används när två drag har lika bra statistik
                        // INTE när motorn spelar mot sig själv
                        this.användAI = new MonteCarloAI((int)this.EgetTecken,
                                                            motståndarminus: 0.1,
                                                            sekundärhotKonstant: 0.01,
                                                            motståndarhotKonstant: 0.001,
                                                            totalaHotKonstant: 0.0001,
                                                            motståndarsekundärKonstant: 0.00001,
                                                            motståndartotalKonstant: 0.000001);
                  } else if (parametrar.typ == Typ.Originalgangster) {
                        this.användAI = new OriginalgangsterAI((int)this.EgetTecken,
                                                                0,
                                                                motståndarminus: 0.1,
                                                                sekundärhotKonstant: 0.01,
                                                                motståndarhotKonstant: 0.01);
                  }
            }

      }
}
