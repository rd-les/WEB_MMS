var MESSAGE_SYSTEM_SUCCESS = "ระบบได้ทำการบันทึกข้อมูลเรียบร้อยแล้ว";
var MESSAGE_SYSTEM_FAILURE = "ระบบไม่สามารถทำการบันทึกข้อมูลได้กรุณาติดต่อผู้ดูแลระบบ";
var MESSAGE_SYSTEM_CONFIRM = "ต้องการที่จะบันทึกข้อมูลใช่หรือไม่ ?";
var MESSAGE_SYSTEM_END_DOCUMENT = "คุณต้องการที่จะ \"สิ้นสุดหนังสือ\" ใช่หรือไม่ !!!  ";
var MESSAGE_SYSTEM_CANCEL_DOCUMENT = "คุณต้องการที่จะ \"ยกเลิกหนังสือ/ยุติหนังสือ\" ใช่หรือไม่ !!!  ";

var RESULT_SUCCESS = "SUCCESS";
var RESULT_FAILURE = "FAILURE";

var DIV_MAIN = "div_main_content" ; 

function linkMenuToPage(url) {

    if (url == "null" || url == null) {
        return false;
    }
    else if (url.substring(0, 4) == "http") {
        window.open(url, "_blank");
        return false;
    }
    $("#div_main_content").html("");
    divBgScreenShow();    
    jQuery.post(url, function (results) {
        $("#div_main_content").html(results);
        divBgScreenHide();
        //initPage();
    })
    .fail(function () {
        alert("ระบบไม่สามารถแสดงผลได้ กรุณากด \"ตกลง\" เพื่อกลับไปหน้าหลัก");
        divBgScreenHide();
        //window.open(""); 
    }); 
}

function divBgScreenShow() {
    $("#div_dim_shadow").show();
}

function divBgScreenHide() {
    $("#div_dim_shadow").hide();
}

// NOTE crate 20140501 for update once filed.
function jsUpdateOnceField(filed, value, table, where) {
    var url = "Shared/updateOnceField";
    jQuery.post(url, { action: "updateOnceField", secure: "TRUE", fieldShare: filed, valueShare: value, tableShare: table, whereShare: where }, function (results) {
        if (results.result == RESULT_FAILURE) { alert(MESSAGE_SYSTEM_FAILURE); return MESSAGE_SYSTEM_FAILURE; }
        else { alert(MESSAGE_SYSTEM_SUCCESS); return MESSAGE_SYSTEM_SUCCESS; }
    });


}


function getDateNowCalendar() {
    var date = new Date()
    var realMonth = parseInt(date.getMonth())+1;
    return date.getDate() + "/" + realMonth + "/" + date.getFullYear();
}

function convertViewBagJsonData(viewBagData) {

    var str = viewBagData.replace(/&quot;/ig, '"').replace(/\n/g, "\\\\n").replace(/\r/g, "\\\\r").replace(/\t/g, "\\\\t");    
    str = str.replace(/\\t/g, '\t');
    return str; 
}

function convertToHTML(data) {
    if (data == "null" || data == null) {
        return "";
    }
    var str = data.replace(/\\r\\n/ig, "<br/>");
    str = str.replace(/\r\n/ig, "<br/>");
    return str;
}

function convertToTextbox(data) {
    if (data == "null" || data == null) {
        return "";
    }
    var str = data.replace(/\\r\\n/ig, "\r\n");
    return str;
}
/*
Params
1=INPUT_NAME
2=TRUE is only number, FALSE accept string.
3=TRUE Use alert() function for display alert message.
4=Message string, in cause of empty value this this alert will show default message.
5=TRUE for compare maching input
6=Index of maching input
7=Form name

Sample
if(
    jQchkByName("INPUT_NAME1",false,true,'',false,0,"formA")
&&  jQchkByName("INPUT_NAME2",false,true,'',false,0,"formA")
){
    jQuery.post("~/Sample",jQuery("[name=formA]").serialize(),function(json){
        //SUCCESS
    },"json");
}
*/

