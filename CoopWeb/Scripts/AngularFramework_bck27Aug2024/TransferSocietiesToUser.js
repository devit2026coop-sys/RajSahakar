var UserRegistrationModule = angular.module('UserTransAddiChargeRequestModule', ['angularUtils.directives.dirPagination'])

UserRegistrationModule.directive('onlyLettersInput', onlyLettersInput);

function onlyLettersInput() {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                var transformedInput = text.replace(/[^a-zA-Z ]/g, '');
                //console.log(transformedInput);
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
UserRegistrationModule.controller("UserTransAddiChargeRequestCont", function ($scope, $http) {
    // SCOPE
    $scope.RoleDetailCurrentIndex = 0;
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.Tehsil = [];
    $scope.City = [];
    $scope.Ward = [];
    $scope.Titles = [];
    $scope.Level = [];
    $scope.RoleTList = [];
    var RoleTempList = [];
    // var ClaimRuleList = new Array();
    $scope.UrbanDiv = false;
    $scope.RuralDiv = false;
    $scope.RuralUrbanRedioDiv = false;

    $scope.ImageUrl = null;
    $scope.Divisionmsg = '';
    $scope.Districtmsg = '';
    $scope.Users = '';
    $scope.SelectedFileForUpload = null;

    $scope.selectFileforUpload = function (file) {
        $scope.SelectedFileForUpload = file[0];
    }
    $scope.ResionIDs = "";
    $scope.ActIds = "";
    $scope.PLDBList = [];
    $scope.USERIDLocal = '0';






    $scope.ActList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function (data, status) {
                $scope.Acts = data;
            }).
            error(function (data, status) {
                $scope.Acts = [];
            });
    };



    $scope.UserList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Users = [];
        $http.post(SiteUrl + "/login/SSOUserListfortrnsfer", { 'RoleId': $scope.transferSocietiesToUser.RoleId }).
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



    $scope.RoleList = function () {
        $http.get(SiteUrl + "/User/SelectRole/?LevelId=" + 0).
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

    //validateAddForm 
    $scope.FormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.transferSocietiesToUser.ActId == 0 || $scope.transferSocietiesToUser.ActId == undefined || $scope.transferSocietiesToUser.ActId == '0') {
            message += $scope._language == 'en-us' ? '\n<br />Please Select Act' : '\n<br />कृपया कार्य प्रवाह का चयन करे  |';
        }

        if ($scope.transferSocietiesToUser.RoleId == 0 || $scope.transferSocietiesToUser.RoleId == undefined || $scope.transferSocietiesToUser.RoleId == '0') {
            message += $scope._language == 'en-us' ? '\n<br />Please Select Role' : '\n<br />कृपया संगठन का चयन करे  |';
        }

        if ($scope.transferSocietiesToUser.FromUser == 0 || $scope.transferSocietiesToUser.FromUser == undefined || $scope.transferSocietiesToUser.FromUser == '0') {
            message += $scope._language == 'en-us' ? '\n<br />Please Select FromUser' : '\n<br />कृपया कार्य प्रवाह का चयन करे  |';
        }

        if ($scope.transferSocietiesToUser.ToUser == 0 || $scope.transferSocietiesToUser.ToUser == undefined || $scope.transferSocietiesToUser.ToUser == '0') {
            message += $scope._language == 'en-us' ? '\n<br />Please Select ToUser' : '\n<br />कृपया संगठन का चयन करे  |';
        }
        if (message.length > 29) {
            ShowModelAlert("Users Charge Request", message);
            return false;
        }
        return true;
    }
    // START : Save/Edit User
    $scope.SaveUser = function () {
        if ($scope.FormValidation()) {
            var formData = new FormData();
            formData.append("FromUser", $scope.transferSocietiesToUser.FromUser);
            formData.append("ToUser", $scope.transferSocietiesToUser.ToUser);
            formData.append("ActId", $scope.transferSocietiesToUser.ActId);
            formData.append("RoleId", $scope.transferSocietiesToUser.RoleId);
            $http.post(SiteUrl + "/Society/SetSocietiesToUser", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    $(".se-pre-con").fadeIn("slow");
                    var message = '';
                    if (data > 0) {
                        message = _language == 'en-us' ? 'Request Update Scuessfully\n' : 'डेटा सहेजा गया है\n';
                        $(".se-pre-con").fadeOut("slow");
                    }

                    else if (data < 0) {
                        message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                        $(".se-pre-con").fadeOut("slow");
                    }
                    alert(message);
                    $scope.transferSocietiesToUser.FromUser = '0';
                    $scope.transferSocietiesToUser.ToUser = '0';
                    $scope.transferSocietiesToUser.ActId = '0';
                    $scope.transferSocietiesToUser.RoleId = '0';

                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    LoaderStop();
                    ShowModelAlert("TransferScoietiesToUser", "Error While Insert Request");
                });
        }

    };


    $scope.GetCurrentUserId = function () {
        $http.get(SiteUrl + "/ApplicationFlow/GetCurrentUserId").
            success(function (data, status) {

                $scope.USERIDLocal = data;
                // alert($scope.UserRegistration.UserId);
            }).
            error(function (data, status) {
                $scope.USERIDLocal = '0';
            });
    }
    $scope.ShowGrid = false;

    $scope.GetAllSubUnitList = function () {
        $scope.ShowGrid = false;
        $http.get(SiteUrl + "/Login/UserSelfReqList").
            success(function (data, status) {
                $scope.UserReqData = data;
                if (data.length > 0) {
                    $scope.ShowGrid = true;
                }
            }).
            error(function (data1, status) {

            });
    };
    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.UserRegistration.DivisionId = '0';
        $scope.UserRegistration.DistrictId = '0';
        $scope.UserRegistration.ResionId = '0';
        $scope.UserRegistration.IsRuralUrban = false;
        $scope.UserRegistration.TehsilId = '0';
        $scope.UserRegistration.CityId = '0';
        $scope.UserRegistration.WardId = '0';
        $scope.UserRegistration.BlockId = '0';
        $scope.UserRegistration.GPId = '0';
        $scope.UserRegistration.VillageId = '0';
        $scope.UserRegistration.UserTitle = '0';
        $scope.UserRegistration.RoleID = '0';
        $scope.UserRegistration.DESIGNATIONCODE = '0';
        $scope.UserRegistration.SSOID = '';
        $scope.UserRegistration.ProcessingUnitId = '0';
        $scope.UserRegistration.DepotId = '0';
        $scope.UserRegistration.WarehouseId = '0';
        $scope.UserRegistration.DesignationLevelMappingId = '0';
        $scope.UserRegistration.OrganizationLevelsMappingID = '0';
        $scope.UserRegistration.OfficeId = '0';
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

    // END : Get List of Divisions and Title
    //Get Department List for dropdown
    $scope.DepartmentList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DEPARTMENT&MasterCode=-1").
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.Department = data;
                if (data.length > 0) {
                    var DeoptCode = data.filter(function (x) { return x.DEPTNAMEENGLISH == 'COOPERATIVE' })[0].ID;
                    if (DeoptCode != undefined && DeoptCode != '' && DeoptCode != null) {
                        $scope.UserRegistration.DEPTCODE = DeoptCode;
                        $scope.GetAllSubOrganisationList();
                        $(".se-pre-con").fadeOut("slow");
                    }
                    else {
                        $scope.UserRegistration.DEPTCODE = '0';
                        $(".se-pre-con").fadeOut("slow");
                    }
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching department list");
                $scope.Department = [];
            });
    };

    //Get Post List for dropdown
    $scope.PostList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=" + $scope.UserRegistration.DESIGNATIONCODE).
            success(function (data, status) {
                $scope.Posts = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list");
                $scope.showLoader = false;
            });
    };


    // Get List of Designations 
    $scope.DesignationList = function () {
        $scope.Designations = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=designation&MasterCode=" + $scope.UserRegistration.DEPTCODE).
            success(function (data, status) {
                $scope.Designations = data;
            }).
            error(function (data, status) {
                $scope.Designations = [];
            });
    };

    //Get Designation List for dropdown

    $scope.OfficeList = function () {
        $scope.OfficeListData = [];
        $http.get(SiteUrl + "/Master/OfficesList/?WhereClause=and o.isactive =1 and   o.OrganizationLevelsMappingID=" + $scope.UserRegistration.OrganizationLevelsMappingID).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.OfficeListData = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching designation list");
                $scope.OfficeListData = [];
                $scope.showLoader = false;
            });
    };




    // START : Get List of Districts
    $scope.DistrictList = function () {
        $scope.Districts = [{}];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=-1").
            success(function (data, status) {
                $scope.Districts = data;

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.Districts = [{}];
            });
    };
    // END : Get List of Districts

    // START : Set Rural/ Urban div show/hide
    $scope.RuralUrbanBlock = function () {
        var SelectedVal = $scope.UserRegistration.IsRuralUrban;
        $("#ddlPanchayatSamiti").val('0');
        $("#ddlGramPanchayat").val('0');
        $("#ddlVillage").val('0');
        $("#ddlCity").val('0');
        $scope.UserRegistration.DivisionId = '0';
        $scope.UserRegistration.DistrictId = '0';
        $scope.UserRegistration.TehsilId = '0';
        $scope.UserRegistration.CityId = '0';
        $scope.UserRegistration.WardId = '0';
        $scope.UserRegistration.BlockId = '0';
        $scope.UserRegistration.ProcessingUnitId = '0';
        $scope.UserRegistration.WarehouseId = '0';
        $scope.UserRegistration.DepotId = '0';
        $scope.UserRegistration.GPId = '0';
        $scope.UserRegistration.VillageId = '0';
        $scope.CityList = [];
        $scope.Blocks = [];
        $scope.GramPanchayats = [];
        if (SelectedVal == 'true') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
        }
        else if (SelectedVal == 'false') {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
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
        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=UNITS&MasterCode=" + $scope.UserRegistration.DEPTCODE).

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=UNITS&MasterCode=0").
            success(function (data, status) {
                $scope.Unit = data;
            }).
            error(function (data, status) {
                $scope.Unit = [];
            });
    };
    //End 
    // GET Society Type
    $scope.GetSocietyTypeList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=3").
            success(function (data, status) {
                $scope.SocietyTypeList = data;
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = [];
            });
    };
    //END   
    //Start: Get Unit List
    $scope.LevelList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Level = [];
        $http.get(SiteUrl + "/MasterSettings/OrganizationLevelsList/?WhereClause=AND OLM.OrganizationId=" + $scope.UserRegistration.OrganisationId).
            success(function (data, status) {
                $scope.Level = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    //End 
    //Start: Get Unit List
    //$scope.GetValuesForDesignationList = function () {
    //    $scope.Designations = [];
    //    $scope.UserRegistration.DistrictId = '0';
    //    $scope.UserRegistration.IsRuralUrban = false;
    //    $scope.UserRegistration.TehsilId = '0';
    //    $scope.UserRegistration.CityId = '0';
    //    $scope.UserRegistration.WardId = '0';
    //    $scope.UserRegistration.BlockId = '0';

    //    $scope.UserRegistration.ProcessingUnitId = '0';
    //    $scope.UserRegistration.WarehouseId = '0';
    //    $scope.UserRegistration.DepotId = '0';
    //    $scope.ProcessingUnitList = [];
    //    $scope.DEPORTList = [];
    //    $scope.PLDBBranches = [];
    //    $scope.WAREHOUSEList = [];

    //    if ($scope.UserRegistration.OrganisationId == 11 && $scope.UserRegistration.LevelId == '4~24') {
    //        $scope.isPLDBBlock = true;
    //    }
    //    else {
    //        $scope.isPLDBBlock = false;
    //    }

    //    var Level_OrganizationLevelsMappingID = $scope.UserRegistration.LevelId;
    //    if (Level_OrganizationLevelsMappingID.indexOf("~") >= 0) {
    //        var arrayOfStrings = Level_OrganizationLevelsMappingID.split("~");
    //        $scope.UserRegistration.OrganizationLevelsMappingID = arrayOfStrings[1];
    //        $scope.UserRegistration.WorkScopeLevel_ID = arrayOfStrings[0];

    //    }
    //    else {
    //        $scope.UserRegistration.OrganizationLevelsMappingID = 0;
    //        $scope.UserRegistration.WorkScopeLevel_ID = 0;
    //    }
    //    $scope.Levelofficedata();
    //    $scope.RoleList();
    //    $scope.OfficeList();
    //};
    $scope.GetDesignationList = function () {
        $scope.Designations = [];


        $http.get(SiteUrl + "/MasterSettings/GetAllDesignationByOrganizationLevelsMappingID/?OrganizationLevelsMappingID=" + $scope.UserRegistration.OrganizationLevelsMappingID + "&OfficeId=" + $scope.UserRegistration.OfficeId).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.Designations = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.Designations = [];
            });
    };
    //End 
    //Get PLDBBranchList List for dropdown
    $scope.PLDBBranchList = function () {
        $scope.PLDBBranches = [];
        if ($scope.UserRegistration.LevelId.split('~')[0] > 3) {
            if ($scope.UserRegistration.PLDBId != '0') {
                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.UserRegistration.PLDBId).
                    success(function (data, status) {
                        $scope.PLDBBranches = data;
                    }).
                    error(function (data, status) {
                        $scope.PLDBBranches = [];
                    });
            }
        }
    };

    //Get SSO Details
    $scope.SSOUserDetails = function () {
        $http.get(SiteUrl + "/Login/GetSSOUserDetails/").
            success(function (data, status) {
                if (data != null && data.SSOID != "Data Could Not Be Retrieved From The Server Please Try After Some Time") {
                    $scope.UserRegistration = data;
                    $scope.UserRegistration.DivisionId = '0';
                    $scope.UserRegistration.DistrictId = '0';
                    $scope.UserRegistration.IsRuralUrban = false;
                    $scope.UserRegistration.TehsilId = '0';
                    $scope.UserRegistration.CityId = '0';
                    $scope.UserRegistration.WardId = '0';
                    $scope.UserRegistration.BlockId = '0';

                    $scope.UserRegistration.ProcessingUnitId = '0';
                    $scope.UserRegistration.WarehouseId = '0';
                    $scope.UserRegistration.DepotId = '0';

                    $scope.UserRegistration.GPId = '0';
                    $scope.UserRegistration.VillageId = '0';
                    $scope.UserRegistration.UserTitle = '0';
                    $scope.UserRegistration.RoleID = '0';
                    $scope.UserRegistration.DESIGNATIONCODE = '0';
                    $scope.UserRegistration.OrganisationId = '0';
                    $scope.UserRegistration.ActId = '0';
                    $scope.UserRegistration.OfficeId = '0';
                    $scope.UserRegistration.DesignationLevelMappingId = '0';
                    $scope.UserRegistration.LevelId = '0';
                    $scope.DepartmentList();
                    // $scope.UserList();
                    $scope.LevelList();
                }
                else
                    alert("Data Could Not Be Retrieved From The Server Please Try After Some Time");
            }).
            error(function (data, status) {
                alert("Error while fetching Role list");
            });
    };
    // Event for Page load
    $scope.ROList = [];
    $scope.GetRAJFEDRO = function () {
        $scope.ROList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=RAJFEDRO&MasterCode=0").
            success(function (data, status) {
                $scope.ROList = data;
            }).
            error(function (data, status) {
                // alert("Error while fetching Post list")

            });
    }

    $scope.OnPageLoad = function () {
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
        $scope.Divisions = [{}];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
            success(function (data, status) {
                $scope.Divisions = data;
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });

        //$scope.Titles = [];
        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Title&MasterCode=-1").
        //    success(function (data, status) {
        //        $scope.Titles = data;
        //    }).
        //    error(function (data, status) {
        //        $scope.Titles = [];
        //    });

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function (data, status) {
                //  $scope.ActList = data;
                $.each(data, function () {
                    $("#ddlAct").append($("<option></option>").val(this.ActId).html(this.ActNameEn));
                });
                $('#ddlAct').multiselect({
                    includeSelectAllOption: true
                });
            }).
            error(function (data, status) {
                alert("Error while fetching Act list.");
                // $scope.showLoader = false;
            });


        $scope.GetRAJFEDRO();
        $scope.GetCurrentUserId();
        $scope.SSOUserDetails();
        $scope.UnitList();
        $scope.DistrictList();
        $scope.GetSocietyTypeList();
        $scope.PostByDesignation();
        $scope.GetAllSubUnitList();
    };

    // START : Get List of PLDBList
    $scope.PLDBListList = function () {

        $scope.PLDBList = [{}];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=" + $scope.UserRegistration.DistrictId).
            success(function (data, status) {
                $scope.PLDBList = data;
            }).
            error(function (data, status) {
                $scope.PLDBList = [{}];
            });
    };
    // END : Get List of PLDBList

    $scope.LeveldataonUrban = function (item) {
        if (item == '2') {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.UserRegistration.WorkScopeLevel_ID = '0';
            $scope.UserRegistration.DIVISION_ID = '0';
            $scope.UserRegistration.DistrictId = '0';
            $scope.UserRegistration.BLOCK_ID = '0';
            $scope.UserRegistration.GP_ID = '0';
            $scope.UserRegistration.VILLAGE_ID = '0';
        }
        else if (item == '1') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
            $scope.UserRegistration.WorkScopeLevel_ID = '0';
            $scope.UserRegistration.CityTypes = '0';
            $scope.UserRegistration.CITY_ID = '0';
            $scope.UserRegistration.WARD_ID = '0';
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.UserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.Blocks = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }

    }

    //start Level of office data
    $scope.Levelofficedata = function () {
        if ($scope.UserRegistration.WorkScopeLevel_ID == '1') {
            $scope.RuralUrbanRedioDiv = false;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
            $scope.IsDistrict = false;
            $scope.IsDivision = false;
            $scope.StateDiv = true;
            $scope.UserRegistration.GP_ID = '0';
            $scope.UserRegistration.VILLAGE_ID = '0';
            $scope.UserRegistration.BLOCK_ID = '0';
            $scope.UserRegistration.CITY_ID = '0';
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];
        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '2') {
            $scope.RuralUrbanRedioDiv = false;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
            $scope.IsDistrict = false;
            $scope.IsDivision = true;
            $scope.UserRegistration.GP_ID = '0';
            $scope.UserRegistration.VILLAGE_ID = '0';
            $scope.UserRegistration.BLOCK_ID = '0';
            $scope.UserRegistration.CITY_ID = '0';
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];

        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '3') {
            $scope.RuralUrbanRedioDiv = false;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
            $scope.IsDistrict = true;
            $scope.IsDivision = false;
            $scope.RuralUrbanRedioDiv = false;
            $scope.UserRegistration.GP_ID = '0';
            $scope.UserRegistration.VILLAGE_ID = '0';
            $scope.UserRegistration.BLOCK_ID = '0';
            $scope.UserRegistration.CITY_ID = '0';
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];
        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '4') {
            $scope.RuralUrbanRedioDiv = true;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.IsDistrict = true;
            //$scope.IsDivision = true;
            $scope.IsDivision = false;
            $scope.UserRegistration.IsRuralUrban = 'false';
            $scope.UserRegistration.GP_ID = '0';
            $scope.UserRegistration.VILLAGE_ID = '0';
            $scope.UserRegistration.CITY_ID = '0';
            $scope.UserRegistration.BLOCK_ID = '0';
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];
        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '5') {
            $scope.RuralUrbanRedioDiv = true;
            $scope.IsDistrict = true;
            //$scope.IsDivision = true;
            $scope.IsDivision = false;
            $scope.RuralurbanDiv = true;
            $scope.Addmore = true;
            $scope.UserRegistration.IsRuralUrban = 'false';
            $scope.RuralUrbanBlock();
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];
        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '6') {
            $scope.RuralUrbanRedioDiv = true;
            $scope.IsDistrict = true;
            //$scope.IsDivision = true;
            $scope.IsDivision = false;
            $scope.RuralurbanDiv = true;
            $scope.IsBlock = true;
            $scope.UserRegistration.IsRuralUrban = 'false';
            $scope.RuralUrbanBlock();
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];
        }
        else {
            $scope.StateDiv = false;
            $scope.DivisionDiv = false;
            $scope.IsDistrict = false;
            $scope.CitytypDiv = false;
        }
    };
    //End Level of office data


    //Get Post List for dropdown
    $scope.PostByDesignation = function () {
        // $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=" + $scope.UserRegistration.DESIGNATIONCODE).
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=10").
            success(function (data, status) {
                $scope.Posts = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list");
                $scope.showLoader = false;
            });
        // END : Post List for dropdown

    };

    //START : Get City List
    $scope.GetCityList = function () {
        $scope.ResionIDs = "";
        $scope.UserRegistration.OfficeId = '0';
        $scope.ResionList = [];
        $scope.IsResion = false;
        if ($scope.UserRegistration.DistrictId != '0') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITY&MasterCode=" + $scope.UserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.CityList = data;
                }).
                error(function (data, status) {
                    $scope.CityList = [];
                });
            //}
            //if ($scope.UserRegistration.IsRuralUrban == 'false') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.UserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.Blocks = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });


            $scope.ProcessingUnitList = [];
            $scope.DEPORTList = [];
            $scope.WAREHOUSEList = [];

            if ($scope.UserRegistration.OrganisationId == 11) {
                $scope.PLDBListList();
            }

            if ($scope.UserRegistration.OrganisationId == 10) {
                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLANT&MasterCode=" + $scope.UserRegistration.DistrictId).
                    success(function (data, status) {
                        $scope.ProcessingUnitList = data;
                    }).
                    error(function (data, status) {
                        $scope.ProcessingUnitList = [];
                    });

                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DEPORT&MasterCode=" + $scope.UserRegistration.DistrictId).
                    success(function (data, status) {
                        $scope.DEPORTList = data;
                    }).
                    error(function (data, status) {
                        $scope.DEPORTList = [];
                    });
            }


            if ($scope.UserRegistration.OrganisationId == 17) {

                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WAREHOUSE&MasterCode=" + $scope.UserRegistration.DistrictId).
                    success(function (data, status) {
                        $scope.WAREHOUSEList = data;
                    }).
                    error(function (data, status) {
                        $scope.WAREHOUSEList = [];
                    });
            }
        }
        $scope.GramPanchayats = [];
    };
    $scope.flag = 0;
    //START : Get ResionMapping LIST
    $scope.GetResionMapping = function () {
        //alert($scope.UserRegistration.DistrictId);
        $scope.ResionIDs = "";
        $scope.ResionList = [];
        if ($scope.UserRegistration.OfficeId == 3 || $scope.UserRegistration.OfficeId == 41 || $scope.UserRegistration.OfficeId == 19) {  //
            if ($scope.UserRegistration.DistrictId != '0') {
                $scope.IsResion = true;
                // alert($scope.UserRegistration.DistrictId);
                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ResionMapping&MasterCode=" + $scope.UserRegistration.DistrictId).
                    success(function (data, status) {
                        $(".se-pre-con").fadeIn("slow");
                        $scope.IsResion = true;
                        data = data.filter(function (x) { return x.OrganisationId == $scope.UserRegistration.OrganisationId });
                        $("#ddlResion").empty();
                        //alert($scope.UserRegistration.OrganisationId);
                        $.each(data, function () {
                            $("#ddlResion").append($("<option></option>").val(this.ResionId).html(this.ResionName));
                        });
                        if ($scope.flag == 1) {
                            $('#ddlResion').multiselect('destroy');
                            $(".se-pre-con").fadeOut("slow");
                        }
                        $('#ddlResion').multiselect({
                            includeSelectAllOption: true
                        });
                        $scope.flag = 1;
                        $(".se-pre-con").fadeOut("slow");
                    }).
                    error(function (data, status) {
                        $scope.ResionList = [];
                        $scope.IsResion = false;
                    });
            }
            else {
                $scope.IsResion = false;
            }
        }
        else {
            $scope.IsResion = false;
        }
    };

    //Get Role List for dropdown
    $scope.UserRoleProfile = function () {
        $scope.Roles = [];
        $http.get(SiteUrl + "/User/SelectRole/?LevelId=" + $scope.UserRegistration.WorkScopeLevel_ID).
            success(function (data, status) {
                $scope.Roles = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Role list");
            });
    };
    //END
    // START : Get Ward List
    //$scope.GetWardListbySociety = function () {
    //    $scope.Ward = [];

    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ward&MasterCode=" + $scope.UserRegistration.CityTypes).
    //        success(function (data, status) {
    //            $scope.Ward = data;
    //        }).
    //        error(function (data, status) {
    //            $scope.Ward = [];
    //        });
    //};


    $scope.GetAllSubOrganisationList = function () {
        $scope.Level = [];
        $http.get(SiteUrl + "/Master/OrganisationsList/?DepartmentId=" + $scope.UserRegistration.DEPTCODE).
            success(function (data, status) {
                $scope.OrganisationData = data;
                if (data.length > 0) {
                    if (data.filter(function (x) { return x.OrganisationNameEn == 'COOPERATIVE' }).length > 0) {
                        var OrgCode = data.filter(function (x) { return x.OrganisationNameEn == 'COOPERATIVE' })[0].OrganisationId;
                        if (OrgCode != undefined && OrgCode != '' && OrgCode != null) {
                            $scope.UserRegistration.OrganisationId = OrgCode;
                            $scope.LevelList();
                        }
                        else {
                            $scope.UserRegistration.OrganisationId = '0';
                        }
                    }
                }
            }).
            error(function (data1, status) {

            });
    };

    $scope.OnPageLoad();
    $scope.ActList();
    //  $scope.UserList();
    $scope.RoleList();
});
