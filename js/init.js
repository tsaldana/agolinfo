dojo.require("esri.map");
dojo.require("esri.toolbars.navigation");
dojo.require("esri.tasks.query");
dojo.require("esri.tasks.find");

dojo.require("dijit.form.FilteringSelect");
dojo.require("dijit.form.Button");
dojo.require("dijit.Toolbar");
dojo.require("dojo.parser");
dojo.require("dojo.behavior");


var layer1, layer2, map, queryTask, query, navToolbar, visible = [], qlayer = [], fldef = [], findlyr, CondIdx, CondIdxTxt, sText = null, queryParams;
var featureSet, bldg, cc, org, act, title, content, attr, floor1defs = [], floor2defs = [], floor3defs = [], floorM1defs = [];
var layersLoaded = 0;
var activeTool;
var roomData, occData, stdData, reportData;

var webversion = gup('version');
//var myElement = document.getElementById('resultsTab');
function versioncheck() {
    if (webversion == 'dashboard') {
        document.getElementById("resultsTab").style.paddingRight = "20px";
        document.getElementById("tabsPane").style.width = "220px";
    }
    else
        document.getElementById("resultsTab").style.paddingRight = "20px";
        document.getElementById("tabsPane").style.width = "220px";
}


var server = "mapdemo1.parsons.com";
//var server = "ausgis01";
var httptype = "https";
//var arch_report_server = "http://pgyrwyc1:8080/archibus/schema/per-site/";

// Layer ID constants
var Leu = '0';
//elect meters
var Lem = "1";
//nat gas meters
var Lng = "2";
//water meters
var Lwm = "3";
//cat codes
var Lcc = "4";
//orgs
var Lorg = "5";
//own costs
var Lown = "6";
//oper costs
var Lop = "7";
//cond idx
var Lci = "8";
//util costs
var Luc = "9";
//gw plums
var Lgwp = "10";
//qrate
var Lqrate = "11";
//bldgs
var Lbldgs = "12";
//rooms with no symbology
var Lrooms = "13";

//future dev functions
var showEmpTab = "false";
var showReportTab = "false";

//------ Code Added to display the facility grid lines by query string varible "grid=true" -----------------------------------
var gridoverlay = false; //gup('grid');
var layer3;
function showGrid() { visible = [0]; layer3.setVisibleLayers(visible); }
function hideGrid() { visible = [99]; layer3.setVisibleLayers(visible); }
//-----------------------------------------------------------------------------------------------------------------------------

