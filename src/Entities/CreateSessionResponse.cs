using System.Text.Json.Serialization;

namespace BlueSkySharp.Entities
{

    internal class CreateSessionResponse
    {
        [JsonPropertyName("handle")]
        public string? Handle { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("emailConfirmed")]
        public bool EmailConfirmed { get; set; }
        [JsonPropertyName("emailAuthFactor")]
        public bool EmailAuthFactor { get; set; }
        [JsonPropertyName("accessJwt")]
        public string? AccessJwt { get; set; }
        [JsonPropertyName("refreshJwt")]
        public string? RefreshJwt { get; set; }
        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }
}
