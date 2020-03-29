public static class InputDirectionHandler
{
    private static float _lastDirection;
    private static float _currentDirection;

    public static float LastDirection { get => _lastDirection; }
    public static float CurrentDirection { get => _currentDirection; }

    public static void StoreLastDirection(float direction)
    {
        _currentDirection = direction;

        if (_currentDirection != 0)
            _lastDirection = _currentDirection;
    }


}
