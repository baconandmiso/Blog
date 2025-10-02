using SnowflakeGenerator;

namespace Blog.Entity.Utils;

public static class SnowflakeService
{
    public static long GenerateId()
    {
        var settings = new Settings
        {
            MachineID = 1,
            CustomEpoch = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero)
        };

        var snowflake = new Snowflake(settings);
        return snowflake.NextID();
    }

    public static DateTimeOffset GetTimestampFromId(long id)
    {
        var settings = new Settings
        {
            MachineID = 1,
            CustomEpoch = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero)
        };

        var snowflake = new Snowflake(settings);
        return Convert.ToDateTime(snowflake.DecodeID(id).Timestamp);
    }
}