function jQchkByName(arg, numonly, alertState, msg, maching, maching_index, objSrc) {
    var index = 0;
    var out = true;
    var check = false;
    var defaultAlert = "กรุณาระบุข้อมูล";
    var MSG_numonly = "กรุณาระบุตัวเลขเท่านั้น";
    var MSG_pleaseSelect = "กรุณาเลือก";
    var arg = arg;
    var numonly = numonly;
    var alertState = alertState;
    var msg = msg;
    var maching = maching;
    var maching_index = maching_index;

    //cause of maching value
    if (typeof (arg) == "object") {
        for (index = 0; index < arg.length; index++) {
            if (chkByName(arg[index], numonly, alertState, msg, maching, maching_index)) {
                if (jQuery(objSrc).find('[name=' + arg[0] + ']')[0].val() != jQuery(objSrc).find(arg[index])[0].val()) {
                    alert('กรุณาระบุข้อมูลให้ตรงกัน');
                    return false;
                }
            }
            else {
                return false;
            }
        }
        return true;
    }
    //cause of maching value
    if (objSrc != undefined) {
        var curentObj = jQuery(objSrc).find('[name=' + arg + ']');
    }
    else {
        var curentObj = jQuery('[name=' + arg + ']');
    }
    if (curentObj != null && curentObj.length > 0) {
        for (index = 0; index < curentObj.length; index++) {
            var jqObj = jQuery(curentObj[index]);

            if (jqObj.attr('readonly') == true
				|| jqObj.prop('disabled')
				|| jqObj.prop('type') == 'hidden'
				|| !jqObj.is(':visible')
				|| (maching && maching_index != index)
			) {
                continue;
            }

            if (jqObj != undefined) {
                if (jqObj.prop('type') == "radio") {
                    out = false;
                    if (jqObj.is(':checked')) {
                        return true;
                    }
                }
                else if (jqObj.prop('type') == "checkbox") {
                    if (jqObj.is(':checked')) {
                        return true;
                    }
                }
                else if (jqObj.prop('type') == "select-one") {
                    if ((jqObj.find('option:selected').val() + '').trim() == ''
						|| jqObj.find('option:selected').text().indexOf(MSG_pleaseSelect) != -1
					) {
                        break;
                    }
                    else {
                        continue;
                    }
                }
                else {
                    if (numonly) {
                        if (!checkNumericOnly(jqObj, '')) {
                            msg = msg + '\n' + MSG_numonly;
                            alert(msg);
                            try { jqObj.focus(); } catch (e) { }
                            //try{ jQuery(curentObj[index]).css('background-color','red'); }catch(e){}
                            return false;
                        }
                    } else if ((jqObj.val() + '').trim() == '') {
                        if (alertState) {
                            if (msg != "") {
                                alert(defaultAlert + " " + msg);
                            }
                            else {
                                alert(defaultAlert);
                            }
                        }
                        jqObj.show();
                        try { jqObj.focus(); }
                        catch (e) {
                            try { jqObj.select(); } catch (e) { }
                            //try{ jQuery(curentObj[index]).css('background-color','red'); }catch(e){}
                        }
                        return false;
                    }
                    else {
                        continue;
                    }
                }
            }
            else {
                continue;
            }
            //End for loop
        }
        if (!out) { index--; }
        var jqObj = jQuery(curentObj[index]);

        if (jqObj != undefined && !jqObj.is(':disabled')) {
            if (jqObj.prop('type') == 'radio') {
                if (alertState) {
                    if (msg != "") {
                        alert(msg);
                    }
                    else {
                        alert(defaultAlert);
                    }
                }

                try { jqObj.focus(); }
                catch (err) {
                    try { jqObj.select(); } catch (e) { }
                }
                //try{ jQuery(curentObj[index]).css('background-color','red'); }catch(e){}
                return false;
            }
            else if (jqObj.prop('type') == "checkbox" || jqObj.prop('type') == "select-one") {
                if (check) {
                    return true;
                }
                else {
                    if (alertState) {
                        if ((msg + '').trim() != "") {
                            alert(msg);
                        }
                        else {
                            alert(defaultAlert);
                        }
                    }
                    try { jqObj.focus(); }
                    catch (e) {
                        try { jqObj.select(); } catch (e) { }
                    }
                    //try{ jQuery(curentObj[index]).css('background-color','red'); }catch(e){}
                }
                return false;
            }
        }
    }
    return out;

}

