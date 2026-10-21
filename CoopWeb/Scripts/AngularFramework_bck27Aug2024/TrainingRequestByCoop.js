var ArrSelectTraningUser = new Array();

function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("TrainingRequestByCoopMod", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("TrainingRequestByCoopCon", function ($scope, $http) {
    // Save Sms Template 
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
                url: SiteUrl + '/Trainning/SaveTrainningRequest',
                data: JSON.stringify({

                    "_Model": {
                        "TrainningId": $scope.Trainning.TrainningId,
                        "ChannelId": 0,
                        "IsAccomodation": $scope.Trainning.IsAccomodation,
                        "IsActive": true,
                        "Location": 0,
                        "ProgrammeNameEn": $scope.Trainning.ProgrammeNameEn,
                        "ProgrammeNameHi": $scope.Trainning.ProgrammeNameHi,
                        "TargetGroupId": $scope.Trainning.OrganisationId,
                        "StartDate": "",
                        "EndDate": "",
                        "StartTime": "",
                        "EndTime": "",
                        "HtmlContent": "",
                        "HtmlContentRequest": $('#HtmlContentRequest').val(),
                        "lstTrainningUsers": ArrSelectTraningUser,
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
                    $(".se-pre-con").fadeOut("slow");
                    window.location.reload();
                },
                error: function (error) {
                    $(".se-pre-con").fadeOut("slow");
                },

            })
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
        $scope.Trainning.TargetGroupId = item.TargetGroupId;
        $scope.Trainning.IsAccomodation = item.IsAccomodation == 1 ? true : false;
        $scope.Trainning.Location = item.Location;
        $scope.Trainning.IsActive = item.IsActive == 1 ? true : false;
        $scope.Trainning.HtmlContentRequest = item.HtmlContentRequest;
        //alert(item.HtmlContentRequest);
        $('#HtmlContentRequest').val(item.HtmlContentRequest);
        $scope.ShowTraningContent();

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


    // START : Back To Home Page
    $scope.Back = function () {
        window.location.reload();
    };
    //END : BAck To Home Page




    $scope.GetAllTrainningList = function () {
        $http.post(SiteUrl + "/Trainning/GetAllTrainningByUser").
            success(function (data, status) {
                $scope.TrainningData = data.filter(x => (x.IsRequestedTrainning === 'True' || x.IsRequestedTrainning === 'true' || x.IsRequestedTrainning === true));
                $scope.colsort = "";
                $scope.reverseSort = false;
                console.log($scope.TrainningData);
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
            error(function (data1, status) {
                $scope.WorkFlowExecution = [];
            });
    }
    $scope.SetCurrentOrg = function () {
        $http.post(SiteUrl + "/Trainning/GetCurrentOrganisation").
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.Trainning = [];
                    $scope.Trainning["OrganisationId"] = data[0].OrganisationId;
                }
            }).
            error(function (data1, status) {
            });
    };
    $scope.GetAllSubOrganisationList = function () {
        $http.get(SiteUrl + "/Master/OrganisationsList").
            success(function (data, status) {
                $scope.OrganisationData = data;
                if (data.length > 0) {
                    $scope.SetCurrentOrg();
                }
            }).
            error(function (data1, status) {

            });
    };

    $scope.ChkdtraningUsr = function (UserId) {
        var _SelectedChkdtraningUsr = new TraningUserSelected();
        _SelectedChkdtraningUsr.UserId = UserId;
        ArrSelectTraningUser.push(_SelectedChkdtraningUsr);
    }

    $scope.ShowTraningContent = function () {
        $scope.showtringdiv = true;
        $scope.DivtringGrid = false;
        $scope.asdf = true;

    }
    $scope.ClearTrainningForm = function () {
        $scope.Trainning.IsAccomodation = false;
        $scope.Trainning.ProgrammeNameEn = "";
        $scope.Trainning.ProgrammeNameHi = "";
        //$scope.Trainning.OrganisationId = '0';
        $('#HtmlContentRequest').val("");

    }
    $scope.AssignUsers = function (TrainningId) {
        window.location = '../trainning/DeptTrainningAcceptance?TrainningId=' + TrainningId;

    }
    //validateAddForm 
    $scope.TraningFormValidation = function () {
        var message = $scope._language === 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';

        if ($scope.Trainning.ProgrammeNameEn === undefined || $scope.Trainning.ProgrammeNameEn.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Programme Name (in Eng).' : '\n <br /> कृपया कार्यक्रम का नाम (अंग्रेजी में) दर्ज करें |';
        }
        if ($scope.Trainning.ProgrammeNameHi == undefined || $scope.Trainning.ProgrammeNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Programme Name (in Hindi)' : '\n <br /> कृपया कार्यक्रम का नाम (हिंदी में) दर्ज करें  |';
        }

        if ($scope.Trainning.OrganisationId === undefined || $scope.Trainning.OrganisationId === '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select Organisation' : '\n <br /> कृपया संस्थान का चयन करे |';
        }
        

        if ($('#HtmlContentRequest').val() === undefined || $('#HtmlContentRequest').val().length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please write content' : '\n <br /> कृपया विवरण दर्ज करे |';
        }

        if (message.length > 23) {

            ShowModelAlert($scope.translation.TrainingRequestByDepartment, message);
            return false;
        }
        return true;

    }
    //End
    $scope.GenPDF = function (TAId) {
        printDataJS(TAId);
    }
    $scope.OnPageLoad();
    $scope.GetAllTrainningList();
    $scope.GetAllSubOrganisationList();
    $scope.DivtringGrid = true;
});

