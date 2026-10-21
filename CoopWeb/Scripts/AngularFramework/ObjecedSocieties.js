
var MasterMod = angular.module('SocietyAmendmentModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("SocietyAmendmentCont", function ($scope, $http, $rootScope) {
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.ShowTab = false;
    $scope.ByLaws = [];
    $scope.ActId = 0;
    $scope.SocietyDirectory = "";
    $scope.SocietyIdEnc = "";
    $scope.MinimMemeber = 5;
    $scope.GetSocietyDetails = true;
    $scope.MemberRegistration = {};
    $scope.SocietyAmendment = { AmendmentSection: 0 };
    //$scope.ShowTab = false;


    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language;
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
            $http.get("../Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }

        
        $scope.showLoader = false;
   
       

       $(".se-pre-con").fadeOut("slow");
    };

    $scope.SocietyRegdata = [];
    $scope.GetSocityDataForObjection = function () {
        $scope.showLoader = true;
        $http.get("../Society/GetSocityDataForObjection/?searchText=" + $scope.SocietyRegistration.SocietyRegistrationNo ).
            success(function (data, status) {
                debugger;
              
                $scope.SocietyRegdata = data;
                $scope.IsSearchResult = true;
                $scope.IsShowProfiling = false;
                $scope.IsShowAmendment = false;

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
                $scope.SocietyRegdata = [];
            });
        $scope.showLoader = true;
    }


    $scope.SelectAllSocieties = function () {
        if ($scope.CheckedAll) {
            $scope.SocietyRegdata.forEach(function (itm) {
                itm.IsChecked = true;
            })

            $scope.chkBulkSelected = true;
        } else {
            $scope.SocietyRegdata.forEach(function (itm) {
                itm.IsChecked = false;
            });
            $scope.chkBulkSelected = false;
        }       
    }
    $scope.UploadReportFile = function (file) {
        
        $scope.ReportSocietyFile = file[0];
    }

    $scope.SelectSociety = function () {
    
        $scope.CheckedAll = $scope.SocietyRegdata.filter(function (x) { return x.IsChecked }).length == $scope.SocietyRegdata.length;

        if ($scope.SocietyRegdata.filter(function (x) { return x.IsChecked }).length > 0) {

            $scope.chkBulkSelected = true;
        }
        else {
            $scope.chkBulkSelected = false;
        }

    }


    $scope.Objection = {};
    $scope.SaveObjectionSocieties = function () {

        var societyId = "";
        var AmendmentIds = "";

        if ($scope.Objection.Remark == '') {
            ShowOkMessage("Alert", _language == 'en-us' ? "Please enter Remark." : "Please enter Remark.", function () { return false; });
        }
        if ($scope.ReportSocietyFile == null || $scope.ReportSocietyFile == "") {
            ShowOkMessage("Alert", _language == 'en-us' ? "Please select Documents." : "Please select Documents.", function () { return false; });
        }

        if ($scope.SocietyRegdata.filter(function (x) { return x.IsChecked }).length>0) {
            $scope.SocietyRegdata.filter(function (x) { return x.IsChecked }).forEach(function (itm) {
                societyId =itm.SocietyId;
                AmendmentIds += (AmendmentIds == "" ? "" : ",") + itm.AmendmentID;
            });
        }

        var formData = new FormData();
        formData.append("Remark", $scope.Objection.Remark || "");
        formData.append("societyId", societyId || "");
        formData.append("AmendmentIds", AmendmentIds || "");
 
        if ($scope.ReportSocietyFile != null && $scope.ReportSocietyFile!="") {
            formData.append("ObjectionFile", $scope.ReportSocietyFile); 
        }


        $http.post("../society/insertObjectedSocieties", formData, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined } // Required for file upload
        }).success(function (data) {
            if (data.data>0) {
                alert("Data saved successfully");
                $scope.clearForm();
                $scope.GetObjectedSocietyList();
              
            } else {
                alert("Failed to save data");
            }
        }).error(function (error) {
            console.error("Error:", error);
            alert("An error occurred while saving data");
        });
    }
    $scope.clearForm = function ()
    {
        $scope.Objection = {};
        $scope.ReportSocietyFile = "";
        $scope.SocietyRegdata = [];
        $scope.chkBulkSelected = false;
        $scope.CheckedAll = false;
        $scope.SelectSociety();
    }

    $scope.GetObjectedSocietyList = function () {
        $scope.showLoader = true;
        $scope.ObjectedSocietyList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get("../Society/AllObjectedSocieties/").
            success(function (data, status) {
                debugger;
                console.log(data);
                $scope.ObjectedSocietyList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {
                $scope.ObjectedSocietyList = [];
                $(".se-pre-con").fadeOut("slow");

            });
        $scope.showLoader = true;
    }


    $scope.ChangeStatus = function (item) {
        $scope.showLoader = true;
        $http.post("../Society/ChangeStatus/?id=" + item.Id).
            success(function (data) {               
                debugger;
                if (data.data > 0) {
                    alert("Data saved successfully");
                    $scope.GetObjectedSocietyList();
                } else {
                    alert("Failed to save data");
                }               

            }).
            error(function (data1, status) {
                alert("Failed to save data");
            });
        $scope.showLoader = true;
    }
    


    $scope.GetObjectedSocietyList();

    $scope.OnPageLoad();







});
MasterMod.directive('validfile', function validFile() {
    var validFormats = ['jpg', 'gif'];
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function () {
                elem.on('change', function () {
                    if (elem[0].files.length > 0) {
                        const fsize = elem[0].files[0].size;
                        const file = Math.round((fsize / 1024));
                        var id = elem.attr("id");

                        // The size of the file. 
                        if (file >= 2048) {
                            ShowOkMessage("Warning", _language == 'en-us' ? "File too Big, please select a file less than 2mb" : "फ़ाइल बहुत बड़ी है, कृपया 2mb से कम फ़ाइल का चयन करें", function () { });
                            elem[0].value = "";
                        } else {

                            if (scope.FileCollection.filter(function (x) { return x.FileId === id }).length <= 0) {
                                scope.FileCollection.push({ 'Id': id, 'FileType': '', 'File': '', 'Title': '', FileName: "", FileId: id, IsFileUpload: true });
                            } else {
                                scope.FileCollection.forEach(function (itm, indx) {
                                    if (itm.FileId == id) {
                                        itm.IsFileUpload = true;
                                    }
                                });
                            }
                            scope.$apply();
                        }
                    }

                });
            };
        }
    };
});

MasterMod.directive('validotherfile', function validFile() {
    var validFormats = ['jpg', 'gif'];
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function () {
                elem.on('change', function () {
                    if (elem[0].files.length > 0) {
                        const fsize = elem[0].files[0].size;
                        const file = Math.round((fsize / 1024));
                        var id = elem.attr("id");
                        var maxLength = 2048;
                        if (file >= parseInt(maxLength)) {
                            ShowOkMessage("Warning", _language == 'en-us' ? "File too Big, please select a file less than " + parseInt(maxLength / 1022) + " MB" : "फ़ाइल बहुत बड़ी है, कृपया " + maxLength / 1022 + " MB से कम फ़ाइल का चयन करें", function () { });
                            elem[0].value = "";
                        } else {

                            if (scope.SocietyOthersDocuments.filter(function (x) { return x.Id == id }).length <= 0) {
                                scope.SocietyOthersDocuments.push({ Id: id, Name: "", FileId: id, IsFileUpload: true });
                            } else {
                                scope.SocietyOthersDocuments.forEach(function (itm, indx) {
                                    if (itm.Id == id) {
                                        itm.IsFileUpload = true;
                                    }
                                });
                            }


                            scope.$apply();
                        }
                    }

                });
            };
        }
    };
});