function reThNo(txt) { 
    txt = txt + "";
    txt = replaceAllReg(txt, "0", "๐");
    txt = replaceAllReg(txt, "1", "๑");
    txt = replaceAllReg(txt, "2", "๒");
    txt = replaceAllReg(txt, "3", "๓");
    txt = replaceAllReg(txt, "4", "๔");
    txt = replaceAllReg(txt, "5", "๕");
    txt = replaceAllReg(txt, "6", "๖");
    txt = replaceAllReg(txt, "7", "๗");
    txt = replaceAllReg(txt, "8", "๘");
    txt = replaceAllReg(txt, "9", "๙");
    return txt;
}

function reENNo(txt) {
    txt = txt + "";
    txt = replaceAllReg(txt, "๐", "0" );
    txt = replaceAllReg(txt, "๑", "1");
    txt = replaceAllReg(txt, "๒", "2");
    txt = replaceAllReg(txt, "๓", "3");
    txt = replaceAllReg(txt, "๔", "4");
    txt = replaceAllReg(txt, "๕", "5");
    txt = replaceAllReg(txt, "๖", "6");
    txt = replaceAllReg(txt, "๗", "7");
    txt = replaceAllReg(txt, "๘", "8");
    txt = replaceAllReg(txt, "๙", "9");
    return txt;
}


function replaceAllReg(txt, replace, with_this) {
    return txt.replace(new RegExp(replace, 'g'), with_this);
    //return txt.replace(/replace/g, with_this);
}

function isValidEmail(str) {
    var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i
    return (filter.test(str));
}

function checkNumberKey(obj, event) { 
    var check = true;
    if ((event.which < 48 || event.which > 57) &&
       (event.which != 46 && event.which != 13 &&
               event.which != 8 && event.which != 9)) {
        event.returnValue = 0;
        check = false;
    }
    if (event.which == 46 && (obj.value).lastIndexOf(".") != -1) {
        event.returnValue = 0;
        check = false;
    }
    return (check);
}

function maskInput(e) {
    //check if we have "e" or "window.event" and use them as "event"
    //Firefox doesn't have window.event 
    var event = e || window.event

    var key_code = event.keyCode;
    var oElement = e ? e.target : window.event.srcElement;
    if (!event.shiftKey && !event.ctrlKey && !event.altKey) {
        if ((key_code > 47 && key_code < 58) ||
            (key_code > 95 && key_code < 106)) {

            if (key_code > 95)
                key_code -= (95 - 47);
            oElement.value = oElement.value;
        } else if (key_code == 8) {
            oElement.value = oElement.value;
        } else if (key_code != 9) {
            event.returnValue = false;
        }
    }
}



function byPassNumericOnly(event) {
    var key = null;
    if (window.event) {//IE
        key = parseInt(window.event.keyCode);
    }
    else if (event.which) {
        key = parseInt(event.which);
    }
    if ((key >= 46 && key <= 57) || (key >= 95 && key <= 105) || key == 8 || key == 9 || key == 13 || key == 190 || key == 110) {//accept ,.0123456789 Tab,backspace,Enter
        return true;
    }
    else {
        return false;
    }
}


function getKeyCode(e) {
    var code;
    if (!e) var e = window.event;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    var character = String.fromCharCode(code);
    return code;
}

function chEnter(e) {
    var code = getKeyCode(e);
    if (code == 13) return true;
    else return false;
}

