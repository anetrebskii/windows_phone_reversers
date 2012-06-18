using System;
using System.Linq;

namespace Reversi
{
    public class GameAIHard: GamePlayer
    {
        static int[,] _costs = new int[8, 8] 
    {
      {1, 8, 2, 2, 2, 2, 8, 1},
      {8, 8, 6, 5, 5, 6, 8, 8},
      {2, 6, 4, 3, 3, 4, 6, 2},
      {2, 5, 3, 1, 1, 3, 5, 2},
      {2, 5, 3, 1, 1, 3, 5, 2},
      {2, 6, 4, 3, 3, 4, 6, 2},
      {8, 8, 6, 5, 5, 6, 8, 8},
      {1, 8, 2, 2, 2, 2, 8, 1}
    };

    static Random _soul = new Random();

    /// <summary>
    /// Calculates best move using some "witty" logic
    /// Вычисляем лучший ход - логика достаточно примитивна // ДА НУ УЖ КАК ЖЕ.
    /// </summary>
    Position CalcBestMoveFor()
    {
      var moves = from c in Game.GetFreeCells()
                  let score = Game.CheckScoreFor(c, Player)
                  where score > 0
                  group c by 100 * _costs[c.X, c.Y] - score;

      var bestMoves = (from m in moves orderby m.Key select m).
                       First().
                       ToList();

      switch (bestMoves.Count)
      {
        case 0:
          return Position.Invalid;

        case 1:
          return bestMoves[0];

        default:
          return bestMoves[_soul.Next(bestMoves.Count)];
      }
    }

    public override Position NextMove { get { return CalcBestMoveFor(); } }
  }
 }
