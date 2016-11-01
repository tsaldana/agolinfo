function doQuery() {

    qOperation = []
    var inputs = dojo.query(".GISRadioButtonD"), input;
    for (var i = 0, il = inputs.length; i < il; i++) {
        if (inputs[i].checked == true) {
            qOperation.push(inputs[i].id);
        }
    }

    type = qOperation[0];

    switch (type) {
        case 'oscre':
            var wingcbo = dojo.byId("cboSelWingOSCREX");
            var oscre1cbo = dojo.byId("cboSelOSCRE1");
            var oscre2cbo = dojo.byId("cboSelOSCRE2");
            var oscre3cbo = dojo.byId("cboSelOSCRE3");
            wing = wingcbo.value;
            oscre1 = oscre1cbo.value;
            oscre2 = oscre2cbo.value;
            oscre3 = oscre3cbo.value;
            console.log("oscrequery " + " " + type + " " + wing + " " + oscre1 + " " + oscre2 + " " + oscre3);
            ZoomToOSCREQ(wing, oscre1, oscre2, oscre3);
            break;
        case 'catcode':
            var wingcbo = dojo.byId("cboSelWingCatCode");
            var catcodeSel = dojo.byId("cboSelCatCode");
            catcodear = catcodeSel.value.split("-");
            var catcode = catcodear[0];
            wing = wingcbo.value;
            ZoomToCatCodeQ(wing, catcode);
            break;
        case 'org':
            //showOrgLoadingGIF('show');
            searchMapOrgRooms(); 
            buildOrgCosts();
            break;
        default: break;
    }
}

function popList(type, cbo) {
    var oscrewing = dojo.byId("cboSelWingOSCREX");
    var oscre1 = dojo.byId("cboSelOSCRE1");
    var oscre2 = dojo.byId("cboSelOSCRE2");
    var oscre3 = dojo.byId("cboSelOSCRE3");
    var catcodewing = dojo.byId("cboSelWingCatCode");
    var catcode = dojo.byId("cboSelCatCode");
    var s;
    var qString;
    var action;
    //console.log("pop " + type);
    switch (type) {
        case 'bldg':
            action = "BldgList";
            qString = "?Action=" + action + "&cbo=" + cbo;
            doBldgListXHRCall(qString);
            break;
        case 'wing':
            action = "WingList";
            qString = "?Action=" + action + "&cbo=" + cbo;
            console.log("winglist " + qString);
            doWingListXHRCall(qString);
            break;
        case 'oscre1':
            if (oscrewing.value.length > 0) {
                action = "OSCREList";
                s = oscrewing.value + "|||";
                qString = "?Action=" + action + "&cbo=" + cbo + "&oscre=" + s;
                qString += "&Filtered=" + document.getElementById("chkFilter").checked;
                console.log('oscre1 ' + qString);
                doOSCREListXHRCall(qString);
            }
            break;
        case 'oscre2':
            action = "OSCREList";
            s = oscrewing.value + "|" + oscre1.value + "||";
            qString = "?Action=" + action + "&cbo=" + cbo + "&oscre=" + s;
            qString += "&Filtered=" + document.getElementById("chkFilter").checked;
            console.log('oscre2 ' + qString);
            doOSCREListXHRCall(qString);
            break;
        case 'oscre3':
            action = "OSCREList";
            s = oscrewing.value + "|" + oscre1.value + "|" + oscre2.value + "|";
            qString = "?Action=" + action + "&cbo=" + cbo + "&oscre=" + s;
            console.log('oscre3 ' + qString);
            doOSCREListXHRCall(qString);
            break;
        case 'catcode':
            action = "CatCodeList";
            //console.log("length " + catcodewing.value.length);
            if (catcodewing.value.length > 0) {
                //console.log("plus");
                s = catcodewing.value;
                qString = "?Action=" + action + "&cbo=" + cbo + "&catcodewing=" + s;
                //console.log('catcode ' + qString);
                doCatCodeListXHRCall(qString);
                //s= catcodewing.value + "|" + catcode.value + "||"; break;
            }
            if (catcodewing.value.length == 0) {
                //console.log("zero");
                document.getElementById('ccTitle').style.display = 'none'; 
                document.getElementById('ccCbo').style.display = 'none';
                document.getElementById('CatCodeTitle').style.display = 'none';
                document.getElementById('divOrgCharge').innerHTML = '';
            }
            break;
        default: s="|||";break;
    }
}

