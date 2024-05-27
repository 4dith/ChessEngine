using System;
using System.Collections.Generic;

public static class Minimax
{
    public const int maxUtility = 50;
    
    static int ColorAtPos(Tuple<int, int> pos, int[,] positionsArray)
    {
        int rank = pos.Item1, file = pos.Item2;
        int occupantNumber = positionsArray[rank, file];
        return Piece.getColor(occupantNumber);
    }

    public static bool PosWithinBounds(Tuple<int, int> pos)
    {
        int rank = pos.Item1;
        int file = pos.Item2;

        if (0 <= rank && rank < 8 && 0 <= file && file < 8) return true;
        return false;
    }

    static void AddDirectionalPositions(int rank, int file, int pieceColor, int[,] positionsArray, List<Tuple<int, int>> directions, List<Tuple<int, int>> positionList)
    {
        foreach (Tuple<int, int> dir in directions)
        {
            int dirR = dir.Item1, dirF = dir.Item2;
            int numSquares = 1;
            Tuple<int, int> nextSquare = new(rank + numSquares * dirR, file + numSquares * dirF);
            while (PosWithinBounds(nextSquare))
            {
                int colorAtPos = ColorAtPos(nextSquare, positionsArray);
                if (colorAtPos == pieceColor) break;
                positionList.Add(nextSquare);
                if (colorAtPos == pieceColor * -1) break;

                numSquares++;
                nextSquare = new(rank + numSquares * dirR, file + numSquares * dirF); ;
            }
        }
    }

