using Courses.Entities;
using Dropbox.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dropbox.Api.Files;
using System.Threading.Tasks;

namespace Courses.DataAccess
{
    public class CoursesRepository
    {
        static string DirectoryName = "";
        static string ModuleDirectoryName = "";
        static string ModuleName = "";
        static string CourseName = "";
        static int dropboxCount = 0;
        protected string CoursesConnectionString { get; set; }
        static string Key = ConfigurationManager.AppSettings["DropboxKey"];
        public CoursesRepository()
        {
            CoursesConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public CoursesRepository(string connection)
        {
            CoursesConnectionString = connection;
        }

        public bool UpdateRole(string role, string email)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "  update [AspNetUsers] set role = "+role+" where Email =  '"+email+"'";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }



        public bool AddCourse(CoursesModel Model)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "insert into [Courses] (CourseName, CourseDuration , CourseStartDate, TeacherUsername) values ('" + Model.CourseName+ "','"+ Model.CourseDuration + "','"+ Model.CourseStartDate + "','"+Model.TeacherUsername+"')";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    DirectoryName = Model.CourseName;
                    var task = Task.Run((Func<Task>)CoursesRepository.Run);
                    task.Wait();

                //    Task.Run(Run);
                    cmd.ExecuteNonQuery();
                    

                }

              

