courseApp.controller("courseController", function courseController($scope, $window, $http, $filter, $timeout, $log, $uibModal) {
    $scope.Author = "Umais Siddiqui";
    $scope.UserName = "";
    $scope.DropboxKey = "";
    $scope.role = [];
    $scope.test = function () { alert("Testing"); };
    var promise = $timeout(function () {
        $scope.UserName = $('#UserName').val();
        $scope.role = $('#role').val();

        if ($scope.role == 1) {
            $scope.getEnrolledStudents();
            $scope.getCourses();
            $scope.getModules();
            $scope.getStudents();
            $scope.getCourseModules();
            $scope.getUserCourses();
            $scope.getExams();
            $scope.getTeachers();
        } else if($scope.role == 2)
        {
            $scope.GetSingleUserCourses();
            $scope.GetSingleUserCourseModules();
            $scope.GetCourseProgress();
            $scope.getExamResult();
        } else if ($scope.role == 3){

            $scope.getSingleTeachersCourses();
            $scope.getStudents();
            $scope.getExamsForTeacher();
            $scope.getUserCoursesForTeacher();
            $scope.getCourseModulesForTeacher();


        }



    })




    $scope.getUsersRole = function () {
        var resource = location.protocol + "//" + location.host + "/api/Search/CheckUser";
        var user = $('#UserName').val();

        var data = {
            UserName: user
        };


        $http.post(resource, data).success(function (data, status) {
            if (data.Role == 1) {
                $scope.isStudent = false;
                $scope.isTeacher = false;
                $scope.isAdmin = true;
              
               
            } else if (data.Role == 2) {
                $scope.isAdmin = false;
                $scope.isTeacher = false;
                $scope.isStudent = true;
              
               

            } else if (data.Role == 3) {
                $scope.isStudent = false;
                $scope.isAdmin = false;
                $scope.isTeacher = true;
              
               
            }

            $scope.role = data.Role;

        })
            .error(function (data, status) {
                // this isn't happening:
            })



    }

    $scope.getUsersRole();

    
    var resource = location.protocol + "//" + location.host + "/api/Search/GetDropboxKey";

    $http.get(resource).success(function (data, status) {
        $scope.DropboxKey = data;
      
    })
     
    $scope.open = function (size, videoSource) {
        $log.info("open", videoSource);
        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'myModal.html',
            controller: 'ModalInstanceCtrl',
            backdrop: true,
            size: size,
            resolve: {
                videoSource: function () {
                    return videoSource;
                }
            }
        });

        modalInstance.result.then(function (result) {
            //
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };

    $scope.videoClick = function ($event, videoSource,c,m,i,cid) {
       // alert(cid);
      //  alert(videoSource);
     //   alert($scope.DropboxKey);
        var config = {
            headers: {
                'Authorization': 'Bearer ' + $scope.DropboxKey,
                'Content-Type': 'application/json'
            }
        }
        var data2 = {
            "path": videoSource


                    }

                    var resource2 = "https://api.dropboxapi.com/2/files/get_temporary_link";
                    $http.post(resource2, data2, config).success(function (data, status) {


                     //   alert(data.metadata.sharing_info.read_only);

                        $log.info("videoClick", data.link)
                        $scope.open('lg', data.link);

                    
                        var data3 = {
                        
                            "Username": $scope.UserName,
                            "ModuleId": i,
                            "CourseName": c,
                            "ModuleName": m,
                            "CourseId": cid,
                            "CountentName": data.metadata.name
                        }

                        var resource = location.protocol + "//" + location.host + "/api/Search/InserContentProgress";
                        $http.post(resource, data3).success(function (data, status) {

                            if (data = "true") {
                                $scope.ShowContentModules(c, m,i);
                            }
                        });


                       
                      //  alert($scope.GetModuleContentDropboxApi.metadata.name + "  " + $scope.GetModuleContentDropboxApi.link)

                     //   $window.location.href = 'https://api.dropboxapi.com/2/files/get_temporary_link';

                    });

               
                  
       
    }


    $scope.fileClick = function ($event, fileSource,c,m,i,cid) {
       // alert($scope.DropboxKey);
        var name = "";
      
        var config = {
            headers: {
                'Authorization': 'Bearer ' + $scope.DropboxKey,
                'Content-Type': 'application/json'
            }
        }
        var data2 = {
            "path": fileSource,
            "short_url": false


        }
        var data3 = {
            "path": fileSource


        }

        var resource2 = "https://api.dropboxapi.com/2/files/get_temporary_link";
        $http.post(resource2, data3, config).success(function (data, status) {

            name = data.metadata.name;
         
         

        });


        var resource2 = "https://api.dropboxapi.com/2/sharing/create_shared_link";
        $http.post(resource2, data2, config).success(function (data, status) {

        

            var data3 = {
               
                "Username": $scope.UserName,
                "ModuleId": i,
                "CourseName": c,
                "ModuleName": m,
                "CourseId": cid,
                "CountentName": name
            }

            var resource = location.protocol + "//" + location.host + "/api/Search/InserContentProgress";
            $http.post(resource, data3).success(function (data, status) {

                if (data = "true") {
                    $scope.ShowContentModules(c, m,i);
                }
            });
           
         

         //   alert(data.url);

            //  alert($scope.GetModuleContentDropboxApi.metadata.name + "  " + $scope.GetModuleContentDropboxApi.link)

            

            $window.open(
                data.url,
                '_blank' // <- This is what makes it open in a new window.
            );



        });




    }

    $scope.AddCourses = function () {
        $scope.isError = false;
        $scope.isSuccess = false;
        var selectedTeacher = $("#selectedTeacher").val();

      
        var data1 = {
            CourseName: $scope.CourseName,
            CourseDuration: $scope.CourseDuration,
            CourseStartDate: $scope.CourseStartDate,
            TeacherUsername: selectedTeacher

        };

        if ($scope.CourseName != null && $scope.CourseDuration != null && $scope.CourseStartDate != null && selectedTeacher != "? undefined:undefined ?") {                
            var resource = location.protocol + "//" + location.host + "/api/Search/AddCourse";
            $http.post(resource, data1).success(function (data, status) {
             
                if (data = "true") {
                    $scope.isError = false;
                    $scope.isSuccess = true;
                    $scope.successMessage = "Course Successfully Added...";
                    $scope.CourseName = null;
                    $scope.CourseDuration = null;
                    $scope.CourseStartDate = null;
                    $scope.getCourses();
                 //   $scope.AddCourseDirectory("/Courses/"+data1.CourseName);
                    //$scope.onPropertySearch();
                }
            });
        } else {

            $scope.isError = true;
            $scope.errormessage = "All Fields Are Required..";
        }
    }

   
  
    $scope.AddModules = function () {


        var data1 = {
            ModuleName: $scope.ModuleName,
           
        };
       
        if ($scope.ModuleName != null) {
            var resource = location.protocol + "//" + location.host + "/api/Search/AddModules";
            $http.post(resource, data1).success(function (data, status) {
                if (data = "true") {
                    $scope.isError = false;
                    $scope.isSuccess = true;
                    $scope.successMessage = "Modules Successfully Added...";
                    $scope.ModuleName = null;
                    $scope.getModules();
                    //$scope.onPropertySearch();
                }
            });
        } else {
            $scope.isError = true;
            $scope.errormessage = "All Fields Are Required..";
        }

    }


    
    $scope.getAllCourses = [];

    $scope.getCourses = function () {
        var resource = location.protocol + "//" + location.host + "/api/Search/GetCourses";

        $http.get(resource).success(function (data, status) {
            $scope.getAllCourses = data;         

        })
            .error(function (data, status) {
                // this isn't happening:
            })



    }
   
   

 
    

     
    $scope.getAllModules = [];

    $scope.getModules = function () {
        var resource = location.protocol + "//" + location.host + "/api/Search/GetModules";

        $http.get(resource).success(function (data, status) {
            $scope.getAllModules = data;

        })
            .error(function (data, status) {
                // this isn't happening:
            })



    }
    
    
       
    $scope.getAllTeachers = [];

    $scope.getTeachers = function () {
        var resource = location.protocol + "//" + location.host + "/api/Search/GetTeachers";

        $http.get(resource).success(function (data, status) {
            $scope.getAllTeachers = data;

        })
            .error(function (data, status) {
                // this isn't happening:
            })



    }

       

    $scope.getAllSingleTeachersCourses = [];

    $scope.getSingleTeachersCourses = function () {

        var data = {
            TeacherUsername: $scope.UserName

        }

        var resource = location.protocol + "//" + location.host + "/api/Search/GetSingleTeachersCourses";
        $http.post(resource,data).success(function (data, status) {
            $scope.getAllSingleTeachersCourses = data;

        })
            .error(function (data, status) {
                // this isn't happening:
            })



    }

   
       
    $scope.InsertCourseModule = function () {
       
        var C_id = $("#selectedCourses").val();
        var M_id = $("#selectedModules").val().toString();

        var CourseName = '';
        var path = '';
        var ModuleName = '';

        var data1 = {
            CourseId: C_id,
            ModuleId: M_id,
            DirectoryPath: path
        }

        


           angular.forEach($scope.getAllCourses, function (value, key) {

               if (value.CourseID == C_id) {

                   CourseName =  value.CourseName;
            }
        });

         
           var ss = data1.ModuleId.split(",");
           for (var i in ss) {

               angular.forEach($scope.getAllModules, function (value, key) {

                   if (value.ModuleId == ss[i]) {


                       path += CourseName + "/Modules/" + value.ModuleName + ",";
                       ModuleName += value.ModuleName + ",";
                    //   $scope.AddCourseDirectory(path);

                   }
               });


           } 
     
         //  alert(path);

      
           var data = {
               CourseId: C_id,
               ModuleId: M_id,
               DirectoryPath: path,
               ModuleName: ModuleName,
           }
     

       // alert(C_id + " " + M_id);
        var resource = location.protocol + "//" + location.host + "/api/Search/AddCourseModules";
        $http.post(resource, data).success(function (data, status) {
            if (data = "true") {
                $scope.isSuccess = true;
                $scope.successMessage = "Successfully Assigned Modules...";
                $scope.getCourseModules();

               


                //$scope.onPropertySearch();
            }
        });

     

    }


    $scope.InsertCourseModuleForTeacher = function () {

        $scope.isError = false;
        $scope.isSuccess = false;

        var C_id = $("#selectedTeacherCourses").val();
       

        
        var ModuleName = $scope.TModuleName;
        var CourseName = '';
        angular.forEach($scope.getAllSingleTeachersCourses, function (value, key) {

            if (value.CourseID == C_id) {
                CourseName = value.CourseName;
            }

        });


   


        var d = CourseName + "/Modules/" + ModuleName;
    
        var data1 = {
            ModuleName: ModuleName,
            CourseId: C_id,
            CourseName: CourseName,
            DirectoryPath: d
        };
      //  alert(C_id + ":" + ModuleName);
        if ((ModuleName != undefined || ModuleName != null) && C_id != "? undefined:undefined ?" ) {

        var resource = location.protocol + "//" + location.host + "/api/Search/AddCourseModuleForTeacher";
            $http.post(resource, data1).success(function (data, status) {
                if (data = "true") {
                    $scope.isError = false;
                    $scope.isSuccess = true;
                    $scope.successMessage = "Modules Successfully Assign...";
                    $scope.TModuleName = null;
                    $scope.getCourseModulesForTeacher();
                  
                }
            });
        } else {
            $scope.isError = true;
            $scope.errormessage = "All Fields Are Required..";
        }
     



    }

    $scope.InsertUserCourses = function () {

        var S_id = $("#selectedStudents").val();
        var C_id = $("#selectedCourses").val().toString();

        var data1 = {
            StudentId: S_id,
            CourseId: C_id
        }

     //  alert(S_id + " dgjk'" + C_id);
        var resource = location.protocol + "//" + location.host + "/api/Search/AddUserCourses";
        $http.post(resource, data1).success(function (data, status) {
            if (data = "true") {
                $scope.isSuccess = true;
                $scope.successMessage = "Successfully Assigned Course...";
                $scope.getUserCourses();

                //$scope.onPropertySearch();
            }
        });

        

    }

    
    $scope.InsertUserCoursesForTeacher = function () {

        var S_id = $("#selectedStudents2").val().toString();
        var C_id = $("#selectedTeacherCourses").val();

        var data1 = {
            StudentId: S_id,
            CourseId: C_id
        }

       //   alert(S_id + " dgjk" + C_id);
        var resource = location.protocol + "//" + location.host + "/api/Search/AddUserCoursesForTeacher";
        $http.post(resource, data1).success(function (data, status) {
            if (data = "true") {
                $scope.isSuccess = true;
                $scope.successMessage = "Successfully Assigned Course...";
                $scope.getUserCoursesForTeacher();

                //$scope.onPropertySearch();
            }
        });



    }

    $scope.getAllStudents = [];

    $scope.getStudents = function () {
        var resource = location.protocol + "//" + location.host + "/api/Search/GetStudents";

        $http.get(resource).success(function (data, status) {
            $scope.getAllStudents = data;

        })
            .error(function (data, status) {
                // this isn't happening:
            })



    }

   
 




    //$scope.getUsersRole = function () {
    //    var resource = location.protocol + "//" + location.host + "/api/Search/CheckUser";
    //    var user = $('#UserName').val();
      
    //    var data = {
    //        UserName: user
    //    };
        

    //    $http.post(resource, data).success(function (data, status) {
    //        if (data.Role == 1) {
    //            $scope.isAdmin = true;
    //        }


            
    //    })
    //        .error(function (data, status) {
    //            // this isn't happening:
    //        })



    //}

  //  $scope.getUsersRole();







    $scope.getAllCourseModules = [];

    $scope.getCourseModules = function () {
        var resource = location.protocol + "//" + location.host + "/api/Search/GetCourseModules";

        $http.get(resource).success(function (data, status) {
            $scope.getAllCourseModules = data;

        })
            .error(function (data, status) {
                // this isn't happening:
            })



    }

  
    $scope.getAllCourseModulesForTeacher = [];

    $scope.getCourseModulesForTeacher = function () {
        var data = {
            Username: $scope.UserName

        }
        var resource = location.protocol + "//" + location.host + "/api/Search/GetTeacherCourseModules";

        $http.post(resource, data).success(function (data, status) {
            $scope.getAllCourseModulesForTeacher = data;

        })
            .error(function (data, status) {
                // this isn't happening:
            })



    }



    $scope.getAllUserCourses = [];

    $scope.getUserCourses = function () {
        var resource = location.protocol + "//" + location.host + "/api/Search/GetUserCourses";

        $http.get(resource).success(function (data, status) {
            $scope.getAllUserCourses = data;

        })
            .error(function (data, status) {
                // this isn't happening:
            })



    }




    $scope.getAllUserCoursesForTeacher = [];

    $scope.getUserCoursesForTeacher = function () {

        var data = {
            Username: $scope.UserName
        }
        var resource = location.protocol + "//" + location.host + "/api/Search/GetUserCoursesForTeacher";

        $http.post(resource,data).success(function (data, status) {
            $scope.getAllUserCoursesForTeacher = data;

        })
            .error(function (data, status) {
                // this isn't happening:
            })



    }


    



    $scope.GetAllSingleUserCourses = [];

        $scope.GetSingleUserCourses = function () {

            var Email = $('#UserName').val();
        var data = {
            Email: Email
            
        }

      //  alert(S_id + " dgjk'" + C_id);
            var resource = location.protocol + "//" + location.host + "/api/Search/GetCourses_Single_User";
            $http.post(resource, data).success(function (data, status) {
          
              
                $scope.GetAllSingleUserCourses = data;
          
        });

        

    }

    


        $scope.GetAllSingleUserCourseModules = [];

        $scope.GetSingleUserCourseModules = function () {

            var Email = $('#UserName').val();
            var C_id = $('#coursid').val();
            var data = {
                Username: Email,
                CourseId: C_id

            }

            //  alert(S_id + " dgjk'" + C_id);
            var resource = location.protocol + "//" + location.host + "/api/Search/GetCourseModules_Single_User";
            $http.post(resource, data).success(function (data, status) {


                $scope.GetAllSingleUserCourseModules = data;

            });



        }

        


        $scope.GetAllCourseProgress = [];

        $scope.GetCourseProgress = function () {

            var Email = $('#UserName').val();
            var C_id = $('#coursid').val();
            var data = {
                Username: Email,
                CourseId: C_id

            }

            //  alert(S_id + " dgjk'" + C_id);
            var resource = location.protocol + "//" + location.host + "/api/Search/GetCourseProgress";
            $http.post(resource, data).success(function (data, status) {


                $scope.GetAllCourseProgress = data;

            });



        }

       




        $scope.GetContentProgress = [];

    

        $scope.GetModuleContentDropboxApi = [];

        $scope.ShowContentModules = function (CourseName, Modulename, ModuleId) {
        //    alert();
            $scope.GetModuleContentDropboxApi = "";
        
            var length;



            var config = {
                headers: {
                    'Authorization': 'Bearer ' + $scope.DropboxKey,
                    'Content-Type': 'application/json'
                }
            }
            //angular.forEach($scope.GetAllSingleUserCourseModules, function (value, key) {



            var path = "/Courses/" + CourseName + "/Modules/" + Modulename + "/";

            var data1 = {
                "path": path


            }
            var resource1 = "https://api.dropboxapi.com/2/files/list_folder";
            $http.post(resource1, data1, config).success(function (data, status) {


                $scope.GetModuleContentDropboxApi = data.entries;

                length =  $scope.GetModuleContentDropboxApi.length;

               $scope.DropboxContent_Id = [];
              

                var resource = location.protocol + "//" + location.host + "/api/Search/selectDropboxContent_Id";

                var d = {
                    "ModuleId": ModuleId
                }
                $http.post(resource, d).success(function (data, status) {

                    $scope.DropboxContent_Id = data;

                    angular.forEach($scope.DropboxContent_Id, function (value, key) {
                    
                        if (value.ModuleId == ModuleId) {
                            $scope.GetModuleContentDropboxApi[key].id = value.ContentId;
                           
                        //    alert(value.ContentId);
                        }

                    });


                }).error(function (data, status) {
                    // this isn't happening:
                })


                var count = 0 ;
                angular.forEach($scope.GetAllSingleUserCourseModules, function (value, key) {
                    count = count + 1
                 //   alert();

                   

                   

                    //if (value.ModuleName == Modulename) {

                    //    $("#"+id).css("display", "block");
                    //} else {


                    //    $("#abc" + count).css("display", "none");
                    //}

                });

                //angular.forEach(data.entries, function (value, key) {

                //  //  alert(value.name)

                //    var path2 = "/Courses/" + CourseName + "/Modules/" + Modulename + "/" + value.name ;

                //    var data2 = {
                //        "path": path2


                //    }

                //    var resource2 = "https://api.dropboxapi.com/2/files/get_temporary_link";
                //    $http.post(resource2, data2, config).success(function (data, status) {

                //        $scope.GetModuleContentDropboxApi = data;

                //        alert($scope.GetModuleContentDropboxApi.metadata.name + "  " + $scope.GetModuleContentDropboxApi.link)

                //     //   $window.location.href = 'https://api.dropboxapi.com/2/files/get_temporary_link';

                //    });

                //    });



                var perCount = 0;

                var data4 = { "Username": $scope.UserName };

                var resource4 = location.protocol + "//" + location.host + "/api/Search/GetContentProgress";

                $http.post(resource4, data4).success(function (data, status) {
                    $scope.GetContentProgress = data;

                    var jun = -1;
                  

                    angular.forEach($scope.GetContentProgress, function (value, key) {

                        if (value.ModuleId == ModuleId) {
                            perCount++;
                            //   alert(ModuleId);

                        }

                    });

                    $scope.modulePer = perCount / $scope.GetModuleContentDropboxApi.length * 100 + "%";

                    angular.forEach($scope.GetAllSingleUserCourseModules, function (value, key) {
                        jun++;
                        if (ModuleId == value.ModuleId) {
                        //    alert(value.Module_Per + "  " + jun)
                            $scope.GetAllSingleUserCourseModules[jun].Module_Per = Math.round(perCount / $scope.GetModuleContentDropboxApi.length * 100) ;
                        }
                    });

                    var Email = $('#UserName').val();
                    var C_id = $('#coursid').val();
                    var data4 = {
                        Username: Email,
                        CourseId: C_id

                    }

                    //  alert(S_id + " dgjk'" + C_id);
                    var resource = location.protocol + "//" + location.host + "/api/Search/GetCourseProgress";
                    $http.post(resource, data4).success(function (data, status) {


                        $scope.GetAllCourseProgress = data;

                    });

                   // alert($scope.GetModuleContentDropboxApi.length + "  " + perCount)
                });


            });
            //});

            //var data44 = {
            //    "path": "id:y0FGkpiUfvAAAAAAAAAB6Q",
            //    "format": "png",
            //    "size": "w64h64",
            //    "mode": "strict"


            //}
            //var resource44 = "https://content.dropboxapi.com/2/files/get_thumbnail";
            //        $http.post(resource44, data44, config).success(function (data, status) {


            //});


            var count = 0;
            angular.forEach($scope.GetModuleContentDropboxApi, function (value, key) {
                count = count + 1
                //   alert();





                //if (value.ModuleName == Modulename) {

                //    $("#"+id).css("display", "block");
                //} else {


                //    $("#abc" + count).css("display", "none");
                //}

            });

          






        }


        $scope.getAllExams = [];

      $scope.getExams = function () {
            var resource = location.protocol + "//" + location.host + "/api/Search/GetExams";

            $http.get(resource).success(function (data, status) {

             
             
                    $scope.getAllExams = data;
          
            })
                .error(function (data, status) {
                    // this isn't happening:
                })



        }

     
      $scope.getExamsForTeacher = function () {
          var data = {
              TeacherUsername: $scope.UserName

          }
          var resource = location.protocol + "//" + location.host + "/api/Search/GetExamsForTeacher";

          $http.post(resource,data).success(function (data, status) {



              $scope.getAllExams = data;

          })
              .error(function (data, status) {
                  // this isn't happening:
              })



      }


        $scope.getAllExamsPerCourse = [];

        $scope.getExamsPerCourse = function () {
            var key;
            var CourseId = $('#coursid').val();
       //     alert(CourseId);
            if (CourseId != undefined) {
               
                var resource = location.protocol + "//" + location.host + "/api/Search/GetDropboxKey";

                $http.get(resource).success(function (data, status) {
                    key = data;
                    
                
            

                var resource = location.protocol + "//" + location.host + "/api/Search/getExamsPerCourse";

                $http.post(resource, CourseId).success(function (data, status) {
                    $scope.getAllExamsPerCourse = data;

                })
                    .error(function (data, status) {
                        // this isn't happening:
                    })


                //$scope.DropboxContent_Id = [];

                //var resource = location.protocol + "//" + location.host + "/api/Search/selectDropboxContent_Id";

                //$http.get(resource).success(function (data, status) {
                //    $scope.DropboxContent_Id = data;

                //}).error(function (data, status) {
                //    // this isn't happening:
                //})








                var CourseName;
                var ModuleId;

                var Email = $('#UserName').val();
                var data2 = {
                    Email: Email

                }

                //  alert(S_id + " dgjk'" + C_id);
                var resource = location.protocol + "//" + location.host + "/api/Search/GetCourses_Single_User";
                $http.post(resource, data2).success(function (data, status) {


                    angular.forEach(data, function (value, key) {

                        if (value.CourseID == CourseId) {
                            CourseName = value.CourseName;
                            //  alert(CourseName); 
                        }




                    });

                });






                var config = {
                    headers: {
                        'Authorization': 'Bearer ' + key,
                        'Content-Type': 'application/json'
                    }
                }




                var data = {
                    Username: Email,
                    CourseId: CourseId

                }
                var Path;
                //  alert(S_id + " dgjk'" + C_id);
                var resource = location.protocol + "//" + location.host + "/api/Search/GetCourseModules_Single_User";
                $http.post(resource, data).success(function (data, status) {


                    $scope.GetAllSingleUserCourseModules = data;


                    angular.forEach($scope.GetAllSingleUserCourseModules, function (value, key) {


                        // alert("Courses/" + CourseName +"/Modules/" +value.ModuleName);
                        ModuleId = value.ModuleId;
                        Path = "/Courses/" + CourseName + "/Modules/" + value.ModuleName;

                        var data1 = {
                            "path": Path


                        }
                        //     alert("1");
                        var jun;
                        var data2 = {

                            "ContentType": "",
                            "ContentName": jun,
                            "ContentURL": Path,
                            "ModuleId": ModuleId,
                            "DropboxId": ""

                        }

                        var resource1 = "https://api.dropboxapi.com/2/files/list_folder";
                        var resource3 = location.protocol + "//" + location.host + "/api/Search/InsertCourseModules_Content";
                        $http.post(resource1, data1, config).success(function (data, status) {


                            angular.forEach(data.entries, function (value, key) {






                                if (value.name.split('.').pop().toLowerCase() == 'mp4' || value.name.split('.').pop().toLowerCase() == 'pdf') {

                                    var type = value.name.split('.').pop().toLowerCase();
                                    data2.ContentName = value.name;
                                    data2.DropboxId = value.id;
                                    //   alert(data2.ContentName + "Ԗ" + data2.ContentURL + "Ԗ"  + data2.ModuleId);
                                    //    alert("2");

                                    var data3 = {

                                        "ContentType": type,
                                        "ContentName": data2.ContentName,
                                        "ContentURL": data2.ContentURL,
                                        "ModuleId": data2.ModuleId,
                                        "DropboxId": data2.DropboxId

                                    }


                                    $http.post(resource3, data3).success(function (data, status) {

                                        //   alert(data);
                                        //      alert(data2.ContentName + "Ԗ" + data2.ContentURL + "Ԗ" + data2.ModuleId);

                                    });



                                }


                            });




                        });



                    });




                });

                })

            } else {
      //          alert("fs");
            }
        }

        $scope.getExamsPerCourse();



        $scope.getAllEnrolledStudents = [];

        $scope.getEnrolledStudents = function () {
            var resource = location.protocol + "//" + location.host + "/api/Search/GetEnrolledStudents";

            $http.get(resource).success(function (data, status) {
                $scope.getAllEnrolledStudents = data;

            })
                .error(function (data, status) {
                    // this isn't happening:
                })



        }

    
        $scope.getAllEnrolledCoursesStudents = [];

        $scope.getEnrolledCoursesStudents = function (courseid) {

            $scope.getAllEnrolledCoursesStudents = "";
            var data = {

                CourseID: courseid
            }
            var resource = location.protocol + "//" + location.host + "/api/Search/GetEnrolledCoursesStudent";

            $http.post(resource,data).success(function (data, status) {
                $scope.getAllEnrolledCoursesStudents = data;

            })
                .error(function (data, status) {
                    // this isn't happening:
                })



        }



        $scope.AddExam = function () {
                       
            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                    templateUrl: 'addExam.html',
                    
                    controller: 'addExamModalInstanceCtrl',
                    windowClass: 'app-modal-window',
                    size: '',
                    resolve: {
                        courses: function () {
                            if ($scope.role == 1){
                                return $scope.getAllCourses;
                            } else {

                                return $scope.getAllSingleTeachersCourses;
                            }
                           
                        }
                      
                    }
                });

          

            modalInstance.result.then(
                function handleResolve(response) {
                    $scope.getExams();
                },
                function handleReject(error) {
                   // alert("Alert rejected!");
                }
            );

        }

        $scope.AddQues = function (Examid, examName) {
            
            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'addQues.html',

                controller: 'addQuesModalInstanceCtrl',
                windowClass: 'app-modal-window',
                size: 'lg',
                resolve: {
                    ExamId: function () {
                        return Examid;
                    },
                    ExamName: function () {
                        return examName;
                    }
                }
            });



            modalInstance.result.then(
                function handleResolve(response) {
                  //  $scope.getExams();
                },
                function handleReject(error) {
                    // alert("Alert rejected!");
                }
            );

        }


        $scope.ViewQues = function (Examid,ExamName) {

            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'viewQues.html',

                controller: 'viewQuesModalInstanceCtrl',
                windowClass: 'app-modal-window',
                size: 'lg',
                resolve: {
                    ExamId: function () {
                        return Examid;
                    },
                    ExamName: function () {
                        return ExamName;
                    }
                   
                }
            });



            modalInstance.result.then(
                function handleResolve(response) {
                    //  $scope.getExams();
                },
                function handleReject(error) {
                    // alert("Alert rejected!");
                }
            );

        }

        $scope.ViewStudentExam = function (Examid, ExamName,Username,Result,comments,resultid,courseid) {

          

            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'viewStudentExam.html',

                controller: 'viewStudentExamModalInstanceCtrl',
                windowClass: 'app-modal-window',
                size: 'lg',
                resolve: {
                    ExamId: function () {
                        return Examid;
                    },
                    ExamName: function () {
                        return ExamName;
                    },
                    Username: function () {
                        return Username;
                    },

                    Result: function () {
                        return Result;
                    },
                    Comments: function () {
                        return comments;
                    },
                    Resultid: function () {
                        return resultid;
                    },
                    Courseid: function () {
                        return courseid;
                    },
                    CheckUser: function () {
                        if($scope.role == 2 ){
                            return "student";
                        } else if ($scope.role == 3) {
                            return "teacher";
                        }
                      
                    }

                }
            });



            modalInstance.result.then(
                function handleResolve(response) {
                    //  $scope.getExams();
                   // alert(response.Courseid);
                    $scope.getEnrolledCoursesStudents(response.Courseid);
                },
                function handleReject(error) {
                //    alert("Alert rejected!", error.locations);
                //    $scope.getEnrolledCoursesStudents(4068);
                }
            );

        }

        $scope.StartExam = function (Examid) {

            var CourseId = $('#coursid').val();
            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'StartExam.html',

                controller: 'StartExamModalInstanceCtrl',
                windowClass: 'app-modal-window',
                size: 'lg',
                resolve: {
                    ExamId: function () {
                        return Examid;
                    },
                    Username: function () {
                        return $scope.UserName;
                    }

                }
            });



            modalInstance.result.then(
                function handleResolve(response) {
                    //  $scope.getExams();
                },
                function handleReject(error) {
                    // alert("Alert rejected!");
                }
            );

        }

        $scope.ShowContent = function (coursename,modulename,moduleid) {

            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'ShowContent.html',

                controller: 'showContentModalInstanceCtrl',
                windowClass: 'app-modal-window',
                size: 'lg',
                resolve: {
                    CourseName: function () {
                        return coursename;

                    }, ModuleName: function () {
                        return modulename;

                    }, ModuleId: function () {
                        return moduleid;

                    }, DropboxKey: function () {
                        return $scope.DropboxKey;

                    }

                }
            });



            modalInstance.result.then(
                function handleResolve(response) {
                    $scope.getExams();
                },
                function handleReject(error) {
                    // alert("Alert rejected!");
                }
            );

        }


        $scope.getAllExamResult = [];

        $scope.getExamResult = function () {

            var data = {
                "TeacherUserName": $scope.UserName
            }
            var resource = location.protocol + "//" + location.host + "/api/Search/GetExamResult";

            $http.post(resource, data).success(function (data, status) {
                if (data == "") {
                    $scope.showtbl = false;
                } else {
                    $scope.showtbl = true;
                    $scope.getAllExamResult = data;
                }
               

            })
                .error(function (data, status) {
                    // this isn't happening:
                })



        }

        $scope.deleteCourses = function (check,CourseName,CourseId) {

          //  alert(CourseId);

            if (CourseName == undefined){

                CourseName = "";
                var path = "/Courses";
               
            } else {
                var path = "/Courses/" + CourseName;

            }
           
          //  alert(courseId + " " + CourseName);

            function deleteCourseFromDropbox() {

              //  alert($scope.DropboxKey);

                var config = {
                    headers: {
                        'Authorization': 'Bearer ' + $scope.DropboxKey,
                        'Content-Type': 'application/json'
                    }
                }

               

                var data1 = {
                    "path": path


                }
                var resource1 = "https://api.dropboxapi.com/2/files/delete_v2";
                if (window.confirm("Are you sure to Delete Course ?")) {
                    $http.post(resource1, data1, config).success(function (data, status) {

                        //        if (data.metadata.path_display == path) {

                   


                      



                        //     } 

                    });

                    var data = {
                        "CourseID": CourseId,
                        "deleteall": check,
                        "CourseName": CourseName
                    }
                    var resource = location.protocol + "//" + location.host + "/api/Search/DeleteCourses";

                    $http.post(resource, data).success(function (data, status) {
                        if (data == true) {

                            $scope.getCourses();

                        } else {

                        }


                    })
                }
            }
            deleteCourseFromDropbox();


        }


});


