public static class InputDirectionStorage
{
    private static float _savedDirection;
    private static float _currentDirection;

    public static float LastNonZeroDirection { get => _savedDirection; }
    public static float CurrentDirection { get => _currentDirection; }

    public static void StoreLastNonZeroDirection(float direction)
    {
        _currentDirection = direction;

        if (_currentDirection != 0)
            _savedDirection = _currentDirection;
    }
}
