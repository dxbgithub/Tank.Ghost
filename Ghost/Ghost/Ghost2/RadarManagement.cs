using System;
using Robocode.Util;

namespace Ghost.Ghost2
{
    public class RadarManagement
    {
        private readonly Ghost2 _ghost2;

        public RadarManagement(Ghost2 ghost2)
        {
            _ghost2 = ghost2;
        }

        public void AdjustRadar(Enemy enemy)
        {
            //if (Math.Abs(RadarTurnRemainingRadians) > 0.01) return;

            if (enemy.None() || _ghost2.Time - enemy.Time > 10L)
            {
                _ghost2.SetTurnRadarRightRadians(Common.TwoPi);
                return;
            }
            
            var absDeg = Common.AbsoluteBearing(_ghost2.X, _ghost2.Y, enemy.X, enemy.Y);
            var relDeg = absDeg - _ghost2.RadarHeadingRadians;
            var enemyDeg = Math.Sign(relDeg)* enemy.OccupiedAngle()*1.5;
            relDeg += enemyDeg;
            _ghost2.SetTurnRadarRightRadians(Utils.NormalRelativeAngle(relDeg));

        }
    }
}