courseApp.controller('showContentModalInstanceCtrl', function ($scope, $http, $uibModal, $uibModalInstance, $log, $window, CourseName, ModuleName, ModuleId,DropboxKey) {
    $scope.modalTitle =  CourseName + "/" + ModuleName;

   // alert(ModuleId);
   // alert(DropboxKey);

    $scope.GetModuleContentDropboxApi = [];

    $scope.ShowContentModules = function () {

    $scope.GetModuleContentDropboxApi = "";
    $scope.GetContent = "";


        var config = {
            headers: {
                'Authorization': 'Bearer ' + DropboxKey,
                'Content-Type': 'application/json'
            }
        }
       
        var path = "/Courses/" + CourseName + "/Modules/" + ModuleName + "/";

        var data1 = {
            "path": path


        }
        var resource1 = "https://api.dropboxapi.com/2/files/list_folder";
        $http.post(resource1, data1, config).success(function (data, status) {


            $scope.GetModuleContentDropboxApi = data.entries;

            if (data.entries == ""){

                $scope.showtable = false;
                $scope.showerror = true;
            } else {
                $scope.showerror = false;
                $scope.showtable = true;
            }

          

           
            var data5 = {
                ModuleId: ModuleId
            }

            var resource = location.protocol + "//" + location.host + "/api/Search/selectDropboxContent_Id";
            $http.post(resource, data5).success(function (data1, status) {



                $scope.GetContent = data1;

           angular.forEach($scope.GetModuleContentDropboxApi, function (value1, key) {

                angular.forEach($scope.GetContent, function (value, key) {

                    if (value.ContentName == value1.name) {


                 //       alert(value.ContentName);
                       }

                 //   alert(value.ContentName);

                });

                });

          //      alert("next")

           angular.forEach($scope.GetContent, function (value1, key) {

               angular.forEach($scope.GetModuleContentDropboxApi, function (value, key) {

                   if (value1.ContentName != value.name) {


                    //   alert(value.name);
                   }

                   //   alert(value.ContentName);

               });

           });





            });




            });

            

                //angular.forEach($scope.GetModuleContentDropboxApi, function (value, key) {

              

                //    angular.forEach($scope.GetContent, function (value1, key) {

                //        if (value1.ModuleId == ModuleId){

                //            if (value.id == value1.DropboxId) {

                //           //     alert("if " + value1.DropboxId + " name: " + value1.ContentName);
                //                alert("if " + value.id + " name: " + value.name);
                            

                //            } else {

                //             //   alert("else  " + value1.DropboxId + " name: " + value1.ContentName);
                //                alert("else  " + value.id + " name: " + value.name);
                //            }
                       
                //        }



                //    });
                //});



         //   });
            
     //   });
      
       




    }

    $scope.ShowContentModules();


        

        $scope.AddTODB = function (filename,title) {


            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'AddToDB.html',

                controller: 'addToDBModalInstanceCtrl',
                windowClass: 'app-modal-window',
                size: '',
                resolve: {
                    CourseName: function () {
                        return CourseName;

                    }, ModuleName: function () {
                        return ModuleName;

                    }, ModuleId: function () {
                        return ModuleId;

                    }, FileName: function () {
                        return filename;

                    }, Title: function () {
                        return title;

                    }


                }
            });



            modalInstance.result.then(
                function handleResolve(response) {

                },
                function handleReject(error) {

                    $scope.ShowContentModules();
                }
            );

        


        }

   $scope.uploadContent = function () {

            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'UploadContent.html',

                controller: 'uploadContentModalInstanceCtrl',
                windowClass: 'app-modal-window',
                size: '',
                resolve: {
                    CourseName: function () {
                        return CourseName;

                    }, ModuleName: function () {
                        return ModuleName;

                    }, ModuleId: function () {
                        return ModuleId;

                    }

                }
            });



            modalInstance.result.then(
                function handleResolve(response) {
                  
                },
                function handleReject(error) {
                 
                    $scope.ShowContentModules();
                }
            );

        }


   $scope.fileClick = function ($event, fileSource) {

   //    alert(DropboxKey);

       var config = {
           headers: {
               'Authorization': 'Bearer ' + DropboxKey,
               'Content-Type': 'application/json'
           }
       }
       var data2 = {
           "path": fileSource,
           "short_url": false


       }



       var resource2 = "https://api.dropboxapi.com/2/sharing/create_shared_link";
       $http.post(resource2, data2, config).success(function (data, status) {


           $window.open(
               data.url,
               '_blank' // <- This is what makes it open in a new window.
           );



       });




   }



   $scope.videoClick = function ($event, videoSource) {
       // alert(cid);
       //  alert(videoSource);
    //   alert(DropboxKey);
       var config = {
           headers: {
               'Authorization': 'Bearer ' + DropboxKey,
               'Content-Type': 'application/json'
           }
       }
       var data2 = {
           "path": videoSource


       }

       var resource2 = "https://api.dropboxapi.com/2/files/get_temporary_link";
       $http.post(resource2, data2, config).success(function (data, status) {


        //   alert(data.metadata.sharing_info.read_only);

         //  alert(data.link)

           $log.info("videoClick", data.link)
           $scope.open('lg', data.link);





       });




   }
 
   $scope.open = function (size, videoSource) {
       $log.info("open", videoSource);
       var modalInstance = $uibModal.open({
           animation: $scope.animationsEnabled,
           templateUrl: 'myModal.html',
           controller: 'ModalInstanceCtrl',
           backdrop: true,
           size: size,
           resolve: {
               videoSource: function () {
                   return videoSource;
               }
           }
       });

       modalInstance.result.then(function (result) {
           //
       }, function () {
           $log.info('Modal dismissed at: ' + new Date());
       });
   };

    $scope.ok = function () {
        $uibModalInstance.close('ok');
    };


    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };


});


