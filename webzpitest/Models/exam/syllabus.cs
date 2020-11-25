using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.exam
{
    public class syllabus
    {
        public int studentcode { set; get; }
        public int subjectcode { set; get; }
        public int subjectsno { set; get; }
        public string subjectname { set; get; }      
        public string portionmidterm { set; get; }
        public string portionendterm { set; get; }
        public string semname { set; get; }
        public string remarks { set; get; }     
    }
    public class syllabusdet
    {
        public string syllabuslink { set; get; }      
    }
    public class studentsyllabus
    {
        public syllabusdet stusyllabus { set; get; }
    }


}