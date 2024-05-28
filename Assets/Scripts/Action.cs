using System;
using System.Collections.Generic;

public class Action
{
    public Tuple<int, int> initialPos;
    public Tuple<int, int> newPos;
    public int promotionTo;
    
    public Action(Tuple<int, int> initialPos, Tuple<int, int> newPos, int promotionTo = Piece.empty) {
        this.initialPos = initialPos;
        this.newPos = newPos;
        this.promotionTo = promotionTo;
    }

    public int ActionScoreGuess(BoardState boardState, int aggressiveness=10) {
        int actionScoreGuess = 0;
        int[,] positionsArray = boardState.positionsArray;
        int initialRank = initialPos.Item1, initialFile = initialPos.Item2;
        int newRank = newPos.Item1, newFile = newPos.Item2;
        
        int movePieceType = Piece.getType(positionsArray[initialRank, initialFile]);
        int capturePieceType = Piece.getType(positionsArray[newRank, newFile]);

        if (capturePieceType != Piece.empty) {
            actionScoreGuess = aggressiveness * Piece.getPoints(capturePieceType) - Piece.getPoints(movePieceType);
        }

        if (promotionTo != Piece.empty) actionScoreGuess += Math.Abs(Piece.getPoints(promotionTo));

        return actionScoreGuess;
    }
}
