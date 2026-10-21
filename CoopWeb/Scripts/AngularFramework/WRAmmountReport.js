var UserMenu = angular.module("WRAmmountReportMOD", ['angularUtils.directives.dirPagination']);


UserMenu.directive('onlyLettersInput', onlyLettersInput);

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

UserMenu.controller("WRAmmountReportCONT", function ($scope, $http) {


    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 400;


    var dd = new Date().getDate()
    var mm = new Date().getMonth() + 1;
    var yy = new Date().getFullYear();

    var toDaydate = (dd.toString().length == 1 ? ("0" + dd.toString()) : dd.toString()) + "/" + (mm.toString().length == 1 ? ("0" + mm.toString()) : mm.toString()) + "/" + yy.toString();
    $("#txtToDate").val(toDaydate);

    $scope.pageChangeHandler = function (num) {
    };
    $scope.pldbdata = '';
  
    $scope.ShowbranchRport = false;
    $scope.DataSet = [];
    $scope.Filter = {};
   // $scope.BakDataset = [];
    $scope.GetWRReport = function () {  
        $(".se-pre-con").fadeIn("slow");
        $scope.FinancialYears = $('#fyyear').val() == "" ? "": $('#fyyear').val();
        $scope.DistrictLists = $('#ddrDistrictId').val() == null ? '0' : $('#ddrDistrictId').val();
        $scope.WarehouseIds = $('#iddwharehouse').val() == "" ? '0' : $('#iddwharehouse').val();
        $scope.fromDates= $('#txtFromDate').val();
        $scope.Todates = $('#txtToDate').val();      
        //$http.get(SiteUrl + "/MIS/GetWRAmmountReport/?Fy=0&DistId=0&WharehousId=0&fromdatId=" + $("#txtToDate").val() + "&TodatId=0").
        return $http.get(SiteUrl + "/MIS/GetWRAmmountReport/?Fy=" + ($scope.Filter.FinancialYear == undefined ? "" : $scope.Filter.FinancialYear) + "&commodityid=" + ($scope.Filter.commoditytype == undefined ? "" : $scope.Filter.commoditytype) + "&WharehousId=0&fromdatId=" + $("#txtToDate").val() + "&TodatId=0").

            success(function (data, status) {
                $scope.DataSet = JSON.parse(data.DataList);;
                $scope.net_quantity = 0;
                $scope.TotalValue = 0;
                $scope.TotalWR = 0;
               // $scope.TotalValue = 0;
                //$scope.TotalValue = 0;
                $scope.DataSet.forEach(function (item, indx) {
                    $scope.net_quantity +=  item.net_quantity;
                    $scope.TotalValue += item.TotalValue;
                    $scope.TotalWR += item.TotalWR;
                    //$scope.net_quantity += item.TotalValue;
                    //$scope.TotalValue += item.TotalValue;
                });
                $scope.net_quantity = $scope.net_quantity.toFixed(2);;
                $scope.TotalValue = $scope.TotalValue.toFixed(2);;
                //$scope.TotalWR = $scope.TotalWR.toFixed(2);;


                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SocietySubClassList = [];
                $(".se-pre-con").fadeOut("slow");
               // $scope.ShowbranchRport = false;
            });
    };

    $scope.Clear = function () {
        $(".se-pre-con").fadeIn("slow");
        $('#fyyear').val('');
        $('#ddrDistrictId').val('0');
        $('#iddwharehouse').val('0'); 
        $scope.fromDates = $('#txtFromDate').val('');
        $scope.Todates = $('#txtToDate').val(''); 
        $http.get(SiteUrl + "/MIS/GetWRAmmountReport/?Fy=" + '' + "&DistId=" + '0' + "&WharehousId=" + '0' + "&fromdatId=" + '' + "&TodatId=" + '').
            success(function (data, status) {
                $scope.DataSet = data;
                // $scope.ShowbranchRport = false;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SocietySubClassList = [];
                $(".se-pre-con").fadeOut("slow");
                // $scope.ShowbranchRport = false;
            });
        $(".se-pre-con").fadeOut("slow");
    };

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
    $scope.WarehouseList = [];

    $scope.WarehouseList = function () {
        $http.post(SiteUrl + "/Master/RajfedWarehouseList/", { "WarehouseId": "0" }).
            success(function (data, status) {
                $scope.WarehouseList = data;
            }).
            error(function () {
                $scope.Warehouses = [];
            });
    };

    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $("#DivisionId").val()).
            success(function (data, status) {
                $scope.DistrictList = data;
                //if ($("#hdnDistrictId").val() != "0") {
                //    $scope.DISTRICT_ID = $("#hdnDistrictId").val()
                //  //  $("#ddrDistrictId").attr("disabled", "disabled");
                //}
                // $scope.BindFY();

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };

    $scope.GetDistrictList();


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
    //start on page load
    $scope.OnPageLoad = function () {
 
    }


    
    //Get list of all users on page list
    $scope.getTranslation($scope);
    $scope.OnPageLoad();
   // $scope.PLDBLoanammountReportList('0');
    $scope.BindFY();
    $scope.WarehouseList();
    $scope.GetDistrictList();
    $scope.GetWRReport();



    $scope.ExportData = function () {
        $(".blockUI").fadeIn("slow");

        var uri = 'data:application/vnd.ms-excel;base64,'
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
        var fileName = "WR Quantity Report_" + $("#txtToDate").val() + ".xls";
        var tabledata = $("#tblExport").html();

        var excelData = "<table  border='1'><tr><th colspan='8' style='text-align:center;font-size:20px'>Daily Information related to E-WRs as on  " + $("#txtToDate").val() + "</th></tr><tr><td>" + tabledata + "</td></tr></table>";


        excelData = excelData.replace(/₹/g, "").replace(/\n/g, "");
        var ctx = { worksheet: "WR Physical Quantity Report" || 'Worksheet', table: excelData }
        //window.location.href = uri + $scope.base64($scope.format(template, ctx))
        var bytes = $scope.base64ToArrayBuffer($scope.base64($scope.format(template, ctx)));
        //var byte = $scope.toUTF8Array(data);
        var blob = new Blob([bytes], { type: "application/vnd.ms-excel" });
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);

        link.download = fileName;
        link.click();
        $(".blockUI").fadeOut("slow");
    }

    $scope.base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
    $scope.format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

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



});










