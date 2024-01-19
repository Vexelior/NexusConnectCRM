using Microsoft.AspNetCore.Mvc;
using NexusConnectCRM.ViewModels;

namespace NexusConnectCRM.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error/404")]
        public IActionResult Error404()
        {
            CustomErrorViewModel viewModel = new()
            {
                StatusCode = 404,
                Message = "The page you are looking for could not be found."
            };

            return View("Error", viewModel);
        }

        [Route("error/500")]
        public IActionResult Error500()
        {
            CustomErrorViewModel viewModel = new()
            {
                StatusCode = 500,
                Message = "An error occurred while processing your request."
            };

            return View("Error", viewModel);
        }

        [Route("error/503")]
        public IActionResult Error503()
        {
            CustomErrorViewModel viewModel = new()
            {
                StatusCode = 503,
                Message = "The service is temporarily unavailable. Please try again later."
            };

            return View("Error", viewModel);
        }

        [Route("error/403")]
        public IActionResult Error403()
        {
            CustomErrorViewModel viewModel = new()
            {
                StatusCode = 403,
                Message = "Access Forbidden. You don't have permission to access this resource."
            };

            return View("Error", viewModel);
        }

        [Route("error/401")]
        public IActionResult Error401()
        {
            CustomErrorViewModel viewModel = new()
            {
                StatusCode = 401,
                Message = "Unauthorized. Please log in to access this resource."
            };

            return View("Error", viewModel);
        }

        [Route("error/400")]
        public IActionResult Error400()
        {
            CustomErrorViewModel viewModel = new()
            {
                StatusCode = 400,
                Message = "Bad Request. Your request is malformed or invalid."
            };

            return View("Error", viewModel);
        }
    }
}
