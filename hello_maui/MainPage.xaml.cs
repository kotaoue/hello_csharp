namespace HelloMaui;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnShowTimeClicked(object sender, EventArgs e)
    {
        TimestampLabel.Text = $"UTC:   {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}\nLocal: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
    }
}
