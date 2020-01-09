using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_V01
{
    //Piece Color 
    public enum C
    {
        White,
        Black,
        Empty
    }
    //Piece Type
    public enum Type
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King,
        Empty
    }
    class Piece
    {
        //Color , type , image instance
        private C _pieceColor;
        private Type _pieceType;
        private Image _pImage;

        //Structor which run every time a new Piece created
        public Piece(C pColor,Type type)
        {
            //Assign piece Color , type & Image
            this.PieceColor = pColor;
            this.PieceType = type;
            this.PImage = GetImage();  //Function
        }

        //getset color
        public C PieceColor
        {
            get
            {
                return this._pieceColor;
            }
            set
            {
                this._pieceColor = value;
            }
        }
        //getsetType
        public Type PieceType
        {
            get
            {
                return this._pieceType;
            }
            set
            {
                this._pieceType = value;
            }
        }
        //getset Image
        public Image PImage
        {
            get
            {
                return this._pImage;
            }
            set
            {
                this._pImage = value;
            }
        }
        
        private Image GetImage()
        {
            //test if the color is white
            if(_pieceColor == C.White)
            {
                //Test the type and return the apropriate Image to the piceImage property
                switch (_pieceType)
                {
                    case Type.Pawn:
                        return Image.FromFile("Images/WPawn.png");
                    case Type.Rook:
                        return Image.FromFile("Images/WRook.png");
                    case Type.Knight:
                        return Image.FromFile("Images/WKnight.png");
                    case Type.Bishop:
                        return Image.FromFile("Images/WBishop.png");
                    case Type.Queen:
                        return Image.FromFile("Images/WQueen.png");
                    case Type.King:
                        return Image.FromFile("Images/WKing.png");
                    default:
                        throw new Exception("The piece Doesn't exist! w");
                }
            }
            //test if the color is Black
            else if (_pieceColor == C.Black)
            {
                //Test the type and return the apropriate Image to the piceImage property
                switch (_pieceType)
                {
                    case Type.Pawn:
                        return Image.FromFile("Images/BPawn.png");
                    case Type.Rook:
                        return Image.FromFile("Images/BRook.png");
                    case Type.Knight:
                        return Image.FromFile("Images/BKnight.png");
                    case Type.Bishop:
                        return Image.FromFile("Images/BBishop.png");
                    case Type.Queen:
                        return Image.FromFile("Images/BQueen.png");
                    case Type.King:
                        return Image.FromFile("Images/BKing.png");
                    default:
                        throw new Exception("The piece Doesn't exist! b");
                }
            }
            //if field is empty
            else
            {
                return null;
            }
        }
    }
}
