'use strict';

var courseApp = angular.module("courses", ['ui.directives', 'ui.filters', 'ngAnimate', 'ui.bootstrap']);

//courseApp.controller('Rolecontroller', function ($scope,$http) {


//    $scope.getUsersRole = function () {
//        var resource = location.protocol + "//" + location.host + "/api/Search/CheckUser";
//        var user = $('#UserName').val();

//        var data = {
//            UserName: user
//        };


//        $http.post(resource, data).success(function (data, status) {
//            if (data.Role == 1) {
//                $scope.isAdmin = true;
//            }



//        })
//            .error(function (data, status) {
//                // this isn't happening:
//            })



//    }

//    $scope.getUsersRole();
//});