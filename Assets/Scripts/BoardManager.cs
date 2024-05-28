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

    Quaternion pieceRotation;

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
        int[] positionsArray = boardState.positionsArray;

        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                int piece = positionsArray[rank * 8 + file];
                if (piece != 0) {
                    Instantiate(GetPrefab(piece), new Vector3(file + 0.5f, rank + 0.5f, 0f), pieceRotation, transform);
                }
            }
        }
    }

    void Start() {
        pieceRotation = FindObjectOfType<PlayerController>().playerIsWhite?Quaternion.identity:Quaternion.Euler(0, 0, 180);

        BoardState initialState = BoardState.FENStringToBoardState(initialFENString);
        SetBoard(initialState);
    }
}