courseApp.controller('uploadContentModalInstanceCtrl', function ($scope, $http, $uibModal, $uibModalInstance, CourseName, ModuleName, ModuleId) {
    $scope.modalTitle = "Upload Content";
    // alert(CourseName + " " + ModuleName + " " + ModuleId);
    var path = "/Courses/" + CourseName + "/Modules/" + ModuleName;
    $scope.isSuccess1 = false;
    $scope.isError1 = false;
    $scope.submit = function () {

        //   $scope.isSuccess1 = false;
        //   $scope.isError1 = false;
        document.getElementById("loadingImg").style.display = "block";
        document.getElementById('isSuccess1').innerHTML = "";
        var data = {

            name: $scope.filename

        }

        //  alert($scope.filename);
        var formdata = new FormData(); //FormData object
        var fileInput = document.getElementById('file');
        var fullPath = document.getElementById('file').value;
        //Iterating through each files selected in fileInput

        //Appending each file to FormData object
        formdata.append(path, fileInput.files[0]);


        if (fullPath && $scope.filename && $scope.filename != undefined) {


            var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
            var filename = fullPath.substring(startIndex);
            if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
                filename = filename.substring(1);
            }

            var filetype = filename.split('.').pop().toLowerCase();

            if (filetype == "pdf" || filetype == "mp4") {




                //   alert(filename +" "+ filetype);



                var xhr = new XMLHttpRequest();
                xhr.open('POST', '/Home/Upload');
                xhr.send(formdata);

                xhr.onreadystatechange = function () {
                    //    document.getElementById("loadingImg").style.display = "block";
                    if (xhr.readyState == 4 && xhr.status == 200) {
                        if (xhr.responseText == "\"OK\"") {
                            document.getElementById("loadingImg").style.display = "none";
                            document.getElementById('isSuccess1').innerHTML = "<div  class='alert alert-success alert-block'>Successfully Upload File ...<div>";
                            //    alert(xhr.responseText + "if");

                            var data3 = {

                                "ContentType": filetype,
                                "ContentName": filename,
                                "ContentURL": path,
                                "ModuleId": ModuleId,
                                "DropboxId": "",
                                "ContentTitle": data.name

                            }

                            var resource3 = location.protocol + "//" + location.host + "/api/Search/InsertCourseModules_Content";
                            $http.post(resource3, data3).success(function (data, status) {



                            });


                        } else {
                            document.getElementById("loadingImg").style.display = "none";
                            document.getElementById('isSuccess1').innerHTML = "<div  class='alert alert-danger alert-block'>Not Upload<div>";

                        }

                    }

                }
            } else {
                document.getElementById("loadingImg").style.display = "none";
                document.getElementById('isSuccess1').innerHTML = "<div  class='alert alert-danger alert-block'>Only Accept pdf or mp4 format ...<div>";
            }

        } else {
            document.getElementById("loadingImg").style.display = "none";
            document.getElementById('isSuccess1').innerHTML = "<div  class='alert alert-danger alert-block'>Required All Fields..<div>";
        }

        //var resource = "/home/upload";
        //$http.post(resource, formdata).success(function (data, status) {




        //    if (data == null) {
        //        $scope.isError = true;
        //        $scope.errormessage = "No Questions Found...";
        //    } else {
        //        $scope.isError = false;

        //        $scope.Correct1 = data;
        //    }


        //    });



    }




    $scope.ok = function () {
        $uibModalInstance.close('ok');
    };


    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

});

