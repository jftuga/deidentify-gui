using HtmlAgilityPack;
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
            string temp = (Environment.GetEnvironmentVariable("TEMP").Length > 0) ? Environment.GetEnvironmentVariable("TEMP") : (Environment.GetEnvironmentVariable("TMP").Length > 0) ? Environment.GetEnvironmentVariable("TMP") : ".";
            temp += Path.DirectorySeparatorChar;
            this.inputFile = temp + "default--input.txt";
            this.jsonFile = temp + "default--tokens.json";
            this.outputFile = temp + "default--output.htm";
            this.jsonFile = temp;
            this.usingDefaultFilenames = true;
        }

        private void Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );

            return textRange.Text;
        }

        private void SetAllFilename()
        {
            string fileNameWithoutExtension = this.inputFile;

            if (this.usingDefaultFilenames == false)
            {
                int i = this.inputFile.LastIndexOf(".");
                
                if ( i > 0 )
                {
                    fileNameWithoutExtension = this.inputFile.Substring(0, i);
                }
                this.jsonFile = fileNameWithoutExtension + "--tokens.json";
                this.outputFile = fileNameWithoutExtension + "--deidentified" + ".htm";
                Console.WriteLine("{0} {1} {2}", this.inputFile, this.jsonFile, this.outputFile);
                Console.WriteLine();
            }
        }

        private void Click_Deidentify(object sender, RoutedEventArgs e)
        {
            Label_Status.Content = "Status: Starting deidentification...";
            if ( usingDefaultFilenames ) {
                File.WriteAllText(this.inputFile, StringFromRichTextBox(RichTextBox_Original));
            }
            File_Deidentify();
        }

        private void File_Deidentify()
        {
            // FIXME - When entering text (and not using Open), this button must be pressed twice
            string filename = @"C:\github.com\jftuga\deidentify\activate.bat"; //FIXME - don't hard code path
            string args = string.Format("-H -r {0} -o {1} {2}", TextBox_Replacement.Text, this.outputFile, this.inputFile);
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

            Label_Status.Content = "Status:";
            proc.Start();
            string line = "";
            while (!proc.StandardError.EndOfStream)
            {
                line = proc.StandardError.ReadLine();
                Label_Status.Content = "Status: " + line;
            }

            proc.StandardError.ReadLine();
            myBrowser.Navigate(this.outputFile);
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
            //FIXME - don't hard code version number
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
            string j = (usingDefaultFilenames) ? this.jsonFile + "default--input--tokens.json" : this.jsonFile;

            string browserPath = GetPathToDefaultBrowser();
            Process.Start(browserPath, j);
        }

        private void Click_Clear(object sender, RoutedEventArgs e)
        {
            RichTextBox_Original.Document.Blocks.Clear();
            myBrowser.Navigate(@"about:blank");
            Label_Status.Content = "Status:";
            resetDefaults();
        }

        private void Click_Copy(object sender, RoutedEventArgs e)
        {

            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(this.outputFile);
            var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//body");

            //FIXME - don't break after one iteration
            foreach (var node in htmlNodes)
            {
                Console.WriteLine(node.InnerHtml);
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, node.InnerText), true);
                break;
            }

        }

        private void Click_Into_Original(object sender, MouseButtonEventArgs e)
        {
            string o = StringFromRichTextBox(RichTextBox_Original);
            if ( o.StartsWith("Original Text") )
            {
                RichTextBox_Original.Document.Blocks.Clear();
            }
        }

        private void Click_Save(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("The SAVE function has not been implemented yet." + "\n" + "Instead, use COPY to copy the deidentifed text to the clipboard.", "Deidentify");
        }
    }
}
