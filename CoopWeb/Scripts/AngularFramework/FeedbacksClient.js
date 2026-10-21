var AppFeedBacks = angular.module("AppFeedBacks", ['angularUtils.directives.dirPagination']);
AppFeedBacks.controller("FeedBacks", function ($scope, $http) {
    // Save Sms Template 

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.FeedBacks = '';
    $scope.Channels = [];
    $scope.Targets = [];
    $scope.Users = [];
   var UseIdList = []; 
   
   
    $scope.SaveFeedBacks = function () {
        //$http.post("/FeedBacks/SaveFeedBacks", { "FeedBacksId": $scope.FeedBacks.FeedBacksId, "ProgrammeNameHi": $scope.FeedBacks.ProgrammeNameEn, "ProgrammeNameHi": $scope.FeedBacks.ProgrammeNameHi, "IsActive": $scope.FeedBacks.IsActive, "classEncID": $scope.FeedBacks.classEncID }).
        // success(function (data, status) {
       
        var tempMenuActions = '';
        
        angular.forEach($scope.Users, function (value) {
            if (value.Checked) {
                var trainObj = new FileDataWithRow();
                trainObj.UserId = value.UserId;
                UseIdList.push(trainObj);
                tempMenuActions += value.UserId + ',';
            }
        });

        
        tempMenuActions = tempMenuActions.substr(0, tempMenuActions.length - 1)
        $scope.FeedBacks.UserIds = tempMenuActions;

        var formData = new FormData();
        //for (var Y = 0; Y < UseIdList.length; Y++) {
        //    alert(UseIdList[Y].UserId);
        //    $scope.FeedBacks.lstFeedBacksUsers.push(UseIdList[Y].UserId);
        //}
        //formData.append("ProgrammeNameEn", $scope.FeedBacks.ProgrammeNameEn);
        //formData.append("ProgrammeNameHi", $scope.FeedBacks.ProgrammeNameHi);
        //formData.append("StartDate", $('#txtStartDate').val().split("/").reverse().join("-"));
        //formData.append("EndDate", $('#txtEndDate').val().split("/").reverse().join("-"));
        //formData.append("StartTime", $scope.FeedBacks.StartTime);
        //formData.append("EndTime", $scope.FeedBacks.EndTime);
        //formData.append("ChannelId", $scope.FeedBacks.ChannelId);
        //formData.append("TargetGroupId", $scope.FeedBacks.TargetGroupId);
        //formData.append("IsAccomodation", $scope.FeedBacks.IsAccomodation == true ? "1" : "0");
        //formData.append("Location", $scope.FeedBacks.Location);
        //formData.append("UserIds", $scope.FeedBacks.UserIds);
        
        
        //formData.append("UserIds", $scope.FeedBacks.UserIds);
        
       //FormData.append("IsActive", $scope.FeedBacks.IsActive == true ? "1" : "0");
        $http.post(SiteUrl + "/FeedBacks/SaveFeedBacks", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
        
            success(function (data, status) {
                var k = data;
                var message = '';

                if (k == 1) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                }
                else if (k == -2) {
                    message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                }
                alert(message);
                window.location = '/FeedBacks/FeedBacks';
                if ($scope.ParentId == '') {
                    $scope.ParentId = 0;
                }

                $scope.GetAllFeedBacksList();
                if (data.Status) {
                    $scope.FeedBacks = null;
                    $scope.ClearFeedBacksForm();
                    $scope.GetAllFeedBacksList();


                }
            }).
            error(function (data, status) {
                alert("Error while Menu insert");
            });


    };
    $scope.pageChangeHandler = function (num) {

    };
    // Update Template  
    $scope.update = function (item) {
        $scope.FeedBacks = item;
        $scope.FeedBacks.ProgrammeNameEn = item.ProgrammeNameEn;
        $scope.FeedBacks.ProgrammeNameHi = item.ProgrammeNameHi;
        $scope.FeedBacks.StartDate = item.StartDate;
        $scope.FeedBacks.EndDate = item.EndDate;
        $scope.FeedBacks.StartTime = item.StartTime;
        $scope.FeedBacks.EndTime = item.EndTime;
        $scope.FeedBacks.ChannelId = item.ChannelId;
        $scope.FeedBacks.TargetGroupId = item.TargetGroupId;
        $scope.FeedBacks.IsAccomodation = item.IsAccomodation == 1 ? true : false;
        $scope.FeedBacks.Location = item.Location;
        $scope.FeedBacks.IsActive = item.IsActive == 1 ? true : false;
        $scope.FeedBacks.classEncID = item.classEncID;

    };
    $scope.SetStatus = function (FeedBacksId, ActionId) {
        alert(ActionId);
        if (ActionId == false)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/FeedBacks/DeleteandstatusFeedBacks", { Id: FeedBacksId, Action: ActionId == true ? "1" : "0" }).
            success(function (data, status) {

                $scope.GetAllFeedBacksList();

            }).
            error(function (data, status) {
            });
    };
   
    // on page load
    $scope.OnPageLoad = function () {
       // $(".se-pre-con").fadeIn("slow");
          
      
        
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
        //$scope.ClearFeedBacksForm() = function () {
        //    $scope.FeedBacks = '';
        //};


        //Get list of sms template page list
        $scope.GetAllFeedBacksList();
        //$(".se-pre-con").fadeOut("slow");

        //$scope.GetAllMailData();

    };
    // Language Selector at page load

    $scope.GetAllFeedBacksList = function () {
        $http.get(SiteUrl + "/FeedBacks/FeedBacksList").
            success(function (data, status) {
                $scope.FeedBacksData = data;
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
            error(function (data1, status) {
                $scope.WorkFlowExecution = [];
            });
    }
         
    $scope.OnPageLoad();
    $scope.GetAllFeedBacksList();
    //$scope.ChannelList();
   // $scope.TargetList();

});

