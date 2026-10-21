var AppClasses = angular.module("BankStatmentModule", ['angularUtils.directives.dirPagination']);
AppClasses.controller("BankStatmentCont", function ($scope, $http) {

    // Save Sms Template 

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.DivSms = true;
    $scope.hidebankbtn = true;
    $scope.newdiv = "";
    $scope.Classes = '';
    $scope.SocietyIDEnc = '';
    $scope.BasicDetails = [];


    $scope.SaveBankDetails = function () {
        var url = window.location.search;
        $scope.SocietyIDEnc = (url.split('=')[1]);      
        var formData = new FormData();
        formData.append("SocietyIDEnc", $scope.SocietyIDEnc);
        formData.append("BankPassbookFile", $scope.BankPassbookFile);
        formData.append("OpeningBalance", $scope.BankStatement.OpeningBalance);
        formData.append("@flag",1);
        $http.post(SiteUrl + "/society/SaveBankDetails", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {                
                alert(_language == 'en-us' ? 'Data Saved Sucessfully\n' : 'डेटा सहेजा गया है\n');
                location.reload();
            }).
            error(function (data, status) {
                $scope.hidebankbtn = true;
            });


    };



    $scope.pageChangeHandler = function (num) {

    };
    // Update Template  
    $scope.UploadBankPassbook = function (file) {        
        $scope.BankPassbookFile = file[0];
        
    }
    // on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get(SiteUrl +"/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl +"/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        $scope.ClearClassForm = function () {
            $scope.Classes = '';
        };



        $(".se-pre-con").fadeOut("slow");

        //$scope.GetAllMailData();

    };
    // Language Selector at page load


    //GET Load Table data
    $scope.DashboardData = function (ActId, SocietyTypeId, ClassId) {
        var url = window.location.search;
        $scope.SocietyIDEnc = (url.split('=')[1]);
        $http.post(SiteUrl + "/Citizen/BankstamentData", { "SocietyIdEnc": $scope.SocietyIDEnc == '' ? '' : $scope.SocietyIDEnc }).
            success(function (data, status) {              
                $scope.TableData = [];               
                $scope.TableData = data;
                if (data.length > 0)
                {
                    $scope.BasicDetails = data[0];
                    if (data[data.length - 1].MaxAppliFlowStatusID != 14)
                    {
                        $scope.hidebankbtn = false;
                    }
                }
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
            error(function (data, status) {
                $scope.TableData = [];
            });

    }
    //END
    //$scope.CheckValidation = function{
    //    if()

    //}
    //validateAddForm START
    //$scope.FormValidation = function () {

    //    var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

    //    if ($scope.SocietyRegistration.ActId == "7") {
    //        if ($scope.SocietyRegistration.IsStatLevel == undefined) {
    //            message += $scope._language == 'en-us' ? '\n <br /> Please Select Sports Association level' : '\n <br /> कृपया क्रीड़ा संगम का स्तर का चयन करे |';

          

    //    }
    //    if (message.length > 29) {
    //        ShowModelAlert("Society Profiling", message);
    //        return false;
    //    }
    //    return true;
    //}
    ////END

    $scope.OnPageLoad();
   // $scope.ShowDetails();
    $scope.DashboardData();

});

