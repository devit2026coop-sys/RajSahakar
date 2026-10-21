var ArrSelectTraningUser = new Array();

function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("AppEntryFormLiqudation", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("Liqudationcnt", function ($scope, $http) {
    // Save Sms Template 
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.Trainning = '';

    $scope.Liqudation = {
        DetailsOfAcceptatnce:"",
        AcceptedAmount: "0",
        PrincipalAmountOfRecovery: "0",
        InterestAmountOfRecovery: "0",
        RecoveryBalance: "0",
        FdAmount: "0",
        FundSavingAccount: "0",
    }


    $scope.showEntryForm = false;       

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
    }
    $scope.GridData2 = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.LiqudationData = [];
        $http.post(SiteUrl + "/MIS/GetLiquidationData1?DistrictId=" + $("#ddrDistrictId").val() + "&Month=" + $("#txtMonth").val()).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                
                    $scope.LiqudationData = JSON.parse(data.DataList);
                
            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
            });
    }

    $scope.ExportData = function () {

        var excel = $JExcel.new("Calibri light 10 #333333");			// Default font	

        excel.set({ sheet: 0, value: "Data" });

        // We want ROW 3 to be EXTRA TALL


       

        var curr = 1

        var formatHColumn = excel.addStyle({ 															// Format for headers
            //border: "thin,thin,thin,thin #333333", 													// 		Border for header
            font: "Calibri 15 #324148"
        });


        excel.set(0, 0, curr, $scope.translation.MonthlyProgressReportofLiquidationFund, formatHColumn);


        if ($('#txtMonth').val() != '' || $('#txtMonth').val() == undefined) {
            ++curr;
            excel.set(0, 0, curr, $scope.translation.Month, formatHColumn);
            excel.set(0, 1, curr, $('#txtMonth').val(), formatHColumn);
        }

        if ($scope.DistrictId != '' && $scope.DistrictId != '0') {
            ++curr;
            excel.set(0, 0, curr, $scope.translation.DistrictHeading, formatHColumn);
            excel.set(0, 1, curr, $scope.DistrictList.filter(function (x) { return x.DISTRICT_ID == $scope.DistrictId })[0].DISTRICT_MANGAL, formatHColumn);
        }

        // This array holds the HEADERS text
        var formatHeader = excel.addStyle({ 															// Format for headers
            border: "thin,thin,thin,thin #333333", 													// 		Border for header
            font: "Calibri 12 #fff B",
            background: "#64c0c5",
        }); 														// 		Font for headers
        curr = curr + 1;
        //for (var i = 0; i < headers.length; i++) {
        //    //++curr;// Loop all the haders
        //    excel.set(0, i, curr + 1, headers[i], formatHeader);													// Set CELL with header text, using header format
        //    excel.set(0, i, undefined, "auto");															// Set COLUMN width to auto (according to the standard this is only valid for numeric columns)
        //}


 


        excel.set(0, 0, curr + 1, $scope.translation.SRNO, formatHeader);
        excel.set(0, 1, curr + 1, $scope.translation.DistrictHeading, formatHeader);
        excel.set(0, 2, curr + 1, $scope.translation.Month, formatHeader);
        excel.set(0, 3, curr + 1, $scope.translation.DetailsOfAcceptatnce, formatHeader);
        excel.set(0, 4, curr + 1, $scope.translation.AmountOfRecovery, formatHeader);
        excel.set(0, 8, curr + 1, $scope.translation.RecoveryBalance, formatHeader);
        excel.set(0, 9, curr + 1, $scope.translation.TotalAmountInFund, formatHeader);



  
        curr = curr + 1;
        excel.set(0, 5, curr + 1, $scope.translation.Principal, formatHeader);
        excel.set(0, 6, curr + 1, $scope.translation.Interest, formatHeader);
        excel.set(0, 7, curr + 1, $scope.translation.Yog, formatHeader);
        excel.set(0, 9, curr + 1, $scope.translation.SavingAccount, formatHeader);
        excel.set(0, 10, curr + 1, $scope.translation.SavingAccount, formatHeader);
        excel.set(0, 11, curr + 1, $scope.translation.Yog, formatHeader);
        
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
        excel.set(0, 11, curr + 1, 11, formatColumn);

        curr = curr + 2;
        $scope.LiqudationData.forEach(function (item, index) {
            excel.set(0, 0, index + curr, (index + 1), formatColumn);
            excel.set(0, 1, index + curr, item.DISTRICT_ENG, formatColumn);
            excel.set(0, 2, index + curr, item.Month, formatColumn);
            excel.set(0, 3, index + curr, item.DetailsOfAcceptatnce, formatColumn);
            excel.set(0, 4, index + curr, item.AcceptedAmount, formatColumn);
            excel.set(0, 5, index + curr, item.PrincipalAmountOfRecovery, formatColumn);
            excel.set(0, 6, index + curr, item.InterestAmountOfRecovery, formatColumn);
            excel.set(0, 7, index + curr, item.PITotal, formatColumn);
            excel.set(0, 8, index + curr, item.RecoveryBalance, formatColumn);
            excel.set(0, 9, index + curr, item.FdAmount, formatColumn);
            excel.set(0, 10, index + curr, item.FundSavingAccount, formatColumn);
            excel.set(0, 11, index + curr, item.FSTotal, formatColumn);
        });

        

        excel.generate($scope.translation.MPR2001MembersShareCapital + ".xlsx");

    }



    $scope.SaveLiquidation = function () {
     
        $scope.Liqudation.LiqudationDataId = $scope.dataid;

    
        //alert($scope.Liqudation.LiqudationDataId);
        if ($scope.LiqudationFormValidation()) {
          var data= {
              LiqudationDataId: $scope.Liqudation.LiqudationDataId  ,
              DistrictId: $scope.DistrictId ,
              Month: $("#txtFromMonth").val(),
              DetailsOfAcceptatnce: $scope.Liqudation.DetailsOfAcceptatnce ,
              AcceptedAmount: $scope.Liqudation.AcceptedAmount,
              PrincipalAmountOfRecovery: $scope.Liqudation.PrincipalAmountOfRecovery,
              InterestAmountOfRecovery: $scope.Liqudation.InterestAmountOfRecovery,
              RecoveryBalance: $scope.Liqudation.RecoveryBalance,
              FdAmount: $scope.Liqudation.FdAmount,
              FundSavingAccount: $scope.Liqudation.FundSavingAccount,
            }

            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Society/SaveLiquidationFormsData", JSON.stringify({ _Model: data})).
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
    $scope.pageChangeHandler = function (num) {

    };
    $scope.update = function (item) {
        //alert(item.LiqudationDataId);
        $scope.showEntryForm = true;
        $scope.DistrictId = item.DistrictId; 
        $scope.Liqudation.DetailsOfAcceptatnce = item.DetailsOfAcceptatnce;
        $scope.Liqudation.AcceptedAmount = parseInt(item.AcceptedAmount);
        $scope.Liqudation.PrincipalAmountOfRecovery = parseInt(item.PrincipalAmountOfRecovery);
        $scope.Liqudation.InterestAmountOfRecovery = parseInt(item.InterestAmountOfRecovery);
        $scope.Liqudation.RecoveryBalance = parseInt(item.RecoveryBalance);
        $scope.Liqudation.FdAmount = parseInt(item.FdAmount);
        $scope.Liqudation.FundSavingAccount = parseInt(item.FundSavingAccount);


        var DT = new Date(item.MonthOfEntry);
        var date = "";
        if ((DT.getMonth() + 1) < 10) {
            date = DT.getFullYear() + '-0' + (DT.getMonth() + 1);
        }
        else {
            date = DT.getFullYear() + '-' + (DT.getMonth() + 1);
        }
        $("#txtFromMonth").val(date);

       };

     


    $scope.ClearLiquidationForm = function () {
        $scope.dataid ='';
        $scope.Liqudation.NameUnit = '';
        $scope.Liqudation.NameUnit = '';
        $scope.Liqudation.DetailsOfAcceptatnce = '';
        $scope.Liqudation.AcceptedAmount = 0;
        $scope.Liqudation.PrincipalAmountOfRecovery = 0;
        $scope.Liqudation.InterestAmountOfRecovery = 0;
        $scope.Liqudation.RecoveryBalance = 0;
        $scope.Liqudation.FdAmount = 0;
        $scope.Liqudation.FundSavingAccount = 0;
        

        
    };
    
    // Update Template  

    $scope.SetStatus = function (ObjObjLiqudation) {
      //  alert("Status " + ObjObjLiqudation.IsActive);
        $scope.Liqudation = ObjObjLiqudation;
        $scope.Liqudation.IsActive = ObjObjLiqudation.IsActive == 1 ? 0 : 1;
      //  alert("Status change" + ObjObjLiqudation.IsActive);
        $scope.dataid = ObjObjLiqudation.LiqudationDataId;
        $scope.SaveLiquidation();
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
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
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



    $scope.ShowLiqudationContent = function () {
        $scope.showEntryForm = true;;
        $scope.showtringdiv = true;
        $scope.ClearLiquidationForm();
        $scope.DivtringGrid = false;
        $scope.asdf = true;

    }

    $scope.HideLiqudationContent = function () {
        $scope.showEntryForm = false;      

    }

  
    //validateAddForm 
    $scope.LiqudationFormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';

        if ($scope.DistrictId == undefined || $scope.DistrictId == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select District' : '\n <br /> कृपया नाम इकाई का चयन करे |';
        }

        if ($("#txtFromMonth").val() == undefined || $("#txtFromMonth").val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Month' : '\n <br /> कृपया माह का चयन करे |';
        }

        if ($scope.Liqudation.DetailsOfAcceptatnce == undefined || $scope.Liqudation.DetailsOfAcceptatnce == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Details Of Acceptatnce' : '\n <br /> कृपया स्वीकृति का विवरण का चयन करे |';
        }
        if ($scope.Liqudation.AcceptedAmount == undefined || $scope.Liqudation.AcceptedAmount == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter AcceptedAmount' : '\n <br /> कृपया स्वीकृत राशि दर्ज करें |';
        }
        if ($scope.Liqudation.PrincipalAmountOfRecovery == undefined || $scope.Liqudation.PrincipalAmountOfRecovery == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Principal Amount Of Recovery' : '\n <br /> कृपया मूल वसूली की गई राशि दर्ज करें |';
        }

        if ($scope.Liqudation.InterestAmountOfRecovery == undefined || $scope.Liqudation.InterestAmountOfRecovery == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Interest Amount Of Recovery' : '\n <br /> कृपया ब्याज वसूली की गई राशि दर्ज करें |';
        }

        if ($scope.Liqudation.RecoveryBalance == undefined || $scope.Liqudation.RecoveryBalance == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Recovery Balance ' : '\n <br /> कृपया वसूली  से  शेष राशि दर्ज करें |';
        }
        if ($scope.Liqudation.FdAmount == undefined || $scope.Liqudation.FdAmount == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Fd Amount' : '\n <br /> कृपया एफ.डी फंड में कुल राशि दर्ज करें |';
        }
        if ($scope.Liqudation.FundSavingAccount == undefined || $scope.Liqudation.FundSavingAccount == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Fund Saving Account' : '\n <br /> कृपया बचत खाता फंड में कुल राशि दर्ज करें |';
        }

        if (message.length > 23) {
            ShowModelAlert("Liqquidation Form", message);
            return false;
        }
        return true;

    }
    //End
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
        $("#txtMonth").val(date);
    };
    $scope.AutoselectMonth();
    $scope.GetDistrictList();

    $scope.OnPageLoad();
    $scope.GridData2();
    $scope.DivtringGrid = true;
});

