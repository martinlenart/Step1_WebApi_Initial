using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Step1_WebApi_Initials.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Step1_WebApi_Initial.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsController : Controller
    {
        private IMockupData _repo;
        private ILogger<FriendsController> _logger;

        //GET: api/friends
        //GET: api/friends/?count={count}
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Friend>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult Get(string count)
        {
            if (string.IsNullOrWhiteSpace(count))
            {
                _logger.LogInformation("GetFriends returned {count} items", _repo.Friends.Count);
                return Ok(_repo.Friends);
            }

            if (!int.TryParse(count, out int _count))
            {
                return BadRequest("count format error");
            }

            _count = Math.Min(_count, _repo.Friends.Count);

            _logger.LogInformation("GetFriends returned {_count} items", _count);
            return Ok(_repo.Friends.Take(_count));
        }

        //GET: api/friends/{Id}
        [HttpGet("{Id}", Name = nameof(GetFriend))]
        [ProducesResponseType(200, Type = typeof(Friend))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public IActionResult GetFriend(string Id)
        {
            if (!Guid.TryParse(Id, out Guid _friendGuid))
            {
                return BadRequest("Guid format error");
            }

            var friend = _repo.Friends.Find(f => f.FriendID == _friendGuid);
            if (friend == null)
            {
                return NotFound();
            }

            //cust is returned in the body
            return Ok(friend);
        }

        //PUT: api/friends/id
        //Body: Friend in Json
        [HttpPut("{Id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public IActionResult UpdateFriend(string Id, [FromBody] Friend friend)
        {
            if (!Guid.TryParse(Id, out Guid _friendGuid))
            {
                return BadRequest("Guid format error");
            }
            if (_friendGuid != friend.FriendID)
            {
                return BadRequest("ID mismatch");
            }


            var idx = _repo.Friends.FindIndex(f => f.FriendID == _friendGuid);
            if (idx != -1)
            {

                _repo.Friends[idx] = friend;
                _logger.LogInformation("Updated friend {_friendGuid}", _friendGuid);

                //Send an empty body response to confirm
                return new NoContentResult();
            }
            else
            {
                return NotFound();
            }
        }

        //POST: api/friends
        //Body: Customer in Json
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult CreateFriend([FromBody] Friend friend)
        {
            if (friend == null)
            {
                return BadRequest("No data in body");
            }

            var idx = _repo.Friends.FindIndex(f => f.FriendID == friend.FriendID);
            if (idx != -1)
            {
                return BadRequest("FriendID already existing");
            }

            _repo.Friends.Add(friend);

            //Response Alternatives
            //200 create ok with newlys created customer
            //return Ok(friend);

            //201 created ok with url details to read newlys created customer
            return CreatedAtRoute(

                //Named Route in the HttpGet request
                routeName: nameof(GetFriend),

                //custId is the parameter in HttpGet
                routeValues: new { Id = friend.FriendID.ToString().ToLower() },

                //Customer detail in the Body
                value: friend);
        }

        //DELETE: api/friends/id
        [HttpDelete("{Id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public IActionResult DeleteFriend(string Id)
        {
            if (!Guid.TryParse(Id, out Guid _friendGuid))
            {
                return BadRequest("Guid format error");
            }

            var idx = _repo.Friends.FindIndex(f => f.FriendID == _friendGuid);
            if (idx == -1)
            {
                return NotFound();
            }

            _repo.Friends.RemoveAt(idx);
            _logger.LogInformation("Deleted friend {_friendGuid}", _friendGuid);

            //Send an empty body response to confirm
            return new NoContentResult();
        }



        public FriendsController(IMockupData repo, ILogger<FriendsController> logger)
        {
            _repo = repo;
            _logger = logger;

            _logger.LogInformation($"FriendsController started.");
        }
    }
}

