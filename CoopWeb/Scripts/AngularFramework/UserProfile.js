var UserProfileModule = angular.module('UserProfileMod', ['angularUtils.directives.dirPagination'])
UserProfileModule.controller("UserProfileCnt", function ($scope, $http) {
    $scope.RoleDetailCurrentIndex = 0;
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.Tehsil = [];
    $scope.City = [];
    $scope.Ward = [];
    $scope.Titles = [];
    $scope.Unit = [];
    $scope.Level = [];
    $scope.MappingFromId = 0;
    $scope.UrbanDiv = true;
    $scope.RuralDiv = false;

    $scope.ImageUrl = null;
    $scope.Divisionmsg = '';
    $scope.Districtmsg = '';
    //Get Department List for dropdown
    $scope.DepartmentList = function () {

        $http.get(SiteUrl + "/User/SelectDepartment").
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


            }).
            error(function (data, status) {
                alert("Error while fetching department list")

            });
    };
    //Get Designation List for dropdown
    $scope.DesignationList = function () {
        $http.get(SiteUrl + "/User/SelectDesignation").
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


            }).
            error(function (data, status) {
                alert("Error while fetching designation list")

            });
    };
    //Get Post List for dropdown
    $scope.PostList = function () {
        $http.get(SiteUrl + "/User/SelectPost").
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


            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
    };

    //Get Role List for dropdown
    $scope.RoleList = function () {
        $http.get(SiteUrl + "/User/SelectRole").
            success(function (data, status) {

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


            }).
            error(function (data, status) {
                alert("Error while fetching Role list")

            });
    };


    // START : Get List of Districts
    $scope.DistrictList = function () {
        $scope.Districts = [{}];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=" + $scope.UserProfile.DivisionId).
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
        var SelectedVal = $scope.UserProfile.IsRuralUrban;
        $("#ddlPanchayatSamiti").val('0');
        $("#ddlGramPanchayat").val('0');
        $("#ddlVillage").val('0');
        $("#ddlTehsil").val('0');
        $("#ddlCity").val('0');
        $("#ddlWard").val('0');
        if (SelectedVal == 'true') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;

            $scope.Tehsil = [];
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=tehsil&MasterCode=" + $scope.UserProfile.DistrictId).
                success(function (data, status) {
                    $scope.Tehsil = data;
                }).
                error(function (data, status) {
                    $scope.Tehsil = [];
                });

            $scope.City = [];
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=city&MasterCode=" + $scope.UserProfile.DistrictId).
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

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.UserProfile.DistrictId).
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

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ward&MasterCode=" + $scope.UserProfile.CityId).
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
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.UserProfile.BlockId).
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

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.UserProfile.GPId).
            success(function (data, status) {
                $scope.Villages = data;
            }).
            error(function (data, status) {
                $scope.Villages = [];
            });
    };
    // END : Get List of Villages
    //Start: Get Unit List
    $scope.UnitList = function () {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=UNITS&MasterCode=" + $scope.UserProfile.DEPTCODE).
            success(function (data, status) {
                $scope.Unit = data;
            }).
            error(function (data, status) {
                $scope.Unit = [];
            });
    };
    //End 
    //Start: Get Unit List
    $scope.LevelList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=LEVELS&MasterCode=" + 0).
            success(function (data, status) {
                $scope.Level = data;
            }).
            error(function (data, status) {
                $scope.Level = [];
            });
    };
    //End
    $scope.UserRoleProfileData = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.UserRoleProfile = [];
        $http.get(SiteUrl + "/User/GetUserRoleAll").
            success(function (data, status) {
                $scope.UserRoleProfile = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Role list")
            });
        $(".se-pre-con").fadeOut("slow");
    };
    $scope.UserRoleProfileData();

    // START : Code for Language label setting
    $scope.getTranslation = function () {
        $scope.language = _language;
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
    $scope.getTranslation();

    $scope.DivisionsList = function () {
        $scope.Divisions = [{}];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
            success(function (data, status) {
                $scope.Divisions = data;
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });
    }
    $scope.DivisionsList();
    $scope.DepartmentList();
    $scope.DesignationList();
    $scope.PostList();
    $scope.RoleList();
    $scope.LevelList();

    $scope.AddRoleClick = function (MapId) {
        $(this).attr("data-target", "#myModal");
        $scope.MappingFromId = MapId;
    };
    // START : Save/Edit User
    $scope.SaveData = function () {
        var validate = true;
        if ($scope.UserProfile.DEPTCODE == "" || $scope.UserProfile.DEPTCODE == "0") {
            alert('Department Department.');
            return;
        }
        if ($scope.UserProfile.UnitId == "" || $scope.UserProfile.UnitId == "0") {
            alert('Unit Required.');
            return;
        }
        if ($scope.UserProfile.LevelId == "" || $scope.UserProfile.LevelId == "0") {
            alert('Level Required.');
            return;
        }
        if ($scope.UserProfile.DESIGNATIONCODE == "" || $scope.UserProfile.DESIGNATIONCODE == "0") {
            alert('DESIGNATION Required.');
            return;
        }
        if ($scope.UserProfile.PostId == "" || $scope.UserProfile.PostId == "0") {
            alert('Post Required.');
            return;
        }
        if ($scope.UserProfile.RoleID == "" || $scope.UserProfile.RoleID == "0") {
            alert('Role Required.');
            return;
        }
        if ($scope.UserProfile.DivisionId == "" || $scope.UserProfile.DivisionId == "0") {
            alert('Division Required.');
            return;
        }
        else if ($scope.UserProfile.DistrictId == "" || $scope.UserProfile.DistrictId == "0") {
            alert('District Required.');
            return;
        }

        LoaderStart();
        var formData = new FormData();
        formData.append("DepartmentId", $scope.UserProfile.DEPTCODE);
        formData.append("UnitId", $scope.UserProfile.UnitId);
        formData.append("LevelId", $scope.UserProfile.LevelId);
        formData.append("DesignationId", $scope.UserProfile.DESIGNATIONCODE);
        formData.append("PostId", $scope.UserProfile.PostId);
        formData.append("RoleId", $scope.UserProfile.RoleID);
        formData.append("DivisionId", $scope.UserProfile.DivisionId);
        formData.append("DistrictId", $scope.UserProfile.DistrictId);
        formData.append("IsRuralUrban", $scope.UserProfile.IsRuralUrban);
        formData.append("TehsilId", $scope.UserProfile.TehsilId);
        formData.append("CityId", $scope.UserProfile.CityId);
        formData.append("WardId", $scope.UserProfile.WardId);
        formData.append("BlockId", $scope.UserProfile.BlockId);
        formData.append("GPId", $scope.UserProfile.GPId);
        formData.append("VillageId", $scope.UserProfile.VillageId);
        formData.append("MappingFromId", $scope.MappingFromId);
        $http.post(SiteUrl + "/User/SaveTransfer", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                $('#modelAlertMessage').html('');
                var errorLst = [];
                for (var i = 0; i < Object.keys(data).length; i++) {
                    errorLst.push(data.data[i]);
                }
                var errorMsg = errorLst;
                if (parseInt(errorMsg[0]) > 0) {
                    ShowModelAlert("Role Transfer", 'Data Saved Successfully.');
                }
                else if (errorMsg < 0) {
                    ShowModelAlert("Role Transfer", 'Error');
                }
                else {
                    ShowModelAlert("Role Transfer", errorMsg);
                }
                // window.location.href = "/User/UserProfile";
                if (data.Status) {
                    $scope.ClearForm();
                }
                LoaderStop();
            }).
            error(function (data, status) {
                LoaderStop();
                ShowModelAlert("Role Transfer", "Error while user insert");
            });
    };

    // END : Save/Edit User

    //START : Clear Form
    $scope.ClearForm = function () {
        $('#btnClose').trigger("click");
        $('.modal-content').find("select").each(function () {
            this.selectedIndex = 0;
        });
    };





    $scope.SaveUserProfile = function () {

        var formData = new FormData();
        formData.append("UserId", $scope.UserRegistration.UserId == undefined ? "0" : $scope.UserRegistration.UserId);
        formData.append("OfficerNameEn", $scope.UserRegistration.OfficerNameEn);
        formData.append("OfficerNameHi", $scope.UserRegistration.OfficerNameHi);
        formData.append("MobileNo", $scope.UserRegistration.MobileNo);
        formData.append("EmailId", $scope.UserRegistration.EmailId);
        formData.append("AddressHi", $scope.UserRegistration.AddressHi);
        formData.append("AddressEn", $scope.UserRegistration.AddressEn);


        //$("select").prop('selectedIndex', 0);
        LoaderStart();
        $http.post("/User/SaveUserProfile", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(data);
                //  ShowModelAlert("User Registration", data);
                alert(data);
                $scope.ShowProfileForm = false;
                $scope.UserRegistration = {};
                window.location.reload();
                $('#myModal').modal('hide');
            }).
            error(function (data, status) {
                alert("Error while user insert");
                // ShowModelAlert("User Registration", "Error while user insert");
            });
        LoaderStop();
    };

    $scope.openPopup = function () {

        $scope.UserRegistration = {}
        $scope.UserRegistration.UserId = $('#hdnUserId').val();
        $scope.UserRegistration.OfficerNameEn = $('#hdnOfficerNameEn').val();
        $scope.UserRegistration.OfficerNameHi = $('#hdnOfficerNameHi').val();
        $scope.UserRegistration.MobileNo = $('#hdnMobileNo').val();
        $scope.UserRegistration.EmailId = $('#hdnEmailId').val();
        $scope.UserRegistration.AddressHi = $('#hdnAddressHi').val();
        $scope.UserRegistration.AddressEn = $('#hdnAddressEn').val();


        // $scope.openForm();
        $scope.ShowProfileForm = true;
        $('#profileUpdate').modal('show');
        $('#profileUpdate').modal({
            backdrop: 'static',
            keyboard: false
        });
    }

    $scope.ShowProfileForm = false;

    $scope.openForm = function () {
        $http.get("/User/GetloginUSer").
            success(function (data, status) {
                //$scope.ShowProfileForm = false;
                $scope.ProfileData = data;
                $scope.UserRegistration = $scope.ProfileData[0];
                $scope.colsort = "";
                $scope.reverseSort = false;

            }).
            error(function (data, status) {
                alert("Error while user insert");
            });
    };
});

UserProfileModule.directive('addressOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            ngModel.$validators.addressOnly = function (value) {
                return /^[a-zA-Z0-9\s-]+$/.test(value || '');
            };
        }
    };
});



