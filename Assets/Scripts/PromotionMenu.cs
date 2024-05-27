using UnityEngine;
using UnityEngine.UI;

public class PromotionMenu : MonoBehaviour
{
    public PlayerController playerCtrl;

    [Header("White Sprites")]
    public Sprite whiteKnightSprite;
    public Sprite whiteBishopSprite;
    public Sprite whiteRookSprite;
    public Sprite whiteQueenSprite;

    [Header("Black Sprites")]
    public Sprite blackKnightSprite;
    public Sprite blackBishopSprite;
    public Sprite blackRookSprite;
    public Sprite blackQueenSprite;

    [Header("Button Images")]
    public Image kinghtButtonImage;
    public Image bishopButtonImage;
    public Image rookButtonImage;
    public Image queenButtonImage;

    void OnEnable() {
        playerCtrl.promotionMode = true;

        if (playerCtrl.whitesTurn) {
            kinghtButtonImage.sprite = whiteKnightSprite;
            bishopButtonImage.sprite = whiteBishopSprite;
            rookButtonImage.sprite = whiteRookSprite;
            queenButtonImage.sprite = whiteQueenSprite;
        } else {
            kinghtButtonImage.sprite = blackKnightSprite;
            bishopButtonImage.sprite = blackBishopSprite;
            rookButtonImage.sprite = blackRookSprite;
            queenButtonImage.sprite = blackQueenSprite;
        }
    }

    public void PromoteTo(int pieceType) {
        BoardManager boardManager = playerCtrl.boardManager;
        if (playerCtrl.whitesTurn) {
            boardManager.SetBoard(Minimax.Result(boardManager.currentState, new Action(new(playerCtrl.prevRank, playerCtrl.prevFile), new(playerCtrl.rank, playerCtrl.file), pieceType * Piece.white)));
        } else {
            boardManager.SetBoard(Minimax.Result(boardManager.currentState, new Action(new(playerCtrl.prevRank, playerCtrl.prevFile), new(playerCtrl.rank, playerCtrl.file), pieceType * Piece.black)));
        }
        
        playerCtrl.whitesTurn = !playerCtrl.whitesTurn;
        playerCtrl.promotionMode = false;
        gameObject.SetActive(false);
    }
}
