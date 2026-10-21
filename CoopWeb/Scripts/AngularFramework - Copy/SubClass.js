
var MasterMod = angular.module('MastersModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("MasterCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Classes = [];
    $scope.divClass = false;
    $scope.ClassesData = [];
    $scope.Subdata = [];
    $scope.SubClass = [];

    // Code for Language label setting
    $scope.getTranslation = function () {
        $scope._language = _language;
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

        $scope.Classes = [];
        $http.get(SiteUrl +"/Master/GetMasterData/?MasterType=CLASSES&MasterCode=-1").
            success(function (data, status) {
                $scope.Classes = data;
            }).
            error(function (data, status) {
                $scope.Classes = '';
            });
    };

    $scope.pageChangeHandler = function (num) {
    };


    $scope.SaveSubClasses = function () {
        $http.post(SiteUrl +"/Master/SaveSubClasses", { "ClassId": $scope.SubClass.ClassId, "SubClassNameEn": $scope.SubClass.SubClassNameEn, "SubClassNameHi": $scope.SubClass.SubClassNameHi, "IsActive": $scope.SubClass.IsActive, "SubClassId": $scope.SubClass.SubClassId }).
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
                //ShowModelAlert(_language == 'en-us' ? 'Sport Association' : 'खेल एसोसिएशन', message);
                window.location = '/master/SubClasses';
                if ($scope.ParentId == '') {
                    $scope.ParentId = 0;
                }

                $scope.GetAllSubClassesList('');
                if (data.Status) {

                    $scope.SubClass = null;
                    $scope.ClearSubClassForm();
                    //  $scope.GetAllSubClassesList();


                }
            }).
            error(function (data, status) {
                alert("Error while subclass insert");
            });


    };


    // Language Selector
    $scope.getTranslation($scope);


    // on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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
        $(".se-pre-con").fadeOut("slow");

    };


    $scope.ClearSubClassForm = function () {
        $scope.SubClass.ClassId = '0';
        $scope.SubClass.SubClassNameEn = '';
        $scope.SubClass.SubClassNameHi = '';
        $scope.SubClass.IsActive = false;
        $scope.SubClass.SubClassId = '0';



    };

    $scope.update = function (item) {
        $scope.SubClass.ClassId = item.ClassId;
        $scope.SubClass.SubClassNameEn = item.SubClassNameEn;
        $scope.SubClass.SubClassNameHi = item.SubClassNameHi;

        $scope.SubClass.IsActive = item.IsActive == 1 ? true : false;
        $scope.SubClass.SubClassId = item.SubClassId;
    };



    $scope.SetStatus = function (SubClassId, ActionId) {
       alert(SubClassId)
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangeStatusSubClass", { Id: SubClassId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllSubClassesList('');

            }).
            error(function (data, status) {
            });
    };
    // Language Selector at page load

    $scope.GetAllSubClassesList = function (ClassId) {
        $http.get(SiteUrl +"/Master/SubClassList/?ClassId=" + ClassId).
            success(function (data, status) {

                $scope.ClassesData = data
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

            });
    }
    $scope.GetAllSubClassesListByClass = function () {
        $scope.GetAllSubClassesList($scope.SubClass.ClassId);
    };




    $scope.OnPageLoad();
    $scope.GetAllSubClassesList('');

});






