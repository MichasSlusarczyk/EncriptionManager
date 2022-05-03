using System.Drawing;

namespace EWMS.Models.Containers
{
    internal class Img
    {
        private int _width;
        private int _height;
        private int _size;
        private byte[,] _R;
        private byte[,] _G;
        private byte[,] _B;
        private WBColor _colorType;

        public int Width { get => _width; set => _width = value; }
        public int Height { get => _height; set => _height = value; }
        internal WBColor ColorType { get => _colorType; set => _colorType = value; }
        public byte[,] R { get => _R; set => _R = value; }
        public byte[,] G { get => _G; set => _G = value; }
        public byte[,] B { get => _B; set => _B = value; }
        public int Size { get => _size; set => _size = value; }

        public Img()
        {
            _colorType = WBColor.None;
            _width = 0;
            _height = 0;
            _size = _width * _height;

            _R = new byte[_width, _height];
            _G = new byte[_width, _height];
            _B = new byte[_width, _height];
        }

        public Img(Bitmap bitmap)
        {
            _R = new byte[bitmap.Width, bitmap.Height];
            _G = new byte[bitmap.Width, bitmap.Height];
            _B = new byte[bitmap.Width, bitmap.Height];

            _width = bitmap.Width;
            _height = bitmap.Height;
            _size = _width * _height;

            BitmapToImg(bitmap);

            CheckColor();
        }

        public void BitmapToImg(Bitmap bitmap)
        {
            for (int w = 0; w < bitmap.Width; w++)
            {
                for (int h = 0; h < bitmap.Height; h++)
                {
                    Color c = bitmap.GetPixel(w, h);
                    _R[w, h] = c.R;
                    _G[w, h] = c.G;
                    _B[w, h] = c.B;
                }
            }
        }

        public Bitmap ImgToBitmap()
        {
            Bitmap bitmap = new(_width, _height);

            for (int w = 0; w < _width; w++)
            {
                for (int h = 0; h < _height; h++)
                {
                    Color c = Color.FromArgb(_R[w, h], _G[w, h], _B[w, h]);
                    bitmap.SetPixel(w, h, c);
                }
            }

            return bitmap;
        }

        private void CheckColor()
        {
            bool isColor = false;

            System.Console.WriteLine("Check Color");

            for (int w = 0; w < _width; w++)
            {
                for (int h = 0; h < _height; h++)
                {

                    if (_R[w, h] != _G[w, h] || _R[w, h] != _B[w, h] || _G[w, h] != _B[w, h])
                    {
                        isColor = true;
                    }

                }
            }

            if (isColor == false)
            {
                _colorType = WBColor.WB;
            }
            else
            {
                _colorType = WBColor.Color;
            }
        }

    }
}
