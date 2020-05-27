using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace QuizManager.Models
{
    public class QuizManagerUser : IdentityUser
    {
        /*This attribute enable values for annotated properties to be included in any request from a user 
        to delete or download their personal data. If those areas of the Identity framework are disabled
        in this application, they can be removed*/
        [PersonalData]
        [Required]
        public string FirstName { get; set; }
        [PersonalData]
        [Required]
        public string LastName { get; set; }
    }
}