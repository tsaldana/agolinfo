function gup(name){
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null) 
        return "";
    else 
        return results[1];
}

function processPageLoad(){
    if ((bldg !== null && bldg !== undefined && bldg !== '') && act == 'hl') {
        findParams.layerIds = [Lbldgs];
        findParams.searchFields = ["FACIL_ID"];
        document.getElementById('searchText').value = bldg; //Find box
        document.getElementById('bldgsf').checked = true; //Identify
        document.getElementById('bldg').checked = true; //Find
        document.getElementById('ccs').checked = true; //Layer
        updateQueryLayer();
        updateLayerVisibility();
        search(bldg);
    }
    else 
        if ((bldg !== null && bldg !== undefined && bldg !== '') && act == 'zo') {
            findParams.layerIds = [Lbldgs];
            findParams.searchFields = ["FACIL_ID"];
            document.getElementById('searchText').value = bldg; //Find box
            document.getElementById('bldgsf').checked = true; //Identify
            document.getElementById('bldg').checked = true; //Find
            document.getElementById('ccs').checked = true; //Layer
            updateQueryLayer();
            updateLayerVisibility();
            search(bldg);
        }
    else 
        if ((cc !== null && cc !== undefined && cc !== '') && act == 'hl') {
            findParams.layerIds = [Lcc];
            findParams.searchFields = ["catcode_id"];
            document.getElementById('searchText').value = cc; //Find box
            document.getElementById('rooms').checked = true; //Identify
    		document.getElementById('catcode').checked = true; //Find                
            document.getElementById('ccs').checked = true; //Layer
            updateQueryLayer();
            updateLayerVisibility();
            search(cc);
        }
    else 
        if ((cc !== null && cc !== undefined && cc !== '') && act == 'zo') {
            findParams.layerIds = [Lcc];
            findParams.searchFields = ["catcode_id"];
            document.getElementById('searchText').value = cc; //Find box
            document.getElementById('rooms').checked = true;
            document.getElementById('catcode').checked = true;
            updateQueryLayer();
            updateLayerVisibility();
            search(cc);
        }
    else 
        if ((org !== null && org !== undefined && org !== '') && act == 'zo') {
            document.getElementById('org').checked = true; //Find
            document.getElementById('orgs').checked = true; //Layers
            findParams.layerIds = [Lcc];
            findParams.searchFields = ["*"];
            var nu_org = org.replace("%20", " ");
            document.getElementById('searchText').value = nu_org; //Find box
            updateQueryLayer();
            updateLayerVisibility();
            search(nu_org);
        }
    else {
        findParams.layerIds = [Lbldgs];
        findParams.searchFields = ["FACIL_ID"];
        updateQueryLayer();
        updateLayerVisibility();
    }
}

function setFindParams(findlyr){
    if (findlyr == 'Building') {
        map.infoWindow.hide();
        map.graphics.clear();
    	featureSet = null;
        findParams.layerIds = [Lbldgs];
        findParams.searchFields = ["FACIL_ID"];
    }
    else 
        if (findlyr == 'Category Code') {
	    map.infoWindow.hide();
	    map.graphics.clear();
	    featureSet = null;        	
            findParams.layerIds = [Lcc];
            findParams.searchFields = ["catcode_id"];
        }
    else 
        if (findlyr == 'Organization') {
            map.infoWindow.hide();
            map.graphics.clear();
	    featureSet = null;
            findParams.layerIds = [Lcc];
            findParams.contains = false;
            findParams.searchFields = ["cmd_id,wing_id,grp_id,squadron_id,flight_id,office_sym"];
        }
    else 
        if (findlyr == 'EU') {
            map.infoWindow.hide();
	    map.graphics.clear();
	    featureSet = null;
	    findParams.layerIds = [Leu];
	    findParams.contains = false;
	    findParams.searchFields = ["AIRASP_ID"];
        }
}

function identify(){
    navToolbar.deactivate();
}

