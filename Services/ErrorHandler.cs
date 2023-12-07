using Microsoft.AspNetCore.Mvc;
using WebInventoryManagement.Models;

namespace WebInventoryManagement.Services
{
    public static class ErrorHandler
    {
        public static ActionResult Error(this Controller controller, int? statusCode, string message = null)
        {
            var errorModel = new ErrorViewModel();

            if (statusCode.HasValue)
            {
                errorModel.StatusCode = statusCode.Value;
                errorModel.ErrorMessage = "An error occurred with status code: " + statusCode.Value;
            }
            else if (!string.IsNullOrEmpty(message))
            {
                errorModel.StatusCode = 500; // Вы можете изменить этот код на более подходящий
                errorModel.ErrorMessage = message;
            }
            else
            {
                errorModel.StatusCode = 500; // Internal Server Error
                errorModel.ErrorMessage = "An unexpected error occurred.";
            }


            return controller.View("Error", errorModel);
        }
    }
}
