using Blog.Entity;

namespace Blog.Repository;

public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(ApplicationDbContext context) : base(context)
    {
    }
}