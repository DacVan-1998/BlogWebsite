using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebsite.Views.Home.Components
{
    [ViewComponent]
    public class RowTreeCategory : ViewComponent
    {
        public RowTreeCategory()
        {
        }
        // data là dữ liệu có cấu trúc
        // { 
        //    categories - danh sách các Category
        //    level - cấp của các Category 
        // }
        public IViewComponentResult Invoke(dynamic data)
        {
            return View(data);
        }
    }
}
