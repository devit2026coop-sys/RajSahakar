var ArrSelectTraningUser = new Array();


function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("AppTrainnging", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("Trainnging", function ($scope, $http) {
    // Save Sms Template 
    //$scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.Trainning = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
    $scope.UserId = '0';
    var UseIdList = [];

    function FileDataWithRow() {
        this.UserId = '';
    }
    $scope.SaveTrainning = function () {
        $(".se-pre-con").fadeIn("slow");
        if ($scope.TraningFormValidation()) {
            $.ajax({
                url: SiteUrl + '/Trainning/SaveTrainning',
                data: JSON.stringify({

                    "_Model": {
                        "TrainningId": $scope.Trainning.TrainningId,
                        "ChannelId": $scope.Trainning.ChannelId,
                        "IsAccomodation": $scope.Trainning.IsAccomodation == undefined ? false : $scope.Trainning.IsAccomodation,
                        "IsActive": $scope.Trainning.IsActive === undefined ? false : $scope.Trainning.IsActive,
                        "Location": $scope.Trainning.Location,
                        "ProgrammeNameEn": $scope.Trainning.ProgrammeNameEn,
                        "ProgrammeNameHi": $scope.Trainning.ProgrammeNameHi,
                        "TargetGroupId": $scope.Trainning.OrganisationId,
                        "TargetGroupName": $("#ddlOrganisation option:selected").text(),
                        "StartDate": $('#txtStartDate').val().split("/").reverse().join("-"),
                        "EndDate": $('#txtEndDate').val().split("/").reverse().join("-"),
                        "StartTime": $('#txtStartTime').val(),
                        "EndTime": $('#txtEndTime').val(),
                        "HtmlContent": $('#HtmlContent').val(),
                        "lstTrainningUsers": ArrSelectTraningUser
                    }
                }),
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                beforeSend: function () {
                },
                success: function (data) {

                    
                    if (data > 0) {
                        var msg = _language === 'en-us' ? 'Record Saved Successfully.' : 'रिकॉर्ड सफलतापूर्वक सहेजा गया।';
                        alert(msg);
                    }
                    else {
                        var msg2 = _language === 'en-us' ? 'Error while data Insert.' : 'रिकॉर्ड सहजते समय त्रुटि ।';
                        alert(msg2);
                    }
                    window.location.reload();
                    $(".se-pre-con").fadeOut("slow");
                },
                error: function (error) {
                    $(".se-pre-con").fadeOut("slow");

                }

            });
        }
        $(".se-pre-con").fadeOut("slow");
    };


    $scope.pageChangeHandler = function (num) {

    };
    // Update Template  
    $scope.update = function (item) {

        $scope.showtringdiv = true;
        $scope.Trainning = item;
        $scope.Trainning.TrainningId = item.TrainningId;
        $scope.Trainning.ProgrammeNameEn = item.ProgrammeNameEn;
        $scope.Trainning.ProgrammeNameHi = item.ProgrammeNameHi;
        $scope.Trainning.StartDate = item.StartDate;
        $scope.Trainning.EndDate = item.EndDate;
        $scope.Trainning.StartTime = item.StartTime;
        $scope.Trainning.EndTime = item.EndTime;
        $scope.Trainning.ChannelId = item.ChannelId;
        $scope.Trainning.OrganisationId = item.TargetGroupId;
       
        $scope.Trainning.TargetGroupId = item.TargetGroupId;
        $scope.Trainning.IsAccomodation = item.IsAccomodation == 1 ? true : false;
        $scope.Trainning.Location = item.Location;
        $scope.Trainning.IsActive = item.IsActive == 1 ? true : false;
        $scope.Trainning.classEncID = item.classEncID;
        $scope.Trainning.HtmlContent = item.HtmlContent;

        
        //alert(item.HtmlContent);
        $('#HtmlContent').val(item.HtmlContent);
        $('#HtmlContent').parent().children().next().next().contents().find('body').html(item.HtmlContent);
        $scope.ShowTraningContent();
        $('#ddlOrganisation').val('4');
    };

    $scope.ClearTrainningForm = function () {
        $scope.Trainning.ProgrammeNameEn = "";
        $scope.Trainning.ProgrammeNameHi = "";
        $('#txtStartDate').val('');
        $('#txtEndDate').val('');
        $('#txtStartTime').val('');
        $('#txtEndTime').val('');
        $scope.Trainning.ChannelId = '0';
        $scope.Trainning.TargetGroupId = '0';
        $scope.Trainning.IsAccomodation = false;
        $scope.Trainning.Location = 0;
        $scope.Trainning.IsActive = false;
        $scope.Trainning.OrganisationId = '0';
        $('#HtmlContent').val("");

    };
    $scope.SetStatus = function (TrainningId, ActionId) {
        $http.post(SiteUrl + "/Trainning/DeleteandstatusTrainning", { Id: TrainningId, Action: ActionId == true ? "1" : "0" }).
            success(function (data, status) {

                $scope.GetAllTrainningList();

            }).
            error(function (data, status) {
            });
    };

    // on page load
    $scope.OnPageLoad = function () {
        var date = new Date();
        var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());
        $('#txtStartTime').prop('autocomplete', 'off').timepicker();
        $('#txtEndTime').prop('autocomplete', 'off').timepicker();
        // $(".se-pre-con").fadeIn("slow");
        $('#txtStartTime').timepicker({
            showOn: "both",
            buttonImage: "../../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtEndTime').timepicker({
            showOn: "both",
            buttonImage: "../../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtStartDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            minDate: today
        });
        $('#txtEndDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            minDate: today
        });
       
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $scope._language = 'en-us';
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $scope._language = 'hi-in';
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
    };
    // Language Selector at page load

    $scope.GetAllTrainningList = function () {
        $http.post(SiteUrl + "/Trainning/TrainningList", $scope.Trainning).
            success(function (data, status) {
                $scope.TrainningData = data;
                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort === column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                    }
                    return '';
                };
            }).
            error(function (data1, status) {
                $scope.WorkFlowExecution = [];
            });
    }
    // Get List of Channel 
    $scope.ChannelList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Channels = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Channel&MasterCode=-1").
            success(function (data, status) {
                //var _Result = JSON.parse(data);
                $scope.Channels = data;
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
                $scope.Channels = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.GetAllSubOrganisationList = function () {
        $http.get(SiteUrl + "/Master/OrganisationsList").
            success(function (data, status) {
                $scope.OrganisationData = data;
                if (data.length > 0) {
                    // Code Not Written
                }
            }).
            error(function (data1, status) {

            });
    };
    // Get List of Channel 
    $scope.UserList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Users = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=USERS&MasterCode=-1").
            success(function (data, status) {
                //var _Result = JSON.parse(data);
                $scope.Users = data;
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
                $scope.Users = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.ChkdtraningUsr = function (UserId) {
        var _SelectedChkdtraningUsr = new TraningUserSelected();
        _SelectedChkdtraningUsr.UserId = UserId;
        ArrSelectTraningUser.push(_SelectedChkdtraningUsr);
    };

    $scope.ShowTraningContent = function () {
        $scope.showtringdiv = true;
        $scope.DivtringGrid = false;
    };
    //validateAddForm 
    $scope.TraningFormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';

        if ($scope.Trainning.ProgrammeNameEn === undefined || $scope.Trainning.ProgrammeNameEn.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Programme Name (in Eng).' : '\n <br /> कृपया कार्यक्रम का नाम (अंग्रेजी में) दर्ज करें |';
        }
        if ($scope.Trainning.ProgrammeNameHi === undefined || $scope.Trainning.ProgrammeNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Programme Name (in Hindi)' : '\n <br /> कृपया कार्यक्रम का नाम (हिंदी में) दर्ज करें  |';
        }

        if ($('#txtStartDate').val() === undefined || $('#txtStartDate').val().length<1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Start Date' : '\n <br /> कृपया आरंभ करने की तिथि दर्ज करें  |';
        }
        if ($('#txtEndDate').val() === undefined || $('#txtEndDate').val().length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter End date' : '\n <br /> कृपया अंतिम तिथि दर्ज करें  |';
        }
        if ($('#txtStartTime').val() === undefined || $('#txtStartTime').val().length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Start Time' : '\n <br /> कृपया स्टार्ट टाइम दर्ज करें  |';
        }
        if ($('#txtEndTime').val() === undefined || $('#txtEndTime').val().length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter End Time' : '\n <br /> कृपया एन्ड टाइम दर्ज करें  |';
        }

        if ($scope.Trainning.ChannelId === undefined || $scope.Trainning.ChannelId === '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select Channel' : '\n <br /> कृपया चैनल का चयन करे |';
        }
        if ($scope.Trainning.OrganisationId === undefined || $scope.Trainning.OrganisationId === '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select Organisation' : '\n <br /> कृपया संस्थान का चयन करे |';
        }
        
        if ($scope.Trainning.Location === undefined) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select Location' : '\n <br /> कृपया स्थान का चयन करें |';
        }
       

        if (message.length > 23) {

            ShowModelAlert("Training", message);
            return false;
        }
        return true;

    };
    //End
    $scope.OnPageLoad();
    $scope.GetAllTrainningList();
    $scope.ChannelList();
    $scope.GetAllSubOrganisationList();
    $scope.DivtringGrid = true;
});

