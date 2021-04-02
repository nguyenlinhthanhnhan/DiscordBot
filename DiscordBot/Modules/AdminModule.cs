using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    [RequireContext(ContextType.Guild)]
    public class AdminModule:ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commandService;
        public AdminModule(CommandService commandService) => _commandService = commandService;

        [Command("admin")]
        [Summary("Các chức năng dành riêng cho Administrator")]
        public async Task HelpAsync()
        {
            List<CommandInfo> commands = _commandService.Commands.ToList();
            EmbedBuilder embedBuilder = new();

            foreach (CommandInfo command in commands)
            {
                if (command.Module.Name.Equals(nameof(AdminModule)))
                {
                    // Get the command symmary attribute information
                    string embedFieldText = command.Summary ?? "No description avaiable\n";
                    embedBuilder.AddField(command.Name, embedFieldText);
                }
            }
            await ReplyAsync("", false, embedBuilder.Build());
        }
        // Ban a user
        [Command("#ban")]
        [Summary("Block user khỏi group, cú pháp: !#ban @[tag user muốn ban]")]   
        // Make sure the bot itself can ban
        [RequireBotPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUserAsync(IGuildUser user, [Remainder] string reason = null)
        {
            await user.Guild.AddBanAsync(user, reason: reason);
            await ReplyAsync($"User {user.Username} đã bị block khỏi group");
        }

        // Unban a user
        [Command("unban")]
        [Summary("Gõ block user, cú pháp: !#unban @[tag user muốn unban]")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task UnbanUserAsync(IGuildUser user)
        {
            await user.Guild.RemoveBanAsync(user);
            await ReplyAsync($"User {user.Username} đã được xóa block");
        }

        // Kick a user
        [Command("kick")]
        [Summary("Đá user khỏi group, cú pháp: !#kick @[tag user muốn đá]")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task KickUserAsync(SocketGuildUser user, [Remainder] string reason = null)
        {
            await user.KickAsync(reason);
            await ReplyAsync($"User {user.Username} đã bị đá");
        }

        // Listed all role
        [Command("role")]
        [Summary("Liệt kê toàn bộ role hiện có")]
        public async Task ListedRolesAsync()
        {
            var user = Context.User as SocketGuildUser;
            var roles = (user as IGuildUser).Guild.Roles.ToList();
            EmbedBuilder embedBuilder = new();
            foreach(var role in roles)
            {
                embedBuilder.AddField("Name: " + role.Name, "ID: " + role.Id);
            }
            await ReplyAsync("", false, embedBuilder.Build());
        }

        // Set role for an user
        [Command("setrole")]
        [Summary("Đặt role cho user, cú pháp !setrole @user [Role ID], để biết Role ID, gõ !role")]
        public async Task SetRoleAsync(IGuildUser user, ulong roleID)
        {
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Id == roleID);
            await user.AddRoleAsync(role);
            await ReplyAsync($"{user.Username} đã được nâng cấp lên {role.Name}");
        }

        // Remove highest role for user
        [Command("remrole")]
        [Summary("Gỡ role cao nhất của user, cú pháp !remrole @user")]
        public async Task RemoveRoleAsync(IGuildUser user)
        {
            var roles = (user as SocketGuildUser).Roles.ToList();
            if (roles[0].Name.Contains("everyone") && roles.Count == 1) await ReplyAsync("User hiện không có role gì để gỡ ngoài everyone");
            else
            {
                await user.RemoveRoleAsync(roles[1]);
                await ReplyAsync($"Đã gỡ cấp {roles[1]} khỏi {user.Username}");
            }
        }
    }
}
