using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlueSkySharp.Entities
{
    internal class CreateRecordResponse
    {
        [JsonPropertyName("uri")]
        public string? Uri { get; set; }
        [JsonPropertyName("cid")]
        public string? Cid { get; set; }
        [JsonPropertyName("commit")]
        public Commit? Commit { get; set; }
        [JsonPropertyName("validationStatus")]
        public string? ValidationStatus { get; set; }
    }

    internal class Commit
    {
        [JsonPropertyName("cid")]
        public string? Cid { get; set; }
        [JsonPropertyName("rev")]
        public string? Rev { get; set; }
    }

    public class Post
    {
        [JsonPropertyName("uri")]
        public string? Uri { get; set; }
        [JsonPropertyName("cid")]
        public string? Cid { get; set; }
    }
}
