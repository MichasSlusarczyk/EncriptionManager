using EWMS.Models.Converters;
using System;
using System.Security.Cryptography;

namespace EWMS.Models.PasswordManagerFolder
{
    internal class PasswordManager
    {
        private string _password;
        private string _passwordHash;
        private string _passwordSalt;
        private PasswordManagerParameters _passwordParameters;

        public PasswordManager()
        {
            _password = string.Empty;
            _passwordHash = string.Empty;
            _passwordSalt = string.Empty;
            _passwordParameters = new();
        }

        public string Password { get => _password; set => _password = value; }
        public string PasswordHash { get => _passwordHash; set => _passwordHash = value; }

        public string PasswordSalt { get => _passwordSalt; set => _passwordSalt = value; }
        internal PasswordManagerParameters PasswordParameters { get => _passwordParameters; set => _passwordParameters = value; }

        public void GeneratePassword()
        {
            string signsusedToPassword = UpdatePossiblePasswordCharacters();
            int signsusedToPasswordLength = signsusedToPassword.Length;

            string tempPassword = string.Empty;

            if (signsusedToPasswordLength != 0)
            {
                Random rand = new();

                for (int i = 0; i < _passwordParameters.PasswordLength; i++)
                {
                    tempPassword += signsusedToPassword[rand.Next(0, signsusedToPasswordLength)];
                }
            }

            _password = tempPassword;
        }

        public void GenerateSalt()
        {
            string salt = string.Empty;

            Random rand = new();
            for (int i = 0; i < _passwordParameters.SaltLength; i++)
            {
                salt += (char)rand.Next(256);
            }

            PasswordSalt = salt;
        }


        public void GenerateHash()
        {
            if (_password != string.Empty)
            {
                string passwordCopy = _password;
                if (_passwordParameters.AddSalt && PasswordSalt != string.Empty)
                {
                    passwordCopy += PasswordSalt;
                }

                HashAlgorithm hashAlgorithm = null;

                if (_passwordParameters.HashType == HashType.MD5)
                {
                    hashAlgorithm = MD5.Create();
                }
                else if (_passwordParameters.HashType == HashType.SHA1)
                {
                    hashAlgorithm = SHA1.Create();
                }
                else if (_passwordParameters.HashType == HashType.SHA256)
                {
                    hashAlgorithm = SHA256.Create();
                }
                else if (_passwordParameters.HashType == HashType.SHA384)
                {
                    hashAlgorithm = SHA384.Create();
                }
                else if (_passwordParameters.HashType == HashType.SHA512)
                {
                    hashAlgorithm = SHA512.Create();
                }

                if (hashAlgorithm != null)
                {
                    byte[] hash = hashAlgorithm.ComputeHash(ByteArrayToStringConverter.StringToByteArray(passwordCopy));
                    _passwordHash = ByteArrayToStringConverter.ByteArrayToString(hash);
                }
            }
        }

        public bool CheckGeneratedPasswordLength()
        {
            if (_passwordParameters.PasswordLength <= _passwordParameters.PasswordMaxLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckGeneratedSaltLength()
        {
            if (_passwordParameters.SaltLength <= _passwordParameters.SaltMaxLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string UpdatePossiblePasswordCharacters()
        {
            string signsUsedToPassword = string.Empty;

            if (_passwordParameters.UppercaseLetters)
            {
                string signs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                signsUsedToPassword += signs;
            }

            if (_passwordParameters.LowercaseLetters)
            {
                string signs = "abcdefghijklmnopqrstuvwxyz";

                signsUsedToPassword += signs;
            }

            if (_passwordParameters.Numbers)
            {
                string signs = "0123456789";

                signsUsedToPassword += signs;
            }

            if (_passwordParameters.Symbols)
            {
                string signs = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

                signsUsedToPassword += signs;
            }

            if (!_passwordParameters.SimilarCharacters)
            {
                string signs = "O0oilL1UuVv";

                foreach (char c in signsUsedToPassword)
                {
                    if (signs.IndexOf(c) != -1)
                    {
                        signsUsedToPassword = signsUsedToPassword.Remove(signsUsedToPassword.IndexOf(c),1);
                    }
                }
                return signsUsedToPassword;
            }

            if (!_passwordParameters.AmbiguousCharacters)
            {
                string signs = "{}[]()/\\\'\"`~,;:.<>";

                foreach (char c in signsUsedToPassword)
                {
                    if (signs.IndexOf(c) != -1)
                    {
                        signsUsedToPassword = signsUsedToPassword.Remove(signsUsedToPassword.IndexOf(c),1);
                    }
                }
                return signsUsedToPassword;
            }

            return signsUsedToPassword;
        }
    }

    internal class PasswordManagerParameters
    {
        private int _passwordLength;
        private int _passwordMaxLength;
        private bool _uppercaseLetters;
        private bool _lowercaseLetters;
        private bool _numbers;
        private bool _symbols;
        private bool _similarCharacters;
        private bool _ambiguousCharacters;
        private HashType _hashType;
        private bool _addSalt;
        private int _saltLength;
        private int _saltMaxLength;

        public PasswordManagerParameters()
        {
            _passwordMaxLength = 100;
            _passwordLength = 20;

            _uppercaseLetters = true;
            _lowercaseLetters = true;
            _numbers = true;
            _symbols = true;
            _similarCharacters = true;
            _ambiguousCharacters = true;

            _hashType = HashType.None;
            _addSalt = false;
            _saltLength = _passwordLength;
            _saltMaxLength = _passwordMaxLength;
        }

        public int PasswordLength { get => _passwordLength; set => _passwordLength = value; }
        public int PasswordMaxLength { get => _passwordMaxLength; set => _passwordMaxLength = value; }
        public bool Numbers { get => _numbers; set => _numbers = value; }
        public bool Symbols { get => _symbols; set => _symbols = value; }
        public bool SimilarCharacters { get => _similarCharacters; set => _similarCharacters = value; }
        public bool AmbiguousCharacters { get => _ambiguousCharacters; set => _ambiguousCharacters = value; }
        public bool AddSalt { get => _addSalt; set => _addSalt = value; }
        public int SaltLength { get => _saltLength; set => _saltLength = value; }
        public int SaltMaxLength { get => _saltMaxLength; set => _saltMaxLength = value; }
        public bool UppercaseLetters { get => _uppercaseLetters; set => _uppercaseLetters = value; }
        public bool LowercaseLetters { get => _lowercaseLetters; set => _lowercaseLetters = value; }
        internal HashType HashType { get => _hashType; set => _hashType = value; }
    }
}
