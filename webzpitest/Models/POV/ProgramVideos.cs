using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webzpitest.Models.POV
{
    public class ProgramVideos
    {
        public int studentcode { get; set; }
        public string admissionno { get; set; }
        public int student_id { get; set; }
        public int video_id { get; set; }
        public string videotitle { get; set; }
        public int videofuration { get; set; }
        public string videothumbnail { get; set; }
        public string videolink { get; set; }
        public string score { get; set; }
        public string status { get; set; }
        public string play { get; set; }
        public string completiondate { get; set; }
       

    }

    public class ProgramVideosDetail
    {
        public List<ProgramVideos> ProgramVideosDetails { get; set; }
    }
    public class ProgramVideosRequest
    {
        public string admissionno { get; set; }
        public int studentcode { get; set; }

    }

    public class POVLogin
    {
        public int student_id { get; set; }
        public string student_name { get; set; }
        public string batch_name { get; set; }
    }

    public class POVQuestions
    {
       
        public int questionno { get; set; }         
        public int questionid { get; set; }
        public string question { get; set; }
        public List<POVOptions> POVOptionsDetails { get; set; }
    }
    public class POVOptions
    {
        public int questionid { get; set; }
        public int optionid { get; set; }
        public string option { get; set; }
        public int correct_ans { get; set; }
    }
    public class POVQuestDetail
    {
        public int noofquestions { get; set; }
        public int student_id { get; set; }
        public int video_id { get; set; }
        public List<POVQuestions> POVQuestDetails { get; set; }
    }
   
    public class POVQuestRequest
    {
        public int video_id { get; set; }
        public int student_id { get; set; }
    }
   
    public class povcompletestatus
    {
        public string povstatus { get; set; }
    }
    public class povstatusdetails
    {
        public povcompletestatus povstatusdetail { get; set; }
    }


    public class POVTestQuestionResult
    {
        public int questionno { get; set; }    
        public int questionid { get; set; }
        public string question { get; set; }
        public List<POVOptionsResult> POVOptionsDetails { get; set; }
    }
    public class POVOptionsResult
    {
        public int questionid { get; set; }
        public int optionid { get; set; }
        public string option { get; set; }
        public int originalans { get; set; }
        public int studentans { get; set; }
    }
    public class POVAnswerDetail
    {
        public int noofquestions { get; set; }
        public int student_id { get; set; }
        public int video_id { get; set; }
        public List<POVTestQuestionResult> POVAnswerDetails { get; set; }
    }
    public class POVTestRequest
    {
        public int questionid { get; set; }
        public int student_id { get; set; }
        public int? optionid { get; set; } 
    }

    public class POVResultRequest
    {
        public int student_id { get; set; } 
        public int video_id { get; set; }
        public int[] questionid { get; set; }
        public int[] optionid { get; set; }
    }

    public class POVResult
    {
        public int student_id { get; set; }
        public int video_id { get; set; }
        public int noofquestions { get; set; }
        public int noofcorrectanswers { get; set; }     
        public float score { get; set; } 

    }
    public class POVResultdetails
    {      
        public POVResult POVResultdetail { get; set; }
    }













}