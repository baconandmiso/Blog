using File = Blog.Entity.File;

namespace Blog.Repository;

public class FileRepository : Repository<File>, IFileRepository
{
    public FileRepository(ApplicationDbContext context) : base(context)
    {
    }
}