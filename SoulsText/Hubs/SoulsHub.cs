using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SoulsText.Models;
using SoulsText.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace SoulsText.Hubs
{
    public class SoulsHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IVoteRepository _voteRepository;

        public SoulsHub(IMessageRepository messageRepository, IUserProfileRepository userProfileRepository, IVoteRepository voteRepository)
        {
            _messageRepository = messageRepository;
            _userProfileRepository = userProfileRepository;
            _voteRepository = voteRepository;
        }

        //handle connection event
        public override async Task OnConnectedAsync()
        {
            //make sure the connected user recieves all messages upon connection
            var profiles = _userProfileRepository.GetAll();
            await Clients.Caller.SendAsync("ReceiveAllUsers", profiles);
            await base.OnConnectedAsync();
        }

        public async Task UserLoggedIn(UserProfile profile)
        {
            //now that user is logged in, they recieve all messages
            var messages = _messageRepository.GetAll();
            await Clients.Caller.SendAsync("ReceiveAllMessages", messages);
            //to ping other users that new user has logged in (stretch goal)
            await Clients.Others.SendAsync("NewUser", profile);
        }

        public async Task SendMessage(Message message)
        {
            _messageRepository.Add(message);
            await Clients.All.SendAsync("ReceiveNewMessage", message);
        }

        public async Task RegisterUser(UserProfile user)
        {
            _userProfileRepository.Add(user);
            await Clients.Others.SendAsync("NewUser", user);
            await Clients.Caller.SendAsync("UserRegistered", user);
        }


        //for using the new vote and then creating an updated message
        public async Task SendVote(Vote vote)
        {
            _voteRepository.Add(vote);
            var message = _messageRepository.GetById(vote.MessageId);
            await Clients.All.SendAsync("ReceiveUpdatedMessage", message);
        }
    }
}
