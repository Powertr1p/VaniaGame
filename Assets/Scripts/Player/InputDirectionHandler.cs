﻿public static class InputDirectionHandler
{
    private static float _savedDirection;
    private static float _currentDirection;

    public static float SavedDirection { get => _savedDirection; }
    public static float CurrentDirection { get => _currentDirection; }

    public static void StoreLastDirection(float direction)
    {
        _currentDirection = direction;

        if (_currentDirection != 0)
            _savedDirection = _currentDirection;
    }


}
