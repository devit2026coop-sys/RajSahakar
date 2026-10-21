var AppClasses = angular.module("AppClassesFormat", ['angularUtils.directives.dirPagination']);
AppClasses.controller("Classes", function ($scope, $http) {
    // Save Sms Template 

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.Classes = '';

    $scope.SaveClasses = function () {
        $http.post(SiteUrl + "/Master/SaveClasses", { "ClassId": $scope.Classes.ClassId, "ClassNameEn": $scope.Classes.ClassNameEn, "ClassNameHi": $scope.Classes.ClassNameHi, "IsActive": $scope.Classes.IsActive, "classEncID": $scope.Classes.classEncID }).
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
                window.location = '/master/classes';
                if ($scope.ParentId == '') {
                    $scope.ParentId = 0;
                }

                $scope.GetAllClassesList();
                if (data.Status) {
                    $scope.Classes = null;
                    $scope.ClearClassForm();
                    $scope.GetAllClassesList();


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
        $scope.Classes = item;
        $scope.Classes.ClassNameEn = item.ClassNameEn;
        $scope.Classes.ClassNameHi = item.ClassNameHi;

        $scope.Classes.IsActive = item.IsActive == 1 ? true : false;
        $scope.Classes.classEncID = item.classEncID;

    };
    $scope.SetStatus = function (ClassId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/DeleteandstatusClass", { Id: ClassId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllClassesList();

            }).
            error(function (data, status) {
            });
    };

    // on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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
        $scope.ClearClassForm = function () {
            $scope.Classes = '';
        };
        //Get list of sms template page list
        $scope.GetAllClassesList();
        $(".se-pre-con").fadeOut("slow");        
    };
    // Language Selector at page load

    $scope.GetAllClassesList = function () {
        $http.get(SiteUrl + "/Master/ClassList").
            success(function (data, status) {
                $scope.ClassesData = data;
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
    $scope.GetAllClassesList();

});

