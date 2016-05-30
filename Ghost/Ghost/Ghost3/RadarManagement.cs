using System;
using Robocode.Util;

namespace Ghost.Ghost3
{
    public class RadarManagement
    {
        private readonly Ghost3 _ghost3;

        public RadarManagement(Ghost3 ghost3)
        {
            _ghost3 = ghost3;
        }

        public void AdjustRadar(Enemy enemy)
        {
            //if (Math.Abs(RadarTurnRemainingRadians) > 0.01) return;

            if (enemy.None() || _ghost3.Time - enemy.Time > 10L)
            {
                _ghost3.SetTurnRadarRightRadians(Common.TwoPi);
                return;
            }
            
            var absDeg = Common.AbsoluteBearingRadians(_ghost3.X, _ghost3.Y, enemy.X, enemy.Y);
            var relDeg = absDeg - _ghost3.RadarHeadingRadians;
            var enemyDeg = Math.Sign(relDeg)* enemy.OccupiedAngle()*0.5;
//            relDeg += enemyDeg;
            _ghost3.SetTurnRadarRightRadians(Utils.NormalRelativeAngle(relDeg));

        }
    }
}