    public static List<Tuple<int, int>> GetNewPositionsUnfiltered(Tuple<int, int> position, BoardState boardState)
    {
        // TODO: Castling
        List<Tuple<int, int>> newPositions = new();

        int rank = position.Item1;
        int file = position.Item2;

        int[,] positionsArray = boardState.positionsArray;

        int pieceNumber = positionsArray[rank, file];

        if (pieceNumber == Piece.empty) return new();

        int pieceType = Piece.getType(pieceNumber);
        int pieceColor = Piece.getColor(pieceNumber);

        switch (pieceType)
        {
            case Piece.pawn:
                Tuple<int, int> enPassantTarget = boardState.enPassantTarget;
                if (pieceColor == Piece.white)
                {
                    Tuple<int, int> oneSquareFwd = new(rank + 1, file);
                    Tuple<int, int> twoDquaresFwd = new(rank + 2, file);
                    Tuple<int, int> westDiagonal = new(rank + 1, file - 1);
                    Tuple<int, int> eastDiagonal = new(rank + 1, file + 1);

                    if (PosWithinBounds(oneSquareFwd) && ColorAtPos(oneSquareFwd, positionsArray) == Piece.empty)
                    {
                        newPositions.Add(oneSquareFwd);
                        if (PosWithinBounds(twoDquaresFwd) && rank == 1 && ColorAtPos(twoDquaresFwd, positionsArray) == Piece.empty)
                        {
                            newPositions.Add(twoDquaresFwd);
                        }
                    }

                    if (PosWithinBounds(westDiagonal) && ColorAtPos(westDiagonal, positionsArray) == Piece.black || westDiagonal.Equals(enPassantTarget))
                    {
                        newPositions.Add(westDiagonal);
                    }

                    if (PosWithinBounds(eastDiagonal) && ColorAtPos(eastDiagonal, positionsArray) == Piece.black || eastDiagonal.Equals(enPassantTarget))
                    {
                        newPositions.Add(eastDiagonal);
                    }
                }
                else
                {
                    Tuple<int, int> oneSquareFwd = new(rank - 1, file);
                    Tuple<int, int> twoDquaresFwd = new(rank - 2, file);
                    Tuple<int, int> westDiagonal = new(rank - 1, file - 1);
                    Tuple<int, int> eastDiagonal = new(rank - 1, file + 1);

                    if (PosWithinBounds(oneSquareFwd) && ColorAtPos(oneSquareFwd, positionsArray) == Piece.empty)
                    {
                        newPositions.Add(oneSquareFwd);
                        if (PosWithinBounds(twoDquaresFwd) && rank == 6 && ColorAtPos(twoDquaresFwd, positionsArray) == Piece.empty)
                        {
                            newPositions.Add(twoDquaresFwd);
                        }
                    }

                    if (PosWithinBounds(westDiagonal) && ColorAtPos(westDiagonal, positionsArray) == Piece.white || westDiagonal.Equals(enPassantTarget))
                    {
                        newPositions.Add(westDiagonal);
                    }

                    if (PosWithinBounds(eastDiagonal) && ColorAtPos(eastDiagonal, positionsArray) == Piece.white || eastDiagonal.Equals(enPassantTarget))
                    {
                        newPositions.Add(eastDiagonal);
                    }
                }
                break;

            case Piece.knight:
                List<Tuple<int, int>> candidateKnightPositions = new() {
                    new(rank + 2, file - 1), new(rank + 2, file + 1),
                    new(rank + 1, file - 2), new(rank + 1, file + 2),
                    new(rank - 1, file - 2), new(rank - 1, file + 2),
                    new(rank - 2, file - 1), new(rank - 2, file + 1)
                };

                foreach (Tuple<int, int> newPos in candidateKnightPositions)
                {
                    if (PosWithinBounds(newPos))
                    {
                        int colorAtPos = ColorAtPos(newPos, positionsArray);
                        if (colorAtPos != pieceColor)
                        {
                            newPositions.Add(newPos);
                        }
                    }
                }
                break;

            case Piece.bishop:
                List<Tuple<int, int>> bishopDirs = new() {
                    new(1, -1), new(1, 1),
                    new(-1, -1), new(-1, 1)
                };
                AddDirectionalPositions(rank, file, pieceColor, positionsArray, bishopDirs, newPositions);
                break;

            case Piece.rook:
                List<Tuple<int, int>> rookDirs = new() {
                    new(1, 0), new(-1, 0),
                    new(0, -1), new(0, 1)
                };
                AddDirectionalPositions(rank, file, pieceColor, positionsArray, rookDirs, newPositions);
                break;

            case Piece.queen:
                List<Tuple<int, int>> queenDirs = new() {
                    new(1, -1), new(1, 1),
                    new(-1, -1), new(-1, 1),
                    new(1, 0), new(-1, 0),
                    new(0, -1), new(0, 1)
                };
                AddDirectionalPositions(rank, file, pieceColor, positionsArray, queenDirs, newPositions);
                break;

            case Piece.king:
                CastlingRights castlingRights = boardState.castlingRights;
                List<Tuple<int, int>> candidateKingPositions = new() {
                    new(rank + 1, file - 1), new(rank + 1, file), new(rank + 1, file + 1),
                    new(rank, file - 1), new(rank, file + 1),
                    new(rank - 1, file - 1), new(rank - 1, file), new(rank - 1, file + 1)
                };

                foreach (Tuple<int, int> newPos in candidateKingPositions)
                {
                    if (PosWithinBounds(newPos))
                    {
                        int colorAtPos = ColorAtPos(newPos, positionsArray);
                        if (colorAtPos != pieceColor)
                        {
                            newPositions.Add(newPos);
                        }
                    }
                }

                if (pieceColor == Piece.white)
                {
                    if (castlingRights.kingSideWhite)
                    {
                        if (positionsArray[0, 5] == Piece.empty && positionsArray[0, 6] == Piece.empty)
                        {
                            newPositions.Add(new(0, 6));
                        }
                    }

                    if (castlingRights.queenSideWhite)
                    {
                        if (positionsArray[0, 3] == Piece.empty && positionsArray[0, 2] == Piece.empty && positionsArray[0, 1] == Piece.empty)
                        {
                            newPositions.Add(new(0, 2));
                        }
                    }
                }
                else
                {
                    if (castlingRights.kingSideBlack)
                    {
                        if (positionsArray[7, 5] == Piece.empty && positionsArray[7, 6] == Piece.empty)
                        {
                            newPositions.Add(new(7, 6));
                        }
                    }

                    if (castlingRights.queenSideBlack)
                    {
                        if (positionsArray[7, 3] == Piece.empty && positionsArray[7, 2] == Piece.empty && positionsArray[7, 1] == Piece.empty)
                        {
                            newPositions.Add(new(7, 2));
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

    public static List<Tuple<int, int>> GetNewPositions(Tuple<int, int> position, BoardState boardState)
    {
        List<Tuple<int, int>> newPositons = new();
        int rank = position.Item1, file = position.Item2;

        foreach (Tuple<int, int> newPos in GetNewPositionsUnfiltered(position, boardState))
        {
            BoardState resultantState = Result(boardState, new Action(position, newPos));
            if (!CanCaptureKing(resultantState))
            {
                if (Piece.getType(boardState.positionsArray[rank, file]) == Piece.king && Math.Abs(file - newPos.Item2) == 2)
                {
                    Action inBetweenAction = new(new(rank, file), new(rank, (file + newPos.Item2) / 2));
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
        int[,] positionsArray = boardState.positionsArray;

        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                if (Piece.getColor(positionsArray[rank, file]) == playerColor)
                {
                    Tuple<int, int> initialPos = new(rank, file);
                    foreach (Tuple<int, int> newPos in GetNewPositionsUnfiltered(initialPos, boardState))
                    {
                        if (Piece.getType(positionsArray[rank, file]) == Piece.pawn && (newPos.Item1 == 0 || newPos.Item1 == 7))
                        {
                            actions.Add(new Action(initialPos, newPos, Piece.knight * playerColor));
                            actions.Add(new Action(initialPos, newPos, Piece.bishop * playerColor));
                            actions.Add(new Action(initialPos, newPos, Piece.rook * playerColor));
                            actions.Add(new Action(initialPos, newPos, Piece.queen * playerColor));
                        }
                        else actions.Add(new Action(initialPos, newPos));
                    }
                }
            }
        }
        return actions;
    }

    public static List<Action> Actions(BoardState boardState)
    {
        List<Action> actions = new();
        int playerColor = boardState.whitesTurn ? Piece.white : Piece.black;
        int[,] positionsArray = boardState.positionsArray;

        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                if (Piece.getColor(positionsArray[rank, file]) == playerColor)
                {
                    Tuple<int, int> initialPos = new(rank, file);
                    foreach (Tuple<int, int> newPos in GetNewPositions(initialPos, boardState))
                    {
                        if (Piece.getType(positionsArray[rank, file]) == Piece.pawn && (newPos.Item1 == 0 || newPos.Item1 == 7))
                        {
                            actions.Add(new Action(initialPos, newPos, Piece.knight * playerColor));
                            actions.Add(new Action(initialPos, newPos, Piece.bishop * playerColor));
                            actions.Add(new Action(initialPos, newPos, Piece.rook * playerColor));
                            actions.Add(new Action(initialPos, newPos, Piece.queen * playerColor));
                        }
                        else actions.Add(new Action(initialPos, newPos));
                    }
                }
            }
        }
        return actions;
    }

    public static bool CanCaptureKing(BoardState boardState)
    {
        foreach (Action action in UnfilteredActions(boardState))
        {
            if (Piece.getType(boardState.positionsArray[action.newPos.Item1, action.newPos.Item2]) == Piece.king)
                return true;
        }
        return false;
    }

    public static BoardState Result(BoardState boardState, Action action)
    {
        int[,] positionsArray = (int[,])boardState.positionsArray.Clone();
        int initialRank = action.initialPos.Item1, initialFile = action.initialPos.Item2;
        int finalRank = action.newPos.Item1, finalFile = action.newPos.Item2;

        CastlingRights castlingRights = boardState.castlingRights;
        Tuple<int, int> enPassantTarget = new(-1, -1);

        int halfMoveClock = boardState.halfMoveClock + 1;
        if (Piece.getType(positionsArray[initialRank, initialFile]) == Piece.pawn || positionsArray[finalRank, finalFile] != Piece.empty)
            halfMoveClock = 0;

        switch (positionsArray[initialRank, initialFile])
        {
            case Piece.king * Piece.white:
                castlingRights.kingSideWhite = false;
                castlingRights.queenSideWhite = false;

                // Queen side castling
                if (initialFile - finalFile == 2)
                {
                    positionsArray[0, 3] = Piece.rook * Piece.white;
                    positionsArray[0, 0] = Piece.empty;
                }
                // King side castling
                else if (finalFile - initialFile == 2)
                {
                    positionsArray[0, 5] = Piece.rook * Piece.white;
                    positionsArray[0, 7] = Piece.empty;
                }

                break;

            case Piece.king * Piece.black:
                castlingRights.kingSideBlack = false;
                castlingRights.queenSideBlack = false;

                // Queen side castling
                if (initialFile - finalFile == 2)
                {
                    positionsArray[7, 3] = Piece.rook * Piece.black;
                    positionsArray[7, 0] = Piece.empty;
                }
                // King side castling
                else if (finalFile - initialFile == 2)
                {
                    positionsArray[7, 5] = Piece.rook * Piece.black;
                    positionsArray[7, 7] = Piece.empty;
                }

                break;

            case Piece.rook * Piece.white:
                if (initialFile == 0) castlingRights.queenSideWhite = false; else castlingRights.kingSideWhite = false;
                break;

            case Piece.rook * Piece.black:
                if (initialFile == 0) castlingRights.queenSideBlack = false; else castlingRights.kingSideBlack = false;
                break;

            case Piece.pawn * Piece.white:
                if (initialRank == 1 && finalRank == 3) enPassantTarget = new(2, initialFile);
                if (action.newPos.Equals(boardState.enPassantTarget)) positionsArray[initialRank, finalFile] = Piece.empty;
                break;

            case Piece.pawn * Piece.black:
                if (initialRank == 6 && finalRank == 4) enPassantTarget = new(5, initialFile);
                if (action.newPos.Equals(boardState.enPassantTarget)) positionsArray[initialRank, finalFile] = Piece.empty;
                break;
        }

        switch (positionsArray[finalRank, finalFile])
        {
            case Piece.rook * Piece.white:
                if (initialFile == 0) castlingRights.queenSideWhite = false; else castlingRights.kingSideWhite = false;
                break;

            case Piece.rook * Piece.black:
                if (initialFile == 0) castlingRights.queenSideBlack = false; else castlingRights.kingSideBlack = false;
                break;
        }

        if (action.promotionTo == Piece.empty)
            positionsArray[finalRank, finalFile] = positionsArray[initialRank, initialFile];
        else
            positionsArray[finalRank, finalFile] = action.promotionTo;
        positionsArray[initialRank, initialFile] = Piece.empty;

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
        int[,] positionsArray = boardState.positionsArray;
        int totalPoints = 0;
        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++) {
                totalPoints += Piece.getPoints(positionsArray[rank, file]);
            }
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
