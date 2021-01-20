using System;
using System.Windows.Forms;
using ImageMagick;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using System.IO;


namespace HeicToPdf
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Pictures|*.jpg;*.png;*.heic";
 
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Format|*.pdf";

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string[] paths = openFileDialog.FileNames;
            ConvertToJpg(paths);

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            PdfWriter writer = new PdfWriter(saveFileDialog.FileName);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            addImagesToPdf(document, paths);
            document.Close();
        }


        static void addImagesToPdf(Document doc, String[] imagePaths)
        {
            foreach(String str in imagePaths)
            {
                iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(str));
                img.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                doc.Add(img);
            }
        }

        static void ConvertToJpg(string[] heicFilePaths)
        {
            for (var i = 0; i < heicFilePaths.Length; i++)
            {
                if (heicFilePaths[i].ToLower().EndsWith("heic"))
                {
                    heicFilePaths[i] = PerformConversion(heicFilePaths[i]);
                }
            }
        }

        static string PerformConversion(string filePath)
        {
            // ReSharper disable once ConvertToUsingDeclaration
            using (var image = new MagickImage(filePath))
            {
                var newFile = "";
                if (filePath.EndsWith(".heic"))
                {
                    newFile = filePath.Replace(".heic", ".jpg");
                } else
                {
                    newFile = filePath.Replace(".HEIC", ".JPG");
                }
                image.Write(newFile);
                File.Delete(filePath);
                return newFile;
            }
        }
    }
}
