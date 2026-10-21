var ArrSelectTraningUser = new Array();

function clickTraningUserSelected() {
    this.UserId = '0';
}


var AppTrainning = angular.module("AppTrainngingAttendance", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("TrainngingAttendance", function ($scope, $http) {
    // Save Sms Template 

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.UserId = '0';
    $scope.StartDate = '';
    $scope.lstTrainningAttendanceUsers = [];
    var UseIdList = [];

    function FileDataWithRow() {
        this.UserId = '';
    }

    $scope.ClearTrainningForm = function () {
        $('input[type=checkbox][name=checkbox1]').each(function (j,item) {
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

    $scope.GetAllTrainningAttendanceUserList = function () {

        $http.post(SiteUrl +"/Trainning/TrainningAttendanceUserList", $scope.Trainning).
            success(function (data1, status) {
                if (data1 == '')
                { $scope.divuser = false; alert("No Record Found."); }
                else {
                    $scope.divuser = true;
                    $scope.AcceptUSerLst = data1;

                }

            }).
            error(function (data1, status) {
                $scope.divuser = false;

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
            $scope.lstTrainningAttendanceUsers.push(
                {
                    "TrainningId": $scope.Trainning.TrainningId,
                    "UserId": item.UserId,
                    "IsActive": item.IsActive == true ? 1 : 0

                    
                })
           
        });
        var formData = new FormData();
       

        formData.append("lstTrainningAttendanceUsers", JSON.stringify($scope.lstTrainningAttendanceUsers));
       
        $http.post(SiteUrl + "../Trainning/SaveTrainningAttendanceUser", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
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
            }).
            error(function (data, status) {
                alert("Error while Menu insert");
            });
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
            $http.get(SiteUrl +"/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl +"/Translations/Translation_HI.json").
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


$(document).ready(function () {
    $("#ckbCheckAll").click(function () {

        $(".checkBoxClass").prop('checked', this.checked);
    });
    $('#TblUser tr').each(function () {
        if ($(this).find('td:eq(1)').find('checkbox:checked')) {
            SocietyRegistrationData.push(
                {
                    "PostId": $(this).find('td:eq(1)').find('checkbox:checked'),
                    "UserType": "4",//Elected board Menbers
                    "AdministratorDesignationId": "-1",
                    "Photo": $(this).find('td:eq(8)').find('label').html()
                });
        }
    });
});