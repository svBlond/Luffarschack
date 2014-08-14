using System;

namespace Luffarschacksprojekt
{
    partial class Form1
    {
        private void människaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spelareEttsArgument.typ = Spelare.Typ.Människa;
        }

        private void människaToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            spelareTvåsArgument.typ = Spelare.Typ.Människa;
        }

        private void oToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spelareEttsArgument.spelarTecken = Spelare.Tecken.Cirkel;
            spelareTvåsArgument.spelarTecken = Spelare.Tecken.Kryss;
        }

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spelareTvåsArgument.spelarTecken = Spelare.Tecken.Cirkel;
            spelareEttsArgument.spelarTecken = Spelare.Tecken.Kryss;
        }

        private void monteCarloToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            spelareEttsArgument.typ = Spelare.Typ.MonteCarlo;
        }

        private void originalgangsterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            spelareEttsArgument.typ = Spelare.Typ.Originalgangster;
        }

        private void monteCarloToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            spelareTvåsArgument.typ = Spelare.Typ.MonteCarlo;
        }

        private void originalgangsterToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            spelareTvåsArgument.typ = Spelare.Typ.Originalgangster;
        }
    }
}
