using AuthServerJWT.Core.Configuration;
using AuthServerJWT.Core.Dtos;
using AuthServerJWT.Core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServerJWT.Core.Interfaces
{
    public interface ITokenService
    {
        TokenDto CreateToken(UserApp userApp);
        ClientTokenDto CreateTokenByClient(Client client);
    }
}
