var queryString;
var myChartSeries;
var currentBuilding;
var currentFeature;
var chartHeadingHTML;
var chartAction;
var numRoomsFromOrgSearch;
        
function doXHRCall(){        
    dojo.xhrGet({                
        url: "./Service.asmx/getFMLWData" + queryString,
        handleAs: "json",
        contentType: "application/json; charset=utf-8",
        load: function(data, args){
            ShowChart(data.d);
        },
        error: function(error, args){
            console.error("Error in method doXHRCall", error);
        }
    });
}

makePieObjects = function(){
    var dc = dojox.charting;
    var pieChart = new dojox.charting.Chart2D("chartDiv");
    pieChart.addPlot("default", {
        type: "Pie",
        font: "normal normal bold 8pt Aerial",
        fontColor: "white",
        labelOffset: 20
    });
    eval(myChartSeries);
    //var anim_d = new dc.action2d.Shake(pieChart, "default");
    //var anim_a = new dc.action2d.MoveSlice(pieChart, "default");
    var anim_b = new dc.action2d.Highlight(pieChart, "default");
    var anim_c = new dc.action2d.Tooltip(pieChart, "default");
    pieChart.render();
    showHideLoading("hide");
}

makeBarObjects = function(){
    var barChart = new dojox.charting.Chart2D("chartDiv");
    barChart.addPlot("default", {type: "Columns", gap: 2});
    eval(myChartSeries);
    barChart.render();
    showHideLoading("hide");
}

function ShowChart(HTMArg){
            var contentNode = dojo.byId("sampleone");
            var mySplitResult = HTMArg.split("*?*?*?*");
            var myResults = document.getElementById("ResultsDiv");
            myResults.innerHTML = mySplitResult[0];
            var CD = document.getElementById("chartDiv");
            CD.innerHTML = "";
            if (chartAction != 'OrgChgBk') {
	            document.getElementById('selFYear').style.display= 'block';
	        }
            if (mySplitResult[1] != undefined) {
                if (mySplitResult[1].substring(3,0) == "Emi") {
                    chartHeadingHTML = mySplitResult[1];
                    CD.innerHTML = "";
                    myChartSeries = undefined;
                    showHideLoading("hide");
                }
                else{
                    myChartSeries = mySplitResult[1];
                }
            }
            document.getElementById('chartHeading').innerHTML = chartHeadingHTML;

            if (myChartSeries != undefined) {
               
                dijit.byId("mainTabContainer").selectChild(dijit.byId("resultsTab"));
                CD.innerHTML = "";
                if (myChartSeries.substring(3,0) == 'pie'){//pie chart
                    CD.style.marginLeft = 'auto';
                    CD.style.height = "180px";
                    CD.style.width = "180px";
                    makePieObjects();
                }
                else{ //barchart
                    CD.style.marginLeft = "0px";
                    CD.style.height = "260px";
                    CD.style.width = "170px";           
                    makeBarObjects();
                }
            }
            else {
                dijit.byId("mainTabContainer").selectChild(dijit.byId("resultsTab"));
                CD.innerHTML = "";
                CD.style.height = "1px";
                CD.style.width = "1px";
                showHideLoading("hide");
            }
}

