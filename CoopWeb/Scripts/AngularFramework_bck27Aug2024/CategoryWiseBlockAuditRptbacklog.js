var ArrSelectTraningUser = new Array();

function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("AppEntryFormCooperativeBankMpr", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("CooperativeBankMprcnt", function ($rootScope,$scope, $http) {
    // Save Sms Template 
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 35;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.Trainning = '';


    function FileDataWithRow() {
        this.UserId = '';
    }
    //START : Get District List
    $scope.GetDistrictWithoutDivisionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.BhandarList = data;
                //$scope.BhandarForm.BhandarId = '5';
            }).
            error(function (data, status) {
                $scope.BhandarList = [];
            });
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

    //GET MONTH
    $scope.GetMonthList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MONTH&MasterCode=-1").
            success(function (data, status) {
                $scope.MonthList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
                alert('');
            });
    };

    $scope.GetYearTargetData = function () {
        $http.post(SiteUrl + "/EntryForms/GetYearTargetData", $scope.BhandarForm).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.BhandarForm.ControlledBusinessTarget = data[0].ControlledBusinessTarget;
                    $scope.BhandarForm.UnControlledBusinessTarget = data[0].UnControlledBusinessTarget;
                }
            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
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
        $("#txtFromMonth").val(date);
    };

    $scope.clearAll = function () {

        $scope.TotKVSS = 0;
        $scope.TotCCB = 0;
        $scope.TotDairy = 0;
        $scope.Toturban = 0;

        $scope.TotPLDB = 0;
        $scope.TotHolsale = 0;
        $scope.TotTilamsangh = 0;
        $scope.TotBunkar = 0;
        $scope.TotKhaadi = 0;
        $scope.TotIndustrial = 0;
        $scope.TotGSS = 0;
        $scope.Totmultipurpose = 0;

        $scope.TotKVSS1 = 0;
        $scope.TotCCB1 = 0;
        $scope.TotDairy1 = 0;
        $scope.Toturban1 = 0;

        $scope.TotPLDB1 = 0;
        $scope.TotHolsale1 = 0;
        $scope.TotTilamsangh1 = 0;
        $scope.TotBunkar1 = 0;
        $scope.TotKhaadi1 = 0;
        $scope.TotIndustrial1 = 0;
        $scope.TotGSS1 = 0;
        $scope.Totmultipurpose1 = 0;

        $scope.TotKVSS2 = 0;
        $scope.TotCCB2 = 0;
        $scope.TotDairy2 = 0;
        $scope.Toturban2 = 0;

        $scope.TotPLDB2 = 0;
        $scope.TotHolsale2 = 0;
        $scope.TotTilamsangh2 = 0;
        $scope.TotBunkar2 = 0;
        $scope.TotKhaadi2 = 0;
        $scope.TotIndustrial2 = 0;
        $scope.TotGSS2 = 0;
        $scope.Totmultipurpose2 = 0;

    }

    $scope.getdateproper = function () {
        //
        const monthNames = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        ];

        
        const today = new Date($("#txtFromMonth").val());
        $scope.dateummu = monthNames[today.getMonth()];
        $scope.dateummu = $scope.dateummu + '-' + today.getFullYear();
       

    };
    $scope.getCurrentFinancialYearPrev = function () {

        var fiscalyear = "";
        var today = new Date();

        if ((today.getMonth() + 1) <= 3) {
            fiscalyear = (today.getFullYear() - 1) + "-" + today.getFullYear()
        } else {
            fiscalyear = today.getFullYear() + "-" + (today.getFullYear() + 1)
        }

        const myArray = fiscalyear.split("-");
        fiscalyear = myArray[0];

        $scope.minDate = fiscalyear + '-03';

        $("#txtFromMonth").val($scope.minDate);
        //  alert($("#txtMonth").val($scope.minDate));

    }


    $scope.GridData2 = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.getCurrentFinancialYearPrev();
      //  $('#txtFromMonth').val();
        $http.post(SiteUrl + "/MIS/GetCateWiseReportaudit/", { "Id": 2, "YearMonth": $('#txtFromMonth').val() }).
            success(function (data, status) {
                if (data.Table.length > 0) {
                    if ($("#txtFromMonth").val() != '') {
                        $scope.getdateproper();
                    }
                    $scope.CoopBankMprData = data.Table;
                    $scope.CoopBankMprData1 = data.Table1;
                    $scope.CoopBankMprData2 = data.Table2;
                    $scope.clearAll();
                    //$scope.toatofcoumn = parseInt(($scope.CoopBankMprData[0].KVSS) +($scope.CoopBankMprData[0].CCB) + ($scope.CoopBankMprData[0].Dairy) + ($scope.CoopBankMprData[0].urban) + ($scope.CoopBankMprData[0].PLDB) + ($scope.CoopBankMprData[0].Holsale) + ($scope.CoopBankMprData[0].Tilamsangh) +
                    //    ($scope.CoopBankMprData[0].Bunkar) + ($scope.CoopBankMprData[0].Khaadi) + ($scope.CoopBankMprData[0].Industrial) + ($scope.CoopBankMprData.GSS) + ($scope.CoopBankMprData.multipurpose));

                    //$scope.toatofcoumn1 = parseInt($scope.CoopBankMprData1[0].KVSS + $scope.CoopBankMprData1[0].CCB + $scope.CoopBankMprData1[0].Dairy + $scope.CoopBankMprData1[0].urban + $scope.CoopBankMprData1[0].PLDB + $scope.CoopBankMprData1[0].Holsale + $scope.CoopBankMprData1[0].Tilamsangh +
                    //    $scope.CoopBankMprData1[0].Bunkar + $scope.CoopBankMprData1[0].Khaadi + $scope.CoopBankMprData1[0].Industrial + $scope.CoopBankMprData1[0].GSS + $scope.CoopBankMprData1[0].multipurpose);

                    //$scope.toatofcoumn2 = parseInt( $scope.CoopBankMprData2[0].KVSS + $scope.CoopBankMprData2[0].CCB + $scope.CoopBankMprData2[0].Dairy + $scope.CoopBankMprData2[0].urban + $scope.CoopBankMprData2[0].PLDB + $scope.CoopBankMprData2[0].Holsale + $scope.CoopBankMprData2[0].Tilamsangh +
                    //    $scope.CoopBankMprData2[0].Bunkar + $scope.CoopBankMprData2[0].Khaadi + $scope.CoopBankMprData2[0].Industrial + $scope.CoopBankMprData2[0].GSS + $scope.CoopBankMprData2[0].multipurpose);
                    //$scope.TotKVSS = 0;
                    //$scope.TotCCB = 0;
                    //$scope.TotDairy = 0;
                    //$scope.Toturban = 0;

                    //$scope.TotPLDB = 0;
                    //$scope.TotHolsale = 0;
                    //$scope.TotTilamsangh = 0;
                    //$scope.TotBunkar = 0;
                    //$scope.TotKhaadi = 0;
                    //$scope.TotIndustrial = 0;
                    //$scope.TotGSS = 0;
                    //$scope.Totmultipurpose = 0;

                    //$scope.TotKVSS1 = 0;
                    //$scope.TotCCB1 = 0;
                    //$scope.TotDairy1 = 0;
                    //$scope.Toturban1 = 0;

                    //$scope.TotPLDB1 = 0;
                    //$scope.TotHolsale1 = 0;
                    //$scope.TotTilamsangh1 = 0;
                    //$scope.TotBunkar1 = 0;
                    //$scope.TotKhaadi1 = 0;
                    //$scope.TotIndustrial1 = 0;
                    //$scope.TotGSS1 = 0;
                    //$scope.Totmultipurpose1 = 0;

                    //$scope.TotKVSS2 = 0;
                    //$scope.TotCCB2 = 0;
                    //$scope.TotDairy2 = 0;
                    //$scope.Toturban2 = 0;

                    //$scope.TotPLDB2 = 0;
                    //$scope.TotHolsale2 = 0;
                    //$scope.TotTilamsangh2 = 0;
                    //$scope.TotBunkar2 = 0;
                    //$scope.TotKhaadi2 = 0;
                    //$scope.TotIndustrial2 = 0;
                    //$scope.TotGSS2 = 0;
                    //$scope.Totmultipurpose2 = 0;
                    $scope.CoopBankMprData.forEach(function (item) {
                        $scope.toatofcoumn = item.KVSS + item.CCB + item.Dairy + item.urban + item.PLDB + item.Holsale + item.Tilamsangh + item.Bunkar + item.Khaadi + item.Industrial + item.GSS + item.multipurpose;
                        $scope.TotKVSS = parseInt($scope.TotKVSS) + parseInt(item.KVSS);
                        $scope.TotCCB = parseInt($scope.TotCCB) + parseInt(item.CCB);
                        $scope.TotDairy = parseInt($scope.TotDairy) + parseInt(item.Dairy);
                        $scope.Toturban = parseInt($scope.Toturban) + parseInt(item.urban);

                        $scope.TotPLDB = parseInt($scope.TotPLDB) + parseInt(item.PLDB);

                        $scope.TotHolsale = parseInt($scope.TotHolsale) + parseInt(item.Holsale);
                        $scope.TotTilamsangh = parseInt($scope.TotTilamsangh) + parseInt(item.Tilamsangh);
                        $scope.TotBunkar = parseInt($scope.TotBunkar) + parseInt(item.Bunkar);
                        $scope.TotKhaadi = parseInt($scope.TotKhaadi) + parseInt(item.Khaadi);

                        $scope.TotIndustrial = parseInt($scope.TotIndustrial) + parseInt(item.Industrial);
                        $scope.TotGSS = parseInt($scope.TotGSS) + parseInt(item.GSS);
                        $scope.Totmultipurpose = parseInt($scope.Totmultipurpose) + parseInt(item.multipurpose);
                        $scope.toatofcoumn = $scope.TotKVSS + $scope.TotCCB + $scope.TotDairy + $scope.Toturban + $scope.TotPLDB + $scope.TotHolsale + $scope.TotTilamsangh +
                            $scope.TotBunkar + $scope.TotKhaadi + $scope.TotIndustrial + $scope.TotGSS + $scope.Totmultipurpose;
                      
                    });
                  

                    $scope.CoopBankMprData1.forEach(function (item) {
                        $scope.TotKVSS1 = parseInt($scope.TotKVSS1) + parseInt(item.KVSS);
                        $scope.TotCCB1 = parseInt($scope.TotCCB1) + parseInt(item.CCB);
                        $scope.TotDairy1 = parseInt($scope.TotDairy1) + parseInt(item.Dairy);
                        $scope.Toturban1= parseInt($scope.Toturban1) + parseInt(item.urban);

                        $scope.TotPLDB1 = parseInt($scope.TotPLDB1) + parseInt(item.PLDB);

                        $scope.TotHolsale1 = parseInt($scope.TotHolsale1) + parseInt(item.Holsale);
                        $scope.TotTilamsangh1 = parseInt($scope.TotTilamsangh1) + parseInt(item.Tilamsangh);
                        $scope.TotBunkar1 = parseInt($scope.TotBunkar1) + parseInt(item.Bunkar);
                        $scope.TotKhaadi1 = parseInt($scope.TotKhaadi1) + parseInt(item.Khaadi);

                        $scope.TotIndustrial1 = parseInt($scope.TotIndustrial1) + parseInt(item.Industrial);
                        $scope.TotGSS1 = parseInt($scope.TotGSS1) + parseInt(item.GSS);
                        $scope.Totmultipurpose1 = parseInt($scope.Totmultipurpose1) + parseInt(item.multipurpose);
                        $scope.toatofcoumn1 = $scope.TotKVSS1 + $scope.TotCCB1 + $scope.TotDairy1 + $scope.Toturban1 + $scope.TotPLDB1 + $scope.TotHolsale1 + $scope.TotTilamsangh1 +
                            $scope.TotBunkar1 + $scope.TotKhaadi1 + $scope.TotIndustrial1 + $scope.TotGSS1 + $scope.Totmultipurpose1;

                    });

                    $scope.CoopBankMprData2.forEach(function (item) {
                        $scope.TotKVSS2 = parseInt($scope.TotKVSS2) + parseInt(item.KVSS);
                        $scope.TotCCB2 = parseInt($scope.TotCCB2) + parseInt(item.CCB);
                        $scope.TotDairy2 = parseInt($scope.TotDairy2) + parseInt(item.Dairy);
                        $scope.Toturban2 = parseInt($scope.Toturban2) + parseInt(item.urban);

                        $scope.TotPLDB2 = parseInt($scope.TotPLDB2) + parseInt(item.PLDB);

                        $scope.TotHolsale2= parseInt($scope.TotHolsale2) + parseInt(item.Holsale);
                        $scope.TotTilamsangh2 = parseInt($scope.TotTilamsangh2) + parseInt(item.Tilamsangh);
                        $scope.TotBunkar2 = parseInt($scope.TotBunkar2) + parseInt(item.Bunkar);
                        $scope.TotKhaadi2 = parseInt($scope.TotKhaadi2) + parseInt(item.Khaadi);

                        $scope.TotIndustrial2 = parseInt($scope.TotIndustrial2) + parseInt(item.Industrial);
                        $scope.TotGSS2 = parseInt($scope.TotGSS2) + parseInt(item.GSS);
                        $scope.Totmultipurpose2 = parseInt($scope.Totmultipurpose2) + parseInt(item.multipurpose);
                        $scope.toatofcoumn2 = $scope.TotKVSS2 + $scope.TotCCB2 + $scope.TotDairy2 + $scope.Toturban2 + $scope.TotPLDB2 + $scope.TotHolsale2 + $scope.TotTilamsangh2 +
                            $scope.TotBunkar2 + $scope.TotKhaadi2 + $scope.TotIndustrial2 + $scope.TotGSS2 + $scope.Totmultipurpose2;

                    });
                   
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
            });
    }

    $scope.SaveLoan = function () {
     
        $scope.Loan.LoanDataId = $scope.dataid;
            
        //alert($scope.Liqudation.LiqudationDataId);
        if ($scope.LoanFormValidation()) {
            $http.post(SiteUrl + "/MIS/SaveLoanFormsData", $scope.Loan ).
                success(function (data, status) {
                   // alert("DataValue "+data);

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
                        $scope.ClearLoanForm();
                    }
                }).
                error(function (data, status) {
                    alert("Error while Loan Data insert");
                });
        }

    };
    $scope.pageChangeHandler = function (num) {

    };
    $scope.update = function (item) {
        //alert(item.LiqudationDataId);
        $scope.showtringdiv = true;
        $scope.Loan.IsActive = item.IsActive = (item.IsActive == 1 ? true : false);
        $scope.dataid = item.LiqudationDataId;
        $scope.Loan.NameUnit = item.NameUnit;
        $scope.Loan.DetailsOfAcceptatnce = item.DetailsOfAcceptatnce;
        $scope.Loan.AcceptedAmount = parseInt(item.AcceptedAmount);
        $scope.Loan.PrincipalAmountOfRecovery = parseInt(item.PrincipalAmountOfRecovery);
        $scope.Loan.InterestAmountOfRecovery = parseInt(item.InterestAmountOfRecovery);
        $scope.Loan.RecoveryBalance = parseInt(item.RecoveryBalance);
        $scope.Loan.FdAmount = parseInt(item.FdAmount);
        $scope.Loan.FundSavingAccount = parseInt(item.FundSavingAccount);
                

       };

     


    $scope.ClearLoanForm = function () {
        $scope.dataid ='';
        $scope.Loan.AprStamt = 0;
        $scope.Loan.AprMtamt = 0;
        $scope.Loan.Aprltamt = 0;
        $scope.Loan.RecStamt = 0;
        $scope.Loan.RecMtamt = 0;
        $scope.Loan.Recltamt = 0;

        //$scope.Loan.RecoveryBalance = 0;
       // $scope.Loan.FdAmount = 0;
      //  $scope.Loan.FundSavingAccount = 0;

        

        
    };
    
    // Update Template  

    $scope.SetStatus = function (ObjLoan) {
      //  alert("Status " + ObjObjLiqudation.IsActive);
        $scope.Loan = ObjLoan;
        $scope.Loan.IsActive = ObjLoan.IsActive == 1 ? 0 : 1;
      //  alert("Status change" + ObjObjLiqudation.IsActive);
        $scope.dataid = ObjLoan.LoanDataId;
        $scope.SaveLoan();
    };


    $scope.SetStatusTrash = function (ObjObjLiqudation) {
       // alert("Status " + ObjObjLiqudation.IsActive);
        $scope.Liqudation = ObjObjLiqudation;
       // $scope.Liqudation.IsActive = ObjObjLiqudation.IsActive == 1 ? 0 : 1;
        $scope.Liqudation.IsActive =2;
        //alert("Status change" + ObjObjLiqudation.IsActive);
       // $scope.dataid = ObjObjLiqudation.LiqudationDataId;
        $scope.dataid = ObjObjLiqudation.LiqudationDataId;
        $scope.SaveLiquidation();
    };



    // on page load
    $scope.OnPageLoad = function () {
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
        //$scope.GetDistrictWithoutDivisionList();
        //$scope.GetFinancialYearList();
        //$scope.GetMonthList();
      
    };
    // Language Selector at page load
    $scope.AutoselectMonth();
    $scope.ExportData = function () {

        var excel = $JExcel.new("Calibri light 10 #333333");			// Default font	

        excel.set({ sheet: 0, value: "Data" });

        // We want ROW 3 to be EXTRA TALL


        var curr = 1;

        var formatHColumn = excel.addStyle({ 															// Format for headers
            //border: "thin,thin,thin,thin #333333", 													// 		Border for header
            font: "Calibri 15 #324148"
        });


        excel.set(0, 0, curr, $scope.translation.CooperativeBankMprRpt, formatHColumn);


        if ($('#txtFromMonth').val() != '' || $('#txtFromMonth').val() == undefined) {
            ++curr;
            excel.set(0, 0, curr, $scope.translation.Month, formatHColumn);
            excel.set(0, 1, curr, $('#txtFromMonth').val(), formatHColumn);
        }

        if ($("#ddrDistrictId").val() != '' && $("#ddrDistrictId").val() != '0') {
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
      

             
                

         
        excel.set(0, 0, curr + 1, $scope.translation.SRNO, formatHeader);
        excel.set(0, 1, curr, $scope.translation.Division, formatHeader);
        excel.set(0, 2, curr, $scope.translation.KVSS, formatHeader);
        excel.set(0, 3, curr, $scope.translation.CentralCoopBank, formatHeader);
        excel.set(0, 4, curr, $scope.translation.Dairy, formatHeader);
        excel.set(0, 5, curr, $scope.translation.PLDB, formatHeader);
        excel.set(0, 6, curr, $scope.translation.Khaadi, formatHeader);
        excel.set(0, 7, curr, $scope.translation.Bunkar, formatHeader);
        excel.set(0, 8, curr, $scope.translation.Industrial, formatHeader);

        excel.set(0, 9, curr, $scope.translation.GSS, formatHeader);
        excel.set(0, 10, curr, "Multipurpose", formatHeader);
        excel.set(0, 11, curr, "Total", formatHeader);


        //add under column
        //curr = curr + 1;
        //excel.set(0, 3, curr + 1, $scope.translation.Number, formatHeader);
        //excel.set(0, 4, curr + 1, $scope.translation.Member, formatHeader);

        //excel.set(0, 6, curr + 1, $scope.translation.Number, formatHeader);
        //excel.set(0, 7, curr + 1, $scope.translation.Member, formatHeader);

        //excel.set(0, 9, curr + 1, $scope.translation.Number, formatHeader);
        //excel.set(0, 10, curr + 1, $scope.translation.Member, formatHeader);

        //excel.set(0, 12, curr + 1, $scope.translation.Number, formatHeader);
        //excel.set(0, 13, curr + 1, $scope.translation.Member, formatHeader);


        // 		in color green

        var formatColumn = excel.addStyle({ 															// Format for headers
            border: "thin,thin,thin,thin #333333", 													// 		Border for header
            font: "Calibri 10 #324148"
        });


        curr = curr + 2;
        $scope.CoopBankMprData.forEach(function (item, index) {

            excel.set(0, 0, index + curr, (index + 1), formatColumn);
            excel.set(0, 1, index + curr, item.DIVISION_ENG, formatColumn);
            excel.set(0, 3, index + curr, item.KVSS, formatColumn);
            excel.set(0, 4, index + curr, item.CCB, formatColumn);
            excel.set(0, 6, index + curr, item.Dairy, formatColumn);
            excel.set(0, 7, index + curr, item.PLDB, formatColumn);
            excel.set(0, 9, index + curr, item.Khaadi, formatColumn);
            excel.set(0, 10, index + curr, item.Bunkar, formatColumn);
            excel.set(0, 12, index + curr, item.Industrial, formatColumn);
            excel.set(0, 13, index + curr, item.GSS, formatColumn);
            excel.set(0, 13, index + curr, item.Multipurpose, formatColumn);
            excel.set(0, 13, index + curr, item.Total, formatColumn);
            
            
        });

        curr = curr + $scope.CoopBankMprData.length;
        excel.set(0, 0, curr, "", formatHeader);
        excel.set(0, 1, curr, $scope.translation.District, formatHeader);
        excel.set(0, 2, curr, $scope.RajStateCoopBank, formatHeader);
        excel.set(0, 3, curr, $scope.CentralCoopBank, formatHeader);
        excel.set(0, 4, curr, $scope.PriLandDevBank, formatHeader);
        excel.set(0, 5, curr, $scope.RajStateLandDevBank, formatHeader);


        excel.generate($scope.translation.CooperativeBankMprRpt + ".xlsx");

    };

    $scope.ShowLiqudationContent = function () {
        $scope.showtringdiv = true;
        $scope.ClearLiquidationForm();
        $scope.DivtringGrid = false;
        $scope.asdf = true;

    }

  
    //validateAddForm 
    $scope.LoanFormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';

        if ($scope.Loan.AprStamt == undefined || $scope.Loan.AprStamt == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Short Term Amount in Loan Advancement' : '\n <br /> कृपया ऋण अग्रिम में अल्पावधि राशि दर्ज करें |';
        }
        if ($scope.Loan.AprMtamt == undefined || $scope.Loan.AprMtamt == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Middle Term Amount in Loan Advancement' : '\n <br /> कृपया ऋण अग्रिम में मध्यावधि राशि दर्ज करें |';
        }
        if ($scope.Loan.Aprltamt == undefined || $scope.Loan.Aprltamt == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Long Term Amount in Loan Advancement' : '\n <br /> कृपया ऋण अग्रिम में दीर्घकालीन राशि दर्ज करें |';
        }

        if ($scope.Loan.RecStamt == undefined || $scope.Loan.RecStamt == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Short Term Amount in Loan Advancement' : '\n <br /> कृपया वसूले गए ऋण में v राशि दर्ज करें |';
        }
        if ($scope.Loan.RecMtamt == undefined || $scope.Loan.RecMtamt == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Middle Term Amount in Loan Advancement' : '\n <br /> कृपया वसूले गए ऋण में मध्यावधि राशि दर्ज करें |';
        }
        if ($scope.Loan.Recltamt == undefined || $scope.Loan.Recltamt == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Long Term Amount in Loan Advancement' : '\n <br /> कृपया वसूले गए ऋण में दीर्घकालीन राशि दर्ज करें |';
        }

        if (message.length > 23) {
            ShowModelAlert("Loan Form", message);
            return false;
        }
        return true;

    }
    //End

    $scope.OnPageLoad();
    $scope.GridData2();
    $scope.DivtringGrid = true;
    $scope.GetDistrictList();
});

