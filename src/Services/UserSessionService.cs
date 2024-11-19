using Inueco.Cinema.Models;

namespace Inueco.Cinema.Services;

public class UserSessionService() {
    private Dictionary<Guid, User> _userTokens = [];

    public Guid CreateToken(User user) {
        var token = Guid.NewGuid();
        _userTokens.Add(token, user);
        return token;
    }

    public bool ValidateToken(Guid token, out User user) {
        return _userTokens.TryGetValue(token, out user);
    }
}