function setUITool(tool){
	if (tool == 'zoomin') {
		navToolbar.activate(esri.toolbars.Navigation.ZOOM_IN);
		}
	else
	if (tool == 'zoomout') {
		navToolbar.activate(esri.toolbars.Navigation.ZOOM_OUT);
		}
	else
	if (tool == 'zoomfullext') {
	    var spatialRef = new esri.SpatialReference({ wkid: 32614 });
	    var startExtent = new esri.geometry.Extent();
	    startExtent.xmin = 642497;
	    startExtent.ymin = 3916879;
	    startExtent.xmax = 649050;
	    startExtent.ymax = 3922975;
	    startExtent.spatialReference = spatialRef;
	    map.setExtent(startExtent);
		}
	else
	if (tool == 'pan') {
		navToolbar.activate(esri.toolbars.Navigation.PAN);
		}
	else
	if (tool == 'identify') {
		identify();
		} 
}

function executeClickQueryTask(evt) {

    var enabIdentify;
    var selTab = dijit.byId('mainTabContainer').selectedChildWidget
    switch (selTab.id) {
        case 'mapTab':
            enabIdentify = true;
            break;
        case 'statisticsTab':
            enabIdentify = false;
            break;
        case 'resultsTab':
            enabIdentify = false;
            break;
    }

    var idntfyCtl = dijit.byId('identify');
    var identifyChkd = idntfyCtl.attr('checked');
    if (identifyChkd && enabIdentify) {
        enabIdentify = true;
    }
    else {
        enabIdentify = false;
    }

    console.log('enable Identify ' + enabIdentify);

    currentFeature = "";
    //map.infoWindow.hide();
    //map.graphics.clear();
    featureSet = null;
    map.infoWindow.setTitle("");
    if (qlayer == 'roomsID' && enabIdentify) {
        map.infoWindow.hide();
        map.graphics.clear();
        //console.log("rooms query");  
        rooms_query.geometry = evt.mapPoint;
        rooms_query.outFields = ["*"];
        rooms_query.where = "";
        queryRoomsTask.execute(rooms_query, function (fset) {
            console.log("clickrooms: " + fset.features.length);
            if (fset.features.length === 1) {
                showFeature(fset.features[0], evt);
            }
            else
                if (fset.features.length !== 0) {
                    showFeatureSet(fset, evt);
                }
                else
                    if (fset.features.length == 0) {
                        showHideLoading("hide");
                    }
            setNumRecords(fset.features.length);
        });
    }
    else
        if (qlayer == 'EUs' && enabIdentify) {
            map.infoWindow.hide();
            map.graphics.clear();
            //console.log("process eu click event");
       		var centerPoint = new esri.geometry.Point(evt.mapPoint.x,evt.mapPoint.y,evt.mapPoint.spatialReference);
        	var mapWidth = map.extent.getWidth();
        	//Divide width in map units by width in pixels
        	var pixelWidth = mapWidth/map.width;
        	//Calculate a 20 pixel envelope width (10 pixel tolerance on each side)
        	var tolerance = 20 * pixelWidth;
        	//Build tolerance envelope and set it as the query geometry
        	var queryExtent = new esri.geometry.Extent(1,1,tolerance,tolerance,evt.mapPoint.spatialReference);
        	eus_query.geometry = queryExtent.centerAt(centerPoint);
        	eus_query.outFields = ["AIRASP_ID"];
        	//console.log("prep eu query");
        	queryEUsTask.execute(eus_query, function (fset) {
        	    //console.log("inside query function");
        	    //console.log(fset.features.length);
        	    if (fset.features.length === 1) {
        	        showFeature(fset.features[0], evt);
        	        var feature = fset.features[0];
        	        var attr = feature.attributes;
        	        var symbol = new esri.symbol.SimpleMarkerSymbol(esri.symbol.SimpleMarkerSymbol.STYLE_DIAMOND, 20, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([0, 92, 230]), 1), new dojo.Color([190, 232, 255, 0.5]));
        	        map.graphics.add(new esri.Graphic(centerPoint, symbol));
        	        currentBuilding = undefined;
        	        currentFeature = attr.AIRASP_ID;
        	        callChart("EUs");
        	        showHideResults(true); //show the divs on the results tab
        	    }
        	    else
        	        if (fset.features.length !== 0) {
        	            title = "Insufficient Zoom Level";
        	            content = "More than one feature selected, " +
            			"<br />please zoom in to ensure only one feature is selected.";
        	            map.infoWindow.resize(275, 90);
        	            map.infoWindow.setTitle(title);
        	            map.infoWindow.setContent(content);
        	            (evt) ? map.infoWindow.show(evt.screenPoint, map.getInfoWindowAnchor(evt.screenPoint)) : null;
        	        }
        	});
        }
	else
	    if (qlayer == 'Owner' && enabIdentify) {
	        map.infoWindow.hide();
	        map.graphics.clear();
            bldgs_query.geometry = evt.mapPoint;
            bldgs_query.outFields = ["FACIL_ID"];
            queryBuildingsTask.execute(bldgs_query, function(fset){
                if (fset.features.length === 1) {
                    showFeature(fset.features[0], evt);
                    var feature = fset.features[0];
                    var attr = feature.attributes;
                    map.graphics.add(feature);
                    currentBuilding = attr.FACIL_ID;
                    callChart("Own");
                    showHideResults(true); //show the divs on the results tab
                }
                else 
                    if (fset.features.length !== 0) {
                        showFeatureSet(fset, evt);
                    }
            });
        }
        else
            if (qlayer == 'OrgChgBk' && enabIdentify) {
                map.infoWindow.hide();
                map.graphics.clear();
                bldgs_query.geometry = evt.mapPoint;
                bldgs_query.outFields = ["FACIL_ID"];
                queryBuildingsTask.execute(bldgs_query, function(fset){
                    if (fset.features.length === 1) {
                        showFeature(fset.features[0], evt);
                        var feature = fset.features[0];
                        var attr = feature.attributes;
                        map.graphics.add(feature);
                        currentBuilding = attr.FACIL_ID;
                        callChart("OrgChgBk");
                        showHideResults(true); //show the divs on the results tab
                    }
                    else 
                        if (fset.features.length !== 0) {
                            showFeatureSet(fset, evt);
                        }
                });
            }
        else
            if (qlayer == 'Occup' && enabIdentify) {
                map.infoWindow.hide();
                map.graphics.clear();
                bldgs_query.geometry = evt.mapPoint;
                bldgs_query.outFields = ["FACIL_ID"];
                queryBuildingsTask.execute(bldgs_query, function(fset){
                    if (fset.features.length === 1) {
                        showFeature(fset.features[0], evt);
                        var feature = fset.features[0];
                        var attr = feature.attributes;
                        map.graphics.add(feature);
                        currentBuilding = attr.FACIL_ID;
                        callChart("Operate");
                        showHideResults(true); //show the divs on the results tab
                    }
                    else 
                        if (fset.features.length !== 0) {
                            showFeatureSet(fset, evt);
                        }
                });
            }
         else
             if (qlayer == 'EnergyCC' && enabIdentify) {
                 map.infoWindow.hide();
                 map.graphics.clear();
                bldgs_query.geometry = evt.mapPoint;
                bldgs_query.outFields = ["FACIL_ID"];
                queryBuildingsTask.execute(bldgs_query, function(fset){
                    if (fset.features.length === 1) {
                        showFeature(fset.features[0], evt);
                        var feature = fset.features[0];
                        var attr = feature.attributes;
                        map.graphics.add(feature);
                        currentBuilding = attr.FACIL_ID;
                        callChart("EnergyCC");
                        showHideResults(true); //show the divs on the results tab
                    }
                    else 
                        if (fset.features.length !== 0) {
                            showFeatureSet(fset, evt);
                        }
                });
            }  
         else
             if (qlayer == 'EnergyMo' && enabIdentify) {
                 map.infoWindow.hide();
                 map.graphics.clear();
                bldgs_query.geometry = evt.mapPoint;
                bldgs_query.outFields = ["FACIL_ID"];
                queryBuildingsTask.execute(bldgs_query, function(fset){
                    if (fset.features.length === 1) {
                        showFeature(fset.features[0], evt);
                        var feature = fset.features[0];
                        var attr = feature.attributes;
                        map.graphics.add(feature);
                        currentBuilding = attr.FACIL_ID;
                        callChart("EnergyMo");
                        showHideResults(true); //show the divs on the results tab
                    }
                    else 
                        if (fset.features.length !== 0) {
                            showFeatureSet(fset, evt);
                        }
                });
            }
         else
             if (qlayer == 'Perf' && enabIdentify) {
                 map.infoWindow.hide();
                 map.graphics.clear();
                bldgs_query.geometry = evt.mapPoint;
                bldgs_query.outFields = ["FACIL_ID"];
                queryBuildingsTask.execute(bldgs_query, function(fset){
                    if (fset.features.length === 1) {
                        showFeature(fset.features[0], evt);
                        var feature = fset.features[0];
                        var attr = feature.attributes;
                        map.graphics.add(feature);
                        currentBuilding = attr.FACIL_ID;
                        callChart("Perf");
                        showHideResults(true); //show the divs on the results tab
                    }
                    else 
                        if (fset.features.length !== 0) {
                            showFeatureSet(fset, evt);
                        }
                });
            }              
         else
             if (qlayer == 'bldgsf' && enabIdentify) {
                 map.infoWindow.hide();
                 map.graphics.clear();
                showHideResults(false); //hide the divs on the results tab
                bldgs_query.geometry = evt.mapPoint;
                bldgs_query.outFields = ["*"];
                showHideLoading("show");
                queryBuildingsTask.execute(bldgs_query, function(fset){
                    if (fset.features.length === 1) {
                        showFeature(fset.features[0], evt);
                    }
                    else 
                        if (fset.features.length !== 0) {
                            showFeatureSet(fset, evt);
                        }
                    else
                    	if  (fset.features.length == 0) {
                    	    showHideLoading("hide");
                    	}
			setNumRecords(fset.features.length);
                });
            }
                 else
                     if (qlayer == 'parcelsf' && enabIdentify) {
                    map.infoWindow.hide();
                    map.graphics.clear();
	                showHideResults(false); //hide the divs on the results tab
	                parcels_query.geometry = evt.mapPoint;
	                parcels_query.outFields = ["*"];
	                showHideLoading("show");
	                queryParcelsTask.execute(parcels_query, function(fset){
	                    if (fset.features.length === 1) {
	                        showFeature(fset.features[0], evt);
	                    }
	                    else 
	                        if (fset.features.length !== 0) {
	                            showFeatureSet(fset, evt);
	                        }
	                    else
	                    	if  (fset.features.length == 0) {
	                    	    showHideLoading("hide");
	                    	}
				setNumRecords(fset.features.length);
	                });
            }
}

