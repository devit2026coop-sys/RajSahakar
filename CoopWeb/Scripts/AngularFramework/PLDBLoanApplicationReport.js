var CommodityWiseWRDetailModule = angular.module('PLDBLoanappReportMod', ['angularUtils.directives.dirPagination']);
CommodityWiseWRDetailModule.controller("PLDBLoanappReportCont", function ($rootScope, $scope, $http) {
    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 20,
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


    

    $scope.getTranslation = function () {
        $scope._language = _language;
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
    };

    //---------------------------------------------------------------------------
    $scope.AppLoanReport = [];
    $scope.CountList = true;
    $scope.ReportType = 'L';


    $scope.GetDetials = function (item) {
        $scope.item = item;
        $scope.searchRequest($scope.GetPLDBLoanAppCountbylist);
    }


    $scope.GetPLDBLoanAppCountbylist = function () {
        $(".blockUI").fadeIn("slow");
            var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
            return $http.post(SiteUrl + "/MIS/GetPLDBLoanListbycount", JSON.stringify({ Details: jsonParameters, ReportType: $scope.ReportType, Value: $scope.item })).
                success(function (data, status) {
                    $scope.PLDBLoanappCountbylist = JSON.parse(data.DataList);
                    $rootScope.DataPagingDetails = data.DataPaging;
                    $rootScope.SetDataPaging();
                    $scope.CountList = false;
                    $scope.SepterdList = true;
                    $(".blockUI").fadeOut("slow");
                }).
                error(function (data, status) {
                    $scope.PLDBLoanappCountbylist = [];
                    $(".blockUI").fadeIn("slow");
                    console.log('Error while fetching WR Detail data.');
                });
    

    }

    $scope.Back = function () {
        $scope.CountList = true;
        $scope.SepterdList = false;
    }


    $scope.GetReportTypeFilterList = function (type) {
        $scope.ReportType = type;
        $scope.searchRequest($scope.SearchData);
    }


    $scope.SearchData = function () {
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/MIS/GetPLDBappLoanReport", JSON.stringify({ Details: jsonParameters, ReportType: $scope.ReportType })).
            success(function (data, status) {
                $scope.PLDBLoanAppListCount = JSON.parse(data.DataList);

                if ($scope.ReportType == "L") {
                    $scope.SepterdList = true;
                    $scope.PLDBLoanappCountbylist = JSON.parse(data.DataList);
                } else {
                    $scope.SepterdList = false;
                    $scope.PLDBLoanAppListCount = JSON.parse(data.DataList);
                }


                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBLoanAppListCount = [];
                $(".blockUI").fadeIn("slow");
                console.log('Error while fetching WR Detail data.');
            });
    }

    $scope.ExportData = function () {
        $(".blockUI").fadeIn("slow");
        var functionname = _language == 'en-us' ? "GetPLDBLoanAppReportExportEn" : "GetPLDBLoanAppReportExportHi";
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": 1, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "IsAllRecords": true };
        return $http.post(SiteUrl + "/MIS/" + functionname, JSON.stringify({ Details: jsonParameters, ReportType: $scope.ReportType, Value: $scope.item })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                var excel = $JExcel.new("Calibri light 10 #333333");			// Default font	

                excel.set({ sheet: 0, value: "PLDB Loan Application Report" });

                // We want ROW 3 to be EXTRA TALL

                var headers = [$scope.translation.SRNO, $scope.translation.ApplicationNo, $scope.translation.MemberName, $scope.translation.MembershipNo,
                    $scope.translation.PLDB, $scope.translation.Branch, $scope.translation.Type, $scope.translation.Sector, $scope.translation.Scheme,
                    $scope.translation.PurposeOfLoan, $scope.translation.LoanType, $scope.translation.EstimatedTotalAmount, $scope.translation.LoanAmountApplied, $scope.translation.ApprovedAmount,
                    $scope.translation.DisbursedAmount, $scope.translation.SubmissionDate, $scope.translation.Status
                ];
                            

                var curr=1

                var formatHColumn = excel.addStyle({ 															// Format for headers
                    //border: "thin,thin,thin,thin #333333", 													// 		Border for header
                    font: "Calibri 15 #324148"
                });


                excel.set(0, 0, curr, 'PLDB LOAN APPLICATION', formatHColumn);
               

                if ($scope.AppLoanReport.FinancialYear != '' || $scope.AppLoanReport.FinancialYear == undefined) {
                    ++curr;
                    excel.set(0, 0, curr, 'Year', formatHColumn);
                    excel.set(0, 1, curr, $scope.AppLoanReport.FinancialYear, formatHColumn);
                }

                if ($scope.AppLoanReport.Month != '' || $scope.AppLoanReport.Month == undefined) {
                    ++curr;
                    excel.set(0, 0, curr, 'Month', formatHColumn);
                    excel.set(0, 1, curr, $scope.AppLoanReport.Month, formatHColumn);
                }
                if ($scope.MembershipReport.DistrictId != '' || $scope.MembershipReport.DistrictId == undefined) {
                    ++curr;
                    excel.set(0, 0, curr, 'District', formatHColumn);
                    excel.set(0, 1, curr, $scope.DistrictList.filter(function (x) { return x.DISTRICT_ID == $scope.MembershipReport.DistrictId })[0].DISTRICT_ENG, formatHColumn);
                }

                if ($scope.AppLoanReport.PLDB_ID != '' || $scope.AppLoanReport.PLDB_ID == undefined) {
                    ++curr;
                    excel.set(0, 0, curr, 'P.L.D.B.', formatHColumn);
                    excel.set(0, 1, curr, $scope.PLDB.filter(function (x) { return x.PldbId == $scope.AppLoanReport.PLDB_ID })[0].PLDBName, formatHColumn);
                }
                if ($scope.AppLoanReport.Branch_ID != '' || $scope.AppLoanReport.Branch_ID == undefined) {
                    ++curr;
                    excel.set(0, 0, curr, 'Branch', formatHColumn);
                    excel.set(0, 1, curr, $scope.PLDBBranches.filter(function (x) { return x.BLOCK_ID == $scope.AppLoanReport.Branch_ID })[0].BLOCK_ENG, formatHColumn);
                }
                if ($scope.MembershipReport.AgeId != '' || $scope.MembershipReport.AgeId == undefined) {
                    ++curr;
                    excel.set(0, 0, curr, 'Age', formatHColumn);
                    excel.set(0, 1, curr, $scope.MembershipReport.AgeId, formatHColumn);
                }
                if ($scope.MembershipReport.CategoryId != '' || $scope.MembershipReport.CategoryId == undefined) {
                    ++curr;
                    excel.set(0, 0, curr, 'Category', formatHColumn);
                    excel.set(0, 1, curr, $scope.CategoryList.filter(function (x) { return x.CODEID == $scope.MembershipReport.CategoryId })[0].CODENAME, formatHColumn);
                }

                // This array holds the HEADERS text
                var formatHeader = excel.addStyle({ 															// Format for headers
                    border: "thin,thin,thin,thin #333333", 													// 		Border for header
                    font: "Calibri 12 #fff B",
                    background: "#64c0c5",
                }); 														// 		Font for headers
                curr = curr + 1;
                for (var i = 0; i < headers.length; i++) {
                    //++curr;// Loop all the haders
                    excel.set(0, i, curr+1, headers[i], formatHeader);													// Set CELL with header text, using header format
                    excel.set(0, i, undefined, "auto");															// Set COLUMN width to auto (according to the standard this is only valid for numeric columns)
                }
                // 		in color green

                var formatColumn = excel.addStyle({ 															// Format for headers
                    border: "thin,thin,thin,thin #333333", 													// 		Border for header
                    font: "Calibri 10 #324148"
                });

                curr = curr + 2;
                JSON.parse(data.DataList).forEach(function (item, index) {
                    excel.set(0, 0, index + curr, (index + 1), formatColumn);
                    excel.set(0, 1, index + curr, item.application_no, formatColumn);
                    excel.set(0, 2, index + curr, item.MemberName, formatColumn);
                    excel.set(0, 3, index + curr, item.MembershipNo, formatColumn);
                    excel.set(0, 4, index + curr, item.PLDBName, formatColumn);
                    excel.set(0, 5, index + curr, ($scope._language == 'en-us' ? item.BLOCK_ENG : item.BLOCK_MANGAL), formatColumn);
                    excel.set(0, 6, index + curr, ($scope._language == 'en-us' ? item.Farming_NonFarming : item.Farming_NonFarming), formatColumn);
                    excel.set(0, 7, index + curr, ($scope._language == 'en-us' ? item.sector_name_en : item.sector_name_hi), formatColumn);
                    excel.set(0, 8, index + curr, ($scope._language == 'en-us' ? item.scheme_name_en : item.scheme_name_hi), formatColumn);
                    excel.set(0, 9, index + curr, ($scope._language == 'en-us' ? item.purpose_name_en : item.purpose_name_hi), formatColumn);
                    excel.set(0, 10, index + curr, ($scope._language == 'en-us' ? item.loan_type_en : item.loan_type_hi), formatColumn); 
                    excel.set(0, 11, index + curr, item.estimated_total_amount, formatColumn);
                    excel.set(0, 12, index + curr, item.loan_amount_applied, formatColumn);
                    excel.set(0, 13, index + curr, item.loan_amount_applied , formatColumn);
                    excel.set(0, 14, index + curr, item.amount_disbursed, formatColumn);
                    excel.set(0, 15, index + curr, item.FormatedDate, formatColumn);
                    excel.set(0, 16, index + curr, ($scope._language == 'en-us' ? item.ProceesStatusTextEn : item.ProceesStatusTextHi), formatColumn);
                });
                excel.generate("PLDBLoanApplication.xlsx");
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });
    }

    $scope.ApplicationEdit = function (th) {
        //window.location.href = th.AppLoanFormUrl + "?pid=" + th.PurposeId + "&id=" + th.loanIdINc;
        window.location.href = "../pldb/LoanApplicationFormReport?pid=" + th.PurposeId + "&id=" + th.loanIdINc;
    }


    $scope.base64ToArrayBuffer = function (base64) {
        var binaryString = window.atob(base64);
        var binaryLen = binaryString.length;
        var bytes = new Uint8Array(binaryLen);
        for (var i = 0; i < binaryLen; i++) {
            var ascii = binaryString.charCodeAt(i);
            bytes[i] = ascii;
        }
        return bytes;
    }



    $scope.MonthList = [];
    $scope.GetAuditMonthList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MONTH&MasterCode=-1").
            success(function (data, status) {
                $scope.MonthList = data;
            }).
            error(function (data, status) {
                $scope.MonthList = [];
            });
    };


    $scope.PLDBBranches = [];
    $scope.PLDBBranchList = function () {
        if ($scope.AppLoanReport.PLDB_ID != undefined || $scope.AppLoanReport.PLDB_ID != '') {
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.AppLoanReport.PLDB_ID).
            success(function (data, status) {
                $scope.PLDBBranches = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
            });
        }
    };

    $scope.PLDB = [];
    $scope.PLDBAll = [];
    $scope.PLDBList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=-1").
            success(function (data, status) {
                $scope.PLDB = data;
                $scope.PLDBAll = data;
                $scope.AppLoanReport.Branch_ID = '';
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDB = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.CategoryList = [];
    $scope.GetCategoryList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function (data, status) {
                $scope.CategoryList = data;
            }).
            error(function (data, status) {
                $scope.CategoryList = [];
            });
    }
    $scope.DistrictList = [];
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };

    $scope.Blocks = [];
    $scope.BlockList = function () {
        $scope.Blocks = [];
        $scope.PLDB = $scope.PLDBAll.filter(function (x) { return x.DistrictId == $scope.MembershipReport.DistrictId });
        //if ($scope.MembershipReport.DistrictId != '0') {
        //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.MembershipReport.DistrictId).
        //        success(function (data, status) {
        //            $scope.Blocks = data;

        //        }).
        //        error(function (data, status) {
        //            $scope.Blocks = [];
        //        });
        //}
    };

    $scope.getTranslation();
    $scope.SearchData();
    $scope.GetAuditMonthList();
    $scope.GetDistrictList();
    $scope.GetCategoryList();
    //$scope.BlockList();
    //$scope.PLDBBranchList();
    $scope.PLDBList();



});