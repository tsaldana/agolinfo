function getRoomReports(feature){
    var qString;
    var action;
    action = "RReport";
    var attr = feature.attributes;
    BLID = attr.Facility;
    FLID = attr.Floor; 
    RMID = attr.rm_id;
    qString = "?Action=" + action + "&bldg=" + BLID + "&FLID=" + FLID + "&RMID=" + RMID;
    doRRepXHRCall(qString);
}

function getRoomOcc_Data(feature){
    var qString;
    var action;
    action = "ROs";
    var attr = feature.attributes;
    BLID = attr.Facility;
    FLID = attr.Floor; 
    RMID = attr.rm_id;
    qString = "?Action=" + action + "&bldg=" + BLID + "&FLID=" + FLID + "&RMID=" + RMID;
    doROXHRCall(qString);
}

function getRoomStd_Data(feature){
    var qString;
    var action;
    action = "RStds";
    var attr = feature.attributes;
    BLID = attr.Facility;
    FLID = attr.Floor; 
    RMID = attr.rm_id;
    qString = "?Action=" + action + "&bldg=" + BLID + "&FLID=" + FLID + "&RMID=" + RMID;
    doRStdXHRCall(qString);
}

function buildContent(feat) {
        var attr = feat.attributes;
        map.graphics.add(feat);
        title = "Facility: " + attr.Facility + ", Floor: " + attr.Floor;
        roomData = "<table><tr><td  colspan=8 class='GISChartTDBold'>AM Room Code:</td><td>" + 
        attr.rm_id +
        "</td></tr><tr><td  colspan=8 class='GISChartTDBold'>Room No:</td><td>" + 
        attr.rm_nbr +
        "</td></tr><tr><td  colspan=8 class='GISChartTDBold'>Area (sq ft):</td><td>" + 
        attr.area_rm +
        "</td></tr><tr><td  colspan=8 class='GISChartTDBold'>Headcount:</td><td>" + 
        attr.count_em +
        "</td></tr><tr><td  colspan=8 class='GISChartTDBold'>Capacity:</td><td>" + 
        attr.cap_em +
        "</td></tr><tr><td  colspan=8 class='GISChartTDBold'>Category Code:</td><td>" + 
        attr.catcode_id + ", " + attr.ccname +
        "</td></tr><tr><td  colspan=8 class='GISChartTDBold'>OSCRE Level 1:</td><td>" + 
        attr.rm_cat +
        "</td></tr><tr><td  colspan=8 class='GISChartTDBold'>OSCRE Level 2:</td><td>" + 
        attr.rm_type +
        "</td></tr><tr><td  colspan=8 class='GISChartTDBold'>OSCRE Level 3:</td><td>" +
        attr.rm_detail + "</td><td></tr></table>"; 
}
function updateRoomContent() {
    var rmTab1 = dijit.byId("roomTab1");
    rmTab1.setContent(roomData);
}

function updateOccContent(s) {
    occData = s
    var rmTab2 = dijit.byId("roomTab2");
    rmTab2.setContent(occData);
}

function updateStdContent(s) {
    stdData = s
    var rmTab3 = dijit.byId("roomTab3");
    rmTab3.setContent(stdData); 
}

function updateReportContent(s) {
    reportData = s
    var rmTab4 = dijit.byId("roomTab4");
    rmTab4.setContent(reportData); 
}
 
function doROXHRCall(s){
    dojo.xhrGet({
        url: "./Service.asmx/getFMLWData" + s,
        handleAs: "json",
        contentType: "application/json; charset=utf-8",
        load: function(data, args){
            updateOccContent(data.d);
        },
        error: function(error, args){
            console.error("Error in method doXHRCall", error);
        }
    });
}; 

function doRStdXHRCall(s){
    dojo.xhrGet({
        url: "./Service.asmx/getFMLWData" + s,
        handleAs: "json",
        contentType: "application/json; charset=utf-8",
        load: function(data, args){
            updateStdContent(data.d);
        },
        error: function(error, args){
            console.error("Error in method doXHRCall", error);
        }
    });
};

function doRRepXHRCall(s){
    dojo.xhrGet({
        url: "./Service.asmx/getFMLWData" + s,
        handleAs: "json",
        contentType: "application/json; charset=utf-8",
        load: function(data, args){
            updateReportContent(data.d);
        },
        error: function(error, args){
            console.error("Error in method doXHRCall", error);
        }
    });
};

function doFireXHRCall(s){
    dojo.xhrGet({
        url: "./Service.asmx/getFMLWData" + s,
        handleAs: "json",
        contentType: "application/json; charset=utf-8",
        load: function(data, args){
            updateReportContent(data.d);
        },
        error: function(error, args){
            console.error("Error in method doXHRCall", error);
        }
    });
};

function openWeb(reportfile,tablename,b,f,r) {
    var url;
    url = arch_report_server + reportfile + "?handler=com.archibus.config.Find&" + tablename + ".bl_id=" + b + "&" + tablename + ".fl_id=" + f + "&" + tablename + ".rm_id=" + r;
    window.open(url);
}