using x10_csharp;

public class BookingHsCode {
  [Mandatory]
  [PlaceholderText("English description of product")]
  public string Description;

  [Mandatory]
  [PlaceholderText("TODO: Need Chinese Translation")]
  public string DescriptionInLocalLanguage;
}
