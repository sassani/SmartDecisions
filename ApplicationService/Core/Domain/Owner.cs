using System;
using System.ComponentModel.DataAnnotations;

namespace ApplicationService.Core.Domain
{
    public class Owner
    {
        [MaxLength(36)]
        public string OwnerId { get; set; }

        public bool IsOwnedByUser(string userId)
        {
            return (OwnerId == userId);
        }
    }
}
