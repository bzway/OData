using OpenData.Framework.Entity;
using System.ComponentModel.DataAnnotations;
using System;
using OpenData.Common;

namespace OpenData.Framework.WebApp.Models
{
    public class MemberProfileViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public GenderType Gender { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public GradeType Grade { get; set; }
        public string Distinct { get; set; }
        public string Birthday { get; set; }
        public bool IsLunarBirthday { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedTime { get; set; }
        public bool IsConfirmed { get; set; }
        public string Roles { get; set; }
        public string OpenID { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
 
    public class MemberSearchViewModel
    {
        [Display(Name = "Name", ResourceType = typeof(ViewModelResource))]
        public string Name { get; set; }
        [Display(Name = "CardNumber", ResourceType = typeof(ViewModelResource))]
        public string CardNumber { get; set; }

        [Display(Name = "Email", ResourceType = typeof(ViewModelResource))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "MobileNumber", ResourceType = typeof(ViewModelResource))]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }

        [Display(Name = "Gender", ResourceType = typeof(ViewModelResource))]
        public GenderType? Gender { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public PagedList<MemberProfileViewModel> SearchResult { get; set; }
    }
    public class CreateMemberViewModel
    {
        [Display(Name = "Name", ResourceType = typeof(ViewModelResource))]
        public string Name { get; set; }

        [Display(Name = "Email", ResourceType = typeof(ViewModelResource))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "MobileNumber", ResourceType = typeof(ViewModelResource))]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }
        [Display(Name = "Gender", ResourceType = typeof(ViewModelResource))]
        public GenderType Gender { get; set; }

        [Display(Name = "NickName", ResourceType = typeof(ViewModelResource))]
        public string NickName { get; set; }
        [Display(Name = "Country", ResourceType = typeof(ViewModelResource))]
        public string Country { get; set; }
        [Display(Name = "Province", ResourceType = typeof(ViewModelResource))]
        public string Province { get; set; }
        [Display(Name = "City", ResourceType = typeof(ViewModelResource))]
        public string City { get; set; }
        [Display(Name = "Grade", ResourceType = typeof(ViewModelResource))]
        public GradeType Grade { get; set; }
        [Display(Name = "Distinct", ResourceType = typeof(ViewModelResource))]
        public string Distinct { get; set; }
        [Display(Name = "Birthday", ResourceType = typeof(ViewModelResource))]
        public string Birthday { get; set; }
        [Display(Name = "IsLunarBirthday", ResourceType = typeof(ViewModelResource))]
        public bool IsLunarBirthday { get; set; }
        [Display(Name = "IsLocked", ResourceType = typeof(ViewModelResource))]
        public bool IsLocked { get; set; }
        [Display(Name = "LockedTime", ResourceType = typeof(ViewModelResource))]
        public DateTime? LockedTime { get; set; }
        [Display(Name = "IsConfirmed", ResourceType = typeof(ViewModelResource))]
        public bool IsConfirmed { get; set; }
        [Display(Name = "Roles", ResourceType = typeof(ViewModelResource))]
        public string Roles { get; set; }
    }

}