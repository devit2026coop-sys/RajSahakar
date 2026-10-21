var MasterMod = angular.module('ApplicationFlowModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApplicationFlowCont", function ($scope, $rootScope, $http) {
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
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else if ($(this).attr("type") == "radio" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
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
                //callback();               
                $scope.GetAppliFlowList(false);

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
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else if ($(this).attr("type") == "radio" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                $scope.DataPaging.currentPageID = 1;
                $scope.GetAppliFlowList(false);
                // callback();
                //$scope.pageRequest();
            }
            ++indx;
        });
    }
    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.GccSign = {};
    $scope.signedBy = "0";
    $scope.esignLocation = "";
    $scope.esignDesignation = "";
    $scope.gcceIdEsign = "0";
    $scope.esignFileName = "";
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.eSignData = {};
    $scope.AadharNoEnc = "";
    $scope.GuarantorName = "";
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        if ($scope.Filter.FilterType == 'wrs.created_date') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();
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
        $scope.SearchData();
    }
    /*------------End Paging and Filter-----------------*/
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.ShowOTPDiv = false;
    $scope.ObjectionFlagBasic = false;
    $scope.ObjectionFlagAdmin = false;
    $scope.ObjectionFlagOffice = false;
    $scope.RoleId = 0;
    $scope.dataPayload = {};
    $scope.esignData = {};

    //Madhuri


    $scope.ApplicationView = function (eId, eRno) {
        if (eId != '' && eRno != '') {
            // var url = "../GopalCreditCard/GopalApplicationFormView?eId=" + eId + "&eRno=" + eRno;
            var url = "../GopalCreditCard/GopalApplicationFormView?eId=" + eId;

            window.open(url, '_blank');
        }
    }
    $scope.ApplicationActionView = function (eId, eRno) {
        if (eId != '' && eRno != '') {
            // var url = "../GopalCreditCard/GopalApplicationFormActionView?eId=" + eId + "&eRno=" + eRno;
            var url = "../GopalCreditCard/GopalApplicationFormActionView?eId=" + eId;

            window.open(url, '_blank');
        }
    }

    $scope.MokaReportView = function (eId) {
        var url = "../GopalCreditCard/Moka_report?eId=" + eId;
        window.open(url, '_blank');     // Open the URL in a new tab
    }


    $scope.BranchLevelReportView = function (eId) {
        var url = "../GopalCreditCard/BranchLevel_Report?eId=" + eId;
        window.open(url, '_blank');     // Open the URL in a new tab
    }


    $scope.GetAppliFlowList = function (isExport) {
        $(".se-pre-con").fadeIn("slow");

        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/GopalCreditCard/GetGCCFlowList", JSON.stringify({ Details: jsonParameters, isExport: isExport })).
            //$http.get(SiteUrl + "/ApplicationFlow/GetSocietyAppliFlowList").
            success(function (responcedata, status) {
                if (isExport) {
                    $scope.downloadFile(responcedata.DataList);
                }
                else {
                    var data = JSON.parse(responcedata.DataList);

                    if (data.length > 0) {

                        $scope.ApplicationFlowList = data;
                        //console.log($scope.ApplicationFlowList);
                        $rootScope.DataPagingDetails = responcedata.DataPaging;
                        $rootScope.SetDataPaging();

                        $scope.colsort = "";
                        $scope.reverseSort = false;
                        $scope.sortData = function (column) {
                            $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                            $scope.colsort = column;
                        };
                        $(".se-pre-con").fadeOut("slow");
                        $scope.getSortClass = function (column) {
                            if ($scope.colsort == column) {
                                return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                            }
                            return '';
                        };

                    } else {
                        $scope.ApplicationFlowList = [];
                        $rootScope.DataPagingDetails = {};
                        $(".se-pre-con").fadeOut("slow");
                    }
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {
                $scope.ApplicationFlowList = [];
                $rootScope.DataPagingDetails = {};
                $(".se-pre-con").fadeOut("slow");
            });

    };
    //end madhuri code

    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $scope.DISTRICT_ID = $("#hdnDistrictId").val();
                    $("#ddrDistrictId").hide();//("disabled", "disabled");
                }
                // $scope.BindFY();

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };
    $scope.GetDistrictList();



    $scope.GSSProceesStatusList = function (StatusId, GccId) {
        $scope.GccProceesStatus_Temp = [];
        $http.post("../gopalCreditCard/GSSProceesStatusList", { "StatusId": StatusId, "GCC_Id": 0 }).
            success(function (data, status) {
                $scope.StatusList = data;

            }).
            error(function (data, status) {
                $scope.StatusList = [];
            });
    };

    $scope.GSSProceesStatusList();

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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

        $scope.showLoader = false;
    };
    //$scope.SelectRadioBtn = function () {
    //    var url = window.location.search;
    //    $scope.statusId = (url.split('=')[1]);

    //    if ($scope.statusId == undefined) {
    //        $scope.searchRequest($scope.GetAppliFlowList);
    //    }
    //    else if ($scope.statusId == 'PL') {
    //        $("#zero").prop("checked", true);
    //        $scope.searchRequest($scope.GetAppliFlowList);
    //    }
    //    else if ($scope.statusId == 'TS') {
    //        $("#five").prop("checked", true);
    //        $scope.searchRequest($scope.GetAppliFlowList);
    //    }
    //    else if ($scope.statusId == 'RS') {
    //        $("#two").prop("checked", true);
    //        $scope.searchRequest($scope.GetAppliFlowList);
    //    }
    //    else if ($scope.statusId == 'PS') {
    //        $("#four").prop("checked", true);
    //        $scope.searchRequest($scope.GetAppliFlowList);
    //    }
    //    else if ($scope.statusId == 'UP') {
    //        $("#four").prop("checked", true);
    //        $scope.searchRequest($scope.GetAppliFlowList);
    //    }
    //    else if ($scope.statusId == 'PP') {
    //        $("#four").prop("checked", true);
    //        $scope.searchRequest($scope.GetAppliFlowList);
    //    }
    //    else if ($scope.statusId === 'RejS') {
    //        $("#one").prop("checked", true);
    //        $scope.searchRequest($scope.GetAppliFlowList);
    //    }
    //    if ($scope.statusId == undefined) {
    //        $scope.selectedStatus = "PS";
    //    }

    //}


    $scope.selectedStatus = "PS";
    $scope.levelId = $('#hdnLevelId').val();

    //$scope.searchRequest($scope.GetAppliFlowList(false));
    $scope.searchRequest();

    $scope.exportExcel = function () {
        // $scope.GetGccApplicationList(true);
        $scope.GetAppliFlowList(true);
    }

    $scope.downloadFile = function (fileUrl) {
        const fileName = fileUrl.split('/').pop();
        $http({
            url: fileUrl,
            method: 'GET',
            responseType: 'blob'
        }).then(function (response) {
            var blob = new Blob([response.data], { type: response.headers('Content-Type') });
            var downloadUrl = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = downloadUrl;
            a.download = fileName;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }).catch(function (error) {
            console.error('Error downloading file:', error);
        });
    };

    $scope.selectedGccIds = []; // Array to hold selected GccIds

    //$scope.toggleSelection = function (gccId) {
    //    const idx = $scope.selectedGccIds.indexOf(gccId);
    //    if (idx > -1) {
    //        // If already selected, remove from the array
    //        $scope.selectedGccIds.splice(idx, 1);
    //    } else {
    //        // Otherwise, add to the array
    //        $scope.selectedGccIds.push(gccId);
    //    }
    //    if ($scope.selectedGccIds.length>0) $scope.chkBulkSelected = true;
    //    else $scope.chkBulkSelected = false;
    //};

    $scope.toggleSelection = function (app) {
        //debugger;
        if (!$scope.SelectedAppflowList) {
            // Initialize if it's still undefined
            $scope.SelectedAppflowList = [];
        }
        // Toggle the 'selected' property
        //app.selected = !app.selected;

        if (app.selected) {
            // If selected, add to SelectedAppflowList
            $scope.SelectedAppflowList.push(app);
        } else {
            // If deselected, remove from SelectedAppflowList
            $scope.SelectedAppflowList = $scope.SelectedAppflowList.filter(
                member => member.GccIdDBEnc !== app.GccIdDBEnc
            );
        }

        if ($scope.SelectedAppflowList.length > 0) $scope.chkBulkSelected = true;
        else $scope.chkBulkSelected = false;
    };



    // Optional: Function to check selected GccIds
    $scope.getSelectedGccIds = function () {
        //console.log($scope.selectedGccIds);
    };



    $scope.StatusChange = function () {
        var ddlStatus = $scope.ddlStatus;
        if (ddlStatus != '')
            $scope.GccProceesStatus = $scope.GccProceesStatus_Temp.filter(x => x.ProcessStatusType == ddlStatus);

        $scope.Appstatus1 = true;
        if ($scope.GccProceesStatus.length == 1) {
            $scope.Appstatus1 = false;
            $scope.ApplicationFlowMaster.StatusId = $scope.GccProceesStatus[0].GccProceesStatusId;
        }

        $scope.ApplicationFlowMaster.LoanAmountRecommendedByDCS = 0;
        $scope.ApplicationFlowMaster.PurposeOfLoan = "0";
        $scope.ApplicationFlowMaster.PurposeOfLoanOther = '';
    }
    $scope.getDistinctProceesStatus = function () {
        const statusMap = new Map();
        $scope.GccProceesStatus_Temp.forEach(item => {
            if (!statusMap.has(item.ProcessStatusType)) {
                statusMap.set(item.ProcessStatusType, item);
            }
        });
        return Array.from(statusMap.values());
    };
    $scope.GSSProceesStatusList = function (StatusId, GccId) {
        $scope.GccProceesStatus_Temp = [];
        StatusId = 8;
        $http.post("../gopalCreditCard/GSSProceesStatusList", { "StatusId": StatusId, "GCC_Id": GccId }).
            success(function (data, status) {
                $scope.GccProceesStatus_Temp = data;
                $scope.ProceesStatus = $scope.getDistinctProceesStatus();

                if ($scope.ProceesStatus.length == 1) {
                    $scope.Appstatus1 = false;
                    $scope.ApplicationFlowMaster.StatusId = $scope.ProceesStatus[0].GccProceesStatusId;
                }
            }).
            error(function (data, status) {
                $scope.GccProceesStatus_Temp = [];
            });

    };

    $scope.GSSProceesStatusList();

    $scope.SaveGSSApplicationFlowStatus = function () {
        if ($scope.ApplicationFlowMaster.StatusId == 0 || $scope.ApplicationFlowMaster.StatusId == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Status ' : '\n <br /> कृपया स्थिति जोड़ें|';
        }
        if ($scope.ApplicationFlowMaster.Remark == '' || $scope.ApplicationFlowMaster.Remark == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Fill Remark ' : '\n <br /> कृपया टिप्पणी भरें|';
        }
        if ($scope.SelectedAppflowList.length) { }

        // $scope.SelectedGcc = {};
        for (var i = 0; i < $scope.SelectedAppflowList.length; i++) {

            $scope.SelectedAppflowList[i].SanctionAmt = $scope.SelectedAppflowList[i].ReqLoanAmount;
            $scope.SelectedAppflowList[i].gcc_id = $scope.SelectedAppflowList[i].GopalCreditCardId;
        }
        $scope.SelectedGcc = {};
        $scope.SelectedGcc.GopalCreditCardFormModel = $scope.SelectedAppflowList;



        $http.get("../ApplicationFlow/GenerateAadhaarOTPForEsing").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.indexOf('NO#') >= 0) {
                    alert("Due to some issue OTP not send");
                    //location.reload();
                    //window.location = '../ApplicationFlow/ApplicationFlow';
                } else {
                    if (data != "LOGOUT") {
                        // alert(data);
                        $scope.ApplicationFlowMaster.TXN = data;
                        alert(_language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n');
                        $scope.ShowOTPDiv = true;
                    }
                    else {
                        $(".se-pre-con").fadeOut("slow");
                        window.location = 'https://sso.rajasthan.gov.in/signin';
                    }
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
            });
    }


    $scope.VerifyOTP = function () {

        $(".se-pre-con").fadeIn("slow");
        //$http.post(SiteUrl + "/gopalCreditCard/SaveGSSApplicationFlowStatus", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "GccId": $scope.GGG_Id, "Remark": $scope.ApplicationFlowMaster.Remark, "StatusId": $scope.ApplicationFlowMaster.StatusId, "UserId": $scope.ApplicationFlowMaster.UserId, "DocResStatus": DocumentCheck, "_Model": $scope.GopalCreditCardModel }).
        $http.post(SiteUrl + "/gopalCreditCard/BulkUploadApplicationFlowStatus_V1", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "_Model": $scope.SelectedGcc, "_AppFlowModel": $scope.ApplicationFlowMaster }).

            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data > 0) {
                    ShowOkMessage("Success", _language == 'en-us' ? 'Data saved successfully\n' : 'डेटा सहेजा गया है\n', function () { window.location.reload(); });
                }
                else if (data == -1) {
                    ShowOkMessage("Error", _language == 'en-us' ? 'The application cannot be forwarded because there is no user specified \n' : 'आवेदन अग्रेषित नहीं किया जा सकता क्योंकि कोई उपयोगकर्ता निर्दिष्ट नहीं है \n');
                }
                else if (data == -2) {
                    ShowOkMessage("Error", _language == 'en-us' ? 'Invalid OTP! \n' : 'अवैध ओटीपी!\n');
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("Error", _language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');

                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while insert");
            });

    }
    $scope.GetApplicationFlowGCC = function (GGG_Id) {
        $scope.showLoader = true;
        $http.get("../gopalCreditCard/GetApplicationFlowGCC/?GCC_Id=" + GGG_Id).
            success(function (data, status) {
                //console.log(data);
                $scope.ApplicationFlowGCC = JSON.parse(data.MainFlow);
                $scope.ApplicationFlowMaster.UserId = $scope.ApplicationFlowGCC[0].UserId; //last UserId
                $scope.ApplicationFlowMaster.RoleId = $scope.ApplicationFlowGCC[0].RoleID; //last RoleID


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
        $scope.showLoader = false;
    }

    $scope.GetUserForGccAssign = function (GGG_Id) {


        $scope.OfficerNotAssignData = [];
        $scope.DivTextForNotMaaped = "";
        return $http.post(SiteUrl + "/gopalCreditCard/GetUserForGccAssign", { "GccId": GGG_Id }).
            success(function (data, status) {

                $scope.OfficerNotAssignData = JSON.parse(data.DataList);
                if ($scope.OfficerNotAssignData.length <= 0) {
                    $scope.DivTextForNotMaaped = "* Officer not found for this application. Please make sure that officer of your district assigned ";
                } else {
                    $scope.DivTextForNotMaaped = "";
                }
            }).
            error(function (data, status) {
                $scope.OfficerNotAssignData = [];
            });
    };

    $scope.OfficerNotAssignData = [];
    $scope.DivTextForNotMaaped = "";
    $scope.updateNotAssign = function (item) {
        $scope.showLoader = true;
        $(".se-pre-con").fadeIn("slow");
        //$scope.GetApplicationFlowGCC(item.GopalCreditCardId).then(function () {


        //});
        $scope.GetApplicationFlowGCC(item.GopalCreditCardId);
        $scope.GetUserForGccAssign(item.GopalCreditCardId).then(function () {
            $scope.GccId = item.GopalCreditCardId;
            $(".se-pre-con").fadeOut("slow");
            $("#myModalNotAssign").modal('show');

        });


        //$scope.CurrentStatusId = item.StatusId;
        $scope.GopalCreditCardModel.GopalCreditCardId = item.GopalCreditCardId;
        //$scope.ApplicationFlowMaster.ActId = item.ActId;
        //$scope.ApplicationFlowMaster.SocietyName = item.SocietyName;
        $scope.GopalCreditCardModel.RegistrationNo = item.RegistrationNo;
        $scope.GopalCreditCardModel.ProceesStatusTextEn = item.ProceesStatusTextEn;
        $scope.GopalCreditCardModel.ProceesStatusTextHi = item.ProceesStatusTextHi;
        //$scope.ApplicationFlowMaster.UserId = item.UserId;
        $scope.showLoader = false;

    };
    //int GccId, int UserId;
    $scope.SavePermanentGccData = function () {
        //   $scope.OfficerNotAssignData = [];
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/gopalCreditCard/GccAssignOfficerSave", { "GccId": $scope.GopalCreditCardModel.GopalCreditCardId, "UserId": $scope.GopalCreditCardModel.OfficerId }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data == "-1") {
                    ShowOkMessage("", "* Officer not found for this type of gcc. Please make sure that officer of your district assigned.", function () { });
                } else {
                    ShowOkMessage("", "GCC assigned successfully", function () { });
                    $scope.GetAppliFlowList();
                }

                $scope.ClearAppFlowForm();
            }).
            error(function (data, status) {
                $scope.OfficerNotAssignData = [];
            });
    };
    $scope.ClearAppFlowForm = function () {
        $scope.GopalCreditCardModel.GopalCreditCardId = 0;
        $scope.GopalCreditCardModel.RegistrationNo = '';
        $scope.GopalCreditCardModel.ProceesStatusTextEn = '';
        $scope.GopalCreditCardModel.ProceesStatusTextHi = '';
        $scope.OfficerNotAssignData = [];
        $("#myModalNotAssign").modal('hide');
    }

    $scope.checkInput = function (event) {
        var key = event.keyCode || event.which;
        var forbiddenKeys = [34, 39, 47]; // ASCII codes for ", ', and /

        if (forbiddenKeys.indexOf(key) !== -1) {
            event.preventDefault();
        }
    };



    //GCC Esign process

    $scope.DocumentEsign = function (data, signedBy) {
        $scope.eSignData = data;
        $scope.DocumentEsignView(data, signedBy);
        // $scope.signedBy = signedBy;
        //$scope.esignLocation = data.PACSNameEn;
        //$scope.esignFileName = data.ApplicantSignedForm;
        //$scope.gcceIdEsign = data.GopalCreditCardId;
        //console.log(data)
        //if (signedBy == 2) {
        //    $scope.esignDesignation = "Gurantor 1";
        //    $scope.esignFileName = data.ApplicantSignedFormGuarantor;
        //    $scope.GenerateEsignOtp(signedBy, data.G1AadharNoEnc);
        //}
        //else if (signedBy == 3) {
        //    $scope.esignDesignation = "Gurantor 2";
        //    $scope.esignFileName = data.ApplicantSignedFormGuarantor;
        //    $scope.GenerateEsignOtp(signedBy, data.G2AadharNoEnc);
        //}
        //else {
        //    $scope.esignDesignation = "PACS Manager";
        //    $scope.esignFileName = data.ApplicantSignedFormPacs;
        //    $scope.GenerateEsignOtp(signedBy, "");
        //}
    };

    $scope.GCCApplicationESign = function (signedBy, location, designation, fileName, gccid) {
        $scope.showLoader = true;
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/GopalCreditCard/GCCApplicationEsign", { "OTP": $scope.GccSign.OTP, "TXN": $scope.GccSign.TXN, "signedBy": signedBy, "location": location, "designation": designation, "fileName": fileName, "gccid": gccid }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data) {
                    $scope.showLoader = false;
                    $("#modelGccPopupOTP").modal("hide");
                    $("#modelPopupDocumentView").modal("hide");
                    if (data > 0) {
                        ShowOkMessage("Success", _language == 'en-us' ? 'Data saved successfully\n' : 'डेटा सहेजा गया है\n', function () { window.location.reload(); });
                    }
                    else if (data == -1) {
                        ShowOkMessage("Error", _language == 'en-us' ? 'The application cannot be forwarded because there is no user specified \n' : 'आवेदन अग्रेषित नहीं किया जा सकता क्योंकि कोई उपयोगकर्ता निर्दिष्ट नहीं है \n');
                    }
                    else if (data == -2) {
                        ShowOkMessage("Error", _language == 'en-us' ? 'Invalid OTP! \n' : 'अवैध ओटीपी!\n');
                    }
                    else if (data == -3) {
                        ShowOkMessage("Error", _language == 'en-us' ? 'Duplicate Jan Aadhaar Number  One Jan Aadhaar (Family) -> One Loan \n' : 'डुप्लीकेट जन आधार नंबर एक जन आधार (परिवार) -> एक ऋण\n');
                    }
                    else {
                        $(".se-pre-con").fadeOut("slow");
                        ShowOkMessage("Error", _language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');

                    }
                }
            }).error(function (data, status) {
                $scope.showLoader = false;
                $(".se-pre-con").fadeOut("slow");
                alert("Error while insert");
            });
    };

    $scope.GenerateEsignOtp = function () {
        //$scope.signedBy = $scope.signedBy;
        $scope.esignLocation = $scope.eSignData.PACSNameEn;
        $scope.esignFileName = $scope.eSignData.ApplicantSignedForm;
        $scope.gcceIdEsign = $scope.eSignData.GopalCreditCardId;
        console.log($scope.eSignData)
        if ($scope.signedBy == 2) {
            $scope.esignDesignation = "Gurantor 1";
            $scope.esignFileName = $scope.eSignData.ApplicantSignedFormGuarantor;
            $scope.AadharNoEnc = $scope.eSignData.G1AadharNoEnc;
            //$scope.GenerateEsignOtp($scope.signedBy, $scope.eSignData.G1AadharNoEnc);
        }
        else if ($scope.signedBy == 3) {
            $scope.esignDesignation = "Gurantor 2";
            $scope.AadharNoEnc = $scope.eSignData.G2AadharNoEnc;
            $scope.esignFileName = $scope.eSignData.ApplicantSignedFormGuarantor;
            //$scope.GenerateEsignOtp($scope.signedBy, $scope.eSignData.G2AadharNoEnc);
        }
        else {
            $scope.esignDesignation = "PACS Manager";
            $scope.AadharNoEnc = "";
            $scope.esignFileName = $scope.eSignData.ApplicantSignedFormPacs;
            //$scope.GenerateEsignOtp($scope.signedBy, "");
        }
        //userType, aadharNoEnc
        $(".se-pre-con").fadeIn("slow");
        $http.get("../ApplicationFlow/GenerateAadhaarOTPForEsignGcc?userType=" + $scope.signedBy + "&aadharNoEnc=" + encodeURIComponent($scope.AadharNoEnc)).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            if (data.indexOf('NO#') >= 0) {
                alert(data.replace('NO#', ''));
                $("#modelGccPopupOTP").modal("hide");
            } else {
                if (data != "LOGOUT") {
                    $scope.GccSign.TXN = data;
                    alert(_language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n');
                    $("#modelGccPopupOTP").modal("show");
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    $("#modelGccPopupOTP").modal("hide");
                }
            }
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
        });
    }

    $scope.DocumentEsignView = function (data, signedBy) {
        $scope.signedBy = signedBy
        $scope.eSignData = data;
        console.log($scope.eSignData);
        if ($scope.eSignData != null) {
            $("#modelPopupDocumentView").modal("show");
            $("#modelGccPopupOTP").modal("hide");
        };
    };

    $scope.ViewDocument = function () {
        //$scope.GGG_Id = $scope.GGG_Id;
        $scope.signedBy = $scope.signedBy;
        $scope.esignLocation = $scope.eSignData.PACSNameEn;
        $scope.esignFileName = $scope.eSignData.ApplicantSignedForm;
        $scope.gcceIdEsign = $scope.eSignData.GopalCreditCardId;
        console.log($scope.eSignData);
        //console.log($scope.GGG_Id);
        if ($scope.signedBy == 2) {
            $scope.esignDesignation = "Gurantor 1";
            $scope.esignFileName = $scope.eSignData.ApplicantSignedFormGuarantor;
            if ($scope.eSignData != null) {
                var url = "/Content/GopalCreditCard/GSSProfiling/" + $scope.eSignData.ApplicantSignedFormGuarantor;
                window.open(url, '_blank');
            }
            //$scope.GenerateEsignOtp($scope.signedBy, $scope.eSignData.G1AadharNoEnc);
        }
        else if ($scope.signedBy == 3) {
            $scope.esignDesignation = "Gurantor 2";
            $scope.esignFileName = $scope.eSignData.ApplicantSignedFormGuarantor;
            if ($scope.eSignData != null) {
                var url = "/Content/GopalCreditCard/GSSProfiling/" + $scope.eSignData.ApplicantSignedFormGuarantor;
                window.open(url, '_blank');
            }


            //$scope.GenerateEsignOtp($scope.signedBy, $scope.eSignData.G2AadharNoEnc);
        }
        else {
            $scope.esignDesignation = "PACS Manager";
            $scope.esignFileName = $scope.eSignData.ApplicantSignedFormPacs;
            if ($scope.eSignData != null && $scope.eSignData.ApplicantSignedFormPacs != null) {
                var url = "/Content/GopalCreditCard/GSSProfiling/" + $scope.eSignData.ApplicantSignedFormPacs;
                window.open(url, '_blank');
            }
            //$scope.GenerateEsignOtp($scope.signedBy, "");
        }

    }
    $scope.VerifyGccOTP = function () {
        if (!$scope.GccSign.OTP) {
            alert("Please enter valid otp");
            return;
        }

        $scope.GCCApplicationESign($scope.signedBy, $scope.esignLocation, $scope.esignDesignation, $scope.esignFileName, $scope.gcceIdEsign);
    };
    $scope.clearGccOTPModel = function () {
        $("#modelGccPopupOTP").modal("hide");
        //$("#modelPopupDocumentView").modal("hide");
        $scope.GccSign.OTP = '';
    }



    //Added New Esign Process on 13-07-2026
    $scope.RedirectToESPportal = function () {
        $("#modelPopupDocumentView").modal("hide");
        $("#modelGccPopupOTP").modal("hide");
        $scope.esignLocation = $scope.eSignData.PACSNameEn;
        $scope.esignFileName = $scope.eSignData.ApplicantSignedForm;
        $scope.gcceIdEsign = $scope.eSignData.GopalCreditCardId;

        if ($scope.signedBy == 2) {
            $scope.esignDesignation = "Gurantor 1";
            $scope.esignFileName = $scope.eSignData.ApplicantSignedFormGuarantor;
            $scope.AadharNoEnc = $scope.eSignData.G1AadharNoEnc;
            $scope.GuarantorName = $scope.eSignData.G1Name;
        }
        else if ($scope.signedBy == 3) {
            $scope.esignDesignation = "Gurantor 2";
            $scope.AadharNoEnc = $scope.eSignData.G2AadharNoEnc;
            $scope.esignFileName = $scope.eSignData.ApplicantSignedFormGuarantor;
            $scope.GuarantorName = $scope.eSignData.G2Name;
        }
        else {
            $scope.esignDesignation = "PACS Manager";
            $scope.AadharNoEnc = "";
            $scope.esignFileName = $scope.eSignData.ApplicantSignedFormPacs;
        }
        $scope.GCCApplicationEsignLatter();
    };

    $scope.GCCApplicationEsignLatter = function () {
        $scope.GCCESignApplicationProcess().then(function (response) {
            var data = response.data;
            if (!data.Success) {
                alert("Document generation failed");
                return;
            }

            $scope.esignData = {
                SiteUrl: SiteUrl,
                DocType: data.DocType,
                ReturnPath: data.ReturnPath,
                DocumentPath: data.DocPath,
                DocumentName: data.DocName,
                docid: $scope.gcceIdEsign,
                SocietyIDEnc: "",
                Remark: $scope.GuarantorName,
                StatusId: $scope.signedBy,
                IsAmendment: "",
                AmendmendAmount: "",
                ReceiptNumber: "",
                Location: $scope.esignLocation,
                Desination: $scope.esignDesignation
            };


            if (!$scope.esignData.DocumentPath || !$scope.esignData.DocumentName) {
                alert("Not valid document");
                return;
            }
            EsignCommonData.validateEsign($http, $scope.esignData, function () {
                EsignCommonData.startEsign($http, $scope.esignData, function (xml) {
                    EsignCommonData.redirectToESP(xml);
                });
            });

        }).catch(function (err) {

            console.error(err);
            alert("Error generating eSign certificate");

        });
    };

    $scope.GCCESignApplicationProcess = function () {
        $scope.showLoader = true;
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/EsignForPacsGuarantor", {
            signedBy: $scope.signedBy,
            fileName: $scope.esignFileName,
            gccid: $scope.gcceIdEsign
        }).then(function (response) {

            $scope.showLoader = false;
            $(".se-pre-con").fadeOut("slow");
            return response;
        }).catch(function (error) {

            $scope.showLoader = false;
            $(".se-pre-con").fadeOut("slow");
            alert("Error while insert");
            throw error;
        });
    };


    $scope.OnPageLoad();

});