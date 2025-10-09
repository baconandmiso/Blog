using SnowflakeGenerator;

namespace Blog.Entity.Utils;

internal static class SnowflakeService
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
        var decoded = snowflake.DecodeID(id);

        var timestampMilliseconds = decoded.Timestamp;
        var customEpoch = settings.CustomEpoch;

        var duration = TimeSpan.FromMilliseconds(timestampMilliseconds);
        var dateTimeOffset = (DateTimeOffset)customEpoch + duration;

        return dateTimeOffset;
    }
}
