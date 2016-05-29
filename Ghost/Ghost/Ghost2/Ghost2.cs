using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace Ghost.Ghost2
{
    public class Ghost2 : AdvancedRobot
    {
        private const double Pi = 3.1415926535897931;
        private const double HPi = Pi/2.0;
        private int _scanDirection = 1;
        private Enemy _enemy;

        public override void Run()
        {
            _enemy = new Enemy();
            IsAdjustRadarForGunTurn = true;
//            IsAdjustGunForRobotTurn = true;
            while (true)
            {
                if (Math.Abs(RadarTurnRemaining) < 10)
                    SetTurnRadarRight(360);
//                AdjustGun();
                Execute();
            }
        }

        private void AdjustGun()
        {
            if (!_enemy.None() && GunHeat < 0.01 && Math.Abs(GunTurnRemaining) < 10)
            {
                SetFire(_enemy.ProporateFilePower());
                // calculate gun turn to predicted x,y location
                double futureX = _enemy.FutureX(Time);
                double futureY = _enemy.FutureY(Time);
                double absDeg = AbsoluteBearing(X, Y, futureX, futureY);
                // turn the gun to the predicted x,y location
                SetTurnGunRight(NormalizeBearing(absDeg - GunHeading));
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
//            SetTurnRadarRight(Heading - RadarHeading + evnt.Bearing);
            _scanDirection *= -1;
            SetTurnRadarRight(360*_scanDirection);

            //if (_enemy.None() || evnt.Distance < _enemy.Distance - 70) _enemy.Update(evnt, this);
            
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
            double hyp = Math.Sqrt(xo*xo + yo*yo);
            double arcSin = Math.Asin(xo / hyp) * HPi;
            double bearing = 0;

            if (xo > 0 && yo > 0)
            { // both pos: lower-Left
                bearing = arcSin;
            }
            else if (xo < 0 && yo > 0)
            { // x neg, y pos: lower-right
                bearing = 360 + arcSin; // arcsin is negative here, actuall 360 - ang
            }
            else if (xo > 0 && yo < 0)
            { // x pos, y neg: upper-left
                bearing = 180 - arcSin;
            }
            else if (xo < 0 && yo < 0)
            { // both neg: upper-right
                bearing = 180 - arcSin; // arcsin is negative here, actually 180 + ang
            }

            return bearing;
        }
    }
}
