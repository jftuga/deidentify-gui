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
        private string jsonFile;
        private string outputFile;
        public MainWindow()
        {
            InitializeComponent();
            jsonFile = "";
            outputFile = "";
        }

        private void Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public string textOriginal
        {
            get
            {
                return "";
            }
        }

        public string textDeidentified
        {
            get
            {
                return "";
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

        private void Click_Deidentify(object sender, RoutedEventArgs e)
        {
            TextBox_Status.Text = "Starting deidentification...";
            jsonFile = @"r:\temp\test.txt";
            File.WriteAllText(jsonFile, StringFromRichTextBox(RichTextBox_Original));
            File_Deidentify();
        }

        private void File_Deidentify()
        {
            string filename = @"C:\github.com\jftuga\deidentify\activate.bat";
            outputFile = @"r:\temp\output.txt";
            string args = string.Format("-r {0} -o {1} {2}", TextBox_Replacement.Text, outputFile, jsonFile);
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
                string line = proc.StandardError.ReadLine();
                TextBox_Status.Text = line;
            }
            string readText = File.ReadAllText(outputFile);
            RichTextBox_Deidentified.Document.Blocks.Add(new Paragraph(new Run(readText)));
        }


        private void Click_OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string readText = "";
            if (openFileDialog.ShowDialog() == true)
            {
                RichTextBox_Original.Document.Blocks.Clear();
                readText = File.ReadAllText(openFileDialog.FileName);
                RichTextBox_Original.Document.Blocks.Add(new Paragraph(new Run(readText)));
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
                    // Get default value and convert to EXE path.
                    // It's stored as:
                    //    "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" -- "%1"
                    // So we want the first quoted string only
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
            string converted = @"R:\Temp\test--tokens.json";
            Process.Start(browserPath, converted);
        }

        private void Click_Clear(object sender, RoutedEventArgs e)
        {
            RichTextBox_Original.Document.Blocks.Clear();
            RichTextBox_Deidentified.Document.Blocks.Clear();
            TextBox_Status.Text = "";
        }

        private void Click_Copy(object sender, RoutedEventArgs e)
        {
            RichTextBox_Deidentified.SelectAll();
            RichTextBox_Deidentified.Copy();
        }
    }
}
