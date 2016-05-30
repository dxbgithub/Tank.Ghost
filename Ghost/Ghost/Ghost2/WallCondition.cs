using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace Ghost.Ghost2
{
    internal class WallCondition : Condition
    {
        private Ghost2 _ghost2;
        private double _wallMargin;
        
        public WallCondition(string name, Ghost2 ghost2) : base(name)
        {
            _ghost2 = ghost2;
            _wallMargin = Math.Max(_ghost2.Width, _ghost2.Height);
        }

        public override bool Test()
        {
            return (
                _ghost2.X <= _wallMargin ||
                _ghost2.X >= _ghost2.BattleFieldWidth - _wallMargin ||
                _ghost2.Y <= _wallMargin ||
                _ghost2.Y >= _ghost2.BattleFieldHeight - _wallMargin
                );
        }
    }
}