courseApp.controller('addToDBModalInstanceCtrl', function ($scope, $http, $uibModal, $uibModalInstance, CourseName, ModuleName, ModuleId, FileName,Title) {
    $scope.modalTitle = "Add To Database";
   
    $scope.titlename = Title;
    $scope.add = function () {

        $scope.isSuccess = false;
        $scope.isError = false;

        if ($scope.titlename && $scope.titlename != undefined && $scope.titlename != null)
        {
            var title = $scope.titlename;
            var filetype = FileName.split('.').pop().toLowerCase();

            var path1 = "/Courses/" + CourseName + "/Modules/" + ModuleName;
            var data3 = {

                "ContentType": filetype,
                "ContentName": FileName,
                "ContentURL": path1,
                "ModuleId": ModuleId,
                "DropboxId": "",
                "ContentTitle": title

            }

            var resource3 = location.protocol + "//" + location.host + "/api/Search/InsertCourseModules_Content";
            $http.post(resource3, data3).success(function (data, status) {

                if (data == true) {

                    $scope.isError = false;
                    $scope.isSuccess = true;

                    $scope.successMessage = "Successfully Added...."
                } else {
                    $scope.isSuccess = false;
                    $scope.isError = true;
                   
                    $scope.errormessage = "Try Again";
                }

            });


        } else {

            $scope.isSuccess = false;
            $scope.isError = true;

            $scope.errormessage = "Enter Your Title...";
        }

          

    };


      


    $scope.ok = function () {
        $uibModalInstance.close('ok');
    };


    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

});


