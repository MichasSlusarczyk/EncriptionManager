using System.IO;


namespace EWMS.Models.Validation
{
    internal class ValidateFileSize
    {
        public static bool ValidateSizeOfFile(string path, int maxSize)
        {
            FileInfo fileInfo = new(path);

            if (fileInfo.Length <= maxSize)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
