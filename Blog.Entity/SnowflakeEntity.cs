using Blog.Entity.Utils;

namespace Blog.Entity;

public abstract class SnowflakeEntity : Entity<long>
{
    /// <summary>
    /// エンティティが最初に作成された時刻を表す <see cref="DateTimeOffset"/>。
    /// </summary>
    public DateTimeOffset CreatedAt { get; protected set; }

    protected SnowflakeEntity()
    {
        this.Id = SnowflakeService.GenerateId();

        this.CreatedAt = SnowflakeService.GetTimestampFromId(this.Id);
    }
}