using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    [RequireOwner]
    public sealed class DeveloperModule:ModuleBase<SocketCommandContext>
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly CommandService _commandService;
        private readonly IUserService _userService;

        public DeveloperModule(ILeagueRepository leagueRepository, CommandService commandService, IUserService userService)
        {
            _leagueRepository = leagueRepository;
            _commandService = commandService;
            _userService = userService;
        }

        [Command("!!dev")]
        [Summary("Hàng thiết kế riêng để Hawk xài, đừng nghĩ dùng =))")]
        public async Task HelpAsync()
        {
            List<CommandInfo> commands = _commandService.Commands.ToList();
            EmbedBuilder embedBuilder = new();

            foreach (CommandInfo command in commands)
            {
                if (command.Module.Name.Equals(nameof(DeveloperModule)))
                {
                    // Get the command symmary attribute information
                    string embedFieldText = command.Summary ?? "Không có gì để trình bày =))\n";
                    embedBuilder.AddField(command.Name, embedFieldText);
                }
            }
            await ReplyAsync("", false, embedBuilder.Build());
        }
        [Command("helo")]
        public async Task HaloAsync() => await ReplyAsync("Hawk vĩ đại anh minh thần võ, thiên thu vạn tái nhất thống giang hồ");

        // Get user permission
        [Command("!!permission")]
        [Summary("Lấy permission của user")]
        public async Task PermissionAsync(IUser user = null)
        {
            user ??= Context.User;
            var permissions = string.Join(", ", (user as SocketGuildUser).GuildPermissions.ToList());
            await ReplyAsync($"List permission: {permissions}");
        }

        // Request bot execute command
        [Command("!!exec")]
        [Summary("Yêu cầu bot thực thi lệnh")]
        public async Task RequireAsync(string command = null)
        {
            if (string.IsNullOrWhiteSpace(command)) await ReplyAsync("Hãy đưa command vào như tham số");
            else
            {
                if (command.Contains("|")) command = command.Replace('|', ' ');
                await ReplyAsync(command);
            }
        }

        [Command("!!addleague")]
        [Summary("Thêm league vào cơ sở dữ liệu")]
        public async Task AddLeague(string league = null)
        {
            if (string.IsNullOrWhiteSpace(league)) await ReplyAsync("Hãy đưa tham số vào");
            else
            {
                _leagueRepository.CreateLeague(new Models.League {LeagueName = league });
                await ReplyAsync($"Đã tạo thêm league {league} vào cơ sở dữ liệu");
            }
        }

        [Command("!!addUniqueVouch")]
        [Summary("Thêm unique vouch cho user")]
        public async Task AddUniqueVouchAsync(IUser user = null, uint point = 0)
        {
            if(user is not null)
            {
                var userDB = await _userService.GetUserAsync(user.Id, true);
                userDB.TotalUniqueVouch += point;
                await _userService.SaveAsync();
                await ReplyAsync($"Đã cộng cho user {user.Username} {point} point vào Unique Vouch");
            }
            else
            {
                await ReplyAsync("Vui lòng thêm tham số user vào");
            }
        }
    }
}
