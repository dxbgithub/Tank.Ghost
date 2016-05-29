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
            
            X = robot.X + Math.Sin(robot.HeadingRadians + _bearingRadians) * Distance;
            Y = robot.Y + Math.Cos(robot.HeadingRadians + _bearingRadians) * Distance;
            Time = robot.Time;
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
    }
}
