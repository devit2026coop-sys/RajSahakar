
var MasterMod = angular.module('niactPRMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("niactPRCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.search = {};
    $scope.DistrictList = [];
    $scope.PLDBList = [];
    $scope.PLDBBranches = [];
    $scope.search.LoanDistrictId = '0';
    $scope.NIActSummary = [];
    $scope.colspan = 7;
    $scope.NIACTReport = 'NI-ACT-Report';
    const urlParams = new URLSearchParams(window.location.search);
    $scope.otsIdEnc = urlParams.get('otsEncId');
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
        $scope.PLDBListData();
        $scope.GetPldbNIActDetails();
        $scope.GetNiActApplicationDetails();
    };
    $scope.GetDistrictList = function () {
        $scope.DistrictList = [];
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
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
                $scope.GetPldbNIActDetails();

            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetPldbNIActDetails = function () {
        $scope.actionType = "GetNIActDetailSummary";

        if ($("#hdnDistrictId").val() != '0') { $scope.search.DISTRICT_ID = $("#hdnDistrictId").val(); }
        if ($("#hdnPldbId").val() != '0') { $scope.search.PLDB_ID = $("#hdnPldbId").val(); }
        if ($("#hdnPldbBranchId").val() != '0') { $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val(); }
        if ($scope.search.PLDB_ID == 0 && $scope.search.PLDB_Branch_ID == 0) { $scope.actionType = "GetNIActDetailSummary" }

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/MIS/GetOtsNiActDetails/?actionType=" + $scope.actionType + "&districtId=" + ($scope.search.DISTRICT_ID ?? 0) + "&pldbId=" + ($scope.search.PLDB_ID ?? 0) + "&pldbBranchId=" + ($scope.search.PLDB_Branch_ID ?? 0)).
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NiActDetails != null) {
                    $scope.NIActSummary = JSON.parse(data.Result.Data.NiActDetails);
                    $scope.colsort = "";
                    $scope.reverseSort = false;
                }
                $(".se-pre-con").fadeOut("slow");
                console.log($scope.NIActSummary);
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.GetNiActApplicationDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/PLDB/GetNiActApplicantDetails?otsIdEnc=" + $scope.otsIdEnc + "&summaryId=0").
            success(function (data, status) {
                console.log(data);
                $(".se-pre-con").fadeOut("slow");
                if (data != null) {
                    $scope.ApplicantDetails = JSON.parse(data.Result.Data.ApplicantDetail)[0];
                }
                console.log('ApplicantDetails', $scope.ApplicantDetails)
            }).
            error(function () {
                alert("Error while fetching OTS application data.");
                $(".se-pre-con").fadeOut("slow");
            });
    }

    //Download Excel
    $scope.ReportDownload = function () {
        let csv = '';
        const rows = document.querySelectorAll("#" + $scope.NIACTReport + " tr");
        let maxCols = 0;

        rows.forEach(row => {
            const style = window.getComputedStyle(row);
            if (!row.classList.contains('ng-hide') && style.display !== 'none') {
                const visibleCols = Array.from(row.querySelectorAll("th, td")).filter(col => {
                    const cellStyle = window.getComputedStyle(col); return !col.classList.contains('ng-hide') && cellStyle.display !== 'none';
                });
                if (visibleCols.length > maxCols) maxCols = visibleCols.length;
            }
        });

        rows.forEach(row => {
            const style = window.getComputedStyle(row);
            const allCols = row.querySelectorAll("th, td");
            const rowData = [];
            let visibleCellCount = 0;

            for (let i = 0; i < allCols.length; i++) {
                const col = allCols[i];
                const cellStyle = window.getComputedStyle(col);

                if (col.classList.contains('ng-hide') || cellStyle.display === 'none') { continue; }
                let cellText = col.innerText != null ? col.innerText.trim().replace(/[^\dA-Za-z\s.,]/g, '') : '';
                cellText = cellText.replace(/"/g, '""');
                rowData.push('"' + cellText + '"');
                visibleCellCount++;
            }
            while (visibleCellCount < maxCols) { rowData.push('""'); visibleCellCount++; }
            csv += rowData.join(",") + '\n';
        });

        const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = $scope.NIACTReport + '.csv';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    };
    $scope.SaveNIDetail = function (OTS) {
        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();
        formData.append("id", 0);
        formData.append("OTSId", OTS.PLDB_OTS_SchemeID);
        formData.append("DocSelfie", ($scope.DocSelfie ?? null));
        formData.append("DocTaamil", ($scope.DocTaamil ?? null));
        formData.append("Remark", ($scope.Remark ?? ''));

        $http.post(SiteUrl + "/PLDB/SaveOTSVerifyReportFile", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data.docId > 0) {
                    $('#Taamilmodal').modal('hide');
                    alert("File upload successfully");
                    $scope.GetNiActApplicationDetails();
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.BackToPage = function () {

    }
    $scope.OnPageLoad();

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






