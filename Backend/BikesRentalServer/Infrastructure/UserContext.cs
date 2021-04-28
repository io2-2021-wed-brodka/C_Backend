using BikesRentalServer.Models;
using System;

namespace BikesRentalServer.Infrastructure
{
    public class UserContext
    {
        private bool _isSet;
        
        public string Username { get; private set; }
        public UserRole Role { get; private set; }

        public void SetOnce(string username, UserRole role)
        {
            if (_isSet)
                throw new InvalidOperationException("User context has already been set.");

            _isSet = true;
            Username = username;
            Role = role;
        }
    }
}
