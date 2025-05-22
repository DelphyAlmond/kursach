namespace UniversityContracts.BindingModels;

public class ReportBindingModel
{
    public string? FileName { get; set; }
    public Stream? Stream { get; set; }
    public int[]? Ids { get; set; }
}