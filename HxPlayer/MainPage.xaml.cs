using CommunityToolkit.Maui.Views;

namespace HxPlayer;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
        SelectedFileLabel.Text = "No file selected";
    }
    private async void OnPickFileButtonClicked(object sender, EventArgs e)
    {
        try
        {
            // Define allowed file types for audio (UTType for iOS/macOS, MIME for Android, extensions for Windows)
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.audio" } },            
                    { DevicePlatform.Android, new[] { "audio/*" } },             
                    { DevicePlatform.WinUI, new[] { ".mp3", ".wav", ".m4a" } },  
                    { DevicePlatform.macOS, new[] { "public.audio" } },          
                });

            var options = new PickOptions
            {
                PickerTitle = "Please select an audio file",
                FileTypes = customFileType
            };

            // Open the file picker
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                // Display selected file name
                SelectedFileLabel.Text = result.FileName;

                // Copy file to local app data directory
                using var sourceStream = await result.OpenReadAsync();
                string localPath = Path.Combine(FileSystem.AppDataDirectory, result.FileName);

                using var localFileStream = File.OpenWrite(localPath);
                await sourceStream.CopyToAsync(localFileStream);

                // Set the media source for playback (MediaSource.FromFile uses the local file path):contentReference[oaicite:7]{index=7}
                mediaElement.Source = MediaSource.FromFile(localPath);
            }
        }
        catch (Exception ex)
        {
            // Handle any errors (e.g. user canceled)
            await DisplayAlert("Error", "Could not select file: " + ex.Message, "OK");
        }
    }

    // Play button
    private void OnPlayButtonClicked(object sender, EventArgs e)
    {
        mediaElement.Play();
    }

    // Pause button
    private void OnPauseButtonClicked(object sender, EventArgs e)
    {
        mediaElement.Pause();
    }

    // Stop button
    private void OnStopButtonClicked(object sender, EventArgs e)
    {
        mediaElement.Stop();
    }
}

