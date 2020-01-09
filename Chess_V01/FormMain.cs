using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess_V01
{
    public partial class FormMain : Form
    {
        //Define field
        Field field;

        Point mousePos;
        Point mousePos2;
        //turn
        bool white = true;
        int click = 0;
        public FormMain()
        {
            //Default Initialize
            InitializeComponent();
            //Field instance & Assign width & Height of the pictureBox
            field = new Field(pictureBoxDraw.Width, pictureBoxDraw.Height);
        }

        //OnDraw
        private void pictureBoxDraw_Paint(object sender, PaintEventArgs e)
        {
            //def graph
            Graphics dev = e.Graphics;
            //set smoothing mode
            dev.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //assign field graph
            field.Dev = dev;
            //draw figures
            field.DrawTable();
            //if it's first click
            if (click == 1)
            {
                //check if clicked Piece has no valid moves
                if(!field.OnClick1(mousePos.X, mousePos.Y, white))
                {
                    //reset clickCounter to 1
                    click = 1;
                }
            }
            //if second click
            if(click == 2)
            {
                //check if the second click is on a valid possible block
                if (field.checkPossible(mousePos2.X, mousePos2.Y))
                {
                    //Move Piece
                    field.ChangePos(mousePos,mousePos2);
                    //reset click
                    click = 1;
                    white = !white;
                    //recall draw function
                    pictureBoxDraw.Invalidate();
                    
                }
                else
                {
                    //Reset click to 1
                    click = 1;
                    //recall draw function
                    pictureBoxDraw.Invalidate();
                }
            }
            field.HeroPawn();
            if(field.KingDeath() == 1)
            {
                MessageBox.Show("White king won the game!!");
                this.Close();
            }
            else if (field.KingDeath() == 2)
            {
                MessageBox.Show("Black King won the game!!");
                this.Close();
            }
        }
        //Onclick event
        private void pictureBoxDraw_MouseClick(object sender, MouseEventArgs e)
        {
            mousePos2 = new Point(e.Location.X, e.Location.Y);

            //if click is less than 3 
            if (click + 1 < 3)
            {
                //increase clicks
                click++;
            }
            else
            {
                //reset click to 1
                click = 1;
            }
            //if one click
            if (click == 1)
                //save the mouse position
                mousePos = new Point(e.Location.X, e.Location.Y);
            //if second click
            else if (click == 2)
            {
                //check if the second click is on a not valid possible block
                if (!field.checkPossible(mousePos2.X, mousePos2.Y))
                {
                    //save mouse position
                    mousePos = new Point(e.Location.X, e.Location.Y);
                    //reset click to 1
                    click = 1;
                }
                //save second mouse position
            }
            //refresh the draw function
            pictureBoxDraw.Invalidate();
        }
    }
}