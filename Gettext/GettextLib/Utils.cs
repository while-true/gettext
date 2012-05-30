namespace GettextLib
{
    public static class Utils
    {
        public static string PseudoTranslate(string str)
        {
            return string.Format("[!{0}!]", str);
        }
    }
}
