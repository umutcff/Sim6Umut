namespace Sim6Umut.ViewModels.Project
{
    public class ProjectCreateVM
    {
        public string Name { get; set; } = string.Empty;
        public IFormFile Image { get; set; } = null!;
        public int CategoryId { get; set; }
    }
}
