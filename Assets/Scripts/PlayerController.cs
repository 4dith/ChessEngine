using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    Color highlightColor;
    Color transparent = new Color(0, 0, 0, 0);

    SpriteRenderer spriteRenderer;

    public bool playerIsWhite;

    [HideInInspector]
    public int prevRank;
    [HideInInspector]
    public int prevFile;

    [HideInInspector]
    public int rank;
    [HideInInspector]
    public int file;

    bool visible;

    [HideInInspector]
    public bool whitesTurn;

    [HideInInspector]
    public bool promotionMode;

    [HideInInspector]
    public bool gameOver;

    List<Tuple<int, int>> newPositions;

    public BoardManager boardManager;
    public Agent agent;
    public GameObject movesIndicator;
    public PromotionMenu promotionMenu;

    Tuple<int, int> GetMouseCoordinates()
    {
        Vector3 mouseToWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Tuple<int, int>((int)Math.Floor(mouseToWorldPos.y), (int)Math.Floor(mouseToWorldPos.x));
    }

    Vector3 GetPositionOnBoard(int rank, int file)
    {
        return new Vector3(file + 0.5f, rank + 0.5f);
    }

    void Disappear()
    {
        while (transform.childCount > 0)
        {
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
        Camera.main.transform.rotation = playerIsWhite ? Quaternion.identity : Quaternion.Euler(0, 0, 180);

        whitesTurn = true;
        promotionMode = false;
        gameOver = false;
        promotionMenu.gameObject.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
        highlightColor = spriteRenderer.color;
        newPositions = new();
        Disappear();
    }

    public bool GameOverCheck()
    {
        if (Minimax.Terminal(boardManager.currentState))
        {
            int utility = Minimax.Utility(boardManager.currentState);
            switch (utility)
            {
                case Piece.white * Minimax.maxUtility:
                    Debug.Log("Game Over: White won");
                    break;

                case Piece.black * Minimax.maxUtility:
                    Debug.Log("Game Over: Black won");
                    break;

                case Piece.empty:
                    Debug.Log("Game Over: It's a draw");
                    break;

                default:
                    throw new Exception($"Invalid Utility: {utility}");
            }
            return true;
        }
        else return false;
    }

    void Update()
    {
        if (!gameOver && playerIsWhite == whitesTurn)
        {
            if (!promotionMode)
            {
                if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
                {
                    Tuple<int, int> mouseCoordinates = GetMouseCoordinates();
                    if (Minimax.PosWithinBounds(mouseCoordinates))
                    {
                        rank = mouseCoordinates.Item1;
                        file = mouseCoordinates.Item2;
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
                        }
                        else if (visible && newPositions.Contains(mouseCoordinates))
                        {
                            Disappear();
                            int piece = currentState.positionsArray[prevRank, prevFile];
                            if (Piece.getType(piece) == Piece.pawn && (rank == 0 || rank == 7))
                            {
                                promotionMenu.gameObject.SetActive(true);
                            }
                            else
                            {
                                boardManager.SetBoard(Minimax.Result(currentState, new Action(new(prevRank, prevFile), new(rank, file))));
                                whitesTurn = !whitesTurn;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            gameOver = GameOverCheck();
            if (!gameOver)
            {
                float timestamp = Time.realtimeSinceStartup;
                agent.MakeAMove(boardManager.currentState);
                Debug.Log($"{Time.realtimeSinceStartup - timestamp} seconds");
                whitesTurn = !whitesTurn;
                gameOver = GameOverCheck();
            }
        }
    }
}
