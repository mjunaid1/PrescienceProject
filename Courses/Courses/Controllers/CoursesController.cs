using Courses.DataAccess;
using Courses.Entities;
using Courses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Courses.Controllers
{
    public class CoursesController : ApiController
    {



        [HttpPost]
        public bool AddCourse([FromBody]CoursesModel CoursesModel)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var Result = _searchRepository.AddCourse(CoursesModel);
                return Result;
            }
            catch (Exception ex)
            {
              throw;
            }
        }


        [HttpPost]
        public bool AddModules([FromBody]Modules Model)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var Result = _searchRepository.AddModules(Model);
                return Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpGet]
        public List<CoursesModel> GetCourses()
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetCourses();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public List<Modules> GetModules()
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetModules();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet] 
        public List<Students> GetTeachers()
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetTeachers();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public bool AddCourseModules([FromBody]CourseModules Model)
        {
            try
            {
               

                CoursesRepository _searchRepository = new CoursesRepository();
                var Result = _searchRepository.AddCourseModules(Model);
                return Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost] 
        public bool AddCourseModuleForTeacher([FromBody]CourseModules Model)
        {
            try
            {


                CoursesRepository _searchRepository = new CoursesRepository();
                var Result = _searchRepository.AddCourseModuleForTeacher(Model);
                return Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public bool AddUserCourses([FromBody]UserCourses Model)
        {
            try
            {


                CoursesRepository _searchRepository = new CoursesRepository();
                var Result = _searchRepository.AddUserCourses(Model);
                return Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public bool AddUserCoursesForTeacher([FromBody]UserCourses Model)
        {
            try
            {


                CoursesRepository _searchRepository = new CoursesRepository();
                var Result = _searchRepository.AddUserCoursesForTeacher(Model);
                return Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet] 
        public List<Students> GetStudents()
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetStudents();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public Students CheckUser([FromBody]Students s)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.CheckUser(s);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public List<CourseModules> GetCourseModules()
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetCourseModules();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost] 
        public List<CourseModules> GetTeacherCourseModules([FromBody] CourseModules r)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetTeacherCourseModules(r.Username);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public List<UserCourses> GetUserCourses()
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetUserCourses();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        [HttpPost]
        public List<UserCourses> GetUserCoursesForTeacher(UserCourses Model)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetUserCoursesForTeacher(Model.Username);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public List<CoursesModel> GetCourses_Single_User([FromBody] RegisterViewModel r)
        {
            try
            {
              //  Username = "0f7520cd-d8e3-4c3f-b4e7-68d9a1a4832f";
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetCourses_Single_User(r.Email);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpPost]
        public List<Modules> GetCourseModules_Single_User([FromBody] CourseModules r)
        {
            try
            {
                //  Username = "0f7520cd-d8e3-4c3f-b4e7-68d9a1a4832f";
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetCourseModules_Single_User(r.Username,r.CourseId);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public List<StudentCoursePercentage> GetCourseProgress([FromBody] CourseModules r)
        {
            try
            {
                //  Username = "0f7520cd-d8e3-4c3f-b4e7-68d9a1a4832f";
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetCourseProgress(r.Username, r.CourseId);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet] 
        public List<Exams> GetExams()
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetExams();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

         [HttpPost] 
        public List<Exams> GetExamsForTeacher([FromBody] CoursesModel Model)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetExamsForTeacher(Model.TeacherUsername);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost] 
        public bool InsertExam([FromBody]Exams ExamModel)
        {
            try
            {
                ExamModel.Created = System.DateTime.Now;
                CoursesRepository _searchRepository = new CoursesRepository();
                var Result = _searchRepository.InsertExam(ExamModel);
                return Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        
       [HttpPost]
        public bool InsertQues([FromBody]Questions Model)
        {
            try
            {


                CoursesRepository _searchRepository = new CoursesRepository();
                var Result = _searchRepository.InsertQues(Model);
                return Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        public QuestionsAndAnswers ViewQuestionAndAnswers([FromBody]int ExamId)
        {
            try
            {

                CoursesRepository _searchRepository = new CoursesRepository();
                var Result = _searchRepository.ViewQuestionAndAnswers(ExamId);
                return Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public QuestionsAndAnswers ExamRecords([FromBody] EnrolledStudents model)
        {
            try
            {

                CoursesRepository _searchRepository = new CoursesRepository();
                var Result = _searchRepository.ExamRecords(model.ExamId,model.Username,model.Result);
                return Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        public bool deleteQues([FromBody]int QuesId)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var Result = _searchRepository.deleteQues(QuesId);
                return Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        

        [HttpPost]
        public bool EditQues([FromBody]Questions model)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var Result = _searchRepository.EditQues(model);
                return Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public QuestionsAndAnswers ViewExamQuestion([FromBody]QuestionPaginations Model)
        {
            try
            {

                CoursesRepository _searchRepository = new CoursesRepository();
                var Result = _searchRepository.ViewExamQuestion(Model);
                return Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost] 
        public List<Exams> getExamsPerCourse([FromBody]int CourseId)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.getExamsPerCourse(CourseId);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        public bool InsertResult([FromBody] Results model)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.InsertResult(model);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public List<EnrolledStudents> GetEnrolledStudents()
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetEnrolledStudents();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public List<EnrolledStudents> GetEnrolledCoursesStudent(CoursesModel Model)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetEnrolledCoursesStudent(Model.CourseID);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public List<CourseCountent> selectDropboxContent_Id([FromBody] CourseCountent model)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.selectDropboxContent_Id(model.ModuleId);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public bool InsertCourseModules_Content([FromBody] CourseCountent model)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.InsertCourseModules_Content(model);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost] 
        public bool InserContentProgress([FromBody] StudentContentProgress model)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.InserContentProgress(model);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        public List<StudentContentProgress> GetContentProgress([FromBody] StudentContentProgress model)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetContentProgress(model);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost] 
        public List<CoursesModel> GetSingleTeachersCourses([FromBody] CoursesModel Model)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetSingleTeachersCourses(Model.TeacherUsername);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost] 
        public bool UpdateTeacherComment([FromBody] EnrolledStudents model)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.UpdateTeacherComment(model.Comments,model.ResultId);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        
      [HttpPost]
        public List<EnrolledStudents> GetExamResult(CoursesModel Model)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.GetExamResult(Model.TeacherUsername);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public bool DeleteCourses([FromBody] CoursesModel Model)
        {
            try
            {
                CoursesRepository _searchRepository = new CoursesRepository();
                var result = _searchRepository.DeleteCourses(Model);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        // GET: api/Courses2
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Courses2/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Courses2
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Courses2/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Courses2/5
        public void Delete(int id)
        {
        }
    }
}
