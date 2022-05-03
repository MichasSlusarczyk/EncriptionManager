using EWMS.Models.Converters;
using System.Collections.Generic;

namespace EWMS.Models.Containers
{
    internal class Data
    {
        private List<byte> _dataOriginal;
        private List<byte> _dataModified;
        private TextParameters _textParam;

        public List<byte> DataOriginal
        {
            get => _dataOriginal;

            set
            {
                _dataOriginal = value;
                OverriteDataModified();
            }
        }

        public List<byte> DataModified { get => _dataModified; set => _dataModified = value; }

        public Data()
        {
            _textParam = new TextParameters();
            _dataOriginal = new();
            _dataModified = new();
        }

        public void OverriteDataModified()
        {
            DataModified = DataOriginal;
        }

        public void UpdateParam(TextParameters param)
        {
            _textParam = param;
        }

        public void ModifyText()
        {
            string text = StringByteListConverter.ByteListToString(DataModified);

            if (_textParam.Uppercase == true)
            {
                text = DoUppercase(text);
            }

            if (_textParam.Lowercase == true)
            {
                text = DoLowercase(text);
            }

            if (_textParam.Symbols == true)
            {
                text = DeleteSymbols(text);
            }

            if (_textParam.AmbiguousSigns == true)
            {
                text = DeleteAmbiguousSigns(text);
            }

            if (_textParam.Numbers == true)
            {
                text = DeleteNumbers(text);
            }

            if (_textParam.Letters == true)
            {
                text = DeleteLetters(text);
            }

            if (_textParam.Space == true)
            {
                text = DeleteSpace(text);
            }

            if (_textParam.OtherCharacters == true)
            {
                text = DeleteOtherCharacters(text);
            }

            DataModified = StringByteListConverter.StringToByteList(text);
        }

        public bool CheckIfDataIsModified()
        {
            if (_textParam.Uppercase == true)
            {
                return true;
            }

            if (_textParam.Lowercase == true)
            {
                return true;
            }

            if (_textParam.Symbols == true)
            {
                return true;
            }

            if (_textParam.AmbiguousSigns == true)
            {
                return true;
            }

            if (_textParam.Numbers == true)
            {
                return true;
            }

            if (_textParam.Letters == true)
            {
                return true;
            }

            if (_textParam.Space == true)
            {
                return true;
            }

            if (_textParam.OtherCharacters == true)
            {
                return true;
            }

            return false;
        }

        private static string DoUppercase(string text)
        {
            return text.ToUpper();
        }

        private static string DoLowercase(string text)
        {
            return text.ToLower();
        }

        private static string DeleteSymbols(string text)
        {
            string signs = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

            foreach (char c in text)
            {
                if (signs.IndexOf(c) != -1)
                {
                    text = text.Remove(text.IndexOf(c), 1);
                }
            }
            return text;
        }

        private static string DeleteAmbiguousSigns(string text)
        {
            string signs = "{}[]()/\\\'\"`~,;:.<>";

            foreach (char c in text)
            {
                if (signs.IndexOf(c) != -1)
                {
                    text = text.Remove(text.IndexOf(c), 1);
                }
            }
            return text;
        }

        private static string DeleteNumbers(string text)
        {
            foreach (char c in text)
            {
                if (c >= '0' && c <= '9')
                {
                    text = text.Remove(text.IndexOf(c), 1);
                }
            }
            return text;
        }

        private static string DeleteLetters(string text)
        {
            foreach (char c in text)
            {
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    text = text.Remove(text.IndexOf(c), 1);
                }
            }
            return text;
        }

        private static string DeleteSpace(string text)
        {
            foreach (char c in text)
            {
                if (c == ' ')
                {
                    text = text.Remove(text.IndexOf(c), 1);
                }
            }
            return text;
        }

        private static string DeleteOtherCharacters(string text)
        {
            foreach (char c in text)
            {
                if (!(c >= ' ' && c <= '~'))
                {
                    text = text.Remove(text.IndexOf(c), 1);
                }
            }
            return text;
        }
    }

    internal class TextParameters
    {
        private bool _uppercase;
        private bool _lowercase;
        private bool _whiteSigns;
        private bool _symbols;
        private bool _ambiguousSigns;
        private bool _numbers;
        private bool _letters;
        private bool _space;
        private bool _otherCharacters;

        public TextParameters()
        {
            Uppercase = false;
            Lowercase = false;
            WhiteSigns = false;
            Symbols = false;
            AmbiguousSigns = false;
            Numbers = false;
            Letters = false;
            Space = false;
            OtherCharacters = false;
        }

        public bool Uppercase { get => _uppercase; set => _uppercase = value; }
        public bool Lowercase { get => _lowercase; set => _lowercase = value; }
        public bool WhiteSigns { get => _whiteSigns; set => _whiteSigns = value; }
        public bool Symbols { get => _symbols; set => _symbols = value; }
        public bool AmbiguousSigns { get => _ambiguousSigns; set => _ambiguousSigns = value; }
        public bool Numbers { get => _numbers; set => _numbers = value; }
        public bool Letters { get => _letters; set => _letters = value; }
        public bool Space { get => _space; set => _space = value; }
        public bool OtherCharacters { get => _otherCharacters; set => _otherCharacters = value; }
    }
}