courseApp.controller('StartExamModalInstanceCtrl', function ($scope, $http, $uibModal, $uibModalInstance, ExamId, Username) {
    $scope.modalTitle = "Exam Questions";
    var totalQuestions = 0;
    var next = 0;
    var previous = 0;
    $scope.ViewAllQuestionAndAnswers = [];
    $scope.Correct = [];
    $scope.okbtn = false;
    $scope.previous_show = false;
    $scope.submitbtn = false;
    $scope.next_show = true;
    $scope.Isresults = false;
    var count = 0;
    //$scope.AnswerID = true;
    //$scope.CorrectAnswer = false;

    //$scope.junv = false;

    //$scope.jun = function (event,ab) {
    //    alert(ab);

    //    if ($scope.junv == event.target.value) $scope.junv = false


    //    $scope.junv = ab;
    //}


    $scope.jun = function (position, entities, qid) {

     //   alert(position);
        angular.forEach(entities, function (subscription, index) {
            
            if (qid == subscription.QuesId && position != subscription.AnswerID) {
            //if (position != index) {
          //      alert(subscription.CorrectAnswer);
               
                subscription.CorrectAnswer = false;
             //   $('#y' + qid).val(false);
                //}
            }
        });
    }


    $scope.submit = function () {

        var Correct1 = []
        var Iscorrect = 0;
        var IsWrong = 0;

        $scope.StudentAnswers = {
            QusestionId: "",
            Answerid: "",
            CorrectAnswer: ""


        };

        var records = "";

       

       

        var resource = location.protocol + "//" + location.host + "/api/Search/ViewQuestionAndAnswers";
        $http.post(resource, ExamId).success(function (data, status) {




            if (data == null) {
                $scope.isError = true;
                $scope.errormessage = "No Questions Found...";
            } else {
                $scope.isError = false;
  
                $scope.Correct1 = data;

                

              

                angular.forEach($scope.Correct1.Questions, function (value, key) {
                    var u_op1 = false;
                    var u_op2 = false;
                    var u_op3 = false;
                    var u_op4 = false;

                    var _op1 = false;
                    var _op2 = false;
                    var _op3 = false;
                    var _op4 = false;
                    var Qid = value.QuesId;
                  
                 //   alert(Qid);
                    var count = 0;
                    for (var i in $scope.Correct1.Answers) {
                       
                        if (Qid == $scope.Correct1.Answers[i].QuesId && Qid == $scope.ViewAllQuestionAndAnswers.Answers[i].QuesId){

                        count++;

                            var r = $scope.Correct1.Answers[i].CorrectAnswer;
                            var r1 = $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer;

                       //     alert(Qid + ":  Api " + $scope.Correct1.Answers[i].CorrectAnswer + " User:" + $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer);
                        //    alert($scope.Correct1.Answers[i].CorrectAnswer);
                         //   alert($scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer);

                            if (count == 1){
                                u_op1 = $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer;
                                _op1 = $scope.Correct1.Answers[i].CorrectAnswer;
                           //     alert(Qid + " op1:" + $scope.ViewAllQuestionAndAnswers.Answers[i].QuesId)
                            //    if (u_op1 == true) {
                                    $scope.StudentAnswers.QusestionId += $scope.ViewAllQuestionAndAnswers.Answers[i].QuesId + "‰";
                                    $scope.StudentAnswers.Answerid += $scope.ViewAllQuestionAndAnswers.Answers[i].AnswerID + "‰";
                                    $scope.StudentAnswers.CorrectAnswer += $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer + "‰";

                                    records += $scope.ViewAllQuestionAndAnswers.Answers[i].QuesId + "‰" +
                                        $scope.ViewAllQuestionAndAnswers.Answers[i].AnswerID + "‰" +
                                        $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer + "‡";
                           //     }

                            }

                            if (count == 2) {
                                u_op2 = $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer;
                                _op2 = $scope.Correct1.Answers[i].CorrectAnswer;
                            //    alert(Qid + " op2:" + $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer)
                          //      if (u_op2 == true){
                                    $scope.StudentAnswers.QusestionId += $scope.ViewAllQuestionAndAnswers.Answers[i].QuesId + "‰";
                                    $scope.StudentAnswers.Answerid += $scope.ViewAllQuestionAndAnswers.Answers[i].AnswerID + "‰";
                                    $scope.StudentAnswers.CorrectAnswer += $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer + "‰";

                                records += $scope.ViewAllQuestionAndAnswers.Answers[i].QuesId + "‰" +
                                    $scope.ViewAllQuestionAndAnswers.Answers[i].AnswerID + "‰" +
                                    $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer + "‡";
                         //       }
                            }
                            if (count == 3) {
                                u_op3 = $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer;
                                _op3 = $scope.Correct1.Answers[i].CorrectAnswer;
                              //  alert(Qid + " op3:" + $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer)


                          //      if (u_op3 == true) {
                                    $scope.StudentAnswers.QusestionId += $scope.ViewAllQuestionAndAnswers.Answers[i].QuesId + "‰";
                                    $scope.StudentAnswers.Answerid += $scope.ViewAllQuestionAndAnswers.Answers[i].AnswerID + "‰";
                                    $scope.StudentAnswers.CorrectAnswer += $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer + "‰";

                                    records += $scope.ViewAllQuestionAndAnswers.Answers[i].QuesId + "‰" +
                                        $scope.ViewAllQuestionAndAnswers.Answers[i].AnswerID + "‰" +
                                        $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer + "‡";
                           //     }
                            }
                            if (count == 4) {
                                u_op4 = $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer;
                                _op4 = $scope.Correct1.Answers[i].CorrectAnswer;
                            //    alert(Qid + " op4:" + $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer)

                            //    if (u_op4 == true) {
                                    $scope.StudentAnswers.QusestionId += $scope.ViewAllQuestionAndAnswers.Answers[i].QuesId + "‰";
                                    $scope.StudentAnswers.Answerid += $scope.ViewAllQuestionAndAnswers.Answers[i].AnswerID + "‰";
                                    $scope.StudentAnswers.CorrectAnswer += $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer + "‰";

                                    records += $scope.ViewAllQuestionAndAnswers.Answers[i].QuesId + "‰" +
                                        $scope.ViewAllQuestionAndAnswers.Answers[i].AnswerID + "‰" +
                                        $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer + "‡";
                          //      }

                            }


                         

                            //if (r == r1) {

                            //    alert(QuesId + " Right Answer");
                            //} else {

                            //    alert(QuesId + " Wrong Answer");
                            //}

                        }

                       



                        //if (u_op1 == _op1 && u_op2 == _op2 && u_op3 == _op3 && u_op4 == _op4) {

                        //    alert(Qid + " Right Answer");
                        //} else {

                        //    alert(Qid + " Wrong Answer");
                        //}


                     //   total += count;


                    }
               
                    if (u_op1 == _op1 && u_op2 == _op2 && u_op3 == _op3 && u_op4 == _op4) {

                      //  alert(u_op1);
                        Iscorrect++;
                //        alert(Qid + " Right Answer");
                    //    alert(Qid + ":\n1 API:" + _op1 + "::User:" + u_op1 + "\n" + "2 API:" + _op2 + "::User:" + u_op2 + "\n" + "3 API:" + _op3 + "::User:" + u_op3 + "\n" + "4 API:" + _op4 + "::User:" + u_op4 + "\n");
                        



                    } else {

                        IsWrong++;
                 //       alert(Qid + " Wrong Answer");

                  //      alert(Qid + ":\n1 API:" + _op1 + "::User:" + u_op1 + "\n" + "2 API:" + _op2 + "::User:" + u_op2 + "\n" + "3 API:" + _op3 + "::User:" + u_op3 + "\n" + "4 API:" + _op4 + "::User:" + u_op4 + "\n");
                     
                    }
                  
                  
                  

                });
             //   alert("QuestionId: " + $scope.StudentAnswers.QusestionId + "AnswerId: " + $scope.StudentAnswers.Answerid + "Answer: " + $scope.StudentAnswers.CorrectAnswer);
             //   alert(records);
                var result = Iscorrect / totalQuestions * 100;

            //    alert("Wrong is: " + IsWrong + " and Right is: " + Iscorrect + ", Total:" + totalQuestions + " % is " + result + "%");


                var data1 = {
                    Username: Username,
                    ExamId: ExamId,
                    TotalWrongAnswers: IsWrong,
                    TotalCorrectAnswers: Iscorrect,
                    Result: result + "%",
                    Records: records

                }


     //   $scope.ViewQuestionAndAnswers = function () {

            var resource = location.protocol + "//" + location.host + "/api/Search/InsertResult";
            $http.post(resource, data1).success(function (data, status) {


                if (data = "true") {

                 //   alert("true");

                    $scope.show = false;
                    $scope.submitbtn = false;
                    $scope.next_show = false;
                    $scope.previous_show = false;
                 
                    $scope.TotalQues = totalQuestions;
                    $scope.WrongAns = IsWrong;
                    $scope.CorrectAns = Iscorrect;
                    $scope.per = result;

                    $scope.Isresults = true;
                    $scope.okbtn = true;
                }


            });


    //    }





                }




           


        });


       

        //angular.forEach($scope.ViewAllQuestionAndAnswers.Answers, function (value, key) {


        //    alert(value.CorrectAnswer);

        //});

        //var jun = $scope.ViewAllQuestionAndAnswers.Answers[0].CorrectAnswer;
        //alert(jun);

        //alert(jun);

        //alert($('#Q1128').val());

        //if ($('#abc1128').attr('checked')) {
        //    alert("fs");
        //}
       

        //$('#abc1128').change(function () {
        //    if ($(this).attr('checked')) {
        //        alert($(this).val('TRUE'));
        //    } else {
        //        alert($(this).val('FALSE'));
        //    }
        //});
    }

  
    $scope.next = function () {
       
        next++;
      
        $scope.show = 'index' + next;
      //  alert(totalQuestions);
        if (next != 1) {
            $scope.previous_show = true;
            $scope.submitbtn = false;








        }
        if (totalQuestions == next) {
            $scope.next_show = false;
            $scope.submitbtn = true;
        }
        //$scope.previous_show = true;
        //previous++;
        
        //var data = {
        //    CourseId: CourseId,
        //    Next: next,
        //    Previous: previous
        //}


        //$scope.ViewQuestionAndAnswers = function () {

        //    var resource = location.protocol + "//" + location.host + "/api/Search/ViewExamQuestion";
        //    $http.post(resource, data).success(function (data, status) {

        //        if (data == null) {
        //            $scope.isError = true;
        //            $scope.errormessage = "No Questions Found...";
        //        } else {
        //            $scope.isError = false;
        //            $scope.ViewQuestionAndAnswers = data;
        //        } 


        //    });


        //}
        //$scope.ViewQuestionAndAnswers();


        angular.forEach($scope.ViewAllQuestionAndAnswers.Questions, function (value, key) {

        //    alert(value.Question);


        });


    }

    $scope.next();



    $scope.previous = function () {

        next--;
        $scope.show = 'index' + next;

        if (next == 1)
            $scope.previous_show = false;

        $scope.next_show = true;
        $scope.submitbtn = false;
        //var data = {
        //    CourseId: CourseId,
        //    Next: next,
        //    Previous: previous
        //}


        //$scope.ViewQuestionAndAnswers = function () {

        //    var resource = location.protocol + "//" + location.host + "/api/Search/ViewExamQuestion";
        //    $http.post(resource, data).success(function (data, status) {

        //        if (data == null) {
        //            $scope.isError = true;
        //            $scope.errormessage = "No Questions Found...";
        //        } else {
        //            $scope.isError = false;
        //            $scope.ViewQuestionAndAnswers = data;
        //        }


        //    });


        //}
        //$scope.ViewQuestionAndAnswers();

    }


  

  
    $scope.ViewQuestionAndAnswers = function () {

      

        var resource = location.protocol + "//" + location.host + "/api/Search/ViewQuestionAndAnswers";
        $http.post(resource, ExamId).success(function (data, status) {




            if (data == null) {
                $scope.isError = true;
                $scope.errormessage = "No Questions Found...";
            } else {
                $scope.isError = false;
                $scope.Correct = data;
                $scope.ViewAllQuestionAndAnswers = data;
               
                totalQuestions = $scope.ViewAllQuestionAndAnswers.Questions.length;
           //     alert($scope.ViewAllQuestionAndAnswers.Questions.length);

                //$scope.ViewAllQuestionAndAnswers.Answers[0].CorrectAnswer = false;


                for (var i in $scope.ViewAllQuestionAndAnswers.Answers) {

                    $scope.ViewAllQuestionAndAnswers.Answers[i].CorrectAnswer = false;

                    //angular.forEach($scope.getAllModules, function (value, key) {

                    //    if (value.ModuleId == ss[i]) {


                    //        path += CourseName + "/Modules/" + value.ModuleName + ",";
                    //        ModuleName += value.ModuleName + ",";
                    //        //   $scope.AddCourseDirectory(path);

                    //    }
                    //});


                }


                //angular.forEach(data.Answers, function (value, key) {


                //    $scope.ViewAllQuestionAndAnswers.Answers.CorrectAnswer = false;

                //});
               

            }

            //var data1 = {

            //    Questions: { Question: [] },
            //    Answers: { Answer: []}
            //}

            //angular.forEach(data.Questions, function (value, key) {

            //    var QId = value.QuesId;

            // //   alert(value.Question);


            //    data1.Question = value.Question;
            //    angular.forEach(data.Answers, function (value1, key) {


            //        if (value1.QuesId == QId) {


            //            data1.Answers = value1.AnswerText;

            //           // alert(value1.AnswerText + "" + value1.CorrectAnswer);


            //        }






            //    });


            // //   alert(data1.Question + " Ans:" + data1.Answers);


            //});



        });


    }


    $scope.ViewQuestionAndAnswers();

  

 
   

    $scope.ok = function () {
        $uibModalInstance.close('ok');
    };


    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

});