function callChart(action)
    {
    if (action!=undefined) {	
       if (currentBuilding!=undefined) 
	    {
	       document.getElementById('toolLinksDiv').style.visibility = 'visible';
	        showHideLoading("show");
	        document.getElementById('resultsContent').style.display='block';
	        var FYear = document.getElementById("selFYear").value;
	        queryString = "?Action=" + action + "&bldg=" + currentBuilding + "&FY=" + FYear;
	        var lPerf = document.getElementById("perfLink");
	        var lOwn = document.getElementById("ownLink");
	        var lOperate = document.getElementById("operateLink");
	        var lUtilCC = document.getElementById("utilCCLink");
	        var lUtilMo = document.getElementById("utilMoLink");
	        document.getElementById('chartHeading').innerHTML = "";
	        document.getElementById('selFYear').style.display ='none';
	        document.getElementById('chartDiv').innerHTML = "";
	        document.getElementById('ResultsDiv').innerHTML = "";
	        if (action == "Own") {
		        chartHeadingHTML = 'Total Cost of Ownership (TCO)<br/> Facility ' + currentBuilding ;
		        lPerf.style.backgroundColor = '#ebebeb';
		        lOwn.style.backgroundColor = "#ffffff";
		        lOperate.style.backgroundColor = "#ebebeb";
		        lUtilCC.style.backgroundColor = "#ebebeb";
		        lUtilMo.style.backgroundColor = "#ebebeb";
		        document.getElementById('Owner').checked = true;
		        qlayer = 'Owner';
		        myChartSeries = undefined;
	        }
	        else 
		        if (action == "Operate") {
			        chartHeadingHTML = 'Operational Cost<br/> Facility ' + currentBuilding ;
			        lPerf.style.backgroundColor = "#ebebeb";
			        lOwn.style.backgroundColor = "#ebebeb";
			        lOperate.style.backgroundColor = "#ffffff";
			        lUtilCC.style.backgroundColor = "#ebebeb";
			        lUtilMo.style.backgroundColor = "#ebebeb";
			        
			        document.getElementById('Occup').checked = true;
			        qlayer = 'Occup';
			        myChartSeries = undefined;
		        }
	        else 
		        if (action == "Perf") {
			        chartHeadingHTML = 'Building Performance<br/> Facility ' + currentBuilding ;
			        lPerf.style.backgroundColor = "#ffffff";
			        lOwn.style.backgroundColor = "#ebebeb";
			        lOperate.style.backgroundColor = "#ebebeb";
			        lUtilCC.style.backgroundColor = "#ebebeb";
			        lUtilMo.style.backgroundColor = "#ebebeb";
			        document.getElementById('Perf').checked = true;
			        myChartSeries = undefined;
			        document.getElementById('chartDiv').innerHTML = "";
			        qlayer = 'Perf';
		        }
           else 
		        if (action == "EnergyMo") {
			        chartHeadingHTML = 'Monthly Energy Use<br/> Facility ' + currentBuilding ;
			        lPerf.style.backgroundColor = "#ebebeb";
			        lOwn.style.backgroundColor = "#ebebeb";
			        lOperate.style.backgroundColor = "#ebebeb";
			        lUtilCC.style.backgroundColor = "#ebebeb";
			        lUtilMo.style.backgroundColor = "#ffffff";
			        document.getElementById('EnergyMo').checked = true;
			        qlayer = 'EnergyMo';
			        myChartSeries = undefined;
		        }
	        else 
		        if (action == "EnergyCC") {
			        chartHeadingHTML ='Energy Use by Cat Code<br/> Facility ' + currentBuilding ;
			        lPerf.style.backgroundColor = "#ebebeb";
			        lOwn.style.backgroundColor = "#ebebeb";
			        lOperate.style.backgroundColor = "#ebebeb";
			        lUtilCC.style.backgroundColor = "#ffffff";
			        lUtilMo.style.backgroundColor = "#ebebeb";
			        document.getElementById('EnergyCC').checked = true;
			        qlayer = 'EnergyCC';
			        myChartSeries = undefined;
		        }
		    	        else 
		        if (action == "OrgChgBk") {
		        myChartSeries = undefined;
			        chartHeadingHTML ='Organization Chargeback<br/> Facility ' + currentBuilding ;
			        //document.getElementById('EnergyCC').checked = true;
			        document.getElementById('toolLinksDiv').style.visibility = 'hidden';
			        document.getElementById("chartDiv").innerHTML = "";
			        qlayer = 'OrgChgBk';
			        myChartSeries = undefined;
		        }
	         else 
		         {
			        document.getElementById('chartFY').innerHTML = '';
			        document.getElementById('chartHeading').innerHTML = '';
			        lPerf.style.backgroundColor = "#ebebeb";
			        lOwn.style.backgroundColor = "#ebebeb";
			        lOperate.style.backgroundColor = "#ebebeb";
			        lUtilCC.style.backgroundColor = "#ebebeb";
			        lUtilMo.style.backgroundColor = "#ebebeb";
			        myChartSeries = undefined;
		        }
	        doXHRCall();
	     }
	     else 
		    if (action == "EUs") {
			    showHideLoading("show");
			     document.getElementById('resultsContent').style.display='block';
		        document.getElementById('toolLinksDiv').style.visibility = 'hidden';	
			    //chartHeadingHTML = ' Emission Unit <br/> ' + currentFeature;
    //			lPerf.style.backgroundColor = "#ebebeb";
    //			lOwn.style.backgroundColor = "#ebebeb";
    //			lOperate.style.backgroundColor = "#ffffff";
    //			lUtilCC.style.backgroundColor = "#ebebeb";
    //			lUtilMo.style.backgroundColor = "#ebebeb";
			    //document.getElementById('EUs').checked = true;
			    var FYear = document.getElementById("selFYear").value;
			    queryString = "?Action=" + action + "&EUID=" + currentFeature + "&FY=" + FYear;
			    qlayer = "EUs";
			    myChartSeries = undefined;
			    doXHRCall();			
		    }
	     }
}

