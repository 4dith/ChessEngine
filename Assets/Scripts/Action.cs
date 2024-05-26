using System;
using System.Collections.Generic;

public class Action
{
    public Tuple<int, int> initialPos;
    public Tuple<int, int> newPos;
    
    public Action(Tuple<int, int> initialPos, Tuple<int, int> newPos) {
        this.initialPos = initialPos;
        this.newPos = newPos;
    }
}
