using EWMS.Models.Containers;
using EWMS.Models.Converters;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EWMS.Models.SteganographyFolder
{
    internal class Steganography
    {
        private EncryptDecrypt _enDe;
        private string _description;
        private SteganographyParameters _param;

        private Img _inputImgE;
        private Data _messageDataE;
        private Img _outputImgE;

        private Img _inputImgD;
        private Data _messageDataD;

        internal EncryptDecrypt EnDe { get => _enDe; set => _enDe = value; }
        internal SteganographyParameters Param { get => _param; set => _param = value; }

        public Img OutputImgE { get => _outputImgE; set => _outputImgE = value; }
        internal Data MessageDataD { get => _messageDataD; set => _messageDataD = value; }
        internal Img InputImgE { get => _inputImgE; set => _inputImgE = value; }
        internal Data MessageDataE { get => _messageDataE; set => _messageDataE = value; }
        internal Img InputImgD { get => _inputImgD; set => _inputImgD = value; }
        public string Description { get => _description; set => _description = value; }

        public Steganography()
        {
            _enDe = EncryptDecrypt.None;
            _messageDataE = new();
            _messageDataD = new();
            _inputImgE = new();
            _inputImgD = new();
            _outputImgE = new();
            _param = new();
            FillDescription();
        }

        public Steganography(EncryptDecrypt type)
        {
            _enDe = type;
            _messageDataE = new();
            _messageDataD = new();
            _inputImgE = new();
            _inputImgD = new();
            _outputImgE = new();
            _param = new();
            FillDescription();

        }

        private void FillDescription()
        {
            _description =
                "SHORT DESCRIPTION:\n" +
                "Implementation of an algorithm based on the least bit method which consists in hiding bits of information in the least significant bits of each pixel of the image. This method is part of the science known as steganography. This algorithm has been developed with the possibility of choosing on which bits the message is to be hidden and how many data matrices from the RGB system are to take part in it.";
        }

        public void Concealment()
        {
            if (EnDe == EncryptDecrypt.Encryption)
            {
                Hide();
            }
            else if (EnDe == EncryptDecrypt.Decryption)
            {
                Reveal();
            }
        }

        private void Hide()
        {
            _outputImgE = DataHide();
        }

        private void Reveal()
        {
            _messageDataD = DataReveal();
        }

        private Img DataHide()
        {
            Img tempImg = _inputImgE;

            List<bool> bits = ByteListBitListConverter.ByteListToBitList(_messageDataE.DataModified);
            int numOfBits = 0;
            for (int b = 0; b < _param.BitsToUse.Length; b++)
            {
                if (_param.BitsToUse[b] == true)
                {
                    numOfBits++;
                }
            }

            int numOfDims = 0;
            for (int d = 0; d < _param.DimToUse.Length; d++)
            {
                if (_param.DimToUse[d] == true)
                {
                    numOfDims++;
                }
            }

            int pos;
            if (tempImg.ColorType == WBColor.None)
            {
                return new();
            }
            else if (tempImg.ColorType == WBColor.WB)
            {
                int bitCounter = 0;

                while (bitCounter < bits.Count)
                {
                    byte[,] tempMatrix = tempImg.R;

                    pos = (int)Math.Floor((double)(bitCounter / numOfBits / numOfDims));

                    ModifyBitsInPixels(tempMatrix, bits, pos, ref bitCounter);

                    tempImg.R = tempMatrix;
                }

                tempImg.G = tempImg.R;
                tempImg.B = tempImg.R;
            }
            else if (tempImg.ColorType == WBColor.Color)
            {
                int bitCounter = 0;

                while (bitCounter < bits.Count)
                {
                    for (int m = 0; m < _param.DimToUse.Length; m++)
                    {
                        if (_param.DimToUse[m] == true)
                        {
                            byte[,] tempMatrix = new byte[tempImg.Width, tempImg.Height];

                            if (m == 0)
                            {
                                tempMatrix = tempImg.R;
                            }
                            else if (m == 1)
                            {
                                tempMatrix = tempImg.G;
                            }
                            else if (m == 2)
                            {
                                tempMatrix = tempImg.B;
                            }

                            pos = (int)Math.Floor((double)(bitCounter / numOfBits / numOfDims));

                            ModifyBitsInPixels(tempMatrix, bits, pos, ref bitCounter);

                            if (m == 0)
                            {
                                tempImg.R = tempMatrix;
                            }
                            else if (m == 1)
                            {
                                tempImg.G = tempMatrix;
                            }
                            else if (m == 2)
                            {
                                tempImg.B = tempMatrix;
                            }
                        }
                    }
                }
            }

            return tempImg;
        }

        private Data DataReveal()
        {
            Img tempImg = _inputImgD;
            Data tempData = _messageDataD;

            List<bool> tempBitArrayList = new();
            int byteCounter = 0;

            if (tempImg.ColorType == WBColor.None)
            {
                return new();
            }
            else if (tempImg.ColorType == WBColor.WB)
            {
                for (int w = 0; w < tempImg.Width; w++)
                {
                    for (int h = 0; h < tempImg.Height; h++)
                    {
                        byte bytee = tempImg.R[w, h];
                        byte[] by = new byte[1];
                        by[0] = bytee;
                        BitArray bitArray = new(by);

                        for (int b = 0; b < _param.BitsToUse.Length; b++)
                        {
                            if (_param.BitsToUse[b] == true)
                            {
                                tempBitArrayList.Add(bitArray[b]);

                                if (tempBitArrayList.Count % 8 == 0)
                                {
                                    byteCounter++;
                                }

                                if (byteCounter == Param.DecryptionDisplayLength)
                                {
                                    b = _param.BitsToUse.Length;
                                    w = tempImg.Width;
                                    h = tempImg.Height;
                                }
                            }

                        }

                    }
                }
            }
            else if (tempImg.ColorType == WBColor.Color)
            {

                for (int w = 0; w < tempImg.Width; w++)
                {
                    for (int h = 0; h < tempImg.Height; h++)
                    {
                        for (int m = 0; m < _param.DimToUse.Length; m++)
                        {
                            if (_param.DimToUse[m] == true)
                            {
                                byte bytee = 0;

                                if (m == 0)
                                {
                                    bytee = tempImg.R[w, h];
                                }
                                else if (m == 1)
                                {
                                    bytee = tempImg.G[w, h];
                                }
                                else if (m == 2)
                                {
                                    bytee = tempImg.B[w, h];
                                }

                                byte[] by = new byte[1];
                                by[0] = bytee;
                                BitArray bitArray = new(by);

                                for (int b = 0; b < _param.BitsToUse.Length; b++)
                                {
                                    if (_param.BitsToUse[b] == true)
                                    {
                                        tempBitArrayList.Add(bitArray[b]);

                                        if (tempBitArrayList.Count % 8 == 0)
                                        {
                                            byteCounter++;
                                        }

                                        if (byteCounter == Param.DecryptionDisplayLength)
                                        {
                                            b = _param.BitsToUse.Length;
                                            m = _param.DimToUse.Length;
                                            w = tempImg.Width;
                                            h = tempImg.Height;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }

            tempData.DataModified = ByteListBitListConverter.BitListToByteList(tempBitArrayList);

            return tempData;
        }
        public void CalculatePossibleSpaceToHide()
        {
            int matrixCounter = 0;

            for (int i = 0; i < _param.DimToUse.Length; i++)
            {
                if (_param.DimToUse[i] == true)
                {
                    matrixCounter++;
                }
            }

            int bitCounter = 0;
            for (int i = 0; i < _param.BitsToUse.Length; i++)
            {
                if (_param.BitsToUse[i] == true)
                {
                    bitCounter++;
                }
            }

            if (_inputImgE.ColorType == WBColor.WB && matrixCounter > 1)
            {
                matrixCounter = 1;
            }

            _param.MessageLength = _messageDataE.DataModified.Count;

            double result = matrixCounter * _inputImgE.Height * _inputImgE.Width * bitCounter / 8;
            result = Math.Floor(result);
            _param.SpaceToHide = (int)result;
        }

        public void CalculatePossibleSpaceToRead()
        {
            int matrixCounter = 0;

            for (int i = 0; i < _param.DimToUse.Length; i++)
            {
                if (_param.DimToUse[i] == true)
                {
                    matrixCounter++;
                }
            }

            int bitCounter = 0;
            for (int i = 0; i < _param.BitsToUse.Length; i++)
            {
                if (_param.BitsToUse[i] == true)
                {
                    bitCounter++;
                }
            }

            if (_inputImgE.ColorType == WBColor.WB && matrixCounter > 1)
            {
                matrixCounter = 1;
            }

            double result = matrixCounter * _inputImgD.Height * _inputImgD.Width * bitCounter / 8;
            result = Math.Floor(result);
            _param.SpaceToRead = (int)result;
        }

        public bool CheckIfTheHideDataFit()
        {
            if (_messageDataE.DataModified.Count <= _param.SpaceToHide)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckIfTheRevealDataFit()
        {
            if (_param.DecryptionDisplayLength <= _param.SpaceToRead)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void CalculateIndex(int height, int pos, ref int i, ref int j)
        {
            i = (int)Math.Floor((double)(pos / height));
            j = pos - i * height;
        }

        private void ModifyBitsInPixels(byte[,] tempMatrix, List<bool> bits, int pos, ref int bitCounter)
        {
            int i = 0;
            int j = 0;

            CalculateIndex(tempMatrix.GetLength(1), pos, ref i, ref j);

            byte[] tempByte = new byte[1];
            tempByte[0] = tempMatrix[i, j];

            BitArray tempByteInBits = new(tempByte);

            for (int b = 0; b < _param.BitsToUse.Length; b++)
            {
                if (_param.BitsToUse[b] == true && bitCounter < bits.Count)
                {
                    tempByteInBits[b] = bits[bitCounter];
                    bitCounter++;
                }
            }

            byte[] bytee = new byte[1];

            tempByteInBits.CopyTo(bytee, 0);

            tempMatrix[i, j] = bytee[0];
        }
    }

    internal class SteganographyParameters
    {
        private BitArray _bitsToUse;
        private bool[] _dimToUse;
        private int _decryptionDisplayLength;
        private int _spaceToHide;
        private int _messageLength;
        private int _spaceToRead;

        public SteganographyParameters()
        {
            _decryptionDisplayLength = 0;
            _spaceToHide = 0;
            _messageLength = 0;
            _bitsToUse = new(8, false);
            _bitsToUse[0] = true;
            _dimToUse = new bool[3];
            _dimToUse[0] = _dimToUse[1] = _dimToUse[2] = true;
        }

        public void SetBits(int index, bool val)
        {
            _bitsToUse[index] = val;
        }

        public BitArray BitsToUse { get => _bitsToUse; set => _bitsToUse = value; }
        public bool[] DimToUse { get => _dimToUse; set => _dimToUse = value; }
        public int DecryptionDisplayLength { get => _decryptionDisplayLength; set => _decryptionDisplayLength = value; }
        public int SpaceToHide { get => _spaceToHide; set => _spaceToHide = value; }
        public int MessageLength { get => _messageLength; set => _messageLength = value; }
        public int SpaceToRead { get => _spaceToRead; set => _spaceToRead = value; }
    }
}
