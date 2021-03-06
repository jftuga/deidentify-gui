using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace deidentify_gui
{
    class Auxiliary
    {
        public static string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );

            return textRange.Text;
        }

        // https://www.codeproject.com/Tips/1273917/Opening-a-File-Specifically-in-the-Default-Browser
        public static string GetPathToDefaultBrowser()
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

        public static void FileDeidentify(string replacementText, string outputFile, string inputFile, Label label)
        {
            // FIXME - When entering text (and not using Open), this button must be pressed twice
            string filename = @"python";
            string args = string.Format(@"deidentify.py -H -r {0} -o {1} {2}", replacementText, outputFile, inputFile);

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

            proc.Start();
            string line = "";
            while (!proc.StandardError.EndOfStream)
            {
                line = proc.StandardError.ReadLine();
                label.Content = "Status: " + line;
            }
        }
        public static string SetJSONFilename(string someFile)
        {
            string fileNameWithoutExtension = someFile;
            int i = someFile.LastIndexOf(".");

            if (i > 0)
            {
                fileNameWithoutExtension = someFile.Substring(0, i);
            }
            string jsonFile = fileNameWithoutExtension + "--tokens.json";
            return jsonFile;
        }

        public static void DeleteTempFiles(string tempDir)
        {
            var allFiles = Directory.GetFiles(tempDir);
            foreach( string fname in allFiles)
            {
                var basename = Path.GetFileName(fname);
                if (basename.StartsWith("default--input"))
                {
                    File.Delete(fname);
                    continue;
                }
                if (basename.StartsWith("default--output")) {
                    File.Delete(fname);
                    continue;
                }
                if (basename.EndsWith("--tokens.json"))
                {
                    File.Delete(fname);
                    continue;
                }
            }
        }
    }
}
