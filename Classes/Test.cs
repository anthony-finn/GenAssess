using GenAssess.Controls;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace GenAssess
{
    public class Test
    {
        #region Variables
        // Instance Variables
        public List<Question> Questions { get; set; }
        public string TestName { get; set; }
        public string FileName { get; set; }

        // Static Variables
        private static readonly string[] ALPHABET = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        // Class Instantiation
        private readonly RNGCryptoServiceProvider Random = new RNGCryptoServiceProvider();
        #endregion

        #region Constructors
        /// <summary>
        ///  public Test: Constructor for Test class.
        /// </summary>
        /// <param name="FileName">String name of the path to be saved..</param>
        /// <param name="Questions">Pre-created List of Questions.</param>
        /// <param name="TestName">String name of a newly created test.</param>
        public Test(string FileName = null, string TestName = "", List<Question> Questions = null)
        {
            // Sets Questions and TestName to Default Values.
            this.Questions = Questions ?? new List<Question>();
            this.TestName = TestName;
            // Create temporary randomly generated file name or one based off the FileName parameter.
            this.FileName = (FileName ?? Guid.NewGuid().ToString("D").ToUpper()) + ".pdf";
        }

        /// <summary>
        ///  public Test: Constructor for Test class for serialization.
        /// </summary>
        public Test()
        {
            // Sets Questions and TestName to Default Values.
            this.Questions = Questions ?? new List<Question>();
            this.TestName = "";
            // Create temporary randomly generated file name or one based off the FileName parameter.
            this.FileName = (FileName ?? Guid.NewGuid().ToString("D").ToUpper()) + ".pdf";
        }

        #endregion

        #region Instance Methods
        /// <summary>
        ///  public void AddQuestion: Adds a question to the List.
        /// </summary>
        /// <param name="question">Question to be added to the Questions List.</param>
        public void AddQuestion(Question question)
        {
            this.Questions.Add(question);
        }

        /// <summary>
        ///  public void RemoveQuestion: Removes a question from the List.
        /// </summary>
        /// <param name="question">Question to be removed from the Questions list.</param>
        public void RemoveQuestion(Question question)
        {
            this.Questions.Remove(question);
        }

        /// <summary>
        ///   public void DrawTest: Draws and creates a Pdf test.  
        /// </summary>
        /// <param name="pageOrientation">Default page orientation of a newly created test.</param>
        public void DrawTest(PageOrientation pageOrientation = PageOrientation.Portrait)
        {
            WindowViewModel.PrepareWebBrowser();
            // Create Pdf Document & initialize graphics for the PdfDocument.
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            page.Orientation = pageOrientation;
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);
            // Base Font Settings.
            double fontSize = 11;
            XFont font = new XFont("Times New Roman", fontSize);
            XFont boldFont = new XFont("Times New Roman", fontSize, XFontStyle.Bold);

            // Current Y Position on the Page.
            double currentY = 0;

            // Draw Name, Date, Period fields on top of first page.
            // Variables used in drawing the strings with mathematical operations.
            string nameDatePeriodString = "Name _________________________ P ____ Date _________";
            double padding = XUnit.FromCentimeter(1);
            double testNameHeight = gfx.MeasureString(TestName, boldFont).Height;
            XSize nameDatePeriodSize = gfx.MeasureString(nameDatePeriodString, font);
            double maxHeaderWidth = (page.Width - padding * 2) / 2;
            double maxImageWidth = (page.Width - padding * 10);
            double maxImageHeight = (page.Height - padding * 10);

            // Draw header strings with the above parameters.
            tf.DrawString($"{document.Pages.Count}", boldFont, XBrushes.Black, new XRect(page.Width / 2, page.Height - padding, page.Width, padding));
            tf.DrawString(TestName, boldFont, XBrushes.Black, new XRect(padding, padding, maxHeaderWidth, testNameHeight));
            tf.DrawString(nameDatePeriodString, font, XBrushes.Black, new XRect(page.Width - padding - nameDatePeriodSize.Width, padding, maxHeaderWidth, nameDatePeriodSize.Height));

            // Update currentY position on the page.
            currentY += padding * 2 + testNameHeight;

            /// <summary>
            ///   bool newPage: Checks if a page is needed in order to draw a string.
            /// </summary>
            bool newPage()
            {
                if (currentY >= page.Height - padding * 2)
                {
                    // Dipose of the current graphics object.
                    gfx.Dispose();

                    // Create new page with specified settings and instantiate new graphics objects.
                    page = document.AddPage();
                    page.Orientation = pageOrientation;
                    gfx = XGraphics.FromPdfPage(page);
                    tf = new XTextFormatter(gfx);

                    // Set currentY to the start of a page.
                    currentY = padding;

                    // Draw page number on new Page.
                    tf.DrawString($"{document.Pages.Count}", boldFont, XBrushes.Black, new XRect(page.Width / 2, page.Height - padding, page.Width, padding));

                    // Return "true" bool that a new page was created.
                    return true;
                }
                // Return "false" bool that a new page wasn't created.
                return false;
            }

            // Draw Questions
            // Variables needed for both Question and Answer Choices.
            double maxQuestionWidth = page.Width - padding * 2;

            // Iterate through each question in a Test object.
            for (int i = 0; i < Questions.Count; i++)
            {

                // Current Question details on the format of a soon to be generated string.
                Question question = Questions[i];
                string questionNumText = $"{i + 1}.    ";
                string questionText = question.Text;
                XSize questionNumSize = gfx.MeasureString(questionNumText, boldFont);
                XSize questionSize = gfx.MeasureString(questionText, font);
                int count = new Regex(Regex.Escape("\n")).Matches(questionText).Count;
                double questionHeight = Math.Ceiling(questionSize.Width / maxQuestionWidth + count) * questionSize.Height;

                // Check if a new page needs to be created.
                if (newPage())
                {
                    // Subtract the current index and repeat the iteration for i - 1.
                    i--;
                    continue;
                }

                // Draws the question with question number.
                tf.DrawString(questionNumText, boldFont, XBrushes.Black, new XRect(padding, currentY, maxQuestionWidth, questionNumSize.Height));
                tf.DrawString(questionText, font, XBrushes.Black, new XRect(padding + questionNumSize.Width, currentY, maxQuestionWidth - questionNumSize.Width, questionHeight + questionSize.Height));

                // Update currentY position on the page.
                currentY += questionHeight + fontSize;

                // Draw an image after the question if the question includes an image.
                if (question.ImagePath != null)
                {

                    // Convert and get Image Details.
                    dynamic[] imageInfo = GetImageFromFile(question.ImagePath, maxImageWidth, maxImageHeight);
                    XImage image = imageInfo[0];
                    int width = imageInfo[1];
                    int height = imageInfo[2];
                    Bitmap bitmapImage = imageInfo[3];

                    // Draw the image.
                    gfx.DrawImage(image, (page.Width + width) / 6, currentY);

                    // Dispose of bitmapImage.
                    bitmapImage.Dispose();
                    image.Dispose();

                    // Update currentY.
                    currentY += height;
                }
                
                // Draw Answer Choices
                // Merge the correct and wrong answer choices into a single array.
                string[] wrongChoices = question.Choices;
                string[] correctChoices = question.Answers;
                string[] choices = new string[wrongChoices.Length + correctChoices.Length];
                Array.Copy(wrongChoices, choices, wrongChoices.Length);
                Array.Copy(correctChoices, 0, choices, wrongChoices.Length, correctChoices.Length);

                // Shuffle the answer choices.
                Shuffle(choices);

                // Loop through each of the answer choices to draw.
                for (int k = 0; k < choices.Length; k++)
                {
                    // Variables to determine the size and string texts of an answer choice.
                    string letter = ALPHABET[k % ALPHABET.Length] + ".  ";
                    XSize letterSize = gfx.MeasureString(letter, boldFont);
                    XSize choiceSize = gfx.MeasureString(choices[k], font);
                    double choiceHeight = Math.Ceiling(choiceSize.Width / (maxQuestionWidth - padding - letterSize.Width)) * choiceSize.Height;

                    // Check if a new page needs to be created.
                    if (newPage())
                    {
                        // Subtract the current index and repeat the loop.
                        k--;
                        continue;
                    }

                    // Draws the answer choice and letter of the choice.
                    tf.DrawString(letter, boldFont, XBrushes.Black, new XRect(padding * 2, currentY, maxQuestionWidth - padding, letterSize.Height));
                    tf.DrawString(choices[k], font, XBrushes.Black, new XRect(padding * 2 + letterSize.Width, currentY, maxQuestionWidth - padding - letterSize.Width, choiceHeight));

                    // Update the currentY position.
                    currentY += letterSize.Height + choiceHeight;
                }
            }

            // Save and Close the newly generated test.
            document.Save(FileName);
            document.Dispose();
        }

        /// <summary>
        ///  public static void RenderPdf: Method to change the PdfViewer control in the custom window style template.
        /// </summary>
        /// <param name="window">Window control.</param>
        /// <param name="pdfViewer">PdfViewer control.</param>
        /// <param name="FileName">File name of a Pdf Document.</param>
        public static void RenderPdf(Window window, WebBrowser pdfViewer, string FileName)
        {
            // Get Document Information
            if (PdfReader.TestPdfFile(FileName) != 0)
            {
                PdfDocument document = PdfReader.Open(FileName, PdfDocumentOpenMode.ReadOnly);
                double documentWidth = document.Pages[0].Width;
                double documentHeight = document.Pages[0].Height;

                // Set Source to Pdf path.
                FileInfo fileInfo = new FileInfo(FileName);
                pdfViewer.Source = new Uri(fileInfo.FullName);

                // Set Width and Height of PdfViewer control based on the document's page size.
                pdfViewer.Width = documentWidth;
                pdfViewer.Height = documentHeight;

                // Set Width and Height of the window.
                double height = documentHeight + 48;
                double width = documentWidth + 48;
                window.Width = width;
                window.Height = height;
                window.MinWidth = width;
                window.MinHeight = height;

                // Close Document
                document.Dispose();

            }
        }

        /// <summary>
        ///  public void RenderPdf: Method to change the PdfViewer control in the custom window style template.
        /// </summary>
        /// <param name="window">Window control.</param>
        /// <param name="pdfViewer">PdfViewer control.</param>
        public void RenderPdf(Window window, WebBrowser pdfViewer)
        {
            Test.RenderPdf(window, pdfViewer, FileName);
        }

        /// <summary>
        ///  public void Shuffle: Modern Fisher-Yates shuffle algorithm.
        /// </summary>
        public void Shuffle()
        {
            int i = Questions.Count;
            while (i > 1)
            {
                i--;
                // Randomly choose the next index from i to the number of strings in the parameter array.
                int j = NextInt(i + 1);
                // Swaps the values of indexes at i and j.
                Question value = Questions[j];
                Questions[j] = Questions[i];
                Questions[i] = value;
            }
        }
        #endregion

        #region Static and Private Methods
        /// <summary>
        ///   public dynamic[] GetImageFromFile: Returns XImage, width, and height of image from a path.
        /// </summary>
        /// <param name="path">Path of a specified image.</param>
        /// <param name="maxWidth">The maximum width of the given image based on the page size.</param>
        /// <param name="maxHeight">The maximum height of the given image based on the page size.</param>
        /// <returns>The method returns an array which includes an XImage object, width integer, and height integer.</returns>
        private dynamic[] GetImageFromFile(string path, double maxWidth, double maxHeight)
        {
            MemoryStream memoryStream = new MemoryStream();
            System.Drawing.Image image = System.Drawing.Image.FromStream(File.OpenRead(path));

            // Math Operations to scale image with maxWidth and maxHeight parameters.
            float scaleHeight = (float)maxHeight / (float)image.Height;
            float scaleWidth = (float)maxWidth / (float)image.Width;
            float scale = Math.Min(scaleHeight, scaleWidth);
            int width = (int)(image.Width * scale);
            int height = (int)(image.Height * scale);

            // Create a resized image in the Png format.
            image = new Bitmap(image, width, height);
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

            // Return parameters for PdfDocument graphics.
            return new dynamic[] {
                XImage.FromStream(memoryStream),
                width,
                height,
                image
            };
        }

        /// <summary>
        ///  private void Shuffle: Modern Fisher-Yates shuffle algorithm for a string array. Overloaded private method of Shuffle with a string array parameter.
        /// </summary>
        /// <param name="array">Array of strings.</param>
        private void Shuffle(string[] array)
        {
            int i = array.Length;
            while (i > 1)
            {
                i--;
                // Randomly choose the next index from i to the number of strings in the parameter array.
                int j = NextInt(i + 1);
                // Swaps the values of indexes at i and j.
                string value = array[j];
                array[j] = array[i];
                array[i] = value;
            }
        }

        /// <summary>
        ///  private int NextInt: Generates a random integer with the RNGCryptoServiceProvider class. 
        ///  Prevents psuedo-random generated numbers which are predictable.
        /// </summary>
        /// <param name="min">Minimum value for generated number.</param>
        /// <param name="max">Maximum value for generated number.</param>
        /// <returns></returns>
        private int NextInt(int min = 0, int max = 0)
        {
            // Create a byte array.
            byte[] bytes = new byte[4];
            // Fill bytes array with random byteds.
            Random.GetBytes(bytes);
            // Convert bytes array to an unsigned integer
            uint scale = BitConverter.ToUInt32(bytes, 0);
            // Returns randomly generated index based on the calculated scale.
            return (int)(min + (max - min) * (scale / (double)uint.MaxValue));
        }

        /// <summary>
        ///  private int NextInt: Generates a random integer with the RNGCryptoServiceProvider class. 
        ///  Prevents psuedo-random generated numbers which are predictable.
        /// </summary>
        /// <param name="max">Maximum value for generated number which ranges from 0 to this value.</param>
        /// <returns></returns>
        private int NextInt(int max)
        {
            return NextInt(0, max);
        }
        #endregion
    }
}
