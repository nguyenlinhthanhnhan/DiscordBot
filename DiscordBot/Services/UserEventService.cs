using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class UserEventService
    {
        private readonly DiscordSocketClient _discord;
        private readonly IUserService _userService;
        private readonly IUserLeagueVouchService _userLeagueVouchService;

        public UserEventService(DiscordSocketClient discord,
                                IUserService userService,
                                IUserLeagueVouchService userLeagueVouchService)
        {
            _discord = discord;
            _userService = userService;
            _userLeagueVouchService = userLeagueVouchService;
        }

        public Task InitializeAsync()
        {
            _discord.UserJoined += UserJoinedAsync;
            _discord.UserLeft += UserLeftAsync;
            _discord.MessageReceived += MessageReceivedAsync;

            return Task.CompletedTask;
        }

        private Task MessageReceivedAsync(SocketMessage msg)
        {
            var mentionedUser = msg.MentionedUsers.FirstOrDefault();
            var author = msg.Author;
            if (msg.Channel.Id == 826709742332280862
                && msg.Content.StartsWith('<')
                && !msg.Content.Contains("everyone"))
            {
                if (author.Id != mentionedUser.Id && !mentionedUser.IsBot)
                {
                    _userLeagueVouchService.AddOrUpdateUserLeagueVouchAsync(msg.Author.Id,
                                                                        "Ritual",
                                                                        msg.MentionedUsers.FirstOrDefault().Id,
                                                                        msg.Content);
                }
                else
                {
                    _discord.GetGuild(753991000715690085)
                            .GetTextChannel(826709742332280862)
                            .SendMessageAsync("Tính tự vote cho chính mình ? Hãy quên ý định đó đi =))\nBtw, cũng đừng vote cho bot");
                }
                
            }

            return Task.CompletedTask;
        }

        private async Task UserLeftAsync(SocketGuildUser user)
        {
            _userService.DeleteUser(user.Id);
            await _userService.SaveAsync();

            await _discord.GetGuild(753991000715690085)
                          .GetTextChannel(753991000715690087)
                          .SendMessageAsync($"{user.Username} đã out group, anh ấy đã rời bỏ chúng ta =))");
        }

        private async Task UserJoinedAsync(SocketGuildUser user)
        {
            var userObject = new User { Id = user.Id, JoinedAt = DateTime.Now.ToLocalTime(), TotalVouch = 0 };
            _userService.CreateUser(userObject);
            await _userService.SaveAsync();
            _userService.DetachUser(userObject);

            var role = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Id == 824311076350722119);
            await user.AddRoleAsync(role);

            var discord = _discord.GetGuild(753991000715690085).GetTextChannel(826112672943702027);
            EmbedBuilder embedBuilder = new();
            embedBuilder.AddField($"Chào mừng {user.Username} đến với nhóm", "Hãy gõ !help để biết các command cần thiết");
            embedBuilder.AddField("Acc đã tạo được ", (DateTime.Now.Date - user.CreatedAt.Date).ToString("dd") + " ngày");
            await discord.SendMessageAsync(user.Mention, false, embedBuilder.Build());
        }
    }
}