function showFeature(feature, evt){
    showHideLoading("hide");
   
    //set symbol
    var symbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([0, 92, 230]), 2), new dojo.Color([190, 232, 255]));
    feature.setSymbol(symbol);
    if (qlayer == 'roomsID') { 
        //construct infowindow title and content
        buildContent(feature);
        map.infoWindow.resize(300, 250);
        map.infoWindow.setTitle(title);
        //destroy old tabs, then the container then create new ones
      //  if (dijit.byId('roomTab1')!== undefined){dijit.byId('roomTab1').destroyRecursive();}
      //  if (dijit.byId('roomTab2')!== undefined){dijit.byId('roomTab2').destroyRecursive();}
       // if (dijit.byId('roomTab3')!== undefined){dijit.byId('roomTab3').destroyRecursive();}
       // if (dijit.byId('roomTab4')!== undefined){dijit.byId('roomTab4').destroyRecursive();}
        if (dijit.byId('iw_tabs') !== undefined) {dijit.byId('iw_tabs').destroyRecursive();}
        var newdiv = document.createElement('div');
        newdiv.setAttribute('id','iw_tabs');
        var ni = document.getElementById('newTabContainer');
        ni.appendChild(newdiv);

        var tc = new dijit.layout.TabContainer({
            style: "height: 100%; width: 100%;",
            tabStrip: 'false'
            },
            "iw_tabs");

        var cp1 = new dijit.layout.ContentPane({
            id:"roomTab1",
            title: "Room",
            style: "height: 100%; width: 95%;",
            selected: 'true'
            });
        tc.addChild(cp1);

        updateRoomContent();
        
        var cp2 = new dijit.layout.ContentPane({
            id:"roomTab2",
            style: "height: 100%; width: 95%;",
            title: "Organizations"
            });
        tc.addChild(cp2);
        getRoomOcc_Data(feature);        
        
        if (showEmpTab == 'true') {
            var cp3 = new dijit.layout.ContentPane({
                id:"roomTab3",
                style: "height: 100%; width: 95%;",
                title: "Employees"
                });
            tc.addChild(cp3);
            getRoomStd_Data(feature);
        }
        
        if (showReportTab == 'true') {
            var cp4 = new dijit.layout.ContentPane({
                id:"roomTab4",
                style: "height: 100%; width: 95%;",
                title: "Reports"
                });
            tc.addChild(cp4);
            getRoomReports(feature);
        } 

        
       
        map.infoWindow.setContent(dijit.byId('iw_tabs').domNode);
        (evt) ? map.infoWindow.show(evt.screenPoint, map.getInfoWindowAnchor(evt.screenPoint)) : null;
        tc.startup();
    }
    else 
        if (qlayer == 'bldgsf') {
            //construct infowindow title and content
            var attr = feature.attributes;
            map.graphics.add(feature);
            title = "Facility: " + attr.FACIL_ID;
            content = "Name: " +
            attr.STRUCTNAME +
            "<br />Build Date: " +
            attr.BUILT_DATE;
	    	map.infoWindow.resize(150,80);            
            map.infoWindow.setTitle(title);
            map.infoWindow.setContent(content);
            (evt) ? map.infoWindow.show(evt.screenPoint, map.getInfoWindowAnchor(evt.screenPoint)) : null;
        }
    else 
        if (qlayer == 'parcelsf') {
            //construct infowindow title and content
            var attr = feature.attributes;
            map.graphics.add(feature);
            title = "Parcel: " + attr.RPAU_ID;
            content = "Acq. ID: " +
            attr.LND_ACQ_ID +
            "<br />Land Area: " +
            attr.LAND_AREA;
	    	map.infoWindow.resize(150,75);            
            map.infoWindow.setTitle(title);
            map.infoWindow.setContent(content);
            (evt) ? map.infoWindow.show(evt.screenPoint, map.getInfoWindowAnchor(evt.screenPoint)) : null;
        }        
}

