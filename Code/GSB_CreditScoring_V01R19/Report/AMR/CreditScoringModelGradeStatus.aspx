<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master"
    AutoEventWireup="true" CodeBehind="CreditScoringModelGradeStatus.aspx.cs" Inherits="GSB.Report.FrontEnd.CreditScoringModelGradeStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <link rel="stylesheet" media="screen" href="<%=Page.ResolveUrl("~/")%>Styles/jquery-ui-1.7.2.custom.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
   <script src="<%=Page.ResolveUrl("~/")%>Scripts/jquery-ui-1.8.10.offset.datepicker.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function dpc() {
            var d = new Date();
            var toDay = d.getDate() + '/' + (d.getMonth() + 1) + '/' + (d.getFullYear() + 543);
<%--            $("#<%=txtOpenDate.ClientID%>").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', isBuddhist: true, defaultDate: toDay, dayNames: ['อาทิตย์', 'จันทร์', 'อังคาร', 'พุธ', 'พฤหัสบดี', 'ศุกร์', 'เสาร์'],
                dayNamesMin: ['อา.', 'จ.', 'อ.', 'พ.', 'พฤ.', 'ศ.', 'ส.'],
                monthNames: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
                monthNamesShort: ['ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.', 'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.', 'ก.ย.', 'ต.ค.', 'พ.ย.', 'ธ.ค.']
            });
            $("#<%=txtCloseDate.ClientID%>").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', isBuddhist: true, defaultDate: toDay, dayNames: ['อาทิตย์', 'จันทร์', 'อังคาร', 'พุธ', 'พฤหัสบดี', 'ศุกร์', 'เสาร์'],
                dayNamesMin: ['อา.', 'จ.', 'อ.', 'พ.', 'พฤ.', 'ศ.', 'ส.'],
                monthNames: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
                monthNamesShort: ['ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.', 'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.', 'ก.ย.', 'ต.ค.', 'พ.ย.', 'ธ.ค.']
            });--%>

        }
        $(document).ready(function () { dpc(); });
        prm.add_endRequest(function () { $(document).ready(function () { dpc(); }); });
    </script>
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <div id="preloader">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Cancel" EventName="Click" />            
            <asp:PostBackTrigger ControlID="btnExcel" />
        </Triggers>
        <ContentTemplate>
            <asp:Table runat="server" Width="950px" CellPadding="0" CellSpacing="0" CssClass="Search">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานการวิเคราะห์และติดตามประเมินผลการใช้งานแบบจำลองวัดระดับความเสี่ยง (Credit Scoring Model)</asp:TableHeaderCell>
                </asp:TableHeaderRow>
<asp:TableRow>
                    <asp:TableCell CssClass="subject">* ประเภทโมเดล</asp:TableCell>
                    <asp:TableCell CssClass="detail">
<asp:DropDownList ID="ddlModel" runat="server" DataTextField="MODEL_NAME" DataValueField="MODEL_CD" OnSelectedIndexChanged="LoadLoan"  AutoPostBack="true"/>
                    </asp:TableCell>
                </asp:TableRow>
<%--                <asp:TableRow>
                    <asp:TableCell CssClass="subject">Model Version</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlModelVersion" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>--%>
                
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* ประเภทสินเชื่อ</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlLoan" runat="server" DataTextField="LOAN_NAME" DataValueField="LOAN_CD" OnSelectedIndexChanged="LoadSubType" AutoPostBack="true" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* ประเภทสินเชื่อย่อย</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlSubType" runat="server" DataTextField="STYPER_NAME" DataValueField="STYPE_CD" AutoPostBack="true" OnSelectedIndexChanged="LoadMarket"  Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                 <asp:TableRow>
                    <asp:TableCell CssClass="subject">* Market Code</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlMarketFrm" runat="server" Enabled="false" DataTextField="MARKET_NAME"
                            DataValueField="MARKET_CD" />&nbsp;ถึง&nbsp;<asp:DropDownList ID="ddlMarketTo" runat="server" Enabled="false" DataTextField="MARKET_NAME"
                            DataValueField="MARKET_CD" />
                    </asp:TableCell>
                </asp:TableRow> 
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* วันที่เปิดใบคำขอสินเชื่อ</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlOpenDateMonth" runat="server" />&nbsp;<asp:DropDownList ID="ddlOpenDateYear" runat="server" />&nbsp;ถึง&nbsp;
                        <asp:DropDownList ID="ddlOpenDateMonthend" runat="server" />&nbsp;<asp:DropDownList ID="ddlOpenDateYearend" runat="server" />
