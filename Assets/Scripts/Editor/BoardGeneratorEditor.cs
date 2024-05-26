using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardGenerator))]
public class BoardGeneratorEditor : Editor
{
    public Color prevWhiteColor;
    public Color prevBlackColor;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BoardGenerator board = (BoardGenerator) target;
        if (GUILayout.Button("Setup Board")) {
            board.GenerateBoard();
            
            BoardManager boardManager = board.GetComponentInChildren<BoardManager>();
            boardManager.SetBoard(BoardState.FENStringToBoardState(boardManager.initialFENString));
        }

        if (prevWhiteColor != board.whiteColor) {
            board.SetBoardColors();
            prevWhiteColor = board.whiteColor;
        }

        if (prevBlackColor != board.blackColor) {
            board.SetBoardColors();
            prevBlackColor = board.blackColor;
        }
    }
}
