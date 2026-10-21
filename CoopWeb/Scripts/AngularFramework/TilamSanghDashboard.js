var Tilam_DashboardController = angular.module("Tilam_DashboardCnt", ['angularUtils.directives.dirPagination']);
Tilam_DashboardController.controller("Tilam_DashboardMod", function ($scope, $http) {

    // START : Page load Function
    $scope.IsStateLevelUser = false;
    $scope.IsDistrictLevelUser = false;
    $scope.IsBlockLevelUser = false;

    $scope.OnPageLoad = function () {
        $scope._language = _language;
        var level = parseInt($('#hdnUserLevel').val(), 10);
        var RoleId = parseInt($('#hdnUserRoleId').val(), 10);

        if (level === 1) {
            $scope.UserTitle = 'Welcome State Level User';
        }
        else if (level === 2) {
            $scope.UserTitle = 'Welcome Division Level User';
        }
        else if (level === 3) {
            $scope.UserTitle = 'Welcome District Level User';
        }
        else if (level === 4) {
            $scope.UserTitle = 'Welcome Block Level User';
        }
        else {
            $scope.UserTitle = 'Welcome User';
        }

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

    // START : Get Data List
    $scope.DashBoardData = function () {
        $http.post(SiteUrl + "/Stock/TilamSanghDashboardData").
            success(function (data, status) {
                $scope.TilamSanghDashboard = data;
                console.log($scope.TilamSanghDashboard);
                var level = parseInt($('#hdnUserLevel').val(), 10);
                if (level === 1) {
                    $scope.IsStateLevelUser = true;
                }
                else {
                    $scope.IsStateLevelUser = true;
                }
                var TotPlant = data.Table;
                $scope.GetGraph();
                $scope.Responsedata1 = [];
                $scope.Responsedata2 = [];
                $scope.Responsedata3 = [];
                $scope.Responsedata4 = [];
                $scope.Responsedata1.push(['Task', '', { role: 'annotation' }]);
                $scope.Responsedata2.push(['Task', '', { role: 'annotation' }]);
                $scope.Responsedata3.push(['Task', '', { role: 'annotation' }]);
                $scope.Responsedata4.push(['Task', '', { role: 'annotation' }]);
                $scope.Plant = '';
                $scope.Plant1 = '';
                $scope.Plant2 = '';
                $scope.Plant3 = '';
                $scope.Plant4 = '';
                $scope.Plant1Str = '';
                $scope.Plant2Str = '';
                $scope.Plant3Str = '';
                $scope.Plant4Str = '';
                var i = 0;
                angular.forEach(TotPlant, function (item) {
                    if (i == 0 || $scope.Plant != item.PlaceName) {
                        i = i + 1;
                        $scope.Plant = item.PlaceName;
                    }
                    if (i == 1) {
                        //$scope.Plant1Str += item.ProductSubCategoryNameEng + ":-" + item.TotQuantity + "<br/>";
                        $scope.Plant1Str += '<a href="#!" class="collection-item">' + item.ProductSubCategoryNameEng + ' <span class="new badge2" ng-model="TotalSociety">' + item.TotQuantity + '</span></a>';
                        $scope.Responsedata1.push([_language == 'en-us' ? item.ProductSubCategoryNameEng : item.ProductSubCategoryNameEng, parseInt(item.TotQuantity, 10), parseInt(item.TotQuantity, 10)]);
                        $scope.Plant1 = item.PlaceName;
                    } else if (i == 2) {
                        $scope.Plant2Str += '<a href="#!" class="collection-item">' + item.ProductSubCategoryNameEng + ' <span class="new badge2" ng-model="TotalSociety">' + item.TotQuantity + '</span></a>';
                        $scope.Responsedata2.push([_language == 'en-us' ? item.ProductSubCategoryNameEng : item.ProductSubCategoryNameEng, parseInt(item.TotQuantity, 10), parseInt(item.TotQuantity, 10)]);
                        $scope.Plant2 = item.PlaceName;
                    } else if (i == 3) {
                        $scope.Plant3Str += '<a href="#!" class="collection-item">' + item.ProductSubCategoryNameEng + ' <span class="new badge2" ng-model="TotalSociety">' + item.TotQuantity + '</span></a>';
                        $scope.Responsedata3.push([_language == 'en-us' ? item.ProductSubCategoryNameEng : item.ProductSubCategoryNameEng, parseInt(item.TotQuantity, 10), parseInt(item.TotQuantity, 10)]);
                        $scope.Plant3 = item.PlaceName;
                    }
                    // else if (i == 4) {
                    //    $scope.Responsedata4.push([_language == 'en-us' ? item.ProductSubCategoryNameEng : item.ProductSubCategoryNameEng, parseInt(item.TotQuantity, 10), parseInt(item.TotQuantity, 10)]);
                    //    $scope.Plant4 = item.PlaceName;
                    //} 
                });
                setTimeout(function () {
                    if (i == 1) {
                        $scope.Plant1Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant1 + " Plant</div><div class='card-image'><div class='collection'>" + $scope.Plant1Str + "</div></div></div>";
                        $("#Graph_1divChart1StrVal").html($scope.Plant1Str);
                        drawVisualizationPlantData($scope.Responsedata1, $scope.Plant1,'divChart1');
                    }
                    else if (i == 2) {
                       // $scope.Plant1Str = "<b>" + $scope.Plant1 + " Plant</b><br/>" + $scope.Plant1Str;
                       // $scope.Plant2Str = "<b>" + $scope.Plant2 + " Plant</b><br/>" + $scope.Plant2Str;
                        $scope.Plant1Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant1 + " Plant</div><div class='card-image'><div class='collection'>" + $scope.Plant1Str + "</div></div></div>";
                        $scope.Plant2Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant2 + " Plant</div><div class='card-image'><div class='collection'>" + $scope.Plant2Str + "</div></div></div>";
                        $("#Graph_1divChart1StrVal").html($scope.Plant1Str);
                        $("#Graph_2divChart2StrVal").html($scope.Plant2Str);
                        drawVisualizationPlantData($scope.Responsedata1, $scope.Plant1, 'divChart1');
                        drawVisualizationPlantData($scope.Responsedata2, $scope.Plant2, 'divChart2');
                    }
                    else if (i == 3) {
                        $scope.Plant1Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant1 + " Plant</div><div class='card-image'><div class='collection'>" + $scope.Plant1Str + "</div></div></div>";
                        $scope.Plant2Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant2 + " Plant</div><div class='card-image'><div class='collection'>" + $scope.Plant2Str + "</div></div></div>";
                        $scope.Plant3Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant3 + " Plant</div><div class='card-image'><div class='collection'>" + $scope.Plant3Str + "</div></div></div>";
                        $("#Graph_1divChart1StrVal").html($scope.Plant1Str);
                        $("#Graph_2divChart2StrVal").html($scope.Plant2Str);
                        $("#Graph_3divChart3StrVal").html($scope.Plant3Str);
                        drawVisualizationPlantData($scope.Responsedata1, $scope.Plant1, 'divChart1');
                        drawVisualizationPlantData($scope.Responsedata2, $scope.Plant2, 'divChart2');
                        drawVisualizationPlantData($scope.Responsedata3, $scope.Plant3, 'divChart3');
                    }
                    else if (i == 4) {
                        $scope.Plant1Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant1 + " Plant</div><div class='card-image'><div class='collection'>" + $scope.Plant1Str + "</div></div></div>";
                        $scope.Plant2Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant2 + " Plant</div><div class='card-image'><div class='collection'>" + $scope.Plant2Str + "</div></div></div>";
                        $scope.Plant3Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant3 + " Plant</div><div class='card-image'><div class='collection'>" + $scope.Plant3Str + "</div></div></div>";
                        $("#Graph_1divChart1StrVal").html($scope.Plant1Str);
                        $("#Graph_2divChart2StrVal").html($scope.Plant2Str);
                        $("#Graph_3divChart3StrVal").html($scope.Plant3Str);
                        drawVisualizationPlantData($scope.Responsedata1, $scope.Plant1, 'divChart1');
                        drawVisualizationPlantData($scope.Responsedata2, $scope.Plant2, 'divChart2');
                        drawVisualizationPlantData($scope.Responsedata3, $scope.Plant3, 'divChart3');
                        //drawVisualizationPlantData($scope.Responsedata4, $scope.Plant4, 'divChart4');
                    }
                }, 1000);


                var TotDepo = data.Table1;
                $scope.Responsedata11 = [];
                $scope.Responsedata22 = [];
                $scope.Responsedata33 = [];
                $scope.Responsedata44 = [];
                $scope.Responsedata11.push(['Task', '', { role: 'annotation' }]);
                $scope.Responsedata22.push(['Task', '', { role: 'annotation' }]);
                $scope.Responsedata33.push(['Task', '', { role: 'annotation' }]);
                $scope.Responsedata44.push(['Task', '', { role: 'annotation' }]);
                $scope.Plant = '';
                $scope.Plant11 = '';
                $scope.Plant22 = '';
                $scope.Plant33 = '';
                $scope.Plant44 = '';
                $scope.Plant11Str = '';
                $scope.Plant22Str = '';
                $scope.Plant33Str = '';
                $scope.Plant44Str = '';
                var i1 = 0;
                angular.forEach(TotDepo, function (item) {
                    if (i1 == 0 || $scope.Plant != item.PlaceName) {
                        i1 = i1 + 1;
                        $scope.Plant = item.PlaceName;
                    }
                    if (i1 == 1) {
                        $scope.Plant11Str += '<a href="#!" class="collection-item">' + item.ProductSubCategoryNameEng + ' - ' + item.PackingNameEng + ' <span class="new badge2" ng-model="TotalSociety">' + item.TotQuantity + '</span></a>';
                        //$scope.Plant11Str += item.PackingNameEng + " " + item.ProductSubCategoryNameEng + ":-" + item.TotQuantity + "<br/>";
                        $scope.Responsedata11.push([_language == 'en-us' ? item.PackingNameEng + " " + item.ProductSubCategoryNameEng : item.PackingNameEng + " " + item.ProductSubCategoryNameEng , parseInt(item.TotQuantity, 10), parseInt(item.TotQuantity, 10)]);
                        $scope.Plant11 = item.PlaceName;
                    } else if (i1 == 2) {
                        $scope.Plant22Str += '<a href="#!" class="collection-item">' + item.ProductSubCategoryNameEng + ' - ' + item.PackingNameEng + ' <span class="new badge2" ng-model="TotalSociety">' + item.TotQuantity + '</span></a>';
                        //$scope.Plant22Str += item.PackingNameEng + " " + item.ProductSubCategoryNameEng + ":-" + item.TotQuantity + "<br/>";
                        $scope.Responsedata22.push([_language == 'en-us' ? item.PackingNameEng + " " + item.ProductSubCategoryNameEng : item.PackingNameEng + " " + item.ProductSubCategoryNameEng , parseInt(item.TotQuantity, 10), parseInt(item.TotQuantity, 10)]);
                        $scope.Plant22 = item.PlaceName;
                    } else if (i1 == 3) {
                        $scope.Plant33Str += '<a href="#!" class="collection-item">' + item.ProductSubCategoryNameEng + ' - ' + item.PackingNameEng + ' <span class="new badge2" ng-model="TotalSociety">' + item.TotQuantity + '</span></a>';
                        //$scope.Plant33Str += item.PackingNameEng + " " + item.ProductSubCategoryNameEng + ":-" + item.TotQuantity + "<br/>";
                        $scope.Responsedata33.push([_language == 'en-us' ? item.PackingNameEng + " " + item.ProductSubCategoryNameEng : item.PackingNameEng + " " + item.ProductSubCategoryNameEng , parseInt(item.TotQuantity, 10), parseInt(item.TotQuantity, 10)]);
                        $scope.Plant33 = item.PlaceName;
                    }
                    // else if (i == 4) {
                    //    $scope.Responsedata4.push([_language == 'en-us' ? item.ProductSubCategoryNameEng : item.ProductSubCategoryNameEng, parseInt(item.TotQuantity, 10), parseInt(item.TotQuantity, 10)]);
                    //    $scope.Plant4 = item.PlaceName;
                    //} 
                });
                setTimeout(function () {
                    if (i1 == 1) {
                        $scope.Plant11Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant11 + " Deport</div><div class='card-image'><div class='collection'>" + $scope.Plant11Str + "</div></div></div>";
                        //$scope.Plant11Str = "<b>" + $scope.Plant11 + " Depot</b><br/>" + $scope.Plant11Str;
                        $("#Graph_1divChart11StrVal").html($scope.Plant11Str);
                        drawVisualizationDepoData($scope.Responsedata11, $scope.Plant11, 'divChart11');
                    }
                    else if (i1 == 2) {
                        $scope.Plant11Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant11 + " Deport</div><div class='card-image'><div class='collection'>" + $scope.Plant11Str + "</div></div></div>";
                        $scope.Plant22Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant22 + " Deport</div><div class='card-image'><div class='collection'>" + $scope.Plant22Str + "</div></div></div>";
                        //$scope.Plant11Str = "<b>" + $scope.Plant11 + " Depot</b><br/>" + $scope.Plant11Str;
                        //$scope.Plant22Str = "<b>" + $scope.Plant22 + " Depot</b><br/>" + $scope.Plant22Str;
                        $("#Graph_1divChart11StrVal").html($scope.Plant11Str);
                        $("#Graph_2divChart22StrVal").html($scope.Plant22Str);
                        drawVisualizationDepoData($scope.Responsedata11, $scope.Plant11, 'divChart11');
                        drawVisualizationDepoData($scope.Responsedata22, $scope.Plant22, 'divChart22');
                    }
                    else if (i1 == 3) {
                        $scope.Plant11Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant11 + " Deport</div><div class='card-image'><div class='collection'>" + $scope.Plant11Str + "</div></div></div>";
                        $scope.Plant22Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant22 + " Deport</div><div class='card-image'><div class='collection'>" + $scope.Plant22Str + "</div></div></div>";
                        $scope.Plant33Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant33 + " Deport</div><div class='card-image'><div class='collection'>" + $scope.Plant33Str + "</div></div></div>";
                        //$scope.Plant11Str = "<b>" + $scope.Plant11 + " Depot</b><br/>" + $scope.Plant11Str;
                        //$scope.Plant22Str = "<b>" + $scope.Plant22 + " Depot</b><br/>" + $scope.Plant22Str;
                        //$scope.Plant33Str = "<b>" + $scope.Plant33 + " Depot</b><br/>" + $scope.Plant33Str;
                        $("#Graph_1divChart11StrVal").html($scope.Plant11Str);
                        $("#Graph_2divChart22StrVal").html($scope.Plant22Str);
                        $("#Graph_3divChart33StrVal").html($scope.Plant33Str);
                        drawVisualizationDepoData($scope.Responsedata11, $scope.Plant11, 'divChart11');
                        drawVisualizationDepoData($scope.Responsedata22, $scope.Plant22, 'divChart22');
                        drawVisualizationDepoData($scope.Responsedata33, $scope.Plant33, 'divChart33');
                    }
                    else if (i1 == 4) {
                        $scope.Plant11Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant11 + " Deport</div><div class='card-image'><div class='collection'>" + $scope.Plant11Str + "</div></div></div>";
                        $scope.Plant22Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant22 + " Deport</div><div class='card-image'><div class='collection'>" + $scope.Plant22Str + "</div></div></div>";
                        $scope.Plant33Str = "<div class='card'><div class='card-header card-header-warning'>" + $scope.Plant33 + " Deport</div><div class='card-image'><div class='collection'>" + $scope.Plant33Str + "</div></div></div>";
                        //$scope.Plant11Str = "<b>" + $scope.Plant11 + " Depot</b><br/>" + $scope.Plant11Str;
                        //$scope.Plant22Str = "<b>" + $scope.Plant22 + " Depot</b><br/>" + $scope.Plant22Str;
                        //$scope.Plant33Str = "<b>" + $scope.Plant33 + " Depot</b><br/>" + $scope.Plant33Str;
                        $("#Graph_1divChart11StrVal").html($scope.Plant11Str);
                        $("#Graph_2divChart22StrVal").html($scope.Plant22Str);
                        $("#Graph_3divChart33StrVal").html($scope.Plant33Str);
                        drawVisualizationDepoData($scope.Responsedata11, $scope.Plant11, 'divChart11');
                        drawVisualizationDepoData($scope.Responsedata22, $scope.Plant22, 'divChart22');
                        drawVisualizationDepoData($scope.Responsedata33, $scope.Plant33, 'divChart33');
                        //drawVisualizationDepoData($scope.Responsedata4, $scope.Plant4, 'divChart4');
                    }
                }, 1000);
                //if (_language == 'en-us')
                //    Arry.push(['TilamSang (Purchase Rs.)', 'Sarson', 'Chana', 'Gehu']);
                //else
                //    Arry.push(['तिलम संघ (खरीद रुपये)', 'सरसों', 'चना', 'गेहू']);

                //if (Small == 35) {
                //    Arry.push([('नाहरगढ़'), 40737900, 11443740, 0]);
                //    Arry.push([('करवर'), 58254000, 29156820, 0]);
                //    Arry.push([('सुमेरगंज मंडी'), 61154100, 8752590, 0]);
                //    Arry.push([('हडोल'), 58818900, 28985880, 0]);
                //    Arry.push([('नाहरगढ़'), 40737900, 11443740, 0]);
                //    Arry.push([('करवर'), 58254000, 29156820, 0]);
                //    Arry.push([('सुमेरगंज मंडी'), 61154100, 8752590, 0]);
                //    Arry.push([('हडोल'), 58818900, 28985880, 0]);
                //}
                //else {
                //    Arry.push([('नाहरगढ़'), 40737900, 11443740, 0]);
                //    Arry.push([('करवर'), 58254000, 29156820, 0]);
                //    Arry.push([('सुमेरगंज मंडी'), 61154100, 8752590, 0]);
                //    Arry.push([('हडोल'), 58818900, 28985880, 0]);
                //}

                //var data1 = google.visualization.arrayToDataTable(Arry);
                //var options = {
                //    title: (_language == 'en-us' ? 'MSP Purchase Report (Price)' : 'एमएसपी खरीद रिपोर्ट (मूल्य)'),
                //    vAxis: { title: '' },
                //    hAxis: { title: '' },
                //    seriesType: 'bars',
                //    series: { 5: { type: 'line' } }
                //};
            }).
            error(function (data1, status) {
                $scope.TilamSanghDashboard = [];
            });
    };

    $scope.GetGraph = function () {
        google.charts.load('current', { 'packages': ['corechart'] });
    };
    // END : Get Data List
    function drawVisualizationPlantData(Responsedata, PlantName, GraphId) {
        //alert(GraphId);
        //$('#' + GraphId).show();
        document.getElementById("Graph_" + GraphId).style.display = "block";
        document.getElementById("Graph1_" + GraphId + "Str").style.display = "block";
        //$('#Graph_' + GraphId).show();
        //$("#Graph_" + GraphId).css("display", "block");
        var data = google.visualization.arrayToDataTable(Responsedata);
        var options = {
            title: (_language == 'en-us' ? PlantName + ' Plant Stock Graph View' : PlantName +' Plant भण्डार ग्राफ देखें'),
            vAxis: { title: (_language == 'en-us' ? 'Quantity' : 'मात्रा') },
            hAxis: { title: (_language == 'en-us' ? PlantName + ' Plant Product' : PlantName +' Plant उत्पाद') },
            seriesType: 'bars',
            series: { 5: { type: 'line' } }
        };
        var chart = new google.visualization.ColumnChart(document.getElementById(GraphId));
        chart.draw(data, options);

    }
    function drawVisualizationDepoData(Responsedata, PlantName, GraphId) {
        //alert(GraphId);
        //$('#' + GraphId).show();
        document.getElementById("Graph_" + GraphId).style.display = "block";
        document.getElementById("Graph1_" + GraphId + "Str").style.display = "block";
        $('#Graph_' + GraphId).show();
        $("#Graph_" + GraphId).css("display", "block");
        var data = google.visualization.arrayToDataTable(Responsedata);
        var options = {
            title: (_language == 'en-us' ? PlantName + ' Depot Stock Graph View' : PlantName + ' Depot भण्डार ग्राफ देखें'),
            vAxis: { title: (_language == 'en-us' ? 'Quantity' : 'मात्रा') },
            hAxis: { title: (_language == 'en-us' ? PlantName + ' Depot Product' : PlantName + ' Depot उत्पाद') },
            seriesType: 'bars',
            series: { 5: { type: 'line' } }
        };
        var chart = new google.visualization.ColumnChart(document.getElementById(GraphId));
        chart.draw(data, options);

    }
    $scope.PlantExpandGraph = function (Small, Id) {
        $("#" + Id).toggleClass("popupGraph");
        if (Small == 1) {
            drawVisualizationPlantData($scope.Responsedata1, $scope.Plant1, 'divChart1');
        }
        else if (Small == 2) {
            drawVisualizationPlantData($scope.Responsedata2, $scope.Plant2, 'divChart2');
        }
        else if (Small == 3) {
            drawVisualizationPlantData($scope.Responsedata3, $scope.Plant3, 'divChart3');
        }
        else if (Small == 4) {
            drawVisualizationPlantData($scope.Responsedata4, $scope.Plant4, 'divChart4');
        }
    };
    $scope.DepoExpandGraph = function (Small, Id) {
        $("#" + Id).toggleClass("popupGraph");
        if (Small == 1) {
            drawVisualizationDepoData($scope.Responsedata11, $scope.Plant11, 'divChart11');
        }
        else if (Small == 2) {
            drawVisualizationDepoData($scope.Responsedata22, $scope.Plant22, 'divChart22');
        }
        else if (Small == 3) {
            drawVisualizationDepoData($scope.Responsedata33, $scope.Plant33, 'divChart33');
        }
        //else if (Small == 4) {
        //    drawVisualizationDepoData($scope.Responsedata4, $scope.Plant44, 'divChart44');
        //}
    };

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.DashBoardData();
    // END : On Page Load
});