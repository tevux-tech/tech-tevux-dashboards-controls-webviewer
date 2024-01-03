namespace Tech.Tevux.Dashboards.Controls;

public partial class WebViewer {
    public static readonly DependencyProperty RefreshPeriodProperty = DependencyProperty.Register(
        nameof(RefreshPeriod),
        typeof(decimal),
        typeof(WebViewer),
        new PropertyMetadata(60m));

    [ExposedNumber]
    [Category(OptionCategory.Main)]
    public decimal RefreshPeriod {
        get { return (decimal)GetValue(RefreshPeriodProperty); }
        set { SetValue(RefreshPeriodProperty, value); }
    }
}