courseApp.controller('viewQuesModalInstanceCtrl', function ($scope, $http, $uibModal, $uibModalInstance, ExamId, ExamName) {
    $scope.modalTitle = "View Questions (" + ExamName + ")";

   



    $scope.ViewAllQuestionAndAnswers = [];
    $scope.ViewQuestionAndAnswers = function () {

    
       
        
        var resource = location.protocol + "//" + location.host + "/api/Search/ViewQuestionAndAnswers";
        $http.post(resource, ExamId).success(function (data, status) {


           

            if (data == null) {
                $scope.isError = true;
                $scope.errormessage = "No Questions Found...";
            } else {
                $scope.isError = false;
                $scope.ViewQuestionAndAnswers = data;
            }

            //var data1 = {

            //    Questions: { Question: [] },
            //    Answers: { Answer: []}
            //}

            //angular.forEach(data.Questions, function (value, key) {
              
            //    var QId = value.QuesId;

            // //   alert(value.Question);

               
            //    data1.Question = value.Question;
            //    angular.forEach(data.Answers, function (value1, key) {

                
            //        if (value1.QuesId == QId) {


            //            data1.Answers = value1.AnswerText;

            //           // alert(value1.AnswerText + "" + value1.CorrectAnswer);


            //        }


                  



            //    });


            // //   alert(data1.Question + " Ans:" + data1.Answers);
              

            //});

          
            
        });

      
    }


    $scope.ViewQuestionAndAnswers();

    $scope.deleteQues = function (QuesId) {

     //   alert(ExamId);
        
            if (window.confirm("Are you sure to Delete Question ?")) {
                var resource = location.protocol + "//" + location.host + "/api/Search/deleteQues";
                $http.post(resource, QuesId).success(function (data, status) {
                   
                    if (data = "true") {
                     //   alert("fghf");


                        var resource = location.protocol + "//" + location.host + "/api/Search/ViewQuestionAndAnswers";
                        $http.post(resource, ExamId).success(function (data, status) {




                            if (data == null) {
                                $scope.isError = true;
                                $scope.errormessage = "No Questions Found...";
                                $scope.ViewQuestionAndAnswers = data;
                            } else {
                                $scope.isError = false;
                                $scope.ViewQuestionAndAnswers = data;
                            }
                        });
                       
                      //  $scope.successMessage = "The customer has been removed from Direct mail.";
                     //   $scope.onPropertySearch();
                    }
                })
            }
        
    }



    $scope.EditQues = function (QuesId) {

        var modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'editQues.html',

            controller: 'editQuesModalInstanceCtrl',
            windowClass: 'app-modal-window',
            size: 'lg',
            resolve: {
                QuesId: function () {
                    return QuesId;
                },
                ViewAllQuestionAndAnswersObj: function () {
                    return $scope.ViewQuestionAndAnswers;
                }

            }
        });



        modalInstance.result.then(
            function handleResolve(response) {
             //   $scope.ViewQuestionAndAnswers;
                //  $scope.getExams();

                var resource = location.protocol + "//" + location.host + "/api/Search/ViewQuestionAndAnswers";
                $http.post(resource, ExamId).success(function (data, status) {




                    if (data == null) {
                        $scope.isError = true;
                        $scope.errormessage = "No Questions Found...";
                    } else {
                        $scope.isError = false;
                        $scope.ViewQuestionAndAnswers = data;
                    }
                });
            },
            function handleReject(error) {
                // alert("Alert rejected!");
            }
        );

    }





    $scope.ok = function () {
        $uibModalInstance.close('ok');
    };


    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

});

