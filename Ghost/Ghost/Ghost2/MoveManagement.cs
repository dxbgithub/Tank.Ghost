using System;
using Robocode.Util;

namespace Ghost.Ghost2
{
    public class MoveManagement
    {
        private readonly Ghost2 _ghost2;
        private int _moveDirection = 1;

        public MoveManagement(Ghost2 ghost2)
        {
            _ghost2 = ghost2;
        }

        public void AdjustMove(Enemy enemy)
        {
            if (Math.Abs(_ghost2.Velocity) < 0.01)
                _moveDirection *= -1;

            if (enemy.None()) return;

            _ghost2.SetTurnRightRadians(Utils.NormalRelativeAngle(enemy.BearingRadians + Common.HalfPi - 15* _moveDirection));
            if (_ghost2.Time%20 == 0)
            {
                _moveDirection *= -1;
                _ghost2.SetAhead(1000* _moveDirection);
            }
        }
    }
}