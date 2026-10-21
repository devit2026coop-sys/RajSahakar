
var MasterDesignationMod = angular.module('DesignationMastersModule', ['angularUtils.directives.dirPagination']);
MasterDesignationMod.controller("DesignationMasterCont", function ($scope, $http) {

    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];


    //save fun for designation()
    $scope.SaveDesignationForm = function () {
        $http.post(SiteUrl + "/Master/SaveDesignationFormdata", $scope.DesignationMaster).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");    
                var k = data;
                var message = '';

                if (k == 1) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    $(".se-pre-con").fadeOut("slow");
                }
                else if (k == -2) {
                    message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    $(".se-pre-con").fadeOut("slow");
                }
                alert(message);

                window.location = '/master/Designation';

                if ($scope.ParentId == '') {
                    $scope.ParentId = 0;
                }

                $scope.DesignationList();
                if (data.Status) {

                    $scope.DesignationMaster = null;
                    $scope.ClearDesignationtForm();
                    //$scope.GetAllSubClassesList();


                }
            }).
            error(function (data, status) {
                alert("Error while subclass insert");
            });


    };

    //End Save

    // Code for Language label setting
    $scope.getTranslation = function () {

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

    $scope.pageChangeHandler = function (num) {

    };
    // Get List of Designations 
    $scope.DesignationList = function () {
        $scope.Designations = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=designation&MasterCode=-3").
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.Designations = data;


                $scope.colsort = "";
                $scope.reverseSort = false;
                $(".se-pre-con").fadeOut("slow");
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
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.Designations = [];
            });
    };

    // Language Selector
    $scope.getTranslation($scope);

    // START : Change Designation Active/Inactive Status
    $scope.SetStatus = function (DESIGNATIONCODE, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangedeleteStatusDesignation", { Id: DESIGNATIONCODE, Action: ActionId }).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.DesignationList();
                $(".se-pre-con").fadeOut("slow");

            }).
            error(function (data, status) {
            });
    };
    // END : Change Designation Active/Inactive Status
    //start update 
    $scope.update = function (item) {
        $scope.DesignationMaster.DEPTCODE = item.DEPTCODE;
        $scope.DesignationMaster.DESIGNATIONDESCENGLISH = item.DESIGNATIONDESCENGLISH;
        $scope.DesignationMaster.DESIGNATIONDESCHINDI = item.DESIGNATIONDESCHINDI;
        $scope.DesignationMaster.IsActive = item.IsActive == 1 ? true : false;
        $scope.DesignationMaster.DESIGNATIONCODE = item.DESIGNATIONCODE;
    };
    //End update
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
        $(".se-pre-con").fadeOut("slow");

        $scope.ClearDesignationtForm = function () {

            $scope.DesignationMaster.DEPTCODE = '0';
            $scope.DesignationMaster.DESIGNATIONDESCENGLISH = '';
            $scope.DesignationMaster.DESIGNATIONDESCHINDI = '';
            $scope.DesignationMaster.IsActive = false;
            $scope.DesignationMaster.DESIGNATIONCODE = '0';
        };

        $scope.Department = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Department&MasterCode=-3").

            success(function (data, status) {

                $scope.Department = data;

            }).
            error(function (data, status) {
                $scope.Department = [];
            });

    };
    // Get list of all designations

    $scope.OnPageLoad();
    $scope.DesignationList();
});

