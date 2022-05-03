using EWMS.Models.Converters;
using EWMS.Models.Validation;
using System;
using System.Collections.Generic;
using System.IO;

namespace EWMS.Models.Containers
{
    internal class DataFile
    {

        public static DateTime GetTimestamp()
        {
            return DateTime.Now;
        }

        public static string GetLoadImagePath()
        {
            string filePath = string.Empty;

            Microsoft.Win32.OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = @"D:\",
                Title = "Browse Image Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "png",

                Filter = "Images (*.BMP;*.PNG;)|*.BMP;*.PNG;",

                FilterIndex = 1,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                filePath = openFileDialog.FileName;
            }

            return filePath;
        }

        public static string GetSaveImagePath()
        {
            string filePath = string.Empty;

            Microsoft.Win32.SaveFileDialog saveFileDialog = new()
            {

                InitialDirectory = @"D:\",
                Title = "Browse Image Files",

                CheckFileExists = false,
                CheckPathExists = true,
                AddExtension = true,

                DefaultExt = "png",

                Filter = "Images (*.BMP;*.PNG;)|*.BMP;*.PNG;|" +
                "All files (*.*)|*.*",

                FilterIndex = 1,
            };


            Nullable<bool> result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                filePath = saveFileDialog.FileName;
            }

            return filePath;
        }

        public static string GetLoadTextPath()
        {
            string filePath = string.Empty;

            Microsoft.Win32.OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = @"D:\",
                Title = "Browse Image Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",

                Filter = "txt files (*.txt)|*.txt",

                FilterIndex = 1,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                filePath = openFileDialog.FileName;
            }

            return filePath;
        }

        public static string GetSaveTextPath()
        {
            string filePath = string.Empty;

            Microsoft.Win32.SaveFileDialog saveFileDialog = new()
            {

                InitialDirectory = @"D:\",
                Title = "Browse Image Files",

                CheckFileExists = false,
                CheckPathExists = true,
                AddExtension = true,

                DefaultExt = "txt",

                Filter = "txt files(*.txt) | *.txt",

                FilterIndex = 1,
            };


            Nullable<bool> result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                filePath = saveFileDialog.FileName;
            }

            return filePath;
        }

        public static string GetLoadFilePathEncritpion()
        {
            string filePath = string.Empty;

            Microsoft.Win32.OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = @"D:\",
                Title = "Browse Image Files",

                CheckFileExists = true,
                CheckPathExists = true,

                Filter = "All files (*.*)|*.*",

                FilterIndex = 1,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                filePath = openFileDialog.FileName;
            }

            return filePath;
        }

        public static string GetLoadFilePathDecritpion()
        {
            string filePath = string.Empty;

            Microsoft.Win32.OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = @"D:\",
                Title = "Browse Image Files",

                CheckFileExists = true,
                CheckPathExists = true,

                Filter = "EWMS files (*.ewms)|*.ewms",

                FilterIndex = 1,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                filePath = openFileDialog.FileName;
            }

            return filePath;
        }

        public static string GetSaveFilePathEncryption()
        {
            string filePath = string.Empty;

            Microsoft.Win32.SaveFileDialog saveFileDialog = new()
            {

                InitialDirectory = @"D:\",
                Title = "Browse Image Files",

                CheckFileExists = false,
                CheckPathExists = true,
                AddExtension = true,

                DefaultExt = "txt",

                Filter = "EWMS files (*.ewms)|*.ewms",

                FilterIndex = 1,
            };


            Nullable<bool> result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                filePath = saveFileDialog.FileName;
            }

            return filePath;
        }

        public static string GetSaveFilePathDecryption(string extention)
        {
            string filePath = string.Empty;

            Microsoft.Win32.SaveFileDialog saveFileDialog = new()
            {

                InitialDirectory = @"D:\",
                Title = "Browse Image Files",

                CheckFileExists = false,
                CheckPathExists = true,
                AddExtension = true,

                DefaultExt = extention,

                Filter = "Files (*" + extention + ")|*" + extention,

                FilterIndex = 1,
            };


            Nullable<bool> result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                filePath = saveFileDialog.FileName;
            }

            return filePath;
        }

        public static List<byte> LoadFile(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);

            return ByteListByteArrayConverter.ByteArrayToByteList(bytes);
        }

        public static List<byte> LoadText(string path)
        {
            string output = ValidateToByteRange.ValidateTextToString(File.ReadAllText(path));

            return StringByteListConverter.StringToByteList(output);
        }

        public static void WriteFile(string path, List<byte> data)
        {
            File.WriteAllBytes(path, ByteListByteArrayConverter.ByteListToByteArray(data));
        }

        public static void WriteText(string path, List<byte> data)
        {
            File.WriteAllText(path, StringByteListConverter.ByteListToString(data));
        }

        public static List<byte> LoadFileForEncryption(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);

            string header = GetInformationAboutFileToEncryption(path);

            List<byte> dataToEncrypt = StringByteListConverter.StringToByteList(header);

            dataToEncrypt.AddRange(ByteListByteArrayConverter.ByteArrayToByteList(bytes));

            return dataToEncrypt;
        }

        public static void WriteFileForEncryption(string path, List<byte> data)
        {
            File.WriteAllBytes(path, ByteListByteArrayConverter.ByteListToByteArray(data));
        }

        public static List<byte> LoadFileForDecryption(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);

            return ByteListByteArrayConverter.ByteArrayToByteList(bytes);
        }

        public static void WriteFileForDecryption(string path, List<byte> data)
        {
            File.WriteAllBytes(path, ByteListByteArrayConverter.ByteListToByteArray(data));
        }

        public static string GetInformationAboutFileToEncryption(string path)
        {
            string fullPath = Path.GetFullPath(path);
            string fileExtention = Path.GetExtension(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            DateTime timeOfCreationFile = File.GetCreationTime(path);
            DateTime timeOfLastAccessFile = File.GetLastAccessTime(path);
            DateTime timeOfLastWriteFile = File.GetLastWriteTime(path);

            FileInfo fileInfo = new(path);
            long fileSize = fileInfo.Length;

            string header = string.Empty;

            header += "[[ ";
            header += "FILE_NAME ";
            header += fileName;
            header += " PATH ";
            header += path;
            header += " FULL_PATH ";
            header += fullPath;
            header += " FILE_EXTENTION ";
            header += fileExtention;
            header += " FILE_SIZE ";
            header += fileSize.ToString();
            header += " TIME_OF_CREATION ";
            header += timeOfCreationFile.ToString();
            header += " TIME_OF_ACCESS ";
            header += timeOfLastAccessFile.ToString();
            header += " TIME_OF_WRITE ";
            header += timeOfLastWriteFile.ToString();
            header += " ]]";

            return header;
        }

        public static string GetInformationAboutFileToPrint(string path)
        {
            string fullPath = Path.GetFullPath(path);
            string fileExtention = Path.GetExtension(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            DateTime timeOfCreationFile = File.GetCreationTime(path);
            DateTime timeOfLastAccessFile = File.GetLastAccessTime(path);
            DateTime timeOfLastWriteFile = File.GetLastWriteTime(path);

            FileInfo fileInfo = new(path);
            long fileSize = fileInfo.Length;

            string info = string.Empty;

            info += "Information about file going to be encrypted:\n";

            info += "\nFile name: ";
            info += fileName;
            info += "\nPath: ";
            info += path;
            info += "\nFull path: ";
            info += fullPath;
            info += "\nFile extention: ";
            info += fileExtention;
            info += "\nFile size: ";
            info += fileSize.ToString() + "B";
            info += "\nTime of file creation: ";
            info += timeOfCreationFile.ToString();
            info += "\nTime of last access to file: ";
            info += timeOfLastAccessFile.ToString();
            info += "\nTime of last write operation to file: ";
            info += timeOfLastWriteFile.ToString();

            return info;
        }

        public static string InfoWhereFileWasSavedAfterEncryption(string path)
        {
            string fullPath = Path.GetFullPath(path);

            string info = string.Empty;

            info += "File saved successfully after encryption at:\n";
            info += fullPath + "\n\n";

            return info;
        }

        public static string InfoWhereFileWasSavedAfterDecryption(string path)
        {
            string fullPath = Path.GetFullPath(path);

            string info = string.Empty;

            info += "File saved successfully after decryption at:\n";
            info += fullPath + "\n\n";

            return info;
        }

        public static string InfoWhereLoadedDecryptionFileWas(string path)
        {
            string fullPath = Path.GetFullPath(path);

            string info = string.Empty;

            info += "File to decryption loaded successfully from:\n";
            info += fullPath + "\n\n";

            return info;
        }

        public static string GetDecryptedFileExtention(List<byte> data)
        {
            string dataAsText = StringByteListConverter.ByteListToString(data);

            int firstIndex = 3;
            int lastIndex = dataAsText.IndexOf("]]") - 1;

            int informationLength = lastIndex - firstIndex;

            string header = dataAsText.Substring(firstIndex, informationLength);

            string[] splitedHeader = header.Split(' ');

            for (int i = 0; i < splitedHeader.Length; i++)
            {

                if (splitedHeader[i].Equals("FILE_EXTENTION"))
                {
                    return splitedHeader[i + 1];
                }
            }

            return string.Empty;
        }

        public static bool CheckDecryptionQuality(List<byte> data)
        {
            string dataAsText = StringByteListConverter.ByteListToString(data);

            if (dataAsText.IndexOf("[[") == 0 && dataAsText.IndexOf("]]") != -1)
            {

                int firstIndex = 3;
                int lastIndex = dataAsText.IndexOf("]]") - 1;

                int informationLength = lastIndex - firstIndex;

                string header = dataAsText.Substring(firstIndex, informationLength);

                string[] splitedHeader = header.Split(' ');

                int counter = 0;

                for (int i = 0; i < splitedHeader.Length; i++)
                {
                    if (splitedHeader[i].Equals("FILE_NAME"))
                    {
                        counter++;
                    }
                    else if (splitedHeader[i].Equals("PATH"))
                    {
                        counter++;
                    }
                    else if (splitedHeader[i].Equals("FULL_PATH"))
                    {
                        counter++;
                    }
                    else if (splitedHeader[i].Equals("FILE_EXTENTION"))
                    {
                        counter++;
                    }
                    else if (splitedHeader[i].Equals("FILE_SIZE"))
                    {
                        counter++;
                    }
                    else if (splitedHeader[i].Equals("TIME_OF_CREATION"))
                    {
                        counter++;
                    }
                    else if (splitedHeader[i].Equals("TIME_OF_ACCESS"))
                    {
                        counter++;
                    }
                    else if (splitedHeader[i].Equals("TIME_OF_WRITE"))
                    {
                        counter++;
                    }
                }

                if (counter == 8)
                {
                    return true;
                }

            }

            return false;
        }

        public static List<byte> ExtractDataFromDecryptedFile(List<byte> data)
        {
            string dataAsText = StringByteListConverter.ByteListToString(data);

            int index = dataAsText.IndexOf("]]") + 2;

            data.RemoveRange(0, index);

            return data;
        }

        public static string GetInformationAboutFileFromDecryption(List<byte> data)
        {
            string dataAsText = StringByteListConverter.ByteListToString(data);

            int firstIndex = 3;
            int lastIndex = dataAsText.IndexOf("]]") - 1;

            int informationLength = lastIndex - firstIndex;

            string header = dataAsText.Substring(firstIndex, informationLength);

            string[] splitedHeader = header.Split(' ');

            string info = string.Empty;

            info += "Information about decrypted file:\n\n";

            for (int i = 0; i < splitedHeader.Length; i++)
            {
                if (splitedHeader[i].Equals("FILE_NAME"))
                {
                    info += "File name: ";
                    info += splitedHeader[i + 1];
                }
                else if (splitedHeader[i].Equals("PATH"))
                {
                    info += "\nPath: ";
                    info += splitedHeader[i + 1];
                }
                else if (splitedHeader[i].Equals("FULL_PATH"))
                {
                    info += "\nFull path: ";
                    info += splitedHeader[i + 1];
                }
                else if (splitedHeader[i].Equals("FILE_EXTENTION"))
                {
                    info += "\nFile extention: ";
                    info += splitedHeader[i + 1];
                }
                else if (splitedHeader[i].Equals("FILE_SIZE"))
                {
                    info += "\nFile size: ";
                    info += splitedHeader[i + 1] + "B";
                }
                else if (splitedHeader[i].Equals("TIME_OF_CREATION"))
                {
                    info += "\nTime of file creation: ";
                    info += splitedHeader[i + 1] + " " + splitedHeader[i + 2];
                }
                else if (splitedHeader[i].Equals("TIME_OF_ACCESS"))
                {
                    info += "\nTime of last access to file: ";
                    info += splitedHeader[i + 1] + " " + splitedHeader[i + 2];
                }
                else if (splitedHeader[i].Equals("TIME_OF_WRITE"))
                {
                    info += "\nTime of last write operation to file: ";
                    info += splitedHeader[i + 1] + " " + splitedHeader[i + 2] + "\n\n";
                }
            }

            return info;
        }

    }
}
