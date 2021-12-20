


(function () {
    'use strict';

    // $(initDatatables);

    // var $DetailUser;

    //function initDatatables() {
    //    if (!$.fn.DataTable) return;
    //    $DetailUser = $('#userList').DataTable({
    //        "order": [[0, 'asc']],
    //        'info': true,
    //        "searching": true,

    //        'ajax': {
    //            'url': '/ListUser/FilterUserList',
    //            'type': 'GET',
    //            //'contentType': "json",
    //            'datatype': 'json',
    //            "dataSrc": "",

    //            //"data": {
    //            //    "UserRoleId": $('#UserRoleId').val(),
    //            //    "EventTypeId": $('#EventTypeId').val()
    //            //}


    //                //function (d) {
    //                //var _UserRoleId = $('#UserRoleId').val();
    //                //var _EventTypeId = $('#EventTypeId').val();
    //                //var _EventCategoryId = $('#EventCategoryId').val();
    //                //var _CV = $('#CV').val();
    //                //var _Class = $('#Class').val();
    //                //if (_UserRoleId.length > 0) d["UserRoleId"] = _UserRoleId;

    //             //    if (_EventTypeId.length > 0) d["EventTypeId"] = _EventTypeId;

    //             //    if (_EventCategoryId.length > 0) d["EventCategoryId"] = _EventCategoryId;

    //             //    if (_CV.length > 0)
    //             //        d["UploadCv"] = _CV;

    //             //    if (_Class.length > 0) d["ClassId"] = _Class;


    //            //}
    //            //            },

    //            columns: [
    //                {
    //                    data: "FirstName", title: "Name"

    //                },

    //                {
    //                    data: "LastName", title: "Surname"

    //                },

    //                {
    //                    data: "UserEmail", title: "Email"

    //                },

    //                {
    //                    data: "UserId", title: "Id"
    //                }

    //            ]
    //        }
    //        });

    //}
    //$("#dataBindBtn").click(function () {

    //   $DetailUser.ajax.reload();
    //});

    var UserRoleId = null;
    var EventTypeId = null;
    var EventCategoryId = null;
    var BeginningDate = null;
    var EndDate = null;
    var UploadCv = null;
    var ClassId = null;

    var result = {};


    function validateData() {
        result = {};
        if ($("#UserRoleId").val().length > 0) {
            result.UserRoleId = parseInt($("#UserRoleId").val(), 10);
        }
        if ($("#EventTypeId").val().length > 0) {
            result.EventTypeId = parseInt($("#EventTypeId").val(), 10);
        }
        if ($("#EventCategoryId").val().length > 0) {
            result.EventCategoryId = parseInt($("#EventCategoryId").val(), 10);
        }
        if ($("#BeginningDate").val().length > 0) {
                result.BeginningDate = document.getElementById("BeginningDate").value;
        }
        if ($("#EndDate").val().length > 0) {
                result.EndDate = document.getElementById("EndDate").value;
        }
        if ($("#UploadCv").val().length > 0) {
            result.UploadCv = Boolean($("#UploadCv").val());
        }
        if ($("#ClassId").val().length > 0) {
            result.ClassId = parseInt($("#ClassId").val(), 10);
        }
        console.log(result);
        return result;
    }

    

    $(document).ready(function () {
        filter();

    });
    function filter() {
        
        validateData();
        $.ajax({
            type: "POST",
            url: "/listUser/FilterUserList",
            data: result,
            dataType: "json",

            success: OnSuccess
            , failure: function (response) {
                alert(response.d);
            },
            error: function (response) {
                alert(response.d);
            }
        });
    }

    

    function OnSuccess(response) {
        console.log(response);
        $("#userList").DataTable({

            bDestroy: true,
            bLengthChange: true,
            lengthMenu: [[10, 10, -1], [10, 10, "All"]],
            bFilter: true,
            bSort: true,
            bPaginate: true,
            data: JSON.parse(response),
            columns: [
                {
                    data: "FirstName", title: "First Name"
                }
                ,
                {
                    data: "LastName", title: "Last Name"
                },
                {
                    data: "UserRoleName", title: "User Role"
                },
                {
                    data: "IncudemistLevelId", title: "Level"
                },
                {
                    data: "UniversityName", title: "University"
                },
                {
                    data: "DepartmantName", title: "Department"
                },
                {
                    data: "ClassName", title: "Class"
                }

            ]
        });
    }
    
    $("#dataBindBtn").click(function () {
      
        filter();
      
    });

})();