courseApp.controller('viewStudentExamModalInstanceCtrl', function ($scope, $http, $uibModal, $uibModalInstance, ExamId, ExamName, Username, Result, Comments, Resultid, Courseid, CheckUser) {
    $scope.modalTitle = "View Result (" + ExamName + ")";

    $scope.comm = false;
    $scope.check = CheckUser;
    $scope.comments = Comments;


    $scope.ViewAllQuestionAndAnswers = [];
    $scope.ViewQuestionAndAnswers = function () {

        $scope.checkAns = [];

        var rc = 0; var wc = 0;

        var data = {
            ExamId: ExamId,
            Username: Username,
            Result: Result

        }

        var resource = location.protocol + "//" + location.host + "/api/Search/ExamRecords";
        $http.post(resource, data).success(function (data, status) {


            $scope.ViewQuestionAndAnswers = data;
          //  $scope.ViewQuestionAndAnswers[0].Question = "Right";

            $scope.comm = true;
            //if (data == null) {
            //    $scope.isError = true;
            //    $scope.errormessage = "No Questions Found...";
            //} else {
            //    $scope.isError = false;
            //    $scope.ViewQuestionAndAnswers = data;
            //}

            var count = 0; var count2 = 0;
            angular.forEach(data.Questions, function (value, key) {

                count = 0; count2 = 0;
                angular.forEach(data.Answers, function (value1, key) {


                    if (value1.QuesId == value.QuesId) {
                        count++;

                        if (value1.is_check_true == "Right"){
                            count2++;
                        }

                //   alert(value1.AnswerText + "" + value1.CorrectAnswer);


                    }



                

                });

            //    alert(count + " :: " + count2 );

                if (count == count2) {

               //     alert("true");
                //    $scope.ViewQuestionAndAnswers.Questions[key].Question = "Right";
                    var r = ++rc;

                    $scope.checkAns[key] = "Correct Answer!";
                    $scope.RightCount = "Total Right Ans: " + r; 
               //     $scope.ViewQuestionAndAnswers[0].Question = "Right";
               //     $scope.CheckQues = "Right";

                } else {
                    var r = ++wc;
                //    $scope.ViewQuestionAndAnswers[key].Question = "Wrong";
               //     $scope.CheckQues = "Wrong";
             //      alert("false");
              //      $scope.ViewQuestionAndAnswers.Questions[key].Question = "Wrong";
                    $scope.checkAns[key] = "Wrong Answer!";
                    $scope.WrongCount = "Total Wrong Ans: " + r; 
                }


            });

        });


    }


    $scope.ViewQuestionAndAnswers();

 
    $scope.UpdateComment = function () {


        if ($scope.comments == null) {
          //  alert('ad');
           var Com = null;
        } else {

            var Com = $scope.comments;
        }

     

        var data = {
            Comments: Com,
            Resultid: Resultid

        }

        var resource = location.protocol + "//" + location.host + "/api/Search/UpdateTeacherComment";
        $http.post(resource, data).success(function (data, status) {

            if (data == true) {
                $scope.ok();
            }
        });
    }


  //  $scope.UpdateComment();


    $scope.ok = function () {
     //   $uibModalInstance.close('ok');
        $uibModalInstance.close({ Courseid: Courseid });
    };



    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    //    $uibModalInstance.close({ locations: 4568 });
    };

});


