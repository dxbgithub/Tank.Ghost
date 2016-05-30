using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
        private int moveDirection = 1;
        private long _lastFireTime;

        public override void Run()
        {
            Init();
            while (true)
            {
                AdjustRadar();
                AdjustGun();
                AdjustMove();
                Execute();
            }
        }

        private void Init()
        {
            _enemy = new Enemy(this);
            IsAdjustRadarForGunTurn = true;
            IsAdjustGunForRobotTurn = true;
            _lastFireTime = Time;
        }

        private void AdjustMove()
        {
            if (Math.Abs(Velocity) < 0.01)
                moveDirection *= -1;

            if (_enemy.None()) return;

            SetTurnRightRadians(Utils.NormalRelativeAngle(_enemy.BearingRadians + HalfPi - 15*moveDirection));
            if (Time%20 == 0)
            {
                moveDirection *= -1;
                SetAhead(1000*moveDirection);
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
            //if (_enemy.Distance < 300 || Time - _lastFireTime > 500)
            if (IsFireOk())
            {
                SetFire(power);
                _lastFireTime = Time;
            }
        }

        private bool IsFireOk()
        {
            return (_enemy.Distance > 300 && Time - _lastFireTime > 100) ||
            (_enemy.Distance <= 300 && _enemy.Distance > 150 && Time - _lastFireTime > 50) ||
            (_enemy.Distance <= 150);
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
            _enemy.Update(evnt);
            
        }


        private static double AbsoluteBearing(double x1, double y1, double x2, double y2)
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
