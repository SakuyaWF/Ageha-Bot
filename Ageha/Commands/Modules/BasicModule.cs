using Ageha.Util;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Ageha.Commands.Modules
{
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Echoes a message
        /// </summary>
        /// <param name="echo">The message to echo</param>
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder][Summary("The text to echo")] string echo) => ReplyAsync(echo);

        /// <summary>
        /// Mocks the user for using help
        /// </summary>
        /// <param name="argument">The argument to get help with</param>
        [Command("help")]
        [Summary("Looks like you need help.")]
        public Task HelpAsync([Remainder][Summary("The topic to get help with")] string argument = "")
        {
            // Sends a different message depending if a specific help was needed
            if (string.IsNullOrWhiteSpace(argument))
            {
                return ReplyAsync("I'm not sure what you need help with. Try |help [topic]");
            }
            else
            {
                // *smugs*
                return ReplyAsync($"It looks like you might need help with {argument}");
            }
        }

        /// <summary>
        /// Smugs
        /// </summary>
        [Command("summon")]
        [Summary("*smug intensifies*")]
        public async Task SummonAsync()
        {
            // Creates a new embed
            EmbedBuilder smug = new EmbedBuilder();

            // Sets the title and the smug
            smug.Title = "**I AM AWAKE !**";
            smug.ImageUrl = @"https://i.imgur.com/gJvYUOM.jpg";

            // Sends the embed back with a smug
            await Context.Channel.SendMessageAsync(embed: smug.Build());
        }

        /// <summary>
        /// Tries to awnser a question
        /// </summary>
        /// <param name="question">The question to ask</param>
        [Command("ask")]
        [Summary("Send a question and she will answer")]
        public Task AskAsync([Remainder][Summary("The question")] string question = null)
        {
            // Default response
            string response = "Try typing something, fag.";

            if (question != null)
            {
                // If the response isn't null, choose from the options
                response = Utils.Choose("Yes", "No", "Maybe");
            }

            // Sends back the response
            return ReplyAsync(response);
        }

        /// <summary>
        /// smug
        /// </summary>
        [Command("smug")]
        [Summary("*smugs*")]
        public Task SmugAsync()
        {
            // Builds the smug
            EmbedBuilder smug = new EmbedBuilder();

            // Sets the smug with a smug
            smug.Title = "*smugs*";
            smug.ImageUrl = JsonWrapper.JsonChoose<string>(@"E:\Development\_Bots\Discord\Ageha\Ageha\Resources\smug.json");

            // Sends back the smug to the command issuer
            return ReplyAsync(embed: smug.Build());
        }

        /// <summary>
        /// Given different options separated by space, she will choose one
        /// </summary>
        /// <param name="options"></param>
        [Command("choose")]
        [Summary("Give her options and she will choose")]
        public Task ChooseAsync([Summary("The options to choose")] params string[] options) => ReplyAsync($"I choose... {Utils.Choose(options)}");

        /// <summary>
        /// Rock, paper, scissors
        /// </summary>
        /// <param name="choice">The user's choice</param>
        [Command("rps")]
        [Summary("Let's play rock, paper and scissors")]
        public async Task RpsAsync([Remainder][Summary("Your turn (rock, paper or scissors)")] string choice)
        {
            // The bot choice
            string bot_choose = Utils.Choose("Rock", "Paper", "Scissors");

            // Crates new embed
            EmbedBuilder rps = new EmbedBuilder();

            // Sets the embed
            rps.Title = "Rock, Paper, Scissors";
            rps.Author = new EmbedAuthorBuilder().WithName("Ageha").WithIconUrl("https://cdn.discordapp.com/attachments/547151965902340117/551614683937898498/NewAgeha1024x1024.png");
            rps.WithColor(0x00AE86);
            rps.Description = $"@{Context.Message.Author.ToString()} chooses: **{choice}** !";
            rps.AddField("I choose...", $"**{bot_choose}**!", false);

            // Calls the method to decide the result
            rps.AddField("And result is...", Games.Rps(bot_choose, choice), true);

            // Sends back the embed
            await Context.Channel.SendMessageAsync(embed: rps.Build());
        }

        /// <summary>
        /// Google it !
        /// </summary>
        /// <param name="search">The query</param>
        [Command("google")]
        [Summary("Google it !")]
        public Task ChooseAsync([Remainder][Summary("The thing to search for")] string search) => ReplyAsync($"http://lmgtfy.com/?q={search.Replace(' ', '+')}");
    }
}