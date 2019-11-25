using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Hasco.Bot.Core.Domain;
using Hasco.Bot.Core.Repositories;
using Hasco.Bot.Core.UOW;
using Hasco.Bot.Infrastructure.DTO;
using Hasco.Bot.Infrastructure.Extensions;
using Hasco.Bot.Infrastructure.Extensions.ClientUsers;
using Hasco.Bot.Infrastructure.Services;
using Hasco.Bot.Infrastructure.Twitch;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Hasco.Bot.Api.Controllers
{
    [EnableCors("angular")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class UsersController : ApiBaseController
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private IClientUserRepository _clientUserRepository;
        private TwitchBot _twitchBot;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IClientUserRepository clientUserRepository,
            TwitchBot twitchBot)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _clientUserRepository = clientUserRepository;
            _twitchBot = twitchBot;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]Authenticate model)
        {
            var user = await _userService.Authenticate(model.ChannelName, model.Password);

            if (user == null)
                return BadRequest(new { message = "Channel Name or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);


            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Email = user.Email,
                ChannelName = user.ChannelName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]Register model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                await _userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);
            var model = _mapper.Map<UserDTO>(user);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]Update model)
        {
            // map model to entity and set id
            var user =  _mapper.Map<UserDTO>(model);
            user.Id = id;

            try
            {
                // update user 
                await _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.Delete(id);
            return Ok();
        }

        [Authorize]
        [HttpPost("connect")]
        public async Task<IActionResult> Connect()
        {
            var user = await _userService.GetById(UserId);

            await _twitchBot.ConnectToAnotherChannel(user.ChannelName);
            user.isOnline = true;

            return Ok();
        }

        [Authorize]
        [HttpPost("disconnect")]
        public async Task<IActionResult> Disconnect()
        {
            var user = await _userService.GetById(UserId);

            await _twitchBot.DisconnectFromChannel(user.ChannelName);
            user.isOnline = false;

            return Ok();
        }

        [HttpPost("channels")]
        public async Task<IActionResult> GetJoinedChannels()
        {
            var channels = await _twitchBot.GetJoinedChannels();

            return Ok(channels);
        }

        [HttpPost("message")]
        public async Task<IActionResult> SendMessage([FromBody]Message msg)
        {
            var user = await _userService.GetById(UserId);
            await _twitchBot.SendMessage(user.ChannelName, msg.MessageToSend);

            return Ok();
        }

        [HttpPost("timeout")]
        public async Task<IActionResult> TimeoutUser([FromBody]Viewer viewer)
        {
            var user = await _userService.GetById(UserId);
            await _twitchBot.SendMessage(user.ChannelName, $"/timeout {viewer.UserName} {viewer.Seconds}");

            return Ok();
        }

        [HttpPost("followers/on")]
        public async Task<IActionResult> SetOnFollowersMod([FromBody]Minutes minutes)
        {
            var user = await _userService.GetById(UserId);
            await _twitchBot.FollowersOn(user.ChannelName, minutes.Value);

            return Ok();
        }

        [HttpPost("followers/off")]
        public async Task<IActionResult> SetOffFollowersMod()
        {
            var user = await _userService.GetById(UserId);
            await _twitchBot.FollowersOff(user.ChannelName);

            return Ok();
        }

        [HttpPost("subscribers/on")]
        public async Task<IActionResult> SetOnSubscriberMod()
        {
            var user = await _userService.GetById(UserId);
            await _twitchBot.SubscriberOn(user.ChannelName);

            return Ok();
        }

        [HttpPost("subscribers/off")]
        public async Task<IActionResult> SetOffSubscriberMod()
        {
            var user = await _userService.GetById(UserId);
            await _twitchBot.SubscriberOff(user.ChannelName);

            return Ok();
        }

        [HttpPost("emoteonly/on")]
        public async Task<IActionResult> SetOnEmoteOnly()
        {
            var user = await _userService.GetById(UserId);
            await _twitchBot.EmoteOnlyOn(user.ChannelName);

            return Ok();
        }
        [HttpPost("emoteonly/off")]
        public async Task<IActionResult> SetOffEmoteOnly()
        {
            var user = await _userService.GetById(UserId);
            await _twitchBot.EmoteOnlyOff(user.ChannelName);

            return Ok();
        }

        [HttpPost("slow/on")]
        public async Task<IActionResult> SetOnSlowMod([FromBody]Seconds seconds)
        {
            var user = await _userService.GetById(UserId);
            await _twitchBot.SlowOn(user.ChannelName, seconds.Value);

            return Ok();
        }
        [HttpPost("slow/off")]
        public async Task<IActionResult> SetOffSlowMod()
        {
            var user = await _userService.GetById(UserId);
            await _twitchBot.SlowOff(user.ChannelName);

            return Ok();
        }

        [HttpPost("clearchat")]
        public async Task<IActionResult> ClearChat()
        {
            var user = await _userService.GetById(UserId);
            await _twitchBot.ClearChat(user.ChannelName);

            return Ok();
        }



        [HttpGet("test")]
        [Authorize]
        public async Task<IActionResult> TestMethod()
        {
            var user = await _userService.GetById(UserId);
            var model = _mapper.Map<UserDTO>(user);
            return Ok(model);
        }
    }
}