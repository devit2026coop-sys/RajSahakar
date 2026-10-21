var ArrSelectTraningUser = new Array();
function clickTraningUserSelected() {
    this.UserId = '0';
}


var AppTrainning = angular.module("AppTrainngingAttendance", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("TrainngingAttendance", function ($scope, $rootScope, $http, $timeout) {
    // Save Sms Template 

    $scope.currentPage = 1;
    $scope.pageSize = 15;
    $scope.UserId = '0';
    $scope.StartDate = '';
    $scope.lstTrainningAttendanceUsers = [];
    var UseIdList = [];
    $scope.dataPayload = {};
    $scope.esignData = {};


    $scope.ClearTrainningForm = function () {
        $('input[type=checkbox][name=checkbox1]').each(function (j, item) {
            $(item).prop('checked', false);
        });
    };


    $scope.GetTrainningListByDate = function () {
        $scope.TrainningData = null;
        $http.post(SiteUrl + "/Trainning/GetTrainningListByDate").
            success(function (data, status) {
                $scope.TrainningData = data;
                $scope.TraningOrgdata = data;
                $scope.TrainningData = data[0];

            }).
            error(function (data, status) {
                $scope.TrainningData = [];
            });

    }

    $scope.GetAllTrainningAttendanceUserListWorking = function () {

        $http.post(SiteUrl + "/Trainning/TrainningAttendanceUserList", $scope.Trainning).
            success(function (data1, status) {
                if (data1 == '') { $scope.divuser = false; alert("No Record Found."); }
                else {
                    $scope.divuser = true;
                    $scope.AcceptUSerLst = data1;
                }

            }).
            error(function (data1, status) {
                $scope.divuser = false;

            });
    };

    $scope.GetAllTrainningAttendanceUserList = function () {
        $http.post(SiteUrl + "/Trainning/TrainningAttendanceUserList", $scope.Trainning).success(function (data1, status) {
            if (data1 == '') {
                $scope.divuser = false;
                alert("No Record Found.");
            }
            else {
                $scope.divuser = true;
                $scope.AcceptUSerLst = data1;
                $scope.currentPage = 1;
                $scope.totalPages = Math.ceil($scope.AcceptUSerLst.length / $scope.pageSize);
                $scope.pages = [];
                for (var i = 1; i <= $scope.totalPages; i++) {
                    $scope.pages.push(i);
                }
            }

            if ($scope.Trainning.TrainningId > 0) {
                $scope.GetDataByTraining();
            }

        }).error(function (data1, status) {

            $scope.divuser = false;
            $scope.AcceptUSerLst = [];
            $scope.pages = [];
            $scope.totalPages = 0;
        });
    };


    //$scope.ClickToChkUsers = function (UserId) {
    //    var _SelectedToChkUsers = new clickTraningUserSelected();
    //    _SelectedToChkUsers.UserId = UserId;
    //    ArrSelectTraningUser.push(_SelectedToChkUsers);
    //}

    $scope.SaveTrainningUsersList = function () {
        var userCount = 0;
        $('input[type=checkbox][name=checkbox1]').each(function (j, item) {
            if ($(item).prop('checked') == true) {
                userCount += 1;
            }
        });

        if (userCount < 1) {
            var msg = _language === 'in-us' ? 'Please select user from list.' : 'कृपया सूची से उपयोगकर्ता का चयन करें';
            ShowModelAlert($scope.translation.TrainngingAttendance, msg, function () { });
            return;
        };
        angular.forEach($scope.AcceptUSerLst, function (item) {
            $scope.lstTrainningAttendanceUsers.push({
                "TrainningId": $scope.Trainning.TrainningId,
                "UserId": item.UserId,
                "MappingId": item.UserRoleDeptMappingId,
                "IsActive": item.IsActive == true ? 1 : 0,
                "TrainningDate": $scope.toDateFormat($scope.Trainning.TrainningDate),
                "IsApproveTraining": $scope.Trainning.IsApproveTraingDate
            })
        });
        var formData = new FormData();
        formData.append("lstTrainningAttendanceUsers", JSON.stringify($scope.lstTrainningAttendanceUsers));
        formData.append("TrainningId", $scope.Trainning.TrainningId);
        $http.post(SiteUrl + "../Trainning/SaveTrainningAttendanceUser", formData, {
            withCredentials: true,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).success(function (data, status) {
            var k = data;
            $scope.lstTrainningAttendanceUsers = [];
            var message = '';
            if (k == 1) {
                alert("Save Sucessfully");
                window.location = '/Trainning/TrainingAttendance';
            }
            else {
                alert("User Already Alloted");
                //window.location = '/Trainning/Trainning';
                //window.location = '/Trainning/DeptTrainningAcceptance';
            }
        }).error(function (data, status) {
            alert("Error while Menu insert");
        });
    };

    $scope.SaveTrainningApproveAttendance = function () {
        var model = {
            TrainningId: $scope.Trainning.TrainningId,
            IsApproveTraining: $scope.Trainning.IsApproveTraingDate,
            TrainningDate: $scope.Trainning.TrainningDate,
            lstTrainningAttendanceUsers: []
        };

        $http.post(SiteUrl + "/Trainning/InsertTraningAttendanceUserApproved", model).then(function (response) {
            if (response.data.Status) {
                alert(response.data.Message);
                location.reload();
            } else {
                alert(response.data.Message);
            }
        }, function () {
            alert("Something went wrong.");
        });
    };


    $scope.ApproveTrainingDateChanged = function () {

    };
    $scope.previousPage = function () {
        if ($scope.currentPage > 1) {
            $scope.currentPage--;
        }
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.totalPages) {
            $scope.currentPage++;
        }
    };

    $scope.goToPage = function (page) {
        $scope.currentPage = page;
    };




    $scope.GetDataByTrainingWorking = function () {
        $scope.startDate = '';
        $scope.endDate = '';
        $scope.trainingData = [];
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/Trainning/GetTrainingDataByTrainingId", {
            "TrainningId": $scope.Trainning.TrainningId
        }).then(function (response) {
            $(".se-pre-con").fadeOut("slow");
            if (response.status == 200 && response.statusText == "OK") {
                if (response.data.Data != null) {
                    $scope.trainingData = JSON.parse(response.data.Data.data);
                    $scope.startDate = $scope.trainingData[0].StartDate;
                    $scope.endDate = $scope.trainingData[0].StartDate;
                    $scope.Trainning.TrainningDate = $scope.parseDate($scope.startDate);
                }
            }
        }).catch(function (error) {
            console.error(error);
            alert("Error");
            throw error;
        })
    };
    $scope.GetDataByTraining = function () {
        $scope.startDate = "";
        $scope.endDate = "";
        $scope.isDateDisabled = true;
        $scope.trainingData = [];

        return $http.post(SiteUrl + "/Trainning/GetTrainingDataByTrainingId", {
            TrainningId: $scope.Trainning.TrainningId
        }).then(function (response) {
            if (response.status == 200 && response.data.Data != null) {
                $scope.trainingData = JSON.parse(response.data.Data.data);
                if ($scope.trainingData.length > 0 &&
                    $scope.trainingData[0].StartDate != null &&
                    $scope.trainingData[0].StartDate != "") {
                    //$scope.startDate = $scope.trainingData[0].StartDate;
                    //$scope.endDate = $scope.trainingData[0].StartDate;
                    //$scope.Trainning.TrainningDate = $scope.parseDate($scope.trainingData[0].StartDate);
                    //$scope.isDateDisabled = false;
                    var start = $scope.trainingData[0].StartDate; 
                    $scope.startDate = $scope.toInputDate(start);
                    $scope.endDate = $scope.toInputDate(start);   
                    $scope.Trainning.TrainningDate = $scope.parseDate(start);
                    $scope.isDateDisabled = false;
                }
                else {

                    $scope.startDate = "";
                    $scope.endDate = "";
                    $scope.Trainning.TrainningDate = null;
                    $scope.isDateDisabled = true;

                    alert("Attendance has already been marked for all training dates.");
                }
            }
        });
    };
    $scope.convertToInputDate = function (dateStr) {
        var parts = dateStr.split('-');
        return parts[2] + '-' + parts[1] + '-' + parts[0];
    }

    $scope.toDateFormat = function (value) {
        if (!value) return "";

        if (value instanceof Date) {
            var yyyy = value.getFullYear();
            var MM = ("0" + (value.getMonth() + 1)).slice(-2);
            var dd = ("0" + value.getDate()).slice(-2);
            return yyyy + "-" + MM + "-" + dd;
        }

        var parts = value.split("-"); // dd-MM-yyyy
        if (parts.length === 3) {
            return parts[2] + "-" + parts[1] + "-" + parts[0];
        }

        return "";
    };

    $scope.parseDate = function (dateStr) {
        if (!dateStr)
            return null;

        var parts = dateStr.split('-');
        return new Date(parts[2], parts[1] - 1, parts[0]);
    };

    $scope.toInputDate = function (dateStr) {
        if (!dateStr) return "";

        var parts = dateStr.split("-");
        return parts[2] + "-" + parts[1] + "-" + parts[0];
    };

    $scope.parseDate = function (dateStr) {
        if (!dateStr) return null;
        var parts = dateStr.split("-");
        return new Date(parts[2], parts[1] - 1, parts[0]);
    };
    $scope.OnPageLoad = function () {
        var date = new Date();
        var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());

        $('#txtStartDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: "0D"
        });
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }

    };

    $scope.OnPageLoad();
    $scope.GetTrainningListByDate();
});
function FileDataWithRow() {
    this.UserId = '';
}

$(document).ready(function () {
    var SocietyRegistrationData = [];
    $("#ckbCheckAll").click(function () {

        $(".checkBoxClass").prop('checked', this.checked);
    });
    $('#TblUser tr').each(function () {
        if ($(this).find('td:eq(1)').find('checkbox:checked')) {
            SocietyRegistrationData.push({
                "PostId": $(this).find('td:eq(1)').find('checkbox:checked'),
                "UserType": "4",//Elected board Menbers
                "AdministratorDesignationId": "-1",
                "Photo": $(this).find('td:eq(8)').find('label').html()
            });
        }
    });
});