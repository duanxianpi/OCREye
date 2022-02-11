using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Vision.V1;
using System.Windows.Forms;
using System.IO;

namespace OCREye
{
    public static class OCR
    {
        public static System.Drawing.Image image;
        public static void GetOCRImage()
        {
            image = Clipboard.GetImage();
        }

        public static String getText()
        {
            ImageAnnotatorClient client = ImageAnnotatorClient.Create();
            Byte[] googleImage = imageToByte(image);
            String result = "";

            TextAnnotation text = client.DetectDocumentText(Image.FromBytes(googleImage));
            Console.WriteLine($"Text: {text.Text}");
            foreach (var page in text.Pages)
            {
                foreach (var block in page.Blocks)
                {
                    string box = string.Join(" - ", block.BoundingBox.Vertices.Select(v => $"({v.X}, {v.Y})"));
                    //Console.WriteLine($"Block {block.BlockType} at {box}");
                    foreach (var paragraph in block.Paragraphs)
                    {
                        box = string.Join(" - ", paragraph.BoundingBox.Vertices.Select(v => $"({v.X}, {v.Y})"));
                        //Console.WriteLine($"  Paragraph at {box}");
                        foreach (var word in paragraph.Words)
                        {
                            foreach (var Symbol in word.Symbols)
                            {
                                result += $"{string.Join("", Symbol.Text)}";
                                if (Symbol.Property == null)
                                {
                                    continue;
                                }
                                if (Symbol.Property.DetectedBreak == null)
                                {
                                    continue;
                                }
                                if (Symbol.Property.DetectedBreak.Type.Equals(TextAnnotation.Types.DetectedBreak.Types.BreakType.Space))
                                {
                                    result += " ";
                                }
                                if (Symbol.Property.DetectedBreak.Type.Equals(TextAnnotation.Types.DetectedBreak.Types.BreakType.EolSureSpace))
                                {
                                    try
                                    {
                                        if (Symbol.Property.DetectedLanguages[0].LanguageCode != "zh")
                                        {
                                            result += " ";
                                        }
                                    }
                                    catch
                                    {

                                    }
                                    
                                }
                                if (Symbol.Property.DetectedBreak.Type.Equals(TextAnnotation.Types.DetectedBreak.Types.BreakType.LineBreak))
                                {
                                    result += "\n";
                                }

                            }
                        }
                            
                            //Console.WriteLine($"    Word: {string.Join("", word.Symbols.Select(s => s.Text))}");
                        }
                    }
                }
            return result;
        }

        public static String doOCR()
        {
            String Result = getText();
            return Result;
        }

        private static byte[] imageToByte(System.Drawing.Image _image)
        {
            MemoryStream ms = new MemoryStream();
            _image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

    }
}
