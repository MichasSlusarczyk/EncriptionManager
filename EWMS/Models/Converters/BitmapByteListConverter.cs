using System.Collections.Generic;
using System.Drawing;

namespace EWMS.Models.Converters
{
    internal class BitmapByteListConverter
    {
        public static List<byte> BitmapToByteList(Bitmap bitmap)
        {
            return ByteListByteArrayConverter.ByteArrayToByteList(BitmapByteArrayConverter.BitmapToByteArray(bitmap));
        }

        public static Bitmap ByteListToBitmap(List<byte> bytes)
        {
            return BitmapByteArrayConverter.ByteArrayToBitmap(ByteListByteArrayConverter.ByteListToByteArray(bytes));
        }

    }
}
