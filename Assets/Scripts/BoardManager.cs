using System;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("White Pieces")]

    public GameObject whitePawnPrefab;
    public GameObject whiteKnightPrefab;
    public GameObject whiteBishopPrefab;
    public GameObject whiteRookPrefab;
    public GameObject whiteQueenPrefab;
    public GameObject whiteKingPrefab;

    [Header("Black Pieces")]
    public GameObject blackPawnPrefab;
    public GameObject blackKnightPrefab;
    public GameObject blackBishopPrefab;
    public GameObject blackRookPrefab;
    public GameObject blackQueenPrefab;
    public GameObject blackKingPrefab;

    [Header("Initial Configuration")]
    public string initialFENString;

    public BoardState currentState;

    public GameObject GetPrefab(int pieceNumber) {
        switch (Piece.getColor(pieceNumber))
        {
            case Piece.white:
                switch (Piece.getType(pieceNumber))
                {
                    case Piece.pawn:
                        return whitePawnPrefab;
                    
                    case Piece.knight:
                        return whiteKnightPrefab;
                    
                    case Piece.bishop:
                        return whiteBishopPrefab;
                    
                    case Piece.rook:
                        return whiteRookPrefab;
                    
                    case Piece.queen:
                        return whiteQueenPrefab;
                    
                    case Piece.king:
                        return whiteKingPrefab;
                }
                break;
            
            case Piece.black:
                switch (Piece.getType(pieceNumber))
                {
                    case Piece.pawn:
                        return blackPawnPrefab;
                    
                    case Piece.knight:
                        return blackKnightPrefab;
                    
                    case Piece.bishop:
                        return blackBishopPrefab;
                    
                    case Piece.rook:
                        return blackRookPrefab;
                    
                    case Piece.queen:
                        return blackQueenPrefab;
                    
                    case Piece.king:
                        return blackKingPrefab;
                }
                break;
        }

        throw new Exception($"Invalid Piece Number: {pieceNumber}");
    }

    public void SetBoard(BoardState boardState) {
        while (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        currentState = boardState;
        int[,] positionsArray = boardState.positionsArray;

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                int piece = positionsArray[y, x];
                if (piece != 0) {
                    Instantiate(GetPrefab(piece), new Vector3(x + 0.5f, y + 0.5f, 0f), Quaternion.identity, transform);
                }
            }
        }
    }

    void Start() {
        BoardState initialState = BoardState.FENStringToBoardState(initialFENString);
        SetBoard(initialState);
    }
}
