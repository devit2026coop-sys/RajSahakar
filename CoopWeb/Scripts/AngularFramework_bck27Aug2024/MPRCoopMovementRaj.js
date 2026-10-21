var societyDistrictwise = angular.module("Mprmodule", ['angularUtils.directives.dirPagination']);
societyDistrictwise.controller("MprCount", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.pageSize = 100;
    $scope.DistrictId = 0;



    $scope.Liqudation = {
        RJCoopMovId:             "0",
        DistrictId:               "0",
        Year: "",
        CooperativeSociety:  "0",
        Membership:          "0",
        ShareCapital:        "0" ,   
        WorkingCapital:         "0",
        St_Loan_Advancement:     "0",
        St_Loan_Recovered:      "0",
        Mt_Loan_Advancement:     "0",
        Mt_Loan_Recovered:      "0",
        Lt_Loan_Advancement: "0",
        Lt_Loan_Recovered:      "0",
        Purchase:            "0",
        Sale:                "0"



    }

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
      

       // $scope.AutoselectMonth();
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
        $("#txtFromMonth").val(date);

    };
    $scope.GatMah = 0;
    $scope.MahMePanjikrit = 0;
    $scope.MahMeNirastPanjiyno = 0;
    $scope.MahKeAntMePanjikritsocietyKiKulSankhya = 0;
    $scope.KalamNo6MeSakriySamitiya = 0;
    $scope.KalamNo6MeNiskriySamitiya = 0;
    $scope.KalamNo6MeLiqividationSamitiya = 0;
    $scope.KalamNo6MeDarshayiAntTakKulSadashySankhya = 0;
    $scope.KalamNo6MeDarshayiAntTakHissaRashi = 0;
    $scope.KalamNo6MeDarshayiAntTakkaryshilPunji = 0;

   

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
    $scope.GetDistrictList();
    $scope.OnPageLoad();





    //Madhuri



    $scope.SaveCoopMovRj = function () {

        //$scope.Coop.RJCoopMovId = $scope.dataid;

        //debugger;
        //alert("Hello");
        //alert($scope.Liqudation.LiqudationDataId);
        if ($scope.CoopMovRjFormValidation()) {
          //  alert("Year:" + $("#txtFromMonth").val());
            var data = {


               // RJCoopMovId = $scope.Coop.RJCoopMovId,

                Year: $("#txtFromMonth").val(),
                CooperativeSociety: $scope.Coop.Cosoc,
                Membership:         $scope.Coop.Member,
                ShareCapital:       $scope.Coop.shca,
                WorkingCapital:     $scope.Coop.woca,
                St_Loan_Advancement:$scope.Coop.SLoanAd,
                St_Loan_Recovered:  $scope.Coop.SLoanRe,
                Mt_Loan_Advancement:$scope.Coop.MLoanAd,
                Mt_Loan_Recovered:  $scope.Coop.MLoanRe,
                Lt_Loan_Advancement:$scope.Coop.LLoanAd,
                Lt_Loan_Recovered:  $scope.Coop.LLoanRe,
                Purchase:           $scope.Coop.Pur,
                Sale:               $scope.Coop.Sale
                                                
            }                                   


            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/MIS/SaveCoopMovFormsData", JSON.stringify({ _Model: data })).
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
                        $scope.ClearForm();
                    }
                }).
                error(function (data, status) {
                    alert("Error while Cooperative Movement In Rajasthan Entry Form insert");
                });
        }


    };


    $scope.ClearForm = function () {


                $scope.Coop.Cosoc ="0",
                $scope.Coop.Member = "0",
                $scope.Coop.shca="0",
                $scope.Coop.woca = "0",
                $scope.Coop.SLoanAd = "0",
                $scope.Coop.SLoanRe = "0",
                $scope.Coop.MLoanAd = "0",
                $scope.Coop.MLoanRe = "0",
                $scope.Coop.LLoanAd = "0",
                $scope.Coop.LLoanRe = "0",
                $scope.Coop.Pur = "0",
                $scope.Coop.Sale = "0"
    }

   

    //validateAddForm 
    $scope.CoopMovRjFormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';

       
        if ($("#txtFromMonth").val() == undefined || $("#txtFromMonth").val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Financial Year' : '\n <br /> कृपया Financial Year का चयन करे |';
        }

        if ($scope.Coop.Cosoc == undefined || $scope.Coop.Cosoc == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter No. Of Cooperative Societies' : '\n <br /> कृपया Cooperative Societies दर्ज करें|';
        }
        if ($scope.Coop.Member == undefined || $scope.Coop.Member == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter No. Of Membership' : '\n <br /> कृपया Membership दर्ज करें |';
        }
        if ($scope.Coop.shca == undefined || $scope.Coop.shca == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Share Capital' : '\n <br /> कृपया Share Capital राशि दर्ज करें |';
        }


        if ($scope.Coop.woca == undefined || $scope.Coop.woca == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Working Capital' : '\n <br /> कृपया Working Capital राशि दर्ज करें |';
        }


        if ($scope.Coop.SLoanAd == undefined || $scope.Coop.SLoanAd == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Short Term Loan Advancement' : '\n <br /> कृपया Short Term Loan Advancement दर्ज करें |';
        }

        if ($scope.Coop.SLoanRe == undefined || $scope.Coop.SLoanRe == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Short Term Loan Recovered' : '\n <br /> कृपया Short Term Loan Recovered दर्ज करें |';
        }

        if ($scope.Coop.MLoanAd == undefined || $scope.Coop.MLoanAd == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Middle Term Loan Advancement' : '\n <br /> कृपयाMiddle Term Loan Advancement राशि दर्ज करें |';
        }

        if ($scope.Coop.MLoanRe == undefined || $scope.Coop.MLoanRe == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Middle Term Loan Recovered' : '\n <br /> कृपया  Middle Term Loan Recovered राशि दर्ज करें |';
        }


        if ($scope.Coop.LLoanAd == undefined || $scope.Coop.LLoanAd == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Long Term Loan Advancement' : '\n <br /> कृपया  Long Term Loan Advancement राशि दर्ज करें |';
        }

        if ($scope.Coop.LLoanRe == undefined || $scope.Coop.LLoanRe == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Long Term Loan Recovered' : '\n <br /> कृपया Long Term Loan Recovered राशि दर्ज करें |';
        }

       
        if ($scope.Coop.Pur == undefined || $scope.Coop.Pur == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Purchase Amount ' : '\n <br /> कृपया Purchase  राशि दर्ज करें |';
        }
        if ($scope.Coop.Sale == undefined || $scope.Coop.Sale == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Sale Amount' : '\n <br /> कृपया Sale  राशि दर्ज करें |';
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
        $scope.CoopRjData = [];
        $http.post(SiteUrl + "/MIS/GetCoopMovFormsData?DistrictId=" + 0 + "&Year=" + $("#FY").val()).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
               
                $scope.CoopRjData = data;
               // $scope.CoopRjData = JSON.parse(data.DataList);
            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
            });
        $("#FY").val('');



        //if (txtFromMonth != '')
        //{
        //    $scope.CoopRjData = [];
        //    $http.post(SiteUrl + "/MIS/GetCoopMovFormsData?DistrictId=" + 0 + "&Year=" + $("#txtFromMonth").val()).
        //        success(function (data, status) {
        //            $(".se-pre-con").fadeOut("slow");
        //            $scope.CoopRjData = data;
        //            // $scope.CoopRjData = JSON.parse(data.DataList);
        //        }).
        //        error(function (data, status) {
        //            alert("Error while fetching Year Target Data");
        //        });

        //    $scope.update(data);
        //    $scope.SaveCoopMovRj();
        //}








      

    }


    $scope.update = function (item) {
        //alert(item.LiqudationDataId);
        $scope.showEntryForm = true;
        $scope.DistrictId = item.DistrictId;
        
        Year: $("#txtFromMonth").val(),
        $scope.Coop.Cosoc = item.CooperativeSociety;
        $scope.Coop.Member = item.Membership;
        $scope.Coop.shca=item.ShareCapital;
        $scope.Coop.woca=item.WorkingCapital;
        $scope.Coop.SLoanAd=item.St_Loan_Advancement;
        $scope.Coop.SLoanRe = item.St_Loan_Recovered;
        $scope.Coop.MLoanAd=item.Mt_Loan_Advancement;
        $scope.Coop.MLoanRe=item.Mt_Loan_Recovered;
        $scope.Coop.LLoanAd=item.Lt_Loan_Advancement;
        $scope.Coop.LLoanRe=item.Lt_Loan_Recovered;
        $scope.Coop.Pur=item.Purchase;
        $scope.Coop.Sale=item.Sale



        $scope.Coop.RJCoopMovId = item.RJCoopMovId;

       
    };





    $scope.dataList = 1;

    $scope.GridData();

    $scope.GetFinancialYearList();


});