namespace SeContCoreLib.Exceptions
{
    [Serializable]
    public class KeyValidationException : Exception
    {
        public KeyValidationException() { }
        public KeyValidationException(string message) : base(message) { }
    }
}
