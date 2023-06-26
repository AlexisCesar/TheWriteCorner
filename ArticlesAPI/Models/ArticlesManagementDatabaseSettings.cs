namespace ArticlesAPI.Models
{
    public class ArticlesManagementDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ArticlesCollectionName { get; set; } = null!;
    }
}
