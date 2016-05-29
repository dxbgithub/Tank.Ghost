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
        private double _x;
        private double _y;
        private long _time;


        public void Update(ScannedRobotEvent e, Ghost2 robot)
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
            
            _x = robot.X + Math.Sin(robot.HeadingRadians + _bearingRadians) * Distance;
            _y = robot.Y + Math.Cos(robot.HeadingRadians + _bearingRadians) * Distance;
            _time = robot.Time;
        }

        public bool None()
        {
            return !_initialized;
        }

        public double ProporateFilePower()
        {
            return Math.Min(400/Distance, 3);
        }

        public double FutureX(long time)
        {
            return _x + Math.Sin(_headingRadians)*(_velocity*(time - _time));
        }

        public double FutureY(long time)
        {
            return _y + Math.Cos(_headingRadians) * (_velocity * (time - _time));
        }
    }
}
