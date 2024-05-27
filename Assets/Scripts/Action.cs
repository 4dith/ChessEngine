using System;
using System.Collections.Generic;

public class Action
{
    public Tuple<int, int> initialPos;
    public Tuple<int, int> newPos;
    public int promotionTo;
    
    public Action(Tuple<int, int> initialPos, Tuple<int, int> newPos, int promotionTo = Piece.empty) {
        this.initialPos = initialPos;
        this.newPos = newPos;
        this.promotionTo = promotionTo;
    }
}
