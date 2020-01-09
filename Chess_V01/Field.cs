using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_V01
{
    class Field
    {
        //List of possible moves to use later
        List<Point> points;

        //Field width
        private int _width;
        //Field Height
        private int _height;
        //Field Draw Graphics
        private Graphics _dev;

        //distance x
        private int disX = 57;
        //distance Y
        private int disY = 61;

        //Hint color
        Color hintColor = Color.FromArgb(150, 200, 0, 0);
        Color hintKColor = Color.FromArgb(100,192,0,0);
        //HintBrush
        SolidBrush hintBrush;

        // 2d chess table array
        private Piece[,] table;
        //Contsructor
        public Field(int width,int height)
        {
            //Assign width
            this._width = width;
            //Assign height
            this._height = height;
            //create array instance
            this.table = new Piece[8, 8];
            //function we need by a new game only to set the table correctly
            SetStartTable();
            //define hint Brush
            hintBrush = new SolidBrush(hintColor);
        }

        //GetSet Graph
        public Graphics Dev
        {
            get
            {
                return this._dev;
            }
            set
            {
                this._dev = value;
            }
        }
        //function we need by a new game only to set the table correctly
        private void SetStartTable()
        {
            //Assign pieces types & colors in the correct Position
            table[0, 0] = table[0, 7] = new Piece(C.Black, Type.Rook);
            table[0, 1] = table[0, 6] = new Piece(C.Black, Type.Knight);
            table[0, 2] = table[0, 5] = new Piece(C.Black, Type.Bishop);
            table[0, 4] = new Piece(C.Black, Type.King);
            table[0, 3] = new Piece(C.Black, Type.Queen);
            table[7, 0] = table[7, 7] = new Piece(C.White, Type.Rook);
            table[7, 1] = table[7, 6] = new Piece(C.White, Type.Knight);
            table[7, 2] = table[7, 5] = new Piece(C.White, Type.Bishop);
            table[7, 4] = new Piece(C.White, Type.King);
            table[7, 3] = new Piece(C.White, Type.Queen);
            //Assign Pawns
            for (int i = 0; i < 8; i++)
            {
                table[1, i] = new Piece(C.Black, Type.Pawn);
                table[6, i] = new Piece(C.White, Type.Pawn);
            }
            //Assign Empty
            for (int i = 2; i < 6; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    table[i, j] = new Piece(C.Empty, Type.Empty);
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////Test Pieces////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////
            
            //table[4, 4] = new Piece(C.Black, Type.King);
        }
        //Draw Figures
        public void DrawTable()
        {
            //start Pos
            Point curser = new Point(-5, 0);
            //rect to control the image
            Rectangle rect;
            //go through array
            foreach (Piece p in table)
            {
                //assign rect to Pos and give it a specific size
                rect = new Rectangle(curser.X, curser.Y, 60, 60);
                //if the figure isn't null
                if (p.PImage != null)
                    //Draw figure
                    Dev.DrawImage(p.PImage, rect);
                //move curser to the right
                curser.X += disX;
                //if curser reach limit
                if(curser.X >= 450)
                {
                    //reset x curser
                    curser.X = -5;
                    //move cursor down
                    curser.Y += disY;
                }
            }
        }
        //check if king is dead
        public int KingDeath()
        {
            //black king exist
            bool bKing = false;
            //White king exist
            bool wKing = false;
            //Go through pieces
            foreach (Piece p in table)
            {
                //if black king exist
                if (p.PieceType == Type.King && p.PieceColor == C.Black)
                    bKing = true;
                //if white king exist
                if (p.PieceType == Type.King && p.PieceColor == C.White)
                    wKing = true;
            }
            //black king is dead T-T
            if (!bKing)
                return 1;
            //white king is dead T-T
            if (!wKing)
                return 2;
            //both kings are alive *_*
            return 0;
        }
        //Clicked on possible block
        public bool checkPossible(int xMouse,int yMouse)
        {
            //if posible points list isn't empty
            if(points != null)
            {
                //iterate throw possible points
                foreach (Point p in points)
                {
                    //if possible has the same position as the click position
                    if (xMouse / disX == p.X / disX && yMouse / disY == p.Y / disY)
                        return true;
                }
            }
            return false;
        }
        //Move Piece Pos
        public void ChangePos(Point org,Point n)
        {
            //assign new Piece to new pos
            table[n.Y / disY, n.X / disX] = 
                new Piece(table[org.Y / disY, org.X / disX].PieceColor, 
                table[org.Y / disY, org.X / disX].PieceType);
            //Remove old Piece
            table[org.Y / disY, org.X / disX] = new Piece(C.Empty, Type.Empty);
        }
        //Check for hero Pawns and upgrade them to Queen
        public void HeroPawn()
        {
            //check first row
            for (int i = 0; i < 8; i++)
            {
                //if it contain a pawn ==> cann't be black Pawn
                if(table[0,i].PieceType == Type.Pawn)
                {
                    //change it to a queen
                    table[0, i] = new Piece(C.White, Type.Queen);
                    break;
                }
            }
            //check last row
            for (int i = 0; i < 8; i++)
            {
                //if it contain a pawn ==> cann't be white Pawn
                if (table[7,i].PieceType == Type.Pawn)
                {
                    //change it to a queen
                    table[7, i] = new Piece(C.Black, Type.Queen);
                    break;
                }
            }
        }
        //Function that run when clickedDown
        public bool OnClick1(int xMouse, int yMouse, bool white)
        {
            points = null;
            //test for piece type
            switch (table[yMouse / disY, xMouse / disX].PieceType)
            {
                //if pawn
                case Type.Pawn:
                    //Get possible Pawn Move and store it in the list
                    points = GetPawnMove(xMouse, yMouse, white);
                    //draw hints
                    DrawHints(points);
                    break;
                //if Rook
                case Type.Rook:
                    //Get possible Rook Move and store it in the list
                    points = GetRookMove(xMouse, yMouse, white);
                    //draw hints
                    DrawHints(points);
                    break;
                //if Knight
                case Type.Knight:
                    //Get possible Knight Move and store it in the list
                    points = GetKnightMove(xMouse, yMouse, white);
                    //draw hints
                    DrawHints(points);
                    break;
                //if Bishop
                case Type.Bishop:
                    //Get possible Bishop Move and store it in the list
                    points = GetBishopMove(xMouse, yMouse, white);
                    //draw hints
                    DrawHints(points);
                    break;
                //if Queen
                case Type.Queen:
                    //Get possible Queen Move and store it in the list
                    points = GetQueenMove(xMouse, yMouse, white);
                    //draw hints
                    DrawHints(points);
                    break;
                //if King
                case Type.King:
                    //Get possible King Move and store it in the list
                    points = GetKingMove(xMouse, yMouse, white);
                    //draw hints
                    DrawHints(points);
                    break;
            }
            //if posible points list is empty
            if (points == null)
                return false;
            return true;
        }
        //draw Hints
        private void DrawHints(List<Point> points)
        {
            foreach (Point p in points)
            {
                Rectangle rect = new Rectangle(p.X + 15, p.Y + 15, 20, 20);
                Dev.FillEllipse(hintBrush, rect);
            }
        }

        //Get posible King Move
        private List<Point> GetKingMove(int xMouse,int yMouse,bool white)
        {
            List<Point> points = new List<Point>();
            //if white player turn
            if (white)
            {
                //check if the clicked King is white !!
                if (table[yMouse / disY, xMouse / disX].PieceColor == C.White)
                {
                    //Check up
                    if(yMouse / disY - 1 >= 0 && table[yMouse / disY - 1,xMouse / disX].PieceColor != C.White)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX) * disX,(yMouse / disY - 1 ) * disY));
                    }
                    //Check up right
                    if (yMouse / disY - 1 >= 0 && xMouse / disX + 1 <= 7 && table[yMouse / disY - 1, xMouse / disX + 1].PieceColor != C.White)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX + 1) * disX, (yMouse / disY - 1) * disY));
                    }
                    //check right
                    if (xMouse / disX + 1 <= 7 && table[yMouse / disY, xMouse / disX + 1].PieceColor != C.White)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX + 1) * disX, (yMouse / disY) * disY));
                    }
                    //check right Down
                    if (yMouse / disY + 1 <= 7 && xMouse / disX + 1 <= 7 && table[yMouse / disY + 1, xMouse / disX + 1].PieceColor != C.White)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX + 1) * disX, (yMouse / disY + 1) * disY));
                    }
                    //check Down
                    if (yMouse / disY + 1 <= 7 && table[yMouse / disY + 1, xMouse / disX].PieceColor != C.White)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX) * disX, (yMouse / disY + 1) * disY));
                    }
                    //check Down Left
                    if (yMouse / disY + 1 <= 7 && xMouse / disX - 1 >= 0 && table[yMouse / disY + 1, xMouse / disX - 1].PieceColor != C.White)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX - 1) * disX, (yMouse / disY + 1) * disY));
                    }
                    //check Left
                    if (xMouse / disX - 1 >= 0 && table[yMouse / disY, xMouse / disX - 1].PieceColor != C.White)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX - 1) * disX, (yMouse / disY) * disY));
                    }
                    //check Left UP
                    if (yMouse / disY - 1 >= 0 && xMouse / disX - 1 >= 0 && table[yMouse / disY -1, xMouse / disX - 1].PieceColor != C.White)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX - 1) * disX, (yMouse / disY - 1) * disY));
                    }
                }
            }
            //if black player turn
            else
            {
                //check if the clicked King is black !!
                if (table[yMouse / disY, xMouse / disX].PieceColor == C.Black)
                {
                    //Check up
                    if (yMouse / disY - 1 >= 0 && table[yMouse / disY - 1, xMouse / disX].PieceColor != C.Black)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX) * disX, (yMouse / disY - 1) * disY));
                    }
                    //Check up right
                    if (yMouse / disY - 1 >= 0 && xMouse / disX + 1 <= 7 && table[yMouse / disY - 1, xMouse / disX + 1].PieceColor != C.Black)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX + 1) * disX, (yMouse / disY - 1) * disY));
                    }
                    //check right
                    if (xMouse / disX + 1 <= 7 && table[yMouse / disY, xMouse / disX + 1].PieceColor != C.Black)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX + 1) * disX, (yMouse / disY) * disY));
                    }
                    //check right Down
                    if (yMouse / disY + 1 <= 7 && xMouse / disX + 1 <= 7 && table[yMouse / disY + 1, xMouse / disX + 1].PieceColor != C.Black)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX + 1) * disX, (yMouse / disY + 1) * disY));
                    }
                    //check Down
                    if (yMouse / disY + 1 <= 7 && table[yMouse / disY + 1, xMouse / disX].PieceColor != C.Black)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX) * disX, (yMouse / disY + 1) * disY));
                    }
                    //check Down Left
                    if (yMouse / disY + 1 <= 7 && xMouse / disX - 1 >= 0 && table[yMouse / disY + 1, xMouse / disX - 1].PieceColor != C.Black)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX - 1) * disX, (yMouse / disY + 1) * disY));
                    }
                    //check Left
                    if (xMouse / disX - 1 >= 0 && table[yMouse / disY, xMouse / disX - 1].PieceColor != C.Black)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX - 1) * disX, (yMouse / disY) * disY));
                    }
                    //check Left UP
                    if (yMouse / disY - 1 >= 0 && xMouse / disX - 1 >= 0 && table[yMouse / disY - 1, xMouse / disX - 1].PieceColor != C.Black)
                    {
                        //add point to list
                        points.Add(new Point((xMouse / disX - 1) * disX, (yMouse / disY - 1) * disY));
                    }
                }
            }
            return points;
        }
        //Get posible Queen Move
        private List<Point> GetQueenMove(int xMouse,int yMouse,bool white)
        {
            List<Point> points = new List<Point>();
            //if white player turn
            if (white)
            {
                //check if the clicked Queen is white !!
                if (table[yMouse / disY, xMouse / disX].PieceColor == C.White)
                {
                    //upper line
                    for (int i = yMouse / disY - 1; i >= 0; i--)
                    {
                        //if next is empty
                        if (table[i, xMouse / disX].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                        }
                        else if (table[i, xMouse / disX].PieceColor == C.Black)
                        {
                            //if black add last point and break
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                            break;
                        }
                        //if white break without adding
                        else break;
                    }
                    //down line
                    for (int i = yMouse / disY + 1; i < 8; i++)
                    {
                        //if next is empty
                        if (table[i, xMouse / disX].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                        }
                        else if (table[i, xMouse / disX].PieceColor == C.Black)
                        {
                            //if black add last point and break
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                            break;
                        }
                        //if white break without adding
                        else break;
                    }
                    //Right line
                    for (int i = xMouse / disX + 1; i < 8; i++)
                    {
                        //if next is empty
                        if (table[yMouse / disY, i].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                        }
                        else if (table[yMouse / disY, i].PieceColor == C.Black)
                        {
                            //if black add last point and break
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                            break;
                        }
                        //if white break without adding
                        else break;
                    }
                    //Left line
                    for (int i = xMouse / disX - 1; i >= 0; i--)
                    {
                        //if next is empty
                        if (table[yMouse / disY, i].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                        }
                        else if (table[yMouse / disY, i].PieceColor == C.Black)
                        {
                            //if black add last point and break
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                            break;
                        }
                        //if white break without adding
                        else break;
                    }
                    //Help variables
                    int i1, j1;
                    //check up right
                    for (i1 = yMouse / disY - 1, j1 = xMouse / disX + 1; i1 >= 0 && j1 <= 7; i1--, j1++)
                    {
                        //if next is empty add it to list and continue
                        if (table[i1, j1].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                        }
                        //if next is black add it to list and break
                        else if (table[i1, j1].PieceColor == C.Black)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                            break;
                        }
                        // if next is white break
                        else break;
                    }
                    //check up left
                    for (i1 = yMouse / disY - 1, j1 = xMouse / disX - 1; i1 >= 0 && j1 >= 0; i1--, j1--)
                    {
                        //if next is empty add it to list and continue
                        if (table[i1, j1].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                        }
                        //if next is black add it to list and break
                        else if (table[i1, j1].PieceColor == C.Black)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                            break;
                        }
                        // if next is white break
                        else break;
                    }
                    //check down right
                    for (i1 = yMouse / disY + 1, j1 = xMouse / disX + 1; i1 <= 7 && j1 <= 7; i1++, j1++)
                    {
                        //if next is empty add it to list and continue
                        if (table[i1, j1].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                        }
                        //if next is black add it to list and break
                        else if (table[i1, j1].PieceColor == C.Black)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                            break;
                        }
                        // if next is white break
                        else break;
                    }
                    //check down Left
                    for (i1 = yMouse / disY + 1, j1 = xMouse / disX - 1; i1 <= 7 && j1 >= 0; i1++, j1--)
                    {
                        //if next is empty add it to list and continue
                        if (table[i1, j1].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                        }
                        //if next is black add it to list and break
                        else if (table[i1, j1].PieceColor == C.Black)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                            break;
                        }
                        // if next is white break
                        else break;
                    }
                }
            }
            //if black player Turn
            else
            {
                //check if the clicked Queen is black !!
                if (table[yMouse / disY, xMouse / disX].PieceColor == C.Black)
                {
                    //upper line
                    for (int i = yMouse / disY - 1; i >= 0; i--)
                    {
                        //if next is empty
                        if (table[i, xMouse / disX].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                        }
                        else if (table[i, xMouse / disX].PieceColor == C.White)
                        {
                            //if white add last point and break
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                            break;
                        }
                        //if Black break without adding
                        else break;
                    }
                    //down line
                    for (int i = yMouse / disY + 1; i < 8; i++)
                    {
                        //if next is empty
                        if (table[i, xMouse / disX].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                        }
                        else if (table[i, xMouse / disX].PieceColor == C.White)
                        {
                            //if White add last point and break
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                            break;
                        }
                        //if black break without adding
                        else break;
                    }
                    //Right line
                    for (int i = xMouse / disX + 1; i < 8; i++)
                    {
                        //if next is empty
                        if (table[yMouse / disY, i].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                        }
                        else if (table[yMouse / disY, i].PieceColor == C.White)
                        {
                            //if White add last point and break
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                            break;
                        }
                        //if black break without adding
                        else break;
                    }
                    //Left line
                    for (int i = xMouse / disX - 1; i >= 0; i--)
                    {
                        //if next is empty
                        if (table[yMouse / disY, i].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                        }
                        else if (table[yMouse / disY, i].PieceColor == C.White)
                        {
                            //if White add last point and break
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                            break;
                        }
                        //if black break without adding
                        else break;
                    }
                    //Help variables
                    int i1, j1;
                    //check up right
                    for (i1 = yMouse / disY - 1, j1 = xMouse / disX + 1; i1 >= 0 && j1 <= 7; i1--, j1++)
                    {
                        //if next is empty add it to list and continue
                        if (table[i1, j1].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                        }
                        //if next is White add it to list and break
                        else if (table[i1, j1].PieceColor == C.White)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                            break;
                        }
                        // if next is black break
                        else break;
                    }
                    //check up left
                    for (i1 = yMouse / disY - 1, j1 = xMouse / disX - 1; i1 >= 0 && j1 >= 0; i1--, j1--)
                    {
                        //if next is empty add it to list and continue
                        if (table[i1, j1].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                        }
                        //if next is White add it to list and break
                        else if (table[i1, j1].PieceColor == C.White)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                            break;
                        }
                        // if next is black break
                        else break;
                    }
                    //check down right
                    for (i1 = yMouse / disY + 1, j1 = xMouse / disX + 1; i1 <= 7 && j1 <= 7; i1++, j1++)
                    {
                        //if next is empty add it to list and continue
                        if (table[i1, j1].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                        }
                        //if next is White add it to list and break
                        else if (table[i1, j1].PieceColor == C.White)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                            break;
                        }
                        // if next is black break
                        else break;
                    }
                    //check down Left
                    for (i1 = yMouse / disY + 1, j1 = xMouse / disX - 1; i1 <= 7 && j1 >= 0; i1++, j1--)
                    {
                        //if next is empty add it to list and continue
                        if (table[i1, j1].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                        }
                        //if next is White add it to list and break
                        else if (table[i1, j1].PieceColor == C.White)
                        {
                            points.Add(new Point(j1 * disX, i1 * disY));
                            break;
                        }
                        // if next is black break
                        else break;
                    }
                }
            }
            return points;
        }
        //Get posible Bishop Move
        private List<Point> GetBishopMove(int xMouse, int yMouse, bool white)
        {
            List<Point> points = new List<Point>();

            //if white player turn
            if (white)
            {
                //check if the clicked Bishop is white !!
                if (table[yMouse / disY, xMouse / disX].PieceColor == C.White)
                {
                    //Help variables
                    int i, j;
                    //check up right
                    for(i = yMouse / disY - 1,j = xMouse / disX + 1;i >= 0 && j <= 7;i--,j++)
                    {
                        //if next is empty add it to list and continue
                        if (table[i, j].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j * disX, i * disY));
                        }
                        //if next is black add it to list and break
                        else if (table[i, j].PieceColor == C.Black)
                        {
                            points.Add(new Point(j * disX, i * disY));
                            break;
                        }
                        // if next is white break
                        else break;
                    }
                    //check up left
                    for(i = yMouse / disY - 1,j = xMouse / disX - 1;i >= 0 && j >= 0; i--,j--)
                    {
                        //if next is empty add it to list and continue
                        if (table[i, j].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j * disX, i * disY));
                        }
                        //if next is black add it to list and break
                        else if (table[i, j].PieceColor == C.Black)
                        {
                            points.Add(new Point(j * disX, i * disY));
                            break;
                        }
                        // if next is white break
                        else break;
                    }
                    //check down right
                    for(i = yMouse / disY + 1,j = xMouse / disX + 1;i <= 7 && j <= 7; i++,j++)
                    {
                        //if next is empty add it to list and continue
                        if (table[i, j].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j * disX, i * disY));
                        }
                        //if next is black add it to list and break
                        else if (table[i, j].PieceColor == C.Black)
                        {
                            points.Add(new Point(j * disX, i * disY));
                            break;
                        }
                        // if next is white break
                        else break;
                    }
                    //check down Left
                    for(i = yMouse / disY + 1,j = xMouse / disX - 1;i <= 7 && j >= 0; i++,j--)
                    {
                        //if next is empty add it to list and continue
                        if (table[i, j].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j * disX, i * disY));
                        }
                        //if next is black add it to list and break
                        else if (table[i, j].PieceColor == C.Black)
                        {
                            points.Add(new Point(j * disX, i * disY));
                            break;
                        }
                        // if next is white break
                        else break;
                    }
                }
            }
            //if black player turn
            else
            {
                //check if the clicked Bishop is black !!
                if (table[yMouse / disY, xMouse / disX].PieceColor == C.Black)
                {
                    //Help variables
                    int i, j;
                    //check up right
                    for (i = yMouse / disY - 1, j = xMouse / disX + 1; i >= 0 && j <= 7; i--, j++)
                    {
                        //if next is empty add it to list and continue
                        if (table[i, j].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j * disX, i * disY));
                        }
                        //if next is White add it to list and break
                        else if (table[i, j].PieceColor == C.White)
                        {
                            points.Add(new Point(j * disX, i * disY));
                            break;
                        }
                        // if next is black break
                        else break;
                    }
                    //check up left
                    for (i = yMouse / disY - 1, j = xMouse / disX - 1; i >= 0 && j >= 0; i--, j--)
                    {
                        //if next is empty add it to list and continue
                        if (table[i, j].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j * disX, i * disY));
                        }
                        //if next is White add it to list and break
                        else if (table[i, j].PieceColor == C.White)
                        {
                            points.Add(new Point(j * disX, i * disY));
                            break;
                        }
                        // if next is black break
                        else break;
                    }
                    //check down right
                    for (i = yMouse / disY + 1, j = xMouse / disX + 1; i <= 7 && j <= 7; i++, j++)
                    {
                        //if next is empty add it to list and continue
                        if (table[i, j].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j * disX, i * disY));
                        }
                        //if next is White add it to list and break
                        else if (table[i, j].PieceColor == C.White)
                        {
                            points.Add(new Point(j * disX, i * disY));
                            break;
                        }
                        // if next is black break
                        else break;
                    }
                    //check down Left
                    for (i = yMouse / disY + 1, j = xMouse / disX - 1; i <= 7 && j >= 0; i++, j--)
                    {
                        //if next is empty add it to list and continue
                        if (table[i, j].PieceColor == C.Empty)
                        {
                            points.Add(new Point(j * disX, i * disY));
                        }
                        //if next is White add it to list and break
                        else if (table[i, j].PieceColor == C.White)
                        {
                            points.Add(new Point(j * disX, i * disY));
                            break;
                        }
                        // if next is black break
                        else break;
                    }
                }
            }
            return points;
        }
        //Get posible Knight Move
        private List<Point> GetKnightMove(int xMouse,int yMouse,bool white)
        {
            //Point List of posible moves to return later
            List<Point> points = new List<Point>();

            //if white player turn
            if (white)
            {
                //check if the clicked Knight is white !!
                if (table[yMouse / disY, xMouse / disX].PieceColor == C.White)
                {
                    //check up right
                    if(yMouse / disY - 2 >= 0 && xMouse / disX + 1 <= 7)
                    {
                        //if its not a white Piece we can move there
                        if(table[yMouse / disY - 2,xMouse / disX + 1].PieceColor != C.White)
                        {
                            points.Add(new Point((xMouse / disX + 1) * disX ,( yMouse / disY - 2)*disY));
                        }
                    }
                    //Check up Left
                    if(yMouse / disY - 2 >= 0 && xMouse / disX -1 >= 0)
                    {
                        if (table[yMouse / disY - 2, xMouse / disX - 1].PieceColor != C.White)
                        {
                            points.Add(new Point((xMouse / disX - 1) * disX, (yMouse / disY - 2) * disY));
                        }
                    }
                    //Check Down right
                    if (yMouse / disY + 2 <= 7 && xMouse / disX + 1 <= 7)
                    {
                        if (table[yMouse / disY + 2, xMouse / disX + 1].PieceColor != C.White)
                        {
                            points.Add(new Point((xMouse / disX + 1) * disX, (yMouse / disY + 2) * disY));
                        }
                    }
                    //Check Down Left
                    if (yMouse / disY + 2 <= 7 && xMouse / disX - 1 >= 0)
                    {
                        if (table[yMouse / disY + 2, xMouse / disX - 1].PieceColor != C.White)
                        {
                            points.Add(new Point((xMouse / disX - 1) * disX, (yMouse / disY + 2) * disY));
                        }
                    }
                    //Check Right up
                    if (yMouse / disY - 1 >= 0 && xMouse / disX + 2 <= 7)
                    {
                        if (table[yMouse / disY - 1, xMouse / disX + 2].PieceColor != C.White)
                        {
                            points.Add(new Point((xMouse / disX + 2) * disX, (yMouse / disY - 1) * disY));
                        }
                    }
                    //Check Right Down
                    if (yMouse / disY + 1 <= 7 && xMouse / disX + 2 <= 7)
                    {
                        if (table[yMouse / disY + 1, xMouse / disX + 2].PieceColor != C.White)
                        {
                            points.Add(new Point((xMouse / disX + 2) * disX, (yMouse / disY + 1) * disY));
                        }
                    }
                    //Check Left up
                    if (yMouse / disY - 1 >= 0 && xMouse / disX - 2 >= 0)
                    {
                        if (table[yMouse / disY - 1, xMouse / disX - 2].PieceColor != C.White)
                        {
                            points.Add(new Point((xMouse / disX - 2) * disX, (yMouse / disY - 1) * disY));
                        }
                    }
                    //Check Left Down
                    if (yMouse / disY + 1 <= 7 && xMouse / disX - 2 >= 0)
                    {
                        if (table[yMouse / disY + 1, xMouse / disX - 2].PieceColor != C.White)
                        {
                            points.Add(new Point((xMouse / disX - 2) * disX, (yMouse / disY + 1) * disY));
                        }
                    }
                }
            }
            //if black player turn
            else
            {
                //check if the clicked Knight is black !!
                if (table[yMouse / disY, xMouse / disX].PieceColor == C.Black)
                {
                    //check up right
                    if (yMouse / disY - 2 >= 0 && xMouse / disX + 1 <= 7)
                    {
                        //if its not a Black Piece we can move there
                        if (table[yMouse / disY - 2, xMouse / disX + 1].PieceColor != C.Black)
                        {
                            points.Add(new Point((xMouse / disX + 1) * disX, (yMouse / disY - 2) * disY));
                        }
                    }
                    //Check up Left
                    if (yMouse / disY - 2 >= 0 && xMouse / disX - 1 >= 0)
                    {
                        if (table[yMouse / disY - 2, xMouse / disX - 1].PieceColor != C.Black)
                        {
                            points.Add(new Point((xMouse / disX - 1) * disX, (yMouse / disY - 2) * disY));
                        }
                    }
                    //Check Down right
                    if (yMouse / disY + 2 <= 7 && xMouse / disX + 1 <= 7)
                    {
                        if (table[yMouse / disY + 2, xMouse / disX + 1].PieceColor != C.Black)
                        {
                            points.Add(new Point((xMouse / disX + 1) * disX, (yMouse / disY + 2) * disY));
                        }
                    }
                    //Check Down Left
                    if (yMouse / disY + 2 <= 7 && xMouse / disX - 1 >= 0)
                    {
                        if (table[yMouse / disY + 2, xMouse / disX - 1].PieceColor != C.Black)
                        {
                            points.Add(new Point((xMouse / disX - 1) * disX, (yMouse / disY + 2) * disY));
                        }
                    }
                    //Check Right up
                    if (yMouse / disY - 1 >= 0 && xMouse / disX + 2 <= 7)
                    {
                        if (table[yMouse / disY - 1, xMouse / disX + 2].PieceColor != C.Black)
                        {
                            points.Add(new Point((xMouse / disX + 2) * disX, (yMouse / disY - 1) * disY));
                        }
                    }
                    //Check Right Down
                    if (yMouse / disY + 1 <= 7 && xMouse / disX + 2 <= 7)
                    {
                        if (table[yMouse / disY + 1, xMouse / disX + 2].PieceColor != C.Black)
                        {
                            points.Add(new Point((xMouse / disX + 2) * disX, (yMouse / disY + 1) * disY));
                        }
                    }
                    //Check Left up
                    if (yMouse / disY - 1 >= 0 && xMouse / disX - 2 >= 0)
                    {
                        if (table[yMouse / disY - 1, xMouse / disX - 2].PieceColor != C.Black)
                        {
                            points.Add(new Point((xMouse / disX - 2) * disX, (yMouse / disY - 1) * disY));
                        }
                    }
                    //Check Left Down
                    if (yMouse / disY + 1 <= 7 && xMouse / disX - 2 >= 0)
                    {
                        if (table[yMouse / disY + 1, xMouse / disX - 2].PieceColor != C.Black)
                        {
                            points.Add(new Point((xMouse / disX - 2) * disX, (yMouse / disY + 1) * disY));
                        }
                    }
                }
            }
            return points;
        }
        //Get posible Rook Move
        private List<Point> GetRookMove(int xMouse, int yMouse, bool white)
        {
            //Point List of posible moves to return later
            List<Point> points = new List<Point>();
            //if white player turn
            if (white)
            {
                //check if the clicked Rook is white !!
                if (table[yMouse / disY, xMouse / disX].PieceColor == C.White)
                {
                    //Rook can move in straight lines
                    //upper line
                    for (int i = yMouse / disY - 1; i >= 0; i--)
                    {
                        //if next is empty
                        if (table[i, xMouse / disX].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                        }
                        else if (table[i, xMouse / disX].PieceColor == C.Black)
                        {
                            //if black add last point and break
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                            break;
                        }
                        //if white break without adding
                        else break;
                    }
                    //down line
                    for (int i = yMouse / disY + 1; i < 8; i++)
                    {
                        //if next is empty
                        if (table[i, xMouse / disX].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                        }
                        else if (table[i, xMouse / disX].PieceColor == C.Black)
                        {
                            //if black add last point and break
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                            break;
                        }
                        //if white break without adding
                        else break;
                    }
                    //Right line
                    for (int i = xMouse / disX + 1; i < 8; i++)
                    {
                        //if next is empty
                        if (table[yMouse / disY,i].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                        }
                        else if (table[yMouse / disY, i].PieceColor == C.Black)
                        {
                            //if black add last point and break
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                            break;
                        }
                        //if white break without adding
                        else break;
                    }
                    //Left line
                    for (int i = xMouse / disX - 1; i >= 0; i--)
                    {
                        //if next is empty
                        if (table[yMouse / disY, i].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                        }
                        else if (table[yMouse / disY, i].PieceColor == C.Black)
                        {
                            //if black add last point and break
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                            break;
                        }
                        //if white break without adding
                        else break;
                    }
                }
            }
            //if black player turn
            else
            {
                //check if the clicked Rook is Black !!
                if (table[yMouse / disY, xMouse / disX].PieceColor == C.Black)
                {
                    //upper line
                    for (int i = yMouse / disY - 1; i >= 0; i--)
                    {
                        //if next is empty
                        if (table[i, xMouse / disX].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                        }
                        else if (table[i, xMouse / disX].PieceColor == C.White)
                        {
                            //if white add last point and break
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                            break;
                        }
                        //if black break without adding
                        else break;
                    }
                    //down line
                    for (int i = yMouse / disY + 1; i < 8; i++)
                    {
                        //if next is empty
                        if (table[i, xMouse / disX].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                        }
                        else if (table[i, xMouse / disX].PieceColor == C.White)
                        {
                            //if White add last point and break
                            points.Add(new Point((xMouse / disX) * disX, i * disY));
                            break;
                        }
                        //if black break without adding
                        else break;
                    }
                    //Right line
                    for (int i = xMouse / disX + 1; i < 8; i++)
                    {
                        //if next is empty
                        if (table[yMouse / disY, i].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                        }
                        else if (table[yMouse / disY, i].PieceColor == C.White)
                        {
                            //if White add last point and break
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                            break;
                        }
                        //if black break without adding
                        else break;
                    }
                    //Left line
                    for (int i = xMouse / disX - 1; i >= 0; i--)
                    {
                        //if next is empty
                        if (table[yMouse / disY, i].PieceType == Type.Empty)
                        {
                            //add position to list
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                        }
                        else if (table[yMouse / disY, i].PieceColor == C.White)
                        {
                            //if White add last point and break
                            points.Add(new Point(i * disX, (yMouse / disY) * disY));
                            break;
                        }
                        //if black break without adding
                        else break;
                    }
             
                }
            }
            return points;
        }
        //Get posible Pawn Move
        private List<Point> GetPawnMove(int xMouse, int yMouse,bool white)
        {
            //Point List of posible moves to return later
            List<Point> points = new List<Point>();
            //if white player turn
            if (white)
            {
                //check if the clicked Pawn is white !!
                if (table[yMouse / disY, xMouse / disX].PieceColor == C.White)
                {
                    //first move row = 6 (2 in chess) of pawn can be one or tow moves
                    if (yMouse / disY == 6)
                    { 
                        //check if no other figure on next place
                        if (table[yMouse / disY - 1, xMouse / disX].PieceType == Type.Empty)
                        {
                            points.Add(new Point((xMouse / disX) * disX, (yMouse / disY - 1) * disY));
                            //check next place
                            if (table[yMouse / disY - 2, xMouse / disX].PieceType == Type.Empty)
                            {
                                points.Add(new Point((xMouse / disX) * disX, (yMouse / disY - 2) * disY));
                            }
                        }
                    }
                    //Pawn move  only one time but should not be at row 0 (8 in chess)
                    else if (yMouse / disY - 1 > 0)
                    {
                        //check if no other figure on next place
                        if (table[yMouse / disY - 1, xMouse / disX].PieceType == Type.Empty)
                        {
                            points.Add(new Point((xMouse / disX) * disX, (yMouse / disY - 1) * disY));
                        }
                    }
                    //Check for Food -_- Left upper
                    if (yMouse / disY - 1 >= 0 && xMouse / disX - 1 >= 0 && table[yMouse / disY -1,xMouse / disX - 1].PieceColor == C.Black)
                    {
                        points.Add(new Point((xMouse / disX - 1) * disX, (yMouse / disY - 1) * disY));
                    }
                    //Check Food -_- Right upper
                    if (yMouse / disY - 1 >= 0 && xMouse / disX + 1 <= 7 && table[yMouse / disY - 1, xMouse / disX + 1].PieceColor == C.Black)
                    {
                        points.Add(new Point((xMouse / disX + 1) * disX, (yMouse / disY - 1) * disY));
                    }
                }
            }
            else
            {
                //check if the clicked Pawn is Black !!
                if (table[yMouse / disY, xMouse / disX].PieceColor == C.Black)
                {
                    //first move row = 1 (2 in chess) of pawn can be one or tow moves
                    if (yMouse / disY == 1)
                    {
                        //check if no other figure on next place
                        if (table[yMouse / disY + 1, xMouse / disX].PieceType == Type.Empty)
                        {
                            points.Add(new Point((xMouse / disX) * disX, (yMouse / disY + 1) * disY));
                            //check next place
                            if (table[yMouse / disY + 2, xMouse / disX].PieceType == Type.Empty)
                            {
                                points.Add(new Point((xMouse / disX) * disX, (yMouse / disY + 2) * disY));
                            }
                        }
                    }
                    //Pawn move  only one time but should not be at row 7 (8 in chess)
                    else if (yMouse / disY + 1 < 7)
                    {
                        //check if no other figure on next place
                        if (table[yMouse / disY + 1, xMouse / disX].PieceType == Type.Empty)
                        {
                            points.Add(new Point((xMouse / disX) * disX, (yMouse / disY + 1) * disY));
                        }
                    }
                    //Check for Food -_- Left down
                    if (yMouse / disY + 1 <= 7 && xMouse / disX - 1 >= 0 && table[yMouse / disY + 1, xMouse / disX - 1].PieceColor == C.White)
                    {
                        points.Add(new Point((xMouse / disX - 1) * disX, (yMouse / disY + 1) * disY));
                    }
                    //Check Food -_- Right down
                    if (yMouse / disY + 1 <= 7 && xMouse / disX + 1 <= 7 && table[yMouse / disY + 1, xMouse / disX + 1].PieceColor == C.White)
                    {
                        points.Add(new Point((xMouse / disX + 1) * disX, (yMouse / disY + 1) * disY));
                    }
                }
            }
            return points;
        }
    }
}