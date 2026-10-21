var CitizenModule = angular.module("SimilarSocietyViewMod", ['angularUtils.directives.dirPagination']);
var ImageDataList = [];

var SportImageDataList = [];
CitizenModule.controller("SimilarSocietyViewCnt", function ($scope, $http) {
    // Global Setting

    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.SocietyIDEnc = '';
    $scope.BackUrl = '';
    $scope.EMitraTransactionId = '';
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.ShowOTPDiv = false;
    $scope.showSocietyReportFile = false;
    $scope.ObjectionFlagBasic = false;
    $scope.ObjectionFlagAdmin = false;
    $scope.ObjectionFlagOffice = false;
    $scope.RoleId = 0;
    $scope.CurrentStatusId = 0;

    //Esign 2.1
    $scope.isEsignProcessing = false;
    $scope.dataPayload = {};
    $scope.esignData = { /*SiteUrl: SiteUrl, DocType: "", ReturnPath: "", docid: "", DocumentPath: "", DocumentName: ""*/ };

    // START : Code for Language label setting
    $scope.getTranslation = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "../Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "../Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
    };

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");      

        var SocietyIDEnc = new URLSearchParams(window.location.search).get("SocietyIDEnc");

        $scope.GetSimilarSocietyDetail(SocietyIDEnc);

        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        $(".se-pre-con").fadeOut("slow");   

    };

    $scope.GetSimilarSocietyDetail = function (SocietyIDEnc) {
        $scope.showLoader = true;
        $http.get("../ApplicationFlow/GetSimilarSocietyDetails/?SocietyIDEnc=" + SocietyIDEnc).
            success(function (data, status) {
                $scope.SimilarSocietyDetail = data;                
                $scope.colsort = "";
                $scope.reverseSort = false;
                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }
                    return '';
                };
            }).
            error(function (data1, status) {

            });
        $scope.showLoader = true;
    }

    $scope.goBack = function () {
        window.location.href = '/ApplicationFlow/ApplicationFlow';
    };

    $scope.update = function (SocietyIDEnc) {
        $scope.showLoader = true;      
        $http.post(SiteUrl + "../ApplicationFlow/GetSocietyDetailBySocietyIDEnc", { "SocietyIDEnc": SocietyIDEnc }).
            success(function (data, status) {
                $scope.GetSimilarSocietyDetail(SocietyIDEnc);
                $scope.SocietyProceesStatusList(data[0].StatusId, data[0].ActId, $scope.SocietyIDEnc);
                $scope.CurrentStatusId = data[0].StatusId;
                $scope.ApplicationFlowMaster.SocietyIDEnc = SocietyIDEnc;
                $scope.ApplicationFlowMaster.ActId = data[0].ActId;
                $scope.ApplicationFlowMaster.SocietyName = data[0].SocietyName;
                $scope.ApplicationFlowMaster.RegistrationNo = data[0].RegistrationNo;
                $scope.ApplicationFlowMaster.UserId = data[0].UserId;
                $scope.ApplicationFlowMaster.SocietyID = data[0].SocietyID;
                $scope.ApplicationFlowMaster.PACS_Code = data[0].PACS_Code;

                $scope.isPacsLocked = !!($scope.ApplicationFlowMaster.PACS_Code && $scope.ApplicationFlowMaster.PACS_Code.trim() !== "");
                $scope.showPacForm = ($scope.CurrentPacsCodeRoleId == 10224 && $scope.CurrentStatusId == 17);

                // GetPaymentDetails
                if (data[0].StatusId == 38 || data[0].StatusId == 39 || data[0].StatusId == 40 || data[0].StatusId == 41) {
                    $scope.GetPaymentDetails($scope.ApplicationFlowMaster.SocietyID);
                }

            }).
            error(function (data, status) {
                $scope.SocietyIDEnc = '';
                $scope.BackUrl = '';
            });
        $scope.showLoader = false;
    };

    $scope.OnPageLoad();
   
});