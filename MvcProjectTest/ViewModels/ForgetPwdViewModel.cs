using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcProjectTest.ViewModels
{
    public class ForgetPwdViewModel
    {
        [Required(ErrorMessage = "請輸入帳號")]
        [Display(Name = "帳號")]
        
        public string CustomerAccount { get; set; }

        [Display(Name = "會員生日")]
        [DisplayFormat(DataFormatString ="{0:yyyy/MM/dd}",ApplyFormatInEditMode =true)]
        [Required(ErrorMessage = "請選擇生日")]
        [DataType(DataType.Date)]
        public DateTime CustomerBirth { get; set; }
    }
}