function doCatCodeListXHRCall(s) {
    dojo.xhrGet({
        url: "./Service.asmx/getFMLWData" + s,
        handleAs: "json",
        contentType: "application/json; charset=utf-8",
        load: function (data, args) {
            //console.log(data.d);
            eval(data.d);
        },
        error: function (error, args) {
            console.error("Error in method doCatCodeListXHRCall", error);
        }
    });
};

function doOSCREListXHRCall(s) {
    dojo.xhrGet({
        url: "./Service.asmx/getFMLWData" + s,
        handleAs: "json",
        contentType: "application/json; charset=utf-8",
        load: function (data, args) {
            //console.log(data.d);
            eval(data.d);
        },
        error: function (error, args) {
            console.error("Error in method doOSCREListXHRCall", error);
        }
    });
};

function doWingListXHRCall(s) {
    dojo.xhrGet({
        url: "./Service.asmx/getFMLWData" + s,
        handleAs: "json",
        contentType: "application/json; charset=utf-8",
        load: function (data, args) {
            //console.log(data.d);
            eval(data.d);
        },
        error: function (error, args) {
            console.error("Error in method doWingListXHRCall", error);
        }
    });
}

function doBldgListXHRCall(s) {
    dojo.xhrGet({
        url: "./Service.asmx/getFMLWData" + s,
        handleAs: "json",
        contentType: "application/json; charset=utf-8",
        load: function (data, args) {
            eval(data.d);
        },
        error: function (error, args) {
            console.error("Error in method doBldgListXHRCall", error);
        }
    });
};

function doSelWingBldgListXHRCall(s) {
    dojo.xhrGet({
        url: "./Service.asmx/getFMLWData" + s,
        handleAs: "json",
        contentType: "application/json; charset=utf-8",
        load: function (data, args) {
            console.log(data.d);
            eval(data.d);
        },
        error: function (error, args) {
            console.error("Error in method doSelWingBldgListXHRCall", error);
        }
    });
}


function updateBldgSelect(s) {
    console.log(s);
}

