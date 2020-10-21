using System;
using System.Collections;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PdfAnalizerv1
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string transcriptStr = pdfText(@"/Users/canoltasgin/Desktop/transcript");

            using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(@"/Users/canoltasgin/Desktop/transcript.txt"))
            {
                file.WriteLine(transcriptStr);
            }
            analizePdf(transcriptStr);
        }

        public static string pdfText(string path)
        {
            PdfReader reader = new PdfReader(path);
            string text = string.Empty;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                text += PdfTextExtractor.GetTextFromPage(reader, page);
            }
            reader.Close();
            return text;
        }

        static Hashtable previousClasses;

        public static void analizePdf(string transcript)
        {
            int a,b;
            string entryYear = "";
            string program = "";
            for (int index = 0; ; index += "-20".Length)
            {
                index = transcript.IndexOf("-20", index);
                a = index;
                break;
            }

            for (int i = 12; i > 0; i--)
                entryYear += transcript[a - i];

              Console.WriteLine(entryYear);
            
              for (int index = 0; ; index += "Program(s) :".Length)
            {
                index = transcript.IndexOf("Program(s) :", index);
                b = index;
                break;
            }

            for (; b < a - entryYear.Length; b++)
                program += transcript[b];

            Console.WriteLine(program);

            previousClasses = new Hashtable();

            List<string> lines = new List<string>();
            int temp = 0;
            string className = "";
            string restOfInfo = "";
            for (int index = 0; ; index += "\n".Length)
            {
                index = transcript.IndexOf("\n", index);
                if (index == -1)
                    break;

                string tempLine = transcript.Substring(temp, index - temp);
                temp = index;

                int counter = 1;
                int value = 0;
                
                foreach (string word in tempLine.Split(' '))
                {
                    if (counter == 2) //if second word
                    {
                        if (int.TryParse(word, out value)) // if 2nd word can become an integer
                        {
                            if (tempLine.Contains("UG") && tempLine.Contains("Repeated") == false)
                            {
                                className = tempLine.Substring(1, tempLine.IndexOf(word) + word.Length - 1);
                                lines.Add(tempLine);
                                restOfInfo = tempLine.Substring(tempLine.IndexOf("UG") + "UG".Length + 1, tempLine.Length - tempLine.IndexOf("UG") - "UG".Length - 1);
                                if (restOfInfo[0] != 'F' && restOfInfo.Substring(0, 2) != "NA" && restOfInfo[0] != 'W')
                                {
                                    if (String.IsNullOrWhiteSpace(restOfInfo.Substring(0, 1)))
                                    {
                                        restOfInfo = "? " + restOfInfo.Substring(2,restOfInfo.Length-2);
                                    }
                                    previousClasses.Add(className, restOfInfo.Substring(0, 2));
                                    //Console.WriteLine(className + "/ / / / / " + restOfInfo);
                                }
                            }
                        }
                    }
                    counter++;
                }   
            }

            foreach (DictionaryEntry entry in previousClasses)
            {
                Console.WriteLine("Course Code: " + entry.Key + "/ Grade: " + entry.Value);
            }

            //Hashtable a eklemeden once DERS KODU - DERS ADI - DERS KREDISI - DERSTEN ALINAN NOT olarak 4 ayri stringle bi struct olustur.
            //Transcript derslerini en son halde Hash Table olarak kur. Boylece sonrasinda mezuniyet yukumluluklerinde gerekenlerle direkt karsilastirilabilir.

            Console.WriteLine();


            //Tek tek tum line lara bak. Eger ikinci kelime 3 haneli integersa List e ekle. Daha sonra Liste eklenenlerin icerigini duzenle.


            
        }
    }
}



