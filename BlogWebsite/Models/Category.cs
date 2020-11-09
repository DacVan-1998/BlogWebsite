using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebsite.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        //Category cha
        [DisplayName("Danh mục cha")]
        public int? ParentId { get; set; }

        //Tieu de Category
        [Required(ErrorMessage = "Phải có tên danh mục")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Tên danh mục")]
        public string Title { get; set; }

        //Noi dung chi tiet ve Category
        [DataType(DataType.Text)]
        [Display(Name = "Nội dung của danh mục")]
        public string Content { get; set; }

        //Chuoi Url
        [Required(ErrorMessage = "Phải tạo url")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [DisplayName("Url hiển thị")]
        public string Slug { get; set; }


        //Cac Category con
        public ICollection<Category> CategoryChildren { get; set; }

        [ForeignKey("ParentId")]
        public Category ParentCategory { set; get; }
    }
}
