using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Robocode;
using Robocode.Util;

namespace Ghost.GhostA
{
	internal class Dancer
	{
		public static int Bins = 47;

		public static double[] SurfStats = new double[Dancer.Bins];

		public Location MyLocation;

		public Location FriendLocation;

		private readonly List<FriendWave> _friendWaves = new List<FriendWave>();

		private readonly List<int> _surfDirections = new List<int>();

		private readonly List<double> _surfAbsBearings = new List<double>();

		private static double _oppEenergy = 100.0;

		private static Rectangle _filedRect = new Rectangle(18, 18, 764, 564);

		private static double WallStick = 160.0;

		private readonly AdvancedRobot _queen;

		public Dancer(AdvancedRobot queen)
		{
			this._queen = queen;
		}

		public double WallSmoothing(Location botLocation, double angle, int orientation)
		{
			while (!Dancer._filedRect.Contains(Dancer.Project(botLocation, angle, Dancer.WallStick).ToPoint()))
			{
				angle += (double)orientation * 0.05;
			}
			return angle;
		}

		public static Location Project(Location sourceLocation, double angle, double length)
		{
			return new Location(sourceLocation.X + Math.Sin(angle) * length, sourceLocation.Y + Math.Cos(angle) * length);
		}

		public static double AbsoluteBearing(Location source, Location target)
		{
			return Math.Atan2(target.X - source.X, target.Y - source.Y);
		}

		public static double Limit(double min, double value, double max)
		{
			return Math.Max(min, Math.Min(value, max));
		}

		public static double BulletVelocity(double power)
		{
			return 20.0 - 3.0 * power;
		}

		public static double MaxEscapeAngle(double velocity)
		{
			return Math.Asin(8.0 / velocity);
		}

		public static void SetBackAsFront(AdvancedRobot robot, double goAngle)
		{
			double num = Utils.NormalRelativeAngle(goAngle - robot.HeadingRadians);
			if (Math.Abs(num) > 1.5707963267948966)
			{
				if (num < 0.0)
				{
					robot.SetTurnRightRadians(3.1415926535897931 + num);
				}
				else
				{
					robot.SetTurnRightRadians(3.1415926535897931 - num);
				}
				robot.SetBack(100.0);
				return;
			}
			if (num < 0.0)
			{
				robot.SetTurnLeftRadians(-1.0 * num);
			}
			else
			{
				robot.SetTurnRightRadians(num);
			}
			robot.SetAhead(100.0);
		}

		public void Surf(ScannedRobotEvent evt)
		{
			this.MyLocation = new Location(this._queen.X, this._queen.Y);
			double num = this._queen.Velocity * Math.Sin(evt.BearingRadians);
			double num2 = evt.BearingRadians + this._queen.HeadingRadians;
			this._surfDirections.Insert(0, (num >= 0.0) ? 1 : -1);
			this._surfAbsBearings.Insert(0, num2 + 3.1415926535897931);
			double num3 = Dancer._oppEenergy - evt.Energy;
			if (num3 < 3.01 && num3 > 0.09 && this._surfDirections.Count<int>() > 2)
			{
				FriendWave friendWave = new FriendWave();
				friendWave.FireTime = this._queen.Time - 1L;
				friendWave.BulletVelocity = Dancer.BulletVelocity(num3);
				friendWave.DistanceTraveled = Dancer.BulletVelocity(num3);
				friendWave.Direction = this._surfDirections[2];
				friendWave.DirectAngle = this._surfAbsBearings[2];
				friendWave.FireLocation = new Location(this.FriendLocation);
				this._friendWaves.Add(friendWave);
			}
			Dancer._oppEenergy = evt.Energy;
			this.FriendLocation = Dancer.Project(this.MyLocation, num2, evt.Distance);
			this.UpdateWaves();
			this.DoSurfing();
		}

		public void UpdateWaves()
		{
			for (int i = 0; i < this._friendWaves.Count; i++)
			{
				FriendWave friendWave = this._friendWaves[i];
				friendWave.DistanceTraveled = (double)(this._queen.Time - friendWave.FireTime) * friendWave.BulletVelocity;
				if (friendWave.DistanceTraveled > this.MyLocation.Distance(friendWave.FireLocation) + 50.0)
				{
					this._friendWaves.RemoveAt(i);
					i--;
				}
			}
		}

