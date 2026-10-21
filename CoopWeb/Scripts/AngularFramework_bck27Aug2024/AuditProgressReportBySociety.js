
var ArrSelectTraningUser = new Array();

function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("AuditProgressReportBySociety", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("AuditProgressReportBySocietycnt", function ($scope, $http) {
    // Save Sms Template 
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.Trainning = '';
    $scope.monthD = '';

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


    $scope.dateummu = '';

    $scope.getdateproper = function () {
        //debugger;
        const monthNames = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        ];

       // alert($("#txtFromMonth").val());
        const today = new Date($("#txtFromMonth").val());
        $scope.dateummu = monthNames[today.getMonth()];
        $scope.dateummu = $scope.dateummu + '-' + today.getFullYear();

        let mm = today.getMonth() + 1; // Months start at 0!
        let dd = today.getDate();
        let yyyy = today.getFullYear();

        if (dd < 10) dd = '0' + dd;
        if (mm < 10) mm = '0' + mm;
        $scope.todaydate = dd + '/' + mm + '/' + yyyy;
        // alert($scope.todaydate);

    };

    //GET Financial Year
    $scope.GetFinancialYearList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;               
                $scope.GridData2();
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
        $scope.DataList = [];
        
        $http.post(SiteUrl + "/MIS/AuditProgressReportBySociety?year=" + '' + "&date=" + $("#txtFromMonth").val()).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");

                if ($("#txtFromMonth").val() != '')
                    $scope.getdateproper();
                

                if (data.length > 0) {
                    $scope.DataList = data;
                    $scope.DataList.forEach(function (x) {
                        x.TotalDepAuditor = parseInt(x.DepAuditor) + parseInt(x.CAAuditor);
                        x.TotalAuditComplete = parseInt(x.DepAuditComplete) + parseInt(x.CAAuditComplete);
                       // x.TotalAuditNotComplete = parseInt(x.DepAuditNotComplete) + parseInt(x.CAAuditNotComplete);
                    });
                }
            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
            });
    }

    $scope.SaveLoan = function () {

        $scope.Loan.LoanDataId = $scope.dataid;

        //alert($scope.Liqudation.LiqudationDataId);
        if ($scope.LoanFormValidation()) {
            $http.post(SiteUrl + "/MIS/SaveLoanFormsData", $scope.Loan).
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

    $scope.ClearLoanForm = function () {
        $scope.dataid = '';
        $scope.Loan.AprStamt = 0;
        $scope.Loan.AprMtamt = 0;
        $scope.Loan.Aprltamt = 0;
        $scope.Loan.RecStamt = 0;
        $scope.Loan.RecMtamt = 0;
        $scope.Loan.Recltamt = 0;



    };




    $scope.SetStatusTrash = function (ObjObjLiqudation) {
        // alert("Status " + ObjObjLiqudation.IsActive);
        $scope.Liqudation = ObjObjLiqudation;
        // $scope.Liqudation.IsActive = ObjObjLiqudation.IsActive == 1 ? 0 : 1;
        $scope.Liqudation.IsActive = 2;
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
        $scope.AutoselectMonth();
        $scope.GridData2();
      

    };
    // Language Selector at page load



    $scope.ShowLoanAdvancementContent = function () {
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
     //   alert($("#txtFromMonth").val());

    };
   // $scope.AutoselectMonth();

   

    $scope.OnPageLoad();

    $scope.DivtringGrid = true;
});

