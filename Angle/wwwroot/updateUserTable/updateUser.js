(function () {
    'use strict';

    $(initDatatables);
    var $DetailUser;
    function initDatatables() {
        if (!$.fn.DataTable) return;
        $DetailUser = $('#userList').DataTable({
            "order": [[0, 'asc']],
            'info': true,
            "searching": false,
            'ajax': {
                'url': '/ProfileUpdate/Update',
                'type': 'POST',
                'contentType': "application/json; charset=utf-8",
                'datatype': 'json',
                "dataSrc": "",
                "data": function (d) {
                    var _UserRoleId = $('#UserRoleId').val();
                    var _EventTypeId = $('#EventTypeId').val();
                    var _EventCategoryId = $('#EventCategoryId').val();
                    var _CV = $('#CV').val();
                    var _Class = $('#Class').val();

                    if (_UserRoleId.lenght > 0) d["UserRoleId"] = _UserRoleId;

                    if (_EventTypeId.lenght > 0) d["EventTypeId"] = _EventTypeId;

                    if (_EventCategoryId.lenght > 0) d["EventCategoryId"] = _EventCategoryId;

                    if (_CV.lenght > 0) d["UploadCv"] = _CV;

                    if (_Class.lenght > 0) d["ClassId"] = _Class;
                    console.log(d);
                }
            },
            columns: [
                {
                    data: "FirstName", title: "Name", render: function (data, type, row) {
                        return [row["FirstName"]].join('');
                    }
                },

                {
                    data: "LastName", title: "Surname", render: function (data, type, row) {
                        return [row["LastName"]].join('');
                    }
                },

                {
                    data: "UserEmail", title: "Email", render: function (data, type, row) {
                        return [row["UserEmail"]].join('');
                    }
                },

                {
                    data: "UserId", title: "Id", render: function (data, type, row) {
                        return [row["UserId"]].join('');
                    }
                }
            ]
        });
    }
    $("#dataBindBtn").click(function () {
        $DetailUser.ajax.reload();
    });
})();