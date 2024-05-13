using System.Collections.Generic;

public class Piece
{    
    public const int pawn = 1;
    public const int knight = 2;
    public const int bishop = 3;
    public const int rook = 4;
    public const int queen = 5;
    public const int king = 6;
    
    public const bool white = true;
    public const bool black = false;

    Dictionary<int, int> pointsDict = new Dictionary<int, int>() {
        {1, 1}, {2, 3}, {3, 3}, {4, 5}, {5, 9}, {6, 1000}
    };

    public readonly int type;
    public readonly bool color;
    
    public Piece(int typeNumber, bool pieceColor) {
        type = typeNumber;
        color = pieceColor;
    }

    public int getPoints() {
        return pointsDict[type];
    }
}