<%--                        <asp:TextBox ID="txtOpenDate" runat="server" CssClass="Cal" />&nbsp;ถึง&nbsp;<asp:TextBox
                            ID="txtCloseDate" runat="server" CssClass="Cal" />--%>
                    </asp:TableCell></asp:TableRow><asp:TableFooterRow>
                    <asp:TableCell ColumnSpan="2" CssClass="action">
                        <asp:Button ID="Search" runat="server" Text="แสดงข้อมูล" OnClick="Search_Click" />&nbsp;
                        <asp:Button ID="Cancel" runat="server" Text="ล้างข้อมูล" OnClick="Cancel_Click" />
                        <asp:Button ID="Button1" runat="server" Text="พิมพ์รายงาน" OnClick="PrintReport_Click"/>&nbsp;
                        <asp:Button ID="btnExcel" runat="server" Text="ดาวน์โหลดไฟล์" OnClick="btnExcel_Click" CssClass="sent_right"/>
                    </asp:TableCell></asp:TableFooterRow></asp:Table><asp:Table ID="ResultTable" runat="server" Width="954px" CellPadding="0" CellSpacing="0" CssClass="Result" >
                <asp:TableRow>
                    <asp:TableCell CssClass="HeaderRow"><u>รายงานการวิเคราะห์และติดตามประเมินผลการใช้งานแบบจำลองวัดระดับความเสี่ยง (Credit Scoring Model)</u> : </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0" CssClass="RatingRpt">
                             <%--<asp:TableRow ID="TableRow2" runat="server">
                                <asp:TableCell Width="95px" >Score Range</asp:TableCell>
                                <asp:TableCell Width="67px" >DEV</asp:TableCell>
                                <asp:TableCell Width="67px" >%DEV</asp:TableCell>
                                <asp:TableCell Width="67px" >Actual</asp:TableCell>
                                <asp:TableCell Width="67px" >%Actual</asp:TableCell>
                                <asp:TableCell Width="67px" >%Change</asp:TableCell>
                                <asp:TableCell Width="67px" >Ratio</asp:TableCell>
                                <asp:TableCell Width="67px" >WOE</asp:TableCell>
                                <asp:TableCell Width="67px" >Index</asp:TableCell>
                                <asp:TableCell Width="67px" >%Approve</asp:TableCell>
                                <asp:TableCell Width="67px" >%Reject</asp:TableCell>
                                <asp:TableCell Width="67px" >Ratio</asp:TableCell>
                                <asp:TableCell Width="67px" >WOE</asp:TableCell>
                                <asp:TableCell Width="67px" >Index</asp:TableCell>
                                <asp:TableCell Width="67px" >%Approve</asp:TableCell>
                                <asp:TableCell Width="67px" >%Reject</asp:TableCell>
                            </asp:TableRow>--%>
                            <%--<asp:TableRow ID="TableRow4" runat="server">
                                <asp:TableCell ID="TableCell21" rowpan="2" Width="201px" runat="server">
                                    <asp:Label id="Label21" text="Ascore<Br>Bureau Ascore" runat="Server" />
                                </asp:TableCell>
                            </asp:TableRow>--%>
                           <%-- <asp:TableRow ID="TableRow1" runat="server">
                                <asp:TableCell RowSpan="2" runat="server" Width="155px" >
                                    <asp:Label id="Label21" text="Ascore<br>Bureau Ascore"  runat="Server" />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell1" colspan="3" runat="server" Width="156px" >
                                    <asp:Label id="Label1" text="ความเสี่ยงต่ำ(A)" runat="Server" />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell4" colspan="3" runat="server" Width="156px">
                                    <asp:Label id="Label4" text="ความเสี่ยงปานกลาง(B)" runat="Server"  />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell9" colspan="3" runat="server">
                                    <asp:Label id="Label9" text="ความเสี่ยงค่อนเข้างสูง(C)" runat="Server" Width="156px" />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell13" colspan="3" runat="server">
                                    <asp:Label id="Label13" text="ความเสี่ยงสูง(D)" runat="Server" Width="156px" />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell17" colspan="3" runat="server" Width="156px" >
                                    <asp:Label id="Label17" text="รวม" runat="Server" />
                                </asp:TableCell>
                            </asp:TableRow>--%>

                            <%--<asp:TableRow ID="TableRow2" runat="server">
                                
                                <asp:TableCell ID="TableCell2" runat="server" Width="53px">
                                    <asp:Label id="Label2" text="PL" runat="Server" />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell3" runat="server" Width="53px">
                                    <asp:Label id="Label3" text="NPLs" runat="Server"  />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell8" runat="server" Width="53px">
                                    <asp:Label id="Label8" text="รวม" runat="Server"  />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell5" runat="server" Width="53px">
                                    <asp:Label id="Label5" text="PL" runat="Server"  />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell6" runat="server" Width="53px">
                                    <asp:Label id="Label6" text="NPLs" runat="Server"  />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell7" runat="server" Width="53px">
                                    <asp:Label id="Label7" text="รวม" runat="Server" />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell10" runat="server" Width="53px">
                                    <asp:Label id="Label10" text="PL" runat="Server" />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell11" runat="server" Width="53px">
                                    <asp:Label id="Label11" text="NPLs" runat="Server"  />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell12" runat="server" Width="53px">
                                    <asp:Label id="Label12" text="รวม" runat="Server"  />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell14" runat="server" Width="53px">
                                    <asp:Label id="Label14" text="PL" runat="Server" />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell15" runat="server" Width="53px">
                                    <asp:Label id="Label15" text="NPLs" runat="Server"  />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell16" runat="server" Width="53px">
                                    <asp:Label id="Label16" text="รวม" runat="Server"  />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell18" runat="server" Width="53px">
                                    <asp:Label id="Label18" text="PL" runat="Server" />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell19" runat="server" Width="53px">
                                    <asp:Label id="Label19" text="NPLs" runat="Server" />
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell20"  runat="server" Width="53px">
                                    <asp:Label id="Label20" text="รวม" runat="Server"  />
                                </asp:TableCell>
                            </asp:TableRow>--%>
                            

                            
                          <asp:TableRow ID="TableRow3" runat="server">
                                    <asp:TableCell  BorderWidth="0">                                        
                        <asp:GridView ID="gvTable" 
                                      runat="server" 
                                      ShowHeader="True" 
                                      AutoGenerateColumns="false"
                                      CssClass="RatingRpt"  
                                      PagerSettings-Mode="NumericFirstLast" 
                                      AllowPaging="True" 
                                      OnPageIndexChanging="gvTable_PageChanging" 
                                      PageSize="15" 
                                      ShowFooter="True" 
                                      OnRowDataBound="gvTable_RowDataBound"
                                       OnRowCreated="gvTable_RowCreated"
                                      >
                        <Columns >