function init() {
    //configure map zoom animation to be slower
    esriConfig.defaults.map.zoomDuration = 250; //time in milliseconds; default is 250
    esriConfig.defaults.map.zoomRate = 25; //refresh rate of zoom animation; default is 25
    var initialExtent = new esri.geometry.Extent(642497, 3916879, 649050, 3922975, new esri.SpatialReference({
        wkid: 32614
    }));
    
    map = new esri.Map("map", {
        extent: initialExtent,
        logo: false
    });

    dojo.connect(map, "onLoad", function () {
        map.disablePan();
        map.disableKeyboardNavigation();
		map.disableScrollWheelZoom();
    });
    
    layer2 = new esri.layers.ArcGISDynamicMapServiceLayer(httptype + "://" + server + "/ArcGIS/rest/services/TAFB/BM/MapServer");


    layer1 = new esri.layers.ArcGISDynamicMapServiceLayer(httptype + "://" + server + "/ArcGIS/rest/services/TAFB/RP/MapServer");
    layer1.setVisibleLayers([Lbldgs]);
    map.addLayer(layer2);
    map.addLayer(layer1);
    dojo.connect(layer1, "onUpdate", hideLoading);
    dojo.connect(layer1, "onVisibilityChange", hideLoading);
    dojo.connect(map.infoWindow, "onShow", function () {
        var iwTabsDijit = dijit.byId("iw_tabs");
        if (iwTabsDijit !== undefined) { iwTabsDijit.resize(); }
    });

    //------ Code Added to display the facility grid lines by query string varible "grid=true" -----------------------------------
    if (gridoverlay == "true") {
        var imageParameters = new esri.layers.ImageParameters();
        imageParameters.layerIds = [0];
        imageParameters.layerOption = esri.layers.ImageParameters.LAYER_OPTION_SHOW;
        layer3 = new esri.layers.ArcGISDynamicMapServiceLayer("https://tk-geoapp-s.tinker.afmc.ds.af.mil/ArcGIS/rest/services/Misc/MapServer", { "imageParameters": imageParameters});
        map.addLayer(layer3);
        dojo.addOnLoad(function() {
            var string = ""
            string = '<div style="position: absolute; bottom: 20px; right: 20px; margin: 0px; padding: 0px;z-index: 999">';
            string = string + '<button dojotype="dijit.form.ToggleButton" iconclass="dijitCheckBoxIcon" checked="true" onclick="showGrid();">on</button>';
            string = string + '<button dojotype="dijit.form.ToggleButton" iconclass="dijitCheckBoxIcon" checked="true" onclick="hideGrid();">off</button>';
            string = string + '</div>';
            dojo.byId("mapTab").innerHTML += string
            dojo.behavior.apply();         
        });
    }
    else { 
    }
    //-----------------------------------------------------------------------------------------------------------------------------	
	
    //bldgGLayer = new esri.layers.GraphicsLayer();
    //map.addLayer(bldgGLayer);

    navToolbar = new esri.toolbars.Navigation(map);

    //Listen for click event on the map, when the user clicks on the map call executeQueryTask function.
    dojo.connect(map, "onClick", executeClickQueryTask);

    //Listen for map events to control Loading image visibility
    dojo.connect(map, "onLoad", hideLoading);
    dojo.connect(map, "onZoomStart", hideLoading);
    dojo.connect(map, "onPanStart", hideLoading);

    //Listen for infoWindow onHide event
    dojo.connect(map.infoWindow, "onHide", function() {
        if (map.graphics !== null && map.graphics !== undefined && map.graphics !== '') {
            map.graphics.clear();
        }
        setNumRecords(0);
    });

    //get the bldg from the url
    bldg = gup('bldg');
    //get the action from the url
    act = gup('act');
    //get the action from the url
    cc = gup('cc');
    //get the action from the url
    org = gup('org');

    //set the document defaults: Category Codes = On, Floor = 1; Identify = Rooms
    //document.getElementById('bldg').checked = true; //Find
    document.getElementById('roomsID').checked = true; //Identify
    //document.getElementById('searchText').value = bldg; //Find box
    document.getElementById("bldgQ").checked = true; //Find Building Query
    document.getElementById("chkFilter").checked = false;// filter checkbox

    dijit.byId('tbPane').width = '200px';

    //set the floor layer definitions
    floor1defs[Lcc] = "Floor = '1'";
    floor1defs[Lorg] = "Floor = '1'";
    floor2defs[Lcc] = "Floor = '2'";
    floor2defs[Lorg] = "Floor = '2'";
    floor3defs[Lcc] = "Floor = '3'";
    floor3defs[Lorg] = "Floor = '3'";
    floorM1defs[Lcc] = "Floor = 'M1'";
    floorM1defs[Lorg] = "Floor = 'M1'";

    //create find task with url to map service
    findTask = new esri.tasks.FindTask(httptype + "://" + server + "/ArcGIS/rest/services/TAFB/RP/MapServer");

    //create find parameters and define known values
    findParams = new esri.tasks.FindParameters();
    findParams.returnGeometry = true;
    findParams.layerIds = [Lbldgs, Lcc];
    findParams.contains = true;
    findParams.searchFields = ["*"];

    //instantiate Rooms query task
    console.log(httptype);
    queryRoomsTask = new esri.tasks.QueryTask(httptype + "://" + server + "/ArcGIS/rest/services/TAFB/RP/MapServer/" + Lorg);
    //build Rooms query filter
    rooms_query = new esri.tasks.Query();
    rooms_query.returnGeometry = true;
    rooms_query.outFields = ["*"];

    //instantiate Buildings query task
    queryBuildingsTask = new esri.tasks.QueryTask(httptype + "://" + server + "/ArcGIS/rest/services/TAFB/RP/MapServer/" + Lbldgs);
    //build Buildings query filter
    bldgs_query = new esri.tasks.Query();
    bldgs_query.returnGeometry = true;
    bldgs_query.outFields = ["*"];

    //instantiate Parcels query task
    //queryParcelsTask = new esri.tasks.QueryTask(httptype + "://" + server + "/ArcGIS/rest/services/TAFB/RP/MapServer/" + Lparcels);
    //build Parcels query filter
    //parcels_query = new esri.tasks.Query();
    //parcels_query.returnGeometry = true;
    //parcels_query.outFields = ["*"];

    //instantiate EU query task
    queryEUsTask = new esri.tasks.QueryTask(httptype + "://" + server + "/ArcGIS/rest/services/TAFB/RP/MapServer/" + Leu);
    //build EU query filter
    eus_query = new esri.tasks.Query();
    eus_query.returnGeometry = true;
    eus_query.outFields = ["AIRASP_ID"];

    //set the query layer per page
    updateQueryLayer();

    //set the floor layer def per the page
    updateLayerDef();

    processPageLoad();

    //populate the org cmdSelect widget
    buildOrgString('');

    updateDivQuery('BLDG');
    //populate the building list
    popList("bldg", "cboBldgSelect");

    //connect the resize content pane event to the resize map event
    // refresh map
    dojo.connect(dijit.byId("tabsPane"), 'resize', function() {
        if (typeof map == 'undefined') {
            return;
        }
        // alert('repositioning map');

        setTimeout('map.reposition()', 500);
        //alert('resize'); 
        setTimeout('map.resize()', 500);

    });

    var servicewakeup = 0;
    while (servicewakeup <= 20) {
        //alert("just to let you know the reload is getting ready to happen");
        map.setExtent(map.extent);
        servicewakeup = servicewakeup + 1;
    }

//    dojo.connect(map, "onExtentChange", showExtent);
//    var scalebar = new esri.dijit.Scalebar({
//        map: map, attachTo: "bottom-left"
//    });
    // Apply patch to InfoWindow::resize method
//    (function () {
//        var oldResize = esri.dijit.InfoWindow.prototype.resize;
//        esri.dijit.InfoWindow.prototype.resize = function (width, height) {
//            if (!width || !height) {
//                return;
//            }
//            oldResize.apply(this, arguments);
//        };
//    } ());
}


dojo.addOnLoad(init);


function showExtent(extent) {
    var s = "";
    s = "XMin: " + extent.xmin.toFixed(2) + "&nbsp;"
           + "YMin: " + extent.ymin.toFixed(2) + "&nbsp;"
           + "XMax: " + extent.xmax.toFixed(2) + "&nbsp;"
           + "YMax: " + extent.ymax.toFixed(2);
    console.log("Default Extents: " + s);
    //dojo.byId("NumRecordsDiv").innerHTML = s;
}
