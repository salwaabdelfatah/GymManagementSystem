using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        public MembersController(IMemberService memberService) { 
            _memberService= memberService;
        }
        //GET BaseUrl/Members/Index
        //Index -List all members
         public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var members = await _memberService.GetALlMemberAsync(cancellationToken);
            return View(members);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model,CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);
            var result = await _memberService.CreateMemberAsync(model, ct);
            if (result)
                TempData["SucessMessage"] = "Member created sucessfully";
            else
                TempData["ErrorMessage"] = "Failed to Create member";

            return RedirectToAction(nameof(Index));
        }
        //Details
        public async Task<IActionResult> MemberDetails (int id ,CancellationToken ct)
        {
            //get member by id 
            var member = await _memberService.GetMemberDetailsById(id, ct);
            if(member is not null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        public async Task<IActionResult> HealthRecordDetails (int id ,CancellationToken ct)
        {
            var result = await _memberService.GetHealthRecordDetails(id, ct);
            if(result is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> EditMember (int id ,CancellationToken ct)
        {
            var member = await _memberService.GetMemberToUpdate(id, ct);
            if( member is null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpPost]

        public async Task<IActionResult> EditMember (int id,MemberToUpdateViewModel model,CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _memberService.UpdateMemberDetails(id, model, ct);
            if (result)
                TempData["SuccessMessage"] = "Member updated successfully";
            else
                TempData["ErrorMessage"] = "Failed to update Member";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete (int id,CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsById(id, ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id,CancellationToken ct)
        {
            var result = await _memberService.DeleteMemberAsync(id, ct);
            if (result)
                TempData["SuccessMessage"] = "Member deleted successfully";
            else
                TempData["ErrorMessage"] = "Failed to delete member";
            return RedirectToAction(nameof(Index));
        }
    }
}
