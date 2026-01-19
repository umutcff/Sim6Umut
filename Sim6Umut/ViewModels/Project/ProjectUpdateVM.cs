namespace Sim6Umut.ViewModels.Project
{
    public class ProjectUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IFormFile Image { get; set; } = null!;
        public int CategoryId { get; set; }
    }
}
