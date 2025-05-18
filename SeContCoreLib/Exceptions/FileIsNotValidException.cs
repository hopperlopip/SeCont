namespace SeContCoreLib.Exceptions
{
    [Serializable]
    public class FileIsNotValidException : Exception
    {
        public FileIsNotValidException() { }
        public FileIsNotValidException(string message) : base(message) { }
    }
}
