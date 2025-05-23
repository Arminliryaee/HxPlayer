﻿using CommunityToolkit.Maui.Views;

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
            // Define allowed file types for audio (UTType for iOS, MIME for Android, extensions for Windows)
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.audio" , "public.video" } },            
                    { DevicePlatform.Android, new[] { "audio/*","video/*" } },             
                    { DevicePlatform.WinUI, new[] { ".mp3",".mp4", ".wav", ".m4a" } },  
                });

            var options = new PickOptions
            {
                PickerTitle = "Please select an audio file",
                FileTypes = customFileType
            };

            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                SelectedFileLabel.Text = result.FileName;

                using var sourceStream = await result.OpenReadAsync();
                string localPath = Path.Combine(FileSystem.AppDataDirectory, result.FileName);

                using var localFileStream = File.OpenWrite(localPath);
                await sourceStream.CopyToAsync(localFileStream);

                mediaElement.Source = MediaSource.FromFile(localPath);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Could not select file: " + ex.Message, "OK");
        }
    }
}

