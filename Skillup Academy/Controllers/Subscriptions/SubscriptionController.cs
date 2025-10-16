using Core.Interfaces.Subscriptions;
using Core.Models.Exams;
using Core.Models.Subscriptions;
using Infrastructure.Repositories.Subscriptions;
using Infrastructure.Services.Subscriptions;
using Microsoft.AspNetCore.Mvc;

namespace Skillup_Academy.Controllers.Subscriptions
{
    public class SubscriptionController : Controller
    {
        //_subscriptionRepository _subscriptionRepository = new _subscriptionRepository();
        ISubscriptionRepository _subscriptionRepository;
        public SubscriptionController(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        // /Subscription/ShowAll
        public IActionResult ShowAll()
        {
            List<Subscription> subscriptionList = _subscriptionRepository.ShowAll();
            return View("ShowAll", subscriptionList);
        }
        public IActionResult ShowDetails(Guid id)
        {
            Subscription subscriptiondetails = _subscriptionRepository.ShowDetails(id);
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
                _subscriptionRepository.SubscriptionAdd(subscription);
                _subscriptionRepository.Save();
                return RedirectToAction("ShowAll");
            }
            return View(nameof(Create), subscription);
        }

        public IActionResult Edit(Guid id)
        {
            Subscription subscriptionEdit = _subscriptionRepository.ShowDetails(id);
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(ShowAll));
            }
            return View("Edit", subscriptionEdit);
        }
        public IActionResult SaveEdit(Subscription subscriptionSent, Guid id)
        {
            Subscription Oldsubscription = _subscriptionRepository.ShowDetails(id);
            if (ModelState.IsValid)
            {
                Oldsubscription.DurationDays = subscriptionSent.DurationDays;
                Oldsubscription.Subscribes = subscriptionSent.Subscribes;
                Oldsubscription.MaxCourses = subscriptionSent.MaxCourses;
                Oldsubscription.Name = subscriptionSent.Name;
                Oldsubscription.Description = subscriptionSent.Description;
                Oldsubscription.Price = subscriptionSent.Price;
                Oldsubscription.Type = subscriptionSent.Type;
                _subscriptionRepository.Update(Oldsubscription);
                _subscriptionRepository.Save();
                return RedirectToAction(nameof(ShowAll));
            }
            return View("Edit", subscriptionSent);
        }
        public IActionResult Delete(Guid id)
        {
            Subscription subscriptionDelete = _subscriptionRepository.ShowDetails(id);
            return View("Delete", subscriptionDelete);
        }
        public IActionResult SaveDelete(Guid id)
        {
            Subscription subscriptionDelete = _subscriptionRepository.ShowDetails(id);
            if (subscriptionDelete != null)
            {
                _subscriptionRepository.DeleteFromDB(subscriptionDelete);
                _subscriptionRepository.Save();
                return RedirectToAction(nameof(ShowAll));
            }
            return NotFound();
        }
    }
}
