using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Angle.Models
{
    public class SurveyModel
    {
        public List<SurveyModel> surveyModel { get; set; }
        public int SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public string SurveyDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<QuestionModel> Questions { get; set; }
        public List<SurveyResponseModel> surveyResponse { get; set; }
        public int SurveyFormOrTest { get; set; }
        public SurveyModel()
        {
            this.Questions = new List<QuestionModel>();
            this.surveyModel = new List<SurveyModel>();
            this.surveyResponse = new List<SurveyResponseModel>();
        }
    }
}