function updateDivQuery(dq) {
    resetQueries();
    hideQDivs();
    if (dq == 'BLDG') {
        console.log("bldgQuery");
        var querydiv = document.getElementById("findBuildingDiv");
        querydiv.style.display = "inline";
        console.log("cboValue: " + dijit.byId('cmdSelect').get('value'));
        dijit.byId('cboBldgSelect').set('value', '0');
        console.log("cboValue: " + dijit.byId('cmdSelect').get('value'));
        document.getElementById('dChkFilter').checked = 'false';
        //var godiv = document.getElementById("cmdGoDiv");
        //godiv.style.display = "none";
    }
    else
        if (dq == 'ORG') {
            dijit.byId('findToolTip').attr('label', 'When checked only the last selected organization level will be shown, suborganizations will be excluded. <br/> When not checked, the selected organization and all of its suborganizations will be shown.');      
            console.log("orgQuery");
            var querydiv = document.getElementById("findOrgDiv");
            querydiv.style.display = "inline";
            //var cboSelect = document.getElementById("cmdSelect");
            console.log("cboValue: " + dijit.byId('cmdSelect').get('value'));
            dijit.byId('cmdSelect').set('value', '0');
            console.log("cboValue: " + dijit.byId('cmdSelect').get('value'));
            document.getElementById('doQueryButton').style.display = 'none';
            document.getElementById('dChkFilter').checked = 'false';
            //var godiv = document.getElementById("cmdGoDiv");
            //godiv.style.display = "inline";

            console.log("orgQuery2");
    }
    else
        if (dq == 'OSCRE') {
            dijit.byId('findToolTip').attr('label', 'When checked only the last selected OSCRE level will be shown, unassigned levels below the selected OSCRE level will be excluded. <br/> When not checked, the selected OSCRE level and all of its sublevels will be shown.');       
            console.log("oscreQuery");
            var querydiv = document.getElementById("findOSCREDiv");
            querydiv.style.display = "inline";
            dijit.byId('cboSelWingOSCREX').set('value', '0');
            //var godiv = document.getElementById("cmdGoDiv");
            //godiv.style.display = "inline";
            document.getElementById('doQueryButton').style.display = 'none';
            document.getElementById('dChkFilter').checked = 'false';
    }
    else
        if (dq == 'CatCode') {
            console.log("ccQuery");
            var querydiv = document.getElementById("findCatCodeDiv");
            querydiv.style.display = "inline";
            dijit.byId('cboSelWingCatCode').set('value', '0');
            //var godiv = document.getElementById("cmdGoDiv");
            //godiv.style.display = "inline";
            document.getElementById('doQueryButton').style.display = 'none';
            document.getElementById('dChkFilter').checked = 'false';
    }
    else // is this used?
        if (dq == 'CMD') {
            console.log("cmdQuery");
            var querydiv = document.getElementById("findCommandDiv");
            querydiv.style.display = "inline";
            document.getElementById('dChkFilter').checked = 'false';
            //var godiv = document.getElementById("cmdGoDiv");
            //godiv.style.display = "inline";
        }
     else
            if (dq == 'BldgWing') {
                console.log("wingQuery");
                var querydiv = document.getElementById("findBldgWingDiv");
                querydiv.style.display = "inline";
                dijit.byId('cboSelWingCatCode').set('value', '0');
                //var godiv = document.getElementById("cmdGoDiv");
                //godiv.style.display = "inline";
                document.getElementById('doQueryButton').style.display = 'none';
                document.getElementById('dChkFilter').checked = 'false';
                //alert("HEY");
            }
}

function hideQDivs() {
    var inputs = dojo.query(".divQuery"), input;
    for (var i = 0, il = inputs.length; i < il; i++) {
        inputs[i].style.display = "none";
    }
    inputs = dojo.query(".divGo"), input;
    for (var i = 0, il = inputs.length; i < il; i++) {
        inputs[i].style.display = "none";
    }
}

function ZoomToCatCodeQ(wing, catcode) {
    var sText;
    if (wing) {
        sText = "wing_id = '" + wing + "'";
    }
    if (catcode) {
        sText += " and catcode_id = '" + catcode + "'";
    }
    rooms_query.where = sText;
    queryRoomsTask.execute(rooms_query, showFeatures);
//    queryRoomsTask.execute(rooms_query, updateTestCount);
}

function ZoomToOSCREQ(wing, oscre1, oscre2, oscre3) {
    var sText;
    if (wing) {
        sText = "wing_id = '" + wing + "'";
    }
    if (oscre1) {
        sText += " and rm_cat = '" + oscre1 + "'";
    }
    if (oscre2) {
        sText += " and rm_type = '" + oscre2 + "'";
    }
    if (oscre3) {
        sText += " and rm_detail = '" + oscre3 + "'";
    }
    //console.log("zoom to oscre", sText);
    rooms_query.where = sText;
    rooms_query.geometry = null;
    queryRoomsTask.execute(rooms_query, showFeatures);
//    queryRoomsTask.execute(rooms_query, updateTestCount);
}

function rmcountCatCode() {
    var wing = dojo.byId("cboSelWingCatCode");
    var catcodeSel = dojo.byId("cboSelCatCode");
    var sText;

    if (wing.value.length > 0) {
        sText = "wing_id = '" + wing.value + "'";
    }
    if (catcodeSel.value.length > 0) {
        var cc = catcodeSel.value.split("-");
        sText += " and catcode_id = '" + dojo.trim(cc[0]) + "'";
    }
    if (sText) {
        console.log("rm count catcode", sText);
        rooms_query.geometry = null;
        rooms_query.where = sText;
        queryRoomsTask.execute(rooms_query, updateRmCount);
    }
}


