namespace FriendsManager.MVC.ViewModels.Friend
{
    public class RelocateFriendModel
    {
        public RelocateFriendModel() { }

        public RelocateFriendModel(Model.Friend friend) {
            this.FriendId = friend.FriendID;
            this.Place = friend.Place;
        }

        public int FriendId { get; set; }
        public string? Place { get; set; }
    }
}
