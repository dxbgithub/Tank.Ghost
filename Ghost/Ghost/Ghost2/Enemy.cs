using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace Ghost.Ghost2
{
    internal class Enemy
    {
        private bool _initialized = false;
        private double _bearing;
        private double _bearingRadians;
        public double Distance;
        private double _energy;
        private double _heading;
        private double _headingRadians;
        private bool _isSentryRobot;
        private string _name;
        private double _velocity;
        public long Time { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        private Ghost2 _ghost2;

        public Enemy(Ghost2 ghost2)
        {
            _ghost2 = ghost2;
        }

        public void Update(ScannedRobotEvent e)
        {
            _initialized = true;
            _bearing = e.Bearing;
            _bearingRadians = e.BearingRadians;
            Distance = e.Distance;
            _energy = e.Energy;
            _heading = e.Heading;
            _headingRadians = e.HeadingRadians;
            _isSentryRobot = e.IsSentryRobot;
            _name = e.Name;
            _velocity = e.Velocity;
            
            X = _ghost2.X + Math.Sin(_ghost2.HeadingRadians + _bearingRadians) * Distance;
            Y = _ghost2.Y + Math.Cos(_ghost2.HeadingRadians + _bearingRadians) * Distance;
            Time = _ghost2.Time;
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
            return X + Math.Sin(_headingRadians) * (_velocity * (time - Time));
        }

        public double FutureY(long time)
        {
            return Y + Math.Cos(_headingRadians) * (_velocity * (time - Time));
        }

        public double OccupiedAngle()
        {
            var max = Math.Max(_ghost2.Width, _ghost2.Height);
            return max/Distance;
        }
    }
}
