using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public struct LevelData
{
    public bool levelSucceed;
    public uint recordMoves;
    public bool[] collectableTaken;
    public bool unlocked;
}
