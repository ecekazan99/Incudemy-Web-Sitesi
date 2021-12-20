using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angle.Models
{
    public class SurveyResponseModel
    {
        public int SurveyQuestionId { get; set; }
        public int SurveyQuestionAnswerId { get; set; }
        public int SurveyQuestionTempId { get; set; }
        public string AnswerText { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public List<SurveyResponseModel> surveyResponseModel { get; set; }
        public SurveyResponseModel()
        {
            this.surveyResponseModel = new List<SurveyResponseModel>();
        }
    }
}
