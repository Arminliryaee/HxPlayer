using CommunityToolkit.Maui.Views;

namespace HxPlayer;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        SelectedFileLabel.Text = "No file selected";
    }

    // This attribute lets Shell inject the "file" query parameter
    public string FilePath
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                LoadAndPlay(value);
        }
    }

    private void LoadAndPlay(string localPath)
    {
        SelectedFileLabel.Text = Path.GetFileName(localPath);
        mediaElement.Source = MediaSource.FromFile(localPath);
        mediaElement.Play();
    }

    private async void OnPickFileButtonClicked(object sender, EventArgs e)
    {
        // your existing PickAsync code…
        // after copying to AppDataDirectory, call LoadAndPlay(localPath);
    }

    private async void OnPlaylistButtonClicked(object sender, EventArgs e)
    {
        // navigate to the Playlist tab
        await Shell.Current.GoToAsync("//Playlist");
    }

    private void OnPlayButtonClicked(object sender, EventArgs e) => mediaElement.Play();
    private void OnPauseButtonClicked(object sender, EventArgs e) => mediaElement.Pause();
    private void OnStopButtonClicked(object sender, EventArgs e) => mediaElement.Stop();
}