function showFeatureSet(fset, evt){
    //remove all graphics on the maps graphics layer
    var screenPoint = evt.screenPoint;
    featureSet = fset;
    var numFeatures = featureSet.features.length;
    //QueryTask returns a featureSet.  Loop through features in the featureSet and add them to the infowindow.
    var title = "Select a feature from the list:";
    var content = "";
    if (qlayer == "roomsID") 
       {
       for (var i = 0; i < numFeatures; i++) {
           var graphic = featureSet.features[i];
           if (null == graphic.attributes.rm_id) 
               {
               rm_id = "NK";
               }
           else 
               {
               rm_id = graphic.attributes.rm_id;
               }
           content = content + "Facility " + graphic.attributes.Facility +", Floor " + graphic.attributes.Floor + ", Room Code " + rm_id + " (<a href='#' onclick='showFeature(featureSet.features[" + i + "]); return false;'>show</a>)<br/>";
           showHideLoading("hide");
       }
       }
    else
    if (qlayer == "parcelsf")    
       {
       for (var i = 0; i < numFeatures; i++) {
           var graphic = featureSet.features[i];
           content = content + "Parcel: " + graphic.attributes.RPAU_ID + " (<a href='#' onclick='showFeature(featureSet.features[" + i + "]); return false;'>show</a>)<br/>";
           showHideLoading("hide");    
       }
    }
    map.infoWindow.resize(325,22*(i+1));
    map.infoWindow.setTitle(title);
    map.infoWindow.setContent(content);
    map.infoWindow.show(screenPoint, map.getInfoWindowAnchor(evt.screenPoint));
}

