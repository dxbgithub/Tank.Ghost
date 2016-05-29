using System;

namespace Ghost.Ghost1
{
    public class Position
    {
        public double X;
        public double Y;

        public Position Clone()
        {
            return new Position
            {
                X = X,
                Y = Y
            };
        }

        public bool IsValid(Ghost1 ghost1)
        {
            var margin = Math.Max(ghost1.Width, ghost1.Height);
            if (X < margin ||
                X > ghost1.BattleFieldWidth - margin ||
                Y < margin ||
                Y > ghost1.BattleFieldHeight- margin)
                return false;
            return true;
        }

        public Position MakeValid(Ghost1 ghost1)
        {
            var margin = Math.Max(ghost1.Width, ghost1.Height);
            if (X < margin) X = margin;
            if (X > ghost1.BattleFieldWidth - margin) X = ghost1.BattleFieldWidth - margin;
            if (Y < margin) Y = margin;
            if (Y > ghost1.BattleFieldHeight - margin) Y = ghost1.BattleFieldHeight - margin;
            return this;
        }

        public double DistanceTo(Position pos)
        {
            var xDiff = X - pos.X;
            var yDiff = Y - pos.Y;
            return Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
        }
    }
}
