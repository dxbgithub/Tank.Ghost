using System;
using Robocode.Util;

namespace Ghost.GhostA
{
	internal static class Descartes
	{
		public const double Pi = 3.1415926535897931;

		public const double TwoPi = 6.2831853071795862;

		public const double QuarterPi = 0.78539816339744828;

		public const double EighthPi = 0.39269908169872414;

		public static double Absbearing(double x1, double y1, double x2, double y2)
		{
			double num = x2 - x1;
			double num2 = y2 - y1;
			double range = Descartes.GetRange(x1, y1, x2, y2);
			if (num > 0.0 && num2 > 0.0)
			{
				return Math.Asin(num / range);
			}
			if (num > 0.0 && num2 < 0.0)
			{
				return 3.1415926535897931 - Math.Asin(num / range);
			}
			if (num < 0.0 && num2 < 0.0)
			{
				return 3.1415926535897931 + Math.Asin(-num / range);
			}
			if (num < 0.0 && num2 > 0.0)
			{
				return 6.2831853071795862 - Math.Asin(-num / range);
			}
			return 0.0;
		}

		public static double GetRange(double x1, double y1, double x2, double y2)
		{
			double arg_07_0 = x2 - x1;
			double num = y2 - y1;
			double arg_0C_0 = arg_07_0 * arg_07_0;
			double expr_0A = num;
			return Math.Sqrt(arg_0C_0 + expr_0A * expr_0A);
		}

		public static double NormaliseBearing(double rad)
		{
			return Utils.NormalRelativeAngle(rad);
		}

		public static bool PointInRegion(double x, double y, double top, double left, double right, double bottom)
		{
			return x >= left && x <= right && y >= top && y <= bottom;
		}
	}
}
