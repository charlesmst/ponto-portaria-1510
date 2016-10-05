using System;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace PontoPortaria1510.Report
{
    public class PaginaComRodape : PageHandler
    {
        public string TextoDireita { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        public string TextoEsquerda { get; set; } = "";
        public System.Drawing.Image Logo { get; set; }
        public PdfPageEventHelper PdfHandler()
        {
            return new PdfFooterHandler()
            {
                TextoDireita = TextoDireita,
                TextoEsquerda = TextoEsquerda,
                ImageLogo = Image.GetInstance(ResizeImage(Logo, 100), System.Drawing.Imaging.ImageFormat.Png)
            };
        }


        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        private System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int height)
        {
            var proportionHeight = Convert.ToDouble(height) / image.Height;
            var width = Convert.ToInt32(image.Width * proportionHeight);
            var destinationImage = new System.Drawing.Bitmap(width, height);

            using (var graphics = System.Drawing.Graphics.FromImage(destinationImage))
            {
                graphics.Clear(System.Drawing.Color.Transparent);
                using (var sourceImage = image)
                {
                    // Use alpha blending in case the source image has transparencies.
                    graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

                    // Use high quality compositing and interpolation.
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    graphics.DrawImage(sourceImage, 0, 0, width, height);
                }
            }
            return destinationImage;
        }
    }
}
