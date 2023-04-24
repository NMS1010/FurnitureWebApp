using FurnitureWeb.APICaller.Role;
using FurnitureWeb.ViewModels.System.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.AdminWebApp.Controllers
{
    [Route("~/admin/roles/")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleAPIClient _roleAPIClient;

        public RoleController(IRoleAPIClient roleAPIClient)
        {
            _roleAPIClient = roleAPIClient;
        }

        public async Task<IActionResult> Index(bool error = false)
        {
            var res = await _roleAPIClient.GetAllRoleAsync(new RoleGetPagingRequest());
            if (error || !res.IsSuccess)
                ViewData["Error"] = res.Errors;
            return View(res.Data);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create(RoleCreateRequest request)
        {
            var res = await _roleAPIClient.CreateRole(request);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccess });
        }

        [HttpGet("delete/{roleId}")]
        public async Task<IActionResult> Delete(string roleId)
        {
            var res = await _roleAPIClient.DeleteRole(roleId);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccess });
        }

        [Route("get/{roleId}")]
        public async Task<IActionResult> GetById(string roleId)
        {
            var res = await _roleAPIClient.GetRoleById(roleId);
            if (!res.IsSuccess)
                return NotFound(res.Errors);
            return Ok(res.Data);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(RoleUpdateRequest request)
        {
            var res = await _roleAPIClient.UpdateRole(request);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccess });
        }
    }
}