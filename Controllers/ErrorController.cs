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
                Message = "The page you are looking for could not be found.",
                Details = "A 404 error means that the requested resource could not be found. This can be due to a mistyped URL, a broken link, or a page that has been removed."
            };

            return View("Error", viewModel);
        }

        [Route("error/500")]
        public IActionResult Error500()
        {
            CustomErrorViewModel viewModel = new()
            {
                StatusCode = 500,
                Message = "An error occurred while processing your request.",
                Details = "A 500 error means that the server encountered an unexpected condition that prevented it from fulfilling the request."
            };

            return View("Error", viewModel);
        }

        [Route("error/503")]
        public IActionResult Error503()
        {
            CustomErrorViewModel viewModel = new()
            {
                StatusCode = 503,
                Message = "The service is temporarily unavailable. Please try again later.",
                Details = "A 503 error means that the server is currently unable to handle the request due to a temporary overload or maintenance of the server."
            };

            return View("Error", viewModel);
        }

        [Route("error/403")]
        public IActionResult Error403()
        {
            CustomErrorViewModel viewModel = new()
            {
                StatusCode = 403,
                Message = "Access Forbidden. You don't have permission to access this resource.",
                Details = "A 403 error means that the server understood the request but refuses to authorize it."
            };

            return View("Error", viewModel);
        }

        [Route("error/401")]
        public IActionResult Error401()
        {
            CustomErrorViewModel viewModel = new()
            {
                StatusCode = 401,
                Message = "Unauthorized. Please log in to access this resource.",
                Details = "A 401 error means that the requested resource requires authentication. This is usually returned by the server when the user is not authenticated."
            };

            return View("Error", viewModel);
        }

        [Route("error/400")]
        public IActionResult Error400()
        {
            CustomErrorViewModel viewModel = new()
            {
                StatusCode = 400,
                Message = "Bad Request. Your request is malformed or invalid.",
                Details = "A 400 error means that the server cannot process the request because the client-side input is invalid."
            };

            return View("Error", viewModel);
        }
    }
}
