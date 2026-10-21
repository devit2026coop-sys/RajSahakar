
var MasterMod = angular.module('BeneficiaryListMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("BeneficiaryListCnt", function ($scope, $http, $rootScope) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.search = {};
    $scope.divClass = false;
    $scope.DistrictList = [];
    $scope.GetPldbSummary = [];
    $scope.TotalGetPldbSummary = []; $scope.BankMasterList = []; $scope.BankBranches = [];
    $scope.search.LoanDistrictId = '0';
    $scope.isLBDisabled = false;

    $scope.ReportType = 1;
    $scope.Islinkcalled = false;
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
            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }
        //$scope.GetDistrictWithoutDivisionList();
        $scope.PLDBListData();


        if (!$("#hdnPldbId").val() || Number($("#hdnPldbId").val()) <= 0) {$scope.GetPldbSummary(false); }
        
        //$scope.LoanBankList();
        //var hdnDistId = $('#hdnDistrictId').val();
        //if (hdnDistId > 0) {
        //    $scope.isLBDisabled = true;
        //    $scope.search.LoanDistrictId = hdnDistId
        //} else
        //    $scope.isLBDisabled = false;
    };

    function getCurrentFinancialYear() {
        var today = new Date();
        var year = today.getFullYear();
        var month = today.getMonth() + 1; // Months are zero-based
        var startYear, endYear;
        if (month >= 4) { // April (4th month) starts the new financial year
            startYear = year;
            endYear = year + 1;
        } else {
            startYear = year - 1;
            endYear = year;
        }
        return startYear + '-' + (endYear.toString().slice(-2));
    }

    $scope.GetDistrictWithoutDivisionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;

                if ($("#hdnDistrictId").val() != "0") {
                    $scope.search.DISTRICT_ID = $("#hdnDistrictId").val();
                    $scope.search.PLDB_ID = "0";
                    $scope.search.PLDB_Branch_ID = "0";                    
                    $("#DISTRICT_ID").attr("disabled", "disabled");
                    $scope.PLDBListData();                            
                }
                
                if ( $scope.Islinkcalled == false ) { $scope.GetPldbSummary(false); }           
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };

    //$scope.BlockListByOffAdd = function (id) {
    //    $(".se-pre-con").fadeIn("slow");
    //    $scope.Blocksdata = [];
    //    $scope.GramPanchayats = [];
    //    $scope.Villages = [];
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.search.DISTRICT_ID).
    //        success(function (data, status) {
    //            $(".se-pre-con").fadeOut("slow");
    //            $scope.Blocksdata = data;
    //            $scope.GetPldbSummary();
    //        }).
    //        error(function (data, status) {
    //            $scope.Blocksdata = [];
    //            $(".se-pre-con").fadeOut("slow");
    //        });
    //};
   


    $scope.PLDBList = [];
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
                $scope.GetPldbSummary(false);
                
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        $scope.GetPldbSummary(false);
    }
    $scope.exportExcel = function (branch_ID) {
        $scope.GetPldbSummary(true);
    }

    $scope.GetPldbSummary = function (isExport) {
        $(".se-pre-con").fadeIn("slow");
       
        var Action = "";


        if ($("#hdnDistrictId").val() != '0') { $scope.search.DISTRICT_ID = $("#hdnDistrictId").val(); }
        if ($("#hdnPldbId").val() != '0') { $scope.search.PLDB_ID = $("#hdnPldbId").val(); }
        if ($("#hdnPldbBranchId").val() != '0') { $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val(); }


        if ($scope.ReportType == '1') { Action = 'PldbSummaryRpt'; }
        else if ($scope.ReportType == '2') { Action = 'Pldb103SummaryRpt'; }
        else if ($scope.ReportType == '3') { Action = 'PldbBothSummaryRpt'; }
        //var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        
        $http.get(SiteUrl + "/MIS/GetPldbDistrictWiseSummary/?financialYearId=" + ($scope.FinancialYearId ?? 0) + "&districtId=" + ($scope.search.DISTRICT_ID ?? 0) + "&PLDB_ID=" + ($scope.search.PLDB_ID ?? 0) + "&PLDB_Branch_ID=" + ($scope.search.PLDB_Branch_ID ?? 0) + "&Type=" + Action + "&isExport=" +isExport ).
            success(function (data, status) {
               
                if (isExport) {
                    $scope.downloadFile(data.filename);
                    return;
                }
                else
                {
                    $scope.GetPldbSummaryList = [];
                    $scope.TotalGetPldbSummary = [];                    
                }
                if (data != null && data.Result != null) {

                    $scope.GetPldbSummaryList = JSON.parse(data.Result.Data.GetPldbSummary);
                    $scope.TotalGetPldbSummary = JSON.parse(data.Result.Data.TotalGetPldbSummary);                  
                  
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.GetPldbSummary = [];
                $scope.TotalGetPldbSummary = [];
                $rootScope.DataPagingDetails.TotalRecords = 0;
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.IsOverall = false;
    $scope.GopalCreditCardModel = {};
    $scope.GopalCreditCardModel.IsPendancy = 0;
    $scope.GridShow = function (val) {
        debugger;
        if (val == 0) {
            $scope.IsOverall = false;
        }
        else {
            $scope.IsOverall = true;

        }
        $scope.GetPldbSummary(false);
    }

    $scope.pagechange = function () {
        $scope.DataPaging = {
            searchBoxDivId: 'search-filter',
            currentPageID: 1,
            pageSize: $scope.pageSize ?? 50,
            sortExpression: '',
            sortDirection: '1',
            searchParameters: [],
            TotalRecords: 0,
        }
        $scope.GetPldbSummary(false);
    }
    $scope.PldbNameChange = function (item)
    {
        $scope.Islinkcalled = true;
        $scope.search.DISTRICT_ID = item.DISTRICT_ID;
        $scope.PLDBListData();
        $scope.search.PLDB_ID = item.PldbId;  
        $scope.GetPldbSummary(false);
    }
    $scope.ReportTypeChange = function () {

        //if ($("#hdnDistrictId").val() != '0') { $scope.search.DISTRICT_ID = "0"; }
        //if ($("#hdnPldbId").val() != '0') { $scope.search.PLDB_ID = "0"; }
        //if ($("#hdnPldbBranchId").val() != '0') { $scope.search.PLDB_Branch_ID = "0"; }

        //$scope.search.PLDB_ID = "0";
        //$scope.search.PLDB_Branch_ID = "0"; 
        $scope.GetPldbSummary(false);
    }
    $scope.PldbBranchNameChange = function (item) {
        $scope.Islinkcalled = true;
        $scope.search.DISTRICT_ID = item.DISTRICT_ID;
        $scope.PLDBListData();
        $scope.search.PLDB_ID = item.PldbId;
        $scope.PLDBBranchList();
        $scope.search.PLDB_Branch_ID = item.BranchId;
        
        $scope.GetPldbSummary(false);

        
    }
    
    $scope.BackToPage = function () {
        window.location.reload();

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


    $scope.exportDivToExcel1= function (divId, filename) {
        var dataType = 'application/vnd.ms-excel';
        var divElement = document.getElementById(divId);

        var html = `
      <html xmlns:o="urn:schemas-microsoft-com:office:office" 
            xmlns:x="urn:schemas-microsoft-com:office:excel" 
            xmlns="http://www.w3.org/TR/REC-html40">
        <head>
          <meta charset="UTF-8">
          <style>
            table, th, td {
              border: 1px solid black;
              border-collapse: collapse;
              padding: 5px;
              text-align: center;
            }
          </style>
        </head>
        <body>${divElement.innerHTML}</body>
      </html>`;



        var blob = new Blob([html], { type: dataType });
        var url = URL.createObjectURL(blob);

        var a = document.createElement("a");
        a.href = url;
        a.download = `${filename || 'export'}.xls`;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    }
    $scope.exportDivToExcel = function (divId, filename) {
        var dataType = 'application/vnd.ms-excel';
        var divElement = document.getElementById(divId);

        // Clone the div element to avoid modifying the original DOM
        var clonedDiv = divElement.cloneNode(true);

        // Find all rows in the cloned div
        var rows = clonedDiv.getElementsByTagName("tr");

        // Loop through each row
        for (var i = 0; i < rows.length; i++) {
            var cells = rows[i].getElementsByTagName(i <2  ? "th" : "td");

            // Loop through each cell (td or th) in the row
            for (var j = 0; j < cells.length; j++) {
                var cell = cells[j];

                // Check if the cell has the class 'ng-hide' which means it's hidden
                if (cell.classList.contains('ng-hide')) {
                    // If the column is hidden, set the display style to 'none' to exclude it
                    cell.style.display = 'none';
                    cell.parentNode.removeChild(cell);
                    j--;
                }
            }
        }

        var html = `
        <html xmlns:o="urn:schemas-microsoft-com:office:office" 
              xmlns:x="urn:schemas-microsoft-com:office:excel" 
              xmlns="http://www.w3.org/TR/REC-html40">
        <head>
          <meta charset="UTF-8">
          <style>
            table, th, td {
              border: 1px solid black;
              border-collapse: collapse;
              padding: 5px;
              text-align: center;
            }
          </style>
        </head>
        <body>${clonedDiv.innerHTML}</body>
      </html>`;

        var blob = new Blob([html], { type: dataType });
        var url = URL.createObjectURL(blob);

        var a = document.createElement("a");
        a.href = url;
        a.download = `${filename || 'export'}.xls`;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    };
    $scope.OnPageLoad();
    
});
