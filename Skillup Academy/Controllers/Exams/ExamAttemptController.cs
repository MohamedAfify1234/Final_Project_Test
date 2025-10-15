using Core.Models.Exams;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services.Exams;


namespace Skillup_Academy.Controllers.Exams
{
    public class ExamAttemptController : Controller
    {
        ExamAttemptBL examattemptbl = new ExamAttemptBL();
        // /ExamAttempt/ShowAll
        public IActionResult ShowAll()
        {
            List<ExamAttempt> ExamList = examattemptbl.ShowAll();
            return View("ShowAll", ExamList);
        }
        public IActionResult ShowDetails(Guid id)
        {
            ExamAttempt examAttempt = examattemptbl.ShowDetails(id);
            return View("ShowDetails", examattemptbl);
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        public IActionResult SaveCreate(ExamAttempt exam)
        {
            if (ModelState.IsValid)
            {
                examattemptbl.ExamAttemptAdd(exam);
                return RedirectToAction("ShowAll");
            }
            return View(nameof(Create), exam);
        }

        public IActionResult Edit(Guid id)
        {
            ExamAttempt ExamEdit = examattemptbl.ShowDetails(id);
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(ShowAll));
            }
            return View("Edit", ExamEdit);
        }
        public IActionResult SaveEdit(ExamAttempt ExamAttemptSent, Guid id)
        {
            ExamAttempt OldExamAttempt = examattemptbl.ShowDetails(id);
            if (ModelState.IsValid)
            {
                OldExamAttempt.StartTime = ExamAttemptSent.StartTime;
                OldExamAttempt.EndTime = ExamAttemptSent.EndTime;
                OldExamAttempt.Score = ExamAttemptSent.Score;
                OldExamAttempt.TotalQuestions = ExamAttemptSent.TotalQuestions;
                OldExamAttempt.CorrectAnswers = ExamAttemptSent.CorrectAnswers;
                OldExamAttempt.IsPassed = ExamAttemptSent.IsPassed;
                OldExamAttempt.AttemptNumber = ExamAttemptSent.AttemptNumber;
                examattemptbl.SaveInDB();
                return RedirectToAction(nameof(ShowAll));
            }
            return View("Edit", ExamAttemptSent);
        }
        public IActionResult Delete(Guid id)
        {
            ExamAttempt ExamDelete = examattemptbl.ShowDetails(id);
            return View("Delete", ExamDelete);
        }
        public IActionResult SaveDelete(Guid id)
        {
            ExamAttempt ExamDelete = examattemptbl.ShowDetails(id);
            if (ExamDelete != null)
            {
                examattemptbl.DeleteFromDB(ExamDelete);
                return RedirectToAction(nameof(ShowAll));
            }
            return NotFound();
        }
    }
}
