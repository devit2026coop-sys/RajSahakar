var WomanInformationSocietyModule = angular.module("WomanInformationSocietyRPTMod", ['angularUtils.directives.dirPagination']);
WomanInformationSocietyModule.controller("WomenInformationSocietyRPTCont", function ($rootScope, $scope, $http) {
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    

        $scope.OnPageLoad = function () {

            $scope._language = _language;
            if (_language == 'en-us') {
                $http.get(SiteUrl + "/Translations/Translation_HI.json").
                    success(function (data, status) {
                        $scope.translation = data;
                        $scope.GetWomenInformationSociety();
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
            };
        }

    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
    }

    $scope.ClearSearch = function () {
        $scope.Filter.SerachText = "";
    }


    $scope.clear = function () {

        $scope.TotReg = 0;
        $scope.TotActive = 0;
        $scope.TotInactive = 0;
        $scope.TotResedential = 0;


    }

    $scope.GetWomenInformationSociety = function () {
       
        $scope.WomenInformationSociety = [];
        $(".se-pre-con").fadeIn("slow");
       // alert("Month" + $("#txtFromMonth").val() + "District Code: " + $("#ddrDistrictId").val() );
      //  debugger;
        $http.post(SiteUrl + "MIS/GetWomenInformationSociety?DistrictId=" + $("#ddrDistrictId").val() + "&Month=" + $("#txtFromMonth").val()).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.clear();
                $scope.WomenInformationSociety = data;
                $scope.WomenInformationSociety.forEach(function (item) {
                
                    $scope.TotReg = parseInt($scope.TotReg) + parseInt(item.TotReg);
                    $scope.TotActive = parseInt($scope.TotActive) + parseInt(item.TotActive);
                    $scope.TotInactive = parseInt($scope.TotInactive) + parseInt(item.TotInactive);
                    $scope.TotResedential = parseInt($scope.TotResedential) + parseInt(item.TotResedential);
                });
            }).
            error(function (data, status) {
                alert("Geting Some Errors");
            });

    };

    $scope.ExportData = function () {

        var excel = $JExcel.new("Calibri light 10 #333333");			// Default font	

        excel.set({ sheet: 0, value: "Data" });

        // We want ROW 3 to be EXTRA TALL




        var curr = 1

        var formatHColumn = excel.addStyle({ 															// Format for headers
            //border: "thin,thin,thin,thin #333333", 													// 		Border for header
            font: "Calibri 15 #324148"
        });


        excel.set(0, 0, curr, $scope.translation.ReportClassifiedWomenSocieties, formatHColumn);


        if ($('#txtFromMonth').val() != '' || $('#txtFromMonth').val() == undefined) {
            ++curr;
            excel.set(0, 0, curr, $scope.translation.Month, formatHColumn);
            excel.set(0, 1, curr, $('#txtFromMonth').val(), formatHColumn);
        }

        if ($("#ddrDistrictId").val() != '' &&  $("#ddrDistrictId").val() != '0') {
            ++curr;
            excel.set(0, 0, curr, $scope.translation.DistrictHeading, formatHColumn);
            excel.set(0, 1, curr, $scope.DistrictList.filter(function (x) { return x.DISTRICT_ID == $("#ddrDistrictId").val() })[0].DISTRICT_MANGAL, formatHColumn);
        }

        // This array holds the HEADERS text
        var formatHeader = excel.addStyle({ 															// Format for headers
            border: "thin,thin,thin,thin #333333", 													// 		Border for header
            font: "Calibri 12 #fff B",
            background: "#64c0c5",
        }); 														// 		Font for headers
        curr = curr + 2;
        //for (var i = 0; i < headers.length; i++) {
        //    //++curr;// Loop all the haders
        //    excel.set(0, i, curr + 1, headers[i], formatHeader);													// Set CELL with header text, using header format
        //    excel.set(0, i, undefined, "auto");															// Set COLUMN width to auto (according to the standard this is only valid for numeric columns)
        //}





        excel.set(0, 0, curr + 1, $scope.translation.SRNO, formatHeader);
        excel.set(0, 1, curr + 1, $scope.translation.NameClassifiedWomenSocieties, formatHeader);
        excel.set(0, 2, curr + 1, $scope.translation.NoWomenSocietiesRegistered, formatHeader);
        excel.set(0, 3, curr + 1, $scope.translation.NumberOfActiveSocieties, formatHeader);
        excel.set(0, 4, curr + 1, $scope.translation.NumberOfInactiveSocieties, formatHeader);
        excel.set(0, 5, curr + 1, $scope.translation.NumberOfLiquidationSocieties, formatHeader);




        // 		in color green

        var formatColumn = excel.addStyle({ 															// Format for headers
            border: "thin,thin,thin,thin #333333", 													// 		Border for header
            font: "Calibri 10 #324148"
        });
        

        curr = curr + 2;
        $scope.WomenInformationSociety.forEach(function (item, index) {
            excel.set(0, 0, index + curr, (index + 1), formatColumn);
            excel.set(0, 1, index + curr, item.CategoryNameHi, formatColumn);
            excel.set(0, 2, index + curr, item.TotReg, formatColumn);
            excel.set(0, 3, index + curr, item.TotActive, formatColumn);
            excel.set(0, 4, index + curr, item.TotInactive, formatColumn);
            excel.set(0, 5, index + curr, item.TotResedential, formatColumn);            
        });
   
        curr = curr+ $scope.WomenInformationSociety.length;
        excel.set(0, 0, curr , "", formatHeader);
        excel.set(0, 1, curr, $scope.translation.Yog, formatHeader);
        excel.set(0, 2, curr , $scope.TotReg, formatHeader);
        excel.set(0, 3, curr, $scope.TotActive, formatHeader);
        excel.set(0, 4, curr , $scope.TotInactive, formatHeader);
        excel.set(0, 5, curr , $scope.TotResedential, formatHeader);


        excel.generate($scope.translation.ReportClassifiedWomenSocieties + ".xlsx");

    }





    $scope.mailfun = function () {
        $scope.truevalue = true;
    }

    $scope.GetWomenInformationSocietyDetails = function (id) {
        $scope.WomenInformationSocietyDetail = [];
        $http.post(SiteUrl + "MIS/GetWomenInformationSocietyDetails", { "Whereclouse": $scope.Filter.SerachText,"ClassId": id }).
            success(function (data, status) {
                $scope.WomenInformationSocietyDetail = data;
                $scope.truevalue = true;
            }).
            error(function (data, status) {
                alert("Geting Some Errors");
            })
    };
    $scope.DistrictList = [];
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0" && $("#hdnDistrictId").val() != "") {
                    $("#ddrDistrictId").attr("disabled", "disabled");
                    $scope.DistrictId = $("#hdnDistrictId").val()
                }
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    $scope.AutoselectMonth = function () {
        var DT = new Date();
        var date = "";
        if ((DT.getMonth() + 1) < 10) {
            date = DT.getFullYear() + '-0' + (DT.getMonth() + 1);
        }
        else {
            date = DT.getFullYear() + '-' + (DT.getMonth() + 1);
        }
       // $("#txtFromMonth").val(date);
    };
    $scope.GetDistrictList();
    $scope.AutoselectMonth();
    $scope.OnPageLoad();
    $scope.GetWomenInformationSociety();
   //$scope.GetWomenInformationSocietyDetails();
   
});