function setCondIdx(CondIdx) {
	if (CondIdx == "1")
	   {
   		CondIdxTxt = "Good";
   	   }
	else
	if (CondIdx == "2")
           {
   		CondIdxTxt = "Fair";
           }
	else
	if (CondIdx == "3")
           {
   		CondIdxTxt = "Poor";
           }
        else
           {
           CondIdxTxt = "N/A";
           }
}

function updateLayerVisibility(){
    var inputs = dojo.query(".GISRadioButtonLV"), input;
    //in this application Buildings (Layer 13) is always on.
    visible = [Lbldgs];
    for (var i = 0, il = inputs.length; i < il; i++) {
        if (inputs[i].checked == true) {
            visible.push(inputs[i].value);
        }
    }
	layer1.setVisibleLayers(visible);
}

function updateLayerDef(){
    fldef = [];
    var inputs = dojo.query(".GISRadioButtonFV"), input;
    for (var i = 0, il = inputs.length; i < il; i++) {
        if (inputs[i].checked == true) {
            fldef.push(inputs[i].id);
        }
        if (fldef[0] == '01') {
            layer1.setLayerDefinitions(floor1defs);
        }
        else 
            if (fldef[0] == '02') {
                layer1.setLayerDefinitions(floor2defs);
            }
        else 
            if (fldef[0] == '03') {
                layer1.setLayerDefinitions(floor3defs);
            }
        else 
            if (fldef[0] == 'M1') {
                layer1.setLayerDefinitions(floorM1defs);
            }
    }
}