function rmcountOSCRE() {
    var wing = dojo.byId("cboSelWingOSCREX");
    var oscre1 = dojo.byId("cboSelOSCRE1");
    var oscre2 = dojo.byId("cboSelOSCRE2");
    var oscre3 = dojo.byId("cboSelOSCRE3");
    var oscreFiltered = document.getElementById("chkFilter").checked;
    var sText;
    
    if (wing.value) {
        sText = "wing_id = '" + wing.value + "'";
    }
    if (oscre1.value.length > 0) {
        if (oscre1.value) {
            sText += " and rm_cat = '" + oscre1.value + "'";
            if (!(oscre2.value) && (oscreFiltered == true)) {
                sText += " and rm_type is null"
            }
        }
        if (oscre2.value) {
            sText += " and rm_type = '" + oscre2.value + "'";
            if (!(oscre3.value) && (oscreFiltered == true)) {
                sText += " and rm_detail is null"
            }
        }
        if (oscre3.value) {
            sText += " and rm_detail = '" + oscre3.value + "'";
        }
    }
    if (sText) {
        console.log("rm count oscre ", sText);
        rooms_query.where = sText;
        rooms_query.geometry = null;
        queryRoomsTask.execute(rooms_query, updateRmCount);
    }
}

function updateRmCount(featureSet) {
    var resultFeatures = featureSet.features;
    var count = resultFeatures.length
    console.log("updateRmCount2");
    console.log("rmcount: " + count);

    qOperation = []
    var inputs = dojo.query(".GISRadioButtonD"), input;
    for (var i = 0, il = inputs.length; i < il; i++) {
        if (inputs[i].checked == true) {
            qOperation.push(inputs[i].id);
        }
    }

    type = qOperation[0];

    switch (type) {
        case 'oscre':
            var oscre1 = dojo.byId("cboSelOSCRE1");
            var oscre3 = dojo.byId("cboSelOSCRE3");
            var divQStatus = dojo.byId("divOrgCharge");
            var sText;
            if (count > 1 && oscre1.value.length > 0) {
                sText = "<table><tr><td>This selection will return " + count + " rooms.</td></tr></table>";
                document.getElementById('cmdGoDiv').style.display = 'inline';
                document.getElementById('doQueryButton').style.display = 'inline';
                document.getElementById('dChkFilter').style.display = 'none';
            }
            if (count == 1 && oscre1.value.length > 0) {
                sText = "<table><tr><td>This selection will return " + count + " room.</td></tr></table>";
                document.getElementById('cmdGoDiv').style.display = 'inline';
                document.getElementById('doQueryButton').style.display = 'inline';
                document.getElementById('dChkFilter').style.display = 'none';
            }
            if (count == 0) {
                sText = "";
                dojo.byId("divOrgCharge").innerHTML = sText;
                //sText = "<table><tr><td>This selection will return " + count + " rooms.</td></tr></table>";
                document.getElementById('doQueryButton').style.display = 'none';
                document.getElementById('cmdGoDiv').style.display = 'none';
                document.getElementById('dChkFilter').style.display = 'none'; 
                //dijit.byId('doQueryButton').setAttribute('disabled', true);
            }
            if (oscre1.value.length == 0) {
                sText = "";
                dojo.byId("divOrgCharge").innerHTML = sText;
                document.getElementById('doQueryButton').style.display = 'none';
                document.getElementById('cmdGoDiv').style.display = 'none';
                //dijit.byId('doQueryButton').setAttribute('disabled', true);
            }
            if (oscre1.value.length > 0) {
                dojo.byId("divOrgCharge").innerHTML = sText;
            }

            break;
        case 'catcode':
            var catcode = dojo.byId("cboSelCatCode");
            var divQStatus = dojo.byId("divOrgCharge");
            var sText;
            if (count > 1) {
                sText = "<table><tr><td>This selection will return " + count + " rooms.</td></tr></table>";
                document.getElementById('doQueryButton').style.display = 'inline';
                document.getElementById('cmdGoDiv').style.display = 'inline';
            }
            if (count == 1) {
                sText = "<table><tr><td>This selection will return " + count + " room.</td></tr></table>";
                document.getElementById('doQueryButton').style.display = 'inline';
                document.getElementById('cmdGoDiv').style.display = 'inline';
            }
            if (count == 0) {
                sText = "<table><tr><td>This selection will return " + count + " rooms.</td></tr></table>";
                document.getElementById('doQueryButton').style.display = 'none';
                document.getElementById('cmdGoDiv').style.display = 'none';
                //dijit.byId('doQueryButton').setAttribute('disabled', true);
            }
            if (catcode.value.length == 0) {
                sText = "";
                dojo.byId("divOrgCharge").innerHTML = sText;
                document.getElementById('doQueryButton').style.display = 'none';
                document.getElementById('cmdGoDiv').style.display = 'none';
            }
            if (catcode.value.length > 0) {
                dojo.byId("divOrgCharge").innerHTML = sText;
            }
            break;
    }
}

