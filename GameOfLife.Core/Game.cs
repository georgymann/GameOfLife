namespace GameOfLife.Core;

public class Game
{
    public int Rows { get; }
    public int Cols { get; }
    public int Generation { get; private set; }

    private bool[,] _grid;
    private bool[,] _nextGrid;

    public bool[,] Grid => (bool[,])_grid.Clone();

    public Game(int rows = 20, int cols = 30)
    {
        Rows = rows;
        Cols = cols;
        Generation = 1;

        _grid = new bool[Rows, Cols];
        _nextGrid = new bool[Rows, Cols];
    }

    public void NextGeneration()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                int n = CountNeighbours(i, j);

                if (_grid[i, j])
                {
                    _nextGrid[i, j] = n == 2 || n == 3;
                }
                else
                {
                    _nextGrid[i, j] = n == 3;
                }
            }
        }

        Array.Copy(_nextGrid, _grid, _grid.Length);

        Generation++;
    }

    private int CountNeighbours(int row, int col)
    {
        int count = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int trgRow = row + i;
                int trgCol = col + j;

                if (!IsValidCell(trgRow, trgCol))
                    continue;

                if (_grid[trgRow, trgCol])
                    count++;
            }
        }

        return count;
    }

    private bool IsValidCell(int row, int col)
    {
        return row >= 0 &&
               row < Rows &&
               col >= 0 &&
               col < Cols;
    }

    public void Clear()
    {
        Array.Clear(_grid, 0, _grid.Length);

        Generation = 1;
    }

    public void ToggleCell(int row, int col)
    {
        if (!IsValidCell(row, col))
            return;

        _grid[row, col] = !_grid[row, col];
    }
}