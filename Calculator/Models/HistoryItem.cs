namespace Calculator.Models;

public class HistoryItem
{
    public string Expression { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;

    public string DisplayText => $"{Expression}  {Result}";

    public override string ToString()
    {
        return DisplayText;
    }
}
