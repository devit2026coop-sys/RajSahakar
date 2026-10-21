var FertilizerReportModule = angular.module('FertilizerReportMod', ['angularUtils.directives.dirPagination']);
FertilizerReportModule.controller("FertilizerReportCnt", function ($scope, $http) {

    // SCOPE
    
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.divStateData = true;
    $scope.divDistrictWiseFertilizer = false;
    $scope.DistrictName = '';
    $scope.divRetailerData = false;
    $scope.divRetailerFertilizerTransData = false;


    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }

    $scope.Fertilizer = {};
    $scope.Fertilizer["DistrictId"] = '0';
    $scope.Fertilizer["FertilizerType"] = '0';
    $scope.Fertilizer["FilterType"] = 1;
    $scope.Fertilizer["dateOn"] = '';

    $("#txtFertilizerDateOn").datepicker("setDate", new Date());

    function FertilizerDetails() {
        this.DistrictNameEn = '';
        this.DistrictNameHi = '';
        this.DistrictId = '';
        this.Compost = 0;
        this.DAP = 0;
        this.MOP = 0;
        this.NPKS = 0;
        this.SSP = 0;
        this.Urea = 0;
        this.Date = '';
        this.RetailerName = '';
        this.BlockName = '';
        this.Mobile = '';
    }

    function FertilizerTransactionDetails() {
        this.DistrictNameEn = '';
        this.DistrictNameHi = '';
        this.TotalTransaction = 0;
        this.TotalSale = 0;
    }

    //START : Filter Type Change
    $scope.FilterTypeChanged = function () {

        $scope.divStateData = false;
        $scope.divStateDataRetailerTransaction = false;

        $scope.divDistrictWiseFertilizer = false;
        $scope.divDistrictWiseRetailerTransaction = false;
        $scope.divRetailerData = false;
        $scope.Fertilizer.NatureId = '0';
        $scope.divRetailerFertilizerTransData = false;

        if ($scope.Fertilizer.FilterType == '1') {
            $scope.RetailerTransactionMonth = [];
            $scope.Fertilizer.FertilizerYear = '0';
            $scope.Fertilizer.FertilizerMonth = '0';
            $scope.Fertilizer.DISTRICT_ID = '0';
            $scope.Fertilizer.FertilizerType = '0';
            $('#txtFertilizerDateOn').val('');
            $scope.GetFilterReportData();
            $scope.divStateData = true;
            
        }
        else {
            $scope.Fertilizer.DateOn = '';
            $scope.Fertilizer.DISTRICT_ID = '0';
            $scope.Fertilizer.FertilizerYear = '0';
            $scope.Fertilizer.FertilizerMonth = '0';
            $scope.GetFertilizerRetailerTransactionReport();
            $scope.divStateDataRetailerTransaction = true;
        }
    }
    //END: Filter Type Change

    // on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");

        $('#txtFertilizerDateOn').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            minDate: '01/08/2019'
        });




        if (_language == 'en-us') {
            $http.get("../Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("../Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });

        }



        $scope.DistrictList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });


        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FERTILIZER&MasterCode=0").
        //    success(function (data, status) {
        //        $scope.Fertilizers = data;
        //    }).
        //    error(function (data, status) {
        //        $scope.Fertilizers = [];
        //    });


        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DEALERNATURE&MasterCode=0").
            success(function (data, status) {
                $scope.DealerNatures = data;
            }).
            error(function (data, status) {
                $scope.DealerNatures = [];
            });

        $scope.RetailerTransactionYear = [];
        $http.get(SiteUrl + "/MIS/RetailerTransactionYearMonthList").
            success(function (data, status) {
                $scope.RetailerTransactionYearMonthAllData = data;
                angular.forEach(data, function (i, j) {
                    if ($scope.RetailerTransactionYear.length == 0) {
                        $scope.RetailerTransactionYear.push(i.YearId);
                    }
                    else if ($scope.RetailerTransactionYear.filter(p => p == i.YearId).length == 0) {
                        $scope.RetailerTransactionYear.push(i.YearId);
                    }
                    
                });
            }).
            error(function (data, status) {
                $scope.RetailerTransactionYear = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $scope.SetDate();
        $(".se-pre-con").fadeOut("slow");
    };
    //END page load 

    $scope.SetDate = function () {
        $('#txtFertilizerDateOn').datepicker('setDate', new Date());
        $(".se-pre-con").fadeOut("slow");
    };

    //START : Get Month For Retailer Transaction
    $scope.GetRetailerMonthList = function () {
        var yearid = $scope.Fertilizer.FertilizerYear;
        $scope.RetailerTransactionMonth = [];

        var temp = $scope.RetailerTransactionYearMonthAllData.filter(c => c.YearId == yearid);

        if (temp.length > 0) {
        }
        angular.forEach(temp, function (i, j) {
            if ($scope.RetailerTransactionMonth.length == 0) {
                $scope.RetailerTransactionMonth.push(i);
            }
            else if ($scope.RetailerTransactionMonth.filter(p => p.YearId == i.YearId).length == 0) {
                $scope.RetailerTransactionMonth.push(i);
            }
        });
    }
    //END : Get Month For Retailer Transaction

    // START : Get Report Data
    $scope.GetFilterReportData = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.DistrictId1 = $scope.Fertilizer.DISTRICT_ID;
        $scope.NatureId1 = $scope.Fertilizer.NatureId;
        $scope.FilterType1 = $scope.Fertilizer.FilterType;
        $scope.dateOn1 = $('#txtFertilizerDateOn').val();


        $http.post("../MIS/GetFertilizerReportData", {
            "DistrictId": $scope.Fertilizer.DISTRICT_ID, "NatureId": $scope.Fertilizer.NatureId, "FilterType": $scope.Fertilizer.FilterType, "dateOn": $('#txtFertilizerDateOn').val(), "param1": "2"
        }).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                if (data.length === 0) {
                    //var msg = _language == 'en-us' ? 'No data found.' : 'कोई डेटा नहीं मिला।';
                    //ShowModelAlert('', msg);
                      $(".se-pre-con").fadeOut("slow");
                    $scope.StateData = [];
                    $scope.DistrictData = [];
                    return;
                }

                $scope.FertilizerFinalData = data;
                $scope.StateData = data.data[data.data.length - 1];
                var RecordOnDate = data.RecordOnDate;
                $scope.ReportHeader = _language === 'en-us' ? 'As On :' + RecordOnDate : 'दिनांक: ' + RecordOnDate + ' पर';

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.StateData = [];
                $(".se-pre-con").fadeOut("slow");
                $scope.DistrictData = [];
                alert('Error while fetching State and District PACS data.');
                $(".se-pre-con").fadeOut("slow");
            });
    }
    //END : GEt Report Data

    // START : Show District Wise PACS data
    $scope.ShowDistrictData = function () {
        $scope.divDistrictWiseFertilizer = true;
        $scope.divStateData = false;
    }
    //END : Show District Wise PACS data


    // START : Get Fertilizer data
    $scope.GetFertilizerData = function () {

        if ($scope.Fertilizer.FilterType == 1) {
            //$scope.RetailerTransactionMonth = [];
            //$scope.Fertilizer.FertilizerYear = '0';
            //$scope.Fertilizer.FertilizerMonth = '0';
            //$scope.Fertilizer.DISTRICT_ID = '0';
            //$scope.Fertilizer.FertilizerType = '0';
            //$('#txtFertilizerDateOn').val('');
            $scope.GetFilterReportData();
            $scope.divStateData = false;
            $scope.divDistrictWiseFertilizer = true;
            $scope.divRetailerData = false;
            $scope.divDistrictWiseRetailerTransaction = false;
        }
        else {
            $scope.Fertilizer.DateOn = '';
            //$scope.Fertilizer.DISTRICT_ID = '0';
            //$scope.Fertilizer.FertilizerYear = '0';
            //$scope.Fertilizer.FertilizerMonth = '0';
            $scope.GetFertilizerRetailerTransactionReport();
            $scope.divStateDataRetailerTransaction = false;
            $scope.divDistrictWiseRetailerTransaction = true;
        }
    }
    //END : Get Fertilizer Data

    $scope.BackToState = function () {
        $scope.divStateData = true;
        $scope.divDistrictWiseFertilizer = false;

    }

    $scope.Reset = function () {
        window.location = '../MIS/FertilizerReport';
        //$scope.OnPageLoad();
        //$scope.GetFilterReportData();
    }

    //START : Get FertilizerRetailerTransactionReport Data
    $scope.GetFertilizerRetailerTransactionReport = function () {
        $scope.NatureId = $scope.Fertilizer.NatureId;
        $scope.FilterType = $scope.Fertilizer.FilterType;
        $scope.Year = $('#ddlTransactionYear:selected').text().trim();
        $scope.Month = $scope.Fertilizer.FertilizerMonth;

        $scope.StateFertilizeTransactionData = [];
        $scope.DistrictFertilizeTransactionData = [];

        $http.post("../MIS/GetFertilizerReportData", {
            "param1": 2, "DistrictId": $scope.Fertilizer.DISTRICT_ID, "NatureId": $scope.Fertilizer.NatureId, "FilterType": $scope.Fertilizer.FilterType, "dateOn": "", "Year": $('#ddlTransactionYear:selected').text().trim(), "Month": $scope.Fertilizer.FertilizerMonth
        }).
            success(function (data, status) {
                var alldata = [];
                if (data.length == 0) {
                    var msg = _language == 'en-us' ? 'No data found.' : 'कोई डेटा नहीं मिला।';
                    ShowOkMessage('', msg, null);
                    $scope.StateRetailerTransactionData = [];
                    $scope.DistrictRetailerTransactionData = [];
                    return;
                }


                $scope.ReportHeader2 = _language === 'en-us' ? 'As On: ' + data[0].Month_En + ', ' + data[0].YearId : data[0].Month_En + ', ' + data[0].YearId + ' में';

                var P = new FertilizerTransactionDetails();
                P.DistrictNameEn = 'Rajasthan';
                P.DistrictNameHi = 'राजस्थान';
                alldata.push(P);

                angular.forEach(data, function (i, j) {
                    P.TotalTransaction += parseFloat(i.TransactionCount);
                    P.TotalSale += parseFloat(i.QuantitySold);
                });

                alldata[0].TotalSale = alldata[0].TotalSale.toFixed(3);
                $scope.StateFertilizeTransactionData = alldata;
                $scope.DistrictFertilizeTransactionData = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.StateData = [];
                $(".se-pre-con").fadeOut("slow");
                $scope.DistrictData = [];
                alert('Error while fetching State and District PACS data.');
                $(".se-pre-con").fadeOut("slow");
            });
    }
    //END : FertilizerRetailerTransactionReport Data

    //START : Get FertilizerRetailerTransactionReport Data
    $scope.GetFertilizerRetailerDetailTransaction = function (id) {
        
        if (id == -1) {
            return;
        }

        $http.post("../MIS/GetFertilizerReportData", {
            "DistrictId": id, "NatureId": $scope.NatureId1, "FilterType": $scope.FilterType1, "dateOn": $scope.dateOn1, "param1": "3"
        }).
            success(function (data, status) {
                if (data.length === 0) {
                    var msg = _language == 'en-us' ? 'No data found.' : 'कोई डेटा नहीं मिला।';
                    ShowModelAlert('', msg);
                    $scope.StateData = [];
                    $scope.DistrictData = [];
                    return;
                }

                $scope.divRetailerFertilizerTransData = true;
                var dataOnDate = data.RecordOnDate;
                $scope.ReportHeader = _language === 'en-us' ? 'As On: ' + dataOnDate : ' दिनांक:  ' + dataOnDate + ' पर ';
                $scope.divDistrictWiseFertilizer = false;
                $scope.retailerTransactionDetail = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.StateData = [];
                $(".se-pre-con").fadeOut("slow");
                $scope.DistrictData = [];
                alert('Error while fetching State and District PACS data.');
                $(".se-pre-con").fadeOut("slow");
            });
    }
    //END : FertilizerRetailerTransactionReport Data

    $scope.BackToDistricteForTransactionRetailer = function () {
        $scope.divRetailerFertilizerTransData = false;
        $scope.divDistrictWiseFertilizer = true;
    }


    //START : Get Retailer Detail Data
    $scope.GetRetailerDetails = function (districtid) {

        $http.post("../MIS/GetFertilizerReportData", {
            "DistrictId": districtid, "NatureId": $scope.NatureId, "FilterType": '2', "param1": "3", "dateOn": "", "Year": $scope.Year, "Month": $scope.Month
        }).
            success(function (data, status) {
                var alldata = [];
                if (data.length === 0) {
                    $scope.divRetailerData = false;
                    var msg = _language === 'en-us' ? 'No data found.' : 'कोई डेटा नहीं मिला।';
                    ShowOkMessage('', msg, null);
                    $scope.RetailerDetailsData = [];
                    return;
                }

                $scope.divDistrictWiseRetailerTransaction = false;
                $scope.divRetailerData = true;
                $scope.RetailerDetailsData = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.StateData = [];
                $(".se-pre-con").fadeOut("slow");
                $scope.DistrictData = [];
                alert('Error while fetching State and District PACS data.');
                $(".se-pre-con").fadeOut("slow");
            });
    };
    //END : Get Retailer Detail

    // START : Show ShowFertilizerTransactionDistrictData
    $scope.ShowFertilizerTransactionDistrictData = function () {
        $scope.divStateData = false;
        $scope.divStateDataRetailerTransaction = false;

        $scope.divDistrictWiseFertilizer = false;
        $scope.divDistrictWiseRetailerTransaction = true;
    }
    //END : Show ShowFertilizerTransactionDistrictData

    // SHOW: Back Transaction State Wise
    $scope.BackToStateForTransaction = function () {
        $scope.divStateData = false;
        $scope.divStateDataRetailerTransaction = true;

        $scope.divDistrictWiseFertilizer = false;
        $scope.divDistrictWiseRetailerTransaction = false;
    }
    // END : Back Transaction State Wise

    // SHOW: Back Transaction State Wise
    $scope.BackToDistricteForTransaction = function () {
        $scope.divStateData = false;
        $scope.divStateDataRetailerTransaction = false;

        $scope.divDistrictWiseFertilizer = false;
        $scope.divRetailerData = false;
        $scope.divDistrictWiseRetailerTransaction = true;
    }
    // END : Back Transaction State Wise

    $scope.OnPageLoad();
    $scope.GetFilterReportData();
});