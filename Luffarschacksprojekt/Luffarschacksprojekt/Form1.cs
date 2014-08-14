using System;
using System.Drawing;
using System.Windows.Forms;

namespace Luffarschacksprojekt
{
      public partial class Form1 : Form
      {
            private const int fältstorlek = 42; // Fältstorlek i pixlar
            private static int spelplansstorlekX = Bräde.sida * fältstorlek;  // Den fysiska storleken för spelplanen i lodrätt
            private static int spelplansstorlekY = Bräde.sida * fältstorlek;  //                                        vågrätt     
            private static int teckenstorlek;  // Storleken för krýssen och ringarna
            private Spelare spelareEtt; // Spelaren som gör första draget
            private Spelare spelareTvå; //                  andra
            private Spelare spelarePåDrag; // Spelaren som ska göra nästa drag
            private static Rectangle spelareEttsKlockposition; // Rektangeln där spelareEtts tid skrivs
            private static Rectangle spelareTvåsKlockposition; //                spelareTvås
            private ArgumentFörSpelare spelareEttsArgument;    // Argumenten till spelareEtts konstruktor
            private ArgumentFörSpelare spelareTvåsArgument;    //                 spelareTvås

            public Form1()
            {
                  InitializeComponent();

                  #region Fixar font storleken
                  Graphics g = CreateGraphics();

                  for (int storleksIndex = 8; storleksIndex < 256; storleksIndex++) {
                        SizeF storleken = g.MeasureString("X", new Font(FontFamily.GenericSerif, storleksIndex));
                        if (storleken.Height >= fältstorlek) {
                              teckenstorlek = storleksIndex + 4;
                              break;
                        }
                  }
                  #endregion

                  spelplansstorlekY += menuStrip1.Size.Height;

                  // Sätter fönsterstorleken
                  this.ClientSize = new System.Drawing.Size(spelplansstorlekX + 400, spelplansstorlekY);

                  // Initialiserar startknappen
                  Button startKnapp = new Button();
                  startKnapp.Text = "Nytt parti";
                  startKnapp.Size = new Size(200, 50);
                  startKnapp.Location = new Point(spelplansstorlekX + 100, spelplansstorlekY / 2 + menuStrip1.Height + 100);
                  this.Controls.Add(startKnapp);
                  startKnapp.Click += new EventHandler(startKnapp_Click);

                  // Initialiserar ångraknappen
                  Button ångraKnapp = new Button();
                  ångraKnapp.Text = "Ångra drag";
                  ångraKnapp.Size = new Size(200, 50);
                  ångraKnapp.Location = new Point(spelplansstorlekX + 100, spelplansstorlekY / 2 + menuStrip1.Height + 175);
                  this.Controls.Add(ångraKnapp);
                  ångraKnapp.Click += ångraKnapp_Click;

                  // Sätter positionerna för rektanglarna där tiderna skrivs
                  spelareEttsKlockposition = new Rectangle(spelplansstorlekX + 50, menuStrip1.Height + 30, 300, 75);
                  spelareTvåsKlockposition = new Rectangle(spelplansstorlekX + 50, menuStrip1.Height + 200, 300, 75);

                  spelareEttsArgument = new ArgumentFörSpelare(Spelare.Tecken.Cirkel, Color.Black, 100, Spelare.Typ.Originalgangster);
                  spelareTvåsArgument = new ArgumentFörSpelare(Spelare.Tecken.Kryss, Color.Black, 100, Spelare.Typ.Människa);
                  spelareEtt = new Spelare(spelareEttsArgument);
                  spelareTvå = new Spelare(spelareTvåsArgument);
                  spelarePåDrag = spelareEtt;
            }

            private void ångraKnapp_Click(object sender, EventArgs e)
            {
                  if (Bräde.gjordaDrag.Count > 1) {
                        // Tar bort de senaste två dragen, motordraget som just gjordes och spelarens senaste drag
                        for (int i = 0; i < 2; i++) {
                              // Indexet för det senaste draget i listan gjordaDrag
                              int gjordaDragIndex = Bräde.gjordaDrag.Count - 1;
                              Graphics g = CreateGraphics();

                              // Ritar över de borttagna dragen
                              g.DrawString(Bräde.ställning[Bräde.gjordaDrag[gjordaDragIndex].X,
                                  Bräde.gjordaDrag[gjordaDragIndex].Y] == (int)Spelare.Tecken.Cirkel ? "O" : "X",
                                  new Font(FontFamily.GenericSerif, teckenstorlek), new SolidBrush(Color.Silver),
                                  new PointF(Bräde.gjordaDrag[gjordaDragIndex].X * fältstorlek,
                                             Bräde.gjordaDrag[gjordaDragIndex].Y * fältstorlek + menuStrip1.Height));

                              // Tar bort draget från spelplansarrayen och listan över gjorda drag
                              // och lägger tillbaks fältet i listan med möjliga drag
                              Bräde.ställning[Bräde.gjordaDrag[gjordaDragIndex].X, Bräde.gjordaDrag[gjordaDragIndex].Y] = 0;
                              Bräde.möjligaDrag.Add(new Bräde.Fält(Bräde.gjordaDrag[gjordaDragIndex].X, Bräde.gjordaDrag[gjordaDragIndex].Y));
                              Bräde.gjordaDrag.RemoveAt(gjordaDragIndex);
                        }
                  }
            }

