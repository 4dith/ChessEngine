using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    Color highlightColor;
    Color transparent = new Color(0, 0, 0, 0);

    SpriteRenderer spriteRenderer;

    int prevRank;
    int prevFile;
    bool visible;
    bool whitesTurn;

    List<Tuple<int, int>> newPositions;

    public BoardManager boardManager;
    public GameObject movesIndicator;

    Tuple<int, int> GetMouseCoordinates()
    {
        Vector3 mouseToWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Tuple<int, int>((int)Math.Floor(mouseToWorldPos.y), (int)Math.Floor(mouseToWorldPos.x));
    }

    Vector3 GetPositionOnBoard(int rank, int file) {
        return new Vector3(file + 0.5f, rank + 0.5f);
    }

    void Disappear()
    {
        while (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        
        spriteRenderer.color = transparent;
        visible = false;
    }

    void Appear(List<Tuple<int, int>> newPositions)
    {
        spriteRenderer.color = highlightColor;
        visible = true;

        foreach (Tuple<int, int> pos in newPositions)
        {
            Instantiate(movesIndicator, GetPositionOnBoard(pos.Item1, pos.Item2), Quaternion.identity, transform);
        }
    }

    void Start()
    {
        whitesTurn = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        highlightColor = spriteRenderer.color;
        newPositions = new();
        Disappear();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
        {
            Tuple<int, int> mouseCoordinates = GetMouseCoordinates();
            int rank = mouseCoordinates.Item1;
            int file = mouseCoordinates.Item2;
            BoardState currentState = boardManager.currentState;

            int colorAtPos = Piece.getColor(currentState.positionsArray[rank, file]);

            if (colorAtPos == Piece.white && whitesTurn || colorAtPos == Piece.black && !whitesTurn)
            {
                newPositions = Minimax.GetNewPositions(mouseCoordinates, currentState);
                if (Minimax.PosWithinBounds(mouseCoordinates))
                {
                    if (prevRank == rank && prevFile == file)
                    {
                        if (visible) Disappear(); else Appear(newPositions);
                    }
                    else
                    {
                        transform.position = GetPositionOnBoard(rank, file);
                        prevRank = rank;
                        prevFile = file;
                        Disappear();
                        Appear(newPositions);
                    }
                }
            } else if (visible && newPositions.Contains(mouseCoordinates)) {
                Disappear();
                boardManager.SetBoard(Minimax.Result(currentState, new Action(new(prevRank, prevFile), new(rank, file))));
                whitesTurn = !whitesTurn;
            }
        }
    }
}
