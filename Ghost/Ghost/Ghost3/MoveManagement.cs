using System;
using Robocode.Util;

namespace Ghost.Ghost3
{
    public class MoveManagement
    {
        private readonly Ghost3 _ghost3;
        private int _moveDirection = 1;

        public MoveManagement(Ghost3 ghost3)
        {
            _ghost3 = ghost3;
        }

        public void AdjustMove(Enemy enemy)
        {
            if (Math.Abs(_ghost3.Velocity) < 0.01)
                _moveDirection *= -1;

            if (enemy.None()) return;

            _ghost3.SetTurnRightRadians(Utils.NormalRelativeAngle(enemy.BearingRadians + Common.HalfPi - 15* _moveDirection));
            if (_ghost3.Time%20 == 0)
            {
                _moveDirection *= -1;
                _ghost3.SetAhead(1000* _moveDirection);
            }
        }
    }
}