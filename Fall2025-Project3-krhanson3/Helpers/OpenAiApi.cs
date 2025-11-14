using Azure;
using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Chat;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json.Nodes;
using VaderSharp2;

namespace Fall2025_Project3_krhanson3.Helpers
{
    public class OpenAiApi
    {
        private readonly Uri _apiEndpoint;
        private readonly ApiKeyCredential _apiCredential;
        private readonly string _deployment;

        public OpenAiApi(IConfiguration configuration)
        {
            _apiEndpoint = new(configuration["AzureOpenAI:Endpoint"]);
            _apiCredential = new(configuration["AzureOpenAI:ApiKey"]);
            _deployment = "gpt-4.1-mini";
        }

        public async Task<(double SentimentAverage, IEnumerable<(string User, string Text, double Sentiment)> Tweets)>
            GenerateTweetsForActor(string actorName)
        {
            ChatClient client = new AzureOpenAIClient(_apiEndpoint, _apiCredential).GetChatClient(_deployment);

            IEnumerable<ChatMessage> messages =
            [
                new SystemChatMessage("Respond with JSON array of tweet objects: { username: '', tweet: '' }"),
                new UserChatMessage($"Generate 5 tweets about the actor {actorName}.")
            ];

            ClientResult<ChatCompletion> result = await client.CompleteChatAsync(messages);

            string jsonText = result.Value.Content.FirstOrDefault()?.Text ?? "[]";
            JsonArray jsonArray = JsonNode.Parse(jsonText)!.AsArray();

            var analyzer = new SentimentIntensityAnalyzer();
            double total = 0;

            var tweetResults = jsonArray.Select(t =>
            {
                string user = t!["username"]?.ToString()!;
                string text = t!["tweet"]?.ToString()!;
                var score = analyzer.PolarityScores(text).Compound;

                total += score;

                return (user, text, score);
            }).ToList();

            double avg = total / tweetResults.Count;

            return (avg, tweetResults);
        }
    }
}