function checkidentify() {
    dijit.byId("identify").attr('checked', true);
}

function updateQueryLayer() {
    qlayer = []
    var inputs = dojo.query(".GISRadioButtonQ"), input;
    for (var i = 0, il = inputs.length; i < il; i++) {
        if (inputs[i].checked == true) {
            qlayer.push(inputs[i].id);
        }
    }
    //console.log(qlayer);
}

function search(text){
    //set the search text to find parameters
    cleargraphics();
	document.getElementById('resultsContent').style.display = 'none';
	if (text !== ''){
	    findParams.searchText = text;
        showHideLoading("show");
	    findTask.execute(findParams, showResults);
	}
}

function searchQ(text){
	var filterCheck;
    //set the search text to find parameters
    cleargraphics();
	document.getElementById('resultsContent').style.display = 'none';
	showHideLoading("show");
    qtext = text.split(",");
	q0 = qtext[0];
	q1 = qtext[1];
	q2 = qtext[2];
	q3 = qtext[3];
	q4 = qtext[4];
	
	if (document.getElementById('chkFilter').checked) {
		filterCheck = true;
	}
	
	if (q0) {
		sText = "cmd_id = '" + q0 + "'";
	}
	if (q1) {
		sText += " and wing_id = '" + q1 + "'";
	}
	else
		if (filterCheck) {
			sText += " and wing_id = 'NA'";
		}
	if (q2) {
		sText += " and grp_id = '" + q2 + "'";
	}
	else 
		if (filterCheck) {
			sText += " and grp_id = 'NA'";
		}
	if (q3) {
		sText += " and squadron_id = '" + q3 + "'";
	}
	else 
		if (filterCheck) {
			sText += " and squadron_id = 'NA'";
		}
	if (q4) {
		sText += " and flight_id = '" + q4 + "'";
	}
	else 
		if (filterCheck) {
			sText += " and flight_id = 'NA'";

}
    console.log(sText);
    rooms_query.where = sText;

    rooms_query.geometry = null;
	queryRoomsTask.execute(rooms_query, showFeatures);
//	queryRoomsTask.execute(rooms_query, updateTestCount);

//	queryRoomsTask.execute(rooms_query, function (fset) {
//	    console.log("qtcount: " + fset.length);
//	    if (fset.features.length === 1) {
//	        showFeature(fset.features[0], evt);
//	        var feature = fset.features[0];
//	        var attr = feature.attributes;
//	        map.graphics.add(feature);
//	        currentBuilding = attr.FACIL_ID;
//	        callChart("EnergyMo");
//	        showHideResults(true); //show the divs on the results tab
//	    }
//	    else
//	        if (fset.features.length !== 0) {
//	            showFeatureSet(fset, evt);
//	        }
//	    });
}

function showFeatures(featureSet) {
    var features = featureSet.features;
    console.log("showFeatures");
	showHideLoading("hide");
	map.graphics.clear();
	var pointSymbol = new esri.symbol.SimpleMarkerSymbol(esri.symbol.SimpleMarkerSymbol.STYLE_DIAMOND, 20, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([0, 92, 230]), 1), new dojo.Color([190, 232, 0, 0.5]));
	var polySymbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([0, 92, 230]), 2), new dojo.Color([190, 232, 255]));
	console.log("count: " + features.length);
	for (var i = 0, il = features.length; i < il; i++) {
	    var graphic = features[i];;
	    graphic.setSymbol(polySymbol);
	    map.graphics.add(graphic);
	}
	//get graphics extent
  var graext = new esri.geometry.Extent;
  if (graext !== undefined) {
      graext = getGraphicsExtent(map.graphics.graphics);
      map.setExtent(graext.expand(3.5));
      setNumRecords(features.length);
  }
}

