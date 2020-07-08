using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace SharkSpirit.Modules.Core.Resources.Controls
{
    public abstract class NumericUpDownBase<T> : NumericUpDownBase where T : IComparable<T>
    {
        internal const string PART_TextBox = "PART_TextBox";
        internal const string PART_TextBlock = "PART_TextBlock";
        internal const string PART_UpButton = "PART_UpButton";
        internal const string PART_DownButton = "PART_DownButton";

        protected TextBox TextBox { get; private set; }
        protected TextBlock TextBlock { get; private set; }
        protected RepeatButton UpButton { get; private set; }
        protected RepeatButton DownButton { get; private set; }
        protected abstract T IncrementValue(T value, T increment);
        protected abstract T DecrementValue(T value, T increment);
        protected abstract bool IsGreaterThan(T value1, T value2);
        protected abstract bool IsLowerThan(T value, T value2);

        protected static void UpdateMetadata(Type type, T step, T minValue, T maxValue)
        {
            DefaultStyleKeyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(type));
            UpdateMetadataCommon(type, step, minValue, maxValue);
        }
        private static void UpdateMetadataCommon(Type type, T increment, T minValue, T maxValue)
        {
            StepProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(increment));
            MaxValueProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(maxValue));
            MinValueProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(minValue));
        }

        #region Value

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(T),
            typeof(NumericUpDownBase<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnValueChanged,
                null, false, UpdateSourceTrigger.LostFocus
            ));

        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is NumericUpDownBase<T> upDownBase)
                upDownBase.OnValueChanged((T)e.NewValue);
        }

        public T Value
        {
            get => (T)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        protected void OnValueChanged(T newValue)
        {
            try
            {
                UpdateControlText(newValue, StringFormat);
            }
            finally
            {
            }
        }

        #endregion

        #region StringFormat

        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register(
            nameof(StringFormat),
            typeof(string),
            typeof(NumericUpDownBase<T>),
            new FrameworkPropertyMetadata(
                "{0}",
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnFormatChanged
            ));

        private static void OnFormatChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is NumericUpDownBase<T> upDownBase)
                upDownBase.UpdateControlText(upDownBase.Value, (string)e.NewValue);
        }

        public string StringFormat
        {
            get => (string)GetValue(StringFormatProperty);
            set => SetValue(StringFormatProperty, value);
        }

        #endregion

        #region MaxValue

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            nameof(MaxValue),
            typeof(T),
            typeof(NumericUpDownBase<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender
            ));

        public T MaxValue
        {
            get => (T)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        #endregion

        #region MinValue

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            nameof(MinValue),
            typeof(T),
            typeof(NumericUpDownBase<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender
            ));

        public T MinValue
        {
            get => (T)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        #endregion

        #region Step

        public static readonly DependencyProperty StepProperty = DependencyProperty.Register(
            nameof(Step),
            typeof(T),
            typeof(NumericUpDownBase<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender
            ));

        public T Step
        {
            get => (T)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TextBlock = GetTemplateChild(PART_TextBlock) as TextBlock;
            if (TextBlock != null)
            {
                TextBlock.Visibility = Visibility.Visible;
            }

            if (UpButton != null)
            {
                UpButton.Click -= UpClick;
            }

            UpButton = GetTemplateChild(PART_UpButton) as RepeatButton;
            if (UpButton != null)
            {
                UpButton.Click += UpClick;
            }

            if (DownButton != null)
            {
                DownButton.Click -= DownClick;
            }

            DownButton = GetTemplateChild(PART_DownButton) as RepeatButton;
            if (DownButton != null)
            {
                DownButton.Click += DownClick;
            }

            UpdateControlText(Value, StringFormat);
        }

        protected virtual void UpdateControlText(T value, string format)
        {
            var text = value.ToString();

            try
            {
                text = string.Format(format, value);
            }
            catch (Exception)
            {
            }

            if (TextBlock != null)
                TextBlock.Text = text;

            if (TextBox != null)
                TextBox.Text = text;
        }

        private void UpClick(object sender, RoutedEventArgs e)
        {
            var success = Add(Step);
            if (success) GetBindingExpression(ValueProperty)?.UpdateSource();
        }

        private void DownClick(object sender, RoutedEventArgs e)
        {
            var success = Subtract(Step);
            if (success) GetBindingExpression(ValueProperty)?.UpdateSource();
        }

        protected bool Add(T step)
        {
            var newValue = IncrementValue(Value, step);

            SetValue(IsGreaterThan(newValue, MaxValue) ? MaxValue : newValue);
            return true;
        }
        protected bool Subtract(T step)
        {
            var newValue = DecrementValue(Value, step);

            if (newValue.Equals(Value)) return false;

            SetValue(IsLowerThan(newValue, MinValue) ? MinValue : newValue);
            return true;
        }

        protected virtual void SetValue(T newValue)
        {
            Value = newValue;
        }
    }

    public class NumericUpDownBase : UserControl
    {
        public static readonly DependencyProperty IsChangedProperty = DependencyProperty.Register(
            nameof(IsChanged),
            typeof(bool),
            typeof(NumericUpDownBase),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnChanged)
            ));

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumericUpDownBase upDownBase)
                upDownBase.OnIsChangedPropertyChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        protected virtual void OnIsChangedPropertyChanged(bool oldValue, bool newValue)
        {
        }

        public bool IsChanged
        {
            get => (bool)GetValue(IsChangedProperty);
            set => SetValue(IsChangedProperty, value);
        }
    }
}
