using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;
using Robocode.Util;

namespace Ghost.Ghost2
{
    public class Ghost2 : AdvancedRobot
    {
        private Enemy _enemy;
        private readonly RadarManagement _radarManagement;
        private readonly GunManagement _gunManagement;
        private readonly MoveManagement _moveManagement;
        
        public override void Run()
        {
            Init();
            while (true)
            {
//                _radarManagement.AdjustRadar(_enemy);
//                _gunManagement.AdjustGun(_enemy);
//                _moveManagement.AdjustMove(_enemy);
                Execute();
            }
        }

        private void Init()
        {
            //_radarManagement = new RadarManagement(this);
//            _gunManagement = new GunManagement(this);
//            _moveManagement = new MoveManagement(this);
//            _enemy = new Enemy(this);
            IsAdjustRadarForGunTurn = true;
            IsAdjustGunForRobotTurn = true;
        }


        public override void OnBulletHit(BulletHitEvent evnt)
        {
            base.OnBulletHit(evnt);
        }

        public override void OnHitByBullet(HitByBulletEvent evnt)
        {
            base.OnHitByBullet(evnt);
        }

        public override void OnHitRobot(HitRobotEvent evnt)
        {
            base.OnHitRobot(evnt);
        }

        public override void OnHitWall(HitWallEvent evnt)
        {
            base.OnHitWall(evnt);
        }

        public override void OnScannedRobot(ScannedRobotEvent evnt)
        {
//            _enemy.Update(evnt);
            
        }
        
        
    }
}
