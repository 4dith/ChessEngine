using System;
using System.Collections.Generic;

public static class Piece
{       
    public const int pawn = 1;
    public const int knight = 2;
    public const int bishop = 3;
    public const int rook = 4;
    public const int queen = 5;
    public const int king = 6;
    
    public const int white = 1;
    public const int black = -1;
    public const int empty = 0;

    static Dictionary<int, int> pointsDict = new Dictionary<int, int>() {
        {1, 1}, {2, 3}, {3, 3}, {4, 5}, {5, 9}, {6, 1000}
    };

    public static int getType(int pieceNumber) {
        return Math.Abs(pieceNumber);
    }
    
    public static int getColor(int pieceNumber) {
        if (pieceNumber < 0) return black;
        if (pieceNumber > 0) return white;
        return empty;
    }

    public static int getPoints(int pieceNumber) {
        return pointsDict[Math.Abs(pieceNumber)];
    }
}
