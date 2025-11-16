namespace MauiCameraRepro
{
    public partial class MainPage : ContentPage
    {
        string message = "";
        string imageUrl;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object? sender, EventArgs e)
        {
            var result = await MediaPicker.Default.PickPhotosAsync(new()
            {
                MaximumHeight = 200,
                RotateImage = true,
                PreserveMetaData = true,
                SelectionLimit = 1
            });

            if (result is null)
            {
                message = "PickPhotosAsync returned null.";
                MessageLabel.Text = message;
                UploadedImage.Source = null;
                return;
            }

            if (result is [])
            {
                message = "PickPhotosAsync returned empty collection.";
                MessageLabel.Text = message;
                UploadedImage.Source = null;
                return;
            }

            // save the file into local storage
            var photo = result.First();
            string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

            using Stream sourceStream = await photo.OpenReadAsync();
            using FileStream localFileStream = File.OpenWrite(localFilePath);

            await sourceStream.CopyToAsync(localFileStream);

            imageUrl = localFilePath;
            message = "Image uploaded successfully.";

            MessageLabel.Text = message;
            UploadedImage.Source = ImageSource.FromFile(imageUrl);
        }
    }
}
