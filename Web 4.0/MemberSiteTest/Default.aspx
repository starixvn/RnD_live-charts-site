<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%=Title%></title>
    <style type="text/css"></style>
</head>
<body>
    <h4>
        <%=Title%></h4>
    Kick Off Date: 
    <input type="text" id="txtKickOffDate" value="<%=DateTime.Now.ToShortDateString()%>" />
    In early day:
    <select id="ddlNumOfEarlyDay">
        <option value="0">Just In Kick Off Date</option>
        <option value="3">Next 3 Days</option>
        <option value="5">Next 5 Days</option>
        <option value="7">Next 7 Days</option>
    </select>
    Match Status: 
    <select id="ddlMatchStatus">
        <option value="all">all</option>
        <option value="running">running</option>
        <option value="closed">closed</option>
        <option value="completed">completed</option>
    </select>
    Language:
     <select id="ddlLanguage">
        <option value="en-US">English</option>
        <option value="zh-TW">Chinese Tranditional</option>
        <option value="zh-CN">Chinese Simplified</option>
        <option value="ja-JP">Japanese</option>
        <option value="th-TH">Thai</option>
        <option value="ko-KR">Korean</option>
        <option value="vi-VN">Vietnamese</option> 
    </select>
    Show on:
    <select id="ddlShowingStatus">
        <option value="0">Only QA Site</option>
        <option value="1">Pro and QA Site</option>
    </select>
    <input type="button" value="Load Chart List" onclick="LoadChartList();" />

    <script type="text/javascript">
        function LoadChartList(matchId) {
            var kickOffDate = document.getElementById('txtKickOffDate').value;
            var matchStatus = document.getElementById('ddlMatchStatus').value;
            var language = document.getElementById('ddlLanguage').value;
            var chartShowingStatus = document.getElementById('ddlShowingStatus').value;
            var numOfEarlyDay = document.getElementById('ddlNumOfEarlyDay').value;
            window.open('LoadChartsList.aspx?kickOffDate=' + kickOffDate
            + '&matchStatus=' + matchStatus
            + '&language=' + language
            + '&chartShowingStatus=' + chartShowingStatus
            + '&numOfEarlyDay=' + numOfEarlyDay, 
            'chartListPage');
        }
    </script>
</body>
</html>
