using SnowflakeGenerator;

namespace Blog.Services;

public static class SnowflakeService
{
    public static ulong GenerateId()
    {
        var settings = new Settings
        {
            MachineID = 1,
            CustomEpoch = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero)
        };

        var snowflake = new Snowflake(settings);
        return (ulong)snowflake.NextID();
    }
}
