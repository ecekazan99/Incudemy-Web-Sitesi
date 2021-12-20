using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angle.Models
{
    public class EventParticipant
    {
        public int EventParticipantListId { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
    }
}
