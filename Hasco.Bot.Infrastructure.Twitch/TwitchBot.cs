using Hasco.Bot.Core.Domain;
using Hasco.Bot.Core.Repositories;
using Hasco.Bot.Infrastructure.DTO;
using Hasco.Bot.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace Hasco.Bot.Infrastructure.Twitch
{
    public class TwitchBot
    {
        private bool _isReady = false;
        private static readonly TwitchClient _twitchClient = new TwitchClient();

        public TwitchBot()
        {
            ConnectionCredentials credentials = new ConnectionCredentials("hascobot", "7hgoswpihof58suvjq033b5dzbzib3");

            //_twitchClient = new TwitchClient();
            _twitchClient.Initialize(credentials);
            _twitchClient.Connect();
            _twitchClient.OnConnected += TwitchClientConnected;
            _twitchClient.OnJoinedChannel += Client_OnJoinedChannel;
            _twitchClient.OnMessageReceived += Client_OnMessageReceived;


    }

        private void TwitchClientConnected(object sender, OnConnectedArgs onConnectedArgs)
        {
            _isReady = true;
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.Contains("Hi"))
            {
                _twitchClient.SendMessage(e.ChatMessage.Channel, $"YO DUDE ");
            }
        }


        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            _twitchClient.SendMessage(e.Channel, $"Welcome on my chat {e.Channel}");
        }

        public async Task ConnectToAnotherChannel(string channelName)
        {
            _twitchClient.JoinChannel(channelName);

            await Task.CompletedTask;
        }

        public async Task DisconnectFromChannel(string channelName)
        {
            _twitchClient.LeaveChannel(channelName);

            await Task.CompletedTask;
        }

        public async Task SendMessage(string channelName, string message)
        {

            _twitchClient.SendMessage(channelName, message);

            await Task.CompletedTask;
        }

        public async Task TimeoutUser(string channelName, string userName, TimeSpan duration, string message)
        {
            var joinedChannel = _twitchClient.GetJoinedChannel(channelName);
            _twitchClient.TimeoutUser(joinedChannel,userName,duration,message);
            await Task.CompletedTask;
        }

        public async Task ClearChat(string channelName)
        {
            var joinedChannel = _twitchClient.GetJoinedChannel(channelName);
            _twitchClient.ClearChat(joinedChannel);

            await Task.CompletedTask;
        }

        public async Task FollowersOn(string channelName, int minutes)
        {
            _twitchClient.FollowersOnlyOn(channelName, new TimeSpan(0, 0, minutes, 0));

            await Task.CompletedTask;
        }

        public async Task FollowersOff(string channelName)
        {
            _twitchClient.FollowersOnlyOff(channelName);

            await Task.CompletedTask;
        }

        public async Task SubscriberOn(string channelName)
        {
            _twitchClient.SubscribersOnlyOn(channelName);

            await Task.CompletedTask;
        }
        public async Task SubscriberOff(string channelName)
        {
            _twitchClient.SubscribersOnlyOff(channelName);

            await Task.CompletedTask;
        }

        public async Task EmoteOnlyOn(string channelName)
        {
            _twitchClient.EmoteOnlyOn(channelName);

            await Task.CompletedTask;
        }
        public async Task EmoteOnlyOff(string channelName)
        {
            _twitchClient.EmoteOnlyOff(channelName);

            await Task.CompletedTask;
        }

        public async Task SlowOn(string channelName, int seconds)
        {
            _twitchClient.SlowModeOn(channelName, new TimeSpan(0, 0, 0, seconds));

            await Task.CompletedTask;

        }

        public async Task SlowOff(string channelName)
        {
            _twitchClient.SlowModeOff(channelName);

            await Task.CompletedTask;
        }



        // just only for tests
        public async Task<IReadOnlyList<JoinedChannel>> GetJoinedChannels()
        {
            var joinedChannels = _twitchClient.JoinedChannels;
            await Task.CompletedTask;

            return joinedChannels;
        }

        public TimeSpan AddSeconds(TimeSpan timeSpan, int secondsToAdd)
        {
            TimeSpan newSpan = new TimeSpan(0, 0, 0, secondsToAdd);
            return timeSpan.Add(newSpan);
        }
        public TimeSpan AddMinutes(TimeSpan timeSpan, int minutesToAdd)
        {
            TimeSpan newSpan = new TimeSpan(0, 0, 0, minutesToAdd);
            return timeSpan.Add(newSpan);
        }
    }
}