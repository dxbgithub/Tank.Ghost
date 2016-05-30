using Robocode.Util;

namespace Ghost.Ghost3
{
    public class GunManagement
    {
        private readonly Ghost3 _ghost3;
        private long _lastFireTime;

        public GunManagement(Ghost3 ghost3)
        {
            _ghost3 = ghost3;
        }

        public void AdjustGun(Enemy enemy)
        {
            if (enemy.None()) return;

            var power = enemy.ProporateFirePower();
            var when = _ghost3.Time + (long)(enemy.Distance/ (20.0 - 3.0 * power));
            var futureX = enemy.FutureX(when);
            var futureY = enemy.FutureY(when);
            var absDeg = Common.AbsoluteBearing(_ghost3.X, _ghost3.Y, futureX, futureY);
            _ghost3.SetTurnGunRightRadians(Utils.NormalRelativeAngle(absDeg - _ghost3.GunHeadingRadians));

            if (IsFireOk(enemy))
            {
                _ghost3.SetFire(power);
                _lastFireTime = _ghost3.Time;
            }
        }

        private bool IsFireOk(Enemy enemy)
        {
            return (enemy.Distance > 300 && _ghost3.Time - _lastFireTime > 100) ||
            (enemy.Distance <= 300 && enemy.Distance > 150 && _ghost3.Time - _lastFireTime > 50) ||
            (enemy.Distance <= 150);
        }
    }
}