function showHideResults(showOrHide){
            var cht = document.getElementById("chartDiv");
            var lnks = document.getElementById("toolLinksDiv");
            var resu = document.getElementById("ResultsDiv");
            if (showOrHide == true) {
               cht.style.display = "block";
               lnks.style.display = "block";
               resu.style.display = "block";
            }
            else {
               cht.style.display = 'none';
               lnks.style.display = 'none';
               resu.style.display = 'none';
            }
}
       
function buildOrgString(orgCtrl){
    console.log("buildOrgString");
    var orgCMD = dojo.byId("cmdSelect");
    var orgWING = dojo.byId("wingSelect");
    var orgGROUP = dojo.byId("grpSelect");
    var orgSQUAD = dojo.byId("squadSelect");
    var orgFLIGHT = dojo.byId("flightSelect");
    var s
    switch (orgCtrl) {
        case "CMD" :  s= orgCMD.value + "!!!!"; break;
        case "WING": s=orgCMD.value + '!' + orgWING.value + '!!!'; break;
        case "GROUP": s= orgCMD.value + '!' + orgWING.value + '!' + orgGROUP.value + '!!'; break;
        case "SQUAD": s=orgCMD.value + '!' + orgWING.value + '!' + orgGROUP.value + '!' + orgSQUAD.value + '!'; break;
        case "FLIGHT": s= orgCMD.value + '!' + orgWING.value + '!' + orgGROUP.value + '!' + orgSQUAD.value + '!' + orgFLIGHT.value; break; 
        default: s="!!!!";
    }
    console.log(s);
    s = s + "&orgsFiltered=" + document.getElementById("chkFilter").checked;
    s = "?Action=OrgCbo&orgs=" + s;
    console.log(s);      
    doOrgXHRCall(s);        
}
        
function doOrgXHRCall(s){
    dojo.xhrGet({
        url: "./Service.asmx/getFMLWData" + s,
        handleAs: "json",
        contentType: "application/json; charset=utf-8",
        load: function (data, args) {
            console.log(data.d);
            eval(data.d);
        },
        error: function (error, args) {
            console.error("Error in method doOrgXHRCall", error);
        }
    });
}        
 
function clearDisableCbo(cboName){
    //clear out value, disable and hide control
    console.log('disable: ' + cboName);
    var cboCtl = dijit.byId(cboName);
    cboCtl.attr('value', '');
    cboCtl.attr('disabled', true);
    cboCtl.domNode.style.display = 'none';
    if (cboName == 'cmdSelect') {
        document.getElementById('dChkFilter').style.display = 'none';
    }    
}
 
function buildOrgCosts(){
    var orgCMD = dojo.byId("cmdSelect");
    var orgWING = dojo.byId("wingSelect");
    var orgGROUP = dojo.byId("grpSelect");
    var orgSQUAD = dojo.byId("squadSelect");
    var orgFLIGHT = dojo.byId("flightSelect");
    var sOrgs = orgCMD.value + '!' + orgWING.value + '!' + orgGROUP.value + '!' + orgSQUAD.value + '!' + orgFLIGHT.value;
    sOrgs = sOrgs + "&orgsFiltered=" + document.getElementById("chkFilter").checked;
    sOrgs = "?Action=OrgCost&orgs=" + sOrgs;
    doOrgXHRCall(sOrgs);
}

function showOrgLoadingGIF(showOrHide){
    if (showOrHide = 'hide'){
    document.getElementById("divOrgCharge").innerHTML = "<br/><br/><br/><br/><br/><img src='./images/loadinfoCircle48.gif' alt='' style='margin-left: 45%; margin-right: 50%;'/>";
    } 
    else
    {
    }
}

function searchMapOrgRooms(){
    if (numRoomsFromOrgSearch < 1000){
        searchQ(dojo.byId('cmdSelect').value + ',' + dojo.byId('wingSelect').value + ',' + dojo.byId('grpSelect').value + ',' + dojo.byId('squadSelect').value + ',' + dojo.byId('flightSelect').value);
	}
}