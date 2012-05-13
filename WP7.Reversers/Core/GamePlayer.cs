using System.Threading;

namespace Reversi
{
  public abstract class GamePlayer
  {
    public Game Game { get; set; }
    public Player Player { get; set; }
    public abstract Position NextMove { get; }
    public virtual EventWaitHandle NextMoveReady { get { return null; } }
  }
}
