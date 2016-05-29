﻿using System;
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
        private const double Pi = 3.1415926535897931;
        private const double HalfPi = Pi/2.0;
        private const double TwoPi = 2*Pi;
        private Enemy _enemy;

        public override void Run()
        {
            _enemy = new Enemy();
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
            if (Math.Abs(RadarTurnRemainingRadians) > 0.01) return;

            if (Time - _enemy.Time > 10L)
            {
                SetTurnRadarRightRadians(TwoPi);
                return;
            }
            
            var absDeg = AbsoluteBearing(X, Y, _enemy.X, _enemy.Y);
            var relDeg = absDeg - RadarHeadingRadians;
            var enemyDeg = Math.Sign(relDeg)*TwoPi/100.0;
            relDeg += enemyDeg;
            TurnRadarRightRadians(NormalizeBearing(relDeg));
            //TurnRadarRightRadians(Utils.NormalRelativeAngle(relDeg));

        }

        private void AdjustGun()
        {
            if (!_enemy.None())
            {
                var power = _enemy.ProporateFirePower();
                var when = Time + (long)(_enemy.Distance/ (20.0 - 3.0 * power));
                var futureX = _enemy.FutureX(when);
                var futureY = _enemy.FutureY(when);
                var absDeg = AbsoluteBearing(X, Y, futureX, futureY);
                SetTurnGunRightRadians(NormalizeBearing(absDeg - GunHeadingRadians));
                SetFire(power);
            }
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
            _enemy.Update(evnt, this);
            
        }
        private double NormalizeBearing(double angle)
        {
            while (angle > 180) angle -= 360;
            while (angle < -180) angle += 360;
            return angle;
        }
        private double AbsoluteBearing(double x1, double y1, double x2, double y2)
        {
            double xo = x2 - x1;
            double yo = y2 - y1;
            double hyp = Math.Sqrt(xo * xo + yo * yo);
            double arcSin = Math.Asin(xo / hyp) ;
            double bearing = 0;

            if (xo > 0 && yo > 0)
            { // both pos: lower-Left
                bearing = arcSin;
            }
            else if (xo < 0 && yo > 0)
            { // x neg, y pos: lower-right
                bearing = TwoPi - arcSin; // arcsin is negative here, actuall TwoPi - ang
            }
            else if (xo > 0 && yo < 0)
            { // x pos, y neg: upper-left
                bearing = Pi - arcSin; // Pi - ang
            }
            else if (xo < 0 && yo < 0)
            { // both neg: upper-right
                bearing = Pi + arcSin; // arcsin is negative here, actually Pi + ang
            }

            return bearing;
        }
    }
}
