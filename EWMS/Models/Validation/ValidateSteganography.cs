using EWMS.Models.SteganographyFolder;

namespace EWMS.Models.Validation
{
    internal class ValidateSteganography
    {
        public static string ValidateDoSteganographyEncryption(Steganography steganography)
        {
            string message = string.Empty;

            if (steganography.InputImgE.Size == 0)
            {
                message += "Input image not loaded!\n";
            }

            if (steganography.MessageDataE.DataModified.Count == 0)
            {
                message += "Input message not loaded!\n";
            }

            if (!steganography.CheckIfTheHideDataFit())
            {
                message += "Message length greater than space to hide data!";
            }

            return message;
        }

        public static string ValidateDoSteganographyDecryption(Steganography steganography)
        {
            string message = string.Empty;

            if (steganography.InputImgD.Size == 0)
            {
                message += "Input image not loaded!\n";
            }

            if (!steganography.CheckIfTheRevealDataFit())
            {
                message += "The length of the message to be displayed is greater than data length!\n";
            }

            if (steganography.Param.DecryptionDisplayLength == 0)
            {
                message += "The length of the message to be displayed is zero!";
            }

            return message;
        }
    }
}
