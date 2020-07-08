using System.Windows;
using System.Windows.Controls;

namespace SharkSpirit.Modules.Core.Resources.Controls
{
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