var societydivisionDistrictwise = angular.module("societydivisionDistrictwisemodule", ['angularUtils.directives.dirPagination']);
societydivisionDistrictwise.controller("societydivisionDistrictwiseCount", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.pageSize = 100;
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        };
        $scope.GetSocietyDivisionDistrictwiseMPR();
        $scope.AutoselectMonth();
      
    }
    $scope.AutoselectMonth = function () {
        var DT = new Date();
        var date = "";
        if ((DT.getMonth() + 1) < 10) {
            date = DT.getFullYear() + '-0' + (DT.getMonth() + 1);
        }
        else {
            date = DT.getFullYear() + '-' + (DT.getMonth() + 1);
        }
       $("#txtFromMonth").val(date);
    };
    $scope.AutoselectMonth();




    $scope.getdateproper = function () {
        //
        const monthNames = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        ];

        // alert($("#txtFromMonth").val());
        const today = new Date($("#txtFromMonth").val());
        $scope.dateummu = monthNames[today.getMonth()];
        $scope.dateummu = $scope.dateummu + '-' + today.getFullYear();

        let mm = today.getMonth() + 1; // Months start at 0!
        let dd = today.getDate();
        let yyyy = today.getFullYear();

        if (dd < 10) dd = '0' + dd;
        if (mm < 10) mm = '0' + mm;
        $scope.todaydate = dd + '/' + mm + '/' + yyyy;
        // alert($scope.todaydate);

    };



    $scope.clear = function (){
    
        $scope.ToatlPrevMonth = [];
        $scope.TotalLastMonth = [];
       

        //$scope.ToatlPrevMonth[0] = 0;
        $scope.ToatlPrevMonth[1] = 0;
        $scope.ToatlPrevMonth[2] = 0;
        $scope.ToatlPrevMonth[3] = 0;
        $scope.ToatlPrevMonth[4] = 0;
        $scope.ToatlPrevMonth[5] = 0;
        $scope.ToatlPrevMonth[6] = 0;
        $scope.ToatlPrevMonth[7] = 0;


       // $scope.TotalLastMonth[0] = 0;
        $scope.TotalLastMonth[1] = 0;
        $scope.TotalLastMonth[2] = 0;
        $scope.TotalLastMonth[3] = 0;

        $scope.TotalLastMonth[4] = 0;
        $scope.TotalLastMonth[5] = 0;
        $scope.TotalLastMonth[6] = 0;
        $scope.TotalLastMonth[7] = 0;

        
    }



    $scope.DivistionList = [];
    $scope.GetSocietyDivisionDistrictwiseMPR = function (DIVISION_ID) {
      
        $scope.SocietyDivisionDistrictwiseMPR = [];
        $scope.DivistionList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "MIS/GetSocietyDivisionDistrictwiseMPR", { "Type":"DivisionDistrict", "Dateval": $('#txtFromMonth').val(), "DIVISION_ID": $scope.DivisionId }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                
                if ($("#txtFromMonth").val() != '') {
                              $scope.getdateproper();
                }

                $scope.SocietyDivisionDistrictwiseMPR = JSON.parse(data.DataList);
                $scope.SocietyDivisionDistrictwiseMPR.forEach(function (item) {

                    $scope.Bika1 = $scope.SocietyDivisionDistrictwiseMPR.filter(x => x.DIVISION_ID == 3);
                    $scope.jpr = $scope.SocietyDivisionDistrictwiseMPR.filter(x => x.DIVISION_ID == 4);
                    $scope.Bhart1 = $scope.SocietyDivisionDistrictwiseMPR.filter(x => x.DIVISION_ID == 2);
                    $scope.Aj1 = $scope.SocietyDivisionDistrictwiseMPR.filter(x => x.DIVISION_ID == 1);
                    $scope.jodh1 = $scope.SocietyDivisionDistrictwiseMPR.filter(x => x.DIVISION_ID == 5);
                    $scope.Kota = $scope.SocietyDivisionDistrictwiseMPR.filter(x => x.DIVISION_ID == 6);
                    $scope.udaipur = $scope.SocietyDivisionDistrictwiseMPR.filter(x => x.DIVISION_ID == 7);
                  
                   
                });
                
                
                $scope.clear();
                $scope.Bika1.forEach(function (item) {
                    $scope.ToatlPrevMonth[3] = parseInt($scope.ToatlPrevMonth[3]) + parseInt(item.ToatlPrevMonth);
                    $scope.TotalLastMonth[3] = parseInt($scope.TotalLastMonth[3]) + parseInt(item.TotalLastMonth);
                    
                });
                
                $scope.jpr.forEach(function (item) {
                    
                    $scope.ToatlPrevMonth[4] = parseInt($scope.ToatlPrevMonth[4]) + parseInt(item.ToatlPrevMonth);
                    $scope.TotalLastMonth[4] = parseInt($scope.TotalLastMonth[4]) + parseInt(item.TotalLastMonth);
                    
                });
                
                $scope.Bhart1.forEach(function (item) {
                    $scope.ToatlPrevMonth[2] = parseInt($scope.ToatlPrevMonth[2]) + parseInt(item.ToatlPrevMonth);
                    $scope.TotalLastMonth[2] = parseInt($scope.TotalLastMonth[2]) + parseInt(item.TotalLastMonth);
                    
                });
                
                $scope.Aj1.forEach(function (item) {
                    $scope.ToatlPrevMonth[1] = parseInt($scope.ToatlPrevMonth[1]) + parseInt(item.ToatlPrevMonth);
                    $scope.TotalLastMonth[1] = parseInt($scope.TotalLastMonth[1]) + parseInt(item.TotalLastMonth);
                    
                });
               
                $scope.jodh1.forEach(function (item) {
                    $scope.ToatlPrevMonth[5] = parseInt($scope.ToatlPrevMonth[5]) + parseInt(item.ToatlPrevMonth);
                    $scope.TotalLastMonth[5] = parseInt($scope.TotalLastMonth[5]) + parseInt(item.TotalLastMonth);
                    
                });
                
                $scope.Kota.forEach(function (item) {
                    $scope.ToatlPrevMonth[6] = parseInt($scope.ToatlPrevMonth[6]) + parseInt(item.ToatlPrevMonth);
                    $scope.TotalLastMonth[6] = parseInt($scope.TotalLastMonth[6]) + parseInt(item.TotalLastMonth);
                    
                });
                
                $scope.udaipur.forEach(function (item) {
                    $scope.ToatlPrevMonth[7] = parseInt($scope.ToatlPrevMonth[7]) + parseInt(item.ToatlPrevMonth);
                    $scope.TotalLastMonth[7] = parseInt($scope.TotalLastMonth[7]) + parseInt(item.TotalLastMonth);
                    
                });


           
                //$scope.SocietyDivisionDistrictwiseMPR.forEach(function (item) {
                   
                //    $scope.TotalLastMonth = parseInt($scope.TotalLastMonth)+ parseInt(item.TotalLastMonth);
                //    $scope.ToatlPrevMonth = parseInt($scope.ToatlPrevMonth) + parseInt(item.ToatlPrevMonth);
                //    $scope.ALLTotal = + parseInt(item.TotalLastMonth) + parseInt(item.ToatlPrevMonth);
                //   // alert(parseInt(item.TotalLastMonth) + parseInt(item.ToatlPrevMonth));
                //});
               
                $scope.SocietyDivisionDistrictwiseMPR.forEach(function (item) {
                    if ($scope.DivistionList.filter(function (x) { return x.DIVISION_ENG == item.DIVISION_ENG }).length <= 0) {
                        item.DistricList = [];
                        $scope.DivistionList.push(item)
                    }
                    $scope.DivistionList.filter(function (x) { return x.DIVISION_ENG == item.DIVISION_ENG })[0].DistricList.push(item);
                     
                }); 
                
            }).
            error(function (data, status) {
                alert("Geting Some Errors");
            })
    };


    $scope.ExportData = function () {

        var excel = $JExcel.new("Calibri light 10 #333333");			// Default font	

        excel.set({ sheet: 0, value: "Data"});

        // We want ROW 3 to be EXTRA TALL


        var curr = 1

        var formatHColumn = excel.addStyle({ 															// Format for headers
            //border: "thin,thin,thin,thin #333333", 													// 		Border for header
            font: "Calibri 15 #324148"
        });


        excel.set(0, 0, curr, $scope.translation.MPR2001SocietyRegistered, formatHColumn);

        var formatHeader = excel.addStyle({ 															// Format for headers
            border: "thin,thin,thin,thin #333333", 													// 		Border for header
            font: "Calibri 12 #fff B",
            background: "#64c0c5",
        });

        var formatColumn = excel.addStyle({ 															// Format for headers
            border: "thin,thin,thin,thin #333333", 													// 		Border for header
            font: "Calibri 10 #324148"
        });

        if ($('#txtFromMonth').val() != '' || $('#txtFromMonth').val() == undefined) {
            ++curr;
            excel.set(0, 0, curr, $scope.translation.Month, formatHColumn);
            excel.set(0, 1, curr, $('#txtFromMonth').val(), formatHColumn);
        }



        var distcount = 0;
        var curr1 = 0;
        ++curr;
        $scope.DivistionList.forEach(function (dvsn, indx) {
            ++curr;
            ++curr;
            ++curr;
            excel.set(0, 0, curr, dvsn.DIVISION_MANGAL, formatHColumn);
            var headers = [$scope.translation.SRNO, $scope.translation.District,
                $scope.translation.GatmahtakpanjikritkisankhyokiShankhya,
                $scope.translation.Mahmepanjikritsankhyaokishankhya,
                $scope.translation.Mahkeantmepanjikritsankhyaokikulshankya           
            ];
            ++curr;
            for (var i = 0; i < headers.length; i++) {
                //++curr;// Loop all the haders
                excel.set(0, i, curr, headers[i], formatHeader);													// Set CELL with header text, using header format
                excel.set(0, i, undefined, "auto");															// Set COLUMN width to auto (according to the standard this is only valid for numeric columns)
            }

            //curr1 = curr1 + 2;
            dvsn.DistricList.forEach(function (item, index) {
                ++curr;
                excel.set(0, 0, curr, (index + 1), formatColumn);
                excel.set(0, 1, curr, item.DISTRICT_MANGAL, formatColumn);
                excel.set(0, 2, curr, item.TotalLastMonth, formatColumn);
                excel.set(0, 3, curr, item.ToatlPrevMonth, formatColumn);
                excel.set(0, 4, curr, item.ALLTotal, formatColumn);
            });
            //distcount = dvsn.DistricList.length+1;
            ////curr = dvsn.DistricList + curr - 1;;
            ++curr;
            excel.set(0, 0, curr, "", formatColumn);
            excel.set(0, 1, curr, $scope.translation.Yog, formatColumn);
            excel.set(0, 2, curr, $scope.TotalLastMonth, formatColumn);
            excel.set(0, 3, curr, $scope.ToatlPrevMonth, formatColumn);
            excel.set(0, 4, curr, $scope.ALLTotal, formatColumn);
        });
      
        excel.generate($scope.translation.MPR2001SocietyRegistered + ".xlsx");

    }





    $scope.DivisionList = [];
    $scope.GetDistrictDivisionReasion = function () {
        $scope.DivisionList = [];
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetDistrictDivisionReasion").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                data.List.forEach(function (itm, index) {
                    if ($scope.DivisionList.filter(function (x) { return x.DIVISION_ENG == itm.DIVISION_ENG }).length <= 0) {
                        $scope.DivisionList.push(itm);
                    }
                });
            }).
            error(function (data, status) {
                $scope.DivisionList = [];
            });
    }

    $scope.GetDistrictDivisionReasion();

    $scope.showHideDivision = function (DivId) {
        
        if (DivId != -1) {
            $('.divItemForHide').hide();
            $('.item_' + DivId).show();
        }
        else {
            $('.divItemForHide').show();
        }
    }

    $scope.OnPageLoad();
});





    //$scope.GetSocietyDivisionDistrictwiseMPR = function () {
    //    $http.post(SiteUrl + "MIS/GetSocietyDivisionDistrictwiseMPR").
    //        success(function (data, status) {
    //            $scope.GetSocietyDivisionDistrictwiseMPR = data;
    //            $scope.SocietyDivisionDistrictwiseMPR();
    //        }).
    //        error(function (data, status) {
    //            alert("Geting Some Errors");
    //        })
    //};
    //$scope.SocietyDivisionDistrictwiseMPR = function () {
    //    var SDDWM = "fitDataFill";
    //    $scope.TableData = new Tabulator("#SocietyDivisionDistrictwiseMPR", {
    //        height: 500,
    //        data: $scope.GetSocietyDivisionDistrictwiseMPR,
    //        layout: "fitDataFill",
    //        pagination: "local",
    //        paginationSize: 500,
    //        tooltips: true,          
    //        groupBy: "",
    //        langs: {
    //            "hi-hi": {
    //                "columns": {
    //                    "SNNO": "एसएन नं",
    //                },
    //                "title": {
    //                    "Work Info": "काम की जानकारी",
    //                },
    //                "pagination": {
    //                    "first": "पहला",
    //                    "first_title": "पहला पृष्ठ",
    //                    "last": "अंतिम",
    //                    "last_title": "अंतिम पृष्ठ",
    //                    "prev": "पिछला",
    //                    "prev_title": "पिछला पृष्ठ",
    //                    "next": "अगला",
    //                    "next_title": "अगला पृष्ठ",
    //                    "all": "सभी",
    //                },
    //            },
    //        },
    //        columns: [

    //            { title: "SR.NO", field: "SNNO", sorter: "int", hozAlign: "center" },
    //            { title: "Nam Ekayi", field: "", hozAlign: "center" },
    //            { title: "Gat Mah Tak Panjikrit Shankhyao ki Sankhya", field: "", hozAlign: "center" },
    //            { title: "Mah Me Panjikrit Shankhyao ki Sankhya ", field: "", hozAlign: "center" },                
    //            { title: "Mah Ke Ant Me Panjikrit Shankhyao ki Kull Sankhya", field: "", hozAlign: "center" },               
    //        ],
    //    });
    //}