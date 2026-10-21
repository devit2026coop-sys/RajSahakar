var pldbApp = angular.module("pldbNewApp", ['angularUtils.directives.dirPagination']);
pldbApp.controller("pldbNewCtrl", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.PaymentModeList = [];

    const urlParams = new URLSearchParams(window.location.search);
    $scope.otsIdEnc = urlParams.get('otsEncId');
    //$scope.otsIdEnc = '0x020000001BAC96EAD32DD2F2D3E1BFB470447CB87A7B597BAC6E278D926C454B3240DB1C';
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


    $scope.GetApplicationDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/PLDB/GetOtsApplicantDetails?otsIdEnc=" + $scope.otsIdEnc + "&summaryId=0" + "&type=1").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data != null) {
                    $scope.ApplicantDetails = JSON.parse(data.Result.Data.ApplicantDetail)[0];

                }
            }).
            error(function () {
                alert("Error while fetching OTS application data.");
                $(".se-pre-con").fadeOut("slow");
            });
    }


    $scope.BackToPage = function () {
        window.location = '../PLDB/OTSApplicationList';
    }

    $scope.Reset = function () {
        OTS.PLDB_OTS_SchemeID = 0
        $scope.DocSelfie = '';
        $scope.DocTaamil = '';
        $scope.DocTaamil = ''
        $scope.RemarkNew = '';
    }



    $scope.UploadeReportFileInDB = function (OTS) {
        debugger;
        if (!confirm('Are you sure you want to upload file?')) {
            var fileInputs = document.getElementsByClassName('fileuploader');
            for (var i = 0; i < fileInputs.length; i++) {
                fileInputs[i].value = '';
            }
            return;
        }

        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();
        formData.append("id", 0);
        formData.append("OTSId", OTS.PLDB_OTS_SchemeID);
        formData.append("DocSelfie", ($scope.DocSelfie??null));
        formData.append("DocTaamil", ($scope.DocTaamil??null));
        formData.append("RemarkNew", ($scope.RemarkNew??''));

        $http.post(SiteUrl + "/PLDB/SaveOTSVerifyReportFile", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data.docId > 0) {
                    $('#Taamilmodal').modal('hide');
                    alert("File upload successfully");
                    $scope.GetApplicationDetails();
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }


    $scope.UploadReportFile = function (file, type) {
        debugger
        if (type == 'Taamil') {
            var maxSize = 2 * 1024 * 1024; // element.getAttribute("checkmaxsize") 
            if (file[0].size > maxSize) { return; }

            if (file) {
                if (file[0].type === 'application/pdf') {
                    $scope.DocTaamil = file[0];
                    // $scope.UploadeReportFileInDB(OTS);
                } else {
                    alert('Please upload a PDF file only.');
                }
            }
        }
        else {
            var maxSize = 5 * 1024 * 1024; // element.getAttribute("checkmaxsize") 
            if (file[0].size > maxSize) { return; }

            if (file) {
                if (file[0].type.startsWith('image/')) {
                    $scope.DocSelfie = file[0];
                    // $scope.UploadeReportFileInDB(OTS);
                } else {
                    alert('Please upload a img file only.');
                }
            }
        }



    }




    $scope.OnPageLoad();
});

