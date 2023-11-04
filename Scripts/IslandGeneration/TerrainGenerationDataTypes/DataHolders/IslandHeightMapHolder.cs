public sealed class IslandHeightMapHolder
{
    private int[,] _heightMap;

    public int[,] Map
    {
        set 
        {
            if (_heightMap == null) _heightMap = value;
        }

        get => _heightMap;
    }
}
