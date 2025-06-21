using System;
using System.Collections.Generic;

namespace WebApiBudget.DomainOrCore.Models.DTOs
{
    public class UserGroupsUpdateDto
    {
        public List<Guid> GroupIds { get; set; } = new List<Guid>();
        public bool ReplaceExisting { get; set; } = false;
    }
}
