using EWMS.Models.PasswordManagerFolder;

namespace EWMS.Models.Validation
{
    internal class ValidatePasswordManager
    {
        public static string ValidateGeneratePassword(PasswordManager passwordManager)
        {
            string message = string.Empty;

            if (passwordManager.Password.Length == 0)
            {
                message += "Password generation length value is zero!";
            }

            if (!passwordManager.CheckGeneratedPasswordLength())
            {
                message += "Password generation length is over maximum!";
            }

            return message;
        }

        public static string ValidateGenerateSalt(PasswordManager passwordManager)
        {
            string message = string.Empty;

            if (passwordManager.PasswordSalt.Length == 0)
            {
                message += "Salt generation length value is zero!";
            }

            if (!passwordManager.CheckGeneratedSaltLength())
            {
                message += "Password generation length is over maximum!";
            }

            return message;
        }

        public static string ValidateGenerateHash(PasswordManager passwordManager)
        {
            string message = string.Empty;

            if (passwordManager.Password.Length == 0)
            {
                message += "Password generation length value is zero!\n";
            }
            if (passwordManager.PasswordParameters.HashType == HashType.None)
            {
                message += "Hash type is not choosed yet!";
            }

            return message;
        }
    }
}
