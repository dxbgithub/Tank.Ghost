using System;
using System.Drawing;
using Robocode;

namespace Ghost.Ghost1
{
    public class Ghost1 : AdvancedRobot
    {
        private const double Pi = 3.1415926535897931;

        private const double TwoPi = 6.2831853071795862;

        private long _lastScanTime;
        private ScanedEnemyInfo _lastScannedEnemyInfo;
        private Position _selfPosition;
        private readonly Random _random = new Random();

        public override void Run()
        {
            Init();
            while (true)
            {
                UpdateSelfPos();
                if (_lastScannedEnemyInfo == null)
                    Scan();
                AdjustRadar();
                //AdjustFire();
                AdjustMovement();
            }
        }

        private void UpdateSelfPos()
        {
            _selfPosition = new Position()
            {
                X = X,
                Y = Y
            };
        }

        private void AdjustFire()
        {
            if (_lastScannedEnemyInfo == null || _lastScanTime != Time) return;

            var fp = CalculateFire();
            SetTurnGunRightRadians(fp.GunPredictRadians - GunHeadingRadians);
            SetFire(fp.Power);
        }

        private FirePrediction CalculateFire()
        {
            var power = GetExpectedPower();
            return new FirePrediction
            {
                Power = power,
                GunPredictRadians = GetRadiansOfPredictEnemy(power)
            };
        }

        private double GetRadiansOfPredictEnemy(double power)
        {
            var predictPos = GetPredictEnemyPosition(power);
            var predictRadians = Math.Asin(Math.Abs(predictPos.X - _selfPosition.X)/_selfPosition.DistanceTo(predictPos));

            if (predictPos.X >= _selfPosition.X && predictPos.Y >= _selfPosition.Y)
                ;
            else if (predictPos.X >= _selfPosition.X && predictPos.Y < _selfPosition.Y) // 90 - 180
                predictRadians = Pi - predictRadians;
            else if (predictPos.X < _selfPosition.X && predictPos.Y <= _selfPosition.Y) // 180 - 270
                predictRadians = Pi + predictRadians;
            else if (predictPos.X < _selfPosition.X && predictPos.Y > _selfPosition.Y) // 270 - 360
                predictRadians = TwoPi - predictRadians;

            return predictRadians;
        }

        private Position GetPredictEnemyPosition(double power)
        {
            var pos = _lastScannedEnemyInfo.Position.Clone();
            if (_lastScannedEnemyInfo.Velocity <= 0.01) return pos;
            
            var bulletSpeed = Rules.GetBulletSpeed(power);
            var enemySpeed = _lastScannedEnemyInfo.Velocity;
            var bulletDistance = 0.0;
            var bulletDistanceTo = _lastScannedEnemyInfo.Distance;
            var time = 0.0;
            
            while (bulletDistance <= bulletDistanceTo)
            {
                time += 1;
                bulletDistance = bulletSpeed*time;
                var enemyDistance = enemySpeed*time;
                pos = new Position
                {
                    X = pos.X + enemyDistance*Math.Sin(_lastScannedEnemyInfo.HeadingRadians),
                    Y = pos.Y + enemyDistance*Math.Cos(_lastScannedEnemyInfo.HeadingRadians)
                };

                if (!pos.IsValid(this)) return pos.MakeValid(this);

                bulletDistanceTo = _selfPosition.DistanceTo(pos);
            }

            return pos;
        }

        private double GetExpectedPower()
        {
            return _lastScannedEnemyInfo.Distance > Math.Min(BattleFieldHeight, BattleFieldWidth)/3.0 ? 3.0 : 1.0;
        }

        private void AdjustMovement()
        {
            if (_lastScannedEnemyInfo == null || _lastScanTime != Time)
            {
                var sign = _random.NextDouble() > 0.5 ? 1 : -1;
                SetAhead(50 + 100*sign);
                return;
            }

            var mp = CalculateMovePlan();
            SetTurnRightRadians(mp.HeadRadians - HeadingRadians);
            SetAhead(mp.Distance);
        }

        private MovePlan CalculateMovePlan()
        {
            var PosTo = GetNewPosition();
            var distance = _selfPosition.DistanceTo(PosTo);
            var planRadians = Math.Asin(Math.Abs(PosTo.X - _selfPosition.X) / _selfPosition.DistanceTo(PosTo));

            if (PosTo.X >= _selfPosition.X && PosTo.Y >= _selfPosition.Y)
                ;
            else if (PosTo.X >= _selfPosition.X && PosTo.Y < _selfPosition.Y) // 90 - 180
                planRadians = Pi - planRadians;
            else if (PosTo.X < _selfPosition.X && PosTo.Y <= _selfPosition.Y) // 180 - 270
                planRadians = Pi + planRadians;
            else if (PosTo.X < _selfPosition.X && PosTo.Y > _selfPosition.Y) // 270 - 360
                planRadians = TwoPi - planRadians;

            return new MovePlan
            {
                Distance = distance,
                HeadRadians = planRadians
            };
        }

        private Position GetNewPosition()
        {
            var pos = new  Position
            {
                X = BattleFieldWidth / 2.0,
                Y = BattleFieldHeight / 2.0
            };

            while (pos.DistanceTo(_lastScannedEnemyInfo.Position) <= Math.Min(BattleFieldHeight, BattleFieldWidth)/2.0)
            {
                pos.X = _random.NextDouble()*BattleFieldWidth;
                pos.Y = _random.NextDouble()*BattleFieldHeight;
                if (!pos.IsValid(this)) pos.MakeValid(this);
            }

            return pos;
        }

        private void AdjustRadar()
        {
            if (_lastScannedEnemyInfo == null || _lastScanTime != Time)
            {
                SetTurnRadarRight(TwoPi);
                return;
            }

            var radians = HeadingRadians - _lastScannedEnemyInfo.BearingRadians + RadarHeadingRadians;
            if (radians < -Pi)
            {
                radians += TwoPi;
            }
            if (radians > Pi)
            {
                radians -= TwoPi;
            }
            SetTurnRadarRightRadians(radians);
        }

        private void Init()
        {
            SetColors(Color.Black, Color.White, Color.Black);
            IsAdjustRadarForGunTurn = true;
            IsAdjustGunForRobotTurn = true;
            IsAdjustRadarForRobotTurn = true;
        }

        public override void OnScannedRobot(ScannedRobotEvent evnt)
        {
            _lastScanTime = Time;
            _lastScannedEnemyInfo = CreateScanedEnemyInfo(evnt);
            
        }

        private ScanedEnemyInfo CreateScanedEnemyInfo(ScannedRobotEvent evnt)
        {
            var angle = HeadingRadians + evnt.BearingRadians;
            var pos = new Position {X = X + evnt.Distance*Math.Sin(angle), Y = Y + evnt.Distance*Math.Cos(angle)};
            return new ScanedEnemyInfo
            {
                Bearing = evnt.Bearing,
                BearingRadians = evnt.BearingRadians,
                Distance = evnt.Distance,
                Energy = evnt.Energy,
                Heading = evnt.Heading,
                HeadingRadians = evnt.HeadingRadians,
                IsSentryRobot = evnt.IsSentryRobot,
                Name = evnt.Name,
                Velocity = evnt.Velocity,
                Position = pos
            };
        }

        public override bool IsAdjustGunForRobotTurn { get; set; }
    }
}
