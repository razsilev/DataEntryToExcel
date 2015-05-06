namespace LibrariesTranscription.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using LibrariesTranscription.Model;

    public class Database
    {
        private LibraruDbContext dbContext;

        public Database(LibraruDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(IEnumerable<LibraryItem> libraryItems)
        {
            foreach (var item in libraryItems)
            {
                this.dbContext.LibraryItems.Add(item);
            }

            this.dbContext.SaveChanges();
        }

        public List<LibraryItem> All()
        {
            return this.dbContext.LibraryItems.ToList();
        }
    }
}
