namespace SeContCoreLib.Exceptions
{
    [Serializable]
    public class HashValidationException : Exception
    {
        public HashValidationException() { }
        public HashValidationException(string message) : base(message) { }
    }
}
