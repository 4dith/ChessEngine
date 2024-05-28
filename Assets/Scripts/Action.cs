using System;

public class Action
{
    public int initialPos;
    public int newPos;
    public int promotionTo;

    public static Random randomGen = new();
    
    public Action(int initialPos, int newPos, int promotionTo = Piece.empty) {
        this.initialPos = initialPos;
        this.newPos = newPos;
        this.promotionTo = promotionTo;
    }

    public int ActionScoreGuess(BoardState boardState) {
        // Improve this function

        int actionScoreGuess = 0;
        int[] positionsArray = boardState.positionsArray;
        
        int movePieceType = Piece.getType(positionsArray[initialPos]);
        int capturePieceType = Piece.getType(positionsArray[newPos]);

        if (capturePieceType != Piece.empty) {
            actionScoreGuess = 10 * Piece.getPoints(capturePieceType) - Piece.getPoints(movePieceType);
        }

        if (promotionTo != Piece.empty) actionScoreGuess += Math.Abs(Piece.getPoints(promotionTo));

        return actionScoreGuess * 10 + randomGen.Next(0, 9);
    }
}
