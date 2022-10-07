using System.Threading.Tasks;
using System.Windows.Controls;
using CefSharp;
using CefSharp.Wpf;

namespace Tech.Tevux.Dashboards.Controls;

[DashboardControl]
[TemplatePart(Name = "PART_MainGrid", Type = typeof(Grid))]
public partial class WebViewer : ControlBase {
    private bool _isDisposed = false;
    private bool _isTaskRunning = true;
    private readonly CancellationTokenSource _globalCts = new();
    internal ChromiumWebBrowser? Browser { get; private set; }

    static WebViewer() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(WebViewer), new FrameworkPropertyMetadata(typeof(WebViewer)));
    }

    public WebViewer() { }

    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        if (DesignerProperties.GetIsInDesignMode(this)) { return; }

        if (Template.FindName("PART_MainGrid", this) is Grid grid) {
            if (Cef.IsInitialized == false) {
                var settings = new CefSettings {
                    CefCommandLineArgs = { ["disable-gpu-shader-disk-cache"] = "1" }
                };

                Cef.Initialize(settings, false, browserProcessHandler: null);
            }

            Browser = new ChromiumWebBrowser(Url);
            grid.Children.Add(Browser);
            decimal localRefreshPeriod = -1;

            Task.Run(async () => {
                while (_globalCts.Token.IsCancellationRequested == false) {
                    Dispatcher.Invoke(() => { localRefreshPeriod = RefreshPeriod; });
                    await Task.Delay(TimeSpan.FromSeconds((double)localRefreshPeriod), _globalCts.Token).ContinueWith(task => { });

                    try {
                        Browser.ReloadCommand.Execute(null);
                    }
                    catch (Exception) {
                        // Swallowing.                        
                    }
                }

                _isTaskRunning = false;
            });
        }

    }

    protected override void Dispose(bool isCalledManually) {
        if (_isDisposed == false) {
            if (isCalledManually) {
                // Dispose managed objects here.
                _globalCts.Cancel();
                while (_isTaskRunning) {
                    Thread.Sleep(1);
                }
                Dispatcher.Invoke(() => { Browser?.Dispose(); });
            }

            // Free unmanaged resources here and set large fields to null.
            Browser = null;
            _isDisposed = true;
        }
    }
}
