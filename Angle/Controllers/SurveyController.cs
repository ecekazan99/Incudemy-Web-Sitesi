using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Angle.Models;
using Angle.Helpers;
using Microsoft.Data.SqlClient;
using RestSharp;
using Microsoft.AspNetCore.Http;
using System.Data;
using Newtonsoft.Json;

namespace Angle.Controllers
{
    public class SurveyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Survey()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            SurveyModel model = new SurveyModel();
            var surveys = new Dictionary<string, object>();
            surveys.Add("UserId", userId);

            var surveyList = DataHelper.ListFromStoredProcedure("Sp_DisplaySurveyByUser_Survey", surveys);

            foreach (DataRow item in surveyList.Rows)
            {
                SurveyModel sModel = new SurveyModel();
                sModel.SurveyId = item.Field<int>("SurveyId");
                sModel.SurveyDescription = item.Field<string>("SurveyDescription");
                sModel.SurveyTitle = item.Field<string>("SurveyTitle");
                sModel.StartDate = item.Field<DateTime>("StartDate");
                sModel.EndDate = item.Field<DateTime>("EndDate");
                model.surveyModel.Add(sModel);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult SurveyFormOrTest()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SurveyFormOrTest(SurveyModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var StartDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var EndDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            var survey = new Dictionary<string, object>();
            survey.Add("SurveyTitle", model.SurveyTitle);
            survey.Add("SurveyDescription", model.SurveyDescription);
            survey.Add("StartDate", StartDate);
            survey.Add("EndDate", EndDate);
            survey.Add("IsResponsesDisplayed", 1);
            survey.Add("DeletedByUserId", userId);

            var surveyQuestion = new Dictionary<string, object>();
            surveyQuestion.Add("DeletedByUserId", userId);

            if (model.SurveyFormOrTest == 0)
                surveyQuestion.Add("QuestionTypeId", 1);
            else if (model.SurveyFormOrTest == 1)
                surveyQuestion.Add("QuestionTypeId", 2);

            surveyQuestion.Add("IsResponsesMandatory",1);
            surveyQuestion.Add("SortOrder", 1);
            surveyQuestion.Add("SurveyQuestionCreatedDate", StartDate);
            surveyQuestion.Add("SurveyQuestionModifiedDate", EndDate);
            surveyQuestion.Add("SurveyQuestionResponseId", 1);

            var surveyQuestionAnswer = new Dictionary<string, object>();
            surveyQuestionAnswer.Add("AnswerModifiedDate", StartDate);
            surveyQuestionAnswer.Add("AnswerCreatedDate", EndDate);
            surveyQuestionAnswer.Add("DeletedByUserId", userId);
            surveyQuestionAnswer.Add("IsDeleted", 0);
            surveyQuestionAnswer.Add("IsActive", 1);

            var sp_Survey = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_Survey", survey);

            foreach (var item in model.Questions)
            {
                surveyQuestion.Add("QuestionText", item.Question);

                var sp_SurveyQuestion = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_SurveyQuestion", surveyQuestion);

                var correctId = item.CorrectAnswer;
                var temp = 0;
                foreach (var itm in item.Options)
                {
                    surveyQuestionAnswer.Add("AnswerText", itm.AnswerText);
                    if(correctId==temp && model.SurveyFormOrTest==1)
                        surveyQuestionAnswer.Add("IsCorrectAnswer",true);
                    else
                        surveyQuestionAnswer.Add("IsCorrectAnswer", false);
                    temp++;

                    var sp_SurveyQuestionAnswer = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_SurveyQuestionAnswer", surveyQuestionAnswer);
                    surveyQuestionAnswer.Remove("AnswerText");
                    surveyQuestionAnswer.Remove("IsCorrectAnswer");
                }
                surveyQuestion.Remove("QuestionText");
            }
            return Redirect("Survey");
        }
        [HttpGet]
        public IActionResult DisplaySurvey()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DisplaySurvey(int id)
        {
            SurveyModel model = new SurveyModel();

            var surveys = new Dictionary<string, object>();
            surveys.Add("SurveyId", id);

            var getSurveyDetail = DataHelper.ListFromStoredProcedure("Sp_Display_Survey", surveys);
        
            foreach (DataRow item in getSurveyDetail.Rows)
            { 
                model.SurveyId = item.Field<int>("SurveyId");
                model.SurveyDescription = item.Field<string>("SurveyDescription");
                model.SurveyTitle = item.Field<string>("SurveyTitle");

                var getSurveyQuestion = DataHelper.ListFromStoredProcedure("Sp_DisplaySurvey_Question", surveys);
                var qId = 0;

                foreach (DataRow itemQuestion in getSurveyQuestion.Rows)
                {
                    QuestionModel qModel = new QuestionModel();
                    qModel.Question = itemQuestion.Field<string>("QuestionText");
                    qModel.countForQuestionId = qId;
                    qModel.questionNum = qId + 1;
                    qModel.QuestionId = itemQuestion.Field<int>("SurveyQuestionId");
                    qId++;

                    var surveyQuestionId = new Dictionary<string, object>();
                    surveyQuestionId.Add("SurveyQuestionId", itemQuestion.Field<int>("SurveyQuestionId"));

                    var getSurveyAnswer = DataHelper.ListFromStoredProcedure("Sp_DisplaySurvey_Answer", surveyQuestionId);

                    foreach (DataRow itemAnswer in getSurveyAnswer.Rows)
                    {
                        OptionModel opModel = new OptionModel();
                        opModel.AnswerText = itemAnswer.Field<string>("AnswerText");
                        opModel.SurveyQuestionAnswerId = itemAnswer.Field<int>("SurveyQuestionAnswerId");
                        qModel.Options.Add(opModel);
                    }
                    model.Questions.Add(qModel);
                }
                model.surveyModel.Add(model);
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult InsertResponseSurvey(SurveyModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var ResponseDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var ResponseUpdateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            var surveyResponse = new Dictionary<string, object>();
            surveyResponse.Add("UserId", userId);
            surveyResponse.Add("DeletedByUserId", userId);
            surveyResponse.Add("ResponseDate", ResponseDate);
            surveyResponse.Add("ResponseUpdateDate", ResponseUpdateDate);

            foreach (var item in model.surveyResponse)
            {
                surveyResponse.Add("SurveyQuestionId", item.SurveyQuestionId);
                surveyResponse.Add("SurveyQuestionAnswerId", item.SurveyQuestionAnswerId);
                surveyResponse.Add("AnswerText", item.AnswerText);

                var sp_SurveyQuestionAnswer = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_SurveyResponse", surveyResponse);

                surveyResponse.Remove("SurveyQuestionId");
                surveyResponse.Remove("SurveyQuestionAnswerId");
                surveyResponse.Remove("AnswerText");
            }
            return Redirect("Survey");
        }
        [HttpPost]
        public IActionResult DisplayResponseSurvey(int id)
        {
            SurveyModel model = new SurveyModel();

            var surveys = new Dictionary<string, object>();
            surveys.Add("SurveyId", id);

            var getSurveyResponseUserId = DataHelper.ListFromStoredProcedure("Sp_DisplayResponse_UserId", surveys);

            foreach (DataRow item in getSurveyResponseUserId.Rows)
            {
                SurveyResponseModel modelUser = new SurveyResponseModel();
                var questionId = new Dictionary<string, object>();
                questionId.Add("UserId", item.Field<int>("UserId"));
                questionId.Add("SurveyId", id);

                var getSurveyResponse = DataHelper.ListFromStoredProcedure("Sp_DisplaySurvey_Response", questionId);
                var temp = 1;
                foreach (DataRow itemRes in getSurveyResponse.Rows)
                {
                    SurveyResponseModel responseModel = new SurveyResponseModel();
                    modelUser.UserFirstName = itemRes.Field<string>("FirstName");
                    modelUser.UserLastName = itemRes.Field<string>("LastName");
                    responseModel.SurveyQuestionId = itemRes.Field<int>("SurveyQuestionId");
                    responseModel.SurveyQuestionTempId = temp;
                    responseModel.SurveyQuestionAnswerId = itemRes.Field<int>("SurveyQuestionAnswerId");
                    responseModel.AnswerText = itemRes.Field<string>("AnswerText");
                    modelUser.surveyResponseModel.Add(responseModel);
                    temp++;
                }
                model.surveyResponse.Add(modelUser);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult DisplayAllQuestionsAndOptions()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DisplayAllQuestionsAndOptions(int id)
        {
            SurveyModel model = new SurveyModel();

            var surveys = new Dictionary<string, object>();
            surveys.Add("SurveyId", id);

            var getSurveyDetail = DataHelper.ListFromStoredProcedure("Sp_Display_Survey", surveys);

            foreach (DataRow item in getSurveyDetail.Rows)
            {
                model.SurveyId = item.Field<int>("SurveyId");
                model.SurveyDescription = item.Field<string>("SurveyDescription");
                model.SurveyTitle = item.Field<string>("SurveyTitle");

                var getSurveyQuestion = DataHelper.ListFromStoredProcedure("Sp_DisplaySurvey_Question", surveys);
                var qId = 0;

                foreach (DataRow itemQuestion in getSurveyQuestion.Rows)
                {
                    QuestionModel qModel = new QuestionModel();
                    qModel.Question = itemQuestion.Field<string>("QuestionText");
                    qModel.countForQuestionId = qId;
                    qModel.questionNum = qId + 1;
                    qModel.QuestionId = itemQuestion.Field<int>("SurveyQuestionId");
                    qId++;

                    var surveyQuestionId = new Dictionary<string, object>();
                    surveyQuestionId.Add("SurveyQuestionId", itemQuestion.Field<int>("SurveyQuestionId"));

                    var getSurveyAnswer = DataHelper.ListFromStoredProcedure("Sp_DisplaySurvey_Answer", surveyQuestionId);
                    var temp = 0;
                    qModel.CorrectAnswer = -1;
                    foreach (DataRow itemAnswer in getSurveyAnswer.Rows)
                    {
                        OptionModel opModel = new OptionModel();
                        opModel.AnswerText = itemAnswer.Field<string>("AnswerText");
                        opModel.SurveyQuestionAnswerId = itemAnswer.Field<int>("SurveyQuestionAnswerId");

                        if (itemAnswer.Field<bool>("IsCorrectAnswer"))
                        {
                            qModel.CorrectAnswer = temp;
                        }
                        opModel.countForOptionId = temp;
                        temp++;
                        qModel.Options.Add(opModel);
                    }
                    model.Questions.Add(qModel);
                }
                model.surveyModel.Add(model);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult DeleteSurvey()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DeleteSurvey(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var DeletedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            var deleteSurvey = new Dictionary<string, object>();
            deleteSurvey.Add("SurveyId", id);
            deleteSurvey.Add("IsActive", 0);
            deleteSurvey.Add("IsDeleted", 1);
            deleteSurvey.Add("DeletedByUserId", userId);

            var deleteSurveyQuestion = new Dictionary<string, object>();
            deleteSurveyQuestion.Add("DeletedByUserId", userId);
            deleteSurveyQuestion.Add("DeletedDate", DeletedDate);
            deleteSurveyQuestion.Add("IsDeleted", 1);
            deleteSurveyQuestion.Add("IsActive", 0);

            var deleteSurveyQuestionAnswer = new Dictionary<string, object>();
            deleteSurveyQuestionAnswer.Add("AnswerModifiedDate", DeletedDate);
            deleteSurveyQuestionAnswer.Add("DeletedByUserId", userId);
            deleteSurveyQuestionAnswer.Add("IsDeleted", 1);
            deleteSurveyQuestionAnswer.Add("IsActive", 0);

            var sp_Survey = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_Survey", deleteSurvey);

            var surveyId = new Dictionary<string, object>();
            surveyId.Add("SurveyId", id);
            var getSurveyQuestion = DataHelper.ListFromStoredProcedure("Sp_DisplaySurvey_Question", surveyId);
            foreach (DataRow itemQuestion in getSurveyQuestion.Rows)
            {
                deleteSurveyQuestion.Add("SurveyQuestionId", itemQuestion.Field<int>("SurveyQuestionId"));
                var sp_SurveyQuestion = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_SurveyQuestion", deleteSurveyQuestion);

                var surveyQuestionId = new Dictionary<string, object>();
                surveyQuestionId.Add("SurveyId", id);
                var getSurveyQuestionAnswer = DataHelper.ListFromStoredProcedure("Sp_DisplaySurvey_Question", surveyQuestionId);
                foreach (DataRow itemAnswer in getSurveyQuestionAnswer.Rows)
                {
                    deleteSurveyQuestionAnswer.Add("SurveyQuestionId", itemAnswer.Field<int>("SurveyQuestionId"));
                    var sp_SurveyQuestionAnswer = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_SurveyQuestionAnswer", deleteSurveyQuestionAnswer);

                    deleteSurveyQuestionAnswer.Remove("SurveyQuestionId");
                }
                deleteSurveyQuestion.Remove("SurveyQuestionId");
            }

            return Redirect("Survey");
        }
        public IActionResult _AddQuestion()
        {
            return View();
        }
        public IActionResult _AddOption()
        {
            return View();
        }
        public ActionResult AddNewQuestion()
        {
            return PartialView("_AddQuestion");
        }
        public ActionResult AddNewOption()
        {
            return PartialView("_AddOption");
        }
    }
}
