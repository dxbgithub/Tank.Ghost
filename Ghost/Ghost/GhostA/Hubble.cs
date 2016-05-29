using System;
using Robocode;

namespace Ghost.GhostA
{
	internal class Hubble
	{
		private readonly AdvancedRobot _queen;

		private const double ScanRange = 0.062831853071795868;

		public Hubble(AdvancedRobot queen)
		{
			this._queen = queen;
		}

		public void KeepInTouchWith(Friend friend)
		{
			if (Math.Abs(this._queen.RadarTurnRemainingRadians) > 0.01)
			{
				return;
			}
			if (this._queen.Time - friend.Time > 10L)
			{
				this._queen.SetTurnRadarRightRadians(6.2831853071795862);
				return;
			}
			double num = Descartes.Absbearing(this._queen.X, this._queen.Y, friend.X, friend.Y);
			double num2 = this._queen.RadarHeadingRadians - num;
			num2 += (double)Math.Sign(num2) * 0.062831853071795868;
			this._queen.SetTurnRadarLeftRadians(Descartes.NormaliseBearing(num2));
		}
	}
}
