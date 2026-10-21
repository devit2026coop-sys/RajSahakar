var UserRegistrationModule = angular.module('UserRegistrationMod', ['angularUtils.directives.dirPagination'])

UserRegistrationModule.directive('onlyLettersInput', onlyLettersInput);

function onlyLettersInput() {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                var transformedInput = text.replace(/[^a-zA-Z ]/g, '');
                if (transformedInput !== text) {
                    ngModelCtrl.$setViewValue(transformedInput);
                    ngModelCtrl.$render();
                }
                return transformedInput;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
};
UserRegistrationModule.controller("UserRegistrationCnt", function ($scope, $http) {


    // SCOPE
    $scope.RoleDetailCurrentIndex = 0;
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.Tehsil = [];
    $scope.City = [];
    $scope.Ward = [];
    $scope.Titles = [];
    $scope.Unit = [];
    $scope.Level = [];
    $scope.RoleTList = [];
    var RoleTempList = [];
    // var ClaimRuleList = new Array();
    $scope.UrbanDiv = true;
    $scope.RuralDiv = false;

    $scope.ImageUrl = null;
    $scope.Divisionmsg = '';
    $scope.Districtmsg = '';
    $scope.SelectedFileForUpload = null;

    $scope.selectFileforUpload = function (file) {
        $scope.SelectedFileForUpload = file[0];
    }


    // START : Get List of all users
    $scope.UserList = function () {
        $http.post(SiteUrl + "/User/ListUsers").
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
    $scope.SaveUser = function () {
        var validate = true;
        //if ($scope.UserRegistration.DivisionId == "" || $scope.UserRegistration.DivisionId == "0") {
        //    $scope.Divisionmsg = 'Division Required.';
        //    return;
        //}
        //else if ($scope.UserRegistration.DistrictId == "" || $scope.UserRegistration.DistrictId == "0") {
        //    $scope.Districtmsg = 'District Required.';
        //    return;
        //}
        LoaderStart();
        var formData = new FormData();

        formData.append("UserId", 0);
        formData.append("UserTitle", $scope.UserRegistration.UserTitle);
        formData.append("OfficerNameEn", $scope.UserRegistration.OfficerNameEn);
        formData.append("OfficerNameHi", $scope.UserRegistration.OfficerNameHi);
        formData.append("Gender", $scope.UserRegistration.Gender);
        formData.append("MobileNo", $scope.UserRegistration.MobileNo);
        formData.append("PhoneNo", $scope.UserRegistration.PhoneNo);
        formData.append("Password", $scope.UserRegistration.Password);
        formData.append("SSOID", $scope.UserRegistration.SSOID);
        formData.append("EmailId", $scope.UserRegistration.EmailId);
        formData.append("ImageUrl", $scope.SelectedFileForUpload);
        //formData.append("DivisionId", $scope.UserRegistration.DivisionId);
        formData.append("DistrictId", $scope.UserRegistration.UserDistrictId);
        //formData.append("IsRuralUrban", $scope.UserRegistration.IsRuralUrban);
        //formData.append("TehsilId", $scope.UserRegistration.TehsilId);
        //formData.append("CityId", $scope.UserRegistration.CityId);
        //formData.append("WardId", $scope.UserRegistration.WardId);
        //formData.append("BlockId", $scope.UserRegistration.BlockId);
        //formData.append("GPId", $scope.UserRegistration.GPId);
        //formData.append("VillageId", $scope.UserRegistration.VillageId);
        formData.append("AddressEn", $scope.UserRegistration.AddressEn);
        formData.append("AddressHi", $scope.UserRegistration.AddressHi);
        formData.append("PIN", $scope.UserRegistration.PIN);
        formData.append("RoleArray", JSON.stringify(RoleTempList));
        //$("select").prop('selectedIndex', 0);
        $http.post(SiteUrl + "/User/SaveUser", formData,
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
                var errorMsg = errorLst.join("</br>");
                if (errorMsg == '1</br>') {
                    ShowModelAlert("User Registration", 'Data Saved Successfully.');
                   // $scope.OnPageLoad();
                }
                else if (errorMsg == '0</br>') {
                    ShowModelAlert("User Registration", 'Data Already Exists.');
                }
                else if (errorMsg == '-1</br>') {
                    ShowModelAlert("User Registration", 'Error');
                }
                else {
                    ShowModelAlert("User Registration", errorMsg);
                }
                // window.location.href = "/User/UserRegistration";
                if (data.Status) {

                    // $scope.ClearForm();
                    // $scope.UserList();
                }
                LoaderStop();
            }).
            error(function (data, status) {
                LoaderStop();
                ShowModelAlert("User Registration", "Error while user insert");
            });
    };
    // END : Save/Edit User
    
    // START : Change Status for User / delete user
    $scope.SetStatus = function (UserId, ActionId) {

        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/User/ChangeStatus", { Id: UserId, Action: ActionId }).
            success(function (data, status) {
                $scope.UserList();
            }).
            error(function (data, status) {
            });
    };
    // END : Change Status for User / delete user

    // START : Get Data for Update
    $scope.Update = function (UserId) {
        $http.post(SiteUrl + "/User/ListUsers", { WhereClause: ' AND [UserId]=' + UserId }).
            success(function (data, status) {
                $scope.UserRegistration.DivisionId = data[0].DivisionId;
                $scope.DistrictList();

                $scope.UserRegistration.DistrictId = data[0].DistrictId;

                //if (data[0].IsRuralArban == "True") {
                //    $scope.UserRegistration.IsRuralUrban = true;
                //    $scope.RuralUrbanBlock();

                //    //$scope.UserRegistration.TehsilId = data[0].TehsilId;
                //    //$scope.UserRegistration.CityId = data[0].CityId;

                //    //$scope.WardList();
                //    //$scope.UserRegistration.WardId = data[0].WardId;
                //}
                //else {
                //    $scope.UserRegistration.IsRuralUrban = false;
                //    $scope.RuralUrbanBlock();

                //    //$scope.GramPanchayatList();
                //    //$scope.UserRegistration.BlockId = '0';

                //    //$scope.GramPanchayatList()
                //    //$scope.UserRegistration.GPId = '0';

                //    //$scope.VillageList();
                //    //$scope.UserRegistration.VillageId = '0';
                //}

                $scope.UserRegistration.BlockId = data[0].DivisionId;
                $scope.UserRegistration.GPId = data[0].GPId;
                $scope.UserRegistration.VillageId = data[0].VillageId;
                $scope.UserRegistration.OfficerNameEn = data[0].OfficerNameEn;
                $scope.UserRegistration.OfficerNameHi = data[0].OfficerNameHi;
                $scope.UserRegistration.PhoneNo = data[0].PhoneNo;
                $scope.UserRegistration.MobileNo = data[0].MobileNo;
                $scope.UserRegistration.EmailId = data[0].EmailId;
                $scope.UserRegistration.AddressEn = data[0].Address;
                $scope.UserRegistration.AddressHi = data[0].AddressHi;
                $scope.UserRegistration.Gender = data[0].Gender;
                $scope.UserRegistration.PIN = data[0].Pincode;
                $scope.UserRegistration.UserTitle = data[0].UserTitle;
            }).
            error(function (data, status) {
            });
    };
    // END : Get Data for Update

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.UserRegistration.DivisionId = '0';
        $scope.UserRegistration.DistrictId = '0';
        $scope.UserRegistration.IsRuralUrban = false;
        $scope.UserRegistration.TehsilId = '0';
        $scope.UserRegistration.CityId = '0';
        $scope.UserRegistration.WardId = '0';
        $scope.UserRegistration.BlockId = '0';
        $scope.UserRegistration.GPId = '0';
        $scope.UserRegistration.VillageId = '0';
        $scope.UserRegistration.OfficerNameEn = '';
        $scope.UserRegistration.OfficerNameHi = '';
        $scope.UserRegistration.PhoneNo = '';
        $scope.UserRegistration.MobileNo = '';
        $scope.UserRegistration.EmailId = '';
        $scope.UserRegistration.AddressEn = '';
        $scope.UserRegistration.AddressHi = '';
        $scope.UserRegistration.Gender = '0';
        $scope.UserRegistration.PIN = '';
        $scope.UserRegistration.UserTitle = '0';

        $scope.Districts = [];
        $scope.Tehsil = [];
        $scope.City = [];
        $scope.Ward = [];

        $scope.Blocks = [];
        $scope.GramPanchayats = [];
        $scope.Villages = [];
    };
    //END : Delete User

    // START : Code for Language label setting
    $scope.getTranslation = function () {
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
    };
    // END : Code for Language label setting

    $scope.pageChangeHandler = function (num) {
    };

    // START : Get List of Divisions and Title(on page load)
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        //$http.get(SiteUrl+"/User/GetUsers").

        //    success(function (data2, status) {

        //        if (data2[0].ActionID != "") {


        //            var str = data2[0].ListUsers;

        //            if (str == undefined) return;

        //            var str_array = str.split(',');


        //            angular.forEach(str_array, function (Datavalue) {
        //                if (Datavalue == 'Add') {

        //                    $scope.IsAdd = true;
        //                }
        //                else if (Datavalue == 'Edit') {
        //                    //$scope.tdEdit.style.display = "none";
        //                    $scope.IsEdit = true;
        //                }
        //                else if (Datavalue == 'Take Action') {
        //                    //$scope.tdEdit.style.display = "none";
        //                    $scope.IsTakeAction = true;
        //                }
        //                else if (Datavalue == 'Status') {
        //                    //$scope.tdEdit.style.display = "none";
        //                    $scope.IsStatus = true;
        //                }
        //                else if (Datavalue == 'delete') {
        //                    //$scope.tdEdit.style.display = "none";
        //                    $scope.IsDelete = true;
        //                }
        //                else {
        //                    $scope.IsAdd = false;
        //                    $scope.IsEdit = false;
        //                    $scope.IsTakeAction = false;
        //                    $scope.IsStatus = false;
        //                    $scope.IsDelete = false;
        //                }
        //            });

        //        }

        //    }).
        //    error(function (data1, status) {

        //        $scope.WorkFlowExecution = [];
        //    });
        // on page load End
        $scope.Divisions = [{}];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
            success(function (data, status) {
                $scope.Divisions = data;
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });

        $scope.Titles = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Title&MasterCode=-1").
            success(function (data, status) {
                $scope.Titles = data;
            }).
            error(function (data, status) {
                $scope.Titles = [];
            });

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Gender&MasterCode=-1").
            success(function (data, status) {
                $scope.Gender = data;
            }).
            error(function (data, status) {
                $scope.Gender = [];
            });
        $scope.DepartmentList();
        $scope.DesignationList();
        $scope.PostList();
        $scope.RoleList();
        $scope.UserDistrictList();
        $scope.LevelList();
        $scope.UserList();
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Get List of Divisions and Title
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {
               alert("Error while fetching department list")
                $scope.showLoader = false;
            });
    };
    //Get Designation List for dropdown
    $scope.DesignationList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DESIGNATION&MasterCode=-1").
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
               alert("Error while fetching designation list")
                $scope.showLoader = false;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {
                alert("Error while fetching Post list")
                $scope.showLoader = false;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {
                alter("Error while fetching Role list");
                $scope.showLoader = false;
            });
    };


    // START : Get List of Districts
    $scope.DistrictList = function () {
        $scope.Districts = [{}];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=" + $scope.UserRegistration.DivisionId).
            success(function (data, status) {
                $scope.Districts = data;
            }).
            error(function (data, status) {
                $scope.Districts = [{}];
            });
    };
    // END : Get List of Districts

    // START : Get List of Districts
    $scope.DistrictChange = function () { 
        $scope.UserRegistration.IsRuralUrban = false;
        $scope.RuralUrbanBlock();
        
        $scope.UserRegistration.TehsilId = '0';
        $scope.UserRegistration.BlockId = '0';
        $scope.UserRegistration.WardId = '0';
        $scope.UserRegistration.CityId = '0';

    };
    // END : Get List of Districts

    // START : Get List of Districts
    $scope.UserDistrictList = function () {
        $scope.UserDistricts = [{}];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=-1").
            success(function (data, status) {
                $scope.UserDistricts = data;
            }).
            error(function (data, status) {
                $scope.UserDistricts = [{}];
            });
    };
    // END : Get List of Districts

    // START : Set Rural/ Urban div show/hide
    $scope.RuralUrbanBlock = function () {
        var SelectedVal = $scope.UserRegistration.IsRuralUrban;
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
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=tehsil&MasterCode=" + $scope.UserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.Tehsil = data;
                }).
                error(function (data, status) {
                    $scope.Tehsil = [];
                });

            $scope.City = [];
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=city&MasterCode=" + $scope.UserRegistration.DistrictId).
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

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.UserRegistration.DistrictId).
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

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ward&MasterCode=" + $scope.UserRegistration.CityId).
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
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.UserRegistration.BlockId).
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

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.UserRegistration.GPId).
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

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=UNITS&MasterCode=" + $scope.UserRegistration.DEPTCODE).
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
    // Event for Page load
  //  $scope.OnPageLoad();


    $scope.SaveTempRole = function () {
        var index = $scope.RoleDetailCurrentIndex;
        if ($("#ddlDept option:selected").val() > 0) {
            var UrbanRural = '';
            var IsUrbanRural = false;
            $("input[type='radio']:checked").each(function () {
                var idVal = $(this).attr("id");
                UrbanRural = $("label[for='" + idVal + "']").text();
                if (UrbanRural == 'Urban') {
                    IsUrbanRural = true;
                }
            });
            if ($('#btnroledetailsadd').text() == "Add" || $('#btnroledetailsadd').text() == "जोड़े") {
                RoleTempList.push({
                    "DepartmentName": $("#ddlDept option:selected").text(),
                    "DepartmentId": $("#ddlDept option:selected").val(),
                    "UnitName": $("#ddlunit option:selected").text(),
                    "UnitId": $("#ddlunit option:selected").val(),
                    "LevelName": $("#ddllevel option:selected").text(),
                    "LevelId": $("#ddllevel option:selected").val(),
                    "DesignationName": $("#ddlDesignation option:selected").text(),
                    "DesignationId": $("#ddlDesignation option:selected").val(),
                    "PostName": $("#ddlPost option:selected").text(),
                    "PostId": $("#ddlPost option:selected").val(),
                    "RoleName": $("#ddlRole option:selected").text(),
                    "RoleId": $("#ddlRole option:selected").val(),
                    "DivisionName": $("#ddlDivision option:selected").text(),
                    "DivisionId": $("#ddlDivision option:selected").val(),
                    "District": $("#ddlDistrict option:selected").text(),
                    "DistrictId": $("#ddlDistrict option:selected").val(),
                    "UrbanRural": UrbanRural,
                    "IsUrbanRural": IsUrbanRural,
                    "Tehsil": $("#ddlTehsil option:selected").text(),
                    "TehsilId": $("#ddlTehsil option:selected").val(),
                    "City": $("#ddlCity option:selected").text(),
                    "CityId": $("#ddlCity option:selected").val(),
                    "Ward": $("#ddlWard option:selected").text(),
                    "WardId": $("#ddlWard option:selected").val(),
                    "PanchayatSamiti": $("#ddlPanchayatSamiti option:selected").text(),
                    "PanchayatSamitiId": $("#ddlPanchayatSamiti option:selected").val(),
                    "GramPanchayat": $("#ddlGramPanchayat option:selected").text(),
                    "GramPanchayatId": $("#ddlGramPanchayat option:selected").val(),
                    "Village": $("#ddlVillage option:selected").text(),
                    "VillageId": $("#ddlVillage option:selected").val(),
                    "IsActive": $("#chkIsActive").prop("checked") == true ? "1" : "0"
                });
            }
            else {
                $scope.RoleTList[index].DepartmentId = $("#ddlDept option:selected").val();
                $scope.RoleTList[index].DepartmentName = $("#ddlDept option:selected").text();
                $scope.RoleTList[index].UnitId = $("#ddlunit option:selected").val();
                $scope.RoleTList[index].UnitName = $("#ddlunit option:selected").text();
                $scope.RoleTList[index].LevelId = $("#ddllevel option:selected").val();
                $scope.RoleTList[index].LevelName = $("#ddllevel option:selected").text();
                $scope.RoleTList[index].DesignationId = $("#ddlDesignation option:selected").val();
                $scope.RoleTList[index].DesignationName = $("#ddllevel option:selected").text();
                $scope.RoleTList[index].PostId = $("#ddlPost option:selected").val();
                $scope.RoleTList[index].PostName = $("#ddlPost option:selected").text();
                $scope.RoleTList[index].RoleId = $("#ddlRole option:selected").val();
                $scope.RoleTList[index].RoleName = $("#ddlRole option:selected").text();
                $scope.RoleTList[index].DivisionId = $("#ddlDivision option:selected").val();
                $scope.RoleTList[index].DivisionName = $("#ddlDivision option:selected").text();
                $scope.RoleTList[index].DistrictId = $("#ddlDistrict option:selected").val();
                $scope.RoleTList[index].UrbanRural = UrbanRural;
                $scope.RoleTList[index].IsUrbanRural = IsUrbanRural;
                $scope.RoleTList[index].District = $("#ddlDistrict option:selected").text();
                $scope.RoleTList[index].TehsilId = $("#ddlTehsil option:selected").val();
                $scope.RoleTList[index].Tehsil = $("#ddlTehsil option:selected").text();
                $scope.RoleTList[index].CityId = $("#ddlCity option:selected").val();
                $scope.RoleTList[index].City = $("#ddlCity option:selected").text();
                $scope.RoleTList[index].WardId = $("#ddlWard option:selected").val();
                $scope.RoleTList[index].Ward = $("#ddlWard option:selected").text();
                $scope.RoleTList[index].PanchayatSamitiId = $("#ddlPanchayatSamiti option:selected").val();
                $scope.RoleTList[index].PanchayatSamiti = $("#ddlPanchayatSamiti option:selected").text();
                $scope.RoleTList[index].GramPanchayatId = $("#ddlGramPanchayat option:selected").val();
                $scope.RoleTList[index].GramPanchayat = $("#ddlGramPanchayat option:selected").text();
                $scope.RoleTList[index].VillageId = $("#ddlVillage option:selected").val();
                $scope.RoleTList[index].Village = $("#ddlVillage option:selected").text();
                $scope.RoleTList[index].IsActive = $("#chkIsActive").prop("checked") == true ? "1" : "0";
            }
            $scope.RoleTList = RoleTempList;
            $('.table-responsive').show();
        }
        else {
            $('.table-responsive').hide();
        }
        $('#btnroledetailsadd').text("Add");
    };

    // START : Get Role for Update
    $scope.EditRoleDetails = function (index) {
        RoleDetailCurrentIndex = index;
        $('#btnroledetailsadd').text("Edit");
        $("#ddlDept").val($scope.RoleTList[index].DepartmentId);

        $("#ddlunit").val($scope.RoleTList[index].UnitId);

        $("#ddllevel").val($scope.RoleTList[index].LevelId);

        $("#ddlDesignation").val($scope.RoleTList[index].DesignationId);

        $("#ddlPost").val($scope.RoleTList[index].PostId);

        $("#ddlRole").val($scope.RoleTList[index].RoleId);

        $("#ddlDivision").val($scope.RoleTList[index].DivisionId);

        $("#ddlDistrict").val($scope.RoleTList[index].DistrictId);
        //"UrbanRural": UrbanRural,
        //"IsUrbanRural": IsUrbanRural,

        $("#ddlTehsil").val($scope.RoleTList[index].TehsilId);

        $("#ddlCity").val($scope.RoleTList[index].CityId);

        $("#ddlWard").val($scope.RoleTList[index].WardId);

        $("#ddlPanchayatSamiti").val($scope.RoleTList[index].PanchayatSamitiId);

        $("#ddlGramPanchayat").val($scope.RoleTList[index].GramPanchayatId);

        $("#ddlVillage").val($scope.RoleTList[index].VillageId);
        // $("#chkIsActive").prop("checked")
    }
    //End
    // Language Selector at page load
    $scope.getTranslation($scope);

    //Get Role List for dropdown
    $scope.UserRoleProfile = function () {
        $scope.UserRoleProfile = [];
        $http.get(SiteUrl + "/User/SelectRole").
            success(function (data, status) {
                $scope.UserRoleProfile = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Role list");
            });
    };

    $scope.OnPageLoad();

});





