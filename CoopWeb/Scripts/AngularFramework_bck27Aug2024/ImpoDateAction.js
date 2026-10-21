var AppClasses = angular.module("ImpoDateActionMod", ['angularUtils.directives.dirPagination']);


AppClasses.controller("ImpoDateActionCnt", function ($scope, $http) {

    $scope.ImpoDateActionArr = '0';
    $scope.ImpoDateActionListData = [];
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        //$('#txt_DOB').datepicker({
        //    showOn: "both",
        //    changeMonth: true,
        //    buttonImage: "../images/calender.png",
        //    changeYear: true,
        //    dateFormat: "dd/mm/yy",
        //    maxDate: '0D',
        //    defaultDate: (new Date()).getDate() + '/' + ((new Date()).getMonth()) + '/' + ((new Date()).getFullYear() - 18).toString(),
        //    yearRange: "-100:-18"
        //});


        $(".se-pre-con").fadeOut("slow");
        // END : Post List for dropdown
        $scope.GetSocietyList();
    }

    $scope.GetImpoDateActionListData = function () {
        if ($("#DDLSocietyID").val() == null || $("#DDLSocietyID").val() == undefined || $("#DDLSocietyID").val() == 0) {
            alert("Please select society.");
        }
        else {
            $http.get(SiteUrl + "/Citizen/GetImpoDateActionList?SocietyIdENC="+ $("#DDLSocietyID").val()).
                success(function (data, status) {
                    alert(data);
                    $scope.ImpoDateActionListData = data;
                }).
                error(function (data, status) {
                    alert("Error while fetching list")

                });
        }
    }
    $scope.GetSocietyList = function () {
        //$http.post("/Citizen/GetAllSocietyList").
        $http.post("/Citizen/GetSocietyList", { "WhereClause": " " }).
            success(function (data, status) {

                $scope.SocietyList = data.DataList;
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                alert('Error while fetching society list data.');
            });
    };

    $scope.OnPageLoad();
});