using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace deidentify_gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string inputFile;
        private string jsonFile;
        private string outputFile;
        private bool usingDefaultFilenames;
        public MainWindow()
        {
            InitializeComponent();
            resetDefaults();

        }

        private void resetDefaults()
        {
            this.inputFile = "default--input.txt";
            this.jsonFile = "default--tokens.json";
            this.outputFile = "default--output.txt";
            this.usingDefaultFilenames = true;
        }

        private void Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public string Status
        {
            get
            {
                return "";
            }
            set
            {

            }
        }


        string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                rtb.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                rtb.Document.ContentEnd
            );

            // The Text property on a TextRange object returns a string
            // representing the plain text content of the TextRange.
            return textRange.Text;
        }

        private void SetAllFilename()
        {
            if (this.usingDefaultFilenames == false)
            {
                int i = this.inputFile.LastIndexOf(".");
                string fileNameWithoutExtension = this.inputFile;
                if ( i > 0 )
                {
                    fileNameWithoutExtension = this.inputFile.Substring(0, i);
                }
                string path = fileNameWithoutExtension;
                this.jsonFile = path + "--tokens.json";
                this.outputFile = path + "--deidentified" + Path.GetExtension(this.inputFile);
                Console.WriteLine("{0} {1} {2}", this.inputFile, this.jsonFile, this.outputFile);
                Console.WriteLine();
            }
        }

        private void Click_Deidentify(object sender, RoutedEventArgs e)
        {
            Status= "Starting deidentification.";
            //File.WriteAllText(this.jsonFile, StringFromRichTextBox(RichTextBox_Original));
            File_Deidentify();
        }

        private void File_Deidentify()
        {
            string filename = @"C:\github.com\jftuga\deidentify\activate.bat";
            //outputFile = @"r:\temp\output.txt";
            string args = string.Format("-r {0} -o {1} {2}", TextBox_Replacement.Text, this.outputFile, this.inputFile);
            Console.WriteLine(args);
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            RichTextBox_Deidentified.Document.Blocks.Clear();
            proc.Start();
            while (!proc.StandardError.EndOfStream)
            {
                //string line = proc.StandardError.ReadLine();
                //TextBox_Status.Text = line;
                Status = proc.StandardError.ReadLine();
            }

            proc.StandardError.ReadLine();
            if (!this.usingDefaultFilenames)
            {
                string readText = File.ReadAllText(this.outputFile);
                RichTextBox_Deidentified.Document.Blocks.Add(new Paragraph(new Run(readText)));
            } else
            {
                string readText = StringFromRichTextBox(RichTextBox_Original);
                RichTextBox_Deidentified.Document.Blocks.Add(new Paragraph(new Run(readText)));
            }
        }


        private void Click_OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string readText = "";
            if (openFileDialog.ShowDialog() == true)
            {
                RichTextBox_Original.Document.Blocks.Clear();
                this.inputFile = openFileDialog.FileName;
                readText = File.ReadAllText(this.inputFile);
                RichTextBox_Original.Document.Blocks.Add(new Paragraph(new Run(readText)));
                this.usingDefaultFilenames = false;
                SetAllFilename();
            }
        }

        private void Click_About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Deidentify GUI" + "\n" + "version 1.0.0" + "\n\n" + "https://github.com/jftuga/deidentify-gui" + "\n" + "https://github.com/jftuga/deidentify", "Deidentify");
        }

        // https://www.codeproject.com/Tips/1273917/Opening-a-File-Specifically-in-the-Default-Browser
        private static string GetPathToDefaultBrowser()
        {
            const string currentUserSubKey =
            @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";
            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(currentUserSubKey, false))
            {
                string progId = (userChoiceKey.GetValue("ProgId").ToString());
                using (RegistryKey kp =
                       Registry.ClassesRoot.OpenSubKey(progId + @"\shell\open\command", false))
                {
                    string rawValue = (string)kp.GetValue("");
                    Regex reg = new Regex("(?<=\").*?(?=\")");
                    Match m = reg.Match(rawValue);
                    return m.Success ? m.Value : "";
                }
            }
        }

        private void Click_Debug(object sender, RoutedEventArgs e)
        {
            string browserPath = GetPathToDefaultBrowser();
            Process.Start(browserPath, this.jsonFile);
        }

        private void Click_Clear(object sender, RoutedEventArgs e)
        {
            RichTextBox_Original.Document.Blocks.Clear();
            RichTextBox_Deidentified.Document.Blocks.Clear();
            TextBox_Status.Text = "";
            resetDefaults();
        }

        private void Click_Copy(object sender, RoutedEventArgs e)
        {
            RichTextBox_Deidentified.SelectAll();
            RichTextBox_Deidentified.Copy();
        }
    }
}
