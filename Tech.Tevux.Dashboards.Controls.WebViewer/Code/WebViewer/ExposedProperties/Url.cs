namespace Tech.Tevux.Dashboards.Controls;

public partial class WebViewer {
    public static readonly DependencyProperty UrlProperty = DependencyProperty.Register(
        nameof(Url),
        typeof(string),
        typeof(WebViewer),
        new PropertyMetadata("http://www.wikipedia.org", (d, e) => {
            if (d is WebViewer webViewer) {
                if (webViewer.Browser is not null) {
                    webViewer.Browser.Address = e.NewValue.ToString();
                }
            }
        }));

    [ExposedSingleLineText]
    [Category(OptionCategory.Main)]
    public string Url {
        get { return (string)GetValue(UrlProperty); }
        set { SetValue(UrlProperty, value); }
    }
}
