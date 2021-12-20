using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Microsoft.Data.SqlClient;

namespace Angle.Models
{
    public class EventLevel
    {
        public int EventLevelId { get; set; }
        public string EventLevelName { get; set; }
    }
}
