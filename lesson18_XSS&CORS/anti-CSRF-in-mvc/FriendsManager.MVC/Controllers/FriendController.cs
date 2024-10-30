using Microsoft.AspNetCore.Mvc;
using FriendsManager.Model;
using FriendsManager.MVC.ViewModels.Friend;

namespace FriendsManager.MVC.Controllers
{
    public class FriendController : Controller
    {
        private static List<Friend> _friends = new List<Friend>()
        {
            new Friend{FriendID=1, FriendName="John", Place="London"},
            new Friend{FriendID=2, FriendName="Jan", Place="Poland"},
            new Friend{FriendID=3, FriendName="Mikalaj", Place="Minsk"},
        };

        public IActionResult Index() // /friends
        {
            return View(_friends);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(Friend friend)
        {
            if (!ModelState.IsValid)
            {
                return View(friend);
            }

            _friends.Add(friend);


            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var friend = _friends
                .FirstOrDefault(f => f.FriendID == id);

            if (friend == null)
            {
                return NotFound();
            }

            return View(friend);
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Friend friend)
        {
            if (!ModelState.IsValid)
            {
                return View(friend);
            }

            var existingFriend = _friends
                .FirstOrDefault(f => f.FriendID == friend.FriendID);

            if (existingFriend == null)
            {
                return NotFound();
            }

            existingFriend.FriendName = friend.FriendName;
            existingFriend.Place = friend.Place;

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var existingFriend = _friends
                .FirstOrDefault(f => f.FriendID == id);

            if (existingFriend == null)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult ConfirmDelete(int id)
        {
            var existingFriendIndex = _friends
                .FindIndex(f => f.FriendID == id);

            if (existingFriendIndex == -1)
            {
                return NotFound();
            }

            _friends.RemoveAt(existingFriendIndex);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Relocate(int id)
        {
            var existingFriend = _friends
                .FirstOrDefault(f => f.FriendID == id);

            if (existingFriend == null)
            {
                return NotFound();
            }

            return View(new RelocateFriendModel(existingFriend));
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Relocate(RelocateFriendModel relocation)
        {
            if (!ModelState.IsValid)
            {
                return View(relocation);
            }

            var existingFriend = _friends
                .FirstOrDefault(f => f.FriendID == relocation.FriendId);

            if (existingFriend == null)
            {
                return NotFound();
            }

            existingFriend.Place = relocation.Place;

            return RedirectToAction(nameof(Index));
        }

    }
}
