using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Microsoft.Data.SqlClient;
namespace Angle.Models
{
    public class Category
    {

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

    }
}
