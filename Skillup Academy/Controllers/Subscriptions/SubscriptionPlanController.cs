using Infrastructure.Services.Subscriptions;
using Core.Models.Exams;
using Core.Models.Subscriptions;
using Microsoft.AspNetCore.Mvc;

namespace Skillup_Academy.Controllers.Subscriptions
{
    public class SubscriptionPlanController : Controller
    {
        SubscriptionPlanBL subscriptionplanbl = new SubscriptionPlanBL();
        // /SubscriptionPlan/ShowAll
        public IActionResult ShowAll()
        {
            List<SubscriptionPlan> SubscriptionPlanList = subscriptionplanbl.ShowAll();
            return View("ShowAll", SubscriptionPlanList);
        }
        public IActionResult ShowDetails(Guid id)
        {
            SubscriptionPlan SubscriptionPlanDetails = subscriptionplanbl.ShowDetails(id);
            return View("ShowDetails", SubscriptionPlanDetails);
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        public IActionResult SaveCreate(SubscriptionPlan subscriptionplan)
        {
            if (ModelState.IsValid)
            {
                subscriptionplanbl.SubscriptionPlanAdd(subscriptionplan);
                return RedirectToAction("ShowAll");
            }
            return View(nameof(Create), subscriptionplan);
        }

        public IActionResult Edit(Guid id)
        {
            SubscriptionPlan subscriptionplanedit = subscriptionplanbl.ShowDetails(id);
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(ShowAll));
            }
            return View("Edit", subscriptionplanedit);
        }
        public IActionResult SaveEdit(SubscriptionPlan subscriptionplansent, Guid id)
        {
            SubscriptionPlan Oldsubscriptionplan = subscriptionplanbl.ShowDetails(id);
            if (ModelState.IsValid)
            {

                subscriptionplanbl.SaveInDB();
                return RedirectToAction(nameof(ShowAll));
            }
            return View("Edit", subscriptionplansent);
        }
        public IActionResult Delete(Guid id)
        {
            SubscriptionPlan subscriptionplandelete = subscriptionplanbl.ShowDetails(id);
            return View("Delete", subscriptionplandelete);
        }
        public IActionResult SaveDelete(Guid id)
        {
            SubscriptionPlan subscriptionplandelete = subscriptionplanbl.ShowDetails(id);
            if (subscriptionplandelete != null)
            {
                subscriptionplanbl.DeleteFromDB(subscriptionplandelete);
                return RedirectToAction(nameof(ShowAll));
            }
            return NotFound();
        }
    }
}