function updateTestCount(featureSet) {
    var resultFeatures = featureSet.features;
    console.log("updatetestCount: " + resultFeatures.length);
}

function showResults(results){
	var exFactor;
    //find results return an array of findResult.
    showHideLoading("hide");
    map.graphics.clear();
    var pointSymbol = new esri.symbol.SimpleMarkerSymbol(esri.symbol.SimpleMarkerSymbol.STYLE_DIAMOND, 20, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([0,92,230]), 1), new dojo.Color([190,232,0,0.5]));		
    var polySymbol =  new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([0, 92, 230]), 2), new dojo.Color([190, 232, 255]));				
    for (var i = 0, il = results.length; i < il; i++) {
        var curFeature = results[i];
        var graphic = curFeature.feature;
		var type = graphic.geometry.type;
		if (type == 'point') {
			//var symbol = 
			exFactor = 4;
			graphic.setSymbol(pointSymbol);
		}
		else 
			if (type == 'polygon') {
		        //var polySymbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_NULL, new dojo.Color([127, 127, 127]), 4), new dojo.Color([255, 255, 255, 0.0]));
				exFactor = 3.5;
				graphic.setSymbol(polySymbol);
			}
        map.graphics.add(graphic);      
    }
    //get feature's extent
    var graext = new esri.geometry.Extent;
    graext = getGraphicsExtent(map.graphics.graphics);
    map.setExtent(graext.expand(3.5));
	setNumRecords(results.length);
}

function cleargraphics(){
    map.infoWindow.hide();
    if (map.graphics !== null && map.graphics !== undefined && map.graphics !== '') {
    	map.graphics.clear();
    }
    showHideResults(true);
    currentBuilding = null;
    showHideLoading("hide");
	setNumRecords(0);
}

function resizeTheMap(){
    if (map !== undefined && map !== null && map !== ''){
       setTimeout("map.resize()",100);
    }
}

function showHideLoading(showOrHide){
    var loaddiv = document.getElementById("LoadingDiv");
    if (showOrHide == "show") {
       loaddiv.style.visibility = "hidden";
    }
    else 
       if (showOrHide == "hide") {
          loaddiv.style.visibility = "hidden";
       }
    }

function showLoading() {
    var loaddiv = document.getElementById("LoadingDiv");
    loaddiv.style.visibility = "hidden";
}

function hideLoading() {
    var loaddiv = document.getElementById("LoadingDiv");
       loaddiv.style.visibility = "hidden";
}

function fixedZoom(inOrOut){
    var zoomLevel = map.getLevel();
    if (inOrOut == 'In'){
        if (zoomLevel < 31){
            zoomLevel +=4;
            map.setLevel(zoomLevel);
        }
        else{
            map.setLevel(34);
        }
    }
    else {
        if (zoomLevel > 3){
            zoomLevel -=4;
            map.setLevel(zoomLevel);
        }
        else{
            map.setLevel(0);
        }
    }
}

function resetToggleTools(btnID){

	if (btnID == activeTool &&  btnID !== 'clearSelection'){
	    var toolCtl = dijit.byId(btnID);
	    toolCtl.attr('checked', true);
	}
	    if (btnID !== 'identify'){
	        var idntfyCtl =  dijit.byId('identify');
	        idntfyCtl.attr('checked', false);
        }

	    if (btnID !== 'pan'){
	        var pnCtl  = dijit.byId('pan');
	        pnCtl.attr('checked', false);
	    }
	    if (btnID !== 'zoomin'){
	        var zminCtl = dijit.byId('zoomin');
	        zminCtl.attr('checked', false);
	    }
	    if (btnID == 'clearSelection'){
	        dijit.byId('identify').attr('checked', false);
	        dijit.byId('pan').attr('checked', false);
	        dijit.byId('zoomin').attr('checked', false);
	        
        }
	activeTool = btnID;
}

