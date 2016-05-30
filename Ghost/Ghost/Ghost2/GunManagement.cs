using Robocode.Util;

namespace Ghost.Ghost2
{
    public class GunManagement
    {
        private readonly Ghost2 _ghost2;
        private long _lastFireTime;

        public GunManagement(Ghost2 ghost2)
        {
            _ghost2 = ghost2;
        }

        public void AdjustGun(Enemy enemy)
        {
            if (enemy.None()) return;

            var power = enemy.ProporateFirePower();
            var when = _ghost2.Time + (long)(enemy.Distance/ (20.0 - 3.0 * power));
            var futureX = enemy.FutureX(when);
            var futureY = enemy.FutureY(when);
            var absDeg = Common.AbsoluteBearing(_ghost2.X, _ghost2.Y, futureX, futureY);
            _ghost2.SetTurnGunRightRadians(Utils.NormalRelativeAngle(absDeg - _ghost2.GunHeadingRadians));

            if (IsFireOk(enemy))
            {
                _ghost2.SetFire(power);
                _lastFireTime = _ghost2.Time;
            }
        }

        private bool IsFireOk(Enemy enemy)
        {
            return (enemy.Distance > 300 && _ghost2.Time - _lastFireTime > 100) ||
            (enemy.Distance <= 300 && enemy.Distance > 150 && _ghost2.Time - _lastFireTime > 50) ||
            (enemy.Distance <= 150);
        }
    }
}