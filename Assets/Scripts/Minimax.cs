using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public static class Minimax
{
    public const int maxUtility = 50;
    
    static int ColorAtPos(int pos, int[] positionsArray)
    {
        return Piece.getColor(positionsArray[pos]);
    }

    static void AddDiagonalPositions(int pos, int rank, int file, int pieceColor, int[] positionsArray, List<int> positionList)
    {       
        // Quadrant 1
        for (int i = 1; i <= Math.Min(7 - file, 7 - rank); i++)
        {
            int colorAtPos = ColorAtPos(pos + 9 * i, positionsArray);
            if (colorAtPos == pieceColor) break;
            positionList.Add(pos + 9 * i);
            if (colorAtPos == pieceColor * -1) break;
        }

        // Quadrant 3
        for (int i = 1; i <= Math.Min(file, rank); i++)
        {
            int colorAtPos = ColorAtPos(pos - 9 * i, positionsArray);
            if (colorAtPos == pieceColor) break;
            positionList.Add(pos - 9 * i);
            if (colorAtPos == pieceColor * -1) break;
        }

        // Quadrant 2
        for (int i = 1; i <= Math.Min(file, 7 - rank); i++)
        {
            int colorAtPos = ColorAtPos(pos + 7 * i, positionsArray);
            if (colorAtPos == pieceColor) break;
            positionList.Add(pos + 7 * i);
            if (colorAtPos == pieceColor * -1) break;
        }

        // Quadrant 4
        for (int i = 1; i <= Math.Min(7 - file, rank); i++)
        {
            int colorAtPos = ColorAtPos(pos - 7 * i, positionsArray);
            if (colorAtPos == pieceColor) break;
            positionList.Add(pos - 7 * i);
            if (colorAtPos == pieceColor * -1) break;
        }
    }

    static void AddStraightPositions(int pos, int rank, int file, int pieceColor, int[] positionsArray, List<int> positionList)
    {        
        // + x axis
        for (int i = 1; i <= 7 - file; i++)
        {
            int colorAtPos = ColorAtPos(pos + i, positionsArray);
            if (colorAtPos == pieceColor) break;
            positionList.Add(pos + i);
            if (colorAtPos == pieceColor * -1) break;
        }

        // - x axis
        for (int i = 1; i <= file; i++)
        {
            int colorAtPos = ColorAtPos(pos - i, positionsArray);
            if (colorAtPos == pieceColor) break;
            positionList.Add(pos - i);
            if (colorAtPos == pieceColor * -1) break;
        }

        // + y axis
        for (int i = 1; i <= 7 - rank; i++)
        {
            int colorAtPos = ColorAtPos(pos + 8 * i, positionsArray);
            if (colorAtPos == pieceColor) break;
            positionList.Add(pos + 8 * i);
            if (colorAtPos == pieceColor * -1) break;
        }

        // - y axis
        for (int i = 1; i <= rank; i++)
        {
            int colorAtPos = ColorAtPos(pos - 8 * i, positionsArray);
            if (colorAtPos == pieceColor) break;
            positionList.Add(pos - 8 * i);
            if (colorAtPos == pieceColor * -1) break;
        }
    }

    public static List<int> GetNewPositionsUnfiltered(int pos, BoardState boardState)
    {
        // Assumes that square is not empty
        
        List<int> newPositions = new();

        int[] positionsArray = boardState.positionsArray;

        int pieceNumber = positionsArray[pos];
        int rank = pos / 8, file = pos % 8; // Make this a function?

        int pieceType = Piece.getType(pieceNumber);
        int pieceColor = Piece.getColor(pieceNumber);

        switch (pieceType)
        {
            case Piece.pawn:
                int enPassantTarget = boardState.enPassantTarget;
                if (pieceColor == Piece.white)
                {
                    int oneSquareFwd = pos + 8;
                    int twoDquaresFwd = pos + 16;
                    int westDiagonal = pos + 7;
                    int eastDiagonal = pos + 9;

                    if (rank < 7 && ColorAtPos(oneSquareFwd, positionsArray) == Piece.empty)
                    {
                        newPositions.Add(oneSquareFwd);
                        if (rank == 1 && ColorAtPos(twoDquaresFwd, positionsArray) == Piece.empty)
                        {
                            newPositions.Add(twoDquaresFwd);
                        }
                    }

                    if ((rank < 7 && file > 0 && ColorAtPos(westDiagonal, positionsArray) == Piece.black) || westDiagonal == enPassantTarget)
                    {
                        newPositions.Add(westDiagonal);
                    }

                    if ((rank < 7 && file < 7 && ColorAtPos(eastDiagonal, positionsArray) == Piece.black) || eastDiagonal == enPassantTarget)
                    {
                        newPositions.Add(eastDiagonal);
                    }
                }
                else
                {
                    int oneSquareFwd = pos - 8;
                    int twoDquaresFwd = pos - 16;
                    int westDiagonal = pos - 9;
                    int eastDiagonal = pos - 7;

                    if (rank > 0 && ColorAtPos(oneSquareFwd, positionsArray) == Piece.empty)
                    {
                        newPositions.Add(oneSquareFwd);
                        if (rank == 6 && ColorAtPos(twoDquaresFwd, positionsArray) == Piece.empty)
                        {
                            newPositions.Add(twoDquaresFwd);
                        }
                    }

                    if ((rank > 0 && file > 0 && ColorAtPos(westDiagonal, positionsArray) == Piece.white) || westDiagonal == enPassantTarget)
                    {
                        newPositions.Add(westDiagonal);
                    }

                    if ((rank > 0 && file < 7 && ColorAtPos(eastDiagonal, positionsArray) == Piece.white) || eastDiagonal == enPassantTarget)
                    {
                        newPositions.Add(eastDiagonal);
                    }
                }
                break;

            case Piece.knight:
                
                if (rank < 7 && file < 6 && ColorAtPos(pos + 10, positionsArray) != pieceColor) 
                    newPositions.Add(pos + 10);
                
                if (rank < 7 && file > 1 && ColorAtPos(pos + 6, positionsArray) != pieceColor) 
                    newPositions.Add(pos + 6);
                
                if (rank > 0 && file < 6 && ColorAtPos(pos - 6, positionsArray) != pieceColor) 
                    newPositions.Add(pos - 6);
                
                if (rank > 0 && file > 1 && ColorAtPos(pos - 10, positionsArray) != pieceColor) 
                    newPositions.Add(pos - 10);
                
                if (rank < 6 && file < 7 && ColorAtPos(pos + 17, positionsArray) != pieceColor) 
                    newPositions.Add(pos + 17);
                
                if (rank < 6 && file > 0 && ColorAtPos(pos + 15, positionsArray) != pieceColor) 
                    newPositions.Add(pos + 15);
                
                if (rank > 1 && file < 7 && ColorAtPos(pos - 15, positionsArray) != pieceColor) 
                    newPositions.Add(pos - 15);
                
                if (rank > 1 && file > 0 && ColorAtPos(pos - 17, positionsArray) != pieceColor) 
                    newPositions.Add(pos - 17);

                break;

            case Piece.bishop:
                AddDiagonalPositions(pos, rank, file, pieceColor, positionsArray, newPositions);
                break;

            case Piece.rook:
                AddStraightPositions(pos, rank, file, pieceColor, positionsArray, newPositions);
                break;

            case Piece.queen:
                AddDiagonalPositions(pos, rank, file, pieceColor, positionsArray, newPositions);
                AddStraightPositions(pos, rank, file, pieceColor, positionsArray, newPositions);
                break;

            case Piece.king:
                CastlingRights castlingRights = boardState.castlingRights;

                // Do this sort of organization for knight (and if possible, pawn)
                if (file < 7 && ColorAtPos(pos + 1, positionsArray) != pieceColor) newPositions.Add(pos + 1);
                
                if (file > 0 && ColorAtPos(pos - 1, positionsArray) != pieceColor) newPositions.Add(pos - 1);

                if (rank < 7) {
                    if (ColorAtPos(pos + 8, positionsArray) != pieceColor) newPositions.Add(pos + 8);
                    
                    if (file < 7 && ColorAtPos(pos + 9, positionsArray) != pieceColor) newPositions.Add(pos + 9);
                    
                    if (file > 0 && ColorAtPos(pos + 7, positionsArray) != pieceColor) newPositions.Add(pos + 7);
                }

                if (rank > 0) {
                    if (ColorAtPos(pos - 8, positionsArray) != pieceColor) newPositions.Add(pos - 8);
                    
                    if (file < 7 && ColorAtPos(pos - 7, positionsArray) != pieceColor) newPositions.Add(pos - 7);
                    
                    if (file > 0 && ColorAtPos(pos - 9, positionsArray) != pieceColor) newPositions.Add(pos - 9);
                }

                if (pieceColor == Piece.white)
                {
                    if (castlingRights.kingSideWhite)
                    {
                        if (positionsArray[5] == Piece.empty && positionsArray[6] == Piece.empty)
                        {
                            newPositions.Add(6);
                        }
                    }

                    if (castlingRights.queenSideWhite)
                    {
                        if (positionsArray[3] == Piece.empty && positionsArray[2] == Piece.empty && positionsArray[1] == Piece.empty)
                        {
                            newPositions.Add(2);
                        }
                    }
                }
                else
                {
                    if (castlingRights.kingSideBlack)
                    {
                        if (positionsArray[7 * 8 + 5] == Piece.empty && positionsArray[7 * 8 + 6] == Piece.empty)
                        {
                            newPositions.Add(7 * 8 + 6);
                        }
                    }

                    if (castlingRights.queenSideBlack)
                    {
                        if (positionsArray[7 * 8 + 3] == Piece.empty && positionsArray[7 * 8 + 2] == Piece.empty && positionsArray[7 * 8 + 1] == Piece.empty)
                        {
                            newPositions.Add(7 * 8 + 2);
                        }
                    }
                }
                break;

            default:
                throw new Exception($"Invalid piece type: {pieceType}");
        }

        return newPositions;
    }

    public static bool IsInCheck(BoardState boardState)
    {
        BoardState checktestState = new(boardState.positionsArray, !boardState.whitesTurn, boardState.castlingRights, boardState.enPassantTarget, boardState.halfMoveClock, boardState.fullMoveNumber);
        return CanCaptureKing(checktestState);
    }

    public static List<int> GetNewPositions(int pos, BoardState boardState)
    {
        List<int> newPositons = new();

        foreach (int newPos in GetNewPositionsUnfiltered(pos, boardState))
        {
            BoardState resultantState = Result(boardState, new Action(pos, newPos));
            if (!CanCaptureKing(resultantState))
            {
                if (Piece.getType(boardState.positionsArray[pos]) == Piece.king && Math.Abs(pos - newPos) == 2)
                {
                    Action inBetweenAction = new(pos, (pos + newPos) / 2);
                    if (!(IsInCheck(boardState) || CanCaptureKing(Result(boardState, inBetweenAction))))
                        newPositons.Add(newPos);
                }
                else
                    newPositons.Add(newPos);
            }
        }

        return newPositons;
    }

    public static List<Action> UnfilteredActions(BoardState boardState)
    {
        List<Action> actions = new();
        int playerColor = boardState.whitesTurn ? Piece.white : Piece.black;
        int[] positionsArray = boardState.positionsArray;

        for (int pos = 0; pos < 64; pos++)
        {
            if (Piece.getColor(positionsArray[pos]) == playerColor)
            {
                foreach (int newPos in GetNewPositionsUnfiltered(pos, boardState))
                {
                    if (Piece.getType(positionsArray[pos]) == Piece.pawn && (newPos / 8 == 0 || newPos / 8 == 7))
                    {
                        actions.Add(new Action(pos, newPos, Piece.knight * playerColor));
                        actions.Add(new Action(pos, newPos, Piece.bishop * playerColor));
                        actions.Add(new Action(pos, newPos, Piece.rook * playerColor));
                        actions.Add(new Action(pos, newPos, Piece.queen * playerColor));
                    }
                    else actions.Add(new Action(pos, newPos));
                }
            }
        }
        return actions;
    }

    public static List<Action> Actions(BoardState boardState)
    {
        List<Action> actions = new();
        int playerColor = boardState.whitesTurn ? Piece.white : Piece.black;
        int[] positionsArray = boardState.positionsArray;

        for (int pos = 0; pos < 64; pos++)
        {
            if (Piece.getColor(positionsArray[pos]) == playerColor)
            {
                foreach (int newPos in GetNewPositions(pos, boardState))
                {
                    if (Piece.getType(positionsArray[pos]) == Piece.pawn && (newPos / 8 == 0 || newPos / 8 == 7))
                    {
                        actions.Add(new Action(pos, newPos, Piece.knight * playerColor));
                        actions.Add(new Action(pos, newPos, Piece.bishop * playerColor));
                        actions.Add(new Action(pos, newPos, Piece.rook * playerColor));
                        actions.Add(new Action(pos, newPos, Piece.queen * playerColor));
                    }
                    else actions.Add(new Action(pos, newPos));
                }
            }
        }
        
        //return actions;
        return actions.OrderByDescending(a=>a.ActionScoreGuess(boardState)).ToList();
    }

    public static bool CanCaptureKing(BoardState boardState)
    {
        foreach (Action action in UnfilteredActions(boardState))
        {
            if (Piece.getType(boardState.positionsArray[action.newPos]) == Piece.king)
                return true;
        }
        return false;
    }

    public static BoardState Result(BoardState boardState, Action action)
    {
        int[] positionsArray = (int[]) boardState.positionsArray.Clone();

        int initialPos = action.initialPos;
        int newPos = action.newPos;

        CastlingRights castlingRights = boardState.castlingRights;
        int enPassantTarget = BoardState.enPassantTargetNull;

        int halfMoveClock = boardState.halfMoveClock + 1;
        if (Piece.getType(positionsArray[initialPos]) == Piece.pawn || positionsArray[newPos] != Piece.empty)
            halfMoveClock = 0;

        switch (positionsArray[initialPos])
        {
            case Piece.king * Piece.white:
                castlingRights.kingSideWhite = false;
                castlingRights.queenSideWhite = false;

                // Queen side castling
                if (initialPos - newPos == 2)
                {
                    positionsArray[3] = Piece.rook * Piece.white;
                    positionsArray[0] = Piece.empty;
                }
                // King side castling
                else if (newPos - initialPos == 2)
                {
                    positionsArray[5] = Piece.rook * Piece.white;
                    positionsArray[7] = Piece.empty;
                }

                break;

            case Piece.king * Piece.black:
                castlingRights.kingSideBlack = false;
                castlingRights.queenSideBlack = false;

                // Queen side castling
                if (initialPos - newPos == 2)
                {
                    positionsArray[7 * 8 + 3] = Piece.rook * Piece.black;
                    positionsArray[7 * 8] = Piece.empty;
                }
                // King side castling
                else if (newPos - initialPos == 2)
                {
                    positionsArray[7 * 8 + 5] = Piece.rook * Piece.black;
                    positionsArray[7 * 8 + 7] = Piece.empty;
                }

                break;

            case Piece.rook * Piece.white:
                if (initialPos == 0) castlingRights.queenSideWhite = false; else castlingRights.kingSideWhite = false;
                break;

            case Piece.rook * Piece.black:
                if (initialPos == 7 * 8) castlingRights.queenSideBlack = false; else castlingRights.kingSideBlack = false;
                break;

            case Piece.pawn * Piece.white:
                if (newPos - initialPos == 16) enPassantTarget = (initialPos + newPos) / 2;
                if (action.newPos == boardState.enPassantTarget) positionsArray[newPos - 8] = Piece.empty;
                break;

            case Piece.pawn * Piece.black:
                if (initialPos - newPos == 16) enPassantTarget = (initialPos + newPos) / 2;
                if (action.newPos == boardState.enPassantTarget) positionsArray[newPos + 8] = Piece.empty;
                break;
        }

        switch (positionsArray[newPos])
        {
            case Piece.rook * Piece.white:
                if (newPos == 0) castlingRights.queenSideWhite = false; else castlingRights.kingSideWhite = false;
                break;

            case Piece.rook * Piece.black:
                if (newPos == 7 * 8) castlingRights.queenSideBlack = false; else castlingRights.kingSideBlack = false;
                break;
        }

        if (action.promotionTo == Piece.empty)
            positionsArray[newPos] = positionsArray[initialPos];
        else
            positionsArray[newPos] = action.promotionTo;
        positionsArray[initialPos] = Piece.empty;

        bool whitesTurn = !boardState.whitesTurn;

        int fullMoveNumber = boardState.fullMoveNumber;
        if (whitesTurn) fullMoveNumber++;

        return new BoardState(positionsArray, whitesTurn, castlingRights, enPassantTarget, halfMoveClock, fullMoveNumber);
    }

    public static bool Terminal(BoardState boardState)
    {
        if (boardState.halfMoveClock > 99) return true;
        if (Actions(boardState).Count == 0) return true;
        return false;
    }

    public static int EvaluationFunction(BoardState boardState) {
        int[] positionsArray = boardState.positionsArray;
        int totalPoints = 0;
        for (int pos = 0; pos < 64; pos++)
        {
            totalPoints += Piece.getPoints(positionsArray[pos]);
        }
        return totalPoints;
    }

    public static int Utility(BoardState boardState)
    {
        if (boardState.halfMoveClock > 99) return Piece.empty;
        if (IsInCheck(boardState)) 
            return (boardState.whitesTurn?Piece.black:Piece.white) * maxUtility;
        else
            return Piece.empty;
    }
}