function callScanner() {
    var oShell = new ActiveXObject("Shell.Application");
    var commandtoRun = "C:\\Program Files\\fiscanner\\scandall pro\\pfusdpssm.exe";
    oShell.ShellExecute(commandtoRun, "", "", "open", "1");
}

function getBrowser() {
    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        return "msie";
        //jQuery.browser.version = RegExp.$1;
    } else if (navigator.userAgent.match(/Chrome/)) {
        return "chrome";
    } else if (navigator.userAgent.match(/Firefox/)) {
        return "firefox";
    }
}

function printPage() {
    var browser = getBrowser();
    if (browser == "msie") {
        printpr();
    } else if (browser == "chrome") {
        window.print();
    } else if (browser == "firefox") {
        window.print();
    }
}

function printpr()
{
    var OLECMDID = 7;
    /* OLECMDID values:
    * 6 – print
    * 7 – print preview
    * 1 – open window
    * 4 – Save As
    */

    var PROMPT = 1; // 2 DONTPROMPTUSER
    var WebBrowser = '<OBJECT ID="WebBrowser1" WIDTH=0 HEIGHT=0 CLASSID="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></OBJECT>';
    document.body.insertAdjacentHTML('beforeEnd', WebBrowser);

    WebBrowser1.ExecWB(OLECMDID, PROMPT);
    WebBrowser1.outerHTML = "";
}

function createYearList(name, from, until) {
    if (until < from)
        return;
    if (!until) {
        until = DateTimeObj.getCurrentDate("YYYY");
    }
    /*
    $('#' + name).empty();
    $('#' + name).append($('<option>', {
        value: "",
        text: ""
    }));
    */
    for (var i = parseInt(until) ; i >= parseInt(from) ; --i) {
        $('#' + name).append($('<option>', {
            value: i + "",
            text: reThNo(i + "")
        }));
    }
    $('#' + name).val(until);
}


function serialiseObject(obj) {
    var pairs = [];
    for (var prop in obj) {
        if (!obj.hasOwnProperty(prop)) {
            continue;
        }
        if (Object.prototype.toString.call(obj[prop]) == '[object Object]') {
            pairs.push(serialiseObject(obj[prop]));
            continue;
        }
        pairs.push(prop + '=' + obj[prop]);
    }
    return pairs.join('&');
}

function getOffset(el) {
    var _x = 0;
    var _y = 0;
    while (el && !isNaN(el.offsetLeft) && !isNaN(el.offsetTop)) {
        _x += el.offsetLeft - el.scrollLeft;
        _y += el.offsetTop - el.scrollTop;
        el = el.offsetParent;
    }
    return { top: _y, left: _x };
}

function getCurrentPage(tableId) {
    if ($("#" + tableId + "_pagination").length > 0) {
        return reENNo($("#" + tableId + "_pagination").pagination('getCurrentPage'));
    } else {
        return 1;
    }
}

