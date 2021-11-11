using DistanceSchool.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistanceSchool.Web.ViewModels.Exams
{
    public class ExamViewModel
    {
        public string Id { get; set; }

        public Evaluation? Evaluation { get; set; }

        public DateTime StartDateTime { get; set; }
    }
}
