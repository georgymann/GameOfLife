using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using GameOfLife.Core;

namespace GameOfLife;

public partial class MainWindow : Window
{
    private Game game;
    private Border[,] cells;
    private DispatcherTimer timer;

    public MainWindow()
    {
        InitializeComponent();

        game = new Game();

        cells = new Border[game.Rows, game.Cols];

        CreateGameGrid();

        UpdateVisual();

        timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(200)
        };

        timer.Tick += TimerTick;
    }

    private void TimerTick(object? sender, EventArgs e)
    {
        game.NextGeneration();

        tbDebug.Text = $"Generation: {game.Generation}";

        UpdateVisual();
    }

    private void CreateGameGrid()
    {
        gameGrid.Children.Clear();

        for (int i = 0; i < game.Rows; i++)
        {
            for (int j = 0; j < game.Cols; j++)
            {
                Border border = new Border
                {
                    Background = Brushes.Black,
                    BorderBrush = Brushes.DimGray,
                    BorderThickness = new Thickness(0.3)
                };

                int iCaptured = i;
                int jCaptured = j;

                border.PointerPressed += (o, e) =>
                {
                    CellClick(iCaptured, jCaptured);
                };

                cells[i, j] = border;

                gameGrid.Children.Add(border);
            }
        }
    }

    private void UpdateVisual()
    {
        for (int i = 0; i < game.Rows; i++)
        {
            for (int j = 0; j < game.Cols; j++)
            {
                if (game.Grid[i, j])
                    cells[i, j].Background = Brushes.Yellow;
                else
                    cells[i, j].Background = Brushes.Black;
            }
        }
    }

    private void CellClick(int row, int col)
    {
        game.ToggleCell(row, col);

        UpdateVisual();
    }

    private void StartClick(object? sender, RoutedEventArgs e)
    {
        timer.Start();
    }

    private void StopClick(object? sender, RoutedEventArgs e)
    {
        timer.Stop();
    }

    private void ResetStopButtonClick(object? sender, RoutedEventArgs e)
    {
        timer.Stop();

        game.Clear();

        tbDebug.Text = $"Generation: {game.Generation}";

        UpdateVisual();
    }
}