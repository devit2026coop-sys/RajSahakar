var MasterMod = angular.module('mwotsMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("mwotsCtrl", function ($rootScope,$scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.search = {};
    $scope.DistrictList = [];
    $scope.PLDBList = [];
    $scope.PLDBBranches = [];
    $scope.search.LoanDistrictId = '0';
    $scope.MonthWiseOtsData = [];
    $scope.colspan = 15;
/*    $scope.search.ConstituencyId = 0;*/
    $scope.MonthWiseOTSRecovery = 'MonthWiseOTSRecoveryReport';

    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 50,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: [],
        TotalRecords: 0,
    }
    $rootScope.DataPagingDetails = {
        PageNumber: 1,
        PageSize: 50,
        Totalrecords: 0

    }

    $scope.pagechange = function () {
        $scope.DataPaging = {
            searchBoxDivId: 'search-filter',
            currentPageID: 1,
            pageSize: $scope.search.pageSize ?? 50,
            sortExpression: '',
            sortDirection: '1',
            searchParameters: [],
            TotalRecords: 0,
        }
        $scope.GetMonthWiseOtsRecoveryDetails();
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
                $scope.GetMonthWiseOtsRecoveryDetails();
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

    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }

        $scope.GetDistrictList();
/*        $scope.GetConstituencyList();*/

        $scope.PLDBListData();
        $scope.GetMonthWiseOtsRecoveryDetails();
    };
    $scope.GetDistrictList = function () {
        debugger
        $scope.DistrictList = [];
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";
  
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                debugger
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $scope.search.DISTRICT_ID = $("#hdnDistrictId").val(); $scope.search.PLDB_ID = "0"; $scope.search.PLDB_Branch_ID = "0"; $("#DISTRICT_ID").attr("disabled", "disabled");
                    $scope.PLDBListData();
                }
            }).error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });

    };

    $scope.PLDBListData = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=" + ($scope.search.DISTRICT_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBList = data;
                $(".se-pre-con").fadeOut("slow");

                if ($("#hdnPldbId").val() != "0") {
                    $scope.search.PLDB_ID = $("#hdnPldbId").val();
                    $scope.PLDBBranchList();
                    $("#PLDB_ID").attr("disabled", "disabled");
                }
            }).
            error(function (data, status) {
                $scope.PLDBList = [];
            });
    };
    $scope.PLDBBranches = [];
    $scope.PLDBBranchList = function (branch_ID) {
        $scope.search.PLDB_Branch_ID = "0";
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + ($scope.search.PLDB_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBBranches = data;

                if ($("#hdnPldbBranchId").val() != "0") {
                    $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val();
                    $("#PLDB_Branch_ID").attr("disabled", "disabled");
                }

                $(".se-pre-con").fadeOut("slow");
                $scope.GetMonthWiseOtsRecoveryDetails();

            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.ConstituencyList = [];
    $scope.GetConstituencyList = function () {
        // Reset selected value
        $scope.search.ConstituencyId = '0';
        $(".se-pre-con").fadeIn("slow");
        if ($scope.search.DISTRICT_ID && $scope.search.DISTRICT_ID != '0') {
            $http.post(SiteUrl + "/FertilizerSupply/GetConstituencyListByDistrictId",
                { DistrictId: $scope.search.DISTRICT_ID })
                .then(function (response) {
                    console.log(response.data);
                    $scope.ConstituencyList = response.data;
                    $(".se-pre-con").fadeOut("slow");
                }, function () {
                    $scope.ConstituencyList = [];
                    $(".se-pre-con").fadeOut("slow");
                    alert('Error while fetching Constituency List data.');
                });

        } else {
            // if no district selected
            $scope.ConstituencyList = [];
            $(".se-pre-con").fadeOut("slow");
        }
    };

    //$scope.ConstituencyList = [];
    //$scope.GetConstituencyList = function () {
    //    $(".se-pre-con").fadeIn("slow");
    //    if ($scope.search.DISTRICT_ID && $scope.search.DISTRICT_ID != '0') {
           
    //        $http.post(SiteUrl + "/FertilizerSupply/GetConstituencyListByDistrictId",
    //            { DistrictId: $scope.search.DISTRICT_ID })
    //            .then(function (response) {

    //                console.log(response.data); // debug
    //                $scope.ConstituencyList = response.data;
    //                $(".se-pre-con").fadeOut("slow");

    //            }, function (error) {
    //                $scope.ConstituencyList = [];
    //                $(".se-pre-con").fadeOut("slow");
    //                alert('Error while fetching Constituency List data.');
    //            });

    //    } else {
    //        $scope.ConstituencyList = [];
    //    }
    //};

    $scope.GetMonthWiseOtsRecoveryDetails = function () {
        debugger
        $scope.MonthWiseOtsData = [];
        $scope.actionType = "GetNIActDetailSummary";

        if ($("#hdnDistrictId").val() != '0') { $scope.search.DISTRICT_ID = $("#hdnDistrictId").val(); }
        if ($("#hdnPldbId").val() != '0') { $scope.search.PLDB_ID = $("#hdnPldbId").val(); }
        if ($("#hdnPldbBranchId").val() != '0') { $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val(); }
        if ($scope.search.PLDB_ID == 0 && $scope.search.PLDB_Branch_ID == 0) { $scope.actionType = "GetNIActDetailSummary" }
        // Added by bhavesh
        if ($scope.search.ConstituencyId == 0 && $scope.search.ConstituencyId == 0) { $scope.actionType = "GetNIActDetailSummary" }
        var jsonParameters = { "TotalRecords": ($scope.DataPaging.TotalRecords ?? 0), "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SearchParameter": $scope.DataPaging.searchParameters };

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/MIS/GetMonthWiseOTSRecovery", JSON.stringify({ Details: jsonParameters, actionType: $scope.actionType, districtId: ($scope.search.DISTRICT_ID ?? 0), pldbId: ($scope.search.PLDB_ID ?? 0), pldbBranchId: ($scope.search.PLDB_Branch_ID ?? 0), ConstituencyId: ($scope.search.ConstituencyId ?? 0) })).
                success(function (data, status) {
                if (data.Data != null && data.Data.monthWiseOtsData != null) {
                    $scope.MonthWiseOtsData = JSON.parse(data.Data.monthWiseOtsData);
                    if ($scope.DataPaging.currentPageID == 1) {
                        $scope.TotalRecords = $scope.MonthWiseOtsData[0].TotalRecords;
                    }
                    $rootScope.DataPagingDetails.TotalRecords = $scope.TotalRecords;
                    $rootScope.DataPagingDetails.PageNumber = $scope.DataPaging.currentPageID;
                    $rootScope.DataPagingDetails.PageSize = $scope.DataPaging.pageSize;
                    $rootScope.SetDataPaging();
                    $scope.colsort = "";
                    $scope.reverseSort = false;
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.ReportDownloadTesting = function (filename) {
        $scope.MonthWiseOtsData = $scope.MonthWiseOtsData.map(item => {
            const newItem = { ...item };
            delete newItem.$$hashKey;
            delete newItem.PLDB_OTS_SchemeID;
            return newItem;
        });
        const workbook = XLSX.utils.book_new();

        const worksheet = XLSX.utils.json_to_sheet($scope.MonthWiseOtsData);

        XLSX.utils.book_append_sheet(workbook, worksheet, "Sheet1");

        const workbookBinary = XLSX.write(workbook, { bookType: "xlsx", type: "array" });

        const blob = new Blob([workbookBinary], { type: "application/octet-stream" });

        const url = URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = filename + '.xlsx';
        document.body.appendChild(a);
        a.click();

        setTimeout(() => {
            document.body.removeChild(a); OnPageLoad
            window.URL.revokeObjectURL(url);
        }, 0);
    };

    $scope.OnPageLoad();

    $scope.ReportDownload = function () {
        const data = $scope.MonthWiseOtsData.map(({ $$hashKey, ...r }) => r);
        if (data.length === 0) return;

        const HeaderRowFirst = [
            "प्राथमिक बैंक",
            "शाखा",
            "ऋणी सदस्य का नाम",
            "पिता/पति का नाम",
            "जाति",
            "निवासी / गांव",
            "ऋण सेक्टर",
            "ऋण का उद्देश्य",
            "ऋण खाता संख्या",
            "ब्याज दर",
            "ऋण वितरण विवरण", "",
            "अवधिपार मांग 31.03.2025", "", "", "", "",
            "चालू मांग 31.03.2025", "", "", "",
            "खाता बंद होने पर दिनांक 01.04.2025 से ", "", "", "",
            "कुल वसूली योग्य राशि माफी सहित", "", "", "",
            "योजनान्तर्गत दी गई राहत", "", "",
            "ऋणी द्वारा वसूली की गई कुल राशि", "", "",
            "अन्तर"
        ];

        const HeaderRowSecond = [
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "दिनांक", "राशि",
            "असल", "बीमा प्रिमियम", "ब्याज", "दं.ब्याज / अन्य", "कुल",
            "असल", "ब्याज", "दं.ब्याज / अन्य", "कुल",
            "असल", "ब्याज", "दं.ब्याज / अन्य खर्च", "कुल",
            "असल + बीमा प्रिमियम", "ब्याज", "दं.ब्याज / अन्य खर्च", "कुल",
            "ब्याज", "दं.ब्याज / अन्य खर्च", "कुल",
            "दिनांक", "रसीद संख्या", " कुल प्राप्त राशि",
            ""
        ];

        const Columns = [
            "PLDBName",
            "PLDBBranch",
            "ApplicantName",
            "FatherHusbandName",
            "CategoryName",
            "Village",
            "Sector",
            "Purpose",
            "LoanAccNo",
            "RateOfInterest",
            "DateOfFirstDisbursement",
            "TotalDisbursedAmt",
            "Principal",
            "InsurancePremium",
            "Interest",
            "PenalInterest",
            "ODTotal",
            "CDPrincipal",
            "CDInterest",
            "CDPenalInterest",
            "CDTotal",
            "CalculatedActual",
            "CalculatedInterest",
            "CalculatedPenalInterestWithOther",
            "CalculatedTotal",
            "ActualTotal",
            "InterestTotal",
            "PenalInterestOtherTotal",
            "RecoverableTotalWithRelief",
            "ReliefInterest",
            "ReliefPenalInterest",
            "ReliefTotal",
            "FRDate",
            "FRReceiptNo",
            "ReceivedAmount",
            "Difference"
        ];

        //const dataRows = data.map(item => Columns.map(key => item[key] || ""));
        const numericColumns = [
            "RateOfInterest",
            "TotalDisbursedAmt",
            "Principal",
            "InsurancePremium",
            "Interest",
            "PenalInterest",
            "ODTotal",
            "CDPrincipal",
            "CDInterest",
            "CDPenalInterest",
            "CDTotal",
            "CalculatedActual",
            "CalculatedInterest",
            "CalculatedPenalInterestWithOther",
            "CalculatedTotal",
            "ActualTotal",
            "InterestTotal",
            "PenalInterestOtherTotal",
            "RecoverableTotalWithRelief",
            "ReliefInterest",
            "ReliefPenalInterest",
            "ReliefTotal",
            "ReceivedAmount",
            "Difference"
        ];
        const dataRows = data.map(item =>
            Columns.map(key => {
                const value = item[key];
                if (numericColumns.includes(key)) {
                    return value !== null && value !== "" ? Number(value) : null;
                }
                return value || "";
            })
        );
        const aoa = [HeaderRowFirst, HeaderRowSecond, ...dataRows];
        const ws = XLSX.utils.aoa_to_sheet(aoa);

        ws["!merges"] = [
            { s: { r: 0, c: 0 }, e: { r: 1, c: 0 } },  // PLDB_OTS_SchemeID
            { s: { r: 0, c: 1 }, e: { r: 1, c: 1 } },  // PLDB Name
            { s: { r: 0, c: 2 }, e: { r: 1, c: 2 } },  // PLDB Branch
            { s: { r: 0, c: 3 }, e: { r: 1, c: 3 } },  // Applicant Name
            { s: { r: 0, c: 4 }, e: { r: 1, c: 4 } },  // Applicant Name
            { s: { r: 0, c: 5 }, e: { r: 1, c: 5 } },  // Category
            { s: { r: 0, c: 6 }, e: { r: 1, c: 6 } },  // Village
            { s: { r: 0, c: 7 }, e: { r: 1, c: 7 } },  // Sector
            { s: { r: 0, c: 8 }, e: { r: 1, c: 8 } },  // Purpose
            { s: { r: 0, c: 9 }, e: { r: 1, c: 9 } },  // Loan App No
            //{ s: { r: 0, c: 10 }, e: { r: 1, c: 10 } },  // Interest Rate

            { s: { r: 0, c: 10 }, e: { r: 0, c: 11 } },  // Loan Disbursement Details
            { s: { r: 0, c: 12 }, e: { r: 0, c: 16 } },  // Overdue Demand
            { s: { r: 0, c: 17 }, e: { r: 0, c: 20 } },  // Current Demand
            { s: { r: 0, c: 21 }, e: { r: 0, c: 24 } },  // Date on which acc closed
            { s: { r: 0, c: 25 }, e: { r: 0, c: 28 } },  // Total Recovery Amount
            { s: { r: 0, c: 29 }, e: { r: 0, c: 31 } },  // Relief under scheme
            { s: { r: 0, c: 32 }, e: { r: 0, c: 34 } },  // Total amount recovered
            { s: { r: 0, c: 35 }, e: { r: 1, c: 35 } }   // Difference (not merged)
        ];

        const borderStyle = {
            top: { style: "thin", color: { auto: 1 } },
            bottom: { style: "thin", color: { auto: 1 } },
            left: { style: "thin", color: { auto: 1 } },
            right: { style: "thin", color: { auto: 1 } }
        };

        for (let r = 0; r < aoa.length; r++) {
            for (let c = 0; c < HeaderRowFirst.length; c++) {
                const cell = XLSX.utils.encode_cell({ r, c });
                if (ws[cell]) {
                    ws[cell].s = {
                        border: borderStyle,
                        alignment: { horizontal: "center", vertical: "center", wrapText: true },
                        font: r < 2 ? { bold: true } : {}
                    };
                }
            }
        }

        const wb = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, "Month Wise Recovery");

        const wbout = XLSX.write(wb, { bookType: "xlsx", type: "array", cellStyles: true });
        const blob = new Blob([wbout], { type: "application/octet-stream" });

        const url = URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = "Month-Wise-OTS-Recovery.xlsx";
        document.body.appendChild(a);
        a.click();
        setTimeout(() => {
            document.body.removeChild(a);
            URL.revokeObjectURL(url);
        }, 0);
    };

});
MasterMod.filter('inrCurrency', function () {
    return function (input) {
        if (!input)
            return input;
        var num = parseFloat(input);
        if (isNaN(num)) return input;
        return num.toLocaleString('en-IN', { style: 'currency', currency: 'INR' });
    };
});