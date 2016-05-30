using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace Ghost.Ghost3
{
    public class Enemy
    {
        private bool _initialized = false;
        private double _bearing;
        private double _energy;
        private double _heading;
        private double _headingRadians;
        private bool _isSentryRobot;
        private string _name;
        private double _velocity;
        public long Time;
        public double X;
        public double Y;
        public double BearingRadians;
        public double Distance;

        private Ghost3 _ghost3;
        private double _margin;

        public Enemy(Ghost3 ghost3)
        {
            _ghost3 = ghost3;
            _margin = Math.Max( _ghost3.Width, _ghost3.Height);
        }

        public void Update(ScannedRobotEvent e)
        {
            _initialized = true;
            _bearing = e.Bearing;
            BearingRadians = e.BearingRadians;
            Distance = e.Distance;
            _energy = e.Energy;
            _heading = e.Heading;
            _headingRadians = e.HeadingRadians;
            _isSentryRobot = e.IsSentryRobot;
            _name = e.Name;
            _velocity = e.Velocity;
            
            X = _ghost3.X + Math.Sin(_ghost3.HeadingRadians + BearingRadians) * Distance;
            Y = _ghost3.Y + Math.Cos(_ghost3.HeadingRadians + BearingRadians) * Distance;
            Time = _ghost3.Time;
        }

        public bool None()
        {
            return !_initialized;
        }

        public double ProporateFirePower()
        {
            return Math.Min(400/Distance, 3);
        }

        public double FutureX(long time)
        {
            var future = X + Math.Sin(_headingRadians) * (_velocity * (time - Time));
            if (future < _margin) future = _margin;
            if (future > _ghost3.BattleFieldWidth - _margin) future = _ghost3.BattleFieldWidth - _margin;

            return future;
        }

        public double FutureY(long time)
        {
            var future = Y + Math.Cos(_headingRadians) * (_velocity * (time - Time));
            if (future < _margin) future = _margin;
            if (future > _ghost3.BattleFieldHeight - _margin) future = _ghost3.BattleFieldHeight - _margin;

            return future;
        }

        public double OccupiedAngle()
        {
            var max = Math.Max(_ghost3.Width, _ghost3.Height);
            return max/Distance;
        }
    }
}
