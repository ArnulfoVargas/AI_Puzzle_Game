public struct LevelData
{
    public bool levelSucceed;
    public uint recordMoves;
    public bool[] collectableTaken;
    public bool unlocked;

    public override string ToString() {
        return $"Level succeed: {levelSucceed}; Collectables Taken: [{collectableTaken[0]} {collectableTaken[1]} {collectableTaken[2]}]; Unlocked: {unlocked}";
    }
}