function setNumRecords(numSelRecs){
	if (numSelRecs == 500) {
		document.getElementById('NumRecordsDiv').innerHTML = "Selected Records: " + numSelRecs;
		document.getElementById('MaxRecordsDiv').style.visibility = "visible";
	}
	else {
		document.getElementById('NumRecordsDiv').innerHTML = "Selected Records: " + numSelRecs;
		document.getElementById('MaxRecordsDiv').style.visibility = "hidden";
	}
}

function zoomToBldg(text) {
    console.log(text);
    findParams.layerIds = [Lbldgs];
    findParams.searchFields = ["FACIL_ID"];	
    findParams.searchText = text;
    findParams.contains = false;
    findParams.returnGeometry = true;
    findTask.execute(findParams, zoomToExtent);
}

function zoomToExtent(results) {
    console.log("zoomToExtent");
    console.log("count: " + results.length);
    map.graphics.clear();
    if (results.length > 0) {
        var pointSymbol = new esri.symbol.SimpleMarkerSymbol(esri.symbol.SimpleMarkerSymbol.STYLE_DIAMOND, 20, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([0, 92, 230]), 1), new dojo.Color([190, 232, 0, 0.5]));
        var polySymbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([0, 92, 230]), 2), new dojo.Color([190, 232, 255]));
        for (var i = 0, il = results.length; i < il; i++) {
            var curFeature = results[i];
            var graphic = curFeature.feature;
            var type = graphic.geometry.type;
            if (type == 'point') {
                //var pointsymbol =
                exFactor = 4;
                graphic.setSymbol(pointSymbol);
            }
            else
                if (type == 'polygon') {
                    console.log("poly");
                    //var polySymbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_NULL, new dojo.Color([127, 127, 127]), 4), new dojo.Color([255, 255, 255, 0.0]));
                    exFactor = 4;
                    graphic.setSymbol(polySymbol);
                }
            map.graphics.add(graphic);
        }
        console.log("zoomToExtent");
        //bgraphic.setSymbol(symbol);
        //map.graphics.add(bgraphic);
        //map.addLayer(bldgGLayer);

        var graext = new esri.geometry.Extent;
        if (graext !== undefined) {
            graext = getGraphicsExtent(map.graphics.graphics);
            map.setExtent(graext.expand(4.0));
            //setNumRecords(features.length);
        }
        //graext = getGraphicsExtent(map.graphics.graphics);
//        console.log("XMin: " + graext.xmin.toFixed(2) + ";"
//           + "YMin: " + graext.ymin.toFixed(2) + ";"
//           + "XMax: " + graext.xmax.toFixed(2) + ";"
//           + "YMax: " + graext.ymax.toFixed(2));
        //map.setExtent(graext.expand(3));
    }
}

function getGraphicsExtent(graphics) {
    console.log("getGraphicsExtent");
    var geometry, extent, ext;
    console.log(graphics);
    if (graphics !== undefined) {
        //        dojo.forEach(graphics, function (graphic, i) {
        //            console.log("loop: " + i);
        //            geometry = graphic.geometry;
        //            if (geometry instanceof esri.geometry.Point) {
        //                ext = new esri.geometry.Extent(geometry.x - 1, geometry.y - 1, geometry.x + 1, geometry.y + 1, geometry.spatialReference);
        dojo.forEach(graphics, function(graphic, i) {
            // console.log("loop: " + i);
            geometry = graphic.geometry;
            if (geometry instanceof esri.geometry.Point) {
                ext = new esri.geometry.Extent(geometry.x - 1, geometry.y - 1, geometry.x + 1, geometry.y + 1, geometry.spatialReference);
            }
            else
                if (geometry instanceof esri.geometry.Extent) {
                ext = geometry;
            }
            else {
                ext = geometry.getExtent();
            }
            if (extent) {
                extent = extent.union(ext);
            }
            else {
                extent = new esri.geometry.Extent(ext);
            }
        });
        console.log("getGraphicsExtent XMin: " + extent.xmin.toFixed(2) + ";"
           + "YMin: " + extent.ymin.toFixed(2) + ";"
           + "XMax: " + extent.xmax.toFixed(2) + ";"
           + "YMax: " + extent.ymax.toFixed(2));
    }
    else
        if (graphics == undefined) {
        console.log("no graphics returned");
    }

    return extent;
}
