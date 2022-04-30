using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SoulsText.Models;
using SoulsText.Repositories;

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
            var messages = _messageRepository.GetAll();
            await Clients.Caller.SendAsync("ReceiveAllMessages", messages);
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(Message message)
        {
            _messageRepository.Add(message);
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task RegisterUser(UserProfile user)
        {
            _userProfileRepository.Add(user);
            await Clients.All.SendAsync("NewUser", user);
        }


        //for using the new vote and then creating an updated message
        public async Task SendVote(Vote vote)
        {
            _voteRepository.Add(vote);
            var message = _messageRepository.GetById(vote.MessageId);
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
