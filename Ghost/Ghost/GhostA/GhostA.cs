using System;
using Robocode;

namespace Ghost.GhostA
{
	public class GhostA : AdvancedRobot
	{
		private readonly Friend _friend;

		private readonly Dancer _dancer;

		private readonly Hubble _hubble;

		private readonly Santa _santa;

		public GhostA()
		{
			this._santa = new Santa(this);
			this._hubble = new Hubble(this);
			this._dancer = new Dancer(this);
			this._friend = new Friend(this);
		}

		public override void Run()
		{
			this.IsAdjustGunForRobotTurn = true;
			base.IsAdjustRadarForGunTurn = true;
			try
			{
				while (true)
				{
					this._santa.SendGiftTo(this._friend);
					this._hubble.KeepInTouchWith(this._friend);
					this.Execute();
				}
			}
			catch (Exception arg_38_0)
			{
				Console.WriteLine(arg_38_0.Message);
			}
		}

		public override void OnScannedRobot(ScannedRobotEvent evnt)
		{
			this._friend.Update(evnt);
			this._dancer.Surf(evnt);
		}

		public override void OnHitByBullet(HitByBulletEvent evnt)
		{
		}

		public override void OnBulletHit(BulletHitEvent evnt)
		{
			this._dancer.MeetBullet(evnt);
		}

		public override void OnBulletHitBullet(BulletHitBulletEvent evnt)
		{
		}

		public override void OnHitWall(HitWallEvent evnt)
		{
		}

		public override void OnHitRobot(HitRobotEvent evnt)
		{
		}

		public override void OnBulletMissed(BulletMissedEvent evnt)
		{
		}
	}
}
