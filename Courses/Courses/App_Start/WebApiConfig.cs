using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Courses
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);


            config.Routes.MapHttpRoute(
                     name: "AddCourse",
                      routeTemplate: "api/Search/AddCourse",
                          defaults: new { controller = "Courses", action = "AddCourse" }
              );


            config.Routes.MapHttpRoute(
                 name: "AddModules",
                  routeTemplate: "api/Search/AddModules",
                      defaults: new { controller = "Courses", action = "AddModules" }
             );

            config.Routes.MapHttpRoute(
                name: "GetCourses",
                 routeTemplate: "api/Search/GetCourses",
                     defaults: new { controller = "Courses", action = "GetCourses" }
            );

            config.Routes.MapHttpRoute(
               name: "GetModules",
                routeTemplate: "api/Search/GetModules",
                    defaults: new { controller = "Courses", action = "GetModules" }
           );

            
            config.Routes.MapHttpRoute(
               name: "GetTeachers",
                routeTemplate: "api/Search/GetTeachers",
                    defaults: new { controller = "Courses", action = "GetTeachers" }
           );
            
            config.Routes.MapHttpRoute(
               name: "AddCourseModules",
                routeTemplate: "api/Search/AddCourseModules",
                    defaults: new { controller = "Courses", action = "AddCourseModules" }
           );

            config.Routes.MapHttpRoute(
           name: "AddCourseModuleForTeacher",
            routeTemplate: "api/Search/AddCourseModuleForTeacher",
                defaults: new { controller = "Courses", action = "AddCourseModuleForTeacher" }
       );
            config.Routes.MapHttpRoute(
               name: "GetStudents",
                routeTemplate: "api/Search/GetStudents",
                    defaults: new { controller = "Courses", action = "GetStudents" }
           );


            config.Routes.MapHttpRoute(
              name: "CheckUser",
               routeTemplate: "api/Search/CheckUser",
                   defaults: new { controller = "Courses", action = "CheckUser" }
          );

            config.Routes.MapHttpRoute(
             name: "GetCourseModules",
              routeTemplate: "api/Search/GetCourseModules",
                  defaults: new { controller = "Courses", action = "GetCourseModules" }
         );
            
            config.Routes.MapHttpRoute(
             name: "GetTeacherCourseModules",
              routeTemplate: "api/Search/GetTeacherCourseModules",
                  defaults: new { controller = "Courses", action = "GetTeacherCourseModules" }
         );

            config.Routes.MapHttpRoute(
            name: "GetUserCourses",
             routeTemplate: "api/Search/GetUserCourses",
                 defaults: new { controller = "Courses", action = "GetUserCourses" }
        );

            
            config.Routes.MapHttpRoute(
            name: "GetUserCoursesForTeacher",
             routeTemplate: "api/Search/GetUserCoursesForTeacher",
                 defaults: new { controller = "Courses", action = "GetUserCoursesForTeacher" }
        );

            config.Routes.MapHttpRoute(
           name: "AddUserCourses",
            routeTemplate: "api/Search/AddUserCourses",
                defaults: new { controller = "Courses", action = "AddUserCourses" }
       );
            
            config.Routes.MapHttpRoute(
           name: "AddUserCoursesForTeacher",
            routeTemplate: "api/Search/AddUserCoursesForTeacher",
                defaults: new { controller = "Courses", action = "AddUserCoursesForTeacher" }
       );

            config.Routes.MapHttpRoute(
         name: "GetCourses_Single_User",
          routeTemplate: "api/Search/GetCourses_Single_User",
              defaults: new { controller = "Courses", action = "GetCourses_Single_User" }
     );

            config.Routes.MapHttpRoute(
        name: "GetCourseModules_Single_User",
         routeTemplate: "api/Search/GetCourseModules_Single_User",
             defaults: new { controller = "Courses", action = "GetCourseModules_Single_User" }
    );


            config.Routes.MapHttpRoute(
      name: "GetExams",
       routeTemplate: "api/Search/GetExams",
           defaults: new { controller = "Courses", action = "GetExams" }
  );
            
            config.Routes.MapHttpRoute(
      name: "GetExamsForTeacher",
       routeTemplate: "api/Search/GetExamsForTeacher",
           defaults: new { controller = "Courses", action = "GetExamsForTeacher" }
  );

            config.Routes.MapHttpRoute(
     name: "InsertExam",
      routeTemplate: "api/Search/InsertExam",
          defaults: new { controller = "Courses", action = "InsertExam" }
 );


            config.Routes.MapHttpRoute(
             name: "InsertQues",
              routeTemplate: "api/Search/InsertQues",
                  defaults: new { controller = "Courses", action = "InsertQues" }
         );


            config.Routes.MapHttpRoute(
          name: "ViewQuestionAndAnswers",
           routeTemplate: "api/Search/ViewQuestionAndAnswers",
               defaults: new { controller = "Courses", action = "ViewQuestionAndAnswers" }
      );

            config.Routes.MapHttpRoute(
      name: "ExamRecords",
       routeTemplate: "api/Search/ExamRecords",
           defaults: new { controller = "Courses", action = "ExamRecords" }
  );

            config.Routes.MapHttpRoute(
             name: "deleteQues",
             routeTemplate: "api/Search/deleteQues",
               defaults: new { controller = "Courses", action = "deleteQues" }
      );
            
            config.Routes.MapHttpRoute(
            name: "EditQues",
            routeTemplate: "api/Search/EditQues",
              defaults: new { controller = "Courses", action = "EditQues" }
     );

            
            config.Routes.MapHttpRoute(
            name: "ViewExamQuestion",
            routeTemplate: "api/Search/ViewExamQuestion",
              defaults: new { controller = "Courses", action = "ViewExamQuestion" }
     );

            config.Routes.MapHttpRoute(
         name: "getExamsPerCourse",
         routeTemplate: "api/Search/getExamsPerCourse",
           defaults: new { controller = "Courses", action = "getExamsPerCourse" }
  );

            
            config.Routes.MapHttpRoute(
         name: "InsertResult",
         routeTemplate: "api/Search/InsertResult",
           defaults: new { controller = "Courses", action = "InsertResult" }
  );




            config.Routes.MapHttpRoute(
      name: "GetEnrolledStudents",
       routeTemplate: "api/Search/GetEnrolledStudents",
           defaults: new { controller = "Courses", action = "GetEnrolledStudents" }
  );

            
            config.Routes.MapHttpRoute(
      name: "GetEnrolledCoursesStudent",
       routeTemplate: "api/Search/GetEnrolledCoursesStudent",
           defaults: new { controller = "Courses", action = "GetEnrolledCoursesStudent" }
  );


            config.Routes.MapHttpRoute(
        name: "InsertCourseModules_Content",
        routeTemplate: "api/Search/InsertCourseModules_Content",
          defaults: new { controller = "Courses", action = "InsertCourseModules_Content" }
 );



                    config.Routes.MapHttpRoute(
        name: "selectDropboxContent_Id",
        routeTemplate: "api/Search/selectDropboxContent_Id",
        defaults: new { controller = "Courses", action = "selectDropboxContent_Id" }
        );


                            config.Routes.MapHttpRoute(
                name: "InserContentProgress",
                routeTemplate: "api/Search/InserContentProgress",
                defaults: new { controller = "Courses", action = "InserContentProgress" }
                );


                            config.Routes.MapHttpRoute(
                name: "GetContentProgress",
                routeTemplate: "api/Search/GetContentProgress",
                defaults: new { controller = "Courses", action = "GetContentProgress" }
                );

            
                                 config.Routes.MapHttpRoute(
                name: "GetCourseProgress",
                routeTemplate: "api/Search/GetCourseProgress",
                defaults: new { controller = "Courses", action = "GetCourseProgress" }
                );

            
                                 config.Routes.MapHttpRoute(
                name: "GetSingleTeachersCourses",
                routeTemplate: "api/Search/GetSingleTeachersCourses",
                defaults: new { controller = "Courses", action = "GetSingleTeachersCourses" }
                );

            
                                 config.Routes.MapHttpRoute(
                name: "UpdateTeacherComment",
                routeTemplate: "api/Search/UpdateTeacherComment",
                defaults: new { controller = "Courses", action = "UpdateTeacherComment" }
                );

            
                config.Routes.MapHttpRoute(
                name: "GetExamResult",
                routeTemplate: "api/Search/GetExamResult",
                defaults: new { controller = "Courses", action = "GetExamResult" }
                );

            //config.Routes.MapHttpRoute(
            //  name: "getCourses",
            //   routeTemplate: "api/Search/getCourses",
            //       defaults: new { controller = "Courses", action = "getCourses" }
            // );

        }
        
    }
}
