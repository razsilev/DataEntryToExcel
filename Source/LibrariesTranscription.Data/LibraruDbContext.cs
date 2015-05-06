namespace LibrariesTranscription.Data
{
    using System.Data.Entity;
    
    using LibrariesTranscription.Model;

    public class LibraruDbContext : DbContext
    {
        public LibraruDbContext()
            : base("DefaultConnection")
        {

        }

        public IDbSet<LibraryItem> LibraryItems { get; set; }
    }
}