courseApp.controller('editQuesModalInstanceCtrl', function ($scope, $http, $uibModalInstance, QuesId, ViewAllQuestionAndAnswersObj) {
    $scope.modalTitle = "Update Question";

    $scope.ans1 = false;
    $scope.ans2 = false;
    $scope.ans3 = false;
    $scope.ans4 = false;

    $scope.ans1Option = "";
    $scope.ans2Option = "";
    $scope.ans3Option = "";
    $scope.ans4Option = "";
    $scope.question = "";
   
    var answerCount = 0;
    var opid1 = -1;
    var opid2 = -1;
    var opid3 = -1;
    var opid4 = -1;
    var ExamId = 0;




    angular.forEach(ViewAllQuestionAndAnswersObj.Questions, function (value, key) {
        

        if (value.QuesId == QuesId) {

        //    alert(value.Question);

            $scope.question = value.Question;
            ExamId = value.ExamId;
           
            $scope.answerType = value.AnswerType;

            angular.forEach(ViewAllQuestionAndAnswersObj.Answers, function (value1, key) {
               

                if (value1.QuesId == QuesId) {
                    answerCount++;
                  //  $scope.ans+answerCount+Option == value1.AnswerText;
                 //   $scope.ans+answerCount == value1.CorrectAnswer;

                  //  alert(answerCount + " "+value1.AnswerText);

                    if (answerCount == 1) {
                        $scope.ans1Option = value1.AnswerText;
                        $scope.ans1 = value1.CorrectAnswer;
                        opid1 = value1.AnswerID;

                        if ($scope.ans1 == true)
                            $scope.option1style = { 'border-color': 'green' };
                        else
                            $scope.option1style = { 'border-color': 'red' };

                    }
                       
                    if (answerCount == 2) {
                        $scope.ans2Option = value1.AnswerText;
                        $scope.ans2 = value1.CorrectAnswer;
                        opid2 = value1.AnswerID;

                        if ($scope.ans2 == true)
                            $scope.option2style = { 'border-color': 'green' };
                        else
                            $scope.option2style = { 'border-color': 'red' };

                    }

                    if (answerCount == 3) {
                        $scope.ans3Option = value1.AnswerText;
                        $scope.ans3 = value1.CorrectAnswer;
                        opid3 = value1.AnswerID;

                        if ($scope.ans3 == true)
                            $scope.option3style = { 'border-color': 'green' };
                        else
                            $scope.option3style = { 'border-color': 'red' };
                        

                    }
                    if (answerCount == 4) {
                        $scope.ans4Option = value1.AnswerText;
                        $scope.ans4 = value1.CorrectAnswer;
                        opid4 = value1.AnswerID;

                        if ($scope.ans4 == true)
                            $scope.option4style = { 'border-color': 'green' };
                        else
                            $scope.option4style = { 'border-color': 'red' };

                    }

                  
                        

                ///         alert(value1.AnswerText + " " + value1.CorrectAnswer);


                    }

                });


        }


             //   var QId = value.QuesId;

             ////   alert(value.Question);


             //   data1.Question = value.Question;
             //   angular.forEach(data.Answers, function (value1, key) {


             //       if (value1.QuesId == QId) {


             //           data1.Answers = value1.AnswerText;

             //           // alert(value1.AnswerText + "" + value1.CorrectAnswer);


             //       }

             //   });


             ////   alert(data1.Question + " Ans:" + data1.Answers);


            });







    $scope.OnCheck = function () {

        if ($scope.ans1 == true)
            $scope.option1style = { 'border-color': 'green' };
        else
            $scope.option1style = { 'border-color': 'red' };


        if ($scope.ans2 == true)
            $scope.option2style = { 'border-color': 'green' };
        else
            $scope.option2style = { 'border-color': 'red' };


        if ($scope.ans3 == true)
            $scope.option3style = { 'border-color': 'green' };
        else
            $scope.option3style = { 'border-color': 'red' };

        if ($scope.ans4 == true)
            $scope.option4style = { 'border-color': 'green' };
        else
            $scope.option4style = { 'border-color': 'red' };
    }


    $scope.EditQues = function () {

        var answerType = $("#answerType").val();


        //  alert($scope.answerType + "");

        var op1_ans1 = $scope.ans1Option + "‰" + $scope.ans1 + "œ" + opid1;
        var op2_ans2 = $scope.ans2Option + "‰" + $scope.ans2 + "œ" + opid2;
        var op3_ans3 = $scope.ans3Option + "‰" + $scope.ans3 + "œ" + opid3;
        var op4_ans4 = $scope.ans4Option + "‰" + $scope.ans4 + "œ" + opid4;




        var data1 = {
            QuestionText: $scope.question,
            AnswerType: $scope.answerType,
            AnswerText: op1_ans1 + "‡" + op2_ans2 + "‡" + op3_ans3 + "‡" + op4_ans4,
            QuesId: QuesId,
            ExamId: ExamId
        };


        if ($scope.question != "" && $scope.answerType != undefined && ($scope.ans1Option != "" || $scope.ans2Option != "" || $scope.ans3Option != "" || $scope.ans4Option != "")) {

         //   alert("true" + opid1 + " " + opid2 + " " + opid3 + " " + opid4);


          
            var resource = location.protocol + "//" + location.host + "/api/Search/EditQues";
            $http.post(resource, data1).success(function (data, status) {

                if (data = "true") {

                    $scope.isError = false;
                    $scope.isSuccess = true;
                    $scope.successMessage = "Questions Successfully Updated...";
                  
                    
                    $uibModalInstance.close('saved');

                  
                }
            });

        } else {
            $scope.isSuccess = false;
            $scope.isError = true;
            $scope.errormessage = "All Fields Are Required And Atleast 1 Option Required..";
        }
    }



    $scope.ok = function () {
        $uibModalInstance.close('ok');
    };


    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };


});


courseApp.controller('addQuesModalInstanceCtrl', function ($scope, $http, $uibModalInstance, ExamId, ExamName) {
    $scope.modalTitle = "Add Question (" + ExamName + ")"  ;

    
   // $scope.getAllCourses = courses;
    $scope.ans1 = false;
    $scope.ans2 = false;
    $scope.ans3 = false;
    $scope.ans4 = false;

    $scope.ans1Option = "";
    $scope.ans2Option = "";
    $scope.ans3Option = "";
    $scope.ans4Option = "";
    $scope.question = "";


    $scope.OnCheck = function () {

        if ($scope.ans1 == true) 
            $scope.option1style = { 'border-color': 'green' };
        else
            $scope.option1style = { 'border-color': 'red' };


        if ($scope.ans2 == true)
            $scope.option2style = { 'border-color': 'green' };
        else
            $scope.option2style = { 'border-color': 'red' };


        if ($scope.ans3 == true)
            $scope.option3style = { 'border-color': 'green' };
        else
            $scope.option3style = { 'border-color': 'red' };

        if ($scope.ans4 == true)
            $scope.option4style = { 'border-color': 'green' };
        else
            $scope.option4style = { 'border-color': 'red' };
    }
  

    $scope.InsertQues = function () {

        var answerType = $("#answerType").val();

       
      //  alert($scope.answerType + "");

        var op1_ans1 = $scope.ans1Option + "‰" + $scope.ans1;
        var op2_ans2 = $scope.ans2Option + "‰" + $scope.ans2;
        var op3_ans3 = $scope.ans3Option + "‰" + $scope.ans3;
        var op4_ans4 = $scope.ans4Option + "‰" + $scope.ans4;

        


        var data1 = {
            QuestionText: $scope.question,
            AnswerType: $scope.answerType,
            AnswerText: op1_ans1 + "‡" + op2_ans2 + "‡" + op3_ans3 + "‡" + op4_ans4,
            ExamId: ExamId
        };

       
        //alert($scope.answerType);

        //if ($scope.answerType != undefined) {
        //    alert("sadf");
        //} else {
        //    alert("dsadfff");
        //}

         if ($scope.question != "" && $scope.answerType != undefined && ($scope.ans1Option != "" || $scope.ans2Option != "" || $scope.ans3Option != "" || $scope.ans4Option != "")) {

        var resource = location.protocol + "//" + location.host + "/api/Search/InsertQues";
        $http.post(resource, data1).success(function (data, status) {

            if (data = "true") {

                $scope.isError = false;
                $scope.isSuccess = true;
                $scope.successMessage = "Questions Successfully Added...";
                $scope.question = "";
                $scope.answerType = undefined;
                $scope.ans1Option = "";
                $scope.ans2Option = "";
                $scope.ans3Option = "";
                $scope.ans4Option = "";
              //$scope.ExamName = '';
              //$uibModalInstance.close('saved');

            }
        });

        } else {
            $scope.isSuccess = false;
            $scope.isError = true;
            $scope.errormessage = "All Fields Are Required And Atleast 1 Option Required..";
        }
    }



    $scope.ok = function () {
        $uibModalInstance.close('ok');
    };


    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };


});


courseApp.controller('addExamModalInstanceCtrl', function ($scope, $http, $uibModalInstance, courses) {
    $scope.modalTitle = "Add Exam";

  //  alert(courses[0].CourseName);
    $scope.getAllCourses = courses;

  




    $scope.InsertExam = function () {

        var C_id = $("#selectedCourses").val();
        var data1 = {
            ExamName: $scope.ExamName,
            CourseID: C_id
            
        };

        //if ($scope.CourseName != null && $scope.CourseDuration != null && $scope.CourseStartDate != null) {

        var resource = location.protocol + "//" + location.host + "/api/Search/InsertExam";
            $http.post(resource, data1).success(function (data, status) {

                if (data = "true") {
                 
                    $scope.isError = false;
                    $scope.isSuccess = true;
                    $scope.successMessage = "Exam Successfully Added...";
                    $scope.ExamName = '';
                    $uibModalInstance.close('saved');
                   
                }
        });

        //} else {

        //    $scope.isError = true;
        //    $scope.errormessage = "All Fields Are Required..";
        //}
    }

   

    $scope.ok = function () {
        $uibModalInstance.close('ok');
    };

  
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };


});

courseApp.controller('ModalInstanceCtrl', function ($scope, $uibModalInstance, videoSource, $log) {
    $log.info("ModalInstanceCtrl", videoSource);

    $scope.id = Math.floor((Math.random() * 100) + 1);
    $scope.videoSource = videoSource;

    $scope.ok = function () {
        $uibModalInstance.close('ok');
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});

courseApp.controller('videocontroller', function ($scope) {


    $scope.videoSources = [

        'http://www.sample-videos.com/video/mp4/720/big_buck_bunny_720p_1mb.mp4'
    ];

});


courseApp.filter("trustUrl", ['$sce', function ($sce) {
    return function (recordingUrl) {
        return $sce.trustAsResourceUrl(recordingUrl);
    };
}]);

