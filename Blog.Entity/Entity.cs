namespace Blog.Entity;

public abstract class Entity<TId> where TId : IEquatable<TId>
{
    /// <summary>
    /// このオブジェクトの一意の識別子を取得します。
    /// </summary>
    public TId Id { get; protected set; }
}