function ZoomToBldgQ() {
    console.log("ZoomToBldgQ");
    var bldgCMD = dojo.byId("cboBldgSelect");
    if (bldgCMD.value.length > 0) {
        findParams.layerIds = [Lbldgs];
        findParams.searchFields = ["FACIL_ID"];
        findParams.searchText = bldgCMD.value;
        findParams.contains = false;
        findTask.execute(findParams, zBldg);
    }
}

function zBldg(results) {
    console.log("count: " + results.length);
    var polygonSymbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([0, 92, 230]), 2), new dojo.Color([190, 232, 255]));
    //showHideLoading("hide");
    map.graphics.clear();
    if (results.length > 0) {
        //Build an array of attribute information and add each found graphic to the map
        dojo.forEach(results, function (result) {
            var graphic = result.feature;
            //dataForGrid.push([result.layerName, result.foundFieldName, result.value]);
            switch (graphic.geometry.type) {
                case "point":
                    graphic.setSymbol(markerSymbol);
                    break;
                case "polyline":
                    graphic.setSymbol(lineSymbol);
                    break;
                case "polygon":
                    graphic.setSymbol(polygonSymbol);
                    break;
            }
            map.graphics.add(graphic);
        });

        //get feature's extent
        var graext = new esri.geometry.Extent;
        graext = getGraphicsExtent(map.graphics.graphics);
        map.setExtent(graext.expand(5.0));
    }
}

function resetQueries() {
    document.getElementById('dChkFilter').checked = 'false';

    //bldg div
    dijit.byId('cboBldgSelect').set('value', '0');
    //document.getElementById('cboBldgSelect').value = '';

    //org div
    //document.getElementById('cmdSelect').value = '';
    dijit.byId('cmdSelect').set('value', '0');
    document.getElementById('wTitle').style.display = 'none';
    document.getElementById('dWING').style.display = 'none';
    document.getElementById('wingSelect').value = '';
    document.getElementById('gTitle').style.display = 'none';
    document.getElementById('dGROUP').style.display = 'none';
    document.getElementById('grpSelect').value = '';
    document.getElementById('sTitle').style.display = 'none';
    document.getElementById('dSQUAD').style.display = 'none';
    document.getElementById('squadSelect').value = '';
    document.getElementById('fTitle').style.display = 'none';
    document.getElementById('dFLIGHT').style.display = 'none';
    document.getElementById('flightSelect').value = '';
    document.getElementById('dChkFilter').style.display = 'none';
    dojo.byId('divOrgCharge').innerHTML = '';

    //oscre
    //document.getElementById('cboSelWingOSCREX').value = '';
    dijit.byId('cboSelWingOSCREX').set('value', '0');
    document.getElementById('cboSelOSCRE1').value = '';
    document.getElementById('o1Title').style.display = 'none';
    document.getElementById('o1Cbo').style.display = 'none';
    document.getElementById('cboSelOSCRE2').value = '';
    document.getElementById('o2Title').style.display = 'none';
    document.getElementById('o2Cbo').style.display = 'none';
    document.getElementById('o3Title').style.display = 'none';
    document.getElementById('o3Cbo').style.display = 'none';
    document.getElementById('cboSelOSCRE3').value = '';

    //catcode
    document.getElementById('ccTitle').style.display = 'none';
    document.getElementById('ccCbo').style.display = 'none';
    dijit.byId('cboSelWingCatCode').set('value', '0');
    dijit.byId('cboSelCatCode').set('value', '0');

    //go button
    //dijit.byId('doQueryButton').setAttribute('disabled', true);
}

