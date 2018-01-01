﻿using Courses.DataAccess;
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