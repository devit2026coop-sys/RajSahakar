var CoopConsumerMPRRPTModule = angular.module("CoopConsumerMPRRPTMod", ['angularUtils.directives.dirPagination']);
CoopConsumerMPRRPTModule.controller("CoopConsumerMPRRPTCont", function ($rootScope, $scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.OnPageLoad = function () {

        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                    //$scope.GetWomanSavingCreditSociety();
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
    $scope.Showgride = true;
    $scope.SearchText = "";
    $scope.AddNewConsumerMPR = function () {
        $scope.caseformShow = true;
        $scope.Showgride = false;
    }
    $scope.BackToPage = function () {
        $scope.caseformShow = false;
        $scope.Showgride = true;
        $scope.ClearConsumerMPRForm();
    }
    $scope.ClearConsumerMPRForm = function () {
        $scope.ConsumerMPR.OrganizationName = '';
        $scope.ConsumerMPR.OrganizationPhoneNumber = '';
        $scope.ConsumerMPR.ChiefExecutive = '';
        $scope.ConsumerMPR.ChiefExecutiveMobileNo = '';
        $scope.ConsumerMPR.ChairmanName = '';
        $scope.ConsumerMPR.ChairmanNameMobileNo = '';
        $scope.ConsumerMPR.Buissness = '';
        $scope.ConsumerMPR.Target = '0';
        $scope.ConsumerMPR.Achievementtilllastmonth = '0';
        $scope.ConsumerMPR.Achievementlastmonth = '0';
        $scope.ConsumerMPR.TotalAchievement = '';
        $scope.ConsumerMPR.Percentageachievement = '';
        $scope.ConsumerMPR.BusinessParticipationPercentageTotalBusiness = '';
        $scope.ConsumerMPR.totalsalescenter = '';
        $scope.ConsumerMPR.NumberProfitSellingPoints = '';
        $scope.ConsumerMPR.Numberofsalespointsrunninginloss = '';
        $scope.ConsumerMPR.percentagegrossprofitbusiness = '';
        $scope.ConsumerMPR.Grossprofit = '';
        $scope.ConsumerMPR.Totalexpenditure = '';
        $scope.ConsumerMPR.Netprofitloss = '';
    }
    $scope.GetFinancialYear = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.FinancialYearList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
                $scope.FinancialYearList = $scope.FinancialYearList.filter(function (x) { return x.FIN_YEAR_MASTR_ID < 38 || x.FIN_YEAR_MASTR_ID > 47 });
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GetMonth = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.MonthList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MONTH&MasterCode=-1").
            success(function (data, status) {
                $scope.MonthList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.MonthList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.ValidationCase = function () {
        message = "";
        if ($scope.ConsumerMPR.OrganizationName == '' || $scope.ConsumerMPR.OrganizationName == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Name of Organization' : '\nकृपया संगठन का नाम दर्ज करें |';
            message += '<br />';
        }

        if ($scope.ConsumerMPR.OrganizationPhoneNumber == '' || $scope.ConsumerMPR.OrganizationPhoneNumber == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Organization PhoneNumber' : '\nकृपया संगठन का फोन नंबर दर्ज करें |';
            message += '<br />';
        }

        if ($scope.ConsumerMPR.ChiefExecutive == '' || $scope.ConsumerMPR.ChiefExecutive == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Name of Chief Executive' : '\nकृपया मुख्य कार्यकारी का नाम दर्ज करें |';
            message += '<br />';
        }

        if ($scope.ConsumerMPR.ChiefExecutiveMobileNo == '' || $scope.ConsumerMPR.ChiefExecutiveMobileNo == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Chief Executive Mobile Number' : '\nकृपया मुख्य कार्यकारी मोबाइल नंबर दर्ज करें |';
            message += '<br />';
        }

        if ($scope.ConsumerMPR.ChairmanName == '' || $scope.ConsumerMPR.ChairmanName == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Name of Chairman' : '\nकृपया अध्यक्ष का नाम दर्ज करें |';
            message += '<br />';
        }

        if ($scope.ConsumerMPR.ChairmanNameMobileNo == '' || $scope.ConsumerMPR.ChairmanNameMobileNo == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Chairman Mobile Number' : '\nकृपया अध्यक्ष का मोबाइल नंबर दर्ज करें |';
            message += '<br />';
        }

        if ($scope.ConsumerMPR.Buissness == '' || $scope.ConsumerMPR.Buissness == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Name of Buissness' : '\nकृपया व्यवसाय का नाम दर्ज करें |';
            message += '<br />';
        }
        if ($scope.ConsumerMPR.Target == '0' || $scope.ConsumerMPR.Target == 'undefined') {
            message += $scope._language == 'en-us' ? '\nPlease Select Target (Year....)' : '\nकृपया लक्ष्य चुनें (वर्ष ....) |';
            message += '<br />';
        }
        if ($scope.ConsumerMPR.Achievementtilllastmonth == '0' || $scope.ConsumerMPR.Achievementtilllastmonth == 'undefined') {
            message += $scope._language == 'en-us' ? '\nPlease Select Achievement till last month' : '\nकृपया पिछले महीने तक उपलब्धि का चयन करें |';
            message += '<br />';
        }
        if ($scope.ConsumerMPR.Achievementlastmonth == '0' || $scope.ConsumerMPR.Achievementlastmonth == 'undefined') {
            message += $scope._language == 'en-us' ? '\nPlease Select Achievement in last month' : '\nकृपया पिछले महीने की उपलब्धि चुनें |';
            message += '<br />';
        }

        if ($scope.ConsumerMPR.TotalAchievement == '' || $scope.ConsumerMPR.TotalAchievement == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Total Achievement' : '\nकृपया कुल उपलब्धि दर्ज करें |';
            message += '<br />';
        }


        if ($scope.ConsumerMPR.Percentageachievement == '' || $scope.ConsumerMPR.Percentageachievement == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Percentage of achievement' : '\nकृपया उपलब्धि का प्रतिशत दर्ज करें |';
            message += '<br />';
        }
        if ($scope.ConsumerMPR.BusinessParticipationPercentageTotalBusiness == '' || $scope.ConsumerMPR.BusinessParticipationPercentageTotalBusiness == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Business Participation Percentage in Total Business' : '\nकृपया कुल व्यवसाय में व्यावसायिक भागीदारी प्रतिशत दर्ज करें |';
            message += '<br />';
        }
        if ($scope.ConsumerMPR.totalsalescenter == '' || $scope.ConsumerMPR.totalsalescenter == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Total Sales Center' : '\nकृपया कुल बिक्री केंद्र दर्ज करें |';
            message += '<br />';
        }
        if ($scope.ConsumerMPR.NumberProfitSellingPoints == '' || $scope.ConsumerMPR.NumberProfitSellingPoints == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Number of Profit Selling Points' : '\nकृपया लाभ विक्रय बिंदुओं की संख्या दर्ज करें |';
            message += '<br />';
        }
        if ($scope.ConsumerMPR.Numberofsalespointsrunninginloss == '' || $scope.ConsumerMPR.Numberofsalespointsrunninginloss == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Number of sales points running in loss' : '\nकृपया घाटे में चल रहे बिक्री बिंदुओं की संख्या दर्ज करें |';
            message += '<br />';
        }
        if ($scope.ConsumerMPR.percentagegrossprofitbusiness == '' || $scope.ConsumerMPR.percentagegrossprofitbusiness == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Percentage of gross profit in the business' : '\nकृपया व्यवसाय में सकल लाभ का प्रतिशत दर्ज करें |';
            message += '<br />';
        }
        if ($scope.ConsumerMPR.Grossprofit == '' || $scope.ConsumerMPR.Grossprofit == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Gross profit' : '\nकृपया सकल लाभ दर्ज करें |';
            message += '<br />';
        }
        if ($scope.ConsumerMPR.Totalexpenditure == '' || $scope.ConsumerMPR.Totalexpenditure == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Total expenditure' : '\nकृपया कुल व्यय दर्ज करें |';
            message += '<br />';
        }
        if ($scope.ConsumerMPR.Netprofitloss == '' || $scope.ConsumerMPR.Netprofitloss == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Net profit/loss' : '\nकृपया शुद्ध लाभ/हानि दर्ज करें |';
            message += '<br />';
        }
        if (message.length > 5) {
            ShowModelAlert("Consumer MPR Form", message);
            return false;
        }
        return true;
    }
    $scope.SaveConsumerMPRForm = function () {
        if ($scope.ValidationCase()) {
            var formData = new FormData();
            formData.append("organization", $scope.ConsumerMPR.OrganizationName);
            formData.append("organizationphone", $scope.ConsumerMPR.OrganizationPhoneNumber);
            formData.append("chiefexecutive", $scope.ConsumerMPR.ChiefExecutive);
            formData.append("chiefexecutiveMobile", $scope.ConsumerMPR.ChiefExecutiveMobileNo);
            formData.append("chairmanname", $scope.ConsumerMPR.ChairmanName);
            formData.append("chairmanmobile", $scope.ConsumerMPR.ChairmanNameMobileNo);
            formData.append("business", $scope.ConsumerMPR.Buissness);
            formData.append("targetyear", $scope.ConsumerMPR.Target);
            formData.append("achievementtilllastmonth", $scope.ConsumerMPR.Achievementtilllastmonth);
            formData.append("achievementlastmonth", $scope.ConsumerMPR.Achievementlastmonth);
            formData.append("totalachievement", $scope.ConsumerMPR.TotalAchievement);
            formData.append("achievementpercentage", $scope.ConsumerMPR.Percentageachievement);
            formData.append("businessparticipationpercentage", $scope.ConsumerMPR.BusinessParticipationPercentageTotalBusiness);
            formData.append("totalsalescenter", $scope.ConsumerMPR.totalsalescenter);
            formData.append("profitsellingpoints", $scope.ConsumerMPR.NumberProfitSellingPoints);
            formData.append("losssellingpoints", $scope.ConsumerMPR.Numberofsalespointsrunninginloss);
            formData.append("profitpercentage", $scope.ConsumerMPR.percentagegrossprofitbusiness);
            formData.append("grossprofit", $scope.ConsumerMPR.Grossprofit);
            formData.append("totalexpenditure", $scope.ConsumerMPR.Totalexpenditure);
            formData.append("netprofitloss", $scope.ConsumerMPR.Netprofitloss);
            $http.post(SiteUrl + "/MIS/SaveConsumerMPRForm", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    alert("Data Save Successfully");
                    $scope.ClearConsumerMPRForm();
                    $scope.caseformShow = false;
                    $scope.Showgride = true;
                    $scope.ConsumerMPRFormReportList();
                }).
                error(function (data, status) {
                    alert("Error while ConsumerMPRForm insert");
                    $scope.ClearConsumerMPRForm();
                    $scope.caseformShow = false;
                    $scope.Showgride = true;
                });
        }
    };
    $scope.ConsumerMPRFormReportList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.DataList = [];
        $http.get(SiteUrl + "/MIS/GetConsumerMPRFormList").
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.DataList = data;
                }
            }).
            error(function (data, status) {
                $scope.DataList = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    $scope.OnPageLoad();
    $scope.GetFinancialYear();
    $scope.GetMonth();
    $scope.ConsumerMPRFormReportList();
});


