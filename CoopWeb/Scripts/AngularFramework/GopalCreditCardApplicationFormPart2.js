var gccApp = angular.module("gccApp", ['angularUtils.directives.dirPagination']);
gccApp.controller("gccCtrl", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.gcc = {};
    $scope.gcc.IsRegisteredBankFirstCharge = "1";
    $scope.gcc.IsPropertyMortgagedEarlier = "0";

    const urlParams = new URLSearchParams(window.location.search);
    $scope.GccIdEnc = urlParams.get('gccEncId');
    $scope.CopyOfLease_doc = '';
    $scope.CopyOfJamabandi_doc = '';
    $scope.OnPageLoad = function () {
        $scope._language = _language;
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
        $scope.GetApplicationDetails();
    };

    function panelDisable(div_name) {
        const div = document.getElementById(div_name);
        const inputs = $('#' + div_name + ' input');
        const select = $('#' + div_name + ' select');
       
        $(inputs).each(function (k, v) {
            $(v).prop("disabled", "disabled");
        });       

        $(select).each(function (k, v) {
            $(v).prop("disabled", "disabled");
        });       

        if ($scope.DocResStatus.indexOf('Doc_Jam-Re') !== -1 || $scope.DocResStatus.indexOf('Doc_Jam-Rj') !== -1 || $scope.DocResStatus.indexOf('Doc_Lease-Rj') !== -1 || $scope.DocResStatus.indexOf('Doc_Lease-Rj') !== -1) {

            
            $(inputs).each(function (k, v) {
                if ($(v).attr('type') === 'file' && ($scope.DocResStatus.toString().indexOf('Doc_Jam-Re') !== -1 || $scope.DocResStatus.indexOf('Doc_Jam-Rj') !== -1)) {
                    if ($(v).attr('onchange').indexOf("CopyOfJamabandi") !== -1) {
                        $(v).prop("disabled", false); // Always enable file inputs
                       // $scope.CopyOfJamabandi_doc = $scope.gcc.CopyOfJamabandi;
                        $scope.gcc.CopyOfJamabandi = '';
                    }
                }
                if ($(v).attr('type') === 'file' && ($scope.DocResStatus.toString().indexOf('Doc_Lease-Rj') !== -1 || $scope.DocResStatus.indexOf('Doc_Lease-Rj') !== -1)) {
                    if ($(v).attr('onchange').indexOf("CopyOfLease") !== -1) {
                        $(v).prop("disabled", false); // Always enable file inputs
                        //$scope.CopyOfLease_doc = $scope.gcc.CopyOfLease;
                        $scope.gcc.CopyOfLease = '';
                        }
                    }

            }); 

        }
    
    }
    $scope.GetApplicationDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/GopalCreditCard/GetApplicationDetail?GccIdEnc=" + $scope.GccIdEnc).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data!=null) {
                    $scope.objGCC = data.Table[0];
                    $scope.StatusId = data.Table[0].StatusId;
                    $scope.gcc.GopalCreditCardIdEnc = $scope.GccIdEnc;
                    $scope.gcc.RegistrationNo = $scope.objGCC.RegistrationNo;
                    if (data.Table1[0] != null)
                        $scope.gcc = data.Table1[0];

                    if (data.Table1[0] != null) {
                        
                        $scope.gcc.TypesOfProperty = data.Table1[0].TypesOfProperty.toString();      
                        $scope.gcc.EarlierLoanSanctionDate = new Date($scope.gcc.EarlierLoanSanctionDate);


                        if ($scope.gcc.TypesOfProperty == "1") {
                            $scope.gcc.CopyOfJamabandi = data.Table1[0].JamabandiDoc;
                            $('#CopyOfJamabandi').val(data.Table1[0].JamabandiDoc);
                            $scope.CopyOfJamabandi_doc = $scope.gcc.CopyOfJamabandi;
                        }

                        $scope.CopyOfLease_doc = $scope.gcc.CopyOfLease;
                        $('#CopyOfLease').val(data.Table1[0].CopyOfLease);

                        if ($scope.StatusId == 13 || $scope.StatusId == 14) {
                            $scope.DocResStatus = data.Table[0].DocResStatus;
                            panelDisable('gccpart2');
                        }
                    }
                   
                   
                }
            }).
            error(function () {
                alert("Error while fetching GCC application data.");
                $(".se-pre-con").fadeOut("slow");
            });
    }
    $scope.BackToPage = function () {
        window.location = '../gopalCreditCard/GopalApplicationForm';
    }
    $scope.SaveGccApplicationPart2 = function (isPermanentSave) {
        if (isPermanentSave == undefined || isPermanentSave == null)
            isPermanentSave = false;
        if (isPermanentSave) {
            if (!confirm('Are you want to final submit your application'))
                return;
        }
        $scope.gcc.JamabandiDoc = $('#CopyOfJamabandi').val();
        $scope.gcc.CopyOfLease = $('#CopyOfLease').val();
        $scope.gcc.IsPermanentSave = isPermanentSave;
        $scope.gcc.GopalCreditCardIdEnc = $scope.GccIdEnc;
        
        if (!$scope.ValidateApplication())
            return;

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/GopalCreditCard/Save_GopalCreditCardFormPart2", $scope.gcc).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            if (data.Status == true) {
                ShowOkMessage("Success", _language == 'en-us' ? 'Data saved successfully\n' : 'डेटा सहेजा गया है\n', function () { $scope.BackToPage(); });
            }
            else if (data.Status == false) {
            }
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
        });
    }

    $scope.ValidateApplication = function () {
        var message = $scope._language == 'en-us' ? 'Validate following :: ' : 'निम्नलिखित मान्य करें :';
        message += '<br />';

        if ($scope.gcc.TypesOfProperty == "" || $scope.gcc.TypesOfProperty == null) {
            message += $scope._language == 'en-us' ? '\nPlease choose type of property.' : '\nकृपया संपत्ति का प्रकार चुनें।';
            message += '<br />';
        }

        if ($scope.gcc.TypesOfProperty == 1) {
            if ($scope.gcc.KhasraNo == "" || $scope.gcc.KhasraNo == null) {
                message += $scope._language == 'en-us' ? '\nPlease enter Khasra No.' : '\nकृपया खसरा नम्बर दर्ज करें।';
                message += '<br />';
            }
            if ($scope.gcc.LandMeasurement == "" || $scope.gcc.LandMeasurement == null) {
                message += $scope._language == 'en-us' ? '\nPlease enter measurement of land.' : '\nकृपया भूमि का माप दर्ज करें।';
                message += '<br />';
            }
            if ($scope.gcc.KhataNo == "" || $scope.gcc.KhataNo == null) {
                message += $scope._language == 'en-us' ? '\nPlease enter khata No.' : '\nकृपया खाता संख्या दर्ज करें।';
                message += '<br />';
            }
            if ($scope.gcc.LandType == "" || $scope.gcc.LandType == null) {
                message += $scope._language == 'en-us' ? '\nPlease enter types of land.' : '\nकृपया भूमि के प्रकार दर्ज करें।';
                message += '<br />';
            }
            if ($scope.gcc.JamabandiDoc == "" || $scope.gcc.JamabandiDoc == null) {
                message += $scope._language == 'en-us' ? '\nPlease upload copy of jamabandi.' : '\nकृपया जमाबंदी की प्रति अपलोड करें।';
                message += '<br />';
            }
        }
       if ($scope.gcc.TypesOfProperty == 2 || $scope.gcc.TypesOfProperty == 3) {
            if ($scope.gcc.ResidentialPlotAddress == "" || $scope.gcc.ResidentialPlotAddress == null) {
                message += $scope._language == 'en-us' ? '\nPlease enter the address of residential plot and residential house.' : '\nकृपया आवासीय भूखण्ड व रहवासी मकान का पता दर्ज करें।';
                message += '<br />';
            }
            if ($scope.gcc.EstimatedPrice == "" || $scope.gcc.EstimatedPrice == null) {
                message += $scope._language == 'en-us' ? '\nPlease estimated price.' : '\nकृपया अनुमानित मूल्य अंकित करें।';
                message += '<br />';
            }
            if ($scope.gcc.CopyOfLease == "" || $scope.gcc.CopyOfLease == null) {
                message += $scope._language == 'en-us' ? '\nPlease upload copy of lease.' : '\nकृपया पट्टे की प्रति अपलोड करें।';
                message += '<br />';
            }
        }
        if ($scope.gcc.LoanAmountRecommendByPACS == "" || $scope.gcc.LoanAmountRecommendByPACS == null) {
            message += $scope._language == 'en-us' ? '\nPlease enter loan amount recommended by PACS.' : '\nकृपया पैक्स मैनेजर द्वारा अनुशंषा की गई ऋण राशि दर्ज करें।';
            message += '<br />';
        }
        if ($scope.gcc.IsPropertyMortgagedEarlier == 1) {
            if ($scope.gcc.EarlierLoanFromBank == "" || $scope.gcc.EarlierLoanFromBank == null) {
                message += $scope._language == 'en-us' ? '\nPlease enter bank name.' : '\nकृपया बैंक का नाम दर्ज करें.।';
                message += '<br />';
            }
            if ($scope.gcc.EarlierLoanAccountNo == "" || $scope.gcc.EarlierLoanAccountNo == null) {
                message += $scope._language == 'en-us' ? '\nPlease enter earlier account number..' : '\nकृपया पूर्व खाता संख्या दर्ज करें।';
                message += '<br />';
            }
            if ($scope.gcc.EarlierLoanAmount == "" || $scope.gcc.EarlierLoanAmount == null) {
                message += $scope._language == 'en-us' ? '\nPlease enter earlier loan amount.' : '\nकृपया पहले ली गई ऋण राशि दर्ज करें।';
                message += '<br />';
            }
            if ($scope.gcc.EarlierLoanSanctionDate == "" || $scope.gcc.EarlierLoanSanctionDate == null) {
                message += $scope._language == 'en-us' ? '\nPlease enter earlier loan sanction date.' : '\nकृपया पूर्व ऋण स्वीकृति तिथि दर्ज करें।';
                message += '<br />';
            }
        }
        if (message.length > 29) {
            ShowModelAlert($scope._language == 'en-us' ? 'Points to ensure loan security from PACS level :: ' : 'पैक्स स्तर से ऋण की सुरक्षा सुनिश्चित किये जाने वाले बिन्दु ::', message);
            return false;
        } else {
            return true;
        }
    }
    $scope.FileUploader = function (file, e) {
        var formData = new FormData();
        formData.append("file", file[0], file[0].name);
        $(".se-pre-con").fadeIn("slow");
        $.ajax({
            type: "POST",
            url: SiteUrl + "/FileUploaders/CommonFileUploader",
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                console.log(data.filePath)
                $('#' + e).val(data.filePath);
            },
            error: function (data, status) {
                console.log(data);
                $(".se-pre-con").fadeOut("slow");
            }
        });
    }
    $scope.OnPageLoad();
});

