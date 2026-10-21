var societyDistrictwise = angular.module("societyDistrictwisemodule", ['angularUtils.directives.dirPagination']);
societyDistrictwise.controller("societyDistrictwiseCount", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.pageSize = 100;
    $scope.DistrictId = 0;

    $scope.Coop = {
        DataId: "0",
        DistrictId: "0",
        Year: "",
        Membership_SocietyNo : "0",
                Membership_TotalAm  :"0",
                Membership_ScNO :"0",
                Membership_ScAmount :"0",
                Membership_StNo :"0",
                Membership_StAmount :"0",
                BorrowingMember_SocietyNo :"0",
                BorrowingMember_TotalAm :"0",
                BorrowingMember_ScNO :"0",
                BorrowingMember_ScAmount :"0",
                BorrowingMember_StNo :"0",
                BorrowingMember_StAmount :"0",               
                MemberaIndebted_SocietyNo :"0",
                MemberaIndebted_TotalAm :"0",
                MemberaIndebted_ScNO :"0",
                MemberaIndebted_ScAmount :"0",
                MemberaIndebted_StNo :"0",
                MemberaIndebted_StAmount :"0",
                MemberaDefaulting_SocietyNo :"0",
                MemberaDefaulting_TotalAm :"0",
                MemberaDefaulting_ScNO :"0",
                MemberaDefaulting_ScAmount :"0",
                MemberaDefaulting_StNo :"0",
                MemberaDefaulting_StAmount :"0",
                St_Loan_Advancement_SocietyNo : "0",
                St_Loan_Advancement_TotalAm : "0",
                St_Loan_Advancement_ScNO : "0",
                St_Loan_Advancement_ScAmount : "0",
                St_Loan_Advancement_StNo : "0",
                St_Loan_Advancement_StAmount : "0",
                Mt_Loan_Advancement_SocietyNo : "0",
                Mt_Loan_Advancement_TotalAm : "0",
                Mt_Loan_Advancement_ScNO : "0",
                Mt_Loan_Advancement_ScAmount : "0",
                Mt_Loan_Advancement_StNo : "0",
                Mt_Loan_Advancement_StAmount : "0",
                St_Loan_Recovered_SocietyNo : "0",
                St_Loan_Recovered_TotalAm : "0",
                St_Loan_Recovered_ScNO: "0",
                St_Loan_Recovered_ScAmount : "0",
                St_Loan_Recovered_StNo : "0",
                St_Loan_Recovered_StAmount : "0",
                Mt_Loan_Recovered_SocietyNo : "0",
                Mt_Loan_Recovered_TotalAm : "0",
                Mt_Loan_Recovered_ScNO : "0",
                Mt_Loan_Recovered_ScAmount : "0",
                Mt_Loan_Recovered_StNo : "0",
                Mt_Loan_Recovered_StAmount : "0",
                St_Loan_Outstanding_SocietyNo : "0",
                St_Loan_Outstanding_TotalAm : "0",
                St_Loan_Outstanding_ScNO : "0",
                St_Loan_Outstanding_ScAmount : "0",
                St_Loan_Outstanding_StNo : "0",
                St_Loan_Outstanding_StAmount : "0",
                MT_Loan_Outstanding_SocietyNo :"0",
                MT_Loan_Outstanding_TotalAm :"0",
                MT_Loan_Outstanding_ScNO :"0",
                MT_Loan_Outstanding_ScAmount :"0",
                MT_Loan_Outstanding_StNo :"0",
                MT_Loan_Outstanding_StAmount :"0",
                St_Loan_OverDue_SocietyNo :"0",
                St_Loan_OverDue_TotalAm :"0",
                St_Loan_OverDue_ScNO :"0",
                St_Loan_OverDue_ScAmount :"0",
                St_Loan_OverDue_StNo :"0",
                St_Loan_OverDue_StAmount :"0",
                Mt_Loan_OverDue_SocietyNo :"0",
                Mt_Loan_OverDue_TotalAm :"0",
                Mt_Loan_OverDue_ScNO :"0",
                Mt_Loan_OverDue_ScAmount :"0",
                Mt_Loan_OverDue_StNo :"0",
                Mt_Loan_OverDue_StAmount :"0"    


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
        $scope.getdetails();
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

    };

    $scope.MainCooperativeSocieties = function () {       
       
        if (!$scope.MainCooperativeSocietiesValidation()) {
            return false;
        }
        else {
        
           var formData = new FormData();
            formData.append("Fyear", $scope.Fyear);     
       formData.append("Membership_SocietyNo", $scope.Coop.MASTN);
        formData.append("Membership_TotalAm", $scope.Coop.MATA);
        formData.append("Membership_ScNO", $scope.Coop.MAScN);
        formData.append("Membership_ScAmount", $scope.Coop.MASR);
        formData.append("Membership_StNo", $scope.Coop.MAStN1);
        formData.append("Membership_StAmount", $scope.Coop.MAStR);
        formData.append("BorrowingMember_SocietyNo", $scope.Coop.BoSTN);
        formData.append("BorrowingMember_TotalAm", $scope.Coop.BoTA);
        formData.append("BorrowingMember_ScNO", $scope.Coop.BoScN);
        formData.append("BorrowingMember_ScAmount", $scope.Coop.BoSR);
        formData.append("BorrowingMember_StNo", $scope.Coop.BoStN1);
        formData.append("BorrowingMember_StAmount", $scope.Coop.BoStR);
        formData.append("MemberaIndebted_SocietyNo", $scope.Coop.InSTN);
        formData.append("MemberaIndebted_TotalAm", $scope.Coop.InTA);
        formData.append("MemberaIndebted_ScNO", $scope.Coop.InScN);
        formData.append("MemberaIndebted_ScAmount", $scope.Coop.InSR);
        formData.append("MemberaIndebted_StNo", $scope.Coop.InStN1);
        formData.append("MemberaIndebted_StAmount", $scope.Coop.InStR);
        formData.append("MemberaDefaulting_SocietyNo", $scope.Coop.DeSTN);
        formData.append("MemberaDefaulting_TotalAm", $scope.Coop.DeTA);
        formData.append("MemberaDefaulting_ScNO", $scope.Coop.DeScN);
        formData.append("MemberaDefaulting_ScAmount", $scope.Coop.DeSR);
        formData.append("MemberaDefaulting_StNo", $scope.Coop.DeStN1);
        formData.append("MemberaDefaulting_StAmount", $scope.Coop.DeStR);
        formData.append("St_Loan_Advancement_SocietyNo", $scope.Coop.LsSTN);
        formData.append("St_Loan_Advancement_TotalAm", $scope.Coop.LsTA);
        formData.append("St_Loan_Advancement_ScNO", $scope.Coop.LsScN);
        formData.append("St_Loan_Advancement_ScAmount", $scope.Coop.LsSR);
        formData.append("St_Loan_Advancement_StNo", $scope.Coop.LsStN1);
        formData.append("St_Loan_Advancement_StAmount", $scope.Coop.LsStR);
        formData.append("Mt_Loan_Advancement_SocietyNo", $scope.Coop.LmSTN);
        formData.append("Mt_Loan_Advancement_TotalAm", $scope.Coop.LmTA);
        formData.append("Mt_Loan_Advancement_ScNO", $scope.Coop.LmScN);
        formData.append("Mt_Loan_Advancement_ScAmount", $scope.Coop.LmSR);
        formData.append("Mt_Loan_Advancement_StNo", $scope.Coop.LmStN1);
        formData.append("Mt_Loan_Advancement_StAmount", $scope.Coop.LmStR);
        formData.append("St_Loan_Recovered_SocietyNo", $scope.Coop.RsSTN);
        formData.append("St_Loan_Recovered_TotalAm", $scope.Coop.RsTA);
        formData.append("St_Loan_Recovered_ScNO", $scope.Coop.RsScN);
        formData.append("St_Loan_Recovered_ScAmount", $scope.Coop.RsSR);
        formData.append("St_Loan_Recovered_StNo", $scope.Coop.RsStN1);
        formData.append("St_Loan_Recovered_StAmount", $scope.Coop.RsStR);
        formData.append("Mt_Loan_Recovered_SocietyNo", $scope.Coop.RmSTN);
        formData.append("Mt_Loan_Recovered_TotalAm", $scope.Coop.RmTA);
        formData.append("Mt_Loan_Recovered_ScNO", $scope.Coop.RmScN);
        formData.append("Mt_Loan_Recovered_ScAmount", $scope.Coop.RmSR);
        formData.append("Mt_Loan_Recovered_StNo", $scope.Coop.RmStN1);
        formData.append("Mt_Loan_Recovered_StAmount", $scope.Coop.RmStR);
        formData.append("St_Loan_Outstanding_SocietyNo", $scope.Coop.LOSSTN);
        formData.append("St_Loan_Outstanding_TotalAm", $scope.Coop.LOSTA);
            formData.append("St_Loan_Outstanding_ScNO", $scope.Coop.LOSScN);   
            formData.append("St_Loan_Outstanding_ScAmount", $('#LsScRsSc').val());
            formData.append("St_Loan_Outstanding_StNo", $scope.Coop.LOSStN1);
            formData.append("St_Loan_Outstanding_StAmount", $('#LsStRsSt').val());
        formData.append("MT_Loan_Outstanding_SocietyNo", $scope.Coop.LOMSTN);
        formData.append("MT_Loan_Outstanding_TotalAm", $scope.Coop.LOMTA);
            formData.append("MT_Loan_Outstanding_ScNO", $scope.Coop.LOMScN);
            formData.append("MT_Loan_Outstanding_ScAmount", $('#LmScRmSc').val());
            formData.append("MT_Loan_Outstanding_StNo", $scope.Coop.LOMStN1);
            formData.append("MT_Loan_Outstanding_StAmount", $('#LmStRmSt').val());
        formData.append("St_Loan_OverDue_SocietyNo", $scope.Coop.OsSTN);
        formData.append("St_Loan_OverDue_TotalAm", $scope.Coop.OsTA);
        formData.append("St_Loan_OverDue_ScNO", $scope.Coop.OsScN);
        formData.append("St_Loan_OverDue_ScAmount", $scope.Coop.OsSR);
        formData.append("St_Loan_OverDue_StNo", $scope.Coop.OsStN1);
        formData.append("St_Loan_OverDue_StAmount", $scope.Coop.OsStR);
        formData.append("Mt_Loan_OverDue_SocietyNo", $scope.Coop.OmSTN);
        formData.append("Mt_Loan_OverDue_TotalAm", $scope.Coop.OmTA);
        formData.append("Mt_Loan_OverDue_ScNO", $scope.Coop.OmScN);
        formData.append("Mt_Loan_OverDue_ScAmount", $scope.Coop.OmSR);
            formData.append("Mt_Loan_OverDue_StNo", $scope.Coop.OmStN1);          
        formData.append("Mt_Loan_OverDue_StAmount", $scope.Coop.OmStR);
        $http.post(SiteUrl + "/MIS/SavePrimaryAgriSocieties", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
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
    $scope.MainCooperativeSocietiesValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';
        if ($("#Fyear").val() == undefined || $("#Fyear").val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Financial Year' : '\n <br /> कृपया Financial Year का चयन करे |';
        }

        if ($scope.Coop.BoSTN == undefined || $scope.Coop.BoTA == '0' || $scope.Coop.BoTA == undefined ||
            $scope.Coop.BoScN == undefined || $scope.Coop.BoSR == '0' || $scope.Coop.BoSR == undefined ||
            $scope.Coop.BoStN1 == undefined || $scope.Coop.BoStR == '0' || $scope.Coop.BoStR == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Fill All Entry of Borrowing Members ' : '\n <br /> कृपया Borrowing Members के लिए सभी फ़ील्ड दर्ज करें  दर्ज करें|';
        }

        if ($scope.Coop.InSTN == undefined || $scope.Coop.InTA == '0' || $scope.Coop.InTA == undefined ||
            $scope.Coop.InScN == undefined || $scope.Coop.InSR == '0' || $scope.Coop.InSR == undefined ||
            $scope.Coop.InStN1 == undefined || $scope.Coop.In_StR == '0' || $scope.Coop.InStR == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Fill All Entry of Members indebted at the end of Year ' : '\n <br /> कृपया  Members indebted at the end of Year के लिए सभी फ़ील्ड दर्ज करें  दर्ज करें|';
        }
        if ($scope.Coop.DeSTN == undefined || $scope.Coop.DeTA == '0' || $scope.Coop.DeTA == undefined ||
            $scope.Coop.DeScN == undefined || $scope.Coop.DeSR == '0' || $scope.Coop.DeSR == undefined ||
            $scope.Coop.DeStN1 == undefined || $scope.Coop.DeStR == '0' || $scope.Coop.DeStR == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Fill All Entry of Members defaulting at the end of the year' : '\n <br /> कृपया  Members defaulting at the end of the year के लिए सभी फ़ील्ड दर्ज करें  दर्ज करें|';
        }


        if ($scope.Coop.LsSTN == undefined || $scope.Coop.LsTA == '0' || $scope.Coop.LsTA == undefined ||
            $scope.Coop.LsScN == undefined || $scope.Coop.LsSR == '0' || $scope.Coop.LsSR == undefined ||
            $scope.Coop.LsStN1 == undefined || $scope.Coop.LsStR == '0' || $scope.Coop.LsStR == undefined ||
            $scope.Coop.LmSTN == undefined || $scope.Coop.LmTA == '0' || $scope.Coop.LmTA == undefined ||
            $scope.Coop.LmScN == undefined || $scope.Coop.LmSR == '0' || $scope.Coop.LmSR == undefined ||
            $scope.Coop.LmStN1 == undefined || $scope.Coop.LmStR == '0' || $scope.Coop.LmStR == undefined

        ) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Fill All Entry For Loan Advanced' : '\n <br /> कृपया  Loan Advanced के लिए सभी फ़ील्ड दर्ज करें  दर्ज करें|';
        }


        if ($scope.Coop.RsSTN == undefined || $scope.Coop.RsTA == '0' || $scope.Coop.RsTA == undefined ||
            $scope.Coop.RsScN == undefined || $scope.Coop.RsSR == '0' || $scope.Coop.RsSR == undefined ||
            $scope.Coop.RsStN1 == undefined || $scope.Coop.RsStR == '0' || $scope.Coop.RsStR == undefined ||
            $scope.Coop.RmSTN == undefined || $scope.Coop.RmTA == '0' || $scope.Coop.RmTA == undefined ||
            $scope.Coop.RmScN == undefined || $scope.Coop.RmSR == '0' || $scope.Coop.RmSR == undefined ||
            $scope.Coop.RmStN1 == undefined || $scope.Coop.RmStR == '0' || $scope.Coop.RmStR == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Fill All Entry For Loan Recovered' : '\n <br /> कृपया  Loan Recovered के लिए सभी फ़ील्ड दर्ज करें  दर्ज करें|';
        }


        if ($scope.Coop.LOSSTN == undefined || $scope.Coop.LOSTA == '0' || $scope.Coop.LOSTA == undefined ||
            $scope.Coop.LOSScN == undefined || $scope.Coop.LOMTA == '0' || $scope.Coop.LOMTA == undefined ||
            $scope.Coop.LOSStN1 == undefined || $scope.Coop.LOMSTN == undefined || $scope.Coop.LOMScN == undefined)
             {
            message += $scope._language == 'en-us' ? '\n <br /> Please Fill All Entry For Loan Over Due' : '\n <br /> कृपया  Loan Over Due के लिए सभी फ़ील्ड दर्ज करें  दर्ज करें|';
        }

        if ($scope.Coop.OsSTN == undefined || $scope.Coop.OsTA == '0' || $scope.Coop.OsTA == undefined ||
            $scope.Coop.OmSTN == undefined || $scope.Coop.OmTA == '0' || $scope.Coop.OmTA == undefined ||
            $scope.Coop.OsScN == undefined || $scope.Coop.OsSR == '0' || $scope.Coop.OsSR == undefined ||
            $scope.Coop.OmScN == undefined || $scope.Coop.OmSR == '0' || $scope.Coop.OmSR == undefined ||
            $scope.Coop.OsStN1 == undefined || $scope.Coop.OsStR == '0' || $scope.Coop.OsStR == undefined ||
            $scope.Coop.OmStN1 == undefined || $scope.Coop.OmStR == '0' || $scope.Coop.OmStR == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Fill Loan Over Due Rs.' : '\n <br /> कृपया Loan Over Due Rs.  के लिए सभी फ़ील्ड दर्ज करें  दर्ज करें|';
        }

        if (message.length > 23) {
            ShowModelAlert("Cooperative Movement In Rajasthan Form", message);
            return false;
        }
        return true;

    };

    $scope.showEntryForm = function () {

        $scope.EntryForm = 1;
        $scope.dataList = 0;

    }
    $scope.back = function () {
        $scope.EntryForm = 0;
        $scope.dataList = 1;

    }
    $scope.getdetails = function () {
        $(".se-pre-con").fadeIn("slow");
        
        $scope.CoopMain = [];
        $http.post(SiteUrl + "/MIS/GetPrimaryAgriSocieties?DistrictId=" + 0 + "&Year=" + $("#FY").val()).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.CoopMain = data;             
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

    $scope.getdetails();
    
    $scope.OnPageLoad();
});