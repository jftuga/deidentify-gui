using HtmlAgilityPack;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
        private string tempDir;
        public MainWindow()
        {
            InitializeComponent();
            ResetDefaults();
            Auxiliary.DeleteTempFiles(this.tempDir);
        }

        private void ResetDefaults()
        {
            string temp = (Environment.GetEnvironmentVariable("TEMP").Length > 0) ? Environment.GetEnvironmentVariable("TEMP") : (Environment.GetEnvironmentVariable("TMP").Length > 0) ? Environment.GetEnvironmentVariable("TMP") : ".";
            temp += Path.DirectorySeparatorChar;
            this.tempDir = temp;
            this.inputFile = temp + "default--input.txt";
            this.outputFile = temp + "default--output.htm";
            this.jsonFile = temp;
            this.usingDefaultFilenames = true;
        }

        private void SetAllFilename()
        {
            string fileNameWithoutExtension = this.inputFile;

            if (this.usingDefaultFilenames == false)
            {
                int i = this.inputFile.LastIndexOf(".");

                if (i > 0)
                {
                    fileNameWithoutExtension = this.inputFile.Substring(0, i);
                }
                this.jsonFile = fileNameWithoutExtension + "--tokens.json";
                this.outputFile = fileNameWithoutExtension + "--deidentified" + ".htm";
            }
        }

        private void Click_Close(object sender, RoutedEventArgs e)
        {
            Auxiliary.DeleteTempFiles(this.tempDir);
            Close();
        }

        // update status begin
        // https://stackoverflow.com/a/10715059/452281
        public delegate void UpdateTextCallback(string message);

        private void UpdateStatus(string message)
        {
            Label_Status.Content = message;
        }

        private void UpdateThread()
        {
            string message = "Status: Starting deidentification...";
            this.Dispatcher.Invoke(
                new UpdateTextCallback(this.UpdateStatus),
                new object[] { message.ToString() }
            );
        }
        // update status end

        private void Click_Deidentify(object sender, RoutedEventArgs e)
        {
            // status is not working, can someone please help me fix this?
            //Thread updaterTh = new Thread(new ThreadStart(UpdateThread));
            //updaterTh.Start();

            try
            {
                string tempFile = Path.GetTempFileName();
                File.WriteAllText(tempFile, Auxiliary.StringFromRichTextBox(RichTextBox_Original));
                Auxiliary.FileDeidentify(TextBox_Replacement.Text, this.outputFile, tempFile, Label_Status);
                File.Delete(tempFile);
                this.jsonFile = Auxiliary.SetJSONFilename(tempFile);
                myBrowser.Navigate(this.outputFile);
            } catch( Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception #18293");
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
            ApplicationEnvironment app = PlatformServices.Default.Application;
            string version = app.ApplicationVersion;
            MessageBox.Show("Deidentify GUI" + "\n" + version + "\n\n" + "https://github.com/jftuga/deidentify-gui" + "\n" + "https://github.com/jftuga/deidentify", "Deidentify");
        }

        private void Click_Debug(object sender, RoutedEventArgs e)
        {
            string browserPath = Auxiliary.GetPathToDefaultBrowser();
            Process.Start(browserPath, this.jsonFile);
        }

        private void Click_Clear(object sender, RoutedEventArgs e)
        {
            RichTextBox_Original.Document.Blocks.Clear();
            myBrowser.Navigate(@"about:blank");
            Label_Status.Content = "Status:";
            ResetDefaults();
        }

        private void Click_Copy(object sender, RoutedEventArgs e)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(this.outputFile);
            var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//body");

            if(htmlNodes.Count() > 0)
            {
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, htmlNodes[0].InnerText), true);

            }
        }

        private void Click_Into_Original(object sender, MouseButtonEventArgs e)
        {
            string o = Auxiliary.StringFromRichTextBox(RichTextBox_Original);
            if ( o.StartsWith("Original Text") )
            {
                RichTextBox_Original.Document.Blocks.Clear();
            }
        }

        private void Click_Save(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("The SAVE function has not been implemented yet." + "\n" + "Instead, use COPY to copy the deidentifed text to the clipboard.", "Deidentify");
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            Auxiliary.DeleteTempFiles(this.tempDir);
        }
    }
}
