using System.Drawing;

namespace Luffarschacksprojekt
{
      public class ArgumentFörSpelare
      {
            public Spelare.Tecken   spelarTecken;
            public Color            spelarFärg;
            public int              starttid;
            public Spelare.Typ      typ;

            public ArgumentFörSpelare(Spelare.Tecken spelarensTecken, Color spelarensFärg, int starttid, Spelare.Typ spelarensTyp)
            {
                  this.spelarTecken = spelarensTecken;
                  this.spelarFärg = spelarensFärg;
                  this.starttid = starttid;
                  this.typ = spelarensTyp;
            }
      }
}
