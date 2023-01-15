using System.Text.Json;
using System.Text.Json.Serialization;

namespace QueryUnmappedTypes.Extensions
{
    public static class JsonExstension
    {
        public static string ToJson<T>(this T @this) => JsonSerializer.Serialize(@this);
    }
}
