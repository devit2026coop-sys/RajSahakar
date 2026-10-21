var ArrSelectTraningUser = new Array();
function TraningUserSelected() {
    this.UserId = '0';
}
var AppTrainning = angular.module("AppEntryAuditProgressBlockWiseRpt", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("AuditProgressBlockWiseRptcnt", function ($scope, $http) {    
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 100;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.Trainning = '';
    $scope.showEntryForm = false;

    function FileDataWithRow() {
        this.UserId = '';
    }
    //START : Get District List
    $scope.GetDistrictWithoutDivisionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.BhandarList = data;                
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
    $scope.getdateproper = function () {      
        const monthNames = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        ];        
        const today = new Date($("#txtMonth").val());
        $scope.dateummu = monthNames[today.getMonth()];
        $scope.dateummu = $scope.dateummu + '-' + today.getFullYear();

        let mm = today.getMonth() + 1; // Months start at 0!
        let dd = today.getDate();
        let yyyy = today.getFullYear();
        if (dd < 10) dd = '0' + dd;
        if (mm < 10) mm = '0' + mm;
        $scope.todaydate = dd + '/' + mm + '/' + yyyy;        

    };
    $scope.Id = 0;
    $scope.GridData2 = function () { 
        $(".se-pre-con").fadeIn("slow");
        $scope.urldata = window.location.href;
        $scope.urldata = $scope.urldata.split('/');
        if ($scope.urldata[4] == 'AuditProgressBlockWiseRpt') {
            $scope.Id = 1;
        }
        else {
            $scope.Id = 2;
        }        
        if ($("#txtMonth").val() == '') {
            $scope.AutoselectMonth();
        }

        if ($("#txtMonth").val() != '') {
            $scope.getdateproper();
        }
        $http.post(SiteUrl + "/MIS/GetAuditData1/?Id=" + $scope.Id + "&Month=" + $("#txtMonth").val()).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.AuditData = data;
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
        var formatHColumn = excel.addStyle({ 										
            font: "Calibri 15 #324148"
        });
        excel.set(0, 0, curr, $scope.translation.MonthlyProgressReportofLiquidationFund, formatHColumn);
        if ($('#txtMonth').val() != '' || $('#txtMonth').val() == undefined) {
            ++curr;
            excel.set(0, 0, curr, $scope.translation.Month, formatHColumn);
            excel.set(0, 1, curr, $('#txtMonth').val(), formatHColumn);
        }

        if ($scope.DistrictId != '' || $scope.DistrictId == undefined) {
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
        excel.set(0, 0, curr + 1, $scope.translation.SRNO, formatHeader);
        excel.set(0, 1, curr + 1, $scope.translation.Month, formatHeader);
        excel.set(0, 2, curr + 1, $scope.translation.DetailsOfAcceptatnce, formatHeader);
        excel.set(0, 3, curr + 1, $scope.translation.AmountOfRecovery, formatHeader);
        excel.set(0, 7, curr + 1, $scope.translation.RecoveryBalance, formatHeader);
        excel.set(0, 8, curr + 1, $scope.translation.TotalAmountInFund, formatHeader);
        curr = curr + 1;
        excel.set(0, 4, curr + 1, $scope.translation.Principal, formatHeader);
        excel.set(0, 5, curr + 1, $scope.translation.Interest, formatHeader);
        excel.set(0, 6, curr + 1, $scope.translation.Yog, formatHeader);
        excel.set(0, 8, curr + 1, $scope.translation.SavingAccount, formatHeader);
        excel.set(0, 9, curr + 1, $scope.translation.SavingAccount, formatHeader);
        excel.set(0, 10, curr + 1, $scope.translation.Yog, formatHeader);
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
        curr = curr + 2;
        $scope.LiqudationData.forEach(function (item, index) {
            excel.set(0, 0, index + curr, (index + 1), formatColumn);
            excel.set(0, 1, index + curr, item.Month, formatColumn);
            excel.set(0, 2, index + curr, item.DetailsOfAcceptatnce, formatColumn);
            excel.set(0, 3, index + curr, item.AcceptedAmount, formatColumn);
            excel.set(0, 4, index + curr, item.PrincipalAmountOfRecovery, formatColumn);
            excel.set(0, 5, index + curr, item.InterestAmountOfRecovery, formatColumn);
            excel.set(0, 6, index + curr, item.PITotal, formatColumn);
            excel.set(0, 7, index + curr, item.RecoveryBalance, formatColumn);
            excel.set(0, 8, index + curr, item.FdAmount, formatColumn);
            excel.set(0, 9, index + curr, item.FundSavingAccount, formatColumn);
            excel.set(0, 10, index + curr, item.FSTotal, formatColumn);
        });
        excel.generate($scope.translation.MPR2001MembersShareCapital + ".xlsx");
    }

    $scope.pageChangeHandler = function (num) {

    };
    
    // Update Template  

    $scope.SetStatus = function (ObjObjLiqudation) {        
        $scope.Liqudation = ObjObjLiqudation;
        $scope.Liqudation.IsActive = ObjObjLiqudation.IsActive == 1 ? 0 : 1;        
        $scope.dataid = ObjObjLiqudation.LiqudationDataId;
        $scope.SaveLiquidation();
    };


    $scope.SetStatusTrash = function (ObjObjLiqudation) {        
        $scope.Liqudation = ObjObjLiqudation;        
        $scope.Liqudation.IsActive = 2;      
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
    $scope.DistrictList = [];
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0" && $("#hdnDistrictId").val() != "") {
                    $("#ddrDistrictId").attr("disabled", "disabled");
                    $scope.DistrictId = $("#hdnDistrictId").val();
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

        if ($scope.Id == 2) {          
            $scope.getCurrentFinancialYearPrev();
        }
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
        $("#txtMonth").val($scope.minDate);       

    }
    $scope.GetDistrictList();
    $scope.OnPageLoad();
    $scope.GridData2();
    $scope.DivtringGrid = true;
});

