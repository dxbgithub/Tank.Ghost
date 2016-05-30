using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;
using Robocode.Util;

namespace Ghost.Ghost2
{
    public class Ghost2 : AdvancedRobot
    {
        private const double Pi = Math.PI;
        private const double HalfPi = Pi/2.0;
        private const double TwoPi = 2*Pi;
        private Enemy _enemy;

        public override void Run()
        {
            _enemy = new Enemy(this);
            IsAdjustRadarForGunTurn = true;
            IsAdjustGunForRobotTurn = true;
            while (true)
            {
                AdjustRadar();
                AdjustGun();
                Execute();
            }
        }

        private void AdjustRadar()
        {
            //if (Math.Abs(RadarTurnRemainingRadians) > 0.01) return;

            if (_enemy.None() || Time - _enemy.Time > 10L)
            {
                SetTurnRadarRightRadians(TwoPi);
                return;
            }
            
            var absDeg = AbsoluteBearing(X, Y, _enemy.X, _enemy.Y);
            var relDeg = absDeg - RadarHeadingRadians;
            var enemyDeg = Math.Sign(relDeg)* _enemy.OccupiedAngle()*1.5;
            relDeg += enemyDeg;
            SetTurnRadarRightRadians(Utils.NormalRelativeAngle(relDeg));

        }

        private void AdjustGun()
        {
            if (_enemy.None()) return;

            var power = _enemy.ProporateFirePower();
            var when = Time + (long)(_enemy.Distance/ (20.0 - 3.0 * power));
            var futureX = _enemy.FutureX(when);
            var futureY = _enemy.FutureY(when);
            var absDeg = AbsoluteBearing(X, Y, futureX, futureY);
            SetTurnGunRightRadians(Utils.NormalRelativeAngle(absDeg - GunHeadingRadians));
            //SetFire(power);
        }

        public override void OnBulletHit(BulletHitEvent evnt)
        {
            base.OnBulletHit(evnt);
        }

        public override void OnHitByBullet(HitByBulletEvent evnt)
        {
            base.OnHitByBullet(evnt);
        }

        public override void OnHitRobot(HitRobotEvent evnt)
        {
            base.OnHitRobot(evnt);
        }

        public override void OnHitWall(HitWallEvent evnt)
        {
            base.OnHitWall(evnt);
        }

        public override void OnScannedRobot(ScannedRobotEvent evnt)
        {

            //            if (_enemy.None() || evnt.Distance < _enemy.Distance - 70 || !_enemy.IsFired())
            _enemy.Update(evnt);
            
        }


        private double AbsoluteBearing(double x1, double y1, double x2, double y2)
        {
            var xo = x2 - x1;
            var yo = y2 - y1;
            var hyp = Math.Sqrt(xo * xo + yo * yo);
            var arcSin = Math.Asin(xo / hyp) ;
            double bearing = 0;

            if (xo > 0 && yo > 0)
            { // both pos: lower-Left
                bearing = arcSin;
            }
            else if (xo < 0 && yo > 0)
            { // x neg, y pos: lower-right
                bearing = TwoPi + arcSin;
            }
            else if (xo > 0 && yo < 0)
            { // x pos, y neg: upper-left
                bearing = Pi - arcSin; // Pi - ang
            }
            else if (xo < 0 && yo < 0)
            { // both neg: upper-right
                bearing = Pi - arcSin; 
            }

            return bearing;
        }
    }
}
