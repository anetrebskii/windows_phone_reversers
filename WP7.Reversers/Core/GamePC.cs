using System.Threading;

namespace Reversi
{
  public class GamePC : GamePlayer
  {
    AutoResetEvent _event = new AutoResetEvent(false);

    public void UserMove(Position position)
    {
      _nextMove = position;
      _event.Set();
    }

    #region NextMove Property

    Position _nextMove = Position.Invalid;
    public override Position NextMove { get { return _nextMove; } }

    #endregion

    public override EventWaitHandle NextMoveReady { get { return _event; } }
  }
}
