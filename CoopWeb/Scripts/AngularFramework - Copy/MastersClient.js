
var MasterMod = angular.module('MastersModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("MasterCont", function ($scope, $http) {

    // SCOPE
    //$scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Divisions = [];
    $scope.Districts = [];
    $scope.Tehsil = [];
    $scope.Block = [];
    $scope.Classes = [];

    $scope.divPanchayatSamiti = false;
    $scope.divcity = false;
    $scope.divDisticts = false;
    $scope.DivGP = false;
    $scope.DivTehsil = false;
    $scope.DivVillage = false;
    $scope.DivWard = false;



    // Code for Language label setting
    $scope.getTranslation = function () {
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

        $scope.GetDistrictWithoutDivisionList();

    };
    $scope.GetDistrictWithoutDivisionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };

    $scope.GetPldbList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=0").
            success(function (data, status) {
                $scope.PldbList = data;
            }).
            error(function (data, status) {
                $scope.PldbList = [];
            });
    };

    $scope.SUBForm = function () {
        $http.post(SiteUrl + "/Society/SAVEBranch", { "DistrictId": $scope.Mapping.PldbId, "BlockId": $scope.Mapping.BLOCK_ID, "BranchNameHi": $scope.Mapping.RegistrationNo, "BranchName_En": $scope.Mapping.SocietyName, "CityId": $scope.Mapping.CITY_ID }).
            success(function (data, status) {
                alert('OK');
            }).
            error(function (data, status) {
                alert('Error');
            });
    };
    $scope.BlockListt = function () {
        $scope.Blocks = [];
        $scope.CityList = [];
        $scope.AddMoreForSportShow = true;
        if ($scope.Mapping.DISTRICT_ID != '0') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.Mapping.DISTRICT_ID).
                success(function (data, status) {

                    $scope.Blocks = data;
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
                    $scope.Blocks = [];
                });
        }
        if ($scope.Mapping.DISTRICT_ID != '0') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=" + $scope.Mapping.DISTRICT_ID).
                success(function (data, status) {
                    $scope.PLDBList = data;
                }).
                error(function (data, status) {

                });
        }

    };

    //START : Get City List
    $scope.GetCityListt = function () {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.Mapping.CityTypes + "&MasterCode2=" + $scope.Mapping.DISTRICT_ID).
            success(function (data, status) {
                $scope.CityList = data;
            }).
            error(function (data, status) {
                $scope.CityList = [];
            });


    };


    $scope.BlockList = function () {
        $scope.Blocks = [];
        $scope.AddMoreForSportShow = true;
        if ($scope.Mapping.DISTRICT_ID != '0') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.Mapping.DISTRICT_ID).
                success(function (data, status) {

                    $scope.Blocks = data;
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
                    $scope.Blocks = [];
                });
        }

    };

    $scope.PACSBranchList = function () {
        $scope.PACS_List = [];
        $scope.divPanchayatSamiti = true;
       // if ($scope.Mapping.DISTRICT_ID != '0') {
            $http.get(SiteUrl + "/Master/PACSBranchList/?DISTRICT_ID=" + $scope.Mapping.DISTRICT_ID).
                success(function (data, status) {

                    $scope.PACS_List = data;
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
                    $scope.PACS_List = [];
                });
        //}

    };

    $scope.pageChangeHandler = function (num) {
    };

    // Get List of Divisions
    $scope.DivisionList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Divisions = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
            success(function (data, status) {
                //var _Result = JSON.parse(data);
                $scope.Divisions = data;
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
                $scope.Divisions = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Get List of Districts
    $scope.DistrictList = function () {
        $(".se-pre-con").fadeIn("slow");
        alert(1);
        $scope.Districts = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=" + $scope.ddlDivisions.DIVISION_ID).
            success(function (data, status) {
                $scope.divDisticts = true;
                alert(JSON.stringify(data));
                $scope.Districts = data;


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
                $scope.Districts = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    $scope.DistrictListDivID = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Districts = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=" + $scope.ddlDivisions.DIVISION_ID).
            success(function (data, status) {
                $scope.divDisticts = true;
                $scope.Districts = data;
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
                $scope.Districts = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Get List of Classes
    $scope.Classes = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Classes = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=" + $scope.ddlDivisions.DIVISION_ID).
            success(function (data, status) {
                $scope.divDisticts = true;

                $scope.Districts = data;


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
                $scope.Districts = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Get List of Tehsil
    $scope.TehsilList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Tehsil = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=tehsil&MasterCode=" + $scope.ddlDistrct.DISTRICT_ID).
            success(function (data, status) {

                $scope.DivTehsil = true;

                $scope.Tehsil = data;


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
                $scope.Tehsil = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };


    // Get List of City // created by Ritesh // 20180213 //
    $scope.CityList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.City = [];

        //$http.get(SiteUrl+"/Master/GetMasterData/?MasterType=city&MasterCode=" + $scope.ddlTehsil). // For tehsil wise get city //
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=city&MasterCode=" + $scope.ddlDistrct.DISTRICT_ID).
            success(function (data, status) {
                $scope.divcity = true;
                $scope.City = data;


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
                $scope.City = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Get List of Ward // created by Ritesh // 20180213 //
    $scope.WardList = function () {
        $(".se-pre-con").fadeOut("slow");
        $scope.Ward = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ward&MasterCode=" + $scope.ddlCity.CITY_ID).
            success(function (data, status) {
                $scope.DivWard = true;
                $scope.Ward = data;


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
                $scope.Ward = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Get List of 

    // created by Ritesh // 20180213 //
    $scope.WidgetList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Widget = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=widget&MasterCode=" + $scope.ddlWidgetType).
            success(function (data, status) {
                $scope.DivWidget = true;
                var _Result = JSON.parse(data);
                $scope.Widget = _Result;



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
                $scope.Widget = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Language Selector
    $scope.getTranslation($scope);

    // Get list of all users on page list
    $scope.DivisionList();


    // Get List of Block
    $scope.BlockList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Blocks = [];
        //alert(JSON.stringify($scope.ddlDistrct));

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.ddlDistrct.DISTRICT_ID).
            success(function (data, status) {
                $scope.divPanchayatSamiti = true;
                $scope.Blocks = data;
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
                $scope.Blocks = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Get List of GramPanchayat
    $scope.GramPanchayatList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GramPanchayats = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.ddlBlock.BLOCK_ID).
            success(function (data, status) {
                $scope.DivGP = true;
                $scope.GramPanchayats = data;


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
                $scope.GramPanchayats = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Get List of GramPanchayat
    $scope.VillageList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Villages = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.ddlGramPanchayat.GP_ID).
            success(function (data, status) {
                $scope.DivVillage = true;

                $scope.Villages = data;


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
                $scope.Villages = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    $scope.LeveldataonUrban = function (val) {
        $scope.Mapping.CityTypes = '0';
        $scope.Mapping.BLOCK_ID = '0';
        $scope.Mapping.CITY_ID = '0';
        if (val == 1)
            $scope.ShowUrban = false;
        else
            $scope.ShowUrban = true;
    };


});

var MasterFincialMod = angular.module('MastersFinancialModule', ['angularUtils.directives.dirPagination']);
MasterFincialMod.controller("MasterFincialCont", function ($scope, $http) {

    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.FinancialYears = [];

    // Code for Language label setting
    $scope.getTranslation = function () {

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
    };

    $scope.pageChangeHandler = function (num) {
    };

    // Get List of Financial 
    $scope.FinancialYearList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.FinancialYears = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYears = data;
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
                $scope.FinancialYears = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Language Selector
    $scope.getTranslation($scope);

    // Get list of all users on page list
    $scope.FinancialYearList();
});

var MasterDepartmentMod = angular.module('MastersDepartmentModule', ['angularUtils.directives.dirPagination']);
MasterDepartmentMod.controller("MasterDepartmentCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Departments = [];
    $scope.DepartmentMaster = [];


    $scope.SaveDepartmentForm = function () {
        $http.post(SiteUrl + "/Master/SaveDepartmentForm", { "DEPTNAMEENGLISH": $scope.DepartmentMaster.DEPTNAMEENGLISH, "DEPTNAMEHINDI": $scope.DepartmentMaster.DEPTNAMEHINDI, "IsActive": $scope.DepartmentMaster.IsActive, "ID": $scope.DepartmentMaster.ID }).
            success(function (data, status) {
                var k = data;
                var message = '';

                if (k == 1) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                }
                else if (k == -2) {
                    message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                }
                alert(message);
                //ShowModelAlert(_language == 'en-us' ? 'Sport Association' : 'खेल एसोसिएशन', message);
                window.location = '/master/Department';
                if ($scope.ParentId == '') {
                    $scope.ParentId = 0;
                }

                $scope.DepartmentList();
                if (data.Status) {

                    $scope.DepartmentMaster = null;
                    $scope.ClearDepartmentForm();
                    //$scope.GetAllSubClassesList();


                }
            }).
            error(function (data, status) {
                alert("Error while subclass insert");
            });


    };

    $scope.update = function (item) {
        $scope.DepartmentMaster.DEPTNAMEENGLISH = item.DEPTNAMEENGLISH;
        $scope.DepartmentMaster.DEPTNAMEHINDI = item.DEPTNAMEHINDI;
        $scope.DepartmentMaster.IsActive = item.IsActive == 1 ? true : false;
        $scope.DepartmentMaster.ID = item.ID;
    };

    $scope.ClearDepartmentForm = function () {
        $scope.DepartmentMaster.DEPTNAMEENGLISH = '';
        $scope.DepartmentMaster.DEPTNAMEHINDI = '';
        $scope.DepartmentMaster.IsActive = false;
        $scope.DepartmentMaster.ID = 0;
    };

    $scope.SetStatusDep = function (ID, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangeStatusDepartment", { Id: ID, Action: ActionId }).
            success(function (data, status) {

                $scope.DepartmentList();

            }).
            error(function (data, status) {
            });
    };
    // Code for Language label setting
    $scope.getTranslation = function () {
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
    };

    $scope.pageChangeHandler = function (num) {
    };


    $scope.DepartmentList = function () {
        $(".se-pre-con").fadeOut("slow");
        $http.get(SiteUrl + "/Master/GetDepartmentList").
            success(function (data, status) {
                $scope.Departments = data

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
        $(".se-pre-con").fadeOut("slow");
    };

    // Language Selector
    $scope.getTranslation($scope);

    // Get list of all users on page list
    $scope.DepartmentList();
});

var MasterDesignationMod = angular.module('MastersDesignationModule', ['angularUtils.directives.dirPagination']);
MasterDesignationMod.controller("MasterDesignationCont", function ($scope, $http) {

    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Designations = [];

    // Code for Language label setting
    $scope.getTranslation = function () {

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
    };

    $scope.pageChangeHandler = function (num) {
    };

    // Get List of Designations 
    $scope.DesignationList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Designations = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=designation&MasterCode=-1").
            success(function (data, status) {
                $scope.Designations = data;

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
                $scope.Designations = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Language Selector
    $scope.getTranslation($scope);

    // START : Change Designation Active/Inactive Status
    $scope.ChangeStatus = function (param1, parem2, param3) {
        $http.post(SiteUrl + "/Master/ChangeDesignationStatus", { DesignationId: param1, ActiveStatus: parem2, ActionId: param3 }).
            success(function (data, status) {
                var Result = parseInt(data, 10);
                var Message = '';
                $scope.DesignationList();
                if (Result == -1) {
                    Message = (_language == 'en-us' ? 'Some error occured.' : 'कुछ त्रुटि हुई');
                    alert(Message);
                }
            }).
            error(function (data, status) {
                alert('Error while designation status change.');
            });
    }
    // END : Change Designation Active/Inactive Status

    // Get list of all designations
    $scope.DesignationList();
});

var MasterPostMod = angular.module('MastersPostModule', ['angularUtils.directives.dirPagination']);
MasterPostMod.controller("MasterPostCont", function ($scope, $http) {

    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Post = [];

    // Code for Language label setting
    $scope.getTranslation = function () {

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
    };

    $scope.pageChangeHandler = function (num) {
    };

    // Get List of Designations 
    $scope.PostList = function () {
        $(".se-pre-con").fadeOut("slow");
        $scope.Designations = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=post&MasterCode=-1").
            success(function (data, status) {
                $scope.Post = data;

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
                $scope.Post = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Language Selector
    $scope.getTranslation($scope);

    // Get list of all designations
    $scope.PostList();
});

var MasterGenderMod = angular.module('MastersGenderModule', ['angularUtils.directives.dirPagination']);
MasterGenderMod.controller("MasterGenderCont", function ($scope, $http) {
    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Gender = [];

    // Code for Language label setting
    $scope.getTranslation = function () {
        //  $scope._language = _language;
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
    };

    $scope.pageChangeHandler = function (num) {
    };

    // Get List of Gender 
    $scope.GenderList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Gender = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=gender&MasterCode=-1").
            success(function (data, status) {
                $scope.Gender = data;
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
                $scope.Gender = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Language Selector
    $scope.getTranslation($scope);

    // Get list of all designations
    $scope.GenderList();
});

var MasterReligionMod = angular.module('MastersReligionModule', ['angularUtils.directives.dirPagination']);
MasterReligionMod.controller("MasterReligionCont", function ($scope, $http) {

    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Religion = [];

    // Code for Language label setting
    $scope.getTranslation = function () {

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
    };

    $scope.pageChangeHandler = function (num) {
    }

    // Get List of Gender 
    $scope.ReligionList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Religion = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=religion&MasterCode=-1").
            success(function (data, status) {
                $scope.Religion = data;

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
                $scope.Religion = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Language Selector
    $scope.getTranslation($scope);

    // Get list of all designations
    $scope.ReligionList();
});

var MasterCastMod = angular.module('MastersCastModule', ['angularUtils.directives.dirPagination']);
MasterCastMod.controller("MasterCastCont", function ($scope, $http) {

    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Cast = [];

    // Code for Language label setting
    $scope.getTranslation = function () {

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
    };

    $scope.pageChangeHandler = function (num) {
    };

    // Get List of Gender 
    $scope.CastList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Cast = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=cast&MasterCode=-1").
            success(function (data, status) {

                $scope.Cast = data;

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
                $scope.Cast = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Language Selector
    $scope.getTranslation($scope);

    // Get list of all designations
    $scope.CastList();
});

var MasterCategoryMod = angular.module('MastersCategoryModule', ['angularUtils.directives.dirPagination']);
MasterCategoryMod.controller("MasterCategoryCont", function ($scope, $http) {

    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Category = [];

    // Code for Language label setting
    $scope.getTranslation = function () {

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
                    //LoaderStop();
                }).
                error(function (data, status) {
                    // LoaderStop();
                    alert("Error while translation");
                });
        }

    };

    $scope.pageChangeHandler = function (num) {
    };

    // Get List of Gender 
    $scope.CategoryList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Category = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=category&MasterCode=-1").
            success(function (data, status) {
                $scope.Category = data;

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
                $scope.Category = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Language Selector
    $scope.getTranslation($scope);

    // Get list of all designations
    $scope.CategoryList();
});

var MasterStateMod = angular.module('MastersStateModule', ['angularUtils.directives.dirPagination']);
MasterStateMod.controller("MasterStateCont", function ($scope, $http) {

    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.State = [];

    // Code for Language label setting
    $scope.getTranslation = function () {

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
    };

    $scope.pageChangeHandler = function (num) {
    };

    // Get List of Gender 
    $scope.StateList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.State = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=state&MasterCode=-1").
            success(function (data, status) {
                $scope.State = data;

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
                $scope.State = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Language Selector
    $scope.getTranslation($scope);

    // Get list of all designations
    $scope.StateList();
});

var MastersWidgetMod = angular.module('MastersWidgetModule', ['angularUtils.directives.dirPagination']);
MastersWidgetMod.controller("MastersWidgetCont", function ($scope, $http) {
    //Insert user role details in db
    $scope.SaveWidget = function () {
        $http.post(SiteUrl + "/Master/SaveWidget", $scope.MastersModule).//$scope.userrole
            success(function (data, status) {
                var msg1 = "";
                if (data > 0) {
                    msg1 += (_language == 'en-us' ? 'Data save sucessfully' : 'डेटा सफलतापूर्वक सहेजा गया') ||
                        $scope.ClearWidget();

                }
                else {
                    alert(_language == 'en-us' ? 'Some error occured' : 'कुछ त्रुटि हुई') ||
                        $scope.ClearWidget();
                    return;
                }

                var _Response = angular.fromJson(data);

                if (_Response.data > 0) {
                    $scope.MastersModule = null;
                }
            }).
            error(function (data, status) {
                alert("Error while widget insert");
            });
    };

    // Get List of all Role
    $scope.WidgetList = function () {
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
            $(".se-pre-con").fadeIn("slow");
        }



        $http.get(SiteUrl + "/Master/Widget").
            success(function (data, status) {
                $scope.RoleList2 = data;
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
                alert("Error while fetching widget list.");

            });
    };

    //Clear user Role
    $scope.ClearWidget = function () {

        $scope.MastersModule = null;
    };

    //Get list of all users on page list
    $scope.WidgetList();

});

var LiabilityAssetMasterController = angular.module("LiabilityAssetCnt", ['angularUtils.directives.dirPagination']);
LiabilityAssetMasterController.controller("LiabilityAssetMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridProducts = true;
    $scope.divInsertProduct = false;
    $scope.LiabilityAsset = [];

    // START : Page load Function
    $scope.OnPageLoad = function () {
        $scope._language = _language;

        if (_language === 'en-us') {
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
    };
    // END : Page load Function

    // START : Get List
    $scope.LiabilityAssetList = function () {
        $http.get("/Master/GetMasterData?MasterType=LIABILITYASSET&MasterCode=0").
            success(function (data, status) {
                $scope.LiabilityAssetMasterData = data;
                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort === column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort === column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                    }
                    return '';
                };
            }).
            error(function (data1, status) {
                $scope.LiabilityAssetMasterData = [];
            });
    };
    // END : Get List

    //START : Clear Form
    $scope.Clear = function () {
        $scope.LiabilityAsset.NameEng = '';
        $scope.LiabilityAsset.NameHi = '';
        $scope.LiabilityAsset.IsLiability = '1';
        $scope.LiabilityAsset.IsFurtherExtended = '2';
        $scope.LiabilityAsset.LiabilityAssetMasterId = '0';
    };
    //END : Clear Form

    //START : Show
    $scope.ShowDivProductInsert = function () {
        $scope.GridProducts = false;
        $scope.divInsertProduct = true;
        $scope.Clear();
    };
    //END : Show 

    //START : Back To List
    $scope.BackToList = function () {
        $scope.ShowDivProductInsert();
        $scope.GridProducts = true;
        $scope.divInsertProduct = false;
        $scope.Clear();
    };
    //END : Back To List

    //START : Validate  Form
    $scope.Validate = function () {
        var message = '';
        if ($scope.LiabilityAsset.NameHi === undefined || $scope.LiabilityAsset.NameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Liabilty/Asset Name (in Eng).' : '\n <br /> कृपया दायित्व/संपत्ति नाम (अंग्रेजी) दर्ज करें |';
        }
        if ($scope.LiabilityAsset.NameHi === undefined || $scope.LiabilityAsset.NameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Liabilty/Asset Name (in Hindi)' : '\n <br /> कृपया दायित्व/संपत्ति नाम (हिंदी) दर्ज करें  |';
        }

        if (message.length > 5) {

            ShowModelAlert($scope.translation.LiabilityAsset, message);
            return false;
        }
        return true;
    };
    //END : Validate Form

    //START : Save/Edit
    $scope.Insert = function () {
        $(".se-pre-con").fadeIn("slow");

        if (!$scope.Validate()) {
            $(".se-pre-con").fadeOut("slow");
            return false;
        }

        var model = {
            "LiabilityAssetMasterId": $scope.LiabilityAsset.LiabilityAssetMasterId,
            "NameEng": $scope.LiabilityAsset.NameEng,
            "NameHi": $scope.LiabilityAsset.NameHi,
            "IsLiability": $scope.LiabilityAsset.IsLiability === '1' ? true : false,
            "IsFurtherExtended": $scope.LiabilityAsset.IsFurtherExtended === '1' ? true : false
        };
        $http.post(SiteUrl + "/Master/InsertUpdateLiabilityAsset/", { "model": model }).
            success(function (data, status) {
                if (parseInt(data, 10) > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    alert($scope._language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                    $scope.LiabilityAssetList();
                    $scope.GridProducts = true;
                    $scope.divInsertProduct = false;
                    $scope.Clear();
                }
                else if (parseInt(data, 10) === -1) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.LiabilityAsset, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else if (parseInt(data, 10) === -2) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.LiabilityAsset, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
                }
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });

        $(".se-pre-con").fadeOut("slow");
    };
    //END : Save/Edit

    //START : Edit
    $scope.Edit = function (item) {
        var LiabilityAsset = [];
        LiabilityAsset["LiabilityAssetMasterId"] = item.LiabilityAssetMasterId;
        LiabilityAsset["NameEng"] = item.NameEng;
        LiabilityAsset["NameHi"] = item.NameHi;
        LiabilityAsset["IsLiability"] = item.IsLiability === 'True' ? 1 : 2;
        LiabilityAsset["IsFurtherExtended"] = item.IsFurtherExtended === 'True' ? 1 : 2;
        $scope.LiabilityAsset = LiabilityAsset;
        $scope.GridProducts = false;
        $scope.divInsertProduct = true;
    };
    //END : Edit

    //START : Set Status
    $scope.SetStatus = function (Id, ActionId) {
        $(".se-pre-con").fadeIn("slow");

        if (ActionId === 2) {
            var confirmMsg = _language === 'en-us' ? 'Are you sure to delete this record?' : 'क्या आप इस रिकॉर्ड को हटाना सुनिश्चित कर रहे हैं?';

            if (!confirm(confirmMsg)) {
                $(".se-pre-con").fadeOut("slow");
                return false;
            }
        }

        $http.post(SiteUrl + "/Master/LiabilityAssetMasterSetStatus/", { "LiabilityAssetMasterId": Id, "ActionId": ActionId }).
            success(function (data, status) {
                if (data === -1) {
                    ShowModelAlert($scope.translation.LiabilityAsset, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else {
                    $scope.LiabilityAssetList();
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    //END : Set Status

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.LiabilityAssetList();
    // END : On Page Load
});



var app = angular.module('plunkr', [])
app.controller('UploadController', function ($scope, fileReader) {
    $scope.imageSrc = "";

    $scope.$on("fileProgress", function (e, progress) {
        $scope.progress = progress.loaded / progress.total;
    });
});

app.directive("ngFileSelect", function (fileReader, $timeout) {
    return {
        scope: {
            ngModel: '='
        },
        link: function ($scope, el) {
            function getFile(file) {
                fileReader.readAsDataUrl(file, $scope)
                    .then(function (result) {
                        $timeout(function () {
                            $scope.ngModel = result;
                        });
                    });
            }

            el.bind("change", function (e) {
                var file = (e.srcElement || e.target).files[0];
                getFile(file);
            });
        }
    };
});

app.factory("fileReader", function ($q, $log) {
    var onLoad = function (reader, deferred, scope) {
        return function () {
            scope.$apply(function () {
                deferred.resolve(reader.result);
            });
        };
    };

    var onError = function (reader, deferred, scope) {
        return function () {
            scope.$apply(function () {
                deferred.reject(reader.result);
            });
        };
    };

    var onProgress = function (reader, scope) {
        return function (event) {
            scope.$broadcast("fileProgress", {
                total: event.total,
                loaded: event.loaded
            });
        };
    };

    var getReader = function (deferred, scope) {
        var reader = new FileReader();
        reader.onload = onLoad(reader, deferred, scope);
        reader.onerror = onError(reader, deferred, scope);
        reader.onprogress = onProgress(reader, scope);
        return reader;
    };

    var readAsDataURL = function (file, scope) {
        var deferred = $q.defer();

        var reader = getReader(deferred, scope);
        reader.readAsDataURL(file);

        return deferred.promise;
    };

    return {
        readAsDataUrl: readAsDataURL
    };
});