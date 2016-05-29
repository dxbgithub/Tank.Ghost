using Robocode;

namespace Ghost.GhostA
{
	internal class Santa
	{
		private readonly AdvancedRobot _queen;

		public Santa(AdvancedRobot queen)
		{
			this._queen = queen;
		}

		public void SendGiftTo(Friend friend)
		{
			double num = 600.0 / friend.Distance;
			long when = this._queen.Time + (long)((int)(friend.Distance / (20.0 - 3.0 * num)));
			double num2 = Descartes.Absbearing(this._queen.X, this._queen.Y, friend.GuessX(when), friend.GuessY(when));
			double rad = this._queen.GunHeadingRadians - num2;
			this._queen.SetTurnGunLeftRadians(Descartes.NormaliseBearing(rad));
			this._queen.SetFire(num);
		}
	}
}
