using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using Discord;

namespace DiscordBot.Services
{
    public class AutoSendMessageService
    {
        private static System.Timers.Timer timer;
        private readonly DiscordSocketClient _client;

        public AutoSendMessageService(DiscordSocketClient client)
        {
            _client = client;
        }

        public void Initialize()
        {
            timer = new();
            timer.Interval = 5400000;

            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            EmbedBuilder embedBuilder = new();
            embedBuilder.AddField("Thông báo tự động", "Kính chào anh em, đây là kênh hướng đến việc mua bán, trao đổi vật phẩm ingame, trao đổi về game và giúp đỡ lẫn nhau");
            embedBuilder.AddField("Nếu cần trợ giúp, đừng ngần ngại hỏi, nếu cần con bot nó giúp gì đó, gõ","!help để biết các command");
            embedBuilder.AddField("Hãy truy cập kênh youtube và fanpage để tìm kiếm thêm thông tin về game", "Youtube: www.youtube.com/channel/UCYQHzPweaiOwV8HH91lYjvw\nPage: www.facebook.com/PoE-C%C3%B9ng-Aqua-105144840974978");

            _client.GetGuild(753991000715690085)
                   .GetTextChannel(753991000715690087)
                   .SendMessageAsync("", false, embedBuilder.Build());
        }
    }
}
