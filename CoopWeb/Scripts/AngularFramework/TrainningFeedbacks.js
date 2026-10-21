var AppFeedBacks = angular.module("AppTrainningFeedBacks", ['angularUtils.directives.dirPagination']);
AppFeedBacks.controller("TrainningFeedBacks", function ($scope, $http) {
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

    $scope.pageChangeHandler = function (num) {

    };


    $scope.SaveTrainningFeedback = function () {
        if ($scope.ValidationFeedbackDetails()) {
            var formData = new FormData();
            formData.append("Feedback", $scope.FeedBacks.FeedBack);
            formData.append("TrainningId", $scope.FeedBacks.TrainningId);
            $http.post(SiteUrl + "/Trainning/SaveTrainningFeedBack", formData,
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
                    $scope.BackToPage();
                    if (data.Status) {
                    }
                }).
                error(function (data, status) {
                    alert("Error while Menu insert");
                });
        }
    };


    // on page load
    $scope.OnPageLoad = function () {
        // $(".se-pre-con").fadeIn("slow");



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
        //$scope.ClearFeedBacksForm() = function () {
        //    $scope.FeedBacks = '';
        //};


        //Get list of sms template page list

        //$(".se-pre-con").fadeOut("slow");

        //$scope.GetAllMailData();

    };
    // Language Selector at page load

    $scope.BackToPage = function () {
        window.location = "../Trainning/TrainningFeedBacks";
    }

    $scope.ValidationFeedbackDetails = function () {
        var message = "";
        if ($scope.FeedBacks.FeedBack == '' || $scope.FeedBacks.FeedBack == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Feedback' : '\n कृपया प्रतिक्रिया दें |';
            message += '<br />';
        }
        if ($scope.FeedBacks.TrainningId == '0' || $scope.FeedBacks.TrainningId == 'undefined') {
            message += $scope._language == 'en-us' ? '\nPlease Select Trainning' : '\nकृपया प्रशिक्षण का चयन करें |';
            message += '<br />';
        }
        if (message.length > 10) {
            ShowModelAlert("Feedback Details", message);
            return false;
        }
        return true;
    }


    $scope.GetAllFeedBacksList = function () {
        $http.get(SiteUrl +"/Trainning/FeedBacksList").
            success(function (data, status) {
                $scope.FeedBacksData = data;

            }).
            error(function (data1, status) {
                $scope.FeedBacksData = [];
            });
    }

    $scope.GetUserTrainningList = function () {
        $http.post(SiteUrl +"/Trainning/GetUserTrainningList").
            success(function (data, status) {
                $scope.UserTrainningData = data;
            }).
            error(function (data1, status) {
                $scope.UserTrainningData = [];
            });
    }

    $scope.OnPageLoad();
    $scope.GetAllFeedBacksList();
    $scope.GetUserTrainningList();

});