function loadDataToTable(tableId, url, action, dataOptions, assignFunction, pageAction, printFunc, callback) {

    var currentPage = getCurrentPage(tableId);
    if (printFunc) {

        if ($(".print_only").length) {
            $(".print_only").remove();
        }

        if (!$("#" + tableId + "_printTable").length) {
            $("<table id='" + tableId + "_printTable' class='datatable print_only'>" +
              "<thead></thead><tbody></tbody></table>").insertBefore("#" + tableId);
        }

        $("#"+tableId+"_printTable > thead").html($("#" + tableId + " > thead").html());
        tableId = tableId+"_printTable";
        currentPage = 0;
    }

    if (dataOptions && dataOptions != "") {
        var objDataOptions = JSON.parse('{"' + decodeURI(dataOptions).replace(/"/g, '\\"').replace(/&/g, '","').replace(/=/g, '":"') + '"}');
        if (!objDataOptions.page) {
            objDataOptions.page = currentPage;
            dataOptions = serialiseObject(objDataOptions);
        }
    } else {
        dataOptions = "page=" + currentPage;
    }

    $.ajax({
        type: "POST",
        url: url,
        async: false,
        data: "action=" + action + (dataOptions != "" ? "&" + dataOptions : ""),
        success: function (results) {

            var isPaging = false;
            $("#" + tableId + " > tbody").empty();

            // Count Column
            var cols = $("#" + tableId + " > thead").find("tr:first th");
            var colCount = 0;
            for (var i = 0; i < cols.length; i++) {
                var colspan = cols.eq(i).attr("colspan");
                if (colspan && colspan > 1) {
                    colCount += colspan;
                } else {
                    colCount++;
                }
            }
            // Find tfoot, if not found, generate it, prepare for paging
            if (!$("#" + tableId).find('tfoot').length) {
                $('<tfoot>').appendTo("#" + tableId);
            } else {
                $("#" + tableId + " > tfoot").empty();
            }

            // Paging
            if (results.currentPage !== undefined && results.count !== undefined && results.pageSize !== undefined && printFunc==null) {
                isPaging = true;

                if (results.count > results.pageSize) {
                   
                    // Write paging
                    $("#" + tableId + " > tfoot").append($("<tr><td colspan=" + colCount + " style='text-align:right;padding:5px;'><img id='" + tableId + "_pageloader' src='../Images/ajax-loader.gif' style='margin-top:5px;margin-right:5px;display:none;' /><div id='" + tableId + "_pagination' style='float:right;'></div></td></tr>"));
                    $("#" + tableId + "_pagination").pagination({
                        items: results.count,
                        itemsOnPage: results.pageSize,
                        currentPage: results.currentPage,
                        cssStyle: 'compact-theme',
                        onPageClick: function (page, event) {
                            divBgScreenShow();
                            $("#" + tableId + "_pageloader").show();
                            var objDataOptions = JSON.parse('{"' + decodeURI(dataOptions).replace(/"/g, '\\"').replace(/&/g, '","').replace(/=/g, '":"') + '"}');
                            objDataOptions.page = reENNo(page);
                            loadDataToTable(tableId, url, action, serialiseObject(objDataOptions), assignFunction, true, printFunc, callback);
                        }
                    });
                }
            }

            // Write Data
            var oldGroup = 1;
            var rowCount = 0;
            $.each(results.list, function (key, val) {
                rowCount += 1;
               // if (Math.ceil(rowCount / 10) != oldGroup) {
               //     $("#" + tableId + " > tbody").append("<tr><td colspan=" + colCount + "><div class='page-break'></div></td></tr>");
               //     oldGroup = Math.ceil(rowCount / 10);
               // }
                assignFunction(tableId, (isPaging ? ((results.currentPage - 1) * results.pageSize) + rowCount : rowCount), val);
            });

            // Write Label If Count = 0
            if (rowCount == 0) {
                $("#" + tableId + " > tbody").append($("<tr><td colspan=" + colCount + " style='text-align:center;padding:5px;'>ไม่มีรายการข้อมูล</td></tr>"));
            }

            $("#" + tableId + "").trigger("create");

            if (pageAction) {
                divBgScreenHide();
                $("#" + tableId + "_pageloader").hide();
                $('html, body').animate({ scrollTop: $("#" + tableId).offset().top }, "slow");
            }

            if (printFunc) {
                printFunc();
            }

            if (callback) {
                callback(results);
            }
        }
    });
    
}




function loadDataToTableArray(tableId, url, action, dataOptions, assignFunction, pageAction, printFunc, callback) {

    var currentPage = getCurrentPage(tableId);
    if (printFunc) {

        if ($(".print_only").length) {
            $(".print_only").remove();
        }

        if (!$("#" + tableId + "_printTable").length) {
            $("<table id='" + tableId + "_printTable' class='datatable print_only'>" +
              "<thead></thead><tbody></tbody></table>").insertBefore("#" + tableId);
        }

        $("#" + tableId + "_printTable > thead").html($("#" + tableId + " > thead").html());
        tableId = tableId + "_printTable";
        currentPage = 0;
    }

   /*
    if (dataOptions && dataOptions != "") {
        var objDataOptions = JSON.parse(decodeURI(dataOptions).replace(/"/g, '\\"').replace(/&/g, '","').replace(/=/g, '":"') );
        //var objDataOptions = dataOptions;
        if (!objDataOptions.page) {
            objDataOptions.page = currentPage;
            dataOptions = serialiseObject(objDataOptions);
        }
    } else {
        dataOptions = "page=" + currentPage;
    }

   /*  */

    $.ajax({
        type: "POST",
        url: url,
        async: false,
        // data: "action=" + action + (dataOptions != "" ? "&" + dataOptions : ""), dataOptions
        data: dataOptions,
        success: function (results) {

            var isPaging = false;
            $("#" + tableId + " > tbody").empty();

            // Count Column
            var cols = $("#" + tableId + " > thead").find("tr:first th");
            var colCount = 0;
            for (var i = 0; i < cols.length; i++) {
                var colspan = cols.eq(i).attr("colspan");
                if (colspan && colspan > 1) {
                    colCount += colspan;
                } else {
                    colCount++;
                }
            }
            // Find tfoot, if not found, generate it, prepare for paging
            if (!$("#" + tableId).find('tfoot').length) {
                $('<tfoot>').appendTo("#" + tableId);
            } else {
                $("#" + tableId + " > tfoot").empty();
            }

            // Paging
            if (results.currentPage !== undefined && results.count !== undefined && results.pageSize !== undefined && printFunc == null) {
                isPaging = true;

                if (results.count > results.pageSize) {

                    // Write paging
                    $("#" + tableId + " > tfoot").append($("<tr><td colspan=" + colCount + " style='text-align:right;padding:5px;'><img id='" + tableId + "_pageloader' src='../Images/ajax-loader.gif' style='margin-top:5px;margin-right:5px;display:none;' /><div id='" + tableId + "_pagination' style='float:right;'></div></td></tr>"));
                    $("#" + tableId + "_pagination").pagination({
                        items: results.count,
                        itemsOnPage: results.pageSize,
                        currentPage: results.currentPage,
                        cssStyle: 'compact-theme',
                        onPageClick: function (page, event) {
                            divBgScreenShow();
                            $("#" + tableId + "_pageloader").show();
                            var objDataOptions = JSON.parse('{"' + decodeURI(dataOptions).replace(/"/g, '\\"').replace(/&/g, '","').replace(/=/g, '":"') + '"}');
                            objDataOptions.page = reENNo(page);
                            loadDataToTable(tableId, url, action, serialiseObject(objDataOptions), assignFunction, true, printFunc, callback);
                        }
                    });
                }
            }

            // Write Data
            var oldGroup = 1;
            var rowCount = 0;
            $.each(results.list, function (key, val) {
                rowCount += 1;
                // if (Math.ceil(rowCount / 10) != oldGroup) {
                //     $("#" + tableId + " > tbody").append("<tr><td colspan=" + colCount + "><div class='page-break'></div></td></tr>");
                //     oldGroup = Math.ceil(rowCount / 10);
                // }
                assignFunction(tableId, (isPaging ? ((results.currentPage - 1) * results.pageSize) + rowCount : rowCount), val);
            });

            // Write Label If Count = 0
            if (rowCount == 0) {
                $("#" + tableId + " > tbody").append($("<tr><td colspan=" + colCount + " style='text-align:center;padding:5px;'>ไม่มีรายการข้อมูล</td></tr>"));
            }

            $("#" + tableId + "").trigger("create");

            if (pageAction) {
                divBgScreenHide();
                $("#" + tableId + "_pageloader").hide();
                $('html, body').animate({ scrollTop: $("#" + tableId).offset().top }, "slow");
            }

            if (printFunc) {
                printFunc();
            }

            if (callback) {
                callback(results);
            }
        }
    });




}