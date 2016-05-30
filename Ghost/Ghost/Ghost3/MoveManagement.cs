using System;
using Robocode.Util;

namespace Ghost.Ghost3
{
    public class MoveManagement
    {
        private readonly Ghost3 _ghost3;
        private readonly Random _random = new Random();
        private int _moveDirection = 1;
        private double _distanceUnit;

        public MoveManagement(Ghost3 ghost3)
        {
            _ghost3 = ghost3;
            _distanceUnit = Math.Max(_ghost3.Height, _ghost3.Width);
        }

        public void AdjustMove(Enemy enemy)
        {
            if (_ghost3.TurnRemainingRadians > 0.01 || _ghost3.DistanceRemaining > 0.01) return;

            _moveDirection *= -1;

            _ghost3.SetTurnRightRadians(Utils.NormalRelativeAngle(enemy.BearingRadians + Common.HalfPi));
            _ghost3.SetAhead(RandomDistance() * _moveDirection);
            
            //            var newX = Common.MakeSureXValid(_ghost3, _random.NextDouble()*_ghost3.BattleFieldWidth, 2.0);
            //            var newY = Common.MakeSureYValid(_ghost3, _random.NextDouble()*_ghost3.BattleFieldHeight, 2.0);
            //            var diffX = newX - _ghost3.X;
            //            var diffY = newY - _ghost3.Y;
            //            var distance = Math.Sqrt(diffX * diffX + diffY * diffY);
            //
            //            var absRadians = Common.AbsoluteBearingRadians(_ghost3.X, _ghost3.Y, newX, newY);
            //            var relRadians = Utils.NormalRelativeAngle(absRadians - _ghost3.HeadingRadians);
            //            _ghost3.SetTurnRightRadians(relRadians);
            //            _ghost3.SetAhead(distance);

        }

        private double RandomDistance()
        {
            return _distanceUnit/10.0*_random.Next(20, 40);
        }
    }
}