using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using WebInventoryManagement.Services;

namespace WebInventoryManagement.Controllers
{
    public class ViewBagLoader
    {
        public static SelectList LoadViewBagList(IEnumerable values, string dataValueField, string dataTextField)
        {
            return new SelectList(values, dataValueField, dataTextField);
        }


    }
}
