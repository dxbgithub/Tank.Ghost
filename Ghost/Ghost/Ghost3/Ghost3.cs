using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace Ghost.Ghost3
{
    public class Ghost3 : AdvancedRobot
    {
        private Enemy _enemy;
        private RadarManagement _radarManagement;
        private GunManagement _gunManagement;
        private MoveManagement _moveManagement;

        public override void Run()
        {
            Init();
            while (true)
            {
                _radarManagement.AdjustRadar(_enemy);
                _gunManagement.AdjustGun(_enemy);
                _moveManagement.AdjustMove(_enemy);
                Execute();
            }
        }

        private void Init()
        {
            _enemy = new Enemy(this);
            _radarManagement = new RadarManagement(this);
            _gunManagement = new GunManagement(this);
            _moveManagement = new MoveManagement(this);
        }

        public override void OnScannedRobot(ScannedRobotEvent evnt)
        {
            _enemy.Update(evnt);
        }
    }
}
