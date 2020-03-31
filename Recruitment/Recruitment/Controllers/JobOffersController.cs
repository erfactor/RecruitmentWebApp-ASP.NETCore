using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Recruitment.Core;
using Recruitment.Data;
using Recruitment.Models;
using Recruitment.Services;
using Recruitment.Utils;
#pragma warning disable 4014

namespace Recruitment.Controllers
{
    public class JobOffersController : Controller
    {
        private readonly IApplicationData _applicationData;
        private readonly IConfiguration _configuration;
        private readonly IJobOfferData _jobOfferData;

        public JobOffersController(IJobOfferData jobOfferData, IApplicationData applicationData,
            IConfiguration configuration)
        {
            _jobOfferData = jobOfferData;
            _applicationData = applicationData;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var offers = _jobOfferData.GetAll();
            return View(offers);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var jobOffer = _jobOfferData.FindById(id);
            if (jobOffer == null) return View("NotFound");

            return View(jobOffer);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Apply(int id)
        {
            var jobOffer = _jobOfferData.FindById(id);
            if (jobOffer == null) return View("NotFound");

            var applyViewModel = new ApplyViewModel();
            applyViewModel.ApplicationId = 0;
            applyViewModel.Info = "sample info";
            applyViewModel.OfferName = jobOffer.Name;
            applyViewModel.JobOfferId = jobOffer.JobOfferId;

            return View(applyViewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var email = User.FindFirst("emails").Value;
            var application = _applicationData.GetUserApplications(email).FirstOrDefault(a => a.ApplicationId == id);
            if (application == null)
                return View("NotFound");
            var jobOffer = _jobOfferData.FindById(application.JobOfferId);
            if (jobOffer == null)
                return View("NotFound");
            
            var applyViewModel = new ApplyViewModel();
            applyViewModel.ApplicationId = application.ApplicationId;
            applyViewModel.Name = application.Name;
            applyViewModel.CommunicationEmail = application.CommunicationEmail;
            applyViewModel.Phone = application.Phone;
            applyViewModel.Info = application.Info;
            applyViewModel.OfferName = jobOffer.Name;
            applyViewModel.JobOfferId = jobOffer.JobOfferId;
            applyViewModel.CvFile = application.CvFile;

            return View(applyViewModel);
//            return View("Apply", applyViewModel);
        }

        [HttpPost]
        [RequestSizeLimit(5000000)]
        public async Task<IActionResult> Apply(ApplyViewModel applyViewModel)
        {
            var uploadResult = await UploadCvFile(applyViewModel);
            if (uploadResult!= null)
                return uploadResult;

            if (!SendApplicationToDatabase(applyViewModel))
                return View("Apply", applyViewModel);
            
            TempData["GoodUpload"] = "Application uploaded succesfully!";
            return RedirectToAction("Index","Applications");
        }

        private async Task<IActionResult> UploadCvFile(ApplyViewModel applyViewModel)
        {
            var formFile = applyViewModel.RealFile;
            if ((formFile == null || formFile.Length <= 0) && String.IsNullOrEmpty(applyViewModel.CvFile))
            {
                TempData["BadUpload"] = "File not specified or upload failed";
                return View("Apply", applyViewModel);
            }
            TempData["BadUpload"] = null;
            if (!ModelState.IsValid) return View("Apply", applyViewModel);

            if (formFile != null)
            {
                var uploadSuccess = false;
                string uploadedUri = null;
                using (var stream = formFile.OpenReadStream())
                {
                    (uploadSuccess, uploadedUri) = await BlobStorageService.UploadToBlob(formFile.FileName,
                        _configuration["storageconnectionstring"], null, stream);
                }
                if (!uploadSuccess)
                {
                    TempData["BadUpload"] = "File not specified or upload failed";
                    return View("Apply", applyViewModel);
                }
                applyViewModel.CvFile = uploadedUri;
            }

            return null;
        }
        
        private bool SendApplicationToDatabase(ApplyViewModel applyViewModel)
        {
            if (applyViewModel.ApplicationId > 0)
            {
                var application = _applicationData.FindById(applyViewModel.ApplicationId);
                if (application == null)
                {
                    return false;
                }
                application.Name = applyViewModel.Name;
                application.Phone = applyViewModel.Phone;
                application.CommunicationEmail = applyViewModel.CommunicationEmail;
                application.Info = applyViewModel.Info;
                application.CvFile = applyViewModel.CvFile;

                _applicationData.Update(application);
            }
            else
            {
                var jobOffer = _jobOfferData.FindById(applyViewModel.JobOfferId);
                if (jobOffer == null)
                    return false;
                var application = new Application();
                application.UserEmail = User.FindFirst("emails").Value;
                application.ApplicationId = 0;
                application.JobOfferId = applyViewModel.JobOfferId;
                application.OfferName = jobOffer.Name;
                application.Name = applyViewModel.Name;
                application.Phone = applyViewModel.Phone;
                application.CommunicationEmail = applyViewModel.CommunicationEmail;
                application.Info = applyViewModel.Info;
                application.CvFile = applyViewModel.CvFile;
                _applicationData.Add(application);

                var hrEmail = _jobOfferData.GetHrEmail(applyViewModel.JobOfferId);
                
                if (hrEmail != null)
                {
                    var apikey = _configuration["sendgridapikey"];
                    SendGridService.SendEmail(hrEmail, applyViewModel.OfferName, apikey, application.CvFile);
                }
            }

            _applicationData.Commit();
            return true;
        }

        [Authorize]
        [UserType(UserType.Hr)]
        [HttpGet]
        public IActionResult Create()
        {
            var jobOffer = new JobOffer();
            jobOffer.HrEmail = User.FindFirst("emails").Value;
            return View(jobOffer);
        }
        
        [Authorize]
        [UserType(UserType.Hr)]
        [HttpPost]
        public IActionResult Create(JobOffer jobOffer)
        {
            jobOffer.HrEmail = User.FindFirst("emails").Value;
            _jobOfferData.Create(jobOffer);
            _jobOfferData.Commit();
            return Json(new {status = "success"});
            //return RedirectToAction("Index");
        }
        
        [HttpPost]
        public IActionResult CheckEmail(string email, int offerId)
        {
            bool emailTaken = _jobOfferData.CheckIfEmailIsTaken(email, offerId);
            return Json(emailTaken ? new {status = "failure"} : new {status = "success"});
        }

        [HttpGet]
        public IActionResult AppNumber()
        {
            Dictionary<int, int> appNumbers = _jobOfferData.GetNumOfApplicants();
            return Json(appNumbers.ToList());
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveChanges(int appId, int offerId, string email, string phone, IFormFile file)
        {
            var application = _applicationData.FindById(appId);
            if (application == null)
            {
                return View("NotFound");
            }
            
            string uploadedUri = null;
            if (file != null)
            {
                var uploadSuccess = false;
                using (var stream = file.OpenReadStream())
                {
                    (uploadSuccess, uploadedUri) = await BlobStorageService.UploadToBlob(file.FileName,
                        _configuration["storageconnectionstring"], null, stream);
                }

                if (uploadSuccess)
                {
                    application.CvFile = uploadedUri;
                }
            }

            PhoneAttribute phoneAttr = new PhoneAttribute();
            bool phoneValid = phoneAttr.IsValid(phone);
            EmailAddressAttribute emailAttr = new EmailAddressAttribute();
            bool emailValid = emailAttr.IsValid(email);
            bool emailFree = !_jobOfferData.CheckIfEmailIsTaken(email, offerId);
            if (email == application.CommunicationEmail)
            {
                emailFree = true;
            }
            bool success = false;

            if (phoneValid && emailValid && emailFree)
            {
                application.Phone = phone;
                application.CommunicationEmail = email;
                _applicationData.Update(application);
                _applicationData.Commit();
                success = true;
            }

            var result = Json(new
            {
                success,
                phoneValid,
                emailValid,
                emailFree,
                uploadedUri
            });

            System.Threading.Thread.Sleep(700);
            return result;
        }
    }
}