		public void DoSurfing()
		{
			FriendWave closestSurfableWave = this.GetClosestSurfableWave();
			if (closestSurfableWave == null)
			{
				return;
			}
			double arg_2F_0 = this.CheckDanger(closestSurfableWave, -1);
			double num = this.CheckDanger(closestSurfableWave, 1);
			double num2 = Dancer.AbsoluteBearing(closestSurfableWave.FireLocation, this.MyLocation);
			if (arg_2F_0 < num)
			{
				num2 = this.WallSmoothing(this.MyLocation, num2 - 1.5707963267948966, -1);
			}
			else
			{
				num2 = this.WallSmoothing(this.MyLocation, num2 + 1.5707963267948966, 1);
			}
			Dancer.SetBackAsFront(this._queen, num2);
		}

		public FriendWave GetClosestSurfableWave()
		{
			double num = 50000.0;
			FriendWave result = null;
			for (int i = 0; i < this._friendWaves.Count; i++)
			{
				FriendWave friendWave = this._friendWaves[i];
				double num2 = this.MyLocation.Distance(friendWave.FireLocation) - friendWave.DistanceTraveled;
				if (num2 > friendWave.BulletVelocity && num2 < num)
				{
					result = friendWave;
					num = num2;
				}
			}
			return result;
		}

		public int GetFactorIndex(FriendWave ew, Location targetLocation)
		{
			double num = Utils.NormalRelativeAngle(Dancer.AbsoluteBearing(ew.FireLocation, targetLocation) - ew.DirectAngle) / Dancer.MaxEscapeAngle(ew.BulletVelocity) * (double)ew.Direction;
			return (int)Dancer.Limit(0.0, num * (double)((Dancer.Bins - 1) / 2) + (double)((Dancer.Bins - 1) / 2), (double)(Dancer.Bins - 1));
		}

		public void LogHit(FriendWave ew, Location targetLocation)
		{
			int i = this.GetFactorIndex(ew, targetLocation);
			int num = 0;
			while (i < Dancer.Bins)
			{
				Dancer.SurfStats[num] += 1.0 / (Math.Pow((double)(i - num), 2.0) + 1.0);
				num++;
			}
		}

		public void MeetBullet(BulletHitEvent evt)
		{
			if (this._friendWaves.Any<FriendWave>())
			{
				Location targetLocation = new Location(evt.Bullet.X, evt.Bullet.Y);
				FriendWave friendWave = null;
				for (int i = 0; i < this._friendWaves.Count; i++)
				{
					FriendWave friendWave2 = this._friendWaves[i];
					if (Math.Abs(friendWave2.DistanceTraveled - this.MyLocation.Distance(friendWave2.FireLocation)) < 50.0 && Math.Abs(Dancer.BulletVelocity(evt.Bullet.Power) - friendWave2.BulletVelocity) < 0.001)
					{
						friendWave = friendWave2;
						break;
					}
				}
				if (friendWave != null)
				{
					this.LogHit(friendWave, targetLocation);
					this._friendWaves.RemoveAt(this._friendWaves.LastIndexOf(friendWave));
				}
			}
		}

		public Location PredictPosition(FriendWave surfWave, int direction)
		{
			Location location = new Location(this.MyLocation);
			double num = this._queen.Velocity;
			double num2 = this._queen.HeadingRadians;
			int num3 = 0;
			bool flag = false;
			do
			{
				double num4 = this.WallSmoothing(location, Dancer.AbsoluteBearing(surfWave.FireLocation, location) + (double)direction * 1.5707963267948966, direction) - num2;
				double num5 = 1.0;
				if (Math.Cos(num4) < 0.0)
				{
					num4 += 3.1415926535897931;
					num5 = -1.0;
				}
				num4 = Utils.NormalRelativeAngle(num4);
				double num6 = 0.0043633231299858239 * (40.0 - 3.0 * Math.Abs(num));
				num2 = Utils.NormalRelativeAngle(num2 + Dancer.Limit(-num6, num4, num6));
				double expr_C8 = num;
				num = expr_C8 + ((expr_C8 * num5 < 0.0) ? (2.0 * num5) : num5);
				num = Dancer.Limit(-8.0, num, 8.0);
				location = Dancer.Project(location, num2, num);
				num3++;
				if (location.Distance(surfWave.FireLocation) < surfWave.DistanceTraveled + (double)num3 * surfWave.BulletVelocity + surfWave.BulletVelocity)
				{
					flag = true;
				}
			}
			while (!flag && num3 < 500);
			return location;
		}

		public double CheckDanger(FriendWave surfWave, int direction)
		{
			int factorIndex = this.GetFactorIndex(surfWave, this.PredictPosition(surfWave, direction));
			return Dancer.SurfStats[factorIndex];
		}
	}
}
