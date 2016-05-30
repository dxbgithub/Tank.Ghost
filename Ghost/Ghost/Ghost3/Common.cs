using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghost.Ghost3
{
    public class Common
    {
        internal const double Pi = Math.PI;
        internal const double HalfPi = Pi / 2.0;
        internal const double TwoPi = 2 * Pi;
        public static double AbsoluteBearingRadians(double x1, double y1, double x2, double y2)
        {
            var xo = x2 - x1;
            var yo = y2 - y1;
            var hyp = Math.Sqrt(xo * xo + yo * yo);
            var arcSin = Math.Asin(xo / hyp);
            double bearing = 0;

            if (xo > 0 && yo > 0)
            { // both pos: lower-Left
                bearing = arcSin;
            }
            else if (xo < 0 && yo > 0)
            { // x neg, y pos: lower-right
                bearing = TwoPi + arcSin;
            }
            else if (xo > 0 && yo < 0)
            { // x pos, y neg: upper-left
                bearing = Pi - arcSin; // Pi - ang
            }
            else if (xo < 0 && yo < 0)
            { // both neg: upper-right
                bearing = Pi - arcSin;
            }

            return bearing;
        }

        public static double MakeSureXValid(Ghost3 ghost3, double future, double ratio = 1.0)
        {
            var margin = Math.Max(ghost3.Width, ghost3.Height) * ratio;
            if (future < margin) future = margin;
            if (future > ghost3.BattleFieldWidth - margin) future = ghost3.BattleFieldWidth - margin;
            return future;
        }

        public static double MakeSureYValid(Ghost3 ghost3, double future, double ratio = 1.0)
        {
            var margin = Math.Max(ghost3.Width, ghost3.Height) * ratio;
            if (future < margin) future = margin;
            if (future > ghost3.BattleFieldHeight - margin) future = ghost3.BattleFieldHeight - margin;
            return future;
        }
    }
}