            private void SkötParti()
            {
                  bytSpelarePådDrag();

                  while (spelarePåDrag.EgenTyp != Spelare.Typ.Människa) {
                        Bräde.Fält nästaDrag = spelarePåDrag.användAI.BestämDrag();
                        Bräde.ställning[nästaDrag.X, nästaDrag.Y] = spelarePåDrag.Teckenvärde;
                        Bräde.gjordaDrag.Add(nästaDrag);
                        RitaSenasteMotorDrag(nästaDrag);
                        Bräde.uppdateraMöjligaDrag(nästaDrag);

                        if (Bräde.ÄrVinstdrag(nästaDrag, spelarePåDrag.Teckenvärde)) {
                              bytSpelarePådDrag();
                              // Om motorn vann över en människa
                              if (spelarePåDrag.EgenTyp == Spelare.Typ.Människa) {
                                    MessageBox.Show("Du blev ägd av en maskin.");
                              }

                              break;
                        }
                        // Om spelplanen är fylldd
                        else if (Bräde.gjordaDrag.Count == Bräde.antalFält) {
                              MessageBox.Show("WTF?");
                              break;
                        } else {
                              bytSpelarePådDrag();
                        }
                  }

                  Refresh();
            }

            private void HanteraMänskligtDrag() { }

            private void startKnapp_Click(object sender, EventArgs e)
            {
                  spelareEtt = new Spelare(spelareEttsArgument);
                  spelareTvå = new Spelare(spelareTvåsArgument);
                  Bräde.ställning = new int[Bräde.sida, Bräde.sida];
                  Bräde.gjordaDrag.Clear();
                  spelarePåDrag = spelareEtt;
                  Bräde.möjligaDrag.Clear();

                  // Lägger till mittfältet i möjliga drag så att motorn börjar där
                  Bräde.möjligaDrag.Add(new Bräde.Fält(Bräde.sida / 2, Bräde.sida / 2));

                  Refresh();

                  if (spelareEtt.EgenTyp != Spelare.Typ.Människa) {
                        Bräde.ställning[Bräde.sida / 2, Bräde.sida / 2] = spelareEtt.Teckenvärde;
                        Bräde.gjordaDrag.Add(new Bräde.Fält(Bräde.sida / 2, Bräde.sida / 2));
                        RitaSenasteMotorDrag(new Bräde.Fält(Bräde.sida / 2, Bräde.sida / 2));
                        Bräde.uppdateraMöjligaDrag(new Bräde.Fält(Bräde.sida / 2, Bräde.sida / 2));
                        SkötParti();
                  }
            }

            // Sköter det som ska hända när spelaren gjort ett drag
            private void Form1_MouseDown(object sender, MouseEventArgs e)
            {
                  if (spelarePåDrag != null && spelarePåDrag.EgenTyp == Spelare.Typ.Människa) {
                        // x och y-koordinaterna för fältet som klickades
                        int x = e.X / fältstorlek;
                        int y = (e.Y - menuStrip1.Size.Height) / fältstorlek; // Menyraden tar upp plats i yled

                        // Kollar att spelaren tryckte på ett fält innanför spelplanen och att fältet är tomt
                        if (x >= 0 && x < Bräde.sida && y >= 0 && y < Bräde.sida && Bräde.ställning[x, y] == 0) {
                              Bräde.ställning[x, y] = spelarePåDrag.Teckenvärde;
                              Refresh();

                              Bräde.Fält drag = new Bräde.Fält(x, y);
                              Bräde.uppdateraMöjligaDrag(drag);
                              Bräde.gjordaDrag.Add(drag);

                              if (Bräde.ÄrVinstdrag(drag, spelarePåDrag.Teckenvärde)) {
                                    // Om motståndaren är en människa
                                    if ((spelarePåDrag == spelareEtt ? spelareTvå : spelareEtt).EgenTyp == Spelare.Typ.Människa) {
                                          MessageBox.Show("Vinst.");
                                    } else {
                                          MessageBox.Show("Fuskare.");
                                    }
                              }
                              // Om spelplanen är fylld
                              else if (Bräde.gjordaDrag.Count == Bräde.antalFält) {
                                    MessageBox.Show("WTF?");
                              } else {
                                    SkötParti();
                              }
                        }
                  }
            }

            private void bytSpelarePådDrag()
            {
                  spelarePåDrag = spelarePåDrag == spelareEtt ? spelareTvå : spelareEtt;
            }
      }
}
