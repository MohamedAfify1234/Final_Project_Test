using Infrastructure.Services.Subscriptions;
using Core.Models.Exams;
using Core.Models.Subscriptions;
using Microsoft.AspNetCore.Mvc;

namespace Skillup_Academy.Controllers.Subscriptions
{
    public class SubscriptionController : Controller
    {
        SubscriptionBL subscriptionbl = new SubscriptionBL();
        // /Subscription/ShowAll
        public IActionResult ShowAll()
        {
            List<Subscription> subscriptionList = subscriptionbl.ShowAll();
            return View("ShowAll", subscriptionList);
        }
        public IActionResult ShowDetails(Guid id)
        {
            Subscription subscriptiondetails = subscriptionbl.ShowDetails(id);
            return View("ShowDetails", subscriptiondetails);
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        public IActionResult SaveCreate(Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                subscriptionbl.SubscriptionAdd(subscription);
                return RedirectToAction("ShowAll");
            }
            return View(nameof(Create), subscription);
        }

        public IActionResult Edit(Guid id)
        {
            Subscription subscriptionEdit = subscriptionbl.ShowDetails(id);
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(ShowAll));
            }
            return View("Edit", subscriptionEdit);
        }
        public IActionResult SaveEdit(Subscription subscriptionSent, Guid id)
        {
            Subscription Oldsubscription = subscriptionbl.ShowDetails(id);
            if (ModelState.IsValid)
            {
                Oldsubscription.DurationDays = subscriptionSent.DurationDays;
                Oldsubscription.Subscribes = subscriptionSent.Subscribes;
                Oldsubscription.MaxCourses = subscriptionSent.MaxCourses;
                Oldsubscription.Name = subscriptionSent.Name;
                Oldsubscription.Description = subscriptionSent.Description;
                Oldsubscription.Price = subscriptionSent.Price;
                Oldsubscription.Type = subscriptionSent.Type;
                subscriptionbl.SaveInDB();
                return RedirectToAction(nameof(ShowAll));
            }
            return View("Edit", subscriptionSent);
        }
        public IActionResult Delete(Guid id)
        {
            Subscription subscriptionDelete = subscriptionbl.ShowDetails(id);
            return View("Delete", subscriptionDelete);
        }
        public IActionResult SaveDelete(Guid id)
        {
            Subscription subscriptionDelete = subscriptionbl.ShowDetails(id);
            if (subscriptionDelete != null)
            {
                subscriptionbl.DeleteFromDB(subscriptionDelete);
                return RedirectToAction(nameof(ShowAll));
            }
            return NotFound();
        }
    }
}
