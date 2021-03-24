using System;

namespace LT.DigitalOffice.FileService.Validation.Helpers
{
    public static class EncodeHelper
    {
        public static bool IsBase64Coded(string base64String)
        {
            if (base64String == null)
            {
                return false;
            }

            var byteString = new Span<byte>(new byte[base64String.Length]);

            return Convert.TryFromBase64String(base64String, byteString, out _);
        }
    }
}
