namespace HelloMaui;

public partial class MainPage : ContentPage
{
    private const string TimestampFormat = "yyyy-MM-dd HH:mm:ss";

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnShowTimeClicked(object sender, EventArgs e)
    {
        var utcNow = DateTime.UtcNow;
        var localNow = DateTime.Now;

        TimestampLabel.Text =
            $"UTC:   {utcNow.ToString(TimestampFormat)}{Environment.NewLine}" +
            $"Local: {localNow.ToString(TimestampFormat)}";
    }
}
