Imports System.Collections.Generic
Imports System.Data

Public Class TicketRecipient : Inherits TicketRecipientBase
#Region " Constructors "
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal ticketRecipientId As Integer)
        MyBase.new(ticketRecipientId)
    End Sub
    Public Sub New(ByVal ticketId As Integer, ByVal adminUserId As Integer)
        MyBase.new()
        Load(ticketId, adminUserId)
    End Sub
#End Region

#Region " Properties "
    Private mTicketRecipients As List(Of TicketRecipient)
    Public Overloads ReadOnly Property TicketRecipients() As List(Of TicketRecipient)
        Get
            If mTicketRecipients Is Nothing Then
                mTicketRecipients = New List(Of TicketRecipient)
                Dim req As New AirArena.Data.DataRequest("List_TicketRecipients")
                req.Parameters.Add("@AdminUserId", Me.AdminUserId)
                Dim dt As DataTable = DataCon.GetDataTable(req)
                For Each dr As DataRow In dt.Rows
                    mTicketRecipients.Add(New TicketRecipient(CInt(dr("TicketRecipientId"))))
                Next dr
            End If
            Return mTicketRecipients
        End Get
    End Property

    Private mTicket As Ticket
    Public ReadOnly Property Ticket() As Ticket
        Get
            If mTicket Is Nothing Then
                mTicket = New Ticket(Me.TicketId)
            End If
            Return mTicket
        End Get
    End Property

    Private mAdminUser As AdminUser
    Public ReadOnly Property AdminUser() As AdminUser
        Get
            If mAdminUser Is Nothing Then
                mAdminUser = New AdminUser(Me.AdminUserId)
            End If
            Return mAdminUser
        End Get
    End Property
#End Region

#Region "Methods"
    Protected Overloads Sub Load(ByVal ticketId As Integer, ByVal adminUserId As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_TicketRecipient")
        req.Parameters.Add("@TicketId", ticketId)
        req.Parameters.Add("@AdminUserId", adminUserId)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            Me.TicketRecipientId = CType(dr("TicketRecipientId"), Integer)
            Me.TicketId = CType(dr("TicketId"), Integer)
            Me.AdminUserId = CType(dr("AdminUserId"), Integer)
            Me.CreationDT = CType(dr("CreationDT"), Date)
            Me.UpdateDT = CType(dr("UpdateDT"), Date)
            Me.isActive = CType(dr("isActive"), Boolean)
        End If
    End Sub
#End Region
End Class
