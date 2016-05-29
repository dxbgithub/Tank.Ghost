using System;
using Robocode;

namespace Ghost.GhostA
{
	internal class Friend
	{
		private readonly AdvancedRobot _queen;

		public double X
		{
			get;
			set;
		}

		public double Y
		{
			get;
			set;
		}

		public double BearingRadians
		{
			get;
			set;
		}

		public double HeadingRadians
		{
			get;
			set;
		}

		public double RadarHeadingRadians
		{
			get;
			set;
		}

		public double Velocity
		{
			get;
			set;
		}

		public double Distance
		{
			get;
			set;
		}

		public long Time
		{
			get;
			set;
		}

		public Friend(AdvancedRobot queen)
		{
			this._queen = queen;
		}

		public void Update(ScannedRobotEvent e)
		{
			double num = (this._queen.HeadingRadians + e.BearingRadians) % 6.2831853071795862;
			this.X = this._queen.X + Math.Sin(num) * e.Distance;
			this.Y = this._queen.Y + Math.Cos(num) * e.Distance;
			this.BearingRadians = e.BearingRadians;
			this.HeadingRadians = e.HeadingRadians;
			this.Time = e.Time;
			this.Velocity = e.Velocity;
			this.Distance = e.Distance;
		}

		public double GuessX(long when)
		{
			long num = when - this.Time;
			return this.X + Math.Sin(this.HeadingRadians) * this.Velocity * (double)num;
		}

		public double GuessY(long when)
		{
			long num = when - this.Time;
			return this.Y + Math.Cos(this.HeadingRadians) * this.Velocity * (double)num;
		}
	}
}
