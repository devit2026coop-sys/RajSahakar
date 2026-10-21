using System;
using Cooperative.DBConnector;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.Collections.Generic;
using Cooperative.Controllers;
using System.Net;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Drawing.Imaging;
using System.Drawing;

namespace Cooperative.Models
{
    public class WatermarkService
    {
        public static string AddWatermarkToPdf(string inputFilePath, string outputFilePath, string watermarkText, bool isOverContent = true)
        {
            if (!Directory.Exists(Path.GetDirectoryName(outputFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));
            }
            using (var reader = new PdfReader(inputFilePath))
            using (var outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
            using (var stamper = new PdfStamper(reader, outputStream))
            {
                int totalPages = reader.NumberOfPages;
                PdfContentByte content;
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.WINANSI, BaseFont.EMBEDDED);

                for (int i = 1; i <= totalPages; i++)
                {
                    //content = stamper.GetUnderContent(i);
                    content = isOverContent ? stamper.GetOverContent(i) : stamper.GetUnderContent(i);

                    PdfGState gState = new PdfGState
                    {
                        FillOpacity = 0.3f,
                        StrokeOpacity = 0.3f
                    };
                    content.SetGState(gState);
                    content.BeginText();
                    content.SetFontAndSize(baseFont, 40);
                    content.SetColorFill(BaseColor.LIGHT_GRAY);
                    var pageSize = reader.GetPageSizeWithRotation(i);
                    float x = pageSize.Width / 2;
                    float y = pageSize.Height / 2;
                    content.ShowTextAligned(Element.ALIGN_CENTER, watermarkText, x, y, 45);
                    content.EndText();
                }
                return outputFilePath;
            }
        }
        public static string AddWatermarkToImage(System.Drawing.Image image, string outputFilePath, string watermarkText)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                int fontSize = 25;
                if (image.Width > 500 && image.Height > 500)
                    fontSize = 60;
                var font = new System.Drawing.Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                var color = Color.FromArgb(75, 255, 255, 255);
                var brush = new SolidBrush(color);
                float angle = -45f;
                var originalState = graphics.Save();
                var centerX = image.Width / 2;
                var centerY = image.Height / 2;
                graphics.TranslateTransform(centerX, centerY);
                graphics.RotateTransform(angle);
                var textSize = graphics.MeasureString(watermarkText, font);
                graphics.DrawString(watermarkText, font, brush, -textSize.Width / 2, -textSize.Height / 2);
                if (!Directory.Exists(Path.GetDirectoryName(outputFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));
                }
                graphics.Restore(originalState);
                image.Save(outputFilePath, ImageFormat.Jpeg);
                return outputFilePath;
            }
        }
    }
}