using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Board))]
public class BoardEditor : Editor
{
    public Color prevWhiteColor;
    public Color prevBlackColor;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Board board = (Board) target;
        if (GUILayout.Button("Setup Board")) {
            board.GenerateBoard();
            
            Pieces pieceManager = board.GetComponentInChildren<Pieces>();
            pieceManager.SetBoard(pieceManager.initialState);
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
