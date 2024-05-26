using System;
using System.Collections.Generic;

public class BoardState
{
    public int[,] positionsArray;
    public bool whitesTurn;
    public CastlingRights castlingRights;
    public Tuple<int, int> enPassantTarget;
    public int halfMoveClock;
    public int fullMoveNumber;

    public BoardState(int[,] positionsArray, bool whitesTurn, CastlingRights castlingRights, Tuple<int, int> enPassantTarget, int halfMoveClock, int fullMoveNumber)
    {
        this.positionsArray = positionsArray;
        this.whitesTurn = whitesTurn;
        this.castlingRights = castlingRights;
        this.enPassantTarget = enPassantTarget;
        this.halfMoveClock = halfMoveClock;
        this.fullMoveNumber = fullMoveNumber;
    }

    public static BoardState FENStringToBoardState(string fenString)
    {
        Dictionary<char, int> fenPiecesDict = new() {
            {'P', Piece.pawn * Piece.white},
            {'p', Piece.pawn * Piece.black},
            {'N', Piece.knight * Piece.white},
            {'n', Piece.knight * Piece.black},
            {'B', Piece.bishop * Piece.white},
            {'b', Piece.bishop * Piece.black},
            {'R', Piece.rook * Piece.white},
            {'r', Piece.rook * Piece.black},
            {'Q', Piece.queen * Piece.white},
            {'q', Piece.queen * Piece.black},
            {'K', Piece.king * Piece.white},
            {'k', Piece.king * Piece.black}
        };

        string[] fields = fenString.Split(' ');
        if (fields.Length != 6)
        {
            throw new Exception($"Invalid FEN string: There are {fields.Length} fields");
        }

        // First Field: Positions of pieces
        int[,] positionsArray = new int[8, 8];
        string[] positionStrings = fields[0].Split('/');

        if (positionStrings.Length != 8)
        {
            throw new Exception($"Invalid FEN string: There are {positionStrings.Length} ranks");
        }

        for (int rank = 7; rank >= 0; rank--)
        {
            string rankString = positionStrings[7 - rank];
            int file = 0;
            foreach (char c in rankString)
            {
                if (Char.IsDigit(c))
                {
                    file += (int)char.GetNumericValue(c);
                }
                else
                {
                    positionsArray[rank, file] = fenPiecesDict[c];
                    file++;
                }
            }
        }

        // Second field: Which player's turn is it?
        bool whitesTurn = fields[1][0] switch
        {
            'w' => true,
            'b' => false,
            _ => throw new Exception($"Invalid FEN String: Active color = {fields[2]}"),
        };

        // Third fields: Castling Rights
        CastlingRights castlingRights = new()
        {
            kingSideWhite = false,
            queenSideWhite = false,
            kingSideBlack = false,
            queenSideBlack = false
        };

        string castlingRightsString = fields[2];
        if (castlingRightsString != "-")
        {
            foreach (char c in castlingRightsString)
            {
                switch (c)
                {
                    case 'K':
                        castlingRights.kingSideWhite = true;
                        break;

                    case 'Q':
                        castlingRights.queenSideWhite = true;
                        break;

                    case 'k':
                        castlingRights.kingSideBlack = true;
                        break;

                    case 'q':
                        castlingRights.queenSideBlack = true;
                        break;

                    default:
                        throw new Exception($"Invalid FEN String: Castling rights = {castlingRightsString}");
                }
            }
        }

        // Fourth Field: Possible En-Passant targets
        Tuple<int, int> enPassantTarget = new(-1, -1);

        string enPassantTargetString = fields[3];
        if (enPassantTargetString != "-")
        {
            if (enPassantTargetString.Length != 2)
                throw new Exception($"Invalid FEN String: En-passant target = {enPassantTargetString}");
            int file = enPassantTargetString[0] - 'a';
            int rank = enPassantTargetString[1] - '1';
            
            if (Minimax.PosWithinBounds(new(rank, file))) {
                enPassantTarget = new(rank, file);
            } else {
                throw new Exception($"Invalid FEN String: En-passant target = {enPassantTargetString}");
            }
        }

        // Fifth and sixth Fields: Half Move Clock and Full Move Number
        if (!Int32.TryParse(fields[4], out int halfMoveClock))
        {
            throw new Exception($"Invalid FEN String: Half Move Clock = {fields[4]}");
        }
        if (!Int32.TryParse(fields[5], out int fullMoveNumber)) {
            throw new Exception($"Invalid FEN String: Full Move Number = {fields[5]}");
        }

        return new BoardState(positionsArray, whitesTurn, castlingRights, enPassantTarget, halfMoveClock, fullMoveNumber);
    }
}

public struct CastlingRights
{
    public CastlingRights(bool K, bool Q, bool k, bool q)
    {
        kingSideWhite = K;
        queenSideWhite = Q;
        kingSideBlack = K;
        queenSideBlack = Q;
    }

    public bool kingSideWhite;
    public bool queenSideWhite;
    public bool kingSideBlack;
    public bool queenSideBlack;
}
