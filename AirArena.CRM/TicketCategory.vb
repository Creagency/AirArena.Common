Imports System.Collections.Generic
Imports System.Data

Public Class TicketCategory : Inherits TicketCategoryBase
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal ticketCategoryId As Integer)
        MyBase.new(ticketCategoryId)
    End Sub
    Public Sub New(ByVal name As String)
        MyBase.New()
        Load(name)
    End Sub

#Region " Properties "
    Public Enum Type
        GeneralEnquiry = 1
        ICannotSeeMyAdd
        LoginProblems
        DailyBudgetClickCosts
        PaymentsandBilling
        PaidProblems
        Other
        SalesEnquiry
        Unsubscribes
        PaymentIssues
        AdwordsChanges
        InternalMail
        AWASalesEnquiry
        AutoResponder
        WelcomeCall
        AccountSetup10Days
        NonBillableLead
        GoogleBudgetIssue
        GoogleZeroTraffic
        WelcomeCall_Ebay
        WelcomeCall_Premium
        RefundRequest
        PauseAdwords
        WPM_GeneralEnquiry
    End Enum

    Private mTicketCategories As List(Of TicketCategory)
    Public Overloads ReadOnly Property TicketCategories(Optional ByVal DisplayStatus As String = "1,2") As List(Of TicketCategory)
        Get
            If mTicketCategories Is Nothing Then
                mTicketCategories = New List(Of TicketCategory)
                Dim req As New AirArena.Data.DataRequest("List_TicketCategories")
                req.Parameters.Add("@DisplayStatus", DisplayStatus)
                Dim dt As DataTable = DataCon.GetDataTable(req)
                For Each dr As DataRow In dt.Rows
                    mTicketCategories.Add(New TicketCategory(CInt(dr("i_Code"))))
                Next dr
            End If
            Return mTicketCategories
        End Get
    End Property

    Private mTickets As List(Of Ticket)
    Public Overloads ReadOnly Property Tickets(ByVal status As String, ByVal languageId As Integer, _
                                Optional ByVal DateFrom As String = "", Optional ByVal DateTo As String = "") As List(Of Ticket)
        Get
            If mTickets Is Nothing Then
                mTickets = New List(Of Ticket)
                Dim req As New AirArena.Data.DataRequest("List_Tickets")
                req.Parameters.Add("@CategoryId", TicketCategoryId)
                req.Parameters.Add("@Status", status)
                req.Parameters.Add("@LanguageId", languageId)
                If Not (DateFrom.Equals("") Or DateTo.Equals("")) Then
                    req.Parameters.Add("@DateFrom", DateFrom)
                    req.Parameters.Add("@DateTo", DateTo)
                End If
                Dim dt As DataTable = DataCon.GetDataTable(req)
                For Each dr As DataRow In dt.Rows
                    mTickets.Add(New Ticket(CInt(dr("i_Code"))))
                Next dr
            End If
            Return mTickets
        End Get
    End Property

#End Region

#Region "Methods"
    Protected Overloads Sub Load(ByVal name As String)
        Dim req As New AirArena.Data.DataRequest("Get_TicketCategory")
        req.Parameters.Add("@Name", name)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            Me.TicketCategoryId = CType(dr("i_Code"), Integer)
            Me.Name = CType(dr("vc_TicketCategory"), String)
        End If
    End Sub

    Shared Function FillCategoryDropDown() As List(Of TicketCategory)
        Return New TicketCategory().TicketCategories
    End Function
#End Region
End Class
