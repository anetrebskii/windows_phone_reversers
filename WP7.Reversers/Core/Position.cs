using System.Diagnostics;

namespace Reversi
{
  [DebuggerDisplay("{X}:{Y}")]
  public struct Position
  {
    public int X, Y;

    public Position(int x, int y)
    {
      X = x;
      Y = y;
    }

    public bool Valid()
    {
      return X >= 0 && Y >= 0 && X < 8 && Y < 8;
    }

    public void Offset(int offsetX, int offsetY)
    {
      X += offsetX;
      Y += offsetY;
    }

    public static readonly Position Invalid = new Position(-1, -1);
  }
}
