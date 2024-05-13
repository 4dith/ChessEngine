using System;
using UnityEditor.UI;
using UnityEngine;

public class Pieces : MonoBehaviour
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

    // Internal representations of pieces

    static readonly Piece whitePawn = new Piece(Piece.pawn, Piece.white);
    static readonly Piece whiteKnight = new Piece(Piece.knight, Piece.white);
    static readonly Piece whiteBishop = new Piece(Piece.bishop, Piece.white);
    static readonly Piece whiteRook = new Piece(Piece.rook, Piece.white);
    static readonly Piece whiteQueen = new Piece(Piece.queen, Piece.white);
    static readonly Piece whiteKing = new Piece(Piece.king, Piece.white);

    static readonly Piece blackPawn = new Piece(Piece.pawn, Piece.black);
    static readonly Piece blackKnight = new Piece(Piece.knight, Piece.black);
    static readonly Piece blackBishop = new Piece(Piece.bishop, Piece.black);
    static readonly Piece blackRook = new Piece(Piece.rook, Piece.black);
    static readonly Piece blackQueen = new Piece(Piece.queen, Piece.black);
    static readonly Piece blackKing = new Piece(Piece.king, Piece.black);

    public Piece[,] initialState = new Piece[8, 8] {
        {whiteRook, whiteKnight, whiteBishop, whiteQueen, whiteKing, whiteBishop, whiteKnight, whiteRook}, 
        {whitePawn, whitePawn, whitePawn, whitePawn, whitePawn, whitePawn, whitePawn, whitePawn},
        {null, null, null, null, null, null, null, null}, 
        {null, null, null, null, null, null, null, null}, 
        {null, null, null, null, null, null, null, null}, 
        {null, null, null, null, null, null, null, null}, 
        {blackPawn, blackPawn, blackPawn, blackPawn, blackPawn, blackPawn, blackPawn, blackPawn},
        {blackRook, blackKnight, blackBishop, blackQueen, blackKing, blackBishop, blackKnight, blackRook},
    };

    public GameObject GetPrefab(Piece piece) {
        if (piece == whitePawn) return whitePawnPrefab;
        if (piece == whiteKnight) return whiteKnightPrefab;
        if (piece == whiteBishop) return whiteBishopPrefab;
        if (piece == whiteRook) return whiteRookPrefab;
        if (piece == whiteQueen) return whiteQueenPrefab;
        if (piece == whiteKing) return whiteKingPrefab;

        if (piece == blackPawn) return blackPawnPrefab;
        if (piece == blackKnight) return blackKnightPrefab;
        if (piece == blackBishop) return blackBishopPrefab;
        if (piece == blackRook) return blackRookPrefab;
        if (piece == blackQueen) return blackQueenPrefab;
        if (piece == blackKing) return blackKingPrefab;

        throw new Exception("Invalid Piece");
    }

    public void SetBoard(Piece[,] boardState) {
        while (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                Piece piece = boardState[y, x];
                if (piece != null) {
                    Instantiate(GetPrefab(piece), new Vector3(x + 0.5f, y + 0.5f, 0f), Quaternion.identity, this.transform);
                }
            }
        }
    }
}
