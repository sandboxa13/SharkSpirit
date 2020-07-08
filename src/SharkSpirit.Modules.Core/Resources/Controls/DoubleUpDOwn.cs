namespace SharkSpirit.Modules.Core.Resources.Controls
{
    public class DoubleUpDown : NumericUpDownBase<double>
    {
        static DoubleUpDown()
        {
            UpdateMetadata(typeof(DoubleUpDown), 1, double.MinValue, double.MaxValue);
        }

        protected override double IncrementValue(double value, double increment)
        {
            return value + increment;
        }

        protected override double DecrementValue(double value, double increment)
        {
            return value - increment;
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
