var eSignApp = angular.module("esignApp", ['angularUtils.directives.dirPagination']);
eSignApp.controller("EsignResultController", function ($scope, $http, $window, $timeout) {
    // Save Sms Template 
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.translation = [];
    $scope.esignResult = {};
    $scope.prnNo = '';
    $scope.esignResult = {}

    var urlParams = new URLSearchParams(window.location.search);
    $scope.prnNo = urlParams.get("prnNo");
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json?id=3").
                success(function (data, status) {

                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json?id=3").
                success(function (data, status) {

                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }

        $scope.GetTransationEsignByPrnNo($scope.prnNo);
    };
    $scope.GetTransationEsignByPrnNo = function (prnNo) {
        $(".blockUI").fadeIn("slow");
        $scope.resultData = [];
        $http.post("/eSign/GetLatestTransactionByPRN", { prnNo: prnNo }).success(function (response, status) {
            $(".blockUI").fadeOut("slow");
            if (response.result != null && response.result != "") {
                if (response.result.Status == "200") {
                    $scope.resultData = response.result.Data.result;
                    $scope.resultData = JSON.parse($scope.resultData);
                    $scope.esignResult = $scope.resultData[0];
                    $scope.esignResult.message = $scope.ValidationMessage($scope.esignResult.ResponseCode, $scope.esignResult.ResponseMessage);
                    //$scope.esignResult.message = $scope.esignResult.ResponseCode === 'E_0045' ? "Your name registered in your RajSahakar portal login does not match the name associated with the Aadhaar number you entered. Please ensure that both names are the same.\n\n(राजसहकार पोर्टल लॉगिन में पंजीकृत आपका नाम, आपके द्वारा दर्ज किए गए आधार नंबर से संबद्ध नाम से मेल नहीं खाता है। कृपया सुनिश्चित करें कि दोनों स्थानों पर नाम एक समान हो।)" : $scope.esignResult.ResponseMessage;
                    //$scope.esignResult.message = $scope.esignResult.ResponseCode == 'E_0045'
                    //    ? (_language == 'en-us' ? "Your name registered in your RajSahakar portal login does not match the name associated with the Aadhaar number you entered. Please ensure that both names are the same." :
                    //        "राजसहकार पोर्टल लॉगिन में पंजीकृत आपका नाम, आपके द्वारा दर्ज किए गए आधार नंबर से संबद्ध नाम से मेल नहीं खाता है। कृपया सुनिश्चित करें कि दोनों स्थानों पर नाम एक समान हो।") : $scope.esignResult.ResponseMessage
                    $timeout(function () {
                        $scope.GoToPrevious($scope.esignResult.ReturnPath);
                    }, 10000);

                }
            }
        }).error(function (data, status) {
            $(".blockUI").fadeOut("slow");
            $scope.resultData = []
        });
    };

    $scope.GoToPrevious = function (path) {
        if (path) {
            $window.location.href = path;
        }
    };

    $scope.DownloadSignedPdf = function (path) {
        if (path) {
            $window.open(path, "_blank");
        }
    };
    $scope.ValidationMessage = function (responseCode, responseMessage) {

        if (responseCode === 'E_0045') {
            return "The name in your RajSahkar Portal profile does not match the name associated with the entered Aadhaar number.\n\n" +
                "To proceed, please update your RajSahkar Portal profile so that your profile name exactly matches the name on your Aadhaar.\n\n" +
                "Steps to update your profile name:\n" +
                "1. Log in to the RajSahkar Portal using your SSO credentials.\n" +
                "2. Click Profile under your name in the top-right corner.\n" +
                "3. Select Edit Profile.\n" +
                "4. Update your name to match your Aadhaar.\n" +
                "5. Save the changes and try the e-Sign process again.\n\n" +
                "--------------------------------------------------------\n\n" +
                "आपकी RajSahkar Portal प्रोफ़ाइल में दर्ज नाम, दर्ज किए गए आधार संख्या से जुड़े नाम से मेल नहीं खाता है।\n\n" +
                "कृपया आगे बढ़ने से पहले RajSahkar Portal में अपनी प्रोफ़ाइल का नाम आधार में दर्ज नाम के अनुसार अपडेट करें।\n\n" +
                "प्रोफ़ाइल का नाम अपडेट करने के चरण:\n" +
                "1. अपनी SSO ID के माध्यम से RajSahkar Portal में लॉगिन करें।\n" +
                "2. ऊपर दाईं ओर अपने नाम पर क्लिक करें और Profile विकल्प चुनें।\n" +
                "3. Edit Profile पर क्लिक करें।\n" +
                "4. अपना नाम आधार के अनुसार अपडेट करें।\n" +
                "5. परिवर्तन Save करें और पुनः e-Sign प्रक्रिया का प्रयास करें।";
        }

        if (responseCode === 'ESP-944') {
            return "The e-Sign process was cancelled by the user.\n\n" +
                "Please try the e-Sign process again to complete the document signing.\n\n" +
                "--------------------------------------------------------\n\n" +
                "उपयोगकर्ता द्वारा e-Sign प्रक्रिया रद्द कर दी गई है।\n\n" +
                "दस्तावेज़ पर हस्ताक्षर पूरा करने के लिए कृपया e-Sign प्रक्रिया को पुनः प्रयास करें।";
        }

        if (responseCode === 'ESP-945') {
            return "The eKYC process was terminated by the user after OTP generation.\n\n" +
                "Please try the e-Sign process again to complete the document signing.\n\n" +
                "--------------------------------------------------------\n\n" +
                "उपयोगकर्ता द्वारा OTP जनरेट होने के बाद eKYC प्रक्रिया समाप्त कर दी गई है।\n\n" +
                "दस्तावेज़ पर हस्ताक्षर पूरा करने के लिए कृपया e-Sign प्रक्रिया को पुनः प्रयास करें।";
        }
        if (responseCode === 'ESP-905') {
            return "The document hash was not received because the document path was not received correctly.\n\n" +
                "Please verify the document path and try the e-Sign process again.\n\n" +
                "--------------------------------------------------------\n\n" +
                "दस्तावेज़ का हैश प्राप्त नहीं हुआ क्योंकि दस्तावेज़ का पथ सही तरीके से प्राप्त नहीं हुआ है।\n\n" +
                "कृपया दस्तावेज़ पथ की जाँच करें और e-Sign प्रक्रिया को पुनः प्रयास करें।";
        }
        if (responseCode === 'ESP-999') {
            return "e-Sign failed. Please try again.\n\n" +
                "--------------------------------------------------------\n\n" +
                "e-Sign विफल हो गया है। कृपया पुनः प्रयास करें।";
        } 

        return responseMessage;
    };


    $scope.OnPageLoad();



});