                return true;

              
            }
        }

        public bool AddModules(Modules Model)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "insert into [Modules] (ModuleName) values ('" + Model.ModuleName +  "')";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                }

                return true;
            }
        }

        
        public List<CoursesModel> GetCourses()
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select * from [Courses]";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    List<CoursesModel> data = new List<CoursesModel>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new CoursesModel(myReader);
                                data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }

        public List<Modules> GetModules()
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select * from [Modules]";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    List<Modules> data = new List<Modules>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new Modules(myReader);
                                data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }


        public bool AddCourseModules(CourseModules Model)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();

                string s = Model.ModuleId.ToString();
                String[] words = s.Split(',');
                String[] words1 = Model.DirectoryPath.Split(',');
                String[] words2 = Model.ModuleName.Split(',');

                foreach (var a in words)
                {



                    string qry = "insert into [CourseModules] ([CourseId] ,[ModuleId] )values (" + Model.CourseId + ", " + a + ")";
                    using (var cmd = new SqlCommand(qry, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();


                    }


                }

                var count = 0;
                foreach (var a in words1)
                {
                    ModuleDirectoryName = a;

                    String[] words11 = a.Split('/');
                    foreach (var ab in words11)
                    {
                        count++;

                        if (count == 1)
                        {
                            CourseName = ab;

                        }

                        if (count == 3)
                        {
                            ModuleName = ab;

                            var task = Task.Run((Func<Task>)CoursesRepository.Run1);
                            task.Wait();

                      //      Task.Run(Run1);

                            count = 0;
                        }
                    }
                                         
                    }


              


                  

            


                return true;
            }
        }

        public bool AddCourseModuleForTeacher(CourseModules Model)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                Modules data = new Modules();

                string qry = "insert into [Modules] (ModuleName) values ('" + Model.ModuleName + "')";
                using (var cmd = new SqlCommand(qry, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();


                    string qry2 = "select max(ModuleId) as ModuleId from Modules where ModuleName = '"+Model.ModuleName+"'";

                    using (var cmd2 = new SqlCommand(qry2, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        var myReader = cmd2.ExecuteReader();

                        myReader.Read();


                        data.ModuleId = (int)myReader["ModuleId"];

                        myReader.Close();

                        string qry3 = "insert into [CourseModules]  (CourseId,ModuleId) values (" + Model.CourseId + ", " + data.ModuleId+ ")";

                        using (var cmd3 = new SqlCommand(qry3, conn))
                        {
                            cmd.CommandType = CommandType.Text;

                            cmd3.ExecuteNonQuery();

                            ModuleDirectoryName = Model.DirectoryPath;
                            CourseName = Model.CourseName;
                            ModuleName = Model.ModuleName;

                            var task = Task.Run((Func<Task>)CoursesRepository.Run1);
                            task.Wait();


                        }






                    }


                }


        

                //var count = 0;
                //foreach (var a in words1)
                //{
                //    ModuleDirectoryName = a;

                //    String[] words11 = a.Split('/');
                //    foreach (var ab in words11)
                //    {
                //        count++;

                //        if (count == 1)
                //        {
                //            CourseName = ab;

                //        }

                //        if (count == 3)
                //        {
                //            ModuleName = ab;

                //            var task = Task.Run((Func<Task>)CoursesRepository.Run1);
                //            task.Wait();

                //            //      Task.Run(Run1);

                //            count = 0;
                //        }
                //    }

                //}










                return true;
            }
        }
        public bool AddUserCourses(UserCourses Model)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();

                string s = Model.CourseId.ToString();
                String[] words = s.Split(',');

                foreach (var a in words)
                {



                    string qry = "INSERT INTO UserCourses (StudentId ,CourseId) VALUES  ('" + Model.StudentId + "', " + a + ")";
                    using (var cmd = new SqlCommand(qry, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();

                    }


                }



                return true;
            }
        }

        public bool AddUserCoursesForTeacher(UserCourses Model)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();

                string s = Model.StudentId.ToString();
                String[] words = s.Split(',');

                foreach (var a in words)
                {



                    string qry = "INSERT INTO UserCourses (StudentId ,CourseId) VALUES  ('" + a + "', " + Model.CourseId + ")";
                    using (var cmd = new SqlCommand(qry, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();

                    }


                }



                return true;
            }
        }

        public List<Students> GetTeachers()
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select * from [AspNetUsers] where role = 3";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    List<Students> data = new List<Students>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new Students(myReader);
                                data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }

        public List<Students> GetStudents()
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select * from [AspNetUsers] where role = 2";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    List<Students> data = new List<Students>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new Students(myReader);
                                data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }

        public Students CheckUser(Students s)
        {
            Students data = null;
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select * from [AspNetUsers] where Email = '" + s.Username + "'";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                  
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                data = new Students(myReader);
                                
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }


        //public List<CourseModules> GetCourseModules()
        //{
        //    using (var conn = new SqlConnection(CoursesConnectionString))
        //    {
        //        conn.Open();
        //        string qry = "select * from [Courses]";
        //        using (var cmd = new SqlCommand(qry, conn))
        //        {
        //            cmd.CommandType = CommandType.Text;

        //            List<CourseModules> data = new List<CourseModules>();
        //            var data1 = new List<CourseModules>();
        //            List<CoursesModel> c = new List<CoursesModel>();
        //            List<Modules> m = new List<Modules>();

        //            //var myReader = cmd.ExecuteReader();
        //            using (var myReader = cmd.ExecuteReader())
        //            {
        //                try
        //                {
        //                    while (myReader.Read())
        //                    {
        //                        var get = new CoursesModel(myReader);


        //                       string  data3 = GetCourseModules1(get.CourseID);

        //                        Console.WriteLine(data3);
        //                            // data.Add(get);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    // LOG ERROR
        //                    throw ex;
        //                }
        //            }
        //            return data;
        //        }
        //    }
        //}

        public List<CourseModules> GetCourseModules()
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select CourseModules.CourseId, CourseModules.ModuleId, Courses.CourseName, Modules.ModuleName from [CourseModules] left join Courses on Courses.CourseId = CourseModules.CourseId left join Modules on Modules.ModuleId = CourseModules.ModuleId order by CourseModules.CourseId";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    // CourseModules cc = new CourseModules();
                    List<CourseModules> data = new List<CourseModules>();

                    var id = 0;
                    var name = "";


                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new CourseModules(myReader);

                                //if (get.CourseId == id)
                                //{

                                //    //        get.CourseId = 0; get.CourseName = ""; get.ModuleId = 0; 
                                //    name = get.ModuleName += "," + name;

                                //    data.Add(get);
                                //}
                                //else
                                //{

                                //    name = get.ModuleName;
                                //    data.Add(get);
                                //}

                                //  string data3 = GetCourseModules1(get.CourseID);
                                //cc.CourseId = (int)myReader["CourseId"];
                                //cc.ModuleId = (int)myReader["ModuleId"];
                                //cc.CourseName = (string)myReader["CourseName"];
                                //cc.ModuleName = (string)myReader["ModuleName"];
                                data.Add(get);

                                id = get.CourseId;


                                //      Console.WriteLine(data3);
                                // data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }

        public List<CourseModules> GetTeacherCourseModules(string Username)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select CourseModules.CourseId, CourseModules.ModuleId, Courses.CourseName, Modules.ModuleName from [CourseModules] left join Courses on Courses.CourseId = CourseModules.CourseId left join Modules on Modules.ModuleId = CourseModules.ModuleId left join UserCourses on UserCourses.CourseId = Courses.CourseID where Courses.TeacherUsername = '"+Username+"' group by CourseModules.CourseId,CourseModules.ModuleId, Courses.CourseName, Modules.ModuleName ";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    // CourseModules cc = new CourseModules();
                    List<CourseModules> data = new List<CourseModules>();

                    var id = 0;
                    var name = "";


                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new CourseModules(myReader);

                                //if (get.CourseId == id)
                                //{

                                //    //        get.CourseId = 0; get.CourseName = ""; get.ModuleId = 0; 
                                //    name = get.ModuleName += "," + name;

                                //    data.Add(get);
                                //}
                                //else
                                //{

                                //    name = get.ModuleName;
                                //    data.Add(get);
                                //}

                                //  string data3 = GetCourseModules1(get.CourseID);
                                //cc.CourseId = (int)myReader["CourseId"];
                                //cc.ModuleId = (int)myReader["ModuleId"];
                                //cc.CourseName = (string)myReader["CourseName"];
                                //cc.ModuleName = (string)myReader["ModuleName"];
                                data.Add(get);

                                id = get.CourseId;


                                //      Console.WriteLine(data3);
                                // data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }

        public List<UserCourses> GetUserCourses()
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select UserCourses.CourseId, UserCourses.StudentId, Courses.CourseName, AspNetUsers.Email from UserCourses left join Courses on Courses.CourseId = UserCourses.CourseId left join AspNetUsers on AspNetUsers.id = UserCourses.StudentId group by UserCourses.CourseId, UserCourses.StudentId, Courses.CourseName, AspNetUsers.Email ";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    // CourseModules cc = new CourseModules();
                    List<UserCourses> data = new List<UserCourses>();

                 
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new UserCourses(myReader);

                               
                                data.Add(get);

                              


                            
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }

        public List<UserCourses> GetUserCoursesForTeacher(string Username)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "  select Courses.CourseID, Courses.CourseName ,(select AspNetUsers.Email from AspNetUsers where AspNetUsers.id = UserCourses.StudentId ) as 'Email' from Courses left join UserCourses on UserCourses.CourseId = Courses.CourseID where Courses.TeacherUsername = '"+Username+"' group by Courses.CourseID, Courses.CourseName ,UserCourses.StudentId";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    // CourseModules cc = new CourseModules();
                    List<UserCourses> data = new List<UserCourses>();


                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new UserCourses(myReader);


                                data.Add(get);





                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }


        public  string GetCourseModules1(int c )
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                string data = null;
                conn.Open();
                string qry = "select  Modules.ModuleId, Modules.ModuleName  from CourseModules left join  Modules on CourseModules.ModuleId = Modules.ModuleId where CourseModules.CourseId = " + c;
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                  
                    List<CoursesModel> c1 = new List<CoursesModel>();
                    List<Modules> m = new List<Modules>();

                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new Modules(myReader);
                                data += get.ModuleName + ",";


                                // data.Add(get);
                            }
                        
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }



        public List<CoursesModel> GetCourses_Single_User(string Username)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select Courses.CourseId, Courses.CourseName from UserCourses left join AspNetUsers on AspNetUsers.Id = UserCourses.StudentId left join Courses on Courses.CourseId = UserCourses.CourseId where AspNetUsers.Email = '" + Username + "'";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    List<CoursesModel> data = new List<CoursesModel>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new CoursesModel(myReader);
                                data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }



        public List<Modules> GetCourseModules_Single_User(string Username, int CourseId)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = " select  cm.ModuleId,(select ModuleName from Modules where  Modules.ModuleId = cm.ModuleId ) as 'ModuleName' , (select CourseName from Courses where  Courses.CourseId = uc.CourseId ) as 'courseName', mp.[Module%], uc.CourseId   from UserCourses uc inner join aspnetUsers a on a.Id = uc.StudentId  inner join CourseModules cm on cm.CourseId = uc.CourseId left join StudentModulesProgress mp on mp.UserName = a.UserName and mp.CourseId = uc.CourseId and mp.ModuleId = cm.ModuleId where a.Email = '"+Username+"' and uc.CourseId =  "+CourseId+" group by cm.ModuleId , uc.CourseId , mp.[Module%]";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    List<Modules> data = new List<Modules>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new Modules(myReader);
                                data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }

        public List<StudentCoursePercentage> GetCourseProgress(string Username, int CourseId)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select [Course%] from  [StudentCoursesProgress] where Username = '"+ Username + "' and CourseId = "+ CourseId;
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    List<StudentCoursePercentage> data = new List<StudentCoursePercentage>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new StudentCoursePercentage(myReader);
                                data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }

        public List<Exams> GetExams()
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select CourseExam.ExamId , Exam.ExamName , Courses.CourseName, Exam.Created from CourseExam , Courses , Exam where CourseExam.CourseId = Courses.CourseId and CourseExam.ExamId = Exam.ExamId order by Exam.Created desc";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    List<Exams> data = new List<Exams>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new Exams(myReader);
                                data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }

        public List<Exams> GetExamsForTeacher(string username)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = " select cx.ExamId , e.ExamName , c.CourseName, e.Created  from CourseExam cx inner join Courses c on c.CourseId = cx.CourseId inner join [Exam] e on e.examid = cx.examid where c.TeacherUsername = '"+username+"'";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    List<Exams> data = new List<Exams>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new Exams(myReader);
                                data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }



        public bool InsertExam(Exams Model)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                Exams data = new Exams();
                conn.Open();
                string qry = "insert into [Exam] (ExamName,Created) values  ('" + Model.ExamName+"','"+Model.Created+"')";
                
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;
                   
                    cmd.ExecuteNonQuery();

                    string qry2 = "select max(ExamId)as ExamId from [Exam] where ExamName = '" + Model.ExamName + "'";

                    using (var cmd2 = new SqlCommand(qry2, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        var myReader = cmd2.ExecuteReader();

                        myReader.Read();
                       
                           
                            data.ExamId = (int)myReader["ExamId"];

                        myReader.Close();

                            string qry3 = "insert into [CourseExam]  (ExamId,CourseId) values (" + data.ExamId + ", " + Model.CourseId + ")";

                            using (var cmd3 = new SqlCommand(qry3, conn))
                            {
                                cmd.CommandType = CommandType.Text;

                                cmd3.ExecuteNonQuery();

                            }


                        

                        

                    }


                    }



                return true;


            }
        }


        
        public bool InsertQues(Questions Model)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                Questions data = new Questions();
                var ansCount = 0;
                var option = "";
                var answer = "";
                string s = Model.AnswerText;
                String[] words = s.Split('‡');


                string qry = "INSERT INTO [ExamQuestions] ([ExamId] ,[Question], [AnswerType]) VALUES  (" + Model.ExamId + ",'" + Model.QuestionText + "','"+ Model.AnswerType + "')";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();


                    string qry2 = "select max(QuestionId)as QuestionId from [ExamQuestions] where Question = '" + Model.QuestionText + "'";

                    using (var cmd2 = new SqlCommand(qry2, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        var myReader = cmd2.ExecuteReader();

                        myReader.Read();


                        data.QuesId = (int)myReader["QuestionId"];

                        myReader.Close();


                        foreach (var a in words)
                        {

                            var abc = a;



                            String[] words1 = abc.Split('‰');
                            foreach (var a1 in words1)
                            {
                                ansCount++;
                                if (ansCount == 1)
                                    option = a1;
                                if (ansCount == 2)
                                {
                                    answer = a1;


                                    if (option != "")
                                    {



                                        var x = option;
                                        var y = answer;


                                        string qry3 = "insert into [ExamAnswers]  ([QuestionId],[AnswerText],CorrectAnswer,Examid) values (" + data.QuesId + ", '" + x + "','"+y+"',"+ Model.ExamId + ")";

                                        using (var cmd3 = new SqlCommand(qry3, conn))
                                        {
                                            cmd.CommandType = CommandType.Text;

                                            cmd3.ExecuteNonQuery();

                                        }





                                    }

                                    ansCount = 0;
                                }


                            }




                        }

                        






                    }






                }



                



                    return true;
            }
        }



        public QuestionsAndAnswers ViewQuestionAndAnswers(int ExamId)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("ExamQuestionAndAnswers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Examid", SqlDbType.BigInt).Value = ExamId;
                    QuestionsAndAnswers data = null;
                  
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                data = new QuestionsAndAnswers(myReader);
                            }
                            myReader.NextResult();
                            while (myReader.Read())
                            {


                                data.Questions.Add(new Questions1(myReader));
                              

                            }

                            myReader.NextResult();
                            while (myReader.Read())
                            {
                                data.Answers.Add(new Answers(myReader));
                            }

                            
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }


        public QuestionsAndAnswers ExamRecords(int ExamId,string email,string result)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("ExamRecords", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Examid", SqlDbType.BigInt).Value = ExamId;
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email; 
                    cmd.Parameters.Add("@result", SqlDbType.NVarChar).Value = result;
                    QuestionsAndAnswers data = null;

                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                data = new QuestionsAndAnswers(myReader);
                            }
                            myReader.NextResult();
                            while (myReader.Read())
                            {


                                data.Questions.Add(new Questions1(myReader));


                            }

                            myReader.NextResult();
                            while (myReader.Read())
                            {
                                data.Answers.Add(new Answers(myReader));
                            }


                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }


        public bool deleteQues(int Qid)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();

                



                string qry = "delete [ExamAnswers] where QuestionId = "+ Qid;
             

                using (var cmd = new SqlCommand(qry, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();

                    string qry2 = "delete [ExamQuestions] where QuestionId = " + Qid;
                    using (var cmd2 = new SqlCommand(qry2, conn))
                    {
                        cmd2.CommandType = CommandType.Text;
                        cmd2.ExecuteNonQuery();
                    }


                    }





                return true;
            }
        }


        
        public bool EditQues(Questions Model)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                Questions data = new Questions();
                var ansCount = 0;
                var option = "";
                var answer = "";
                var answerid = "";
                string s = Model.AnswerText;
                String[] words = s.Split('‡');


                string qry = "update [ExamQuestions] set Question = '"+Model.QuestionText+"' , AnswerType = '"+Model.AnswerType+"' where QuestionId = "+ Model.QuesId;
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                        foreach (var a in words)
                        {

                            var abc = a;



                            String[] words1 = abc.Split('‰');
                            foreach (var a1 in words1)
                            {
                           //   ansCount++;
                            //if (ansCount == 1)
                            //    option = a1;
                            //if (ansCount == 2)
                            //{
                            //    answer = a1;

                            var xyz = a1;

                            //if(ansCount == 1)
                            //{
                            //    option = a1;
                              
                            //}
                                

                            String[] words2 = xyz.Split('œ');

                            foreach (var a2 in words2)
                            {
                                ansCount++;

                                var jun = a2;
                                if (ansCount == 1)
                                {
                                    option = jun;

                                }
                                if (ansCount == 2)
                                {
                                    answer = jun;

                                }
                                if (ansCount == 3)
                                {
                                    answerid = jun;



                                    if (option != "" && answerid != "-1" )
                                    {

                                        // up 
                                        string qry1 = "update [ExamAnswers] set QuestionId = "+Model.QuesId+" , AnswerText = '"+ option + "' , CorrectAnswer = '"+ answer + "' , ExamId = "+Model.ExamId+" where [AnswerID] =  " + answerid;
                                        using (var cmd1 = new SqlCommand(qry1, conn))
                                        {
                                            cmd1.CommandType = CommandType.Text;
                                            cmd1.ExecuteNonQuery();
                                        }

                                    }
                                    else if ((option == "" && answerid == "-1") || (option == "" && answerid != "-1"))
                                    {
                                        // up 

                                        string qry1 = "delete [ExamAnswers] where [AnswerID] = " + answerid;
                                        using (var cmd1 = new SqlCommand(qry1, conn))
                                        {
                                            cmd1.CommandType = CommandType.Text;
                                            cmd1.ExecuteNonQuery();
                                        }

                                        //   return true;

                                    }
                                    else
                                    {
                                        // In 


                                        string qry1 = "insert into [ExamAnswers]  ([QuestionId],[AnswerText],CorrectAnswer,Examid) values (" + Model.QuesId + ", '" + option + "','" + answer + "'," + Model.ExamId + ")";
                                        using (var cmd1 = new SqlCommand(qry1, conn))
                                        {
                                            cmd1.CommandType = CommandType.Text;
                                            cmd1.ExecuteNonQuery();
                                        }

                                    }






                                    ansCount = 0;
                                }



                            }


                                //if (option != "")
                                //{



                                //    var x = option;
                                //    var y = answer;


                                //    string qry3 = "insert into [ExamAnswers]  ([QuestionId],[AnswerText],CorrectAnswer,Examid) values (" + data.QuesId + ", '" + x + "','" + y + "'," + Model.ExamId + ")";

                                //    using (var cmd3 = new SqlCommand(qry3, conn))
                                //    {
                                //        cmd.CommandType = CommandType.Text;

                                //        cmd3.ExecuteNonQuery();

                                //    }


                                //}

                                //ansCount = 0;
                                //}


                            }




                        }








          






                }







                return true;
            }
        }

        public QuestionsAndAnswers ViewExamQuestion(QuestionPaginations Model)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("ExamQuestions_single", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CourseId", SqlDbType.BigInt).Value = Model.CourseId;
                    cmd.Parameters.Add("@previous", SqlDbType.BigInt).Value = Model.Previous;
                    cmd.Parameters.Add("@next", SqlDbType.BigInt).Value = Model.Next;

                    QuestionsAndAnswers data = null;

                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                data = new QuestionsAndAnswers(myReader);
                            }
                            myReader.NextResult();
                            while (myReader.Read())
                            {


                                data.Questions.Add(new Questions1(myReader));


                            }

                            myReader.NextResult();
                            while (myReader.Read())
                            {
                                data.Answers.Add(new Answers(myReader));
                            }


                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }



        public List<Exams> getExamsPerCourse(int CourseId)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select [CourseExam].ExamId , Exam.ExamName, [CourseExam].CourseId , Courses.CourseName from [CourseExam] , Exam , Courses where Exam.Examid = [CourseExam].Examid and [CourseExam].CourseId =  Courses.CourseID and  [CourseExam].CourseId = "+CourseId;
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    List<Exams> data = new List<Exams>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new Exams(myReader);
                                data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }


        
        public bool InsertResult(Results Model)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                Results r = new Results();
                var Count = 0;
                var Answerid = "";
                var Questionid = "";
                var CorrectAnswer = "";

                string s = Model.Records;
                String[] words = s.Split('‡');
                conn.Open();
                string qry = "INSERT INTO [StudentExamResults] ([StudentUserName],[ExamId],[Result],[TotalWrongAnswers],[TotalCorrectAnswers]) VALUES ('"+Model.UserName+"',"+Model.ExamId+",'"+Model.Result + "',"+Model.TotalWrongAnswers+","+Model.TotalCorrectAnswers+")";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;
                         cmd.ExecuteNonQuery();


                    string qry2 = "select max(ResultId) as ResultId from [StudentExamResults] where StudentUserName = '"+ Model.UserName+ "' and ExamId = "+ Model.ExamId + " and Result = '"+ Model.Result+ "' and TotalWrongAnswers = "+Model.TotalWrongAnswers+" and TotalCorrectAnswers = "+ Model.TotalCorrectAnswers;

                    using (var cmd2 = new SqlCommand(qry2, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        var myReader = cmd2.ExecuteReader();

                        myReader.Read();


                        r.ResultId = (int)myReader["ResultId"];

                        myReader.Close();


                        foreach (var a in words)
                        {

                            var Options = a;


                            if (Options != "")
                            {
                                String[] words1 = Options.Split('‰');
                                foreach (var a1 in words1)
                                {

                                    Count++;
                                    if (Count == 1)
                                        Questionid = a1;
                                    if (Count == 2)
                                        Answerid = a1;
                                    if (Count == 3)
                                    {
                                        CorrectAnswer = a1;


                                        string qry3 = "insert into [StudentExamRecords]  ([ResultId],[QuestionId],AnswerId,CorrectAnswer) values (" + r.ResultId + "," + Questionid + "," + Answerid + ",'" + CorrectAnswer + "')";

                                        using (var cmd3 = new SqlCommand(qry3, conn))
                                        {
                                            cmd.CommandType = CommandType.Text;

                                                  cmd3.ExecuteNonQuery();

                                        }





                                        Count = 0;
                                    }

                                


                                }


                            }

                        }

                    }
                }

                return true;
            }
        }


        public List<EnrolledStudents> GetEnrolledStudents()
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select a.username ,   (select CourseName from Courses where Courses.CourseId = c.CourseId) as 'CourseName', iif(count(e.Result)<>0,'Yes','No') as 'Exam', e.Result as 'Result' , cp.[Course%]+'%' as 'Course%' , (select ModuleName from Modules where Modules.Moduleid = m.ModuleId) as 'Current Module Name', m.[Module%]+'%' as 'Module%' from UserCourses u inner join aspnetUsers a on a.Id = u.StudentId inner join Courses c on c.CourseId = u.CourseId left join [CourseExam] cx on cx.CourseId = u.CourseId left join StudentExamResults e on e.StudentUserName = a.UserName and e.ExamId = cx.ExamId left join StudentCoursesProgress cp on cp.UserName = a.UserName and cp.CourseId =c.CourseId left join StudentModulesProgress m on m.UserName = a.UserName and m.CourseId = c.CourseId and m.ModuleId in (select StudentModulesProgress.ModuleId from UserCourses , CourseModules, StudentModulesProgress where UserCourses.CourseId = CourseModules.CourseId and StudentModulesProgress.ModuleId =  CourseModules.ModuleId and StudentModulesProgress.Username = a.UserName   ) and m.Created in (select max(StudentModulesProgress.Created) from UserCourses , CourseModules, StudentModulesProgress where UserCourses.CourseId = CourseModules.CourseId and StudentModulesProgress.ModuleId =  CourseModules.ModuleId and StudentModulesProgress.Username = a.UserName ) group by  a.UserName , c.CourseId , e.Result ,cp.[Course%] , m.[Module%] , m.ModuleId";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    List<EnrolledStudents> data = new List<EnrolledStudents>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new EnrolledStudents(myReader);
                                data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }

        public List<EnrolledStudents> GetEnrolledCoursesStudent(int courseid)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select e.ResultId, a.username ,   e.ExamId, ( select ExamName from exam where exam.examid = e.ExamId) as 'ExamName' ,(select CourseName from Courses where Courses.CourseId = c.CourseId) as 'CourseName', iif(count(e.Result)<>0,'Yes','No') as 'Exam',e.Result as 'Result' , cp.[Course%]+'%' as 'Course%' , (select ModuleName from Modules where Modules.Moduleid = m.ModuleId) as 'Current Module Name',m.[Module%]+'%' as 'Module%' ,iif(count(e.Comments)<>0,1,0) as 'CheckComments' , e.Comments from UserCourses u inner join aspnetUsers a on a.Id = u.StudentId inner join Courses c on c.CourseId = u.CourseId left join [CourseExam] cx on cx.CourseId = u.CourseId left join StudentExamResults e on e.StudentUserName = a.UserName and e.ExamId = cx.ExamId left join StudentCoursesProgress cp on cp.UserName = a.UserName and cp.CourseId =c.CourseId left join StudentModulesProgress m on m.UserName = a.UserName and m.CourseId = c.CourseId and m.ModuleId in (select StudentModulesProgress.ModuleId from UserCourses , CourseModules, StudentModulesProgress where UserCourses.CourseId = CourseModules.CourseId and StudentModulesProgress.ModuleId =  CourseModules.ModuleId and StudentModulesProgress.Username = a.UserName   ) and m.Created in (select max(StudentModulesProgress.Created) from UserCourses , CourseModules, StudentModulesProgress where UserCourses.CourseId = CourseModules.CourseId and StudentModulesProgress.ModuleId =  CourseModules.ModuleId and StudentModulesProgress.Username = a.UserName ) where u.CourseId = " + courseid+ " group by  e.ResultId, a.UserName , c.CourseId , e.Result ,cp.[Course%] , m.[Module%] , m.ModuleId, e.ExamId, e.Comments";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    List<EnrolledStudents> data = new List<EnrolledStudents>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new EnrolledStudents(myReader);
                                data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }

        public List<CourseCountent> selectDropboxContent_Id(int url)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry1 = "select * from [CourseContent] where ModuleId = "+url+"" ;
                using (var cmd3 = new SqlCommand(qry1, conn))
                {
                    cmd3.CommandType = CommandType.Text;

                    List<CourseCountent> data = new List<CourseCountent>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd3.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new CourseCountent(myReader);
                                data.Add(get);





                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }




        public bool InsertCourseModules_Content(CourseCountent Model)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();

                string qry1 = "select DropboxId from [CourseContent] ";


                //using (var cmd3 = new SqlCommand(qry1, conn))
                //{
                //    cmd3.CommandType = CommandType.Text;

                //    List<CourseCountent> data = new List<CourseCountent>();
                //    //var myReader = cmd.ExecuteReader();
                //    using (var myReader = cmd3.ExecuteReader())
                //    {
                //        try
                //        {
                //            while (myReader.Read())
                //            {
                //                var get = new CourseCountent(myReader);
                //                data.Add(get);





                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            // LOG ERROR
                //            throw ex;
                //        }
                //    }

                //    foreach(var a in data.Where(x => x.DropboxId != Model.DropboxId))
                //    {

                //        var abc = Model.DropboxId;



                //        //if(a.DropboxId != Model.DropboxId)
                //        //{


                //        //    string qry = "delete [CourseContent] where DropboxId = '"+Model.DropboxId+"'";
                //        //    using (var cmd = new SqlCommand(qry, conn))
                //        //    {
                //        //        cmd.CommandType = CommandType.Text;
                //        //        cmd.ExecuteNonQuery();

                //        //    }


                //        //}



                //    }


                //}



                string qry = "INSERT INTO [CourseContent] ([ContentType],[ContentName],[ContentURL],ModuleId,DropboxId) VALUES ('" + Model.ContentType + "','" + Model.ContentName + "','" + Model.ContentURL + "'," + Model.ModuleId + ",'" + Model.DropboxId + "')";
                using (var cmd = new SqlCommand("[InsertCourse_Content]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ContentType", SqlDbType.NVarChar).Value = Model.ContentType;
                    cmd.Parameters.Add("@ContentName", SqlDbType.NVarChar).Value = Model.ContentName;
                    cmd.Parameters.Add("@ContentURL", SqlDbType.NVarChar).Value = Model.ContentURL;
                    cmd.Parameters.Add("@ModuleId", SqlDbType.Int).Value = Model.ModuleId;
                    cmd.Parameters.Add("@DropboxId", SqlDbType.NVarChar).Value = Model.DropboxId;
                    //cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = Model.ContentTitle;

                    cmd.ExecuteNonQuery();


            
                     
                    string qry12 = "update CourseContent set [ContentTitle] = '" + Model.ContentTitle + "' where  [ContentType] = '" + Model.ContentType + "' and ContentName = '"+ Model.ContentName + "' and ContentURL = '"+Model.ContentURL+ "'and ModuleId = "+ Model.ModuleId;

                    using (var cmd2 = new SqlCommand(qry12, conn))
                        {
                            cmd2.CommandType = CommandType.Text;
                            cmd2.ExecuteNonQuery();
                          
                        }
                    }

                




                return true;
            }
        }


        
        public bool InserContentProgress(StudentContentProgress Model)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "INSERT INTO [StudentExamResults] ([ContentId],[Username]) VALUES ('" + Model.ContentId + "','"+Model.Username+"')";
                  ModuleName = Model.ModuleName;
                  CourseName = Model.CourseName;

                var task = Task.Run((Func<Task>)CoursesRepository.CheckModuleContentCount);
                task.Wait();

                using (var cmd = new SqlCommand("[InsertContentProgress]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@ContentId", SqlDbType.NVarChar).Value = Model.ContentId;
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = Model.Username;
                    cmd.Parameters.Add("@ModuleId", SqlDbType.NVarChar).Value = Model.ModuleId;
                    cmd.Parameters.Add("@dropboxContentCount", SqlDbType.Int).Value = dropboxCount;
                    cmd.Parameters.Add("@CourseId", SqlDbType.Int).Value = Model.CourseId;
                    cmd.Parameters.Add("@ModuleName", SqlDbType.NVarChar).Value = Model.ModuleName;
                    cmd.Parameters.Add("@CourseName", SqlDbType.NVarChar).Value = Model.CourseName;
                    cmd.Parameters.Add("@ContentName", SqlDbType.NVarChar).Value = Model.CountentName;



                    cmd.ExecuteNonQuery();

                }

                return true;
            }
        }


        public List<StudentContentProgress> GetContentProgress(StudentContentProgress model)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry1 = "  select [StudentContentProgress].ContentId, [CourseContent].ModuleId from [StudentContentProgress] , [CourseContent] where[CourseContent].ContentId = [StudentContentProgress].ContentId and [StudentContentProgress].Username = '" + model.Username+"' ";
                using (var cmd3 = new SqlCommand(qry1, conn))
                {
                    cmd3.CommandType = CommandType.Text;

                    List<StudentContentProgress> data = new List<StudentContentProgress>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd3.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new StudentContentProgress(myReader);
                                data.Add(get);





                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                  
                    return data;
                }
            }
        }


        
        public List<CoursesModel> GetSingleTeachersCourses(string Username)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select * from courses where TeacherUsername = '"+ Username + "'";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    List<CoursesModel> data = new List<CoursesModel>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new CoursesModel(myReader);
                                data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }
        
        public List<EnrolledStudents> GetExamResult(string Username)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select r.resultId, ce.courseId, r.ExamId , (select ExamName from Exam where Exam.ExamId = r.ExamId) as 'ExamName', r.StudentUserName as 'Username' , r.Result,r.Comments  from CourseExam ce inner join [StudentExamResults] r on r.ExamId = ce.ExamId where r.StudentUserName = '"+ Username + "'";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    List<EnrolledStudents> data = new List<EnrolledStudents>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new EnrolledStudents(myReader);
                                data.Add(get);
                            }
                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return data;
                }
            }
        }

        static async Task Run()
        {
            using (var dbx = new DropboxClient(CoursesRepository.Key))
            {

                var full = await dbx.Users.GetCurrentAccountAsync();

                Console.WriteLine("{0} - {1}", full.Name.DisplayName, full.Email);


                //    await Upload(dbx, @"/MyApp/test", "test.txt", "Testing!");

                //  await p.UploadDoc();

                //using (var abc1 = await dbx.Files.DownloadAsync(@"/Courses/xyz/Modules/Lesson 2/bbc.txt"))
                //{
                //    //foreach (var a in abc1.Entries)
                //    //{
                //    Console.WriteLine(await abc1.GetContentAsStringAsync() + "  ");
                //}
                //}

                var list1 = await dbx.Files.ListFolderAsync(@"");

                var a11 = list1.Entries.Where(i => i.Name == "Courses").Count();
                if (a11 != 1)
                {
                    await dbx.Files.CreateFolderAsync("/Courses");

                }



                    var list = await dbx.Files.ListFolderAsync(@"/Courses/");

                //await Upload(dbx, @"/MyApp/test", "test.txt", "Testing!");
                //Console.ReadLine();

                var a1 = list.Entries.Where(i => i.Name == CoursesRepository.DirectoryName).Count();
                if (a1 == 1)
                {


                    //  await dbx.Files.CreateFolderAsync(@"/Courses" + "/" + data.CourseName + "/" + "Modules/" + data1.ModuleName);

                }
                else
                {
                    await dbx.Files.CreateFolderAsync("/Courses/" + CoursesRepository.DirectoryName+"/Modules");
                }

            }
        }



        static async Task Run1()
        {
            using (var dbx = new DropboxClient(CoursesRepository.Key))
            {

        //        var full = await dbx.Users.GetCurrentAccountAsync();

      //          Console.WriteLine("{0} - {1}", full.Name.DisplayName, full.Email);


                //    await Upload(dbx, @"/MyApp/test", "test.txt", "Testing!");

                //  await p.UploadDoc();

                //using (var abc1 = await dbx.Files.DownloadAsync(@"/Courses/xyz/Modules/Lesson 2/bbc.txt"))
                //{
                //    //foreach (var a in abc1.Entries)
                //    //{
                //    Console.WriteLine(await abc1.GetContentAsStringAsync() + "  ");
                //}
                //}
              

                //await Upload(dbx, @"/MyApp/test", "test.txt", "Testing!");
                //Console.ReadLine();
                if (CoursesRepository.ModuleDirectoryName != "") {

                    var list = await dbx.Files.ListFolderAsync(@"/Courses/" + CoursesRepository.CourseName + "/Modules/");
                    var a1 = list.Entries.Where(i => i.Name == CoursesRepository.ModuleName).Count();

                if (a1 != 1)
                {
                    //  CoursesRepository.ModuleName = "";
                    //       CoursesRepository.CourseName = "";
                    await dbx.Files.CreateFolderAsync("/Courses/" + CoursesRepository.ModuleDirectoryName);
                    CoursesRepository.ModuleDirectoryName = "";
                    //     CoursesRepository.ModuleDirectoryName = "";
                    //  await dbx.Files.CreateFolderAsync(@"/Courses" + "/" + data.CourseName + "/" + "Modules/" + data1.ModuleName);

                }

                }
            }
        }

        static async Task CheckModuleContentCount()
        {
            using (var dbx = new DropboxClient(CoursesRepository.Key))
            {



                //if (CoursesRepository.ModuleDirectoryName != "")
                //{

                var list = await dbx.Files.ListFolderAsync(@"/Courses/" + CoursesRepository.CourseName + "/Modules/"+ CoursesRepository.ModuleName);
                dropboxCount = list.Entries.Count();



                //  }
            }
        }


        
        public bool UpdateTeacherComment(string com, int rid)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "  update [StudentExamResults] set Comments = '" + com + "'where ResultId =  " + rid + "";
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }

        //public bool DeleteCourses(CoursesModel Model)
        //{
        //    using (var conn = new SqlConnection(CoursesConnectionString))
        //    {
        //        conn.Open();
        //        string qry = "  update [AspNetUsers] set role = where Email =  ''";
        //        using (var cmd = new SqlCommand(qry, conn))
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            cmd.ExecuteNonQuery();
        //            return true;
        //        }
        //    }
        //}

        public bool DeleteCourses(CoursesModel Model)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
             //   DeleteModules(Model.CourseID);

                using (var cmd = new SqlCommand("[DeleteCourses]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CourseName", SqlDbType.NVarChar).Value = Model.CourseName;
                    cmd.Parameters.Add("@deleteall", SqlDbType.NVarChar).Value = Model.deleteall;
             



                    cmd.ExecuteNonQuery();

                }

                return true;
            }
        }


        public bool DeleteModules(int id)
        {
            using (var conn = new SqlConnection(CoursesConnectionString))
            {
                conn.Open();
                string qry = "select *  from coursemodules where CourseId = " + id ;
                using (var cmd = new SqlCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    List<Modules> data = new List<Modules>();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                var get = new Modules(myReader);
                                data.Add(get);
                            }
                            myReader.Close();
                            foreach (var a in data)
                            {

                                string qry1 = "delete [Coursemodules] where ModuleId = " + a.ModuleId;


                                using (var cmd2 = new SqlCommand(qry1, conn))
                                {
                                    cmd2.CommandType = CommandType.Text;
                                    cmd2.ExecuteNonQuery();
                                }




                            }


                            foreach (var a in data)
                            {

                                string qry1 = "delete [modules] where ModuleId = " + a.ModuleId;


                                using (var cmd2 = new SqlCommand(qry1, conn))
                                {
                                    cmd2.CommandType = CommandType.Text;
                                    cmd2.ExecuteNonQuery();
                                }




                            }


                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return true;
                }
            }
        }
    }
}