<%--                        <asp:TemplateField>  
                            <HeaderTemplate>  
                                <tr id="ctl00_Content_TableRow1">
                                    <td style="width:0px"></td>
				                    <td style="width: 196px;" rowspan="2"><span id="ctl00_Content_Label21">Ascore</span></td>
                                    <td id="ctl00_Content_TableCell1" style="width: 184px;" colspan="2"><span id="ctl00_Content_Label1">APPROVE</span></td>
                                    <td id="ctl00_Content_TableCell4" style="width: 184px;" colspan="2"><span id="ctl00_Content_Label4">REJECT</span></td>
                                    <td id="ctl00_Content_TableCell9" style="width: 184px;"colspan="2"><span id="ctl00_Content_Label9" style="width: 156px; display: inline-block;">อื่นๆ</span></td>
                                    <td id="ctl00_Content_TableCell13" colspan="2"><span id="ctl00_Content_Label13" style="width: 184px; display: inline-block;">รวมทั้งหมด</span></td>
			                    </tr>
                                <tr id="ctl00_Content_TableRow2">
                                    <td style="width:0px" ></td>
				                    <td id="ctl00_Content_TableCell2" style="width: 92px;"><span id="ctl00_Content_Label2">ราย</span></td>
                                    <td id="ctl00_Content_TableCell3" style="width: 92px;"><span id="ctl00_Content_Label3">ร้อยละ</span></td>
                                    <td id="ctl00_Content_TableCell8" style="width: 92px;"><span id="ctl00_Content_Label8">ราย</span></td>
                                    <td id="ctl00_Content_TableCell5" style="width: 92px;"><span id="ctl00_Content_Label5">ร้อยละ</span></td>
                                    <td id="ctl00_Content_TableCell6" style="width: 92px;"><span id="ctl00_Content_Label6">ราย</span></td>
                                    <td id="ctl00_Content_TableCell7" style="width: 92px;"><span id="ctl00_Content_Label7">ร้อยละ</span></td>
                                    <td id="ctl00_Content_TableCell10" style="width: 92px;"><span id="ctl00_Content_Label10">ราย</span></td>
                                    <td id="ctl00_Content_TableCell11" style="width: 92px;"><span id="ctl00_Content_Label11">ร้อยละ</span></td>
                                    
			                    </tr>   
                            </HeaderTemplate>  
                             <ItemTemplate>  
                                <td ><%# Eval("SUBJECT")%></td>  
                                <td><%# Eval("APPROVENUMBER")%></td>  
                               <td><%# Eval("APPROVEPERCENT")%></td>  
                              <td><%# Eval("REJECTNUMBER")%></td>  
                              <td><%# Eval("REJECTPERCENT")%></td>  
                              <td><%# Eval("OTHERNUMBER")%></td>  
                               <td><%# Eval("OTHERPERCENT")%></td>  
                              <td><%# Eval("TOTALNUMBER")%></td>  
                              <td><%# Eval("TOTALPERCENT")%></td> 
                              
                            </ItemTemplate>  
                  
            </asp:TemplateField>  	--%>				
                        <asp:BoundField DataField="SUBJECT" ItemStyle-Width="200px" DataFormatString="{0:N2}" HtmlEncode="false" headertext="Cutoff Grade" HeaderStyle-Font-Bold="true"  />
                        <asp:BoundField DataField="APPROVENUMBER" ItemStyle-Width="53px" DataFormatString="{0:N2}" HtmlEncode="false" headertext="ราย" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="APPROVEPERCENT" ItemStyle-Width="53px" DataFormatString="{0:N0}" HtmlEncode="false" headertext="ร้อยละ" HeaderStyle-Font-Bold="true"  />
                        <asp:BoundField DataField="REJECTNUMBER" ItemStyle-Width="53px" DataFormatString="{0:N2}" HtmlEncode="false" headertext="ราย" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="REJECTPERCENT" ItemStyle-Width="53px" DataFormatString="{0:N0}" HtmlEncode="false" headertext="ร้อยละ" HeaderStyle-Font-Bold="true"  />
                        <asp:BoundField DataField="OTHERNUMBER" ItemStyle-Width="53px" DataFormatString="{0:N2}" HtmlEncode="false" headertext="ราย" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="OTHERPERCENT" ItemStyle-Width="53px" DataFormatString="{0:N2}" HtmlEncode="false" headertext="ร้อยละ" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="TOTALNUMBER" ItemStyle-Width="53px" DataFormatString="{0:N2}" HtmlEncode="false" headertext="ราย" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="TOTALPERCENT" ItemStyle-Width="53px" DataFormatString="{0:N2}" HtmlEncode="false" headertext="ร้อยละ" HeaderStyle-Font-Bold="true" />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell></asp:TableRow><asp:TableFooterRow HorizontalAlign="Center">
            </asp:TableFooterRow></asp:Table></ContentTemplate></asp:UpdatePanel></asp:Content>