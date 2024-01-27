using Newtonsoft.Json;

namespace pizza_api.Models
{
    public class Pizza
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("types")]
        public int[] Types { get; set; }
        [JsonProperty("sizes")]
        public int[] Sizes { get; set; }
        [JsonProperty("price")]
        public float Price { get; set; }
        [JsonProperty("category")]
        public int Category { get; set; }
        [JsonProperty("rating")]
        public float Rating { get; set; }

    }
}
