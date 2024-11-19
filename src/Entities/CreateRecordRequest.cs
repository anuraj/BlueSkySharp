using System.Text.Json.Serialization;

namespace BlueSkySharp.Entities
{
    internal class CreateRecordRequest
    {
        [JsonPropertyName("repo")]
        public string? Repo { get; set; }
        [JsonPropertyName("collection")]
        public string? Collection { get; set; }
        [JsonPropertyName("record")]
        public Record? Record { get; set; }
    }

    internal class Record
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
        [JsonPropertyName("createdAt")]
        public string? CreatedAt { get; set; }
    }
}