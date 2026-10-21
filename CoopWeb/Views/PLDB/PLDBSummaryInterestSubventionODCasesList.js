
var MasterMod = angular.module('ApplicationFlowODCasesModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApplicationFlowODCasesCont", function ($scope, $rootScope, $http) {
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
    $scope.PendingLevelList = [];
    $scope.MLPendingLevelList = [];


    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        if ($scope.Filter.FilterType == 'wrs.created_date') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;            
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }
    $scope.ClearSearch = function () {
        $scope.DISTRICT_ID = "";
        $scope.PLDB_ID = "0";
        $scope.PLDB_Branch_ID = "0";
        $scope.q = "";       
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        window.location = "../PLDB/PLDBSummaryInterestSubventionODCasesList";
    }
    /*------------End Paging and Filter-----------------*/
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;

    $scope.GetPendingLevelList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=OTSPENDINGLEVEL").
            success(function (data, status) {
                $scope.PendingLevelList = data;
            }).
            error(function (data, status) {
                $scope.PendingLevelList = [];
            });
    };

    $scope.GetMLPendingLevelList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=OTSPENDINGLEVEL&MasterCode=1").
            success(function (data, status) {
                data.forEach(function (item) {
                    if (item.OtsPendingLevelId == 6) {
                        item.Name = 'Mortgage Done';
                    }
                });
                $scope.MLPendingLevelList = data;
            }).
            error(function (data, status) {
                $scope.MLPendingLevelList = [];
            });
    };
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $scope.DISTRICT_ID = $("#hdnDistrictId").val();
                    $scope.PLDBListData();
                    $("#ddrDistrictId").hide();
                }                

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };
    $scope.GetDistrictList();


    $scope.DeletePLDB = function (id) {        

        if (confirm("Are you sure you want to delete this record?")) {

            $http.post(SiteUrl + "/PLDB/DeletePldbLoanSubsidyApplication", { id: id })
                .success(function () {
                    alert("Record deleted successfully");
                    $scope.GetOTSList();

                });

        }

    };

    $scope.GetOTSList = function () {
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/PLDB/PLDBApplicationInterestSubventionODCasesList", JSON.stringify({ Details: jsonParameters })).
            success(function (responcedata, status) {
                 
                var data = JSON.parse(responcedata.DataList);             
                $scope.OTSList = data;
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
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });

    };


    $scope.exportAllToExcel = function () {
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, PageSize: 100000, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

        $http.post(SiteUrl + "/PLDB/PLDBApplicationInterestSubventionODCasesList", jsonParameters)
            .success(function (data) {
                var allData = JSON.parse(data.DataList);
                if (allData.length == 0) {
                    alert("No records found");
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }
                exportJsonToExcel(allData, "PLDB Loan Subsidy Application List");
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                alert("Error while exporting data");
                $(".se-pre-con").fadeOut("slow");
            });
    };

    function exportJsonToExcel(data, fileName) {

        var allowedColumns = [
            "PLDBName",
            "PLDBBranch",
            "ApplicantName",
            "FatherHusbandName",
            "MobileNo",
            "Village",
            "NomineeName",
            "NomineeMobileNo",
            "LoanAccNo",
            "Sector",
            "Purpose",
            "DateOfFirstDisbursement",
            "TotalDisbursedAmt",
            "DateOfOD",
            "ODPrincipal",
            "ODInsurancePremium",
            "ODInterest",
            "ODPenalInterast",
            "ODTotal",
            "CDPrincipal",
            "CDInterest",
            "CDPenalInterest",
            "CDTotal",
            "ODCDTotal",
            "Principal",
            "OtherCharges",
            "IntSubvention",
            "ReccoveryTotalAmount"
        ];

        var filteredData = data.map(function (item, index) {

            var newItem = {};
            newItem["S.No"] = index + 1;

            allowedColumns.forEach(function (key) {
                newItem[key] = item[key];
            });

            return newItem;
        });

        var worksheet = XLSX.utils.json_to_sheet(filteredData);
        var workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, "Data");
        XLSX.writeFile(workbook, fileName + ".xlsx");
    }




    $scope.GetOtsProcessStatus = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=OTSPROCESSSTATUS&MasterCode=0").
            success(function (data, status) {
                $scope.StatusList = data;
            }).
            error(function (data, status) {
                $scope.StatusList = [];
            });
    };

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

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }

        $scope.showLoader = false;
        $scope.GetOtsProcessStatus();
        $scope.GetPendingLevelList();
        $scope.GetMLPendingLevelList();
        $scope.openLevelModal = function (item) {
            $(document).ready(function () {
                $('#pendingLevelModal').modal('show');
            });
            $scope.PLDB = item;

            $scope.PLDB.PendingLevelId = $scope.PLDB.PendingLevelId + "";
            $scope.PLDB.LandMortgagedPendingLevelId = $scope.PLDB.LandMortgagedPendingLevelId + "";
            if ($scope.PLDB.PendingLevelId == 6 && $scope.PLDB.MutationDocument != null && $scope.PLDB.MutationDocument != '') {
                if ($('#hdnRoleId').val() == 10235 || $('#hdnRoleId').val() == 10236)
                    $('#PendingLevel,#MutationNo,#MutationDate,.mutation-doc').removeAttr("disabled");
                else
                    $('#PendingLevel,#MutationNo,#MutationDate,.mutation-doc').prop("disabled", "disabled");
            }
            else {
                $('#PendingLevel,#MutationNo,#MutationDate,.mutation-doc').removeAttr("disabled");
            }
            if ($scope.PLDB.LandMortgagedPendingLevelId == 6 && $scope.PLDB.LandMortgagedMutationDocument != null && $scope.PLDB.LandMortgagedMutationDocument != '') {
                if ($('#hdnRoleId').val() == 10235 || $('#hdnRoleId').val() == 10236)
                    $('#MLPendingLevel,#MLMutationNo,#MLMutationDate,.ml-mutation-doc,.ml-mortgage-mandatory').removeAttr("disabled");

                else
                    $('#MLPendingLevel,#MLMutationNo,#MLMutationDate,.ml-mutation-doc,.ml-mortgage-mandatory').prop("disabled", "disabled");
            }
            else {
                $('#MLPendingLevel,#MLMutationNo,#MLMutationDate,.ml-mutation-doc,.ml-mortgage-mandatory').removeAttr("disabled");
            }
            if ($scope.PLDB.MutationDate != null)
                $scope.PLDB.MutationDate = new Date($scope.PLDB.MutationDate);
            if ($scope.PLDB.LandMortgagedMutationDate != null)
                $scope.PLDB.LandMortgagedMutationDate = new Date($scope.PLDB.LandMortgagedMutationDate);
        }

        $scope.openCaseNotMortgageModal = function (item) {
            $(document).ready(function () {
                $('#caseNotMortgageModal').modal('show');
            });            
            $scope.PLDB = item;
            $scope.PLDB.OtsIdEnc = item.EncPldbOtsSchemeId;
        }

        $scope.SavePendingLevelData = function () {
            $scope.PLDB.MutationDocument = $('#MutationDocument').val();
            $scope.PLDB.LandMortgagedMutationDocument = $('#ML_MutationDocument').val();
            $(".se-pre-con").fadeIn("slow");
            if ($scope.PLDB.PendingLevelId == 6) {
                var mutationDateObj = new Date(($scope.PLDB.MutationDate ?? new Date()));
                $scope.PLDB.MutationDate = mutationDateObj.getFullYear() + '/' + (mutationDateObj.getMonth() + 1) + '/' + mutationDateObj.getDate();
            }
            else
                $scope.PLDB.MutationDate = null;
            if ($scope.PLDB.LandMortgagedPendingLevelId == 6) {
                var mlMutationDateObj = new Date(($scope.PLDB.LandMortgagedMutationDate ?? new Date()));
                $scope.PLDB.LandMortgagedMutationDate = mlMutationDateObj.getFullYear() + '/' + (mlMutationDateObj.getMonth() + 1) + '/' + mlMutationDateObj.getDate();
            }
            else
                $scope.PLDB.LandMortgagedMutationDate = null;

            $http.post(SiteUrl + "/PLDB/SavePLDB_PendingLevelData", $scope.PLDB).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data > 0) {
                        alert($scope._language == 'en-us' ? 'Data Saved Sucessfully' : ' डेटा सफलतापूर्वक सहेजा गया।');
                        window.location.reload();
                    }
                    else {
                        ShowModelAlert($scope._language == 'en-us' ? 'OTS 2025-26 Scheme :: ' : 'OTS 2025-26 Scheme ::', 'An Error Occurred Please Try Again Later');
                    }
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                });
        }

        $scope.SaveCaseNotMortgageStatus = function () {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/PLDB/SaveCaseNotMortgageStatus", $scope.PLDB).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data.Result.Message == "Success") {
                        alert($scope._language == 'en-us' ? 'Data Saved Sucessfully' : ' डेटा सफलतापूर्वक सहेजा गया।');
                        window.location.reload();
                    }
                    else {
                        ShowModelAlert($scope._language == 'en-us' ? 'OTS 2025-26 Scheme :: ' : 'OTS 2025-26 Scheme ::', 'An Error Occurred Please Try Again Later');
                    }
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                });
        }

        $scope.FileUploader = function (file, e) {
            var formData = new FormData();
            formData.append("file", file[0], file[0].name);
            $(".se-pre-con").fadeIn("slow");
            $.ajax({
                type: "POST",
                url: SiteUrl + "/FileUploaders/CommonFileUploader",
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    $('#' + e).val(data.filePath);
                },
                error: function (data, status) {                    
                    $(".se-pre-con").fadeOut("slow");
                }
            });
        }

        const urlParams = new URLSearchParams(window.location.search);
        if (urlParams.get('Status')) {
            $scope.StatusId = urlParams.get('Status');
            var parameter = { Key: 'StatusId', Value: $scope.StatusId }
            $scope.DataPaging.searchParameters.push(parameter);

        }      

        //Login Wise Level ID
        var hdnLevelId = $('#hdnLevelId').val();      

        $scope.showClearButton = (parseInt(hdnLevelId) === 1);

        $scope.GetOTSList();
    };

    $scope.OnPageLoad();

    //madhuri

    $scope.PLDBList = [];
    $scope.PLDBListData = function () {
        $scope.PLDB_ID = "0";
        $scope.PLDB_Branch_ID = "0";

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=" + ($scope.DISTRICT_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBList = data;
                if ($("#hdnPldbId").val() != "0") {
                    $scope.PLDB_ID = $("#hdnPldbId").val();
                    $scope.PLDBBranchList();
                    $("#PLDB_ID").attr("disabled", "disabled");
                }
            }).
            error(function (data, status) {
                $scope.PLDBList = [];
            });
    };

    $scope.PLDBListData();

    $scope.PLDBBranches = [];
    $scope.PLDBBranchList = function (branch_ID) {
        $scope.PLDB_Branch_ID = "0";
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + ($scope.PLDB_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBBranches = data;

                if ($("#hdnPldbBranchId").val() != "0") {
                    $scope.PLDB_Branch_ID = $("#hdnPldbBranchId").val();
                    $("#PLDB_Branch_ID").attr("disabled", "disabled");
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.toggleSelection = function (OTS) {

        if (!$scope.SelectedOTSList) {
            $scope.SelectedOTSList = [];
        }
        if (OTS.selected) {
            $scope.SelectedOTSList.push(OTS);
        } else {
            $scope.CheckedAll = false;
            $scope.SelectedOTSList = $scope.SelectedOTSList.filter(
                member => member.LoanAccNo !== OTS.LoanAccNo
            );
        }
        if ($scope.SelectedOTSList.length > 0)
            $scope.chkBulkSelected = true;
        else $scope.chkBulkSelected = false;
    };

    $scope.CheckedAllOTS = function () {
        $scope.SelectedOTSList = [];
        if ($scope.CheckedAll) {
            angular.forEach($scope.OTSList, function (item) {
                item.selected = true;
            });
            $scope.SelectedOTSList = $scope.OTSList;
        }
        else {
            angular.forEach($scope.OTSList, function (item) {
                item.selected = false;
            });
        }

        if ($scope.SelectedOTSList.length > 0)
            $scope.chkBulkSelected = true;
        else $scope.chkBulkSelected = false;
        //  $scope.DownloadMultipleOTSForm();
    }

    $scope.downloadFile = function (OTS) {
        angular.forEach($scope.OTSList, function (item) {
            item.selected = false;
        });
        $scope.SelectedOTSList = [];
        $scope.SelectedOTSList.push(OTS);
        $scope.DownloadMultipleOTSForm();
    }


    $scope.DownloadMultipleOTSForm = function () {
        if ($scope.SelectedOTSList.length <= 0) {
            alert("Please select Pdf which should be download");
            return;
        }
        $scope.SelectedOTS = {};
        $scope.SelectedOTS.PLDB_OTSSchemeData = $scope.SelectedOTSList;

        $http.post(SiteUrl + "/PLDB/BulkOTSFilesGenerated", { "_Model": $scope.SelectedOTS }, {
            responseType: 'arraybuffer'
        }).

            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.byteLength > 0) {

                    const blob = new Blob([data], { type: 'application/pdf' });

                    // Create a download link
                    const url = URL.createObjectURL(blob);
                    const a = document.createElement('a');
                    a.href = url;
                    var filename = "";
                    if ($scope.SelectedOTSList.length == 1) {
                        filename = $scope.SelectedOTSList[0].ApplicantName + "_" + $scope.SelectedOTSList[0].LoanAccNo.replace(/\//g, '-') + ".pdf";
                    }
                    else if ($scope.SelectedOTSList.length > 1 && $("#hdnPldbBranchId").val() != "0") {
                        filename = $scope.SelectedOTSList[0].PLDBBranch + "OtsApplicationList.pdf";

                    }
                    else if ($scope.SelectedOTSList.length > 1 && $("#hdnPldbId").val() != "0") {
                        filename = $scope.SelectedOTSList[0].PLDBName + "OtsApplicationList.pdf";

                    }
                    else {
                        filename = "BulkOtsApplicationList.pdf";
                    }
                    a.download = filename;
                    document.body.appendChild(a); // Needed for Firefox
                    a.click();
                    document.body.removeChild(a);
                    URL.revokeObjectURL(url);

                    angular.forEach($scope.OTSList, function (item) {
                        item.selected = false;
                    });
                    $scope.SelectedOTSList = [];
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


    // $scope.PLDBList();
    $scope.UploadeReportFileInDB = function (OTS) {

        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();
        if (OTS.DocId == 0)
            OTS.DocId = 1;//AcceptanceLetter
        // $scope.OTS.id = 0;
        formData.append("id", 0);
        formData.append("DocId", OTS.DocId);
        formData.append("OTSId", OTS.PLDB_OTS_SchemeID);
        formData.append("OTSReportFile", $scope.OTSReportFile);

        $http.post(SiteUrl + "/PLDB/SaveOTSVerifyReportFile", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //$scope.ApplicationFlowMaster.DocumentURL = data.fileName;
                //$scope.ApplicationFlowMaster.DocId = data.docId;
                $scope.GetOTSList();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }


    $scope.UploadReportFile = function (OTS, file) {
        if (!confirm('Are you sure you want to upload file?')) {
            var fileInputs = document.getElementsByClassName('fileuploader');

            for (var i = 0; i < fileInputs.length; i++) {
                fileInputs[i].value = '';
            }
            return;
        }

        var maxSize = 5 * 1024 * 1024; // element.getAttribute("checkmaxsize") 
        if (file[0].size > maxSize) { return; }

        if (file) {
            if (file[0].type === 'application/pdf') {
                $scope.OTSReportFile = file[0];
                $scope.UploadeReportFileInDB(OTS);
            } else {
                alert('Please upload a PDF file only.');
            }
        }

    }

    $scope.Redirect = function (eId, type) {
        if (eId != '') {

            if (type == 'PreLoanPayment')
                window.location = "../PLDB/PldbPrLoanPayment?otsEncId=" + eId;
            else if (type == 'FRPayment' || type == 'GenCertificate')
                window.location = "../PLDB/PldbFinalLoanPayment?otsEncId=" + eId;
            else if (type == 'Edit')
                window.location = "../PLDB/OTSApplicationForm?eId=" + eId;
            else if (type == 'View')
                window.location = "../PLDB/OTSApplicationForm?eId=" + eId + "&mode=v";

            if (type == 'UploadTaamil')
                window.location = "../PLDB/PldbTaamil?otsEncId=" + eId;

        }
    }

});

//$scope.tableToExcel = function () {
//    $scope.GetOTSList(true);
//}
function tableToExcel(tableid, filename = '') {

    var table = document.getElementById(tableid);
    var html = table.outerHTML.replace(/ /g, '%20');

    var file = filename ? filename + '.xls' : 'excel_data.xls';

    var downloadlink = document.createElement("a");
    document.body.appendChild(downloadlink);

    downloadlink.href = 'data:application/vnd.ms-excel,' + html;
    downloadlink.download = file;

    downloadlink.click();
    document.body.removeChild(downloadlink);
}

MasterMod.filter('inrCurrency', function () {
    return function (input) {
        if (!input)
            return input;
        var num = parseFloat(input);
        if (isNaN(num)) return input;
        return num.toLocaleString('en-IN', { style: 'currency', currency: 'INR' });
    };
});