function applyFilter() {

    qOperation = []
    var inputs = dojo.query(".GISRadioButtonD"), input;
    for (var i = 0, il = inputs.length; i < il; i++) {
        if (inputs[i].checked == true) {
            qOperation.push(inputs[i].id);
        }
    }

    type = qOperation[0];

    switch (type) {
        case 'org':
            buildOrgString('FLIGHT');
            break;
        case 'oscre':
            rmcountOSCRE();
            break;
        default:
            break;
    }
}

function disableCCGo() {
    var btnDoQ = dijit.byId('doQueryButton');
    var catcode = dojo.byId("cboSelCatCode");
    console.log(catcode.value);
    if (catcode.value === "") {
        //btnDoQ.attr('disabled', true);
    }
    if (catcode.value) {
        //btnDoQ.attr('disabled', false);
    }
    rmcountCatCode();
}

function ZoomToBldgW() {
    console.log("ZoomToBldgW");
    var wingCMD = dojo.byId("cboBldgWingSelect");
    if (wingCMD.value.length > 0) {
        action = "selWingBldgList";
        qString = "?Action=" + action + "&wing=" + wingCMD.value;
        console.log("winglist " + qString);
        doSelWingBldgListXHRCall(qString);
        //alert(s);  



    }
}

//var jsonData = { identifier: 'index', items: [], label: 'wing' };
//var storeWing = new dojo.data.ItemFileWriteStore({ data: jsonData });
//storeWing.newItem({ index: 0, wing: '' });
//storeWing.newItem({ index: 1, wing: '12 AF' });
//storeWing.newItem({ index: 2, wing: '137 ARW' });
//storeWing.newItem({ index: 3, wing: '22 AF' });
//storeWing.newItem({ index: 4, wing: '327 ASW' });
//storeWing.newItem({ index: 5, wing: '448 CSW' });
//storeWing.newItem({ index: 6, wing: '498 ARSW' });
//storeWing.newItem({ index: 7, wing: '507 ARW' });
//storeWing.newItem({ index: 8, wing: '552 ACW' });
//storeWing.newItem({ index: 9, wing: '552 ACW/ACCC' });
//storeWing.newItem({ index: 10, wing: '554 ESW' });
//storeWing.newItem({ index: 11, wing: '653 ELSW' });
//storeWing.newItem({ index: 12, wing: '72 ABW' });
//storeWing.newItem({ index: 13, wing: '72 MSG' });
//storeWing.newItem({ index: 14, wing: '76 MXW' });
//storeWing.newItem({ index: 15, wing: '82 TRW' });
//storeWing.newItem({ index: 16, wing: 'AAFES' });
//storeWing.newItem({ index: 17, wing: 'CSCW-1' });
//storeWing.newItem({ index: 18, wing: 'DDC' });
//storeWing.newItem({ index: 19, wing: 'DECA' });
//storeWing.newItem({ index: 20, wing: 'DISA' });
//storeWing.newItem({ index: 21, wing: 'DLA' });
//storeWing.newItem({ index: 22, wing: 'DRMS' });
//storeWing.newItem({ index: 23, wing: 'FNB' });
//storeWing.newItem({ index: 24, wing: 'OC-ALC' });
//storeWing.newItem({ index: 25, wing: 'SWING SPACE' });
//storeWing.newItem({ index: 26, wing: 'TFCU' });
//var cboCtl = dijit.byId('cboSelWingOSCRE');
//cboCtl.attr('store', storeWing);
//cboCtl.attr('disabled', false);
//cboCtl.attr('value', '');
//dijit.byId('cboSelWingOSCRE').domNode.style.visibility = '';
