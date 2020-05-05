using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.WatchDog.Services
{
    public class SpamFilterService : IService
    {
        private readonly TelegramBotClient botClient;
        private readonly User user;

        public IEnumerable<UpdateType> RequiredUpdates => new UpdateType[] { UpdateType.Message, UpdateType.EditedMessage };


        public SpamFilterService(TelegramBotClient botClient, User user)
        {
            this.botClient = botClient;
            this.user = user;
        }


        public Task Initialize(CancellationToken cancellationToken)
        {
            this.botClient.OnMessage += this.BotClient_OnMessage;

            return Task.CompletedTask;
        }

        private void BotClient_OnMessage(object sender, Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type == MessageType.Photo && e.Message.Photo.Any())
            {
                var photo = e.Message.Photo.OrderByDescending(it => it.FileSize).First();

                using MemoryStream streamToRead = new MemoryStream();

                var file = this.botClient.GetInfoAndDownloadFileAsync(photo.FileId, streamToRead).GetAwaiter().GetResult();

                var extension = System.IO.Path.GetExtension(file.FilePath);

                string fileName = System.IO.Path.Combine("../../", $"{Guid.NewGuid().ToString("D")}{extension}");

                streamToRead.Position = 0;

                using (var streamToWrite = new FileStream(path: fileName, mode: FileMode.OpenOrCreate))
                {
                    streamToRead.CopyTo(streamToWrite);
                }

                RestSharp.RestClient client = new RestSharp.RestClient("http://localhost:32781/");
                RestSharp.RestRequest request = new RestSharp.RestRequest("Tesseract/ocr", RestSharp.Method.POST);
                request.AddFile("file", fileName);
                
                var response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    throw new InvalidOperationException("Error");
                }
                string output = response.Content;


            }
        }

        public Task Stop(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
