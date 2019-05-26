using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcProjectTest.ViewModels
{
    public class ResetPwdViewModel
    {
        [Display(Name = "會員密碼")]
        [Required]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,30}$", ErrorMessage = "至少有一個數字、小寫英文字母、大寫英文字母")]
        public string CustomerPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "確認密碼")]
        [Compare("CustomerPassword", ErrorMessage = "密碼和確認密碼不相符")]
        public string ConfirmPassword { get; set; }
    }
}