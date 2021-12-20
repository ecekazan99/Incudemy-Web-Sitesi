using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angle.Models
{
    public class QuestionModel
    {
        public int QuestionId { get; set; }
        public int countForQuestionId { get; set; }
        public int questionNum { get; set; }
        public string Question { get; set; }
        public List<OptionModel> Options { get; set; }
        public int CorrectAnswer { get; set; }
        public QuestionModel()
        {
            this.Options = new List<OptionModel>();
        }
    }
}
