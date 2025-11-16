using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Core.Interfaces;
using Core.Models.Subscriptions;
using Core.Models.Users;
using Infrastructure.Services.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Skillup_Academy.ViewModels.PaymentViewModels;

namespace Skillup_Academy.Controllers.Subscriptions
{		
	[Authorize]
	public class PaymentController : Controller
	{
		private readonly IRepository<Subscription> _repoSubscription;
		private readonly PaymentByPaymob _paymentByPaymob;
		private readonly UserManager<User> _userManager;

		public PaymentController(IRepository<Subscription> repoSubscription, PaymentByPaymob paymentByPaymob, UserManager<User> userManager)
		{
			_repoSubscription = repoSubscription;
			_paymentByPaymob = paymentByPaymob;
			_userManager = userManager;
		}

		[HttpGet]
 		public async Task<IActionResult> Checkout(Guid planId)
		{
			var plan =await _repoSubscription.GetByIdAsync(planId);
			if (plan == null) return NotFound();


			var vm = new CheckoutViewModel
			{
				PlanId = plan.Id,
				Name = plan.Name,
				Description = plan.Description,
				Price = plan.Price,
				DurationDays = plan.DurationDays,
				MaxCourses = plan.MaxCourses
			};


			return View(vm);
		}
		 

		[HttpPost]
		public async Task<IActionResult> Processing(CheckoutViewModel checkoutVM) 
		{
			var user = _userManager.GetUserAsync(User).Result;
			if (user == null)
				return RedirectToAction("Login","Account");
			var plan = _repoSubscription.GetByIdAsync(checkoutVM.PlanId);

			if (checkoutVM.TypeMethod == PaymentMethod.Paymob.ToString())
			{
				var paymentUrl = await _paymentByPaymob.StartPaymentAsync(checkoutVM.Price,user);
				return Redirect(paymentUrl);	
			}

			return RedirectToAction("Checkout",checkoutVM);
		}




 		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> PaymobCallback()
		{
 			try
			{
				Request.EnableBuffering();
			}
			catch
			{
 			}

			using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
			var body = await reader.ReadToEndAsync();
 			try { Request.Body.Position = 0; } catch { }
			 
			try
			{
				using var doc = System.Text.Json.JsonDocument.Parse(body);
				var root = doc.RootElement;

 				string merchantOrderId = "";
				if (root.TryGetProperty("merchant_order_id", out var moid))
					merchantOrderId = moid.GetString();

 				string txnId = "";
				string status = "";
				if (root.TryGetProperty("transaction", out var tx))
				{
					if (tx.TryGetProperty("id", out var idProp)) txnId = idProp.GetRawText().Trim('"');
					if (tx.TryGetProperty("status", out var stProp)) status = stProp.GetRawText().Trim('"');
				}

 				if (!string.IsNullOrEmpty(merchantOrderId))
				{
					var user = await _userManager.GetUserAsync(User);
					user.CanViewPaidCourses = true;

				}

				return Ok();
			}
			catch (System.Text.Json.JsonException)
			{
 				return BadRequest("invalid json");
			}
			catch (Exception ex)
			{
  				return StatusCode(500);
			}
		}



		[AllowAnonymous]
		[HttpGet]
		public IActionResult PaymentResult()
		{ 
			return View();
		}


	}
}
