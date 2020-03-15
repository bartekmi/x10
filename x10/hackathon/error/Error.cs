namespace x10.error {
    public class Error {
        public string Path { get; set; }
        public int? Line { get; set; }
        public int? LinePosition { get; set; }
        public string Message { get; set; }

        public override string ToString() {
            return string.Format("{0}:{1}:{2} {3}", Path, Line, LinePosition, Message);
        }
    }
}