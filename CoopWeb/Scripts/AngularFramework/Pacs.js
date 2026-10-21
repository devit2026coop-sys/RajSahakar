var MasterMod = angular.module('PacsMastersModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("PacsMasterCont", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.divClass = false;

 
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
   
        $(".se-pre-con").fadeOut("slow");

        $scope.GetPacsList();
    };


    $scope.SetStatus = function (OfficeId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangeStatusOffice", { Id: OfficeId, Action: ActionId }).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.GetAllOfficeList('AND 1=1');
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
            });
    };


  


    //madhuri

    $scope.BankMasterList = [];
    $scope.PacsListData = [];
    $scope.PLDBBranches = [];
    $scope.CCBMaster = {};
    $scope.LoanBankList = function (DistrictId) {     
        
        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=" + 0).
            success(function (data, status) {
                $scope.BankMasterList = data;   
                $scope.CCB.BankID = '0';
                $scope.CCBMaster.BankID = '0';

            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
            });
    };

    $scope.LoanBankList();
    $scope.Get_LoanBankBranch = function (branch_ID) {      

        $scope.CCB.BranchId ='0';
        $scope.CCBMaster.BranchId = '0';

        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?LoanBankId=" + branch_ID).
            success(function (data, status) {
                $scope.PLDBBranches = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.ClearPacsForm = function () {
        $scope.CCB = {};
        $scope.CCB.BankID = '0';
        $scope.CCBMaster.BankID = '0';
        $scope.CCB.BranchId = '0';
        $scope.CCBMaster.BranchId = '0';
    }

    $scope.SavePacsForm = function () {
        if ($scope.CCB.BankID != 0 && $scope.CCB.BranchId!=0) {
            $http.post(SiteUrl + "/Master/SavePacsForm", $scope.CCB ).
                success(function (data, status) {
                    $(".se-pre-con").fadeIn("slow");
                    var k = data;
                    var message = '';

                    if (k > 0) {
                        message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                        $(".se-pre-con").fadeOut("slow");
                    }
                    else if (k == -2) {
                        message = _language == 'en-us' ? 'Duplicate Entry\n' : 'डुप्लिकेट प्रविष्टि\n';
                        $(".se-pre-con").fadeOut("slow");
                    }
                    else {
                        message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                        $(".se-pre-con").fadeOut("slow");
                    }
                    alert(message);
                    //ShowModelAlert(_language == 'en-us' ? 'Sport Association' : 'खेल एसोसिएशन', message);
                    window.location = '/master/Pacs';
                    if ($scope.ParentId == '') {
                        $scope.ParentId = 0;
                    }

                    $scope.GetAllOfficeList('AND 1=1');
                    if (data.Status) {
                        $scope.OfficeMaster = [];
                        $scope.ClearPacsForm();
                        //$scope.GetAllSubClassesList();
                    }
                }).
                error(function (data, status) {
                    alert("Error while subclass insert");
                });
        }
        else {
            alert(_language == 'en-us' ? 'Please Select Level.\n' : 'कृपया स्तर का चयन करे \n');
            return;

        }
    };

   
    $scope.GetPacsList = function () {
        $http.post(SiteUrl + "/Master/PacsList").
            success(function (data, status) {
                $scope.PacsListDataTemp = data;
                $scope.PacsListData = data;

            }).
            error(function (data, status) {
                $scope.PacsListData = [];
            });
    };
   

    
    $scope.SortPacsList = function () {
        $scope.PacsListData = $scope.PacsListDataTemp;
        if ($scope.CCBMaster.BankID!=0)
        {
            $scope.PacsListData = $scope.PacsListData.filter(x => x.CCBId == $scope.CCBMaster.BankID);
        }
        if ($scope.CCBMaster.BranchId != 0)
        {
            $scope.PacsListData = $scope.PacsListData.filter(x => x.Branch_ID == $scope.CCBMaster.BranchId);

        }

    };




    $scope.OnPageLoad();
});



