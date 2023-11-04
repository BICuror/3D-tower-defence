public sealed class IslandGridHolder
{
    private BlockGrid _grid;

    public BlockGrid Grid
    {
        set 
        {
            if (_grid == null) _grid = value;
        }

        get => _grid;
    }
}
