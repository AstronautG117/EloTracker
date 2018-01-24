using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloTracker.Utilities
{
    public static class CSharpUtilities
    {
        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        {
            var sortableList = new List<T>(collection);
            sortableList.Sort(comparison);

            for (int i = 0; i < sortableList.Count; i++)
            {
                collection.Move(collection.IndexOf(sortableList[i]), i);
            }
        }

        public static bool IsFileLocked(string filePath)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (!File.Exists(filePath))
            {
                return false;
            }

            FileInfo file = new FileInfo(filePath);

            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        public static string GetSavePath(FileDialogParameters fileParams, string defaultFileName, string defaultDirectory,
            string initialDirectory = null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (initialDirectory != null)
            {
                saveFileDialog.InitialDirectory = initialDirectory;
            }
            else
            {
                saveFileDialog.InitialDirectory = defaultDirectory;
            }
            saveFileDialog.FileName = defaultFileName;
            saveFileDialog.Filter = fileParams.Filter;
            saveFileDialog.DefaultExt = fileParams.DefaultExtension;
            saveFileDialog.AddExtension = true;

            string savePath = null;
            if (saveFileDialog.ShowDialog() == true)
            {
                savePath = saveFileDialog.FileName;
            }
            return savePath;
        }
        public static string GetLoadPath(FileDialogParameters fileParams, string defaultDirectory, string initialDirectory = null)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (initialDirectory != null)
            {
                openFileDialog.InitialDirectory = initialDirectory;
            }
            else
            {
                openFileDialog.InitialDirectory = defaultDirectory;
            }
            openFileDialog.Filter = fileParams.Filter;
            openFileDialog.DefaultExt = fileParams.DefaultExtension;
            openFileDialog.AddExtension = true;

            string savePath = null;
            if (openFileDialog.ShowDialog() == true)
            {
                savePath = openFileDialog.FileName;
            }
            return savePath;
        }

        public struct FileDialogParameters
        {
            public string Filter;
            public string DefaultExtension;

            #region File Parameters
            public static FileDialogParameters ExcelFileParameters
            {
                get
                {
                    FileDialogParameters fileParams;
                    fileParams.Filter = "Excel files (*.xlsx)|*.xlsx";
                    fileParams.DefaultExtension = "xlsx";
                    return fileParams;
                }
            }
            public static FileDialogParameters EloFileParameters
            {
                get
                {
                    FileDialogParameters fileParams;
                    fileParams.Filter = "Elo Files (*.elo)|*.elo" + "|All Files (*.*)|*.*";
                    fileParams.DefaultExtension = "elo";
                    return fileParams;
                }
            }
            #endregion
        }
    }
}
