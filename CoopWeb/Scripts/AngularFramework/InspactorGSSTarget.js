
var MasterMod = angular.module('UnitMastersModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("UnitMasterCont", function ($scope, $http, $rootScope) {

    // SCOPE

    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 10,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }

    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                //  _currentPageID = 1;
                callback();
            }
            ++indx;
        });


        //if (($scope.SelectPage.filter(x => x === $scope.DataPaging.currentPageID).length > 0)) {
        //    $("#chk_all").prop("checked", true);
        //} else {
        //    $("#chk_all").prop("checked", false);
        //}
    }
    $rootScope.SetDataPaging = function () {
        if ($rootScope.DataPagingDetails.TotalRecords > 0) {
            $rootScope.DataPagingDetails.TotalPages = parseInt(($rootScope.DataPagingDetails.TotalRecords / $rootScope.DataPagingDetails.PageSize)) + (($rootScope.DataPagingDetails.TotalRecords % $rootScope.DataPagingDetails.PageSize == 0) ? 0 : 1);
            $rootScope.DataPagingDetails.StartRecord = ($rootScope.DataPagingDetails.TotalRecords == 0) ? 0 : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + 1);
            $rootScope.DataPagingDetails.EndRecord = ($rootScope.DataPagingDetails.TotalPages == $rootScope.DataPagingDetails.PageNumber || $rootScope.DataPagingDetails.TotalRecords == 0) ? $rootScope.DataPagingDetails.TotalRecords : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + $rootScope.DataPagingDetails.PageSize);


            $rootScope.DataPagingDetails.StartPageNumber = 1;
            $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
            if ($rootScope.DataPagingDetails.TotalPages > 5 && $rootScope.DataPagingDetails.PageNumber > 0) {
                $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.PageNumber == 1 ? 1 : $rootScope.DataPagingDetails.PageNumber - 1;
                $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.StartPageNumber + 4;
                if ($rootScope.DataPagingDetails.EndPageNumber > $rootScope.DataPagingDetails.TotalPages) {
                    $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
                    $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.StartPageNumber > 4 ? $rootScope.DataPagingDetails.EndPageNumber - 4 : $rootScope.DataPagingDetails.StartPageNumber;
                }
            }
        }
        $scope.DataPaging.TotalRecords = $rootScope.DataPagingDetails.TotalRecords
    }
    $rootScope.PagingRange = function (startPage, endPage, totalPages, step) {
        step = step || 1;
        var input = [];

        for (var tStart = startPage; tStart <= endPage && tStart <= totalPages; tStart++) {
            input.push(tStart);
        }
        return input;
    };
    $scope.searchRequest = function (callback) {
        //$scope.DataPaging.searchParameters = [];
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                $scope.DataPaging.currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.CCSId = '0';
        if ($scope.Filter.FilterType == 'created_dated') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();```1    
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }


    $scope._language = 'en-us';
    //  $scope.currentPage = 1;
    //$scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];



    $scope.UserRegistration = {};
    $scope.Level = [];
    $scope.DESIGNATION = [];
    $scope.Districts = [];
    $scope.ROList = [];
    var InspactorId = 0;
    $scope.isEditMode = false;

    $scope.IsShowAssignForm = true;
    $scope.IsShowAssignForm1 = false;

    $scope.SaveUnitForm = function () {
        $http.post(SiteUrl + "/Master/SaveUnitForm", $scope.UnitMaster).
            success(function (data, status) {

                if (data > 0) {
                    alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n')
                        || $scope.ClearUnitForm();
                }
                else {
                    alert(_language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.ClearUnitForm();
                    return;
                }
                if ($scope.ParentId == '') {
                    $scope.ParentId = 0;
                }

                $scope.GetAllSubUnitList();
                if (data.Status) {

                    $scope.UnitMaster = null;
                    $scope.ClearUnitForm();
                    //$scope.GetAllSubClassesList();


                }
            }).
            error(function (data, status) {
                alert("Error while subclass insert");
            });


    };

    // on page load
    $scope.OnPageLoad = function () {
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

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }

        $scope.Department = [];
        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=INSPECTOR&MasterCode=-1").

        //    success(function (data, status) {

        //        $scope.Department = data;
        //    }).
        //    error(function (data, status) {
        //        $scope.Department = [];
        //    });
        $scope.LevelList();
        $scope.GetAuditMonthList();
        var dataObj = {};

        var storedData = sessionStorage.getItem('GSSData');
        if (storedData) {
            sessionStorage.removeItem('GSSData');
            $scope.AssignNew();
            dataObj = JSON.parse(storedData);
            console.log(dataObj);
            $scope.EditForm(dataObj);

        }
        else { $scope.GetInspactorTargetsCount(InspactorId); $scope.DistrictListData(); $scope.LoanBankList(); }

    };


    $scope.dateFormatChange = function (str) {

        var d = new Date(str.replace(/(\d{1,2}) (\d{4})/, '$2-$1')); // unreliable, better to map month

        // Safe version:
        var months = { Jan: 0, Feb: 1, Mar: 2, Apr: 3, May: 4, Jun: 5, Jul: 6, Aug: 7, Sep: 8, Oct: 9, Nov: 10, Dec: 11 };
        var parts = str.split(" "); // ["Sep","23","2025","12:00AM"]
        var dateObj = new Date(parts[2], months[parts[0]], parseInt(parts[1]));


        return dateObj.toISOString().substring(0, 10);
    }
    //$scope.isEditMode  = new Date() ;

    $scope.EditForm = function (itm) {
        debugger;

        $scope.IsTspArea = itm.IsTspArea ? 1 : 0;
        $scope.TspAreaType = $scope.IsTspArea == 1 ? itm.AreaType : 0;
        $scope.NonTspAreaType = $scope.IsTspArea == 0 ? itm.AreaType : 0;
        //  $scope.SurveyDate = itm.SurveyDate;
        $scope.Checkboxvalue = 2;
        $scope.CityId = 0;
        $scope.isEditMode = true;
        $scope.InspactorTargetsId = itm.InspactorTargetsId;
        $scope.IsMahilaMpacs = itm.IsMahilaMpacs;
        $scope.OldInspactorId = itm.Inspector_ID;
        $scope.LoanBankList()
            .then(function () {
                $scope.LoanBankID = itm.LoanBankID.toString(); $scope.Get_LoanBankBranch(0);
            })
            .then(function () {
                $scope.LoanBranchId = itm.LoanBranchId.toString();
            })
            .catch(function (err) { console.error('Error loading data:', err); });


        $scope.GetAuditMonthList().then(function () { $scope.MonthID = itm.MonthId.toString(); })

        $scope.AllotmentLetter = itm.AllotmentLetter;
        $scope.FileName = itm.CompetentAcceptanceLetter;
        $scope.IsUpload = true;
        $scope.AssemblyId = itm.AssemblyId ? itm.AssemblyId.toString() : '0';

        $scope.IsUploadFileName['AllotmentLetter'] = itm.AllotmentLetter && itm.AllotmentLetter.trim() !== '' ? true : false;
        $scope.IsUploadFileName['FileName'] = itm.CompetentAcceptanceLetter && itm.CompetentAcceptanceLetter.trim() !== '' ? true : false;

        $scope.DistrictListData()
            .then(function () {
                $scope.DistrictId = itm.DistictId.toString();
                return $scope.getAssembly();
            })
            .then(function () {
                $scope.AssemblyId = itm.AssemblyId ? itm.AssemblyId.toString() : '0';
                $scope.DistrictId = itm.DistictId.toString();
                return $scope.getblock();
            })
            //$scope.getblock()
            .then(function () {
                $scope.BlockId = itm.Block_Id.toString();
                return $scope.getGrampanchyat();
            })
            .then(function () {
                $scope.PanchayatId = itm.GramPanchayat_ID.toString();
                return $scope.getvillage();
            })
            .then(function () {
                $scope.VillageId = itm.VillageId.toString();
            })
            .catch(function (err) {
                console.error('Error loading data:', err);
            });


        $scope.SurveyDate = new Date($scope.dateFormatChange(itm.SurveyDate));
    }


    $scope.AssignNew = function () {
        $scope.isEditMode = false;
        // _http = $http;
        $scope.IsShowAssignForm = false;
        $scope.IsShowAssignForm1 = true;
        $scope.AllDetails = false;
        $scope.M_PacsClear();
    }


    $scope.InspacorData = [];
    $scope.GetInspactor = function () {
        $(".se-pre-con").fadeIn("slow");
        // $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=INSPECTOR&MasterCode=-1").//previous

        $http.post(SiteUrl + "/Master/InspectorList/?OfficeId=" + $scope.UserRegistration.OfficeId + "&DistrictId=" + $scope.UserRegistration.DistrictId + "&DivisionId=" + $scope.UserRegistration.DivisionId).

            success(function (data, status) {
                $scope.InspacorData = data;
            }).
            error(function (data, status) {
                $scope.AssignedAuditor = [];
                alert('Error while auditor data list fetch.');
            });
        $(".se-pre-con").fadeOut("slow");
    }




    $("#HouseBuildingNo_MemberAdd").parent().hide();
    $("#LaneStreetName_MemberAdd").parent().hide();
    $("#Locality_MemberAdd").parent().hide();
    $("#ddlstate_MemberAdd").parent().hide();
    $("#ddlDivision_MemberAdd").parent().hide();
    $("#ddlDivision_MemberAdd").parent().hide();
    //$("#district_MemberAdd").parent().hide();
    if ($("#hdnDistrictId").val() != "" && $("#hdnDistrictId").val() != "0") {
        $("#district_MemberAdd").attr("disabled", "disabled");
    }
    $("#ddlward_MemberAdd").parent().hide();
    $("#ddlVillage_MemberAdd").parent().hide();
    $("#pincode_MemberAdd").parent().hide();



    // START : Change Status for SMS / delete SMS,Status
    $scope.SetStatus = function (UnitId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangeStatusunit", { Id: UnitId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllSubUnitList();

            }).
            error(function (data, status) {
            });
    };


    $scope.update = function (item) {

        $scope.UnitMaster.DepartmentId = item.departmentId;
        $scope.UnitMaster.UnitNameEn = item.UnitNameEn;
        $scope.UnitMaster.UnitNameHi = item.UnitNameHi;
        $scope.UnitMaster.IsActive = item.IsActive == 1 ? true : false;
        $scope.UnitMaster.UnitId = item.UnitId;
    };


    //$scope.GetInspactor();

    $scope.ischeckdtrue = false;

    $scope.OtherPersonList = [];
    $scope.OtherPersonListNew = [];
    //$scope.OtherPersonList.push({ "PersonId": 0, "AdId": 0, "Name": "", "Address": "", "Post": "", "Mobile": "" });
    //$scope.OtherPersonListNew.push({ "PersonId": 0, "AdId": 0, "Name": "", "Address": "", "Post": "", "Mobile": "" });
    //Submit level to District committee
    $scope.GSSStatusList = [];

    $scope.GSSStatusList.push({ GSSSStatusId: "1", StatusNameEn: "Survey Submitted ", StatusNameHi: "Survey Submitted" })
    $scope.GSSStatusList.push({ GSSSStatusId: "2", StatusNameEn: "Close", StatusNameHi: "Close" })
    $scope.GSSStatusList.push({ GSSSStatusId: "3", StatusNameEn: "District Level Committee Report Submitted", StatusNameHi: "Submit level to District committee" })

    $scope.InspactorId = "0";
    $scope.AddGSS_old = function () {
        if ($scope.ValidateAddGSS()) {
            var data = GetAddressinfo('MemberAdd');
            $scope.GramPanchayatduplicat = false;
            $scope.Cityduplicat = false;
            if (data.Urban_Rural != "Urban") {
                var message1 = ' ';
                $scope.OtherPersonListNew.forEach(function (item, index) {
                    if (item.GramPanchayat_ID == data.PanchayatId) {
                        $scope.GramPanchayatduplicat = true;
                    }
                });
            }
            else {
                var message1 = ' ';
                $scope.OtherPersonListNew.forEach(function (item, index) {
                    if (item.City_Block_ID == data.CityId) {
                        $scope.Cityduplicat = true;
                    }
                });

                if (data.Urban_Rural == "Urban") {
                    data.PanchayatId = null;
                }
            }
            if (data.CityId == 0) {
                data.City_Name = "";
            }
            if ($scope.GramPanchayatduplicat != true && $scope.Cityduplicat != true) {


                var InspacorName = $scope._language == 'en-us' ? $scope.InspacorData.filter(function (x) { return x.MappingID == $scope.InspactorId })[0].OfficerNameEn : $scope.InspacorData.filter(function (x) { return x.MappingID == $scope.InspacorId })[0].OfficerNameHi;


                $scope.OtherPersonListNew.push({ "Inspector": InspacorName, "Urban_Rural": data.Urban_Rural, "City_Block": data.Checkboxvalue == "2" ? data.Block_Name : data.City_Name, "GramPanchayat": data.Checkboxvalue == "2" ? data.Panchayat_Name : "", "Inspector_ID": $scope.InspactorId, "City_Block_ID": data.Checkboxvalue == "2" ? data.BlockId : data.CityId, "GramPanchayat_ID": data.PanchayatId, "DistrictId": data.DistrictId, "MonthId": $scope.MonthID });
                if ($scope.checksta == true) {
                    var addressdataset1 = {
                        Housebuilding: "",
                        State: "RJ",
                        District: " " + $("#hdnDistrictId").val(),
                        Lanestreet: "",
                        Locality: "",
                        Pincode: "",
                        CITY_ID: "0",
                        BLOCK_ID: "0",
                        GP_ID: "0"

                    }
                    SetAddressinfo($scope.translation, 'MemberAdd', addressdataset1, $http);
                }
            }
            else {

                if ($scope.GramPanchayatduplicat == true) {
                    message1 += $scope._language == 'en-us' ? '<br />already added gram panchayat, please another gram panchayat select.' : '<br />पहले से जोड़ी गई ग्राम पंचायत है कृपया अन्य  ग्राम पंचायत का चयन करें।';
                    if (message1.length > 1) {
                        ShowOkMessage("Inspector Error", message1, function () { });
                    }
                }
                if ($scope.Cityduplicat == true) {
                    message1 += $scope._language == 'en-us' ? '<br />already added city please another city select.' : '<br />पहले से जोड़ी गई शहर है कृपया अन्य  शहर का चयन करें।';
                    if (message1.length > 1) {
                        ShowOkMessage("Inspector Error", message1, function () { });
                    }
                }

            }

        }

    }
    $scope.CurrentUserDistrictId = $('#hdnDistrictId').val();
    $scope.AddGSS = function () {
        if ($scope.ValidateAddGSS()) {
            //var data = GetAddressinfo('MemberAdd');
            $scope.Urban_Rural = "Rural";
            $scope.Checkboxvalue = "2";
            $scope.GramPanchayatduplicat = false;
            $scope.Cityduplicat = false;
            $scope.Villageduplicat = false;


            if ($scope.Urban_Rural != "Urban") {
                var message1 = ' ';
                $scope.OtherPersonListNew.forEach(function (item, index) {
                    if (item.GramPanchayat_ID == $scope.PanchayatId) {
                        $scope.GramPanchayatduplicat = true;
                    }
                });
                $scope.OtherPersonListNew.forEach(function (item, index) {
                    if (item.VillageId == $scope.VillageId && item.VillageId != 0) {
                        $scope.Villageduplicat = true;
                    }
                });
            }
            else {
                var message1 = ' ';
                $scope.OtherPersonListNew.forEach(function (item, index) {
                    if (item.City_Block_ID == $scope.CityId) {
                        $scope.Cityduplicat = true;
                    }
                });

                if ($scope.Urban_Rural == "Urban") {
                    $scope.PanchayatId = null;

                }
            }
            if ($scope.CityId == 0) {
                $scope.City_Name = "";
            }

            if ($scope.GramPanchayatduplicat != true && $scope.Cityduplicat != true && $scope.Villageduplicat != true) {


                // var InspacorName = $scope._language == 'en-us' ? $scope.InspacorData.filter(function (x) { return x.MappingID == $scope.InspactorId })[0].OfficerNameEn : $scope.InspacorData.filter(function (x) { return x.MappingID == $scope.InspacorId })[0].OfficerNameHi;
                var InspacorName = ($scope.getDropDownListName($scope.InspacorData, $scope.InspactorId, "MappingID", "OfficerNameEn"));
                $scope.Panchayat_Name = ($scope.getDropDownListName($scope.GramPancyatList, $scope.PanchayatId, "GP_ID", "GP_ENG")) ?? '';
                $scope.Block_Name = ($scope.getDropDownListName($scope.BlockDataList, $scope.BlockId, "BLOCK_ID", "BLOCK_ENG")) ?? '';
                $scope.VillageName = ($scope.getDropDownListName($scope.VillageList, $scope.VillageId, "VILLAGE_ID", "VILLAGE_ENG")) ?? '';
                $scope.AssemblyName = ($scope.getDropDownListName($scope.AssemblyList, $scope.AssemblyId, "ASSEMBLY_ID", "AC_ENG")) ?? '';


                $scope.AssemblyId
                //$scope.City_Name = ($scope.getDropDownListName($scope.GramPancyatList, $scope.PanchayatId, "GP_ID", "GP_ENG"));

                var SurveyDateObj = new Date($scope.SurveyDate ?? new Date());
                $scope.SurveyDate = (SurveyDateObj.getMonth() + 1) + '/' + SurveyDateObj.getDate() + '/' + SurveyDateObj.getFullYear();

                $scope.OtherPersonListNew.
                    push({
                        "Inspector": InspacorName,
                        "Urban_Rural": $scope.Urban_Rural,
                        "City_Block": $scope.Checkboxvalue == "2" ? $scope.Block_Name : $scope.City_Name,
                        "GramPanchayat": $scope.Checkboxvalue == "2" ? $scope.Panchayat_Name : "",
                        "Inspector_ID": $scope.InspactorId,
                        "City_Block_ID": $scope.Checkboxvalue == "2" ? $scope.BlockId : $scope.CityId,
                        "GramPanchayat_ID": $scope.PanchayatId,
                        "DistrictId": $scope.DistrictId,
                        "MonthId": $scope.MonthID,
                        "SurveyDate": $scope.SurveyDate,
                        "Document": $scope.FileName,


                        //madhuri
                        "IsTspArea": $scope.IsTspArea,
                        "AreaType": $scope.IsTspArea === "1" ? $scope.TspAreaType : $scope.NonTspAreaType,
                        //"TspAreaType": $scope.IsTspArea=="1"? $scope.TspAreaType:0,
                        //"NonTspAreaType": $scope.IsTspArea == "0" ? $scope.NonTspAreaType:0,
                        "VillageId": $scope.VillageId ?? 0,
                        "VillageName": $scope.VillageName,
                        "AllotmentLetter": $scope.AllotmentLetter,
                        "CompetentAcceptanceLetter": $scope.FileName,

                        "InspactorTargetsId": $scope.InspactorTargetsId,
                        "IsMahilaMpacs": $scope.IsMahilaMpacs,
                        "LoanBankID": $scope.LoanBankID,
                        "LoanBranchId": $scope.LoanBranchId,
                        "AssemblyId": $scope.AssemblyId,
                        "ProposedNameMPACS": $scope.ProposedNameMPACS ?? "",
                    });
                $scope.$apply();

                if ($scope.isEditMode) {
                    $scope.isEditMode = false;
                    $scope.SaveIODetailsUser();
                }
                else {
                    $scope.M_PacsClear();
                }

            }
            else {

                if ($scope.NonTspAreaType != "3" && $scope.GramPanchayatduplicat == true) {
                    message1 += $scope._language == 'en-us' ? '<br />Already added gram panchayat please select another gram panchayat .' : '<br />पहले से जोड़ी गई ग्राम पंचायत है कृपया अन्य  ग्राम पंचायत का चयन करें।';
                    if (message1.length > 1) {
                        ShowOkMessage("Inspector Error", message1, function () { });
                    }
                }
                if ($scope.NonTspAreaType == "3" && $scope.Villageduplicat == true) {
                    message1 += $scope._language == 'en-us' ? '<br />Already added Village please select another village .' : '<br />पहले से ही गांव जोड़ा गया है कृपया किसी अन्य गांव का चयन करें।';
                    if (message1.length > 1) {
                        ShowOkMessage("Inspector Error", message1, function () { });
                    }
                }
                if ($scope.Cityduplicat == true) {
                    message1 += $scope._language == 'en-us' ? '<br />Already added city please select another city .' : '<br />पहले से जोड़ी गई शहर है कृपया अन्य  शहर का चयन करें।';
                    if (message1.length > 1) {
                        ShowOkMessage("Inspector Error", message1, function () { });
                    }
                }
            }
        }

        // $scope.$apply;
    }


    $scope.M_PacsClear = function () {
        InspacorName = "";
        $scope.Block_Name = "";
        $scope.City_Name = "";
        $scope.Panchayat_Name = "";
        $scope.InspactorId = "0";
        $scope.BlockId = "0";
        $scope.CityId = "0";
        $scope.PanchayatId = "0";
        $scope.DistrictId = "0";
        $scope.AssemblyId = "0";
        $scope.InspactorTargetsId = "0";

        $scope.LoanBankID = '0';
        $scope.LoanBranchId = '0';

        //  $scope.MonthID = "0";
        // $scope.FileName = "";
        if (!$scope.isEditMode) {
            $scope.SurveyDate = new Date();
            $scope.ChangeFile1(1, 'AllotmentLetter');
            $scope.ChangeFile1(1, 'FileName');
            $('#FileName').val('');
            $('#AllotmentLetter').val('');
            $scope.IsMahilaMpacs = false;
        }

        $scope.IsTspArea = -1;
        $scope.TspAreaType = '';
        $scope.NonTspAreaType = '';
        $scope.VillageId = 0;
        $scope.VillageName = '';
        $scope.ProposedNameMPACS = "";

        if (!$scope.$root.$$phase && !$scope.$$phase) {
            $scope.$apply();
        }
        // $scope.$apply();
    };

    $scope.SaveIODetailsUser = function () {

        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();
        var OtherPersonListNewIns = [];
        $scope.OtherPersonListNew.forEach(function (itm) {
            OtherPersonListNewIns.push({
                Inspector_ID: itm.Inspector_ID,
                InspacorName: itm.Inspector,
                City_Block: itm.City_Block,
                City_Block_ID: itm.City_Block_ID,
                GramPanchayat: itm.GramPanchayat,
                GramPanchayat_ID: itm.GramPanchayat_ID,
                DistictId: itm.DistrictId,
                Urban_Rural: itm.Urban_Rural,
                MonthId: itm.MonthId,
                SurveyDate: itm.SurveyDate,
                Document: itm.Document,

                //madhuri
                IsTspArea: itm.IsTspArea == "1" ? true : false,
                AreaType: itm.AreaType,
                VillageId: itm.VillageId,
                VillageName: itm.VillageName,
                AllotmentLetter: itm.AllotmentLetter,
                CompetentAcceptanceLetter: itm.CompetentAcceptanceLetter,


                InspactorTargetsId: itm.InspactorTargetsId,
                IsMahilaMpacs: itm.IsMahilaMpacs,
                LoanBankID: itm.LoanBankID,
                LoanBranchId: itm.LoanBranchId,
                AssemblyId: itm.AssemblyId,
                ProposedNameMPACS: itm.ProposedNameMPACS,

            });
        });


        if (OtherPersonListNewIns.length > 0) {

            $http.post(SiteUrl + "/Society/SaveInspactorTargets", { "InspactorTargets": OtherPersonListNewIns }).
                success(function (data, status) {
                    if (data > 0) {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? " Successfully Submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                            if (OtherPersonListNewIns.length == 1 && OtherPersonListNewIns[0].InspactorTargetsId > 0) {
                                window.location.href = "../society/InspactorTargetsAction";
                            }
                        });
                        $scope.OtherPersonListNew = [];
                        $scope.GetInspactorTargetsCount(InspactorId);
                        $scope.IsShowAssignForm1 = false;
                        $scope.IsShowAssignForm = true;
                        $(".se-pre-con").fadeOut("slow");
                    }
                    $(".se-pre-con").fadeOut("slow");

                }).
                error(function (data, status) {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    $(".se-pre-con").fadeOut("slow");
                })
        } else {
            $(".se-pre-con").fadeOut("slow");
            var message = _language == 'en-us' ? 'Please Add Inspector Details. \n' : 'कृपया इंस्पेक्टर विवरण जोड़ें \n';
            if (message.length > 1) {
                ShowOkMessage("Inspector Details", message, function () { });
                return false;
            }
            return true;
        }
    }

    $scope.Status1 = '0';

    $scope.Backtomove = function () {
        $scope.IsShowAssignForm = true;
        $scope.IsShowAssignForm1 = false;
    }

    $scope.RemoveOtherPersonListRow = function (index) {
        $scope.OtherPersonListNew.splice(index, 1);
        $scope.$apply();
    }

    $scope.AllDetails = false;


    $scope.GetInspactorTargetsCount = function () {

        $http.post("/Society/GetInspactorTargetsCount", JSON.stringify({ InspactorId: $scope.InspactorId, GSSSStatusId: $scope.GSSSStatusId })).
            success(function (data, status) {
                $scope.InspactorTargetsList = JSON.parse(data.DataList);
                //console.log('$scope.InspactorTargetsList', $scope.InspactorTargetsList);
                $scope.TotalData = {
                    GSSCount: 0,
                    AssignedCount: 0,
                    SurveyCount: 0,
                    TotalJRPlanApprovalCount: 0,
                    CompletedCount: 0,
                    UploadSanctionCount: 0,
                    TotalUnderProcess: 0
                };

                angular.forEach($scope.InspactorTargetsList, function (item) {
                    $scope.TotalData.GSSCount += Number(item.GSSCount || 0);
                    $scope.TotalData.AssignedCount += Number(item.AssignedCount || 0);
                    $scope.TotalData.SurveyCount += Number(item.SurveyCount || 0);
                    $scope.TotalData.TotalJRPlanApprovalCount += Number(item.TotalJRPlanApprovalCount || 0);
                    $scope.TotalData.CompletedCount += Number(item.CompletedCount || 0);
                    $scope.TotalData.UploadSanctionCount += Number(item.UploadSanctionCount || 0);
                    $scope.TotalData.TotalUnderProcess += Number(item.TotalUnderProcess || 0);
                });

                $scope.IsShowAssignForm = true;
            }).
            error(function (data, status) {
                $scope.InspactorTargetsList = [];
                alert('Error while Society Election Detail data.');
            });
    };

    $scope.ClearSearch = function () {

        $scope.InspactorId = '0';
        InspactorId = $scope.InspactorId;
        $scope.GetInspactorTargetsCount(InspactorId);
    }





    //old code 10-07-2026

    //$scope.getinsepectorwise = function (month, Inspector_ID, statusid) {
    //    debugger;

    //    var flag = (Inspector_ID == 0) ? 7 : 2;

    //    $scope.Status1 = '0';
    //    $scope.GSSSStatusId = statusid;


    //    if (statusid == 1) {
    //        $scope.Ischeckedall = false;
    //    }
    //    $scope.InspactorId = Inspector_ID;
    //    //$scope.searchRequest(GetInspactorTargetsDetails());
    //    $scope.DataPaging.currentPageID = 1;
    //    $scope.GetInspactorTargetsDetails(month);
    //    $scope.ischeckdtrue = false;
    //}
    //$scope.GetInspactorTargetsDetails = function (month) {
    //    $(".blockUI").show();
    //    var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "SearchType": $scope.SearchType };
    //    $http.post(SiteUrl + "/Society/GetInspactorTargetsDetails", JSON.stringify({ Details: jsonParameters, InspactorId: $scope.InspactorId, StatusId: $scope.Status1, GSSSStatusId: $scope.GSSSStatusId, Flag: 2, Month: month })).
    //        success(function (data, status) {
    //            $scope.InspactorTargetsListDetils = JSON.parse(data.DataList);
    //            $scope.AllDetails = true;
    //            $scope.IsShowAssignForm = false;
    //            $rootScope.DataPagingDetails = data.DataPaging;
    //            $rootScope.SetDataPaging();
    //            $(".blockUI").hide();

    //        }).
    //        error(function (data, status) {
    //            $(".blockUI").hide();
    //            $scope.InspactorTargetsListDetils = [];
    //            alert('Error while Society Election Detail data.');
    //        });
    //};


    $scope.InspactorId = '0';
    $scope.getinsepectorwise = function (month, Inspector_ID, statusid) {
        var flag = '';
        flag = (Inspector_ID == 0) ? 7 : 2;

        $scope.Status1 = '0';
        $scope.GSSSStatusId = statusid;


        if (statusid == 1) {
            $scope.Ischeckedall = false;
        }
        $scope.InspactorId = Inspector_ID;
        //$scope.searchRequest(GetInspactorTargetsDetails());
        $scope.DataPaging.currentPageID = 1;
        $scope.GetInspactorTargetsDetails(month, flag);
        $scope.ischeckdtrue = false;
    }
    $scope.GetInspactorTargetsDetails = function (month, flag) {
        $(".blockUI").show();
        if (angular.isUndefined(flag))
            flag = 7;

        if (angular.isUndefined(month))
            month = 0;
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "SearchType": $scope.SearchType };
        $http.post(SiteUrl + "/Society/GetInspactorTargetsDetails", JSON.stringify({ Details: jsonParameters, InspactorId: $scope.InspactorId, StatusId: $scope.Status1, GSSSStatusId: $scope.GSSSStatusId, Flag: flag, Month: month })).
            success(function (data, status) {
                $scope.InspactorTargetsListDetils = JSON.parse(data.DataList);
                $scope.AllDetails = true;
                $scope.IsShowAssignForm = false;
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").hide();

            }).
            error(function (data, status) {
                $(".blockUI").hide();
                $scope.InspactorTargetsListDetils = [];
                alert('Error while Society Election Detail data.');
            });
    };




    $scope.SubmitSurveyReport = function () {
        $scope.GSSSStatusId = '1';
        $scope.Status1 = '5';
        $scope.Ischeckedall = false;
        $scope.InspactorId = 0;
        $scope.DataPaging.currentPageID = 1;
        $scope.GetInspactorTargetsDetails(-1);
        $scope.ischeckdtrue = false;
        $scope.PrintDatalist = [];
    }

    $scope.BacktoMainDiv = function () {

        $scope.Ischeckedall = true;
        $scope.ischeckdtrue = false;
        $scope.InspactorId = '0';
        $scope.AllDetails = false;
        $scope.IsShowAssignForm = true;
    }


    $scope.ComplainUndeVlaue = {};
    $scope.ValidateAddGSS_old = function () {
        var data = GetAddressinfo('MemberAdd');
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.InspactorId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please  Select Inspactor.' : '<br />कृपया इंस्पेक्टर चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }


        //if ($scope.MonthID == "0" || $scope.MonthID == '' || $scope.MonthID == undefined) {
        //    message += $scope._language == 'en-us' ? '<br />Please  Select Month.' : '<br />कृपया माह का चयन करें।';
        //    //$("#txtAccountName_" + index).addClass("notValid");            
        //}


        if (data.Checkboxvalue == undefined || data.Checkboxvalue == "") {
            message += $scope._language == 'en-us' ? '<br />Please  Select Urban/Rural.' : '<br />कृपया शहरी/ग्रामीण चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }

        if (data.Checkboxvalue != undefined && data.Checkboxvalue == "2" && data.BlockId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please  Select Block.' : '<br />कृपया ब्लॉक चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }

        if (data.Checkboxvalue != undefined && data.Checkboxvalue == "2" && data.PanchayatId == "0" || data.PanchayatId == "" && data.Checkboxvalue == "2") {
            message += $scope._language == 'en-us' ? '<br />Please  Select Gram Panchayat.' : '<br />कृपया ग्राम पंचायत चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }

        if (data.Checkboxvalue != undefined && data.Checkboxvalue == "1" && data.CityId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please  Select City .' : '<br />कृपया सिटी चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }

        if (message.length > 29) {
            ShowOkMessage("Society Complaint ", message, function () { });
            $scope.checksta = false;
            return false;

        }
        $scope.checksta = true;
        return true;

    }
    $scope.ValidateDuplicateProcess = async function () {

        if ($scope.ValidateAddGSS()) {


            var message = "";
            //if ($scope.PanchayatId == "0" || $scope.PanchayatId == "") {
            //    message += $scope._language == 'en-us' ? '<br />Please Select Gram Panchayat.' : '<br />कृपया ग्राम पंचायत चुनें।';
            //    //$("#txtAccountName_" + index).addClass("notValid");            
            //}

            //if ($scope.NonTspAreaType == "3" && ($scope.VillageId == "0" || $scope.VillageId == "")) {
            //    message += $scope._language == 'en-us' ? '<br />Please Select Village.' : '<br />कृपया गाँव चुनें।';
            //    //$("#txtAccountName_" + index).addClass("notValid");            
            //}


            const ok = await $scope.ChkDuplicateGramPanchayatOrVillage();
            if (ok && !$scope.isEditMode) {

                if (($scope.VillageId > 0 && $scope.NonTspAreaType == "3") || ($scope.PanchayatId > 0 && $scope.NonTspAreaType != "3")) {
                    if ($scope.NonTspAreaType == "3") {
                        message += $scope._language == 'en-us' ? '<br />Already added Village please select another village .' : '<br />पहले से ही गांव जोड़ा गया है कृपया किसी अन्य गांव का चयन करें।';
                    }
                    else {
                        message += $scope._language == 'en-us' ? '<br />Already added gram panchayat please select another gram panchayat .' : '<br />पहले से जोड़ी गई ग्राम पंचायत है कृपया अन्य  ग्राम पंचायत का चयन करें।';
                    }
                }

            }

            if (message.length > 29) {
                ShowOkMessage("M.PACS Target ", message, function () { });
                return false;
            }
            else {
                //if ($scope.ValidateAddGSS()) {
                //} 
                $scope.AddGSS();
                //return true;
            }

        }


    }
    $scope.ValidateAddGSS = function () {

        //  if (!ValidateDuplicateProcess()) { return; };

        // var data = GetAddressinfo('MemberAdd');
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.MonthID == "0" || $scope.MonthID == '' || $scope.MonthID == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please  Select Month.' : '<br />कृपया माह का चयन करें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }
        if ($scope.InspactorId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please  Select Inspactor.' : '<br />कृपया इंस्पेक्टर चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }

        if ($scope.IsTspArea == -1 || $scope.IsTspArea === undefined || $scope.IsTspArea === '') {
            message += $scope._language === 'en-us' ? '<br />Please choose TSP/Non-TSP Area.' : '<br />कृपया टीएसपी / गैर-टीएसपी क्षेत्र चुनें।';
        }

        // ---- If TSP is selected, ensure a TSP sub-type ----
        if ($scope.IsTspArea === 1 && ($scope.TspAreaType === undefined || $scope.TspAreaType === '')) {
            message += $scope._language === 'en-us' ? '<br />Please select TSP Area Type.' : '<br />कृपया टीएसपी क्षेत्र का प्रकार चुनें।';
        }

        // ---- If Non-TSP is selected, ensure a Non-TSP sub-type ----
        if ($scope.IsTspArea === 0 && ($scope.NonTspAreaType === undefined || $scope.NonTspAreaType === '')) {
            message += $scope._language === 'en-us' ? '<br />Please select Non-TSP Area Type.' : '<br />कृपया गैर-टीएसपी क्षेत्र का प्रकार चुनें।';
        }

        // ---- Example: District required only after a subtype is chosen ----
        if ((($scope.IsTspArea === 1 && $scope.TspAreaType) || ($scope.IsTspArea === 0 && $scope.NonTspAreaType)) && ($scope.DistrictId === undefined || $scope.DistrictId === '0')) {
            message += $scope._language === 'en-us' ? '<br />Please select District.' : '<br />कृपया जिला चुनें।';
        }



        //if (data.Checkboxvalue == undefined || data.Checkboxvalue == "") {
        //    message += $scope._language == 'en-us' ? '<br />Please  Select Urban/Rural.' : '<br />कृपया शहरी/ग्रामीण चुनें।';
        //    //$("#txtAccountName_" + index).addClass("notValid");            
        //}
        if ($scope.DistrictId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please Select District.' : '<br />कृपया ज़िला चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }
        if ($scope.AssemblyId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please Select Assembly.' : '<br />कृपया विधानसभा चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }
        if ($scope.BlockId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please Select Block.' : '<br />कृपया ब्लॉक चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }

        if ($scope.PanchayatId == "0" || $scope.PanchayatId == "") {
            message += $scope._language == 'en-us' ? '<br />Please Select Gram Panchayat.' : '<br />कृपया ग्राम पंचायत चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }

        if ($scope.NonTspAreaType == "3" && ($scope.VillageId == "0" || $scope.VillageId == "")) {
            message += $scope._language == 'en-us' ? '<br />Please Select Village.' : '<br />कृपया गाँव चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }
        if ($scope.NonTspAreaType == "3" && ($scope.ProposedNameMPACS == "" || $scope.ProposedNameMPACS == null || $scope.ProposedNameMPACS == undefined)) {
            message += $scope._language == 'en-us' ? '<br />Please Enter Name of Proposed MPACS.' : '<br />Please Enter Name of Proposed MPACS.।';
        }
        if ($scope.AllotmentLetter == '' || $scope.AllotmentLetter == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please upload Allotment Letter .' : '<br />कृपया आवंटन पत्र अपलोड करें।';
        }

        if ($('#hdnDistrictId').val() != $scope.UserRegistration.DistrictId && ($scope.FileName == '' || $scope.FileName == undefined)) {
            message += $scope._language == 'en-us' ? '<br />Please upload Sanction Letter By Competent Authority .' : '<br />कृपया सक्षम प्राधिकारी द्वारा स्वीकृति पत्र अपलोड करें।';
        }

        if ($scope.LoanBankID == "0" || $scope.LoanBankID == "") {
            message += $scope._language == 'en-us' ? '<br />Please Select Bank Name .' : '<br />कृपया बैंक का नाम चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }
        if ($scope.LoanBranchId == "0" || $scope.LoanBranchId == "") {
            message += $scope._language == 'en-us' ? '<br />Please Select Branch Name.' : '<br />कृपया शाखा का नाम चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }

        //if (data.Checkboxvalue != undefined && data.Checkboxvalue == "1" && data.CityId == "0") {
        //    message += $scope._language == 'en-us' ? '<br />Please  Select City .' : '<br />कृपया सिटी चुनें।';
        //    //$("#txtAccountName_" + index).addClass("notValid");
        //}


        if ($scope.InspactorId == $scope.OldInspactorId) {
            message += $scope._language == 'en-us' ? '<br />Please Select Another Inspactor.' : '<br />कृपया अन्य निरीक्षक का चयन करें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }


        if (message.length > 29) {
            ShowOkMessage("M.PACS Target ", message, function () { });
            $scope.checksta = false;
            return false;
        }
        $scope.checksta = true;
        return true;

    }
    function _Selectedinspector() {
        this.InspactorTargetsId = '0';
    }
    $scope.ShowSelectedInspector = {
        IsChecked: false
    }

    $scope.PrintDatalist = [];
    $scope.ArrInspector = [];
    $scope.Chkdinspector = function (item) {

        var _SelectedinspectorList = new _Selectedinspector();
        _SelectedinspectorList.InspactorTargetsId = item.InspactorTargetsId;
        if (item.chkdboxValue) {
            $scope.ArrInspector.push(item);
            $scope.PrintDatalist.push(item)
            $scope.ischeckdtrue = true;
        }
        else {
            var data = $scope.ArrInspector;
            $scope.ArrInspector = [];
            $scope.ischeckdtrue = false;
            data.forEach(function (itm, idx) {
                if (itm.InspactorTargetsId != item.InspactorTargetsId) {
                    $scope.ArrInspector.push(item);
                    $scope.PrintDatalist.push(item);
                    $scope.ischeckdtrue = true;
                }
            });
        }
        console.log($scope.ArrInspector);
    }
    $scope.ischeckdtrue = false;


    $scope.ChkdiAllnspector = function () {

        if ($scope.Ischeckedall) {
            $scope.InspactorTargetsListDetils.forEach(function (itm, index) {
                itm.chkdboxValue = true;
                $scope.ArrInspector.push(itm);
                $scope.PrintDatalist.push(itm)
            });
        }
        else {
            $scope.InspactorTargetsListDetils.forEach(function (itm, index) {
                itm.chkdboxValue = false;
                $scope.ArrInspector = [];
                $scope.PrintDatalist = [];

            });
        }
        $scope.ischeckdtrue = $scope.InspactorTargetsListDetils.filter(function (x) { return x.chkdboxValue == true }).length > 0;
    }




    //Senction Checkbox


    $scope.isSenction = false;
    $scope.ChkdiAllSenction = function () {

        if ($scope.IscheckedallSenction) {
            $scope.InspactorTargetsListDetils.forEach(function (itm, index) {
                itm.chkdboxValue = true;
                $scope.ArrInspector.push(itm);
            });
        }
        else {
            $scope.InspactorTargetsListDetils.forEach(function (itm, index) {
                itm.chkdboxValue = false;
                $scope.ArrInspector = [];
            });
        }
        $scope.isSenction = $scope.InspactorTargetsListDetils.filter(function (x) { return x.chkdboxValue == true }).length > 0;
    }

    $scope.ArrInspector = [];

    $scope.ChkdSenction = function (item) {
        var _SelectedinspectorList = new _Selectedinspector();
        _SelectedinspectorList.InspactorTargetsId = item.InspactorTargetsId;
        if (item.chkdboxValue) {
            $scope.ArrInspector.push(item);
            $scope.isSenction = true;
        }
        else {
            var data = $scope.ArrInspector;
            $scope.ArrInspector = [];
            $scope.ischeckdtrue = false;
            data.forEach(function (itm, idx) {
                if (itm.InspactorTargetsId != item.InspactorTargetsId) {
                    $scope.ArrInspector.push(item);
                    $scope.isSenction = true;
                }
            });
        }
        console.log($scope.ArrInspector);
    }


    $scope.ApprovedForRagistration = function () {
        if ($scope.validatestop()) {
            $scope.ReqoestStatus = '8';
            $(".se-pre-con").fadeIn("slow");
            var checkedData = $scope.InspactorTargetsListDetils.filter(function (x) { return x.chkdboxValue });
            $http.post(SiteUrl + "/Society/SaveInspactorStatusDR", { InspactorStatusDetails: checkedData, Remarks: $scope.RemarksSenction, Inspectorfile: $scope.FileName, status: $scope.ReqoestStatus, TXNno: "", SenctionDate: "" }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data == "1") {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                            window.location.href = "../society/InspactorTargets";
                        });
                    } else {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "There is a problem while submitting the request. Please try after some time.<br/></b>" : "अनुरोध सबमिट करते समय समस्या आ रही  है। कृपया कुछ समय बाद प्रयास करें।<br/></b>", function () { });
                    }
                }).
                error(function (data, status) {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    $(".se-pre-con").fadeOut("slow");
                });
        }
    }

    //end--Senction

    $scope.validatestop = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.InspactorTargetsListDetils.filter(function (x) { return x.chkdboxValue }).length == 0) {
            message += $scope._language == 'en-us' ? '<br />Please select at least one record for process.' : '<br />कृपया प्रक्रिया के लिए कम से कम एक रिकॉर्ड का चयन करें।';
        }


        if ($scope.GSSSStatusId != 4) {
            if ($scope.ReqoestStatus == undefined || $scope.ReqoestStatus == "") {
                message += $scope._language == 'en-us' ? '<br />Please select Approved/Rreject.' : '<br />कृपया स्वीकृत/अस्वीकार का चयन करें।';
            }

            if ($scope.ReqoestStatus == 7 && ($scope.Remarks == null || $scope.Remarks == "" || $scope.Remarks == undefined)) {
                message += $scope._language == 'en-us' ? '<br />Please Enter Remarks.' : '<br />कृपया रिमार्क दर्ज़ करें।';
            }

            if ($scope.ReqoestStatus == 3 && ($scope.FileName == null || $scope.FileName == "" || $scope.FileName == undefined)) {
                message += $scope._language == 'en-us' ? '<br />Please select the proposal report file.' : '<br />कृपया प्रस्ताव रिपोर्ट फ़ाइल का चयन करें।';
            }
        }


        if ($scope.GSSSStatusId == 4 && ($scope.RemarksSenction == null || $scope.RemarksSenction == "" || $scope.RemarksSenction == undefined)) {
            message += $scope._language == 'en-us' ? '<br />Please Enter Remarks.' : '<br />कृपया रिमार्क दर्ज़ करें।';
        }

        if (message.length > 29) {
            ShowOkMessage("Inspector Status ", message, function () { });
            $scope.checksta = false;
            return false;

        }
        $scope.checksta = true;
        return true;
    }
    $scope.SubmitDRToDistrict = function () {
        if ($scope.validatestop()) {
            $(".se-pre-con").fadeIn("slow");
            var checkedData = $scope.InspactorTargetsListDetils.filter(function (x) { return x.chkdboxValue });
            $http.post(SiteUrl + "/Society/SaveInspactorStatusDR", { InspactorStatusDetails: checkedData, Remarks: $scope.Remarks, Inspectorfile: $scope.FileName, status: $scope.ReqoestStatus, TXNno: "", SenctionDate: "" }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data == "1") {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                            window.location.href = "../society/InspactorTargets";
                        });
                    } else {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "There is a problem while submitting the request. Please try after some time.<br/></b>" : "अनुरोध सबमिट करते समय समस्या आ रही  है। कृपया कुछ समय बाद प्रयास करें।<br/></b>", function () { });
                    }
                }).
                error(function (data, status) {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    $(".se-pre-con").fadeOut("slow");
                });
        }
    }

    $scope.SubmitDRToDistrict123 = function () {

        if ($scope.validatestop()) {
            $(".se-pre-con").fadeIn("slow");
            if ($scope.InspactorTargetsListDetils.length > 1) {
                if ($('input:checkbox').prop('checked') == true || ($('input:checkbox.chk').prop('checked') == true) || (JSON.stringify($scope.ArrSelectSociety)).length > 50) {

                    $http.post(SiteUrl + "/Society/SaveInspactorStatusDR", { InspactorStatusDetails: $scope.ArrInspector, Remarks: $scope.Remarks, Inspectorfile: $scope.FileName }).
                        success(function (data, status) {
                            if (data == "1") {
                                ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () { });
                                $(".se-pre-con").fadeOut("slow");
                            }
                        }).
                        error(function (data, status) {
                            message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                            alert(message);
                            $(".se-pre-con").fadeOut("slow");
                        })
                }

                else {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("Warning", "Please select at least one Inspector Details for process.", function () { });
                }

            }
            else {


                $http.post(SiteUrl + "/Society/SaveInspactorStatusDR", { InspactorStatusDetails: $scope.ArrInspector, Remarks: $scope.Remarks, Inspectorfile: $scope.FileName }).
                    success(function (data, status) {
                        if (data == "1") {
                            ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () { });
                            $(".se-pre-con").fadeOut("slow");
                        }
                    }).
                    error(function (data, status) {
                        message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                        alert(message);
                        $(".se-pre-con").fadeOut("slow");
                    })


            }
        }
    }


    /*
    $scope.IsUploadFileName = false;
    
    $scope.AddFile1 = function (type, id) {      
        $scope.FileType = type;
        $scope.IsUploadFileName = true;
        $scope.IsUpload = false;
    }
    $scope.ChangeFile1 = function (type, id) {
       
        if (type == 1) {
            $scope.IsUploadFileName = false;
            $scope.FileName = "";
            $scope.IsUpload = false;
        }
    }

    $scope.UploadFile_new = function (id, type) {
      
        try {
            // FormData
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                //var blob = b64toBlob(realData, contentType, 512);
                var formData = new FormData();
                formData.append("MemberFile", files[0]);
                $(".blockUI").show();
                $scope.xhr1(SiteUrl + "/Society/UploadinInspactorFile", formData, type);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }

    $scope.xhr1 = function (url, data, type) {
      
        try {
            var request = new XMLHttpRequest();
            request.onreadystatechange = function () {
                if (request.readyState == 4 && request.status == 200) {
                    $(".blockUI").hide();
                    $("#filuploadProgress").hide();
                    $("#filuploadProgress").find("div").html("");
                    $("#filuploadProgress").find("div").attr("aria-valuenow", "1");
                    $("#filuploadProgress").find("div").attr("style", "width:1%");
                    if (request.responseText == "") {
                        ShowOkMessage("Error", $scope == 'en-us' ? "There is some problem in uploading file on server. Please try after some time" : "सर्वर पर फ़ाइल अपलोड करने में कुछ समस्या है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    } else {
                        if (type == 1) {
                           
                            $scope.FileName = request.responseText;
                            $scope.IsUpload = true;
                            $scope.IsUploadFileName = true;
                        }
                        $scope.$apply();
                    }
                }
            };

            request.upload.onloadstart = function () {
                //percentage.innerHTML = 'Upload started...';
            };
            $("#filuploadProgress").show();
            request.upload.onprogress = function (event) {
                $("#filuploadProgress").find("div").html(Math.round(event.loaded / event.total * 100) + "%");
                $("#filuploadProgress").find("div").attr("aria-valuenow", Math.round(event.loaded / event.total * 100));
                $("#filuploadProgress").find("div").attr("style", "width:" + Math.round(event.loaded / event.total * 100) + "%");
            };

            request.upload.onload = function (result) {
            }
            request.open('POST', url);
            request.send(data);
        }
        catch (ex) { alert(ex) }
    }

    */
    //$scope.FileName = "";
    //$scope.AllotmentLetter = "";
    $scope.FileType = "";



    $scope.IsUploadFileName = {};   // { 'AllotementLetter': false, 'SurveyReport': false }

    $scope.AddFile1 = function (type, id) {
        $scope.FileType = type;
        $scope.IsUploadFileName[id] = true;
        $scope.IsUpload = false;
        $scope[id] = "";
    };
    $scope.fileChanged = function (input, fieldName, type) {
        if (input.files && input.files.length > 0) {
            var file = input.files[0];
            $scope.$apply(function () {
                const acceptTypes = (input.getAttribute("accept") || "").split(",");
                const allowed = acceptTypes.some(t => file.type === t.trim());
                if (!allowed) {
                    alert("Invalid file type. Allowed: " + acceptTypes.join(", "));
                    input.value = "";        // clear the input
                    return;                  // stop further execution
                }

                // 2️⃣  Check size (e.g., < 4 MB)
                const sizeKB = file.size / 1024;
                if (sizeKB >= 4096) {
                    ShowOkMessage("Warning",
                        $scope._language === 'en-us'
                            ? "File too big, please select a file less than 4 MB"
                            : "फ़ाइल बहुत बड़ी है, कृपया 4MB से कम फ़ाइल का चयन करें",
                        function () { }
                    );
                    input.value = "";
                    return;
                }


                $scope.FileType = type;
                $scope.IsUploadFileName[fieldName] = true;
                $scope.IsUpload = false;
                $scope[fieldName] = input.files[0].name; // store file name
                $scope.UploadFile_new1(input, fieldName, 1);
            });
        } else {
            // User clicked cancel, reset
            $scope.$apply(function () {
                $scope.IsUploadFileName[fieldName] = false;
                $scope[fieldName] = "";
            });
        }
    };

    //$scope.AddFile1 = function (index, fieldName) {
    //    var fileInput = document.getElementById(fieldName);
    //    if (fileInput && fileInput.files.length > 0) {
    //        $scope[fieldName] = "";
    //        $scope.IsUploadFileName[fieldName] = true;
    //        $scope.AllotmentLetter = fileInput.files[0]; // store the selected file
    //    } else {
    //        $scope.IsUploadFileName[fieldName] = false;
    //    }
    //    $scope.IsUpload = false;
    //    fileInput.value = null;
    //};
    $scope.ChangeFile1 = function (type, id) {
        if (type == 1) {
            $scope.IsUploadFileName[id] = false;
            //$scope.FileName = "";
            $scope.IsUpload = false;
            $scope[id] = "";

            var inputElem = document.getElementById(id);
            if (inputElem) {
                inputElem.value = null; // This clears the selected file
            }
        }
    };
    $scope.checkFile = function (id) {
        var fileInput = document.getElementById(id);
        $scope.IsUploadFileName[id] = (fileInput.files.length > 0) ? true : false;
    };
    $scope.UploadFile_new = function (id, type) {
        try {
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                var formData = new FormData();
                formData.append("MemberFile", files[0]);
                $(".blockUI").show();
                $scope.xhr1(SiteUrl + "/Society/UploadinInspactorFile", formData, type, id);
            } else {
                alert("FormData is not supported.");
            }
        } catch (ex) { alert(ex) }
    };

    $scope.UploadFile_new1 = function (input, id, type) {

        try {
            // FormData
            if (window.FormData !== undefined) {
                var files = input.files[0];
                var formData = new FormData();
                formData.append("MemberFile", files);
                $(".blockUI").show();
                $scope.xhr1(SiteUrl + "/Society/UploadinInspactorFile", formData, type, id);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }

    $scope.xhr1 = function (url, data, type, id) {
        try {
            var request = new XMLHttpRequest();
            request.onreadystatechange = function () {
                if (request.readyState == 4 && request.status == 200) {
                    $(".blockUI").hide();
                    $("#filuploadProgress").hide();
                    $("#filuploadProgress").find("div").html("");
                    $("#filuploadProgress").find("div").attr("aria-valuenow", "1");
                    $("#filuploadProgress").find("div").attr("style", "width:1%");
                    if (request.responseText != "") {
                        if (type == 1) {
                            $scope[id] = request.responseText;   // store file name in its own variable
                            $scope.IsUploadFileName[id] = true;
                            $scope.IsUpload = true;
                        }

                        $scope.$apply();
                    }
                    else {
                        ShowOkMessage("Error", $scope == 'en-us' ? "There is some problem in uploading file on server. Please try after some time" : "सर्वर पर फ़ाइल अपलोड करने में कुछ समस्या है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    }
                }
            };
            request.open('POST', url);
            request.send(data);
        } catch (ex) { alert(ex) }
    };





    $scope.MonthList = [];
    $scope.GetAuditMonthList = function () {
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MONTH&MasterCode=-1").
            success(function (data, status) {
                $scope.MonthList = data;
                const currentMonth = new Date().getMonth() + 1; // JS months are 0-based

                $scope.MonthList = data.filter(m => m.Month_Id >= currentMonth);
            }).
            error(function (data, status) {
                $scope.MonthList = [];
            });
    };



    // Madhuri 10 SEP 2025

    $scope.LevelList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Level = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=LEVELS").
            success(function (data, status) {
                $scope.Level = data;
                $scope.Level = data.filter(x => x.LevelId == 2 || x.LevelId == 3)
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.OfficeList = function () {
        //$scope.UserRegistration.DistrictId = "0";
        if ($scope.CurrentUserDistrictId == $scope.UserRegistration.DistrictId) {
            $scope.FileName = "";
        }


        $scope.InspactorId = "0";
        $scope.InspacorData = [];
        $scope.UserRegistration.OfficeId = "0";
        $scope.OfficeListData = [];
        $http.get(SiteUrl + "/Master/OfficesList/?WhereClause=and o.isactive =1 " + ($scope.UserRegistration.LevelId && $scope.UserRegistration.LevelId != 0 ? " and l.LevelId=" + $scope.UserRegistration.LevelId : "")).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.OfficeListData = data;
                if (window.location.href.includes("localhost")) {
                    $scope.OfficeListData = data.filter(x => x.OfficeId == 48 || x.OfficeId == 42 || x.OfficeId == 10 || x.OfficeId == 3);

                }
                else {

                    //   $scope.OfficeListData = data.filter(x => x.OfficeId == 3 || x.OfficeId == 27 || x.OfficeId == 10 || x.OfficeId == 65      );// 3,27 district ,10,65 division
                    $scope.OfficeListData = data.filter(x => x.OfficeId == 3 || x.OfficeId == 27 || x.OfficeId == 2 || x.OfficeId == 65); //3,27 district 
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching designation list");
                $scope.OfficeListData = [];
                $scope.showLoader = false;
            });
    };
    $scope.DistrictList = function () {
        //$scope.Districts = [{}];
        // if (!$scope.UserRegistration.LevelId && $scope.UserRegistration.LevelId != 3) { return; }

        $scope.UserRegistration.DistrictId = "0";
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=-1").
            success(function (data, status) {
                $scope.Districts = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.Districts = [];
            });
    };

    $scope.GetRAJFEDRO = function () {
        //  if (!$scope.UserRegistration.LevelId && $scope.UserRegistration.LevelId != 2) { return; }

        $scope.UserRegistration.DivisionId = "0";
        $scope.ROList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DIVISION&MasterCode=0").
            success(function (data, status) {
                $scope.ROList = data;
            }).
            error(function (data, status) {
                // alert("Error while fetching Post list")

            });
    }



    $scope.FinancialYearList = function () {
        $scope.years = [];
        const startYear = 2021;
        const currentYear = new Date().getFullYear();
        // push years from 2021 up to current year
        for (let y = startYear; y <= currentYear; y++) {
            $scope.years.push(y);
        }

        // set selected year to current year
        $scope.selectedYear = currentYear;
    }

    $scope.FinancialYearList();
    $scope.DistrictListData = function () {
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district")
            .then(function (resp) {
                $scope.DistrictLst = resp.data;
                $scope.DistrictId = "0";
                return resp.data;
            });
    };

    //$scope.DistrictListData = function () {
    //    //$scope.UserRegistration.DistrictId = "0";
    //    $(".se-pre-con").fadeIn("slow");
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=-1").
    //        success(function (data, status) {
    //            $scope.DistrictLst = data;
    //            $(".se-pre-con").fadeOut("slow");
    //        }).
    //        error(function (data, status) {
    //            $scope.DistrictLst = [];
    //        });
    //};

    //function getRural(id) {

    //    $('#ddlGP_MemberAdd').empty();
    //    $('#ddlGP_MemberAdd').append('<option value=""  class="ng-binding">--- Select ---</option>');
    //    var radioValue = $("input[name='Rurals']:checked").val();
    //    if (radioValue == 2) {
    //        $(".Rurals_MemberAdd").show();
    //        $(".cityward_MemberAdd").hide();
    //        var ddlFruits = document.getElementById("district_MemberAdd");
    //        // var selectedText = district_MemberAdd.options[district_MemberAdd.selectedIndex].innerHTML;
    //        var selectedValue = district_MemberAdd.value;
    //        var divisionid = parseInt(selectedValue)
    //        getblock(divisionid).then(function () {
    //            $("#ddlblocki_" + id).val(blockid);
    //            getGrampanchyat(blockid).then(function () {
    //                $("#ddlGP_" + id).val(panchayatid);
    //                getvillage(panchayatid).then(function () {
    //                    $("#ddlVillage_" + id).val(villageid);
    //                });
    //            });
    //        })
    //    }
    //}

    $scope.getAssembly = function () {
        disid = $scope.DistrictId;
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ASSEMBLY&MasterCode=" + disid).
            then(function (resp) {
                //success(function (data, status) {
                $scope.AssemblyList = resp.data;
                $scope.AssemblyId = '0';
                $(".se-pre-con").fadeOut("slow");
                // _callback(Addressgetinfo);
                return resp.data;
            }).
            catch(function () {
                //error(function (data, status) {
                $scope.AssemblyList = [];
                $(".se-pre-con").fadeOut("slow");
            });

    }
    $scope.getblock = function () {
        debugger;
        disid = $scope.DistrictId;
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + disid).
            then(function (resp) {
                //success(function (data, status) {
                $scope.BlockDataList = resp.data;
                $scope.BlockId = '0';
                $(".se-pre-con").fadeOut("slow");
                // _callback(Addressgetinfo);
                return resp.data;
            }).
            catch(function () {
                //error(function (data, status) {
                $scope.BlockDataList = [];
                $(".se-pre-con").fadeOut("slow");
            });

    }

    $scope.getGrampanchyat = function () {
        divisionid = $scope.BlockId;
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + divisionid).success(function (data, status) {
            $scope.GramPancyatList = data;
            $scope.PanchayatId = '0';
            $(".se-pre-con").fadeOut("slow");
        }).
            error(function (data, status) {
                $scope.GramPancyatList = [];
                $(".se-pre-con").fadeOut("slow");

            });

    }

    $scope.getvillage = function () {
        pnchid = $scope.PanchayatId;
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + pnchid).
            success(function (data, status) {
                $scope.VillageList = data;
                $scope.VillageId = '0';
            }).
            error(function (data, status) {
                $scope.Villages = [];
            });
    }


    $scope.getSelectedOfficeName = function () {
        selectedOffice = $scope.OfficeListData.find(function (item) {
            return item.OfficeId == $scope.UserRegistration.OfficeId;
        });
    };
    $scope.getSelectedDesignationName = function () {
        selectedDesignation = $scope.DESIGNATION.find(function (item) {
            return item.DESIGNATIONCODE == $scope.UserRegistration.DesignationId;
        });
    };

    $scope.getSelectedDivisionName = function () {
        selectedDivision = $scope.ROList.find(function (item) {
            return item.RajfedROId == $scope.UserRegistration.DivisionId;
        });

    };
    $scope.getSelectedDistrictName = function () {
        selectedDistrict = $scope.Districts.find(function (item) {
            return item.DISTRICT_ID == $scope.UserRegistration.DistrictId;
        });
    };
    $scope.getLevelName = function () {
        selectedLevel = $scope.Level.find(function (item) { return item.LevelId == $scope.UserRegistration.LevelId; });
    };

    $scope.getDropDownListName = function (list, selectedId, key, returnColumn) {
        selectedItem = list.find(function (item) { return item[key] == selectedId; });
        return selectedItem ? selectedItem[returnColumn] : null;
    };

    $scope.M_PacsDataclear = function () {
        // InspacorName = "";
        $scope.Block_Name = "";
        $scope.City_Name = "";
        $scope.Panchayat_Name = "";
        // $scope.InspactorId = "0";
        $scope.BlockId = "0";
        $scope.CityId = "0";
        $scope.PanchayatId = "0";
        $scope.DistrictId = "0";
        $scope.AssemblyId = "0";

        $scope.VillageId = "0";
        $scope.FileName = "";
        $scope.ProposedNameMPACS = "";

        //$scope.LoanBankID = '0';
        //$scope.LoanBranchId = '0';

        if (!$scope.isEditMode) {
            $scope.SurveyDate = new Date();
            $scope.ChangeFile1(1, 'AllotmentLetter');
            $scope.ChangeFile1(1, 'FileName');
            $scope.FileName = '';
            $('#FileName').val('');
            //  $scope.IsMahilaMpacs = false;
        }
    };


    $scope.setParameterValue = function (key, value) {
        var parameter = { Key: key, Value: value }
        //  $scope.DataPaging.searchParameters.push(parameter);

        var existing = $scope.DataPaging.searchParameters
            .find(function (p) { return p.Key === key; });

        if (existing) {
            // Key already present – update its Value
            existing.Value = value;
        } else {
            // Key not present – add new object
            $scope.DataPaging.searchParameters.push({ Key: key, Value: value });
        }
    }
    $scope.ChkDuplicateGramPanchayatOrVillage = async function () {

        $(".se-pre-con").fadeIn("slow");

        if ($scope.VillageId > 0 && $scope.ProposedNameMPACS && $scope.ProposedNameMPACS.trim() !== "" && $scope.NonTspAreaType == "3") {
            $scope.setParameterValue('VillageId', $scope.VillageId);
            $scope.setParameterValue('Action', "chkDuplicateVillage");
            $scope.setParameterValue('ProposedNameMPACS', $scope.ProposedNameMPACS);
            // var parameter = { Key: $(this).attr('VillageId'), Value: $scope.VillageId }
        }

        if ($scope.PanchayatId > 0 && $scope.NonTspAreaType != "3") {
            // var parameter = { Key: $(this).attr('PanchayatId'), Value: $scope.PanchayatId }
            $scope.setParameterValue('PanchayatId', $scope.PanchayatId);
            $scope.setParameterValue('Action', "chkDuplicateGp");

        }

        var jsonParameters = { "TotalRecords": ($scope.DataPaging.TotalRecords ?? 0), "PageNumber": $scope.DataPaging.currentPageID, "PageSize": 500, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

        return $http.post(
            SiteUrl + "/MIS/GccLoanReports",
            JSON.stringify({ Details: jsonParameters, Exportjson: null, Action: "GSSTargetData" })
        ).then(function (response) {
            var data = response.data.DataList ? JSON.parse(response.data.DataList) : [];
            $(".se-pre-con").fadeOut("slow");
            return data.length > 0 && data[0].IsAlreadyExist == 1;

        }).catch(function () {
            $(".se-pre-con").fadeOut("slow");
            return false; // on error, treat as not duplicate


        });

    };



    $scope.$watchGroup(['IsTspArea', 'NonTspAreaType', 'TspAreaType'], function (newVals, oldVals) {
        var isTsp = newVals[0];
        var nonTsp = newVals[1];
        var tspType = newVals[2];

        // Reset first
        $scope.M_PacsDataclear();

        // Then handle CAD special case
        if (nonTsp === '3') {
            $scope.DistrictId = '16';
            $scope.getblock();
            $scope.getAssembly();

        }
    });

    $scope.LoanBankList = function () {
        $scope.LoanBankID = '0';
        $scope.BankMasterList = [];
        return $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=0").
            success(function (data, status) {
                $scope.BankMasterList = data;

                $scope.LoanBranchId = '0';
            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
            });
    };


    $scope.goBackToInspactorTargets = function () {
        window.location.href = "../society/InspactorTargets";
    };

    //document.getElementById('BankID').setAttribute('disabled', 'disabled');
    $scope.Get_LoanBankBranch = function (branch_ID) {
        $scope.BankBranches = [];
        $scope.LoanBranchId = '0';
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?" + "LoanBankId=" + $scope.LoanBankID).
            success(function (data, status) {
                $scope.BankBranches = data;

                //  $scope.GetGccSummary();
                // $scope.GetGccApplicationList();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.BankBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.OnPageLoad();
});

MasterMod.directive('validfile', function validFile() {
    var validFormats = ['jpg', 'gif'];

    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function () {
                elem.on('change', function () {

                    if (elem[0].files.length > 0) {
                        const fsize = elem[0].files[0].size;
                        const file = Math.round((fsize / 1024));
                        // The size of the file. 

                        if (file >= 4096) {

                            ShowOkMessage("Warning", scope == 'en-us' ? "File too Big, please select a file less than 4mb" : "फ़ाइल बहुत बड़ी है, कृपया 4mb से कम फ़ाइल का चयन करें", function () { });
                            elem[0].value = "";
                            scope.IsUploadFileName1 = false;
                        } else {
                            if (scope.FileType == 1) {
                                scope.IsUploadFileName1 = true;
                            }
                            scope.$apply();

                        }
                    }


                });
            };
        }
    };



});




