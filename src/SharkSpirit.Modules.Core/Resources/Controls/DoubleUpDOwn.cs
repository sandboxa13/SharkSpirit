using System;

namespace SharkSpirit.Modules.Core.Resources.Controls
{
    public class DoubleUpDown : NumericUpDownBaseGeneric<double>
    {
        static DoubleUpDown()
        {
            UpdateMetadata(typeof(DoubleUpDown), 1, double.MinValue, double.MaxValue);
        }

        protected override double IncrementValue(double value, double increment)
        {
            return Math.Round(value, 3) + increment;
        }

        protected override double DecrementValue(double value, double increment)
        {
            return Math.Round(value, 3) - increment;
        }

        protected override bool IsLowerThan(double value1, double value2)
        {
            return value1.CompareTo(value2) < 0;
        }

        protected override bool IsGreaterThan(double value1, double value2)
        {
            return value1.CompareTo(value2) > 0;
        }
    }
}
