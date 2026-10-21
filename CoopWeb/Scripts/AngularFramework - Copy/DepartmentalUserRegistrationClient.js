
var DeptUserRegistrationModule = angular.module('DepartmenalUserRegistrationMod', ['angularUtils.directives.dirPagination'])

DeptUserRegistrationModule.controller("DepartmenalUserRegistrationCnt", function ($scope, $http) {


    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Tehsil = [];
    $scope.City = [];
    $scope.Ward = [];
    $scope.Titles = [];
    $scope.UrbanDiv = true;
    $scope.RuralDiv = false;


    // START : Get List of all users
    $scope.UserList = function () {
        $http.post(SiteUrl+"/Login/ListUsers").
            success(function (data, status) {
                $scope.Users = data;//data;

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
                alert("Error while user insert");
            });
    };
    // END : Get List of all users

    // START : Save/Edit User
    $scope.SaveDepartmenatalUser = function () {
        $http.post(SiteUrl+"/Login/SaveUser", $scope.DeptUserRegistration).
            success(function (data, status) {
                if (data.Status) {
                    $scope.UserRegistration = null;
                    $scope.UserList();
                }
            }).
            error(function (data, status) {
                alert("Error while user insert");
            });
    };
    // END : Save/Edit User

    // START : Change Status for User / delete user
    $scope.SetStatus = function (UserId, ActionId) {

        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl+"/User/ChangeStatus", { Id: UserId, Action: ActionId }).
            success(function (data, status) {
                $scope.UserList();
            }).
            error(function (data, status) {
            });
    };
    // END : Change Status for User / delete user

    // START : Get Data for Update
    $scope.Update = function (UserId) {
        $http.post(SiteUrl+"/User/ListUsers", { WhereClause: ' AND [UserId]=' + UserId }).
            success(function (data, status) {
                $scope.UserRegistration = data[0];
            }).
            error(function (data, status) {
            });
    };
    // END : Get Data for Update

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.UserRegistration = null;
    };
    //END : Delete User

    // START : Code for Language label setting
    $scope.getTranslation = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl+"/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl+"/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
    };
    // END : Code for Language label setting

    $scope.pageChangeHandler = function (num) {
      
    };

    // START : Get List of Divisions and Title
    $scope.OnPageLoad = function () {
        $scope.Divisions = [{}];
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=division&MasterCode=-1").
            success(function (data, status) {
                $scope.Divisions = data;
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });

        $scope.Titles = [{}];
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=Title&MasterCode=-1").
            success(function (data, status) {
                $scope.Titles = data;
            }).
            error(function (data, status) {
                $scope.Titles = [];
            });

        $scope.UserList();
    };
    // END : Get List of Divisions and Title

    // START : Get List of Districts
    $scope.DistrictList = function () {
        $scope.Districts = [{}];

        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=district&MasterCode=" + $scope.DeptUserRegistration.DivisionId).
            success(function (data, status) {
              
                $scope.Districts = data;
            }).
            error(function (data, status) {
                $scope.Districts = [{}];
            });
    };
    // END : Get List of Districts

    // START : Set Rural/ Urban div show/hide
    $scope.RuralUrbanBlock = function () {
        var SelectedVal = $scope.DeptUserRegistration.IsRuralUrban;

        if (SelectedVal == 'true') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;

            $scope.Tehsil = [];
            $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=tehsil&MasterCode=" + $scope.DeptUserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.Tehsil = data;
                }).
                error(function (data, status) {
                    $scope.Tehsil = [];
                });

            $scope.City = [];
            $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=city&MasterCode=" + $scope.DeptUserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.City = data;
                }).
                error(function (data, status) {
                    $scope.City = [];
                });
        }
        else if (SelectedVal == 'false') {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;

            $scope.Blocks = [];

            $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.DeptUserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.Blocks = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }
        else {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }
    };
    // END : Set Rural/ Urban div show/hide

    // START : Get Ward List
    $scope.WardList = function () {
        $scope.Ward = [];

        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=ward&MasterCode=" + $scope.DeptUserRegistration.CityId).
            success(function (data, status) {
                $scope.Ward = data;
            }).
            error(function (data, status) {
                $scope.Ward = [];
            });
    };
    // END : Get Ward List

    // START : Get List of GramPanchayat
    $scope.GramPanchayatList = function () {
        $scope.GramPanchayats = [];
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.DeptUserRegistration.BlockId).
            success(function (data, status) {
                $scope.GramPanchayats = data;
            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];
            });
    };
    // END : Get List of GramPanchayat

    // START : Get List of Villages
    $scope.VillageList = function () {
        $scope.Villages = [];

        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.DeptUserRegistration.GPId).
            success(function (data, status) {
                $scope.Villages = data;
            }).
            error(function (data, status) {
                $scope.Villages = [];
            });
    };

    //Get Department List for dropdown
    $scope.DepartmentList = function () {
        $http.get(SiteUrl+"/Login/SelectDepartment").
            success(function (data, status) {

                $scope.Departments = data;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {
                $scope.showLoader = false;
            });
    };

    //Get Designation List for dropdown
    $scope.DesignationList = function () {
        $http.get(SiteUrl+"/Login/SelectDesignation").
            success(function (data, status) {

                $scope.Designations = data;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {
                alert("Error while fetching designation list");
                $scope.showLoader = false;
            });
    };

    //Get Post List for dropdown
    $scope.PostList = function () {
        $http.get(SiteUrl+"/Login/SelectPost").
            success(function (data, status) {

                $scope.Posts = data;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {
                alert("Error while fetching Post list");
                $scope.showLoader = false;
            });
    };

    //Get Role List for dropdown
    $scope.RoleList = function () {
        $http.get(SiteUrl+"/Login/SelectRole").
            success(function (data, status) {
                //alert(JSON.stringify(data));
                //return false;
                $scope.Roles = data;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {
                alert("Error while fetching Role list");
                $scope.showLoader = false;
            });
    };

    // END : Get List of Villages

    // Event for Page load
    $scope.getTranslation($scope);
    $scope.OnPageLoad();
    $scope.DepartmentList();
    $scope.DesignationList();
    $scope.PostList();
    $scope.RoleList();

    // Language Selector at page load
   
});





