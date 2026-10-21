var UserRegistrationModule = angular.module('UserRquestMod', ['angularUtils.directives.dirPagination'])

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
UserRegistrationModule.controller("UserRequestCnt", function ($scope, $http) {
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
    $scope.ShowGrid = false;
    $scope.UserRegistration = [];

    $scope.gccData = -1;



    // START : Get List of all users
    $scope.UserList = function () {
        $http.post(SiteUrl + "/Login/ListUsers", { WhereClause: " and SSO_ID ='" + $scope.UserRegistration.SSOID + "'" }).
            success(function (data, status) {
                $scope.Users = data;
                if ($scope.Users.length > 0) {
                    $scope.UserRegistration.USERID = $scope.Users[0].UserId;
                }
                else {
                    $scope.UserRegistration.USERID = 0;
                }
                var Usersnew = data.filter(function (x) { return x.UserRoleDeptMapping_IsActive == 3 });//data;
                if (Usersnew.length > 0) { //  + "--------" + $scope.Users[0].IsActive
                    $("#BtnSubmit").hide();
                    alert("Request of " + Usersnew[0].OfficerNameEn + "(" + $scope.UserRegistration.SSOID + ") is already in pending state.");
                    //$scope.GetAllSubUnitList()
                }
                $scope.GetAllSubUnitList()
            }).
            error(function (data, status) {
                alert("Error while user insert");
            });
    };
    // END : Get List of all users
    //validateAddForm 
    
    $scope.FormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';


        if ($scope.UserRegistration.SSOID == undefined) {
            message += $scope._language == 'en-us' ? '\n<br />Please Enter SSO ID.' : '\n<br />कृपया एसएसओ आईडी दर्ज करें |';
            message += '<br />';
        }
        if ($scope.UserRegistration.EmailId == undefined) {
            message += $scope._language == 'en-us' ? '\n<br />Please Enter EmailId' : '\n<br />कृपया ईमेल आईडी दर्ज करें |';
            message += '<br />';
        }
        if ($scope.UserRegistration.OfficerNameEn == undefined) {
            message += $scope._language == 'en-us' ? '\n<br />Please Enter Name' : '\n<br />कृपया नाम दर्ज करे |';
            message += '<br />';
        }
        else if ($scope.UserRegistration.OfficerNameEn.indexOf("@") > -1 || $scope.UserRegistration.OfficerNameEn.indexOf(".") > -1) {
            message += $scope._language == 'en-us' ? '\n<br />Please Enter Proper Officer Name' : '\n<br />कृपया सही अधिकारी नाम दर्ज करे |';
            message += '<br />';
        }
        if ($scope.UserRegistration.MobileNo == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Mobile Name' : '\n <br /> कृपया मोबाइल नंबर दर्ज करें |';
        }
        if ($scope.UserRegistration.DesignationEn == 0) {
            message += $scope._language == 'en-us' ? '\n<br />Please Enter Designation' : '\n<br />कृपया पद का नाम दर्ज करें |';
        }
        if ($scope.UserRegistration.DepartmentEn == 0) {
            message += $scope._language == 'en-us' ? '\n<br />Please Enter Department Name' : '\n<br />कृपया विभाग का नाम दर्ज करें |';
        }

        if ($scope.UserRegistration.OrganisationId == 0 || $scope.UserRegistration.OrganisationId == undefined) {
            message += $scope._language == 'en-us' ? '\n<br />Please Select Organisation' : '\n<br />कृपया संगठन का चयन करे  |';
        }
        //if ($scope.UserRegistration.ActId == 0 || $scope.UserRegistration.ActId == undefined) {
        //    message += $scope._language == 'en-us' ? '\n<br />Please Select Act' : '\n<br />कृपया अधिनियम का चयन करे  |';
        //}
        if (($scope.UserRegistration.WorkScopeLevel_ID == 0 || $scope.UserRegistration.WorkScopeLevel_ID == undefined) && $scope.gccData == -1) {
            message += $scope._language == 'en-us' ? '\n<br />Please Select Work Scope Level' : '\n<br />कृपया कार्य स्कोप स्तर का चयन करे |';
        }


        if ($scope.UserRegistration.WorkScopeLevel_ID != undefined && $scope.UserRegistration.WorkScopeLevel_ID == 2) {
            if ($scope.UserRegistration.DivisionId == undefined || $scope.UserRegistration.DivisionId == 0) {
                message += $scope._language == 'en-us' ? '\n<br />Please Select Division' : '\n<br />कृपया संभाग का चयन करे |';
            }
        }

        if ($scope.UserRegistration.WorkScopeLevel_ID != undefined && ($scope.UserRegistration.WorkScopeLevel_ID > 2) && $scope.gccData==-1) {
            if ($scope.UserRegistration.DistrictId != undefined && $scope.UserRegistration.DistrictId == 0) {
                message += $scope._language == 'en-us' ? '\n<br />Please Select District' : '\nकृपया जिले का चयन करे |';
            }
        }

        if ($scope.UserRegistration.WorkScopeLevel_ID != undefined && $scope.UserRegistration.WorkScopeLevel_ID > 3 && $scope.gccData == -1) {
            if ($scope.UserRegistration.IsRuralUrban != undefined && ($scope.UserRegistration.IsRuralUrban == true || $scope.UserRegistration.IsRuralUrban == 'true')) {
                if ($scope.UserRegistration.CITY_ID != undefined && $scope.UserRegistration.CITY_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n<br />Please Select City' : '\nकृपया शहर का चयन करे |';
                }
            }
            else if ($scope.UserRegistration.IsRuralUrban != undefined && ($scope.UserRegistration.IsRuralUrban == false || $scope.UserRegistration.IsRuralUrban == 'false') && $scope.gccData == -1) {
                if ($scope.UserRegistration.BlockId != undefined && $scope.UserRegistration.BlockId == 0 && $scope.UserRegistration.OrganisationId != 11) {
                    message += $scope._language == 'en-us' ? '\n<br />Please Select Block' : '\nकृपया खंड का चयन करे |';
                }
            }
        }
        if ($scope.UserRegistration.WorkScopeLevel_ID != undefined && $scope.UserRegistration.WorkScopeLevel_ID > 4 && $scope.gccData == -1) {
            if ($scope.UserRegistration.GPId != undefined && $scope.UserRegistration.GPId == 0) {
                message += $scope._language == 'en-us' ? '\n<br />Please Select Gram Panchayat' : '\nकृपया ग्राम पंचायत का चयन करे |';
            }
        }

        if ($scope.UserRegistration.WorkScopeLevel_ID != undefined && $scope.UserRegistration.WorkScopeLevel_ID > 5 && $scope.gccData == -1) {
            if ($scope.UserRegistration.VillageId != undefined && $scope.UserRegistration.VillageId == 0) {
                message += $scope._language == 'en-us' ? '\n<br />Please Select Village' : '\nकृपया ग्राम का चयन करे |';
            }
        }
        if ($scope.UserRegistration.DEPTCODE == 0 || $scope.UserRegistration.DEPTCODE == undefined) {
            message += $scope._language == 'en-us' ? '\n<br />Please Select Department' : '\n<br />कृपया कार्यालय का चयन करे |';
        }
        if ($scope.UserRegistration.DesignationLevelMappingId == 0 || $scope.UserRegistration.DesignationLevelMappingId == undefined) {
            message += $scope._language == 'en-us' ? '\n<br />Please Select Designation' : '\n<br />कृपया पद का चयन करे |';
        }
        if ($scope.UserRegistration.OfficeId == 0 || $scope.UserRegistration.OfficeId == undefined) {
            message += $scope._language == 'en-us' ? '\n<br />Please Select Office' : '\n<br />कृपया कार्यालय का चयन करे |';
        }



        if ($scope.UserRegistration.WorkScopeLevel_ID != 0 || $scope.UserRegistration.WorkScopeLevel_ID != undefined || $scope.UserRegistration.WorkScopeLevel_ID > 6) {

            if ($scope.UserRegistration.WorkScopeLevel_ID >= 7 && ($scope.GopalCreditCardModel.LoanBankID == undefined || $scope.GopalCreditCardModel.LoanBankID == null || $scope.GopalCreditCardModel.LoanBankID == 0))
                message += $scope._language == 'en-us' ? '\n<br />Please Select Bank' : '\n<br />कृपया बैंक का चयन करें |';

            if ($scope.UserRegistration.WorkScopeLevel_ID >= 8 && ($scope.GopalCreditCardModel.LoanBranchId == undefined || $scope.GopalCreditCardModel.LoanBranchId == null || $scope.GopalCreditCardModel.LoanBranchId == 0))
                message += $scope._language == 'en-us' ? '\n<br />Please Select Bank Banch' : '\n<br />कृपया बैंक शाखा का चयन करें |';


            if ($scope.UserRegistration.WorkScopeLevel_ID == 9 && ($scope.GopalCreditCardModel.PACS_ID == undefined || $scope.GopalCreditCardModel.PACS_ID == null || $scope.GopalCreditCardModel.PACS_ID == 0))
                message += $scope._language == 'en-us' ? '\n<br />Please Select PACS Name' : '\n<br />कृपया पैक्स नाम का चयन करें |';

        }
        //remove workscope
        //if ($scope.UserRegistration.OfficeId == 3 || $scope.UserRegistration.OfficeId == '3' || $scope.UserRegistration.OfficeId == 41 || $scope.UserRegistration.OfficeId == '41') {
        //    var ddlResionVal = "";
        //    var selected = $("#ddlResion option:selected");
        //    selected.each(function () {
        //        ddlResionVal += $(this).val() + ",";
        //    });
        //    if (ddlResionVal == "") {
        //        message += $scope._language == 'en-us' ? '\n<br />Please Select Work Scope' : '\n<br />कृपया कार्य स्कोप का चयन करे |';
        //    }
        //}

        if (message.length > 29) {
            ShowModelAlert("User Profiling", message);
            return false;
        }
        return true;

    }
    // START : Save/Edit User



    $scope.SaveUser = function () {
        if ($scope.FormValidation()) {
            var formData = new FormData();
            RoleTempList = [];
            $scope.ActIds = ""
            // alert(ActIds);$scope.WorkActId ='';
            var selectedAct = $("#ddlAct option:selected");
            selectedAct.each(function () {
                $scope.ActIds += $(this).val() + ",";
            });

            var ApexGccMappingId = 0;
            if ($scope.gccData > 0) {
                if ($scope.gccData == 3) {
                    ApexGccMappingId = $scope.GopalCreditCardModel.PACS_ID ?? $('PacsId').val();
                } else if ($scope.gccData == 2) {
                    ApexGccMappingId = $scope.GopalCreditCardModel.LoanBranchId ?? $('BranchId').val();
                }
                else {
                    ApexGccMappingId = $scope.GopalCreditCardModel.LoanBankID ?? $('BankID').val();
                }

            }
            //if ($scop.UserRegistration.OfficerNameEn == undefined || $scop.UserRegistration.OfficerNameEn == '' || $scop.UserRegistration.OfficerNameEn == null || $scop.UserRegistration.OfficerNameEn.length < 4)
            //{               
            //    alert($scope._language == 'en-us' ? "Please Enter a valid user Name" :"कृपया मान्य उपयोगकर्ता नाम दर्ज करें|");
            //    return;
            //}

            $scope.ResionIDs = "";
            //Remove workcope 29April2024
            //var selected = $("#ddlResion option:selected");
            //selected.each(function () {
            //    $scope.ResionIDs += $(this).val() + ",";
            //});
            RoleTempList.push({
                "DepartmentName": $scope.UserRegistration.DepartmentEn,
                "UnitId": $scope.UserRegistration.OrganisationId,
                "LevelId": $scope.UserRegistration.WorkScopeLevel_ID,
                "DesignationId": $scope.UserRegistration.DESIGNATIONCODE,
                "PostId": 0,
                "RoleId": $scope.UserRegistration.RoleID,
                "DivisionId": $scope.UserRegistration.DivisionId,
                "IsRural": $scope.UserRegistration.IsRuralUrban,
                "DistrictId": $scope.UserRegistration.DistrictId,
                "TehsilId": $scope.UserRegistration.BlockId,
                "PanchayatSamitiId": $scope.UserRegistration.BlockId,
                "CityId": $scope.UserRegistration.CITY_ID,
                "WardId": 0,
                "GramPanchayatId": $scope.UserRegistration.GPId,
                "DesignationLevelMappingId": $scope.UserRegistration.DesignationLevelMappingId,
                "OrganizationLevelsMappingID": $scope.UserRegistration.OrganizationLevelsMappingID,
                "OfficeId": $scope.UserRegistration.OfficeId,
                "ProcessingUnitId": $scope.UserRegistration.ProcessingUnitId,
                "DepotId": $scope.UserRegistration.DepotId,
                "WarehouseId": $scope.UserRegistration.WarehouseId,
                "VillageId": $scope.UserRegistration.VillageId,
                "PLDBId": $scope.UserRegistration.PLDBId,
                "PLDBBrancchId": $scope.UserRegistration.PLDBBrancchId,
                "IsActive": $("#chkIsActive").val = 3,
                "ApexGccMappingId": ApexGccMappingId,//madhuri
            });
            formData.append("USERID", $scope.UserRegistration.USERID);
            formData.append("UserTitle", $scope.UserRegistration.UserTitle = 1);
            formData.append("OfficerNameEn", $scope.UserRegistration.OfficerNameEn);
            formData.append("OfficerNameHi", $scope.UserRegistration.OfficerNameHi);
            formData.append("Gender", $scope.UserRegistration.Gender);
            formData.append("MobileNo", $scope.UserRegistration.MobileNo);
            formData.append("SSOID", $scope.UserRegistration.SSOID);
            formData.append("EmailId", $scope.UserRegistration.EmailId);
            formData.append("ImageUrl", $scope.UserRegistration.ImageUrl);
            formData.append("DivisionId", $scope.UserRegistration.DivisionId);
            formData.append("DistrictId", $scope.UserRegistration.DistrictId);
            formData.append("IsRuralUrban", $scope.UserRegistration.IsRuralUrban);
            formData.append("TehsilId", $scope.UserRegistration.TehsilId);
            formData.append("CityId", $scope.UserRegistration.CityId);
            formData.append("WardId", $scope.UserRegistration.WardId);
            formData.append("BlockId", $scope.UserRegistration.BlockId);
            formData.append("GPId", $scope.UserRegistration.GPId);
            formData.append("VillageId", $scope.UserRegistration.VillageId);
            formData.append("AddressEn", $scope.UserRegistration.AddressEn);
            formData.append("AddressHi", $scope.UserRegistration.AddressHi);
            formData.append("PIN", $scope.UserRegistration.PIN);
            formData.append("IsActive", $scope.UserRegistration.IsActive = 3);
            formData.append("RoleArray", JSON.stringify(RoleTempList));
            formData.append("WorkScopResionIds", $scope.ResionIDs); //$scope.UserRegistration.ResionId
            formData.append("ActId", $scope.ActIds); //
            //$("select").prop('selectedIndex', 0);
            $http.post(SiteUrl + "/Login/SaveUserRequest", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    if (data.data > 0) {
                        ShowModelAlert("User Registration", "Request Received Successfully");
                    }
                    else if (data == 0) {
                        ShowModelAlert("User Registration", "Request Already Received");
                    }
                    else if (data < 0) {
                        ShowModelAlert("User Registration", "Error In User Request");
                    }
                    $scope.ClearForm();
                    $scope.GetAllSubUnitList();
                }).
                error(function (data, status) {
                    LoaderStop();
                    ShowModelAlert("Registration Request", "Error While Insert Request");
                });
        }

    };


    $scope.GetAllSubUnitList = function () {
        $scope.ShowGrid = false;
        $http.get(SiteUrl + "/Login/UserSelfReqList?SSOID=" + $scope.UserRegistration.SSOID).
            success(function (data, status) {
                $scope.UserReqData = data;
                if (data.length > 0) {
                    $scope.OrganisationId = $scope.UserReqData[0].OrganisationId;
                    $scope.ShowGrid = true;
                }
                $(".se-pre-con").fadeOut("slow");

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
    };

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
                $scope.PLDBListList();

                $scope.UserRegistration.DistrictId = data[0].DistrictId;
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
        $scope.UserRegistration.ProcessingUnitId = '0';
        $scope.UserRegistration.WarehouseId = '0';
        $scope.UserRegistration.DepotId = '0';
        $scope.UserRegistration.GPId = '0';
        $scope.UserRegistration.VillageId = '0';
        $scope.UserRegistration.UserTitle = '0';
        $scope.UserRegistration.DEPTCODE = '0';
        $scope.UserRegistration.RoleID = '0';
        $scope.UserRegistration.DESIGNATIONCODE = '0';
        $scope.UserRegistration.OfficerNameEn = '';
        $scope.UserRegistration.OfficerNameHi = '';
        $scope.UserRegistration.PhoneNo = '';
        $scope.UserRegistration.MobileNo = '';
        $scope.UserRegistration.EmailId = '';
        $scope.UserRegistration.AddressEn = '';
        $scope.UserRegistration.AddressHi = '';
        // $scope.UserRegistration.Gender = '0';
        $scope.UserRegistration.PIN = '';
        $scope.UserRegistration.DepartmentEn = '';
        $scope.UserRegistration.DesignationEn = '';
        $scope.UserRegistration.MobileNo = '';
        $scope.UserRegistration.EmailId = '';
        $scope.UserRegistration.SSOID = '';
        $scope.UserRegistration.OfficerNameEn = '';
        $scope.Districts = [];
        $scope.PLDBList = [];
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
                $scope.Department = data;
                if (data.length > 0) {
                    var DeoptCode = data.filter(function (x) { return x.DEPTNAMEENGLISH == 'COOPERATIVE' })[0].ID;
                    if (DeoptCode != undefined && DeoptCode != '' && DeoptCode != null) {
                        $scope.UserRegistration.DEPTCODE = DeoptCode;
                        $scope.GetAllSubOrganisationList();
                    }
                    else {
                        $scope.UserRegistration.DEPTCODE = '0';
                    }
                }
            }).
            error(function (data, status) {
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
    //$scope.DesignationList = function () {
    //    if ($scope.gccData > 0) {


    //        $scope.Designations = [
    //            { DesignationLevelMappingId: 96, DESIGNATIONDESCENGLISH: 'EXECUTIVE OFFICER (E O)' },
    //            { DesignationLevelMappingId: 90, DESIGNATIONDESCENGLISH: 'Managing Director' }              
    //        ];

    //    } else {
    //        $scope.Designations = [];
    //        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=designation&MasterCode=" + $scope.UserRegistration.DEPTCODE).
    //            success(function (data, status) {
    //                $scope.Designations = data;
    //            }).
    //            error(function (data, status) {
    //                $scope.Designations = [];
    //            });
    //    }



    //};

    //Get Designation List for dropdown

    $scope.OfficeList = function () {
        $scope.OfficeListData = [];
        $http.get(SiteUrl + "/Master/OfficesList/?WhereClause=and o.isactive =1 and   o.OrganizationLevelsMappingID=" + $scope.UserRegistration.OrganizationLevelsMappingID).
            success(function (data, status) {
                $scope.OfficeListData = data;
            }).
            error(function (data, status) {
                alert("Error while fetching designation list");
                $scope.OfficeListData = [];
                $scope.showLoader = false;
            });
    };

    // Language Selector
    $scope.RoleList = function () {
        ////$http.get(SiteUrl + "/User/SelectRole").        
        //$http.get(SiteUrl + "/Master/SelectRole/?LevelId=" + $scope.UserRegistration.WorkScopeLevel_ID).
        //    success(function (data, status) {
        //        $scope.Roles = data;
        //        $scope.showLoader = false;
        //        //$('#ddlResion').multiselect({
        //        //    includeSelectAllOption: true
        //        //});
        //    }).
        //    error(function (data, status) {
        //        alert("Error while fetching Role list");
        //        $scope.showLoader = false;
        //    });

        //function GetDropDownData() {
        //    $.ajax({
        //        type: "POST",
        //        url: SiteUrl + "/Master/SelectRole",
        //        data: '{LevelId: "' + $scope.UserRegistration.WorkScopeLevel_ID + '" }',
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (data.d) {
        //            $.each(data.d, function () {
        //                $(".ddlResion").append($("<option     />").val(this.KeyName).text(this.ValueName));
        //            });
        //        },
        //        failure: function () {
        //            alert("Failed!");
        //        }
        //    });
        //}

    };



    // START : Get List of Districts
    $scope.DistrictList = function () {
        $scope.Districts = [{}];

        // $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=-1" + $scope.UserRegistration.DivisionId).
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=-1").
            success(function (data, status) {
                $scope.Districts = data;
            }).
            error(function (data, status) {
                $scope.Districts = [{}];
            });
    };
    // END : Get List of Districts

    // START : Get List of PLDBList
    $scope.PLDBListList = function () {
        $scope.PLDBList = [{}];

        // $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=-1" + $scope.UserRegistration.DivisionId).
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=-1").
            success(function (data, status) {
                $scope.PLDBList = data;
            }).
            error(function (data, status) {
                $scope.PLDBList = [{}];
            });
    };
    // END : Get List of PLDBList
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
        $scope.Level = [];
        $http.get(SiteUrl + "/MasterSettings/OrganizationLevelsList/?WhereClause=AND OLM.OrganizationId=" + $scope.UserRegistration.OrganisationId).
            success(function (data, status) {
                $scope.Level = data;
            }).
            error(function (data, status) {
            });
    };
    //End 
    //Start: Get Unit List
    $scope.GetValuesForDesignationList = function () {
      //  var selectedIndex = $('#ddllevel').prop('selectedIndex');
        //var selectedText = $('#ddllevel option:selected').text();
        //var replacedText = selectedText.trim().replace(/ /g, '');
        var arrayOfStrings = $scope.UserRegistration.LevelId.split("~");
        var level = arrayOfStrings[0];
        
        if (level == 7 || level == 8 || level == 9) {
            if (level == 7) { $scope.gccData = 1 } else if (level == 8) { $scope.gccData = 2 } else { $scope.gccData = 3 }

      
        //if (replacedText == 'CCB_Data' || replacedText == 'CCB_Branches' || replacedText == 'CCB_Branch_PACS') {
        //    if (replacedText == 'CCB_Data') { $scope.gccData = 1 } else if (replacedText == 'CCB_Branches') { $scope.gccData = 2 } else { $scope.gccData = 3 }


            $scope.IsDistrict = false;
            $scope.IsDivision = false;
            $scope.IsResion = false;
            $scope.IsRural = false;
            $scope.IsRuralUrban = false;
            $scope.Designations = [];
            $scope.UserRegistration.DistrictId = '0';
            $scope.UserRegistration.IsRuralUrban = false;
            $scope.UserRegistration.TehsilId = '0';
            $scope.UserRegistration.CityId = '0';
            $scope.UserRegistration.WardId = '0';
            $scope.UserRegistration.BlockId = '0';
            $scope.UserRegistration.ProcessingUnitId = '0';
            $scope.UserRegistration.WarehouseId = '0';
            $scope.UserRegistration.DepotId = '0';            
             
            $scope.GopalCreditCardModel.PACS_ID = '0';          
            $scope.GopalCreditCardModel.LoanBranchId = '0';                
            $scope.GopalCreditCardModel.LoanBankID = '0';
         
            $scope.ProcessingUnitList = [];
            $scope.DEPORTList = [];
            $scope.PLDBBranches = [];
            $scope.WAREHOUSEList = [];
            $scope.PLDBBranches = [];
            $scope.PACSNameList = [];
            $scope.BankMasterList = [];
           
            

            var Level_OrganizationLevelsMappingID = $scope.UserRegistration.LevelId;
            if (Level_OrganizationLevelsMappingID.indexOf("~") >= 0) {
                var arrayOfStrings = Level_OrganizationLevelsMappingID.split("~");
                $scope.UserRegistration.OrganizationLevelsMappingID = arrayOfStrings[1];
                $scope.UserRegistration.WorkScopeLevel_ID = arrayOfStrings[0];
                $scope.OfficeList();
            }
            else {
                $scope.UserRegistration.OrganizationLevelsMappingID = 0;
                $scope.UserRegistration.WorkScopeLevel_ID = 0;
            }

           
            $scope.LoanBankList();
            $scope.GetDesignationList();
        }
        else {
            $scope.gccData = -1;
            $scope.Designations = [];
            $scope.UserRegistration.DistrictId = '0';
            $scope.UserRegistration.IsRuralUrban = false;
            $scope.UserRegistration.TehsilId = '0';
            $scope.UserRegistration.CityId = '0';
            $scope.UserRegistration.WardId = '0';
            $scope.UserRegistration.BlockId = '0';

            $scope.UserRegistration.ProcessingUnitId = '0';
            $scope.UserRegistration.WarehouseId = '0';
            $scope.UserRegistration.DepotId = '0';
            $scope.ProcessingUnitList = [];
            $scope.DEPORTList = [];
            $scope.PLDBBranches = [];
            $scope.WAREHOUSEList = [];

            var Level_OrganizationLevelsMappingID = $scope.UserRegistration.LevelId;
            if (Level_OrganizationLevelsMappingID.indexOf("~") >= 0) {
                var arrayOfStrings = Level_OrganizationLevelsMappingID.split("~");
                $scope.UserRegistration.OrganizationLevelsMappingID = arrayOfStrings[1];
                $scope.UserRegistration.WorkScopeLevel_ID = arrayOfStrings[0];
            }
            else {
                $scope.UserRegistration.OrganizationLevelsMappingID = 0;
                $scope.UserRegistration.WorkScopeLevel_ID = 0;
            }
            $scope.Levelofficedata();
            $scope.RoleList();
            $scope.OfficeList();
        }



    };
    $scope.GetDesignationList = function () {
        $scope.Designations = [];


        $http.get(SiteUrl + "/MasterSettings/GetAllDesignationByOrganizationLevelsMappingID/?OrganizationLevelsMappingID=" + $scope.UserRegistration.OrganizationLevelsMappingID + "&OfficeId=" + $scope.UserRegistration.OfficeId).
            success(function (data, status) {
                $scope.Designations = data;
            }).
            error(function (data, status) {
                $scope.Designations = [];
            });
    };
    //End 

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
                    $scope.UserList();
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


        $scope.GetRAJFEDRO();



        $scope.Titles = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Title&MasterCode=-1").
            success(function (data, status) {
                $scope.Titles = data;
            }).
            error(function (data, status) {
                $scope.Titles = [];
            });

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
        // Get List of all ACT
        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
        //    success(function (data, status) {
        //        $scope.ActList = data;
        //    }).
        //    error(function (data, status) {
        //        alert("Error while fetching Act list.");
        //        $scope.showLoader = false;
        //    });

        $scope.SSOUserDetails();
        $scope.UnitList();
        $scope.DistrictList();
        $scope.PLDBListList();
        $scope.GetSocietyTypeList();
        $scope.PostByDesignation();

    };

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
            $scope.WAREHOUSEList = [];
            $scope.DEPORTList = [];

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

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WAREHOUSE&MasterCode=" + $scope.UserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.WAREHOUSEList = data;
                }).
                error(function (data, status) {
                    $scope.WAREHOUSEList = [];
                });
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
                        $scope.IsResion = true;
                        data = data.filter(function (x) { return x.OrganisationId == $scope.UserRegistration.OrganisationId });


                        // $("#ddlResion").empty();                        
                        //$.each(data, function () {
                        //    $("#ddlResion").append($("<option></option>").val(this.ResionId).html(this.ResionName));
                        //});
                        //if ($scope.flag == 1) {
                        //    $('#ddlResion').multiselect('destroy');
                        //}
                        //$('#ddlResion').multiselect({
                        //    includeSelectAllOption: true
                        //});
                        $scope.flag = 1;
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


    //Get PLDBBranchList List for dropdown
    $scope.PLDBBranchList = function () {
        $scope.PLDBBranches = [];
        if ($scope.UserRegistration.PLDBId != '0') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.UserRegistration.PLDBId).
                success(function (data, status) {
                    $scope.PLDBBranches = data;
                }).
                error(function (data, status) {
                    $scope.PLDBBranches = [];
                });
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
    $scope.LoanBankList = function () {

        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=" + ($scope.GopalCreditCardModel.LoanDistrictId ?? 0)).
            success(function (data, status) {

                $scope.BankMasterList = data;
            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
            });
    };

    $scope.Get_LoanBankBranch = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?LoanDistrictId=" + ($scope.GopalCreditCardModel.LoanDistrictId ?? 0) + "&LoanBankId=" + ($scope.GopalCreditCardModel.LoanBankID ?? 0)).
            success(function (data, status) {
                $scope.PLDBBranches = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBranch_PACS/?LoanBranchId=" + $scope.GopalCreditCardModel.LoanBranchId).
            success(function (data, status) {
                $scope.PACSNameList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PACSNameList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.OnPageLoad();
});
