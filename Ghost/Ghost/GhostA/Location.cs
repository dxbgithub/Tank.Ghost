using System;
using System.Drawing;

namespace Ghost.GhostA
{
	internal class Location
	{
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

		public Location(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		public Location(Location friendLocation)
		{
			this.X = friendLocation.X;
			this.Y = friendLocation.Y;
		}

		public Point ToPoint()
		{
			return new Point((int)this.X, (int)this.Y);
		}

		public double Distance(Location fireLocation)
		{
			return Math.Sqrt(Math.Pow(fireLocation.Y - this.Y, 2.0) + Math.Pow(fireLocation.X - this.X, 2.0));
		}
	}
}
