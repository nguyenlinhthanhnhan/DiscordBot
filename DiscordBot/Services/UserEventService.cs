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

        private async Task MessageReceivedAsync(SocketMessage msg)
        {            
            if (msg.Channel.Id == 826709742332280862)
            {
                if (msg.Content.StartsWith('<') && !msg.Content.Contains("everyone")) 
                {
                    var mentionedUser = msg.MentionedUsers.FirstOrDefault();

                    if (msg.Author.Id != mentionedUser.Id && !mentionedUser.IsBot)
                    {
                        var result = await _userLeagueVouchService.AddOrUpdateUserLeagueVouchAsync(msg.Author.Id,
                                                                                                      "Ritual",
                                                                                                      mentionedUser.Id,
                                                                                                      msg.Content);
                        if (result.Item1)
                        {
                            var user = mentionedUser as IGuildUser;
                            var guild = _discord.GetGuild(753991000715690085);
                            switch (result.Item2)
                            {
                                case >= 20 and <50:
                                    // Add role Transmutation
                                    if (!user.RoleIds.ToList().Contains(827869613919305758))
                                    {
                                        await user.AddRoleAsync(guild.GetRole(827869613919305758));
                                        var channel = await user.GetOrCreateDMChannelAsync();
                                        await channel.SendMessageAsync($"Chúc mừng bạn đã được up lên rank {guild.GetRole(827869613919305758).Name}");
                                    }
                                    break;
                                case >= 50 and <100:
                                    // Add role Regal
                                    if (!user.RoleIds.ToList().Contains(827869590162898994))
                                    {
                                        await user.AddRoleAsync(guild.GetRole(827869590162898994));
                                        var channel = await user.GetOrCreateDMChannelAsync();
                                        await channel.SendMessageAsync($"Chúc mừng bạn đã được up lên rank {guild.GetRole(827869590162898994).Name}");
                                    }
                                    break;
                                case >= 100 and < 170:
                                    // Add role Chaos
                                    if (!user.RoleIds.ToList().Contains(827869597377495060))
                                    {
                                        await user.AddRoleAsync(guild.GetRole(827869597377495060));
                                        var channel = await user.GetOrCreateDMChannelAsync();
                                        await channel.SendMessageAsync($"Chúc mừng bạn đã được up lên rank {guild.GetRole(827869597377495060).Name}");
                                    }
                                    break;
                                case >= 170 and < 250:
                                    // Add role Exalted
                                    if (!user.RoleIds.ToList().Contains(827870146826731581))
                                    {
                                        await user.AddRoleAsync(guild.GetRole(827870146826731581));
                                        var channel = await user.GetOrCreateDMChannelAsync();
                                        await channel.SendMessageAsync($"Chúc mừng bạn đã được up lên rank {guild.GetRole(827870146826731581).Name}");
                                    }
                                    break;
                                case >= 250 and < 1000:
                                    // Add role Awakener
                                    if (!user.RoleIds.ToList().Contains(827870348439453706))
                                    {
                                        await user.AddRoleAsync(guild.GetRole(827870348439453706));
                                        var channel = await user.GetOrCreateDMChannelAsync();
                                        await channel.SendMessageAsync($"Chúc mừng bạn đã được up lên rank {guild.GetRole(827870348439453706).Name}");
                                    }
                                    break;
                                case > 1000:
                                    // Add role Mirror
                                    if (!user.RoleIds.ToList().Contains(827870901499854888))
                                    {
                                        await user.AddRoleAsync(guild.GetRole(827870901499854888));
                                        var channel = await user.GetOrCreateDMChannelAsync();
                                        await channel.SendMessageAsync($"Chúc mừng bạn đã được up lên rank {guild.GetRole(827870901499854888).Name}");
                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                    }
                    else
                    {
                        await _discord.GetGuild(753991000715690085)
                                .GetTextChannel(826709742332280862)
                                .SendMessageAsync("Tính tự vote cho chính mình ? Hãy quên ý định đó đi =))\nBtw, cũng đừng vote cho bot");
                    }

                }
                else
                {
                    if (!msg.Author.IsBot)
                    {
                        await _discord.GetGuild(753991000715690085)
                                .GetTextChannel(826709742332280862)
                                .SendMessageAsync($"Không spam ở đây, {msg.Author.Username}");
                        await msg.DeleteAsync();
                    }
                }
            }
            if(msg.Channel.Id == 821742347199971368)
            {
                if (!msg.Author.IsBot)
                {
                    var mess = msg.Content.ToLower();
                    if (!(mess.Contains("wtb") && (mess.Contains("rhc") || mess.Contains("rsc"))))
                    {
                        await msg.DeleteAsync();
                        await _discord.GetGuild(753991000715690085)
                                      .GetTextChannel(821742347199971368)
                                      .SendMessageAsync($"{msg.Author.Username} đã đăng sai mẫu, vui lòng đăng như sau: `WTB RHC thứ cần mua`, thay RHC = RSC nếu mua ở RSC");
                    }
                }
            }
            if (msg.Channel.Id == 821743454152687626)
            {
                if (!msg.Author.IsBot)
                {
                    var mess = msg.Content.ToLower();
                    if (!(mess.Contains("wts") && (mess.Contains("rhc") || mess.Contains("rsc"))))
                    {
                        await msg.DeleteAsync();
                        await _discord.GetGuild(753991000715690085)
                                      .GetTextChannel(821743454152687626)
                                      .SendMessageAsync($"{msg.Author.Username} đã đăng sai mẫu, vui lòng đăng như sau: `WTS RHC thứ cần mua`, thay RHC = RSC nếu mua ở RSC");
                    }
                }
            }
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
            var userObject = new User { Id = user.Id, JoinedAt = DateTime.Now.ToLocalTime(), TotalVouch = 0, TotalUniqueVouch = 0 };
            _userService.CreateUser(userObject);
            await _userService.SaveAsync();
            _userService.DetachUser(userObject);

            var role = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Id == 827869475880566815);
            await user.AddRoleAsync(role);

            var discord = _discord.GetGuild(753991000715690085).GetTextChannel(826112672943702027);
            EmbedBuilder embedBuilder = new();
            embedBuilder.AddField($"Chào mừng {user.Username} đến với nhóm", "Hãy gõ !help để biết các command cần thiết");
            embedBuilder.AddField("Acc đã tạo được ", (DateTime.Now.Date - user.CreatedAt.Date).ToString("dd") + " ngày");
            await discord.SendMessageAsync(user.Mention, false, embedBuilder.Build());
        }
    }
}
