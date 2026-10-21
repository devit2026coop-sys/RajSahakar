var CommodityWiseWRDetailModule = angular.module('PLDBMemberShipReportMod', ['angularUtils.directives.dirPagination']);
CommodityWiseWRDetailModule.controller("PLDBMemberShipReportCont", function ($rootScope, $scope, $http) {
    /*------------Paging and Filter-----------------*/
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

    $scope.ReportType = 'M';

    $scope.MembershipReport = [];

    $scope.GetWarehouseFilterList = function (type) {
        $scope.ReportType = type;
        $scope.searchRequest($scope.SearchData);
    }


    $scope.FinancialYearList = [];
    $scope.BindFY = function () {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                //alert(JSON.stringify(data));
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
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

 
    $scope.PLDBBranches = [];
    $scope.PLDBBranchList = function () {
        if ($scope.MembershipReport.PLDB_ID != undefined || $scope.MembershipReport.PLDB_ID != '') {
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.MembershipReport.PLDB_ID).
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
                $scope.MembershipReport.PLDB_ID = '';
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDB = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.SearchData = function () {
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/MIS/GetPLDBMemberShipReport", JSON.stringify({ Details: jsonParameters, ReportType: $scope.ReportType })).
            success(function (data, status) {
                if ($scope.ReportType == "M") {
                    $scope.SepterdList = true;
                    $scope.PLDBMemberReportCountby = JSON.parse(data.DataList);
                } else {
                    $scope.SepterdList = false;
                    $scope.PLDBMemberReportDetails = JSON.parse(data.DataList);
                }
           
                    $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBMemberReportDetails = [];
                $(".blockUI").fadeIn("slow");
                console.log('Error while fetching WR Detail data.');
            });
    }


    $scope.Back = function () {
        $scope.CountList = true;
        $scope.SepterdList = false;
    }

    $scope.CountList = true;
    $scope.SepterdList = false;
    $scope.GetDetials = function (item) {
        $scope.item = item;
        $scope.searchRequest($scope.CountbySearchData);
    }
    $scope.ExportData = function () {

        $scope.ReportExportList = [];
        $(".blockUI").fadeIn("slow");
        var functionname = _language == 'en-us' ? "GetPLDBMembershipReportExportEn" : "GetPLDBMembershipExportHi";
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": 1, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "IsAllRecords": true };
        return $http.post(SiteUrl + "/MIS/" + functionname, JSON.stringify({ Details: jsonParameters, ReportType: $scope.ReportType, Value: $scope.item })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                var excel = $JExcel.new("Calibri light 10 #333333");			// Default font	

                excel.set({ sheet: 0, value: "PLDB Membership Report" });
               
                // We want ROW 3 to be EXTRA TALL

                var headers = [$scope.translation.SRNO, $scope.translation.ApplicantName, $scope.translation.Age, $scope.translation.PLDB, $scope.translation.Branch, $scope.translation.MobileNo, $scope.translation.ApplicationNo, $scope.translation.MembershipNo, $scope.translation.MembershipFee,
                    $scope.translation.Date, $scope.translation.Status
                ];							// This array holds the HEADERS text


                var curr = 1

                var formatHColumn = excel.addStyle({ 															// Format for headers
                    //border: "thin,thin,thin,thin #333333", 													// 		Border for header
                    font: "Calibri 15 #324148"
                });


                excel.set(0, 0, curr, 'PLDB MEMBERSHIP APPLICATION', formatHColumn);


                if ($scope.MembershipReport.FinancialYear != '' || $scope.MembershipReport.FinancialYear == undefined) {
                    ++curr;
                    excel.set(0, 0, curr, 'Year', formatHColumn);
                    excel.set(0, 1, curr, $scope.MembershipReport.FinancialYear, formatHColumn);
                }

                if ($scope.MembershipReport.Month != '' || $scope.MembershipReport.Month == undefined) {
                    ++curr;
                    excel.set(0, 0, curr, 'Month', formatHColumn);
                    excel.set(0, 1, curr, $scope.MembershipReport.Month, formatHColumn);
                }
                if ($scope.MembershipReport.DistrictId != '' || $scope.MembershipReport.DistrictId == undefined) {
                    ++curr;
                    excel.set(0, 0, curr, 'District', formatHColumn);
                    excel.set(0, 1, curr, $scope.DistrictList.filter(function (x) { return x.DISTRICT_ID == $scope.MembershipReport.DistrictId })[0].DISTRICT_ENG, formatHColumn);
                }

                if ($scope.MembershipReport.PLDB_ID != '' || $scope.MembershipReport.PLDB_ID == undefined) {
                    ++curr;
                    excel.set(0, 0, curr, 'P.L.D.B.', formatHColumn);
                    excel.set(0, 1, curr, $scope.PLDB.filter(function (x) { return x.PldbId == $scope.MembershipReport.PLDB_ID })[0].PLDBName, formatHColumn);
                }
                if ($scope.MembershipReport.Branch_ID != '' || $scope.MembershipReport.Branch_ID == undefined) {
                    ++curr;
                    excel.set(0, 0, curr, 'Branch', formatHColumn);
                    excel.set(0, 1, curr, $scope.PLDBBranches.filter(function (x) { return x.BLOCK_ID == $scope.MembershipReport.Branch_ID })[0].BLOCK_ENG, formatHColumn);
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





                var formatHeader = excel.addStyle({ 															// Format for headers
                    border: "thin,thin,thin,thin #333333", 													// 		Border for header
                    font: "Calibri 12 #fff B",
                    background: "#64c0c5",
                }); 														// 		Font for headers

                curr = curr + 1;
                for (var i = 0; i < headers.length; i++) {
                    //++curr;// Loop all the haders
                    excel.set(0, i, curr + 1, headers[i], formatHeader);													// Set CELL with header text, using header format
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
                    excel.set(0, 1, index + curr, item.Name, formatColumn);
                    excel.set(0, 2, index + curr, item.Age, formatColumn);
                    excel.set(0, 3, index + curr, ($scope._language == 'en-us' ? item.PLDBName : item.PLDBNameHi), formatColumn);
                    excel.set(0, 4, index + curr, ($scope._language == 'en-us' ? item.BLOCK_ENG : item.BLOCK_MANGAL), formatColumn);
                    excel.set(0, 5, index + curr, item.MobileNo, formatColumn);
                    excel.set(0, 6, index + curr, item.ApplicationNo, formatColumn);
                    excel.set(0, 7, index + curr, (item.ActionStatus == 1043 ? item.MembershipNo : ''), formatColumn);
                    excel.set(0, 8, index + curr, item.Amount, formatColumn);
                    excel.set(0, 9, index + curr, item.InsertDate, formatColumn);
                    excel.set(0, 10, index + curr, item.ProceesStatusTextHi, formatColumn);
                });

                excel.generate("PLDBMembershipReport.xlsx");
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });

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

    $scope.CountbySearchData = function () {
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/MIS/GetPLDBMemberShipReportbycount", JSON.stringify({ Details: jsonParameters, ReportType: $scope.ReportType, Value: $scope.item  })).
            success(function (data, status) {
                $scope.PLDBMemberReportCountby = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;

                $rootScope.SetDataPaging();
                    $scope.CountList = false;
                    $scope.SepterdList = true;
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBMemberReportCountby = [];
                $(".blockUI").fadeIn("slow");
                console.log('Error while fetching WR Detail data.');
            });
    }

    $scope.TackActionShow = false;
    $scope.ViewMemberApplication = function (ReqIdEnc, status) {
        $(".se-pre-con").fadeIn("slow");
        $scope.TackActionShow = status;

        $http.get(SiteUrl + "/SocietyMemberRegistration/GetPLDBUserRegistrationDetails?ApplicationIdEnc=" + ReqIdEnc).
            success(function (data, status) {
                $scope.divGrid = false;
                $scope.ViewApplication = data[0];
                $scope.MemberApplicationId = ReqIdEnc;
                $scope.divViewApplicationDetailShow = true;

                $("#divViewApplicationDetailShow").modal("show");

                $http.post(SiteUrl + "/SocietyMemberRegistration/GetPLDBMembershipStatus", { StatusId: data[0].ActionStatus }).
                    success(function (data2, status) {
                        $scope.NextActionList = data2;
                    }).
                    error(function (data, status) {
                        $scope.MemberDetailsList = [];
                        $(".se-pre-con").fadeOut("slow");
                    });
                $(".se-pre-con").fadeOut("slow");


                //$scope.MemberDetailsList = data[0];
                //$scope.MemberRegistration = data[0];
                //$scope.divViewShow = true;
                //$scope.divRegistration = false;
                //$scope.divGrid = false;


                if (data[0].ResponseString != null && data[0].ResponseString != "") {
                    var temp = [];
                    temp.push(JSON.parse(data[0].ResponseString));
                    if (temp.length > 0) {
                        if (data[0].KioskID == 'SSO' ? (temp[0].RESPONSEMESSAGE.indexOf('Success') > 0) : (temp[0].MSG.indexOf('Success') > 0)) {
                            $scope.IsPaymentDone = true;

                            $scope.TRANSACTIONID = temp[0].TRANSACTIONID;
                            $scope.ReceiptNo = temp[0].RECEIPTNO;
                            $scope.BANK_NAME = temp[0].BANK_NAME;
                            $scope.BANK_ACC_NO = temp[0].BANK_ACC_NO;
                            if (data[0].KioskID == 'SSO') {
                                $scope.IsCityzen = true;
                                $scope.IsKiosk = false;
                                $scope.Fee = temp[0].PAIDAMOUNT;
                                $scope.RESPONSEMESSAGE = temp[0].RESPONSEMESSAGE;
                            }
                            else {
                                $scope.RESPONSEMESSAGE = temp[0].MSG;
                                $scope.Fee = temp[0].TRANSAMT;
                                $scope.IsCityzen = false;
                                $scope.IsKiosk = true;
                            }

                        }


                        if (data[0].OldMembershipNo != '') {
                            $scope.IsPaymentDone = false;
                        }
                    }
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.MemberDetailsList = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // START : Get Transaction Log
    $scope.ShowTransactionLog = function (RequestId) {
        $(".se-pre-con").fadeIn("slow");
        $scope.ApplicationNo = '';
        $scope.MemberShipNo = '';
        $scope.TransactionMemberName = '';
        $scope.TransactionPLDB = '';
        $scope.TransactionBranch = '';
        $http.post("../SocietyMemberRegistration/GetTransactionLogByRequestId", { "RequestIdEnc": RequestId }).
            success(function (data, status) {
                $scope.MemberApprvalTransactionLog = [];
                if (data.length > 0) {
                    $scope.MemberApprvalTransactionLog = data;
                    $scope.ApplicationNo = data[0].ApplicationNo;
                    $scope.MemberShipNo = data[0].MembershipNo;
                    $scope.TransactionMemberName = data[0].Name;
                    $scope.TransactionPLDB = data[0].PLDBName;
                    $scope.TransactionBranch = data[0].BLOCK_MANGAL;
                    $(".se-pre-con").fadeOut("slow");
                }
            }).
            error(function (data, status) {
                $scope.MemberApprvalTransactionLog = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    }
// END: Get Transaction Log

    $scope.SearchData();
    $scope.getTranslation();
    $scope.BindFY();
    $scope.GetAuditMonthList();
    $scope.GetDistrictList();
    $scope.GetCategoryList();
    $scope.PLDBList();

});