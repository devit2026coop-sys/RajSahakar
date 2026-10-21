var AppClasses = angular.module("LokGuaraanteeService", ['angularUtils.directives.dirPagination']);
AppClasses.controller("LokGuaraanteeServiceCont", function ($scope, $http, $rootScope) {
    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 50,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }
    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        var DateOne = "";
        DateOne = $('#txtMonth').Value
        var parameter = { Key: "DateOne", Value: DateOne }
        $scope.DataPaging.searchParameters.push(parameter);

        var Datefrom = "";
        Datefrom = $('#txtFromMonth').Value

        var parameter = { Key: "Datefrom", Value: Datefrom }
        $scope.DataPaging.searchParameters.push(parameter);

        var DateTo = "";
        Datefrom = $('#txtToMonth').Value
        var parameter = { Key: "DateTo", Value: DateTo }
        $scope.DataPaging.searchParameters.push(parameter);

    }

    $scope.AutoselectMonth = function () {

        var DT = new Date();
        var date = "";
        if ((DT.getMonth() + 1) < 10) {
            date = DT.getFullYear() + '-0' + (DT.getMonth() + 1);
        }
        else {
            date = DT.getFullYear() + '-' + (DT.getMonth() + 1);
        }
        $("#txtMonth").val(date);
    };
    $scope.AutoselectMonth();

    $scope.OnPageLoad = function () {
        $scope.AutoselectMonth();
        $("#txtFromMonth").val("");
        $("#txtToMonth").val("");
        $('#From_Time').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $scope.GetCCSListNew();
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
       
        var DateOne = "";

        DateOne = $('#txtMonth').Value
        var parameter = { Key: "DateOne", Value: DateOne }
        $scope.DataPaging.searchParameters.push(parameter);

        var Datefrom = "";
        Datefrom = $('#txtFromMonth').Value

        var parameter = { Key: "Datefrom", Value: Datefrom }
        $scope.DataPaging.searchParameters.push(parameter);

        var DateTo = "";
        Datefrom = $('#txtToMonth').Value
        var parameter = { Key: "DateTo", Value: DateTo }
        $scope.DataPaging.searchParameters.push(parameter);
        callback();
        
    }
    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.CCSId = '0';
        if ($scope.Filter.FilterType == 'created_dated') {
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
        $scope.Filter.CCSId = '0';
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];

        $scope.GetMonthlySummary();
    }
    /*------------End Paging and Filter-----------------*/

    $scope._language = 'en-us';
    $scope._language = _language;
    if (_language == 'en-us') {
        $http.get("../Translations/Translation_HI.json").
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

        $scope.ClearUnitForm = function () {
            $scope.UnitMaster = null;
        };
    }



    var monthStartDate = "01/" + ((new Date().getMonth() + 1).toString().length <= 1 ? "0" + (new Date().getMonth() + 1).toString() : (new Date().getMonth() + 1).toString()) + "/" + new Date().getFullYear();
    var currentDate = (new Date().getDate().toString().length <= 1 ? "0" + new Date().getDate().toString() : new Date().getDate().toString()) + "/" + ((new Date().getMonth() + 1).toString().length <= 1 ? "0" + (new Date().getMonth() + 1).toString() : (new Date().getMonth() + 1).toString()) + "/" + new Date().getFullYear();


    $("#txtFromDate").val(monthStartDate);
    $("#txtToDate").val(currentDate);

    $scope.NoOfApplicationReceived = 0;
    $scope.NoOfApplicationSubmited = 0;
    $scope.NoOfApplicationPending = 0;
    $scope.NoOfApplicationReceivedCurrMonth = 0;
    $scope.Col5N6 = 0;
    $scope.NoOfApplicationSubmitedCurrMonth = 0,
        $scope.OutOfDate = 0;
    $scope.UnderDate = 0;
    $scope.TotC0l10n11 = 0;

    $scope.DigitalRegisterList = [];
    $scope.MonthlySummaryRegister = [];
    $scope.RjApexBody = [];
    $scope.RjCentraBody = [];
    $scope.OfficeListData = [];
    $scope.OpeningMonth = "01.01.2019";
    $scope.lastMonth = "";


    $scope.getdateproper = function () {
        const monthNames = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        ];

        const today = $scope.monthD;
        $scope.dateummu = monthNames[today.getMonth()];
        $scope.dateummu = $scope.dateummu + '-' + today.getFullYear();

        let mm = today.getMonth() + 1; // Months start at 0!
        let dd = today.getDate();




        if (dd < 10) dd = '0' + dd;
        if (mm < 10) mm = '0' + mm;
        $scope.todaydate = dd + '/' + mm + '/' + yyyy;


    };

    



    $scope.GetMonthlySummary = function () {

        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };


         return $http.post(SiteUrl + "/MIS/GetLokGuaranteeService?Month=" + $("#txtMonth").val() + '&FromMonth=' + $("#txtFromMonth").val() + '&ToMonth=' + $("#txtToMonth").val(), JSON.stringify({ Details: jsonParameters })).
              success(function (data, status) {
                $(".blockUI").fadeOut("slow");


                  if ($("#txtFromMonth").val() != "") {
                      //const today = $("#txtFromMonth").val();
                      const today = $("#txtFromMonth").val();
                      today.split('-')
                      let yyyy = today.split('-')[0];
                      let mm = today.split('-')[1]; // Months start at 0!
                      let dd = today.split('-')[2];




                      //if (dd < 10) dd = '0' + dd;
                      //if (mm < 10) mm = '0' + mm;
                      $scope.todaydatefrom = dd + '/' + mm + '/' + yyyy;
                      
                } 

                  if ($("#txtToMonth").val() != "") {
                      const today = $("#txtToMonth").val();
                    

                      let yyyy = today.split('-')[0];
                      let mm = today.split('-')[1]; // Months start at 0!
                      let dd = today.split('-')[2];




                      //if (dd < 10) dd = '0' + dd;
                      //if (mm < 10) mm = '0' + mm;
                      $scope.todaydateTo = dd + '/' + mm + '/' + yyyy;
                      
                  } else {
                    //  $scope.lastMonth = "01.01.2019";
                  }



                $scope.RjApexBody1 = JSON.parse(data.DataList);
                $scope.RjApexBody = JSON.parse(data.DataList).filter(function (x) { return x.Type == 1 });
                $scope.RjCentraBody = JSON.parse(data.DataList).filter(function (x) { return x.Type == 2 });
                $scope.OfficeListData = JSON.parse(data.DataList).filter(function (x) { return x.Type != 1 });
                $scope.MonthlySummaryRegister = [];
                $scope.DigitalRegisterList = JSON.parse(data.DataList);

                $scope.NoOfApplicationReceived = 0;
                $scope.NoOfApplicationSubmited = 0;
                $scope.NoOfApplicationPending = 0;
                $scope.NoOfApplicationReceivedCurrMonth = 0;
                $scope.Col5N6 = 0;
                $scope.NoOfApplicationSubmitedCurrMonth = 0;
                $scope.OutOfDate = 0;
                $scope.UnderDate = 0;
                $scope.TotC0l10n11 = 0;

                $scope.DigitalRegisterList.forEach(function (itm, index) {
                    $scope.NoOfApplicationReceived += itm.NoOfApplicationReceived;
                    $scope.NoOfApplicationSubmited += itm.NoOfApplicationSubmited;
                    $scope.NoOfApplicationPending += itm.NoOfApplicationPending;
                    $scope.NoOfApplicationReceivedCurrMonth += itm.NoOfApplicationReceivedCurrMonth;
                    $scope.Col5N6 += itm.Col5N6;
                    $scope.NoOfApplicationSubmitedCurrMonth += itm.NoOfApplicationSubmitedCurrMonth;
                    $scope.OutOfDate += itm.OutOfDate;
                    $scope.UnderDate = itm.UnderDate;
                    $scope.TotC0l10n11 = itm.TotC0l10n11;
                });
               


                $scope.DigitalRegisterList.forEach(function (itm, index) {
                    if ($scope.MonthlySummaryRegister.filter(function (x) { return }).length <= 0) {
                        var Ydata = {
                            dt2: itm.SocietyName,
                            dt3: itm.Act, dt4: itm.NoOfApplicationReceived, dt5: itm.NoOfApplicationSubmited,
                            dt6: itm.NoOfApplicationPending, dt7: itm.NoOfApplicationReceivedCurrMonth, dt8: itm.Col5N6, dt9: itm.NoOfApplicationSubmitedCurrMonth, dt10: itm.OutOfDate, dt11: itm.UnderDate, dt12: itm.TotC0l10n11
                        }
                        $scope.DigitalRegisterList.filter(function (x) { return }).forEach(function (x, i) {
                            Ydata.dt4 += x.NoOfApplicationReceived;
                            Ydata.dt5 += x.NoOfApplicationSubmited;

                            Ydata.dt6 += x.NoOfApplicationPending;
                            Ydata.dt7 += x.NoOfApplicationReceivedCurrMonth;

                            Ydata.dt8 += x.Col5N6;
                            Ydata.dt9 += x.NoOfApplicationSubmitedCurrMonth;
                            Ydata.dt10 += x.OutOfDate;
                            Ydata.dt11 += x.UnderDate;
                            Ydata.dt12 += x.TotC0l10n11;
                        });
                        $scope.MonthlySummaryRegister.push(Ydata);

                        $scope.DigitalRegisterList.filter(function (a) { return }).forEach(function (z, i) {
                            if ($scope.MonthlySummaryRegister.filter(function (a) { return }).length <= 0) {
                                var Mdata = {
                                    dt2: itm.SocietyName,
                                    dt3: z.Act, dt4: "", dt5: "",
                                    dt6: 0, dt7: 0, dt8: 0, dt9: 0, dt10: 0, dt11: 0, dt12: 0
                                }

                                $scope.DigitalRegisterList.filter(function (a) { return a.Year === itm.Year && a.Month === z.Month }).forEach(function (b, i) {
                                    Mdata.dt4 += x.NoOfApplicationReceived;
                                    Mdata.dt5 += x.NoOfApplicationSubmited;
                                    Mdata.dt6 += x.NoOfApplicationPending;
                                    Mdata.dt7 += x.NoOfApplicationReceivedCurrMonth;
                                    Mdata.dt8 += x.Col5N6;
                                    Mdata.dt9 += x.NoOfApplicationSubmitedCurrMonth;
                                    Mdata.dt10 += x.OutOfDate;
                                    Mdata.dt11 += x.UnderDate;
                                    Mdata.dt12 += x.TotC0l10n11;

                                });
                                $scope.MonthlySummaryRegister.push(Mdata);
                                $scope.DigitalRegisterList.filter(function (a) { return a.Year === itm.Year && a.Month === z.Month }).forEach(function (b, i) {
                                    $scope.MonthlySummaryRegister.push({
                                        //dt1: itm.Year,
                                        dt2: b.SocietyName,
                                        dt3: b.Act,
                                        dt4: b.NoOfApplicationReceived,
                                        dt5: b.NoOfApplicationSubmited,
                                        dt6: b.NoOfApplicationPending,
                                        dt7: b.NoOfApplicationReceivedCurrMonth,
                                        dt8: b.Col5N6,
                                        dt9: b.NoOfApplicationSubmitedCurrMonth,
                                        dt10: b.OutOfDate,
                                        dt11: b.UnderDate,
                                        dt12: b.TotC0l10n11
                                    });
                                });
                            }

                        });
                    }
                });

             
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.DigitalRegisterList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };
    $scope.GetMonthlySummary();

    $scope.ExportData = function () {
        $(".blockUI").fadeIn("slow");
       
        $(".blockUI").fadeOut("slow");
        var uri = 'data:application/vnd.ms-excel;base64,'
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
        var fileName = "Monthly Summary Report";

        var excelData = "<table  border='1'><tr><th colspan='12' style='text-align:center;font-size:20px'>" + fileName + "</th></tr> " + $("#divExportPrint").html() + "</table>"
        excelData = excelData.replace(/₹/g, "");

        var ctx = { worksheet: "Monthly Summary" || 'Worksheet', table: excelData }
        //window.location.href = uri + $scope.base64($scope.format(template, ctx))


        var bytes = $scope.base64ToArrayBuffer($scope.base64($scope.format(template, ctx)));
        //var byte = $scope.toUTF8Array(data);
        var blob = new Blob([bytes], { type: "application/vnd.ms-excel" });
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);

        link.download = fileName;
        link.click();
        

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

    // $scope.SendMSG();
});