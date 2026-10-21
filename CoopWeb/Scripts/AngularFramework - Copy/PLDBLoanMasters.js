


var MasterMod = angular.module('PLDBLoanMastersMOD', ['angularUtils.directives.dirPagination']);
MasterMod.controller("PLDBLoanMastersCONT", function ($scope, $http) {


    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;

    $scope.MasterModel = {
        master_id: 0,
        name_en: "",
        name_hi: "",
        is_active: true,
        scheme_id: "0",
        sector_id:"0",
        effective_date: "",
        normal_interest_rate: "",
        penal_interest_rate:""
    }

    $scope.ResetAddNew = function () {
        $scope.MasterModel = {
            master_id: 0,
            name_en: "",
            name_hi: "",
            is_active: true,
            scheme_id: "0",
            sector_id: "0",
            effective_date: "",
            normal_interest_rate: "",
            penal_interest_rate: ""
        }
    }
    $scope.AddNewScheme = function () {
        $("#addModal").modal("show");
    }

    // on page load
    $scope.OnPageLoad = function () {

        //$(".se-pre-con").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get("../Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $scope._language = 'hi-in';
            $http.get("../Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
            // $(".se-pre-con").fadeOut("slow");
        }
        $(".blockUI").fadeIn("slow");
        if ($("#MasterType").val() == "Scheme") {
            $scope.GetSchemeMaster("").then(function () { $(".blockUI").fadeOut("slow"); });
        } else if ($("#MasterType").val() == "Sector") {
            $scope.GetSchemeMaster(" and is_active=1").then(function () {
                $scope.GetSectorMaster("").then(function () { $(".blockUI").fadeOut("slow"); });
            });
        }
        else if ($("#MasterType").val() == "Purpose") {
            $scope.GetSchemeMaster(" and is_active=1").then(function () {
                $scope.GetPurposeMaster("").then(function () { $(".blockUI").fadeOut("slow"); });               
            });
        }

    };

    /*PLDB Loan Scheme Master*/
    $scope.LoanSchemeList = [];
    $scope.GetSchemeMaster = function (WhereCase) {
      
       return $http.post(SiteUrl + "/PLDBLoan/GetSchemeMaster", { "WhereCase": WhereCase }).
          success(function (data, status) {         
                $scope.LoanSchemeList = [];
                $scope.LoanSchemeList = data;
                $scope.TotalRecord = $scope.LoanSchemeList.length;
                //$(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching Society Details.");
                //$(".blockUI").fadeOut("slow");
            });

    }
    $scope.SaveSchemeMasterData = function () {
        if ($scope.ValidateSchemeMaster()) {

            $(".blockUI").fadeIn("slow");


            var formData = {
                Id: $scope.MasterModel.master_id,
                IsActive: $scope.MasterModel.is_active,
                NameEn: $scope.MasterModel.name_en,
                NameHi: $scope.MasterModel.name_hi
            }



            $http.post(SiteUrl + "/PLDBLoan/SchemeMaster", formData).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data.Status == 2) {
                        ShowOkMessage("Warning", $scope._language == 'en-us' ? "Scheme Name Already Exist." : "योजना का नाम पहले से मौजूद है", function () { });
                    }
                    else if (data.Status == 3) {
                        ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    }
                    else {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Scheme Name successfully submited." : "योजना का नाम सफलतापूर्वक सबमिट किया गया", function () {
                            window.location.href = "/pldbloan/schememaster";
                        });
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                });
        }
    }
    $scope.EditScheme = function (index) {
        $scope.ResetAddNew();
        $("#addModal").modal("show");
        $scope.LoanSchemeList.forEach(function (itm, indx) {
            if (index == indx) {
                $scope.MasterModel.master_id = itm.scheme_id;
                $scope.MasterModel.name_en = itm.scheme_name_en;
                $scope.MasterModel.name_hi = itm.scheme_name_hi;
                $scope.MasterModel.is_active = itm.is_active;            
            }
        });        
    }
    $scope.ValidateSchemeMaster = function () {
        $("#name_en").removeClass("notValid");
        $("#name_hi").removeClass("notValid");
        var message = _language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.MasterModel.name_en.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter scheme name in English.' : '\n <br />कृपया योजना का नाम अंग्रेजी में दर्ज करें।';
            $("#name_en").addClass("notValid");
        }
        if ($scope.MasterModel.name_hi.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter scheme name in Hindi.' : '\n <br />कृपया योजना का नाम हिंदी में दर्ज करें।';
            $("#name_hi").addClass("notValid");
        }
        if (message.length > 29) {
            ShowModelAlert("Scheme Master", message);
            return false;
        }
        return true;
    }
    /*End PLDB Loan Scheme Master*/

    /*PLDB Loan Sector Master*/
    $scope.LoanSectorList = [];
    $scope.GetSectorMaster = function (WhereCase) {       
        return $http.post(SiteUrl + "/PLDBLoan/GetSectorMaster", { "WhereCase": WhereCase }).
            success(function (data, status) {
                $scope.LoanSectorList = [];
                $scope.LoanSectorList = data;
                $scope.TotalRecord = $scope.LoanSchemeList.length;             
            }).
            error(function (data, status) {
                alert("Error while fetching Society Details.");
              
            });

    }
    $scope.SaveSectorMasterData = function () {
        if ($scope.ValidateSectorMaster()) {

            $(".blockUI").fadeIn("slow");


            var formData = {
                Id: $scope.MasterModel.master_id,
                IsActive: $scope.MasterModel.is_active,
                NameEn: $scope.MasterModel.name_en,
                NameHi: $scope.MasterModel.name_hi,
                SchemeId: $scope.MasterModel.scheme_id,
            }



            $http.post(SiteUrl + "/PLDBLoan/SectorMaster", formData).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data.Status == 2) {                        
                        ShowOkMessage("Warning", $scope._language == 'en-us' ? "Sector Name Already Exist." : "सेक्टर का नाम पहले से मौजूद है", function () { });
                    }
                    else if (data.Status == 3) {
                       
                        ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    }
                    else {                      
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Sector Name successfully submited." : "सेक्टर का नाम सफलतापूर्वक सबमिट किया गया", function () {
                            window.location.href = "/pldbloan/sectormaster";
                        });
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                });
        }
    }
    $scope.ValidateSectorMaster = function () {
        $("#name_en").removeClass("notValid");
        $("#name_hi").removeClass("notValid");
        $("#scheme_id").removeClass("notValid");
        var message = _language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.MasterModel.scheme_id == "0") {
            message += _language == 'en-us' ? '\n <br />Please select scheme name.' : '\n <br />कृपया योजना का नाम चुनें।';
            $("#scheme_id").addClass("notValid");
        }
        if ($scope.MasterModel.name_en.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter sector name in English.' : '\n <br />कृपया सेक्टर का नाम अंग्रेजी में दर्ज करें।';
            $("#name_en").addClass("notValid");
        }
        if ($scope.MasterModel.name_hi.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter sector name in Hindi.' : '\n <br />कृपया सेक्टर का नाम हिंदी में दर्ज करें।';
            $("#name_hi").addClass("notValid");
        }
       // alert($scope.MasterModel.scheme_id);
     
        if (message.length > 29) {
            ShowModelAlert("Sector Master", message);
            return false;
        }
        return true;
    }
    $scope.EditSector = function (index) {       
        $scope.ResetAddNew();
        $("#addModal").modal("show");
        $scope.LoanSectorList.forEach(function (itm, indx) {
            if (index == indx) {
                $scope.MasterModel.master_id = itm.sector_id;
                $scope.MasterModel.scheme_id = itm.scheme_id.toString();
                $scope.MasterModel.name_en = itm.sector_name_en;
                $scope.MasterModel.name_hi = itm.sector_name_hi;
                $scope.MasterModel.is_active = itm.is_active;
            }
        });
    }
    /*End PLDB Loan Sector Master*/

    /*PLDB Purpose Master*/
    $scope.LoanPurposeList = [];
    $scope.LoadSector = function () {
        $(".blockUI").fadeIn("slow");
        $scope.GetSectorMaster("  and se.is_active=1 and se.scheme_id=" + $scope.MasterModel.scheme_id).then(function () { $(".blockUI").fadeOut("slow"); });
    };
    $scope.GetPurposeMaster = function (WhereCase) {
        return $http.post(SiteUrl + "/PLDBLoan/GetPurposeMaster", { "WhereCase": WhereCase }).
            success(function (data, status) {
                $scope.LoanPurposeList = [];
                $scope.LoanPurposeList = data;
                $scope.TotalRecord = $scope.LoanSchemeList.length;
            }).
            error(function (data, status) {
                alert("Error while fetching Society Details.");

            });

    }
    $scope.SavePurposeMasterData = function () {
        if ($scope.ValidatePurposeMaster()) {

            $(".blockUI").fadeIn("slow");


            var formData = {
                Id: $scope.MasterModel.master_id,
                IsActive: $scope.MasterModel.is_active,
                NameEn: $scope.MasterModel.name_en,
                NameHi: $scope.MasterModel.name_hi,
                SectorId: $scope.MasterModel.sector_id,
                EffectiveDate: $("#EffectiveDate").val().split('/')[2] + "/" + $("#EffectiveDate").val().split('/')[1] + '/' + $("#EffectiveDate").val().split('/')[0],
                NormalInterestRate: $scope.MasterModel.normal_interest_rate,
                PenalInterestRate: $scope.MasterModel.penal_interest_rate
            }

         

            $http.post(SiteUrl + "/PLDBLoan/PurposeMaster", formData).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data.Status == 2) {
                        ShowOkMessage("Warning", $scope._language == 'en-us' ? "Purpose Name Already Exist." : "उद्देश्य का नाम पहले से मौजूद है", function () { });
                    }
                    else if (data.Status == 3) {

                        ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    }
                    else {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Purpose Name successfully submited." : "उद्देश्य का नाम सफलतापूर्वक सबमिट किया गया", function () {
                            window.location.href = "/pldbloan/purposemaster";
                        });
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                });
        }
    }
    $scope.ValidatePurposeMaster = function () {
        $("#name_en").removeClass("notValid");
        $("#name_hi").removeClass("notValid");
        $("#scheme_id").removeClass("notValid");
        $("#sector_id").removeClass("notValid");
        $("#EffectiveDate").removeClass("notValid");
        $("#normal_interest_rate").removeClass("notValid");
        $("#penal_interest_rate").removeClass("notValid");
        var message = _language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.MasterModel.scheme_id == "0") {
            message += _language == 'en-us' ? '\n <br />Please select scheme name.' : '\n <br />कृपया योजना का नाम चुनें।';
            $("#scheme_id").addClass("notValid");
        }

        if ($scope.MasterModel.sector_id == "0") {
            message += _language == 'en-us' ? '\n <br />Please select sector name.' : '\n <br />कृपया सेक्टर का नाम चुनें।';
            $("#sector_id").addClass("notValid");
        }

        if ($scope.MasterModel.name_en.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter Purpose name in English.' : '\n <br />कृपया उद्देश्य का नाम अंग्रेजी में दर्ज करें।';
            $("#name_en").addClass("notValid");
        }
        if ($scope.MasterModel.name_hi.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter sector name in Hindi.' : '\n <br />कृपया उद्देश्य का नाम हिंदी में दर्ज करें।';
            $("#name_hi").addClass("notValid");
        }

        if ($("#EffectiveDate").val().replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please select effective date.' : '\n <br />कृपया प्रभावी होने की तिथि चुनें।।';
            $("#EffectiveDate").addClass("notValid");
        }
        if ($scope.MasterModel.normal_interest_rate == "") {
            message += _language == 'en-us' ? '\n <br />Please enter normal interest rate.' : '\n <br />कृपया सामान्य ब्याज दर दर्ज करें।';
            $("#normal_interest_rate").addClass("notValid");
        }
        if ($scope.MasterModel.penal_interest_rate == "") {
            message += _language == 'en-us' ? '\n <br />Please enter penal interest rate.' : '\n <br />कृपया दंड ब्याज दर दर्ज करें।';
            $("#penal_interest_rate").addClass("notValid");
        }

        // alert($scope.MasterModel.scheme_id);

        if (message.length > 29) {
            ShowModelAlert("Sector Master", message);
            return false;
        }
        return true;
    }
    $scope.EditPurpose = function (index) {
        $scope.ResetAddNew();
        $("#addModal").modal("show");
        $scope.LoanPurposeList.forEach(function (itm, indx) {
            if (index == indx) {
                $scope.MasterModel.master_id = itm.purpose_id;
           
                $scope.MasterModel.sector_id = itm.sector_id.toString();
                $scope.MasterModel.name_en = itm.purpose_name_en;
                $scope.MasterModel.name_hi = itm.purpose_name_hi;
                $("#EffectiveDate").val(itm.EffectiveDate);
                $scope.MasterModel.effective_date = itm.EffectiveDate;
                $scope.MasterModel.normal_interest_rate = itm.normal_interest_rate;
                $scope.MasterModel.penal_interest_rate = itm.penal_interest_rate;
                $scope.MasterModel.is_active = itm.is_active;

                $(".blockUI").fadeIn("slow");
                $scope.GetSectorMaster("  and se.is_active=1 and se.scheme_id=" + itm.scheme_id).then(function () {
                    $scope.MasterModel.scheme_id = itm.scheme_id.toString();
                    $(".blockUI").fadeOut("slow");
                });
            }
            

        });
    }
    /*End PLDB Purpose Master*/
    $scope.OnPageLoad();

});






