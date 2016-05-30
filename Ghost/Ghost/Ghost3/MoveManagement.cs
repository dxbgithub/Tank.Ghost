using System;
using Robocode.Util;

namespace Ghost.Ghost3
{
    public class MoveManagement
    {
        private readonly Ghost3 _ghost3;
        private readonly Random _random = new Random();

        public MoveManagement(Ghost3 ghost3)
        {
            _ghost3 = ghost3;
        }

        public void AdjustMove(Enemy enemy)
        {
            var newX = Common.MakeSureXValid(_ghost3, _random.NextDouble()*_ghost3.BattleFieldWidth, 2.0);
            var newY = Common.MakeSureYValid(_ghost3, _random.NextDouble()*_ghost3.BattleFieldHeight, 2.0);
            var diffX = newX - _ghost3.X;
            var diffY = newY - _ghost3.Y;
            var distance = Math.Sqrt(diffX * diffX + diffY * diffY);

            var absRadians = Common.AbsoluteBearingRadians(_ghost3.X, _ghost3.Y, newX, newY);
            var relRadians = Utils.NormalRelativeAngle(absRadians - _ghost3.HeadingRadians);
            _ghost3.SetTurnRightRadians(relRadians);
            _ghost3.SetAhead(distance);

        }
    }
}