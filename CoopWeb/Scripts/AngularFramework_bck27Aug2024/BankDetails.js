var BankDetailsController = angular.module("BankDetailsMod", ['angularUtils.directives.dirPagination']);
BankDetailsController.controller("BankDetailsCnt", function ($scope, $rootScope, $http) {

    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 300,
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
    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        if ($scope.Filter.FilterType == 'SubmissionDate') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            $scope.AddCalender();
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }
    $scope.ClearSearch = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GetElectionSocietyListData();
    }
    /*------------End Paging and Filter-----------------*/

    $scope.BankMaster = [];
    $scope.ShowAddNew = false;
    $scope.ShowForm = function () {
        $scope.ShowAddNew = true;

        $scope.BankMaster.SocietyID = "0";
        $scope.BankMaster.BranchId = "0";
        $scope.BankMaster.BankID = "0";
        $scope.BankMaster.BankBranch = "";
        $scope.BankMaster.IFSCCode = "";
        $scope.BankMaster.BankAccountNo = "";
        $scope.BankMaster.AccHolderName = "";
        $scope.BankMaster.IsActive = true;
        $scope.BankMaster.Id = 0;



        if ($("#hdnPLDBId").val() != undefined && $("#hdnPLDBId").val() != "0") {
            $scope.BankMaster.SocietyID = $("#hdnPLDBId").val();
            $("#SocietyID").attr("disabled","disabled")
            $scope.PLDBBranchList().then(function () {
                if ($("#hdnBranchId").val() != "" && $("#hdnBranchId").val() != "0") {
                    $("#BranchId").attr("disabled", "disabled")
                    $scope.BankMaster.BranchId = $("#hdnBranchId").val();
                }
            });
        }

    }

    $scope.SocietyType = $("#hdnSocietyType").val();

    $scope.BackToShow = function () {
        $scope.ShowAddNew = false;
    }

    // START : Page load Function
    $scope.OnPageLoad = function () {
        //$scope.WarehouseId = $('#hdnWarehouseId').val();
        $scope._language = _language;
        //$scope.DistrictList();
        if (_language === 'en-us') {
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

        $('.SetCalender').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            minDate: '0'
        });

        var userLevel = parseInt($('#hdnUserLevel').val(), 10);
        var districtId = $('#hdnUserDistrict').val();

        if (userLevel === 3) {
            $scope.IsDisabled = true;
            $scope.GetCityList();
        }
        else {
            $scope.IsDisabled = false;
        }
    };
    // END : Page load Function






    // START : Get Data List
    $scope.BankDetailList = [];
    $scope.GetBankDetailList = function () {
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/Master/_BankDetailList", JSON.stringify({ Details: jsonParameters, Level: "0", SocietyType: $scope.SocietyType })).
            success(function (data, status) {
                $scope.BankDetailList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list")
                $(".blockUI").fadeOut("slow");
                $scope.BankDetailList = [];
            });
    };
    // END : Get Data List



    //START : Show Div For Insert / Update
    $scope.ShowDivForInsert = function () {
        $scope.ClearForm();
        $scope.GridWarehouses = false;
        $scope.divInsert = true;
    };
    //END : Show Div For Insert / Update

    //START : Back To List
    $scope.BackToList = function () {
        //$scope.ClearForm();
        $scope.BackToShow();
    };
    //END : Back To List
    $scope.Warehouse = [];
    //START : Validate Form
    $scope.ValidateForm = function () {
        var message = '';

        if ($scope.Warehouse === undefined || $scope.Warehouse.WarehousNameEn === undefined || $scope.Warehouse.WarehousNameEn.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Warehouse Name (in Eng).' : '\n <br /> कृपया वेयरहाउस नाम (अंग्रेज़ी) दर्ज करें |';
        }

        if ($scope.Warehouse === undefined || $scope.Warehouse.WarehousNameHi === undefined || $scope.Warehouse.WarehousNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Warehouse Name (in Hindi)' : '\n <br /> कृपया वेयरहाउस नाम (हिंदी) दर्ज करें  |';
        }

        if ($scope.Warehouse === undefined || $scope.Warehouse.LicenceNo === undefined || $scope.Warehouse.LicenceNo.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Licence No.' : '\n <br /> कृपया लाइसेंस नंबर दर्ज करें |';
        }

        if ($('#txtLicenceValidUpto').val().length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Licence vality date' : '\n <br /> कृपया लाइसेंस वैधता तिथि दर्ज करें |';
        }

        if ($scope.Warehouse.UrbanRural === undefined) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Urban/Rural Type' : '\n <br /> कृपया शहरी / ग्रामीण चुने|';
        }
        if ($scope.Warehouse.IsCentralWarehouse === undefined) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Is Central Warehouse?' : '\n <br /> सेंट्रल वेयरहाउस है? चुने|';
        }

        if ($scope.Warehouse === undefined || $scope.Warehouse.DistrictId === undefined || $scope.Warehouse.DistrictId === '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select District' : '\n <br /> कृपया ज़िला चुनें|';
        }

        if ($scope.Warehouse === undefined || $scope.Warehouse.Address === undefined || $scope.Warehouse.Address.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Address' : '\n <br /> कृपया पता दर्ज करें  |';
        }


        if ($scope.Warehouse.UrbanRural === 1 || $scope.Warehouse.UrbanRural === '1') {

            if ($scope.Warehouse === undefined || $scope.Warehouse.CityId === undefined || $scope.Warehouse.CityId === '0') {
                message += $scope._language === 'en-us' ? '\n <br /> Please Select City' : '\n <br /> कृपया शहर चुनें|';
            }
        }

        if ($scope.Warehouse.UrbanRural === 2 || $scope.Warehouse.UrbanRural === '2') {

            if ($scope.Warehouse === undefined || $scope.Warehouse.BlockId === undefined || $scope.Warehouse.BlockId === '0') {
                message += $scope._language === 'en-us' ? '\n <br /> Please Select Block/Tehsil' : '\n <br /> कृपया ब्लाक चुनें|';
            }
        }

        if (message.length > 5) {

            ShowModelAlert($scope.translation.Warehouse, message);
            return false;
        }
        return true;
    };
    //END : Validate Form

    //START : Save/Edit Data
    $scope.Insert = function () {
        //$(".se-pre-con").fadeIn("slow");

        if (!$scope.ValidateBankDetailsMaster()) {

            return false;
        } else {
            $(".se-pre-con").fadeIn("slow");
            var model = {
                BankDetails: {}
            }
            model.BankDetails = {
                SocietyID: $scope.BankMaster.SocietyID,
                BranchId: $scope.BankMaster.BranchId,
                BankID: $scope.BankMaster.BankID,
                BankBranch: $scope.BankMaster.BankBranch,
                IFSCCode: $scope.BankMaster.IFSCCode,
                BankAccountNo: $scope.BankMaster.BankAccountNo,
                AccHolderName: $scope.BankMaster.AccHolderName,
                IsActive: $scope.BankMaster.IsActive,
                Id: $scope.BankMaster.Id == undefined ? 0 : $scope.BankMaster.Id,
                SocietyType: $scope.SocietyType
            };
            console.log(model);
            $http.post(SiteUrl + "/Master/SaveBankDetails/", { "objBankDetailsVM": model }).
                success(function (data, status) {
                    if (data > 0) {
                        $(".se-pre-con").fadeOut("slow");
                        alert(_language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                        $scope.GetBankDetailList();
                        $scope.BackToList();
                    }
                    else if (parseInt(data, 10) === -1) {
                        $(".se-pre-con").fadeOut("slow");
                        ShowModelAlert($scope.translation.GoDown, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                    }
                    else if (parseInt(data, 10) === -2) {
                        $(".se-pre-con").fadeOut("slow");
                        ShowModelAlert($scope.translation.GoDown, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
                    }
                }).
                error(function (data1, status) {
                    $(".se-pre-con").fadeOut("slow");
                });
        }
    };

    $scope.ValidateBankDetailsMaster = function () {

        var SocietyId = $("#SocietyID").removeClass("notValid").val();
        var PLDBBranchId = $("#BranchId").removeClass("notValid").val();
        var ApplicationNo = $("#ApplicationNo").removeClass("notValid").val();
        var RegID = $("#RegID").removeClass("notValid").val();
        var BankName = $("#BankName").removeClass("notValid").val();
        var BankID = $("#BankID").removeClass("notValid").val();
        var BankBranch = $("#BankBranch").removeClass("notValid").val();
        var IFSCCode = $("#IFSCCode").removeClass("notValid").val();
        var BankAccountNo = $("#BankAccountNo").removeClass("notValid").val();
        var AccHolderName = $("#AccHolderName").removeClass("notValid").val();

        var message = _language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        //if (SocietyId == 0) {
        //    message += _language == 'en-us' ? '\n <br />Please select Society.' : '\n <br />कृपया सेक्टर का नाम चुनें।';
        //    $("#SocietyID").addClass("notValid");
        //}

        //SocietyID: BankMaster.SocietyID,
        //    BranchId: $scope.BankMaster.BranchId,
        //        BankID: $scope.BankMaster.BankID,
        //            BankBranch: $scope.BankMaster.BankBranch,
        //                IFSCCode: $scope.BankMaster.IFSCCode,
        //                    BankAccountNo: $scope.BankMaster.BankAccountNo,
        //                        AccHolderName: $scope.BankMaster.AccHolderName,
        //                            IsActive: $scope.BankDetails.IsActive,
        //                                BankDetails: $scope.BankDetails.BankDetails,


        //if (ApplicationNo == 0) {
        //    message += _language == 'en-us' ? '\n <br />Please enter Application No.' : '\n <br />कृपया आवेदन संख्या दर्ज करें।';
        //    $("#ApplicationNo").addClass("notValid");
        //}

        //if (RegID == "") {
        //    message += _language == 'en-us' ? '\n <br />Please enter RegID.' : '\n <br />कृपया पंजीकरण आईडी दर्ज करें।';
        //    $("#RegID").addClass("notValid");
        //}

        if ($scope.BankMaster.BankID == 0) {
            message += _language == 'en-us' ? '\n <br />Please select Bank.' : '\n <br />कृपया बैंक का चयन करें।';
            $("#BankID").addClass("notValid");
        }

        if (BankBranch == "") {
            message += _language == 'en-us' ? '\n <br />Please enter your Bank Branch name.' : '\n <br />कृपया अपना बैंक शाखा नाम दर्ज करें।।';
            $("#BankBranch").addClass("notValid");
        }
        if (IFSCCode == "") {
            message += _language == 'en-us' ? '\n <br />Please enter IFSC Code.' : '\n <br />कृपया IFSC कोड दर्ज करें।';
            $("#IFSCCode").addClass("notValid");
        }

        if (BankAccountNo == "") {
            message += _language == 'en-us' ? '\n <br />Please enter Bank Account No.' : '\n <br />कृपया बैंक खाता संख्या दर्ज करें।।';
            $("#BankAccountNo").addClass("notValid");
        }
        if (AccHolderName == 0) {
            message += _language == 'en-us' ? '\n <br />Please enter Account Holder Name.' : '\n <br />कृपया खाता धारक का नाम दर्ज करें।';
            $("#AccHolderName").addClass("notValid");
        }

        if ($scope.BankMaster.SocietyID == undefined || $scope.BankMaster.SocietyID == "0" || $scope.BankMaster.SocietyID == 0) {
            message += _language == 'en-us' ? '\n <br /> Please  Select the ' + ($scope.SocietyType == 1 ? 'PLDB' : 'Society') + '.' : '\n <br /> कृपया ' + ($scope.SocietyType == 1 ? 'पी.एल.डी.बी.' : 'सोसाइटी') +'   का  चयन  करें।';
            $("#SocietyID").addClass("notValid");
        }
        if ($scope.SocietyType == 1) {
            if ($scope.BankMaster.BranchId == "" || $scope.BankMaster.BranchId == "0" || $scope.BankMaster.BranchId == 0) {
                message += _language == 'en-us' ? '\n <br /> Please  Select the PLDB Branch.' : '\n <br /> कृपया पी.एल.डी.बी. ब्रांच चयन  करें।';
                $("#BranchId").addClass("notValid");
            }
        }


        if (message.length > 29) {
            ShowModelAlert("Sector Master", message);
            return false;
        }
        else {
            ///$("#formSavebankDetail").submit();
        }
        return true;
    }



    //END : Save/Edit Data

    //START : Edit
    $scope.Edit = function (item) {
        $scope.Warehouse["WarehousId"] = item.WarehousId;
        $scope.Warehouse["WarehousNameEn"] = item.WarehousNameEn;
        $scope.Warehouse["WarehousNameHi"] = item.WarehousNameHi;
        $scope.Warehouse["LicenceNo"] = item.LicenceNo;
        $('#txtLicenceValidUpto').val(item.LicenceValidUpTo);
        $scope.Warehouse["UrbanRural"] = item.ISCITY === true || item.ISCITY === 'TRUE' || item.ISCITY === 'True' || item.ISCITY === '1' || item.ISCITY === 1 ? '1' : '2';
        $scope.Warehouse["IsCentralWarehouse"] = item.IsCentralWarehouse === true || item.IsCentralWarehouse === 'TRUE' || item.IsCentralWarehouse === 'True' || item.IsCentralWarehouse === '1' || item.IsCentralWarehouse === 1 ? '1' : '2';

        // $scope.DistrictList();
        $scope.Warehouse["DistrictId"] = item.DISTRICT_ID;

        if (item.ISCITY === true || item.ISCITY === 'TRUE' || item.ISCITY === 'True' || item.ISCITY === '1' || item.ISCITY === 1) {
            $scope.GetCityList();
            $scope.Warehouse["CityId"] = item.CITY_ID;
        }
        else {
            $scope.BlockList();
            $scope.Warehouse["BlockId"] = item.BLOCK_ID;
        }

        $scope.Warehouse["Address"] = item.Address;
        $scope.GridWarehouses = false;
        $scope.divInsert = true;
    };
    //END : Edit

    //START : Set Status
    $scope.SetStatus = function (Id, ActionId) {
        $(".se-pre-con").fadeIn("slow");

        if (ActionId === 2) {
            var confirmMsg = _language === 'en-us' ? 'Are you sure to delete this record?' : 'क्या आप इस रिकॉर्ड को हटाना सुनिश्चित कर रहे हैं?';

            if (!confirm(confirmMsg)) {
                $(".se-pre-con").fadeOut("slow");
                return false;
            }
        }

        $http.post(SiteUrl + "/Master/RajfedWarehouseSetStatus/", { "WarehousId": Id, "ActionId": ActionId }).
            success(function (data, status) {
                if (data === -1) {
                    ShowModelAlert($scope.translation.Warehous, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else {
                    $scope.WarehouseList();
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    //END : Set Status

    // START : Get List of PLDB
    $scope.PLDBList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=-1").
            success(function (data, status) {
                $scope.PLDB = data;
            }).
            error(function (data, status) {
                $scope.PLDB = [];
            });
    };
    //END : Get List of PLDB

    // START : Get List of Society
    $scope.SocietyList = [];
    $scope.GetSocietyList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GSS_DAIRY&MasterCode=-1").
            success(function (data, status) {
                $scope.SocietyList = data;
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
            });
    };
    //END : Get List of Society

    if ($scope.SocietyType == 1) {
        $scope.PLDBList();
    } else if ($scope.SocietyType == 2) {
        $scope.GetSocietyList();
    }

    // START : Bank  List 
    $scope.BankMasterList = [];
    $scope.MasterList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=BANKLIST&MasterCode=-1").
            success(function (data, status) {
                $scope.BankMasterList = data;
            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
            });
    };
    $scope.MasterList();
    //END : Bank  List 


    $scope.PLDBBranches = [];
    $scope.PLDBBranchList = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.BankMaster.SocietyID).
            success(function (data, status) {
                $scope.PLDBBranches = data;

               // $scope.BankMaster.Branch_ID = branch_ID.toString();
               // $scope.BankMaster.Branch_ID = '0';
                //$scope.PropertyDetailsList[0].TehsilList = data;
                //$scope.PropertyDetailsList[0].Tehsil = '0';;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    // START : On Page Load
    $scope.OnPageLoad();
    $scope.GetBankDetailList();
    // END : On Page Load

    $scope.update = function (item)  {
        //$(".blockUI").fadeIn("slow");
        // $("#addModal").modal("show");
        $scope.ShowForm();

        $scope.BankMaster.SocietyID = item.SocietyID.toString();
        $scope.BankMaster.BranchId = item.BranchId.toString();;
        $scope.BankMaster.BankID = item.BankID.toString();
        $scope.BankMaster.BankBranch = item.BankBranch;
        $scope.BankMaster.IFSCCode = item.IFSCCode;
        $scope.BankMaster.BankAccountNo = item.BankAccountNo;
        $scope.BankMaster.AccHolderName = item.AccHolderName;
        $scope.BankMaster.IsActive = item.IsActive ==1? true : false;
        $scope.BankMaster.Id = item.Id;

        $scope.PLDBBranchList().then(function () {
            $scope.BankMaster.BranchId = item.BranchId.toString();
        });

        //$.get("../Master/_BankDetails?id=" + id, function (data, status) {
        //    $("#hdnID").val(data.Id);
        //    $("#SocietyID").val(data.SocietyID);
        //    $("#ApplicationNo").val(data.ApplicationNo);
        //    $("#RegID").val(data.RegID);
        //    $("#BankName").val(data.BankName);
        //    $("#BankID").val(data.BankID);
        //    $("#BankBranch").val(data.BankBranch);
        //    $("#IFSCCode").val(data.IFSCCode);
        //    $("#CreatedDate").val(data.CreatedDate);
        //    $("#BankAccountNo").val(data.BankAccountNo);
        //    $("#AccHolderName").val(data.AccHolderName);
        //    $("#IsActive").val(data.IsActive);
        //    $("#ModelList").hide();
        //    $("#addModal").show();
        //    $("#addModal").removeClass("modal-backdrop");
        //    // $("#dvmodelBody").html(data);
        //    $(".blockUI").fadeOut("slow");
        //})
    }

    //Delete Bank Details
    $scope.deleteDetails = function (id, IsActive, type) {
        if (IsActive == 2)
            if (!confirm("Are you sure you to delete record !!"))
                return false;

        $(".blockUI").fadeIn("slow");
        $.post("../Master/DeleteBankDetails?Id=" + id + "&ActionId=" + IsActive + '&Type=' + type, function (data, status) {
            //location.reload();
            if (data > 0 && IsActive == 2) {
                //alert('Record delete sucessfully!');
                $scope.GetBankDetailList();
                $scope.BackToList();
            }
            $(".blockUI").fadeOut("slow");
        })
    }

    

});