using System;
using System.Collections.Generic;
using UnityEngine;

public class Agent: MonoBehaviour
{
    public int maxDepth = 3;
    public BoardManager boardManager;

    int MaxValue(BoardState boardState, int alpha, int beta, int depth=1) {
        int maxVal = int.MinValue;
        if (Minimax.Terminal(boardState)) return Minimax.Utility(boardState);
        if (depth == maxDepth) return Minimax.EvaluationFunction(boardState);

        foreach (Action action in Minimax.Actions(boardState)) {
            int eval = MinValue(Minimax.Result(boardState, action), alpha, beta, depth + 1);
            maxVal = Math.Max(maxVal, eval);
            alpha = Mathf.Max(alpha, eval);
            if (beta <= alpha) break;
        }

        return maxVal;
    }

    int MinValue(BoardState boardState, int alpha, int beta, int depth=1) {
        int minVal = int.MaxValue;

        if (Minimax.Terminal(boardState)) return Minimax.Utility(boardState);
        if (depth == maxDepth) return Minimax.EvaluationFunction(boardState);

        foreach (Action action in Minimax.Actions(boardState)) {
            int eval = MaxValue(Minimax.Result(boardState, action), alpha, beta, depth + 1);
            minVal = Math.Min(minVal, eval);
            beta = Math.Min(beta, eval);
            if (beta <= alpha) break;
        }

        return minVal;
    }
    
    public void MakeAMove(BoardState boardState) {
        List<Action> actions = Minimax.Actions(boardState);
        Action pickedAction = actions[0];

        int alpha = int.MinValue, beta = int.MaxValue;
        
        if (boardState.whitesTurn) {
            int maxVal = int.MinValue;
            foreach (Action action in actions)
            {
                int eval = MinValue(Minimax.Result(boardState, action), alpha, beta);
                if (eval > maxVal) {
                    maxVal = eval;
                    pickedAction = action;
                }
            }
        } else {
            int minVal = int.MaxValue;
            foreach (Action action in actions)
            {
                int eval = MaxValue(Minimax.Result(boardState, action), alpha, beta);
                if (eval < minVal) {
                    minVal = eval;
                    pickedAction = action;
                }
            }
        }

        boardManager.SetBoard(Minimax.Result(boardState, pickedAction));
    }
}
