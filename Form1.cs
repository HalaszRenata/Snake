using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Joc_Snake_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        PictureBox tabla = new PictureBox(); // PictureBox pentru tabla de joc
        PictureBox Snake = new PictureBox(); // PictureBox pentru șarpe
        PictureBox Mar = new PictureBox(); // PictureBox pentru măr
        PictureBox[] Coada = new PictureBox[1001]; // Vector de PictureBox-uri pentru coada șarpelui

        int dx = 1; // Direcția inițială pe axa X (dreapta)
        int dy = 0; // Direcția inițială pe axa Y (stând)
        int cl = 0; // Lungimea inițială a șarpelui

        private void btnStart_Click(object sender, EventArgs e)
        {
            timer1.Start(); // Începe jocul, se porneste timerul
        }

        private void btnReset_Click(object sender, EventArgs e)
        {   
            // Resetează poziția șarpelui
            Snake.Location = new Point(100, 100);

            // Resetează poziția mărului
            Random r = new Random();
            int x1 = r.Next(24) * 20; // Generează o poziție aleatoare pe axa X
            int y1 = r.Next(24) * 20; // Generează o poziție aleatoare pe axa Y
            Mar.Location = new Point(x1, y1);

            // Resetează lungimea șarpelui
            cl = 0;

            // Elimină toți copiii controlului tabla 
            tabla.Controls.Clear();

            // Inițializează tabla, șarpele și mărul
            tabla.Width = tabla.Height = 500; // Stabilește dimensiunea tablei de joc
            tabla.BackColor = Color.Black; // Setează culoarea fundalului tablei
            tabla.Location = new Point(30, 30); // Plasează tabla în cadrul formularului
            this.Location = new Point(50, 50); // Plasează formularul pe ecran
            this.Width = this.Height = 600; // Stabilește dimensiunile formularului
            this.Controls.Add(tabla); // Adaugă tabla la lista de controale a formularului

            Snake.Width = Snake.Height = 18; // Stabilește dimensiunile șarpelui
            Snake.Location = new Point(100, 100); // Plasează inițial șarpele pe tabla
            Snake.BackColor = Color.White; // Setează culoarea șarpelui
            tabla.Controls.Add(Snake); // Adaugă șarpele la tabla de joc

            Mar.Width = Mar.Height = 18; // Stabilește dimensiunile mărului
            Mar.Location = new Point(300, 400); // Plasează inițial mărul pe tabla
            Mar.BackColor = Color.Red; // Setează culoarea mărului
            tabla.Controls.Add(Mar); // Adaugă mărul la tabla de joc

            timer1.Stop(); // Oprește timerul
        }

        private void Form1_Load(object sender, EventArgs e)
        { 
            // Inițializează tabla, șarpele și mărul la încărcarea formularului
            tabla.Width = tabla.Height = 500; // Setează dimensiunile și culoarea tablei de joc
            tabla.BackColor = Color.Black;
            tabla.Location = new Point(30, 30);// Plasează tabla în cadrul formularului
            this.Location = new Point(50, 50);// Plasează formularul pe ecran
            this.Width = this.Height = 600;// Stabilește dimensiunile formularului
            this.Controls.Add(tabla); // Adaugă tabla la lista de controale a formularului

            Snake.Width = Snake.Height = 18; // Setează dimensiunile și culoarea șarpelui
            Snake.BackColor = Color.White;
            Snake.Location = new Point(100, 100);// Plasează inițial șarpele pe tabla de joc
            tabla.Controls.Add(Snake);// Adaugă șarpele la tabla de joc

            Mar.Width = Mar.Height = 18;// Setează dimensiunile și culoarea mărului
            Mar.BackColor = Color.Red;
            Mar.Location = new Point(300, 400);// Plasează inițial mărul pe tabla de joc
            tabla.Controls.Add(Mar);// Adaugă mărul la tabla de joc

            // Atașează evenimente pentru butoanele de Start și Reset
            btnStart.Click += new EventHandler(btnStart_Click);
            btnReset.Click += new EventHandler(btnReset_Click);

            // Activează funcționalitatea de captare a evenimentelor de apăsare a tastelor
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown); 
        }

        private void timer1_Tick(object sender, EventArgs e)
        { 
            // Actualizează pozițiile segmentelor coadei șarpelui
            for (int i = cl; i >= 2; --i)
            {
                Coada[i].Location = Coada[i - 1].Location;
            }
            // Actualizează poziția primului segment al coadei la poziția șarpelui
            if (cl > 0)
            {
                Coada[1].Location = Snake.Location;
            }

            // Calculează noile coordonate ale capului șarpelui
            int newX = Snake.Location.X + dx * 20;
            int newY = Snake.Location.Y + dy * 20;
            Snake.Location = new Point(newX, newY);

            // Verifică dacă s-a câștigat jocul (șarpele a ajuns la lungimea maximă)
            if (cl == 1001) // Dacă lungimea șarpelui este de 1001 segmente
            {
                timer1.Stop(); // Oprește timerul
                MessageBox.Show("Ai câștigat!"); // Afișează un mesaj de câștig
            }

            // Verifică coliziunile cu coada șarpelui
            for (int i = 1; i <= cl; ++i)
            {
                if (Snake.Location == Coada[i].Location)
                {
                    timer1.Stop(); // Oprește timerul
                    MessageBox.Show("Game Over!"); // Afișează un mesaj de sfârșit al jocului
                }
            }

            // Verifică coliziunea cu mărul
            if (Snake.Bounds.IntersectsWith(Mar.Bounds))
            {
                Random r = new Random();
                int x1 = r.Next(24) * 20;
                int y1 = r.Next(24) * 20;
                Mar.Location = new Point(x1, y1); // Repune mărul într-o poziție aleatoare pe tabla de joc
                cl++; // Crește lungimea șarpelui
                Coada[cl] = new PictureBox();
                Coada[cl].BackColor = Color.White;
                Coada[cl].Size = new Size(18, 18);
                tabla.Controls.Add(Coada[cl]); // Adaugă un nou segment al coadei șarpelui
            }

            // Verifică coliziunile cu marginile tablei și realizează teletransportarea șarpelui
            if (newX >= tabla.Width)
                newX = 0;
            if (newX < 0)
                newX = tabla.Width - 20;
            if (newY >= tabla.Height)
                newY = 0;
            if (newY < 0)
                newY = tabla.Height - 20;
            Snake.Location = new Point(newX, newY); // Actualizează poziția șarpelului
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && dx != 1)
            {
                dx = -1; dy = 0; // deplasare la stânga
            }
            if (e.KeyCode == Keys.Right && dx != -1)
            {
                dx = 1; dy = 0; // deplasare la dreapta
            }
            if (e.KeyCode == Keys.Up && dy != 1)
            {
                dx = 0; dy = -1; //  deplasare în sus
            }
            if (e.KeyCode == Keys.Down && dy != -1)
            {
                dx = 0; dy = 1; // deplasare în jos
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
