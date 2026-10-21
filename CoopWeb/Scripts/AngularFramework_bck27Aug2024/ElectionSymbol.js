var AppClasses = angular.module("ElectionSymbolMod", ['angularUtils.directives.dirPagination']);

var ImageDataList = [];
var PDFDataList = [];
function FileDataWithRow() {
    this.File = '';
    this.RowNo = '';
    this.Flag = '';
    this.FileName = '';
}
AppClasses.controller("ElectionSymbolCnt", function ($scope, $http, $rootScope) {
    
    $scope.ElectionSymbolsList = [];
    $scope.TotalSymbols = 0;
    $scope._language = "";
    $scope.pageSize = 50;
    $scope.currentPage = 1;
    $scope.AddNewSymbol = {
        symbol_name_en: "",
        symbol_name_hi: "",
        symbol_url: "",
        NewsymbolImg: "",
        symbol_id:0,
        is_active:true
    }

    $scope.ResetAddNew = function () {
        $scope.AddNewSymbol = {
            symbol_name_en: "",
            symbol_name_hi: "",
            symbol_url: "",
            NewsymbolImg: "",
            symbol_id: 0,
            is_active: true
        }
        $("#imgPreview").hide();
    }
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";       
    }
    $scope.GetAllElectionSymbol = function (SocietyIdEnc, ElectionMasterId) {       
        $scope.ShowSocietyList = false;      
        $scope.ShowVoterList = true;     
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Election/GetAllElectionSymbol", { "WhereCase": "" }).
            success(function (data, status) {               
                $scope.ElectionSymbolsList = data;                
                $scope.ElectionSymbolsList.forEach(function (item, indx) {                   
                    item.is_active = item.is_active? true : false;
                    
                });               
                $scope.TotalSymbols = $scope.ElectionSymbolsList.length;
                $(".blockUI").fadeOut("slow");
                
            }).
            error(function (data, status) {
                alert("Error while fetching Society Details.");
                $(".blockUI").fadeOut("slow");
            });

    }

    $scope.SaveElectionSymbol = function (IsSubmit) {
        if ($scope.Validate()) {          
         
            var formData = new FormData();           
            $(".blockUI").fadeIn("slow");
            var fileUpload = $("#AddSymbolImg").get(0);
            var files = fileUpload.files;            
            formData.append("symbolImg", files[0]);
            formData.append("symbol_name_en", $scope.AddNewSymbol.symbol_name_en);
            formData.append("symbol_name_hi", $("#symbol_name_hi").val());
            formData.append("symbol_url", $scope.AddNewSymbol.symbol_url);
            formData.append("is_active", $scope.AddNewSymbol.is_active);
            formData.append("symbol_id", $scope.AddNewSymbol.symbol_id);          
            $http.post(SiteUrl + "/Election/ElectionSymbol", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                   $(".blockUI").fadeOut("slow");
                    if (data.Status.indexOf("-2") > -1) {                        
                        ShowOkMessage("", "You can't change the member detail.", function () { });                     
                    }
                    else if (data.Status.indexOf("-1") > -1) {                         
                        ShowOkMessage("", "Some technical error occur", function () { });
                    }
                    else {
                        ShowOkMessage("", "Data saved successfully", function () {
                            $("#addModal").modal("hide");
                            $scope.ResetAddNew();
                            $scope.GetAllElectionSymbol();
                        });
                       
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                });
            // }
        }

    }

    $scope.Validate = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        $("#myTable2").find('input[type="text"]').removeClass("notValid");
        $("#myTable2").find('input[type="file"]').removeClass("notValid");


        if ($scope.AddNewSymbol.symbol_name_en == "") {
               message += $scope._language == 'en-us' ? '\n <br />Please Enter Symbol Name In English.' : '\n <br />कृपया अंग्रेजी में प्रतीक नाम दर्ज करें।';            
            $("#AddSymbolName").addClass("notValid");
        }

        if ($scope.AddNewSymbol.symbol_name_hi == "") {
            message += $scope._language == 'en-us' ? '\n <br />Please Enter Symbol Name In Hindi.' : '\n <br />कृपया हिंदी में प्रतीक नाम दर्ज करें।';
            $("#symbol_name_hi").addClass("notValid");
        }

        if ($("#AddSymbolImg").val == "" && $scope.AddNewSymbol.symbol_url=="") {
            message += $scope._language == 'en-us' ? '\n <br />Please choose Symbol image.' : '\n <br />कृपया प्रतीक इमेज चुनें।';
            $("#AddSymbolImg").addClass("notValid");
        }

        if (message.length > 29) {
            ShowModelAlert("Election Symbol", message);
            return false;
        }
        return true;
    }
    $scope.EditSymbol = function (index) {
        $scope.AddNewSymbol.symbol_name_en = $scope.ElectionSymbolsList[index].symbol_name_en;
        $scope.AddNewSymbol.symbol_name_hi = $scope.ElectionSymbolsList[index].symbol_name_hi;
        $scope.AddNewSymbol.symbol_url = $scope.ElectionSymbolsList[index].symbol_url;
        $scope.AddNewSymbol.NewsymbolImg = $scope.ElectionSymbolsList[index].NewsymbolImg;
        $scope.AddNewSymbol.symbol_id = $scope.ElectionSymbolsList[index].symbol_id;
        $scope.AddNewSymbol.is_active = $scope.ElectionSymbolsList[index].is_active;
        $("#addModal").modal("show");
        $("#imgPreview").show();
    }
  
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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
       $scope.GetAllElectionSymbol();
    }
    
    $scope.OnPageLoad();
    
    $scope.AddNewSymbols = function () {
        $scope.ResetAddNew();
        $("#addModal").modal("show");
        $("#imgPreview").hide();
    }

});
