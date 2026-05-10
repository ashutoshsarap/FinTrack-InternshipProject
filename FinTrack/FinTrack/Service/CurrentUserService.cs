using FinTrack.Service.IService;
using System.Security.Claims;

namespace FinTrack.Service
{
    public class CurrentUserService : ICurrentUserService
    {
        //context -> all the information about the current HTTP request, including the user making the request.
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        //public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        public string UserId
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
        }
    }
}
