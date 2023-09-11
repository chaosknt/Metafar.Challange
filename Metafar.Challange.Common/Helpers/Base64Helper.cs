namespace Metafar.Challange.Common.Helpers
{
    public static class Base64Helper
    {
        public static string Encode(byte[] plainText)
        {
            return System.Convert.ToBase64String(plainText);
        }
    }
}
