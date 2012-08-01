using System;
using System.Linq;

namespace Reversi
{
  public class GameAISimple : GamePlayer
  {
    static Random _soul = new Random();

    /// <summary>
    /// Calculates best move using some "witty" logic
    /// Вычисляем лучший ход - логика достаточно примитивна 
    /// </summary>
    Position CalcBestMoveFor()
    {
      var moves = from c in Game.GetFreeCells()
                  let score = Game.CheckScoreFor(c, Player)
                  where score > 0
                  group c by score;

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
