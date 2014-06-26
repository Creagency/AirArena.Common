Public Class EmailTemplate : Inherits EmailTemplateBase
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal emailTemplateId As Integer)
        MyBase.new(emailTemplateId)
    End Sub
 
#Region " Properties "
    Enum TemplateName
        Paid = 1
        AccountSetup
        AccountSetup_Day3
        AccountSetup_Day5
        AccountSetup_Day35
        AccountSetup_Day62
        AccountActive
        EBook
        Unsub
        TicketReceived_Sales
        TicketReceived
        Unpaid
        InvalidWebsite
        Ebay_Welcome1
        Ebay_Welcome2
        Ebay_Day3
        Paid_FPL
        AccountActive_FPL
        Paid_Premium
        AuctionMonster_Paid
        AuctionMonster_AccountSetup1
        AuctionMonster_AccountSetup2
        AuctionMonster_AccountSetup_Day3
        AuctionMonster_AccountSetup_Day5
        AuctionMonster_AccountSetup_Day15
        AuctionMonster_AccountSetup_Day50
        AuctionMonster_AccountComplete
        Refund
        Paid_Disapproved
        Ebay_Day7
        Ebay_Day15
        Ebay_Day47
        WebProfitMonster_Paid
        WebProfitMonster_Unsubscribe
        TicketCreated = 51
        TicketUpdated
        TicketUpdated_Sales
        Adwords_Keywords_Report = 55
    End Enum

    Shared ReadOnly Property EmailTemplates() As List(Of EmailTemplate)
        Get
            Dim DataCon As New AirArena.Data.DataConnector
            Dim mEmailTemplates As New List(Of EmailTemplate)
            Dim req As New AirArena.Data.DataRequest("List_EmailTemplates")
            Dim dt As DataTable = DataCon.GetDataTable(req)
            For Each dr As DataRow In dt.Rows
                mEmailTemplates.Add(New EmailTemplate(CInt(dr("EmailTemplateId"))))
            Next dr
            Return mEmailTemplates
        End Get
    End Property

    Private mLanguages As List(Of Language)
    Public ReadOnly Property Languages() As List(Of Language)
        Get
            Dim DataCon As New AirArena.Data.DataConnector
            Dim mLanguages As New List(Of Language)
            Dim req As New AirArena.Data.DataRequest("Find_EmailTemplateLanguage")
            req.Parameters.Add("@EmailTemplateId", Me.EmailTemplateId)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            For Each dr As DataRow In dt.Rows
                mLanguages.Add(New Language(CInt(dr("LanguageId"))))
            Next dr
            Return mLanguages
        End Get
    End Property

#End Region

End Class