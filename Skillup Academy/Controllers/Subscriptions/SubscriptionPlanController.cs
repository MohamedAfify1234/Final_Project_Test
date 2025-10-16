using Core.Interfaces.Subscriptions;
using Core.Models.Exams;
using Core.Models.Subscriptions;
using Infrastructure.Repositories.Subscriptions;
using Infrastructure.Services.Subscriptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Skillup_Academy.ViewModels.ExamsViewModels;
using Skillup_Academy.ViewModels.SubscriptionsViewModels;

namespace Skillup_Academy.Controllers.Subscriptions
{
    public class SubscriptionPlanController : Controller
    {
        //_subscriptionPlanRepository _subscriptionPlanRepository = new _subscriptionPlanRepository();
        ISubscriptionPlanRepository _subscriptionPlanRepository;
        public SubscriptionPlanController(ISubscriptionPlanRepository subscriptionPlanRepository)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
        }

        // /SubscriptionPlan/ShowAll
        public IActionResult ShowAll()
        {
            List<SubscriptionPlan> SubscriptionPlanList = _subscriptionPlanRepository.ShowAll();
            return View("ShowAll", SubscriptionPlanList);
        }
        public IActionResult ShowDetails(Guid id)
        {
            SubscriptionPlan SubscriptionPlanDetails = _subscriptionPlanRepository.ShowDetails(id);
            return View("ShowDetails", SubscriptionPlanDetails);
        }

        public IActionResult Create()
        {
            SubscriptionPlanViewModel SPVM = new SubscriptionPlanViewModel();
            SPVM.Users = new SelectList(_subscriptionPlanRepository.ShowAll(), "Id", "FullName");
            SPVM.Courses = new SelectList(_subscriptionPlanRepository.ShowAll(), "Id", "Name");
            SPVM.Subscriptions = new SelectList(_subscriptionPlanRepository.ShowAll(), "Id", "Name");
            return View("Create", SPVM);
        }

        public IActionResult SaveCreate(SubscriptionPlanViewModel subscriptionplan)
        {
            if (ModelState.IsValid)
            {
                SubscriptionPlan subscriptionPlan = new SubscriptionPlan();
                subscriptionPlan.StartDate = subscriptionplan.StartDate;
                subscriptionPlan.EndDate = subscriptionplan.EndDate;
                subscriptionPlan.PaidAmount = subscriptionplan.PaidAmount;
                subscriptionPlan.TransactionId = subscriptionplan.TransactionId;
                subscriptionPlan.IsActive = subscriptionplan.IsActive;
                subscriptionPlan.UserId = subscriptionplan.UserId;
                subscriptionPlan.CourseId = subscriptionplan.CourseId;
                subscriptionPlan.SubscriptionId = subscriptionplan.SubscriptionId;
                _subscriptionPlanRepository.SubscriptionPlanAdd(subscriptionPlan);
                _subscriptionPlanRepository.Save();
                return RedirectToAction("ShowAll");
            }
            subscriptionplan.Users = new SelectList(_subscriptionPlanRepository.ShowAll(), "Id", "FullName");
            subscriptionplan.Courses = new SelectList(_subscriptionPlanRepository.ShowAll(), "Id", "Name");
            subscriptionplan.Subscriptions = new SelectList(_subscriptionPlanRepository.ShowAll(), "Id", "Name");
            return View(nameof(Create), subscriptionplan);
        }

        public IActionResult Edit(Guid id)
        {
            SubscriptionPlan subscriptionplanedit = _subscriptionPlanRepository.ShowDetails(id);
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(ShowAll));
            }
            return View("Edit", subscriptionplanedit);
        }
        public IActionResult SaveEdit(SubscriptionPlan subscriptionplansent, Guid id)
        {
            SubscriptionPlan Oldsubscriptionplan = _subscriptionPlanRepository.ShowDetails(id);
            if (ModelState.IsValid)
            {
                Oldsubscriptionplan.StartDate = subscriptionplansent.StartDate;
                Oldsubscriptionplan.EndDate = subscriptionplansent.EndDate;
                Oldsubscriptionplan.PaidAmount = subscriptionplansent.PaidAmount;
                Oldsubscriptionplan.TransactionId = subscriptionplansent.TransactionId;
                Oldsubscriptionplan.IsActive = subscriptionplansent.IsActive;
                _subscriptionPlanRepository.Update(Oldsubscriptionplan);
                _subscriptionPlanRepository.Save();
                return RedirectToAction(nameof(ShowAll));
            }
            return View("Edit", subscriptionplansent);
        }
        public IActionResult Delete(Guid id)
        {
            SubscriptionPlan subscriptionplandelete = _subscriptionPlanRepository.ShowDetails(id);
            return View("Delete", subscriptionplandelete);
        }
        public IActionResult SaveDelete(Guid id)
        {
            SubscriptionPlan subscriptionplandelete = _subscriptionPlanRepository.ShowDetails(id);
            if (subscriptionplandelete != null)
            {
                _subscriptionPlanRepository.DeleteFromDB(subscriptionplandelete);
                _subscriptionPlanRepository.Save();
                return RedirectToAction(nameof(ShowAll));
            }
            return NotFound();
        }
    }
}
