using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commandService;

        public PublicModule(CommandService commandService)
        {
            _commandService = commandService;
        }

        [Command("help")]
        [Alias("h")]
        [Summary("Liệt kê toàn bộ các command")]
        public async Task HelpAsync()
        {
            List<CommandInfo> commands = _commandService.Commands.ToList();
            EmbedBuilder embedBuilder = new();

            foreach (CommandInfo command in commands)
            {
                if (command.Module.Name.Equals(nameof(PublicModule)))
                {
                    // Get the command symmary attribute information
                    string embedFieldText = command.Summary ?? "No description avaiable\n";
                    embedBuilder.AddField(command.Name, embedFieldText);
                }
            }
            await ReplyAsync("", false, embedBuilder.Build());
        }

        // Get user info
        [Command("userinfo")]
        [Alias("ui", "u")]
        [Summary("Lấy thông tin của user, cú pháp: !userinfo hoặc !ui hoặc !u, tùy thích, nếu muốn tra người khác, gõ theo cú pháp: !u @[tag người đó vào]")]
        public async Task UserInfoAsync(IUser user = null)
        {
            user ??= Context.User;
            var roles = (user as SocketGuildUser).Roles.ToList();
            string role;
            if (roles[0].Name.Contains("everyone") && roles.Count == 1) role = "Newbie";
            else role = roles[1].ToString();
            EmbedBuilder embedBuilder = new();
            embedBuilder.AddField(user.Username, user.Status);
            embedBuilder.AddField("Acc đã tạo được ", (DateTime.Now.Date - user.CreatedAt.Date).ToString("dd") + " ngày");
            embedBuilder.AddField("Hiện đang", user.Activity is null ? "Không làm gì cả" : user.Activity);
            embedBuilder.AddField("Cấp độ", role);
            await ReplyAsync("", false, embedBuilder.Build());
        }

        

        // Count user online/offline/unk
        [Command("count")]
        [Summary("Đếm lượng user online, offline, vân vân")]
        public async Task ListAsync()
        {
            var guild = Context.Guild;
            var online = guild.Users.Where(x => x.Status == UserStatus.Online).Count();
            var offline = guild.Users.Where(x => x.Status == UserStatus.Offline).Count();
            var dnd = guild.Users.Where(x => x.Status == UserStatus.DoNotDisturb).Count();
            var idle = guild.Users.Where(x => x.Status == UserStatus.Idle).Count();
            var invisible = guild.Users.Where(x => x.Status == UserStatus.Invisible).Count();
            var afk = guild.Users.Where(x => x.Status == UserStatus.AFK).Count();
            var total = guild.Users.Count();
            await ReplyAsync($"Online: {online}\nOffline: {offline}\nAFK: {afk}\nDND: {dnd}\nIdle: {idle}\nInvisible: {invisible}\nTổng cộng: {total}");
        }       
    }
}
