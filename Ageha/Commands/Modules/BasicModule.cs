using Ageha.Util;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Ageha.Commands.Modules
{
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder][Summary("The text to echo")] string echo) => ReplyAsync(echo);

        [Command("says")]
        [Summary("Echoes a message.")]
        public async Task SaysAsync([Remainder][Summary("The text to echo")] string echo)
        {
            foreach (var tag in Context.Message.Tags)
            {
                await ReplyAsync($"{tag.Type.ToString()} {tag.Key.ToString()} {tag.Value.ToString()}");

                if (tag.Type == TagType.Emoji)
                    await ReplyAsync(Emote.Parse($"<:{(tag.Value as Emote).Name}:{tag.Key.ToString()}>").Url);
            }
        }

        [Command("summon")]
        [Summary("*smug intensifies*")]
        public async Task SummonAsync()
        {
            EmbedBuilder smug = new EmbedBuilder();

            smug.Title = "**I AM AWAKE !**";
            smug.ImageUrl = @"https://i.imgur.com/gJvYUOM.jpg";

            await Context.Channel.SendMessageAsync(embed: smug.Build());
        }

        [Command("ask")]
        [Summary("Send a question and she will answer")]
        public Task AskAsync([Remainder][Summary("The question")] string question = null)
        {
            string response = "Try typing something, fag.";

            if (question != null)
            {
                response = Utils.Choose("Yes", "No", "Maybe");
            }

            return ReplyAsync(response);
        }

        [Command("smug")]
        [Summary("*smugs*")]
        public Task SmugAsync()
        {
            EmbedBuilder smug = new EmbedBuilder();

            smug.Title = "*smugs*";
            smug.ImageUrl = JsonWrapper.JsonChoose<string>(@"E:\Development\_Bots\Discord\Ageha\Ageha\Resources\smug.json");

            return ReplyAsync(embed: smug.Build());
        }

        [Command("choose")]
        [Summary("Give her options and she will choose")]
        public Task ChooseAsync([Summary("The options to choose")] params string[] options) => ReplyAsync(Utils.Choose(options));

        [Command("rps")]
        [Summary("Let's play rock, paper and scissors")]
        public async Task RpsAsync([Remainder][Summary("Your turn (rock, paper or scissors)")] string choice)
        {
            string bot_choose = Utils.Choose("Rock", "Paper", "Scissors");

            EmbedBuilder rps = new EmbedBuilder();

            rps.Title = "Rock, Paper, Scissors";
            rps.Author = new EmbedAuthorBuilder().WithName("Ageha").WithIconUrl("https://cdn.discordapp.com/attachments/547151965902340117/551614683937898498/NewAgeha1024x1024.png");
            rps.WithColor(0x00AE86);
            rps.Description = $"@{Context.Message.Author.ToString()} chooses: **{choice}** !";
            rps.AddField("I choose...", $"**{bot_choose}**!", false);
            rps.AddField("And result is...", Games.Rps(bot_choose, choice), true);

            await Context.Channel.SendMessageAsync(embed: rps.Build());
        }

        [Command("google")]
        [Summary("Google it !")]
        public Task ChooseAsync([Remainder][Summary("The thing to search for")] string search) => ReplyAsync($"http://lmgtfy.com/?q={search.Replace(' ', '+')}");
    }
}