using Ageha.Util;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ageha.Commands.Modules
{
    public class QuoteModule : ModuleBase<SocketCommandContext>
    {
        // The part of the link to remove
        private readonly string MessageBaseLink = @"https://discordapp.com/channels/";

        /// <summary>
        /// Sends an embed to the channel containg the quoted message
        /// </summary>
        /// <param name="link">The link from the quoted message</param>
        /// <param name="quoteMessage">An additional message from the user that quoted the link</param>
        /// <returns></returns>
        [Command("quote")]
        [Summary("Quotes a message")]
        [Alias("q")]
        public async Task QuoteAsync([Summary("The message link to quote")] string link, [Remainder][Summary("The message to follow the quote.")] string quoteMessage = "")
        {
            // Initializes the control boolean
            bool success = false;

            // Remove the message base part and splits all the IDs into an array
            string[] stringIDs = link.Replace(MessageBaseLink, "").Split('/');

            // Creates a new array to hold the IDs
            ulong[] ids = new ulong[3];

            // Parses all the IDs from strings to unsigned longs and saves the result into the array, also it stores the successes or failures into the boolean
            for (int i = 0; i < 3; i++)
            {
                success = UInt64.TryParse(stringIDs[i], out ids[i]);
            }

            // If the parse fails, delete the original user command message
            if (!success)
            {
                await Context.Message.DeleteAsync();
                await Ageha.Log(LogSeverity.Error, "Could not parse some ID");
                return;
            }

            // Free up memory
            stringIDs = null;

            // Tries to get the guild from the ID
            var sourceGuild = Context.Client.GetGuild(ids[0]);

            // If the guild is null, reply to the user
            if (sourceGuild == null)
            {
                await ReplyAsync("I can't quote this message, because I'm not in the server");
                return;
            }

            // Tries to get the channel from the ID
            var sourceChannel = sourceGuild?.GetTextChannel(ids[1]);

            // If the channel is null, reply to the user
            if (sourceChannel == null)
            {
                await ReplyAsync("I can't quote this message, because I can't view the channel");
                return;
            }

            // Tries to get the message from the ID
            var quotedMessage = sourceChannel?.GetMessageAsync(ids[2]).Result;

            // If the message is null, reply to the user
            if (quotedMessage == null)
            {
                await ReplyAsync("I can't quote this message, because it doesn't exist");
                return;
            }

            // Free up some memory
            ids = null;

            // Initialize the embed builder, sets the author as the user from the quoted message and their avatar,
            // put the message in the description with a follow up link, also puts the channel, server and timestamp
            EmbedBuilder response = new EmbedBuilder();
            response.WithAuthor($"{quotedMessage.Author.ToString()}", quotedMessage.Author.GetAvatarUrl());
            response.WithDescription($"{quotedMessage.Content} \n\n [*Go to message*]({link})");
            response.WithFooter($"{sourceChannel.Name} | {sourceGuild.Name} | {quotedMessage.Timestamp.ToString()}", sourceGuild.IconUrl);
            response.Color = Color.LighterGrey;

            // Sends the message back to the user, containing a disclaimer and the additional message if any
            if (string.IsNullOrWhiteSpace(quoteMessage))
            {
                await ReplyAsync($"*Quoted by {Context.Message.Author.Mention}*", embed: response.Build());
            }
            else
            {
                await ReplyAsync($"*Quoted by {Context.Message.Author.Mention}*:\n> {quoteMessage}", embed: response.Build());
            }

            // Delete the original user command message
            await Context.Message.DeleteAsync();
        }

        /// <summary>
        /// Saves a quantity of links before the sent link
        /// </summary>
        /// <param name="link">The link to start searching</param>
        /// <param name="quantity">The quantity of links to retrieve</param>
        [Command("savelinks")]
        [Summary("Save all the links in a chat")]
        public async Task SavelinksAsync([Summary("The message link to quote")] string link, [Summary("The qauntities of link to save")] int quantity)
        {
            // Initializes the control boolean
            bool success = false;

            // Remove the message base part and splits all the IDs into an array
            string[] stringIDs = link.Replace(MessageBaseLink, "").Split('/');

            // Creates a new array to hold the IDs
            ulong[] ids = new ulong[3];

            // Parses all the IDs from strings to unsigned longs and saves the result into the array, also it stores the successes or failures into the boolean
            for (int i = 0; i < 3; i++)
            {
                success = UInt64.TryParse(stringIDs[i], out ids[i]);
            }

            // If the parse fails, delete the original user command message
            if (!success)
            {
                await Context.Message.DeleteAsync();
                await Ageha.Log(LogSeverity.Error, "Could not parse some ID");
                return;
            }

            // Free up memory
            stringIDs = null;

            // Tries to get the guild from the ID
            SocketGuild sourceGuild = Context.Client.GetGuild(ids[0]);

            // If the guild is null, reply to the user
            if (sourceGuild == null)
            {
                await ReplyAsync("I'm not in the linked server");
                return;
            }

            // Tries to get the channel from the ID
            SocketTextChannel sourceChannel = sourceGuild?.GetTextChannel(ids[1]);

            // If the channel is null, reply to the user
            if (sourceChannel == null)
            {
                await ReplyAsync("I can't view the linked channel");
                return;
            }

            // Tries to get the message from the ID
            IMessage linkedMessage = sourceChannel?.GetMessageAsync(ids[2]).Result;

            // If the message is null, reply to the user
            if (linkedMessage == null)
            {
                await ReplyAsync("The linked message doesn't exist");
                return;
            }

            // Free up some memory
            ids = null;

            // Gets the cached messages
            var cachedMessages = await sourceChannel.GetMessagesAsync(linkedMessage, Direction.Before, quantity).FlattenAsync();

            // Puts the linked message in the list
            cachedMessages.Append(linkedMessage);

            // Reverses the list to be in chronological order
            cachedMessages.Reverse();

            // Sends the quantity of found messages in the channel
            await ReplyAsync($"Found {cachedMessages.Count()} links.");

            // Creates an empty dictionary and a counter
            Dictionary<int, string> links = new Dictionary<int, string>();
            int counter = 0;

            // Only proceeds if any messages are found
            if (cachedMessages != null || cachedMessages.Count() > 0)
            {
                // Creates a valid file name to save the links
                string fileName = $"{MakeValidFileName(sourceGuild.Name)}_{MakeValidFileName(sourceChannel.Name)}_{linkedMessage.Id}.json";

                // Iterates over all messages found
                foreach (IMessage message in cachedMessages)
                {
                    // Only tries to pick emotes if the message isn't empty
                    if (!String.IsNullOrWhiteSpace(message.Content))
                    {
                        // Iterates over all tags
                        foreach (ITag tag in message.Tags)
                        {
                            // Only procceeds on emote tags
                            if (tag.Type == TagType.Emoji)
                            {
                                // Adds the emote link and updates the counter
                                links.Add(counter++, Emote.Parse($"<:{(tag.Value as Emote).Name}:{tag.Key.ToString()}>").Url);
                            }
                        }
                    }

                    // Checks to seed if there are any embeds
                    if (message.Embeds.Count > 0)
                    {
                        GetEmbeds(message.Embeds, ref links, ref counter);
                    }

                    // Checks to seed if there are any attachments
                    if (message.Attachments.Count > 0)
                    {
                        GetAttachments(message.Attachments, ref links, ref counter);
                    }
                }

                // Writes the json on a file
                JsonWrapper.WriteJSON<int, string>(fileName, links);

                // Sends the json back in the channel
                await Context.Channel.SendFileAsync(fileName, $"Here is the json containing {cachedMessages.Count()} links from before the message : {linkedMessage.GetJumpUrl()}");

                // Deletes the original file
                File.Delete(fileName);
            }
        }

        // Removes any file breaking characters
        private static string MakeValidFileName(string name)
        {
            string invalidChars = Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return Regex.Replace(name, invalidRegStr, "_");
        }

        /// <summary>
        /// Gets the embeds
        /// </summary>
        /// <param name="embedList">The embeds list</param>
        /// <param name="links">The dictionary containing the links</param>
        /// <param name="counter">The counter</param>
        public void GetEmbeds(IReadOnlyCollection<IEmbed> embedList, ref Dictionary<int, string> links, ref int counter)
        {
            // Iterates over all embeds
            foreach (IEmbed embed in embedList)
            {
                // Only procceeds if there is any image
                if (embed.Image.HasValue)
                {
                    // Adds the image link and updates the counter
                    links.Add(counter++, embed.Image.Value.Url);
                }

                // Only procceeds if there is any video
                if (embed.Video.HasValue)
                {
                    // Adds the video link and updates the counter
                    links.Add(counter++, embed.Video.Value.Url);
                }

                // Only procceeds if there is any link
                if (!String.IsNullOrWhiteSpace(embed.Url))
                {
                    // Adds the link and updates the counter
                    links.Add(counter++, embed.Url);
                }
            }
        }

        /// <summary>
        /// Gets all the attachments
        /// </summary>
        /// <param name="attachmentList">The attachements list</param>
        /// <param name="links">The dictionary containing the links</param>
        /// <param name="counter">The counter</param>
        public void GetAttachments(IReadOnlyCollection<IAttachment> attachmentList, ref Dictionary<int, string> links, ref int counter)
        {
            // Iterates over all the attachments
            foreach (IAttachment attachment in attachmentList)
            {
                // Adds the attachment link and updates the counter
                links.Add(counter++, attachment.Url);
            }
        }
    }
}