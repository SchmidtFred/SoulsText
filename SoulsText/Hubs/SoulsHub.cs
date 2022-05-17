using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SoulsText.Models;
using SoulsText.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace SoulsText.Hubs
{
    public class SoulsHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly ILogger<SoulsHub> _logger;

        public SoulsHub(IMessageRepository messageRepository, IUserProfileRepository userProfileRepository, IVoteRepository voteRepository, ILogger<SoulsHub> logger)
        {
            _messageRepository = messageRepository;
            _userProfileRepository = userProfileRepository;
            _voteRepository = voteRepository;
            _logger = logger;
            _logger.LogInformation("Hub Started");
        }


        //handle connection event
        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("User Attempting Connection");
            //make sure the connected user recieves all users upon connection
            try
            {
                var profiles = _userProfileRepository.GetAll();
                await Clients.Caller.SendAsync("ReceiveAllUsers", profiles);
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Something went wrong when attempting to connect to hub.");
                throw;
            }
        }

        public async Task UserLoggedIn(UserProfile profile)
        {
            _logger.LogInformation($"User - {profile.Id} - Logging In");
            try
            {
                //now that user is logged in, they recieve all messages
                var messages = _messageRepository.GetAll();
                await Clients.Caller.SendAsync("ReceiveAllMessages", messages);
                //to ping other users that new user has logged in (stretch goal)
                await Clients.Others.SendAsync("NewUser", profile);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Something went wrong with User - attempting to get all messages.");
                throw;
            }
        }

        public async Task SendMessage(Message message)
        {
            _logger.LogInformation($"User - ID: {message.UserProfileId} - sending message - {message.Content}");
            try
            {
                _messageRepository.Add(message);
                await Clients.All.SendAsync("ReceiveNewMessage", message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Something went wrong with User's - ID: {message.UserProfileId} - message");
                throw;
            }
        }

        public async Task RegisterUser(UserProfile user)
        {
            _logger.LogInformation("New User Registering");
            try
            {
                _userProfileRepository.Add(user);
                await Clients.Others.SendAsync("NewUser", user);
                await Clients.Caller.SendAsync("UserRegistered", user);
                _logger.LogInformation($"New User - ID: {user.Id} - registered");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Something went wrong with registering a new user");
            }
        }


        //for using the new vote and then creating an updated message
        public async Task SendVote(Vote vote)
        {
            _logger.LogInformation("New Vote Being Recieved");
            try
            {
                _voteRepository.Add(vote);
                var message = _messageRepository.GetById(vote.MessageId);
                await Clients.All.SendAsync("ReceiveUpdatedMessage", message);
                _logger.LogInformation($"New Vote - ID: {vote.Id} - created");
            } catch (Exception ex)
            {
                _logger.LogWarning(ex, "Something went wrong when creating new vote");
            }
        }
    }
}
