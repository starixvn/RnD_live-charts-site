<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoadChartsList.aspx.cs" Inherits="LoadChartsList" %>
<script runat="server">  
//http://localhost:12247/MemberSiteTest/Default.aspx?kickOffDate=2013-07-16&matchStatus=closed
    public Fanex.LiveCharts.ClientAuthentication.Models.ChartInfo[] Charts;
    public DateTime kickOffDate;
    public string Language;
    public byte earlyDay = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        kickOffDate = DateTime.Now;
        Language = "en-US";
        if (Request["kickOffDate"] != null)
        {
            kickOffDate = DateTime.Parse(Request["kickOffDate"].ToString());
        }
        string matchStatus = "all";
        if(Request["matchStatus"] != null)
        {
            matchStatus = Request["matchStatus"].ToString();
        }
        if (Request["language"] != null)
        {
            Language = Request["language"];
        }
        Fanex.LiveCharts.ClientAuthentication.Core.ChartShowingType chartShowingStatus = Fanex.LiveCharts.ClientAuthentication.Core.ChartShowingType.ShowOnProAndQASite;
        if (Request["chartShowingStatus"] != null)
        {
            chartShowingStatus = (Fanex.LiveCharts.ClientAuthentication.Core.ChartShowingType)Enum.Parse(typeof(Fanex.LiveCharts.ClientAuthentication.Core.ChartShowingType), Request["chartShowingStatus"].ToString());
        }
        if (Request["earlyDay"] != null)
        {
            earlyDay = byte.Parse(Request["earlyDay"]);
        }
        Title = "Member Site Test";
        ////new DateTime(2013, 6, 17)
        Charts = Fanex.LiveCharts.ClientAuthentication.LiveChartsTokenService.GetCharts(
            kickOffDate, 
            -4, 
            0, 
            matchStatus,
            chartShowingStatus,
            earlyDay);
    }
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div>
        <label for="memberId">MemberId:</label>
        <input type="text" name="memberId" id="memberId" value="1"/>
    </div>
    <h4>
        Chart List on
        <%= kickOffDate.ToShortDateString()%>
        :
    </h4>
    <% if (Charts != null && Charts.Length > 0)
       {
           foreach (Fanex.LiveCharts.ClientAuthentication.Models.ChartInfo chart in Charts)
           {   
        %>
        <ul>
            <li>Status:
                <%=chart.MatchStatus.ToString() %></li>
            <li>Match Id:
                <%=chart.MatchId.ToString()%></li>
            <li>Kick Off Time:
                <%=chart.KickOffTime.ToString()%></li>
                    <% if (chart.BetTypeList != null && chart.BetTypeList.Length > 0)
                       {
                           foreach (Fanex.LiveCharts.ClientAuthentication.Models.BetTypeInfo betType in chart.BetTypeList)
                           {   
                    %>
                        <li>Bet Type Id: <%=betType.BetTypeId %></li>
                        <ul>
                        <% if (betType.OddsIds != null && betType.OddsIds.Length > 0)
                           {
                               foreach (int oddsId in betType.OddsIds)
                               {  
                                    %>
                                         <li>Odds Id: <%=oddsId.ToString()%></li>
                                    <%
                                }
                            }%>
                            </ul>
                        <%
                        }
                    }%>
           
        </ul>
    <div>
        <input type="button" value="View Chart <%=chart.MatchId.ToString()%>" onclick="LoadChart(<%=chart.MatchId.ToString()%>,'<%=Language%>')" />
    </div>
    <%
        }
    }
       else
       {
    %>
    <div>
        There is no data available !</div>
    <%
    }
    %>
    <script type="text/javascript">
        function LoadChart(matchId, language) {
            var memberId = document.getElementById('memberId').value;
            if (isNaN(memberId)) {
                memberId = -1;
            }
            window.open('LoadChart.aspx?matchId=' + matchId
            + '&language=' + language
            + '&memberId=' + memberId, '' + matchId, 'width=1000,height=690,directories=0,titlebar=0,toolbar=0,location=0,status=0,menubar=0,scrollbars=no,resizable=no');
        }
    </script>
</body>
</html>
