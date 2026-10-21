var societyDistrictwise = angular.module("MPRStateCoopBankmodule", ['angularUtils.directives.dirPagination']);
societyDistrictwise.controller("MPRStateCoopBankCnt", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.pageSize = 100;
    $scope.DistrictId = 0;
    debugger;
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
           
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        };
         $scope.GetFinancialYearList();
      //  $scope.AutoselectMonth();
    };
  
  


    //GET Financial Year
    $scope.GetFinancialYearList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
              
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    };


  

    $scope.ExportData = function () {
       
                var excel = $JExcel.new("Calibri light 10 #333333");			// Default font	

        excel.set({ sheet: 0, value: "Data" });

                // We want ROW 3 to be EXTRA TALL

        var headers = [$scope.translation.SRNO, $scope.translation.Sansthaokawarg, $scope.translation.Gatmahtakpanjikritsocietykisankhya, $scope.translation.Mahmepanjikritsocietykisankhya,
            $scope.translation.Mahmenirstpanjiynokisankhya, $scope.translation.Mahkeantmepanjikritsocietykikullsankhya, $scope.translation.columnsankhya6medarsayigyisamitiyomesesakriysamitiya,
            $scope.translation.columnsankhya6medarsayigyisamitiyomeseNiskriysamitiya, $scope.translation.columnsankhya6medarsayigyisamitiyomeseAAwasaynadhinsamitiya,
            $scope.translation.columnsankhya6medarsayigyisamitiyokemahkeanttakkulsadashyasankha, $scope.translation.columnsankhya6medarsayigyisamitiyokemahkeanttaksishsharashihajarome, $scope.translation.columnsankhya6medarsayigyisamitiyokemahkeanttakaryashilpunjihajarome
                     ];


                var curr = 1

                var formatHColumn = excel.addStyle({ 															// Format for headers
                    //border: "thin,thin,thin,thin #333333", 													// 		Border for header
                    font: "Calibri 15 #324148"
                });


                excel.set(0, 0, curr, $scope.translation.MPR2001MembersShareCapital, formatHColumn);


        if ($('#txtFromMonth').val() != '' || $('#txtFromMonth').val() == undefined) {
                    ++curr;
                  excel.set(0, 0, curr, $scope.translation.Month, formatHColumn);
            excel.set(0, 1, curr, $('#txtFromMonth').val(), formatHColumn);
                }

                if ($scope.DistrictId != '' || $scope.DistrictId == undefined) {
                    ++curr;
                    excel.set(0, 0, curr, $scope.translation.DistrictHeading, formatHColumn);
                    excel.set(0, 1, curr, $scope.DistrictList.filter(function (x) { return x.DISTRICT_ID == $scope.DistrictId })[0].DISTRICT_MANGAL , formatHColumn);
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
                    excel.set(0, i, curr + 1, headers[i], formatHeader);													// Set CELL with header text, using header format
                    excel.set(0, i, undefined, "auto");															// Set COLUMN width to auto (according to the standard this is only valid for numeric columns)
                }
                // 		in color green

                var formatColumn = excel.addStyle({ 															// Format for headers
                    border: "thin,thin,thin,thin #333333", 													// 		Border for header
                    font: "Calibri 10 #324148"
                });

        curr = curr + 1;
        excel.set(0, 0, curr + 1, 1, formatColumn);
        excel.set(0, 1, curr + 1, 2, formatColumn);
        excel.set(0, 2, curr + 1, 3, formatColumn);
        excel.set(0, 3, curr + 1, 4, formatColumn);
        excel.set(0, 4, curr + 1, 5, formatColumn);
        excel.set(0, 5, curr + 1, 6, formatColumn);
        excel.set(0, 6, curr + 1, 7, formatColumn);
        excel.set(0, 7, curr + 1, 8, formatColumn);
        excel.set(0, 8, curr + 1, 9, formatColumn);
        excel.set(0, 9, curr + 1, 10, formatColumn);
        excel.set(0, 10, curr + 1, 11, formatColumn);
        excel.set(0, 11, curr + 1, 12, formatColumn);




                curr = curr + 2;
        $scope.SocietyDistrictwiseMPR.forEach(function (item, index) {
            excel.set(0, 0, index + curr, (index + 1), formatColumn);
            excel.set(0, 1, index + curr, item.CategoryNameHi, formatColumn);
            excel.set(0, 2, index + curr, item.GatMah, formatColumn);
            excel.set(0, 3, index + curr, item.MahMePanjikrit, formatColumn);
            excel.set(0, 4, index + curr, item.MahMeNirastPanjiyno, formatColumn);
            excel.set(0, 5, index + curr, item.MahKeAntMePanjikritsocietyKiKulSankhya, formatColumn);
            excel.set(0, 6, index + curr, item.KalamNo6MeSakriySamitiya, formatColumn);
            excel.set(0, 7, index + curr, item.KalamNo6MeNiskriySamitiya, formatColumn);
            excel.set(0, 8, index + curr, item.KalamNo6MeLiqividationSamitiya, formatColumn);
            excel.set(0, 9, index + curr, item.KalamNo6MeDarshayiAntTakKulSadashySankhya, formatColumn);
            excel.set(0, 10, index + curr, item.KalamNo6MeDarshayiAntTakHissaRashi, formatColumn);
            excel.set(0, 11, index + curr, item.KalamNo6MeDarshayiAntTakkaryshilPunji, formatColumn);
        });

        curr = $scope.SocietyDistrictwiseMPR.length + curr - 1;;
        excel.set(0, 0, curr + 1, "", formatColumn);
        excel.set(0, 1, curr + 1, $scope.translation.Yog, formatColumn);
        excel.set(0, 2, curr + 1, $scope.GatMah, formatColumn);
        excel.set(0, 3, curr + 1, $scope.MahMePanjikrit, formatColumn);
        excel.set(0, 4, curr + 1, $scope.MahMeNirastPanjiyno, formatColumn);
        excel.set(0, 5, curr + 1, $scope.MahKeAntMePanjikritsocietyKiKulSankhya, formatColumn);
        excel.set(0, 6, curr + 1, $scope.KalamNo6MeSakriySamitiya, formatColumn);
        excel.set(0, 7, curr + 1, $scope.KalamNo6MeNiskriySamitiya, formatColumn);
        excel.set(0, 8, curr + 1, $scope.KalamNo6MeLiqividationSamitiya, formatColumn);
        excel.set(0, 9, curr + 1, $scope.KalamNo6MeDarshayiAntTakKulSadashySankhya, formatColumn);
        excel.set(0, 10, curr + 1, $scope.KalamNo6MeDarshayiAntTakHissaRashi, formatColumn);
        excel.set(0, 11, curr + 1, $scope.KalamNo6MeDarshayiAntTakkaryshilPunji, formatColumn);

        excel.generate($scope.translation.MPR2001MembersShareCapital+".xlsx");
           
    }


  
    //Madhuri


    //Mem_Soc
    //Sh_G
    //Sh_soc
    //Dep
    //Res
    //Ow_f
    //Borr
    //Woc
    //LoAd
    //LoRe
    //Profit



    $scope.SaveCoopState = function () {

        //$scope.Coop.RJCoopMovId = $scope.dataid;
       // alert("Hello");
        //debugger;
      
        //alert($scope.Liqudation.LiqudationDataId);
        if ($scope.CoopStFormValidation()) {
           // alert("Year:" + $("#txtFromMonth").val());
            var data = {
             Year: $("#txtFromMonth").val(),

            
            Number              :$scope.Coop.Number,
            Membership_Gov      :$scope.Coop.MemGov,
            Membership_Soc      :$scope.Coop.Mem_Soc,
            ShareCapital_Gov    :$scope.Coop.Sh_G,
            ShareCapital_Soc    :$scope.Coop.Sh_soc,
            Deposit             :$scope.Coop.Dep,
            Reserve             :$scope.Coop.Res,
            OwnedFunds          :$scope.Coop.Ow_f,
            Borrowing           :$scope.Coop.Borr,
            WorkingCapital      :$scope.Coop.Woc,
            LoanAdvances        :$scope.Coop.LoAd,
            LoanRecoveries      :$scope.Coop.LoRe,
            ProfitDuringTheYear :$scope.Coop.Profit
          
           

            }

            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/MIS/SaveCoopState", JSON.stringify({ _Model: data })).
                success(function (data, status) {
                    // alert("DataValue "+data);
                    $(".se-pre-con").fadeOut("slow");
                    var message = '';
                    if (data > 0) {
                        message = _language == 'en-us' ? 'Data Save Successfully\n' : 'डेटा सहेजा गया है\n';
                    }
                    else if (data == -1) {
                        message = _language == 'en-us' ? 'Data Update successfully\n' : 'डेटा सफलतापूर्वक अपडेट किया गया है\n';
                    }
                    else if (data == -2) {
                        message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                    }
                    else if (data == -3) {
                        message = _language == 'en-us' ? 'Data Delete Successfully\n' : 'डेटा सफलतापूर्वक हटाएं\n';

                    }
                    else {
                        message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    }
                    alert(message);
                    window.location.reload();

                    if (data.Status) {
                        $scope.ClearLiquidationForm();
                    }
                }).
                error(function (data, status) {
                    alert("Error while Liquidation Data insert");
                });
        }

    };
 
    //validateAddForm 
    $scope.CoopStFormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';


        if ($("#txtFromMonth").val() == undefined || $("#txtFromMonth").val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Financial Year' : '\n <br /> कृपया Financial Year का चयन करे |';
        }

        if ($scope.Coop.Number == undefined || $scope.Coop.Number == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Number ' : '\n <br /> कृपया  Number दर्ज करें|';
        }
        if ($scope.Coop.MemGov == undefined || $scope.Coop.MemGov == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Membership of Govt.' : '\n <br /> कृपया Membership of Govt. दर्ज करें |';
        }
        
        if ($scope.Coop.Mem_Soc == undefined || $scope.Coop.Mem_Soc == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter  Membership of of Societies' : '\n <br /> कृपया  Membership of of Societies राशि दर्ज करें |';
        }


        if ($scope.Coop.Sh_G == undefined || $scope.Coop.Sh_G == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Share Capital of Govt. ' : '\n <br /> कृपया Share Capital of Govt. राशि दर्ज करें |';
        }


        if ($scope.Coop.Sh_soc == undefined || $scope.Coop.Sh_soc == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Share Capital of Societies' : '\n <br /> कृपया  Share Capital of Societies राशि दर्ज करें |';
        }

        if ($scope.Coop.Dep == undefined || $scope.Coop.Dep == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Amount of Deposit' : '\n <br /> कृपया Deposit  राशि दर्ज करें |';
        }

        if ($scope.Coop.Res == undefined || $scope.Coop.Res == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Amount of Reserve' : '\n <br /> कृपया Reserve राशि दर्ज करें |';
        }

        if ($scope.Coop.Ow_f == undefined || $scope.Coop.Ow_f == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Amount of Owned Fund' : '\n <br /> कृपया  Owned Fund राशि दर्ज करें |';
        }


        if ($scope.Coop.Borr == undefined || $scope.Coop.Borr == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter  Amount of Borrowing' : '\n <br /> कृपया  Borrowing राशि दर्ज करें |';
        }

        if ($scope.Coop.Woc == undefined || $scope.Coop.Woc == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter  Amount of Working Capital' : '\n <br /> कृपया Working Capital राशि दर्ज करें |';
        }


        if ($scope.Coop.LoAd == undefined || $scope.Coop.LoAd == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter  Amount of Loan Advances ' : '\n <br /> कृपया Loan Advances  राशि दर्ज करें |';
        }
        if ($scope.Coop.LoRe == undefined || $scope.Coop.LoRe == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter  Amount of Loan Recovering' : '\n <br /> कृपया Loan Recovering  राशि दर्ज करें |';
        }
        if ($scope.Coop.Profit == undefined || $scope.Coop.Profit == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Profit During the Year' : '\n <br /> कृपया Profit During the Year  राशि दर्ज करें |';
        }


        
        if (message.length > 23) {
            ShowModelAlert("Cooperative Movement In Rajasthan Form", message);
            return false;
        }
        return true;

    }



    $scope.showEntryForm = function () {

        $scope.EntryForm = 1;
        $scope.dataList = 0;


    }
    $scope.back = function () {

        $scope.EntryForm = 0;
        $scope.dataList = 1;


    }


    $scope.GridData = function () {

        $(".se-pre-con").fadeIn("slow");
        //debugger;
        $scope.CoopStateData = [];
        $http.post(SiteUrl + "/MIS/GetCoopStateData?DistrictId=" + 0 + "&Year=" + $("#FY").val()).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");

                $scope.CoopStateData = data;
                // $scope.CoopRjData = JSON.parse(data.DataList);
            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
            });
    }


    $scope.update = function (item) {
        //alert(item.LiqudationDataId);
        $scope.showEntryForm = true;
        $scope.DistrictId = item.DistrictId;

        Year: $("#txtFromMonth").val(),
            $scope.Coop.Cosoc = item.CooperativeSociety;
        $scope.Coop.Member = item.Membership;
        $scope.Coop.shca = item.ShareCapital;
        $scope.Coop.woca = item.WorkingCapital;
        $scope.Coop.SLoanAd = item.St_Loan_Advancement;
        $scope.Coop.SLoanRe = item.St_Loan_Recovered;
        $scope.Coop.MLoanAd = item.Mt_Loan_Advancement;
        $scope.Coop.MLoanRe = item.Mt_Loan_Recovered;
        $scope.Coop.LLoanAd = item.Lt_Loan_Advancement;
        $scope.Coop.LLoanRe = item.Lt_Loan_Recovered;
        $scope.Coop.Pur = item.Purchase;
        $scope.Coop.Sale = item.Sale



    };





    $scope.dataList = 1;

    $scope.GridData();

    $scope.GetFinancialYearList();





    $scope.OnPageLoad();
});