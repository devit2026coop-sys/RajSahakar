var LovModule = angular.module('WrHolidayMod', ['angularUtils.directives.dirPagination'])
LovModule.controller("WrHolidayCnt", function ($scope, $http) {

    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.GridProducts = true;
    $scope.divInsertProduct = false;


    $scope.SaveWrHoliday = function () {
        $(".se-pre-con").fadeIn("slow");

        var Msg = '';

        if (Msg.length > 0) {
            alert(Msg);
            return;
        }

        var formData = new FormData();
        formData.append("HolidayDate", $('#txtHolidayDate').val().split("/").reverse().join("-"));
        formData.append("EndTime", $('#txtendtime').val());
        $http.post(SiteUrl + "/WrHoliday/SaveWrHoliday", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            //  $http.post(SiteUrl + "/WrHoliday/SaveWrHoliday", $scope.WrHolidayModel).
            success(function (data, status) {
                var msg1 = "";
                if ($scope.WrHolidayModel.Id != data) {
                    var k = data;
                    $(".se-pre-con").fadeOut("slow");
                    alert(_language == 'en-us' ? 'Data Save Sucessfully\n' : 'डेटा सहेजा गया है\n');
                    $scope.ClearWrHolidayForm();
                    //alert(msg1);
                    $scope.GetWrHolidayList();
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    alert(_language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n');
                    $scope.ClearWrHolidayForm();
                    return;
                }

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while Lov insert");
            });
    };

    //START : Get Language on Page Load
    $scope.OnPageLoad = function () {
        $('#txtendtime').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('.SetCalender').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            minDate: '0'
        });
        $scope._language = _language;
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


        $scope.GetWrHolidayList();

        //var myJson = [{ "Id": "1", "Value": "Jitendra" }, { "Id": "2", "Value": "Rajeev" }, { "Id": "3", "Value": "Vaibhav" }, { "Id": "4", "Value": "Govind" }];
        //var keycount = Object.keys(myJson[0]);
        //var range = [];
        //for (var i = 0; i < keycount.length; i++) {
        //    range.push(keycount[i]);
        //}

        //$scope.myJson = myJson;
        //$scope.range = range;
    };
    //END : Get Language on Page Load

    //START : Show Div For Processing Unit Insert
    $scope.ShowDivProductInsert = function () {
        $scope.GridProducts = false;
        $scope.divInsertProduct = true;
    };

    // START : Get all the WrHoliday Items
    $scope.GetWrHolidayList = function () {
        $scope.GridProducts = true;
        $scope.divInsertProduct = false;
        $http.get(SiteUrl + "/WrHoliday/WrHolidayList").
            success(function (data, status) {
                $scope.WrHoildayData = data;
                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };

                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }

                    return '';
                };
            }).
            error(function (data, status) {
                alert("Error while fetching Lov list.");
                $scope.showLoader = false;
            });
    }
    //END : Get all the WrHoliday Items


    //START : Update data
    $scope.update = function (item) {   
        $scope.WrHolidayModel = [];
        $scope.GridProducts = false;
        $scope.divInsertProduct = true;
        $scope.WrHolidayModel.Id = item.Id;
        $scope.WrHolidayModel.HolidayDate = new Date(item.Date);
       // $('#txtHolidayDate').val(item.Date);
        $scope.WrHolidayModel.EndTime = item.EndTime;
    };
    //END : Update data

    $scope.BackToPage = function () {
        $scope.GridProducts = true;
        $scope.divInsertProduct = false;
    }

    // START : Clear form
    $scope.ClearWrHolidayForm = function () {
        $scope.WrHolidayModel = null;
        $('#txtendtime').val("");
    };

    // END : Clear form

    // START : Change Status
    $scope.SetStatus = function (_Id, _status) {
        if (_status == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        _status = (_status == 0 ? 1 : (_status == 1 ? 0 : _status));
        $http.post(SiteUrl + "/WrHoliday/ChangeStatus", { Id: _Id, Status: _status }).
            success(function (data, status) {

                if (parseInt(data, 10) > 0) {
                    if (_status != 2)
                        alert($scope.translation.StatusChange);

                    else
                        alert($scope.translation.Deleted);
                }
                $scope.GetWrHolidayList();
            }).
            error(function (data, status) {
            });
    };
    // END : Change Status


    // Page Load event list
    $scope.OnPageLoad();
    $scope.GetWrHolidayList();

});
