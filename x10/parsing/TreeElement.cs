namespace x10.parsing
{
    public abstract class TreeElement
    {
            public FileInfo FileInfo { get; set; }
    public int LineNumber { get; set; }
    public int CharacterPosition { get; set; }

    }
}