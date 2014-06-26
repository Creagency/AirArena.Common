Imports System.Data
Imports System.Collections.Generic

Public MustInherit Class TicketPhoneMessageBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal ticketMessageId As Integer)
        Me.New()
        Load(ticketMessageId)
    End Sub
#End Region
#Region " Properties "

    Private mTicketMessageId As Integer
    Public Overridable Property TicketMessageId() As Integer
        Get
            Return mTicketMessageId
        End Get
        Set(ByVal value As Integer)
            mTicketMessageId = value
        End Set
    End Property
    Private mTicketMessage As TicketMessage
    Public Overridable ReadOnly Property TicketMessage() As TicketMessage
        Get
            If mTicketMessage Is Nothing Then
                mTicketMessage = New TicketMessage(mTicketMessageId)
            End If
            Return mTicketMessage
        End Get
    End Property


    Private mDurationInMinutes As Integer
    Public Overridable Property DurationInMinutes() As Integer
        Get
            Return mDurationInMinutes
        End Get
        Set(ByVal value As Integer)
            mDurationInMinutes = value
        End Set
    End Property


    Private mDirection As String
    Public Overridable Property Direction() As String
        Get
            Return mDirection
        End Get
        Set(ByVal value As String)
            mDirection = value
        End Set
    End Property

#End Region
#Region " Methods "
    Protected Overridable Sub Load(ByVal ticketMessageId As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_TicketPhoneMessage")
        req.Parameters.Add("@ticketMessageId", ticketMessageId)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            mTicketMessageId = CType(dr("TicketMessageId"), Integer)
            mDurationInMinutes = CType(dr("DurationInMinutes"), Integer)
            mDirection = CType(dr("Direction"), String)
        End If
    End Sub
    Public Overridable Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_TicketPhoneMessage")
        With req.Parameters
            .Add("@TicketMessageId", mTicketMessageId)
            .Add("@DurationInMinutes", mDurationInMinutes)
            .Add("@Direction", mDirection)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            'If dt.Rows.Count > 0 Then
            '    'TODO: If this object represents a mulit-key table then remove this if/then condition
            '    Me.mTicketMessageId = CType(dt.Rows(0)("TicketMessageId"), Integer)
            'End If
        End With
    End Sub
#End Region
End Class

Public MustInherit Class TicketCategoryBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal ticketCategoryId As Integer)
        Me.New()
        Load(ticketCategoryId)
    End Sub
#End Region
#Region " Properties "

    Private mTickets As List(Of Ticket)
    Public Overridable ReadOnly Property Tickets() As List(Of Ticket)
        Get
            If mTickets Is Nothing Then
                mTickets = New List(Of Ticket)
                Dim req As New AirArena.Data.DataRequest("Find_Ticket")
                req.Parameters.Add("@CategoryId", mTicketCategoryId)
                Dim dt As DataTable = DataCon.GetDataTable(req)
                For Each dr As DataRow In dt.Rows
                    mTickets.Add(New Ticket(CInt(dr("TicketId"))))
                Next dr
            End If
            Return mTickets
        End Get
    End Property


    'Private mTicketFAQs As List(Of TicketFAQ)
    'Public Overridable ReadOnly Property TicketFAQs() As List(Of TicketFAQ)
    '    Get
    '        If mTicketFAQs Is Nothing Then
    '            mTicketFAQs = New List(Of TicketFAQ)
    '            Dim req As New AirArena.Data.DataRequest("Find_TicketFAQ")
    '            'req.Parameters.add("@SomethingId", SomethingId)
    '            Dim dt As DataTable = DataCon.GetDataTable(req)
    '            For Each dr As DataRow In dt.Rows
    '                mTicketFAQs.Add(New TicketFAQ(CInt(dr("TicketFAQId"))))
    '            Next dr
    '        End If
    '        Return mTicketFAQs
    '    End Get
    'End Property

    Private mTicketCategoryId As Integer
    Public Overridable Property TicketCategoryId() As Integer
        Get
            Return mTicketCategoryId
        End Get
        Protected Set(ByVal value As Integer)
            mTicketCategoryId = value
        End Set
    End Property


    Private mName As String
    Public Overridable Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property

#End Region
#Region " Methods "
    Protected Overridable Sub Load(ByVal ticketCategoryId As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_TicketCategory")
        req.Parameters.Add("@ticketCategoryId", ticketCategoryId)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            mTicketCategoryId = CType(dr("i_Code"), Integer)
            mName = CType(dr("vc_TicketCategory"), String)
        End If
    End Sub
    Public Overridable Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_TicketCategory")
        With req.Parameters
            .Add("@TicketCategoryId", mTicketCategoryId)
            .Add("@Name", mName)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                'TODO: If this object represents a mulit-key table then remove this if/then condition
                Me.mTicketCategoryId = CType(dt.Rows(0)("TicketCategoryId"), Integer)
            End If
        End With
    End Sub
#End Region
End Class


Public MustInherit Class TicketAuthorBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal i_Code As Integer)
        Me.New()
        Load(i_Code)
    End Sub
#End Region
#Region " Properties "

    Private mTicketMessages As List(Of TicketMessage)
    Public Overridable ReadOnly Property r_TicketMessages() As List(Of TicketMessage)
        Get
            If mTicketMessages Is Nothing Then
                mTicketMessages = New List(Of TicketMessage)
                Dim req As New AirArena.Data.DataRequest("Find_TicketMessage")
                'req.Parameters.add("@SomethingId", SomethingId)
                Dim dt As DataTable = DataCon.GetDataTable(req)
                For Each dr As DataRow In dt.Rows
                    mTicketMessages.Add(New TicketMessage(CInt(dr("TicketMessageId"))))
                Next dr
            End If
            Return mTicketMessages
        End Get
    End Property

    Private mCode As Integer
    Public Overridable Property i_Code() As Integer
        Get
            Return mCode
        End Get
        Protected Set(ByVal value As Integer)
            mCode = value
        End Set
    End Property


    Private mName As String
    Public Overridable Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property

#End Region
#Region " Methods "
    Protected Overridable Sub Load(ByVal i_Code As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_TicketAuthor")
        req.Parameters.Add("@TicketAuthorId", i_Code)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            mCode = CType(dr("i_Code"), Integer)
            mName = CType(dr("vc_TicketAuthor"), String)
        End If
    End Sub
    Public Overridable Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_TicketAuthor")
        With req.Parameters
            .Add("@i_Code", mCode)
            .Add("@vc_TicketAuthor", mName)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                'TODO: If this object represents a mulit-key table then remove this if/then condition
                Me.mCode = CType(dt.Rows(0)("i_Code"), Integer)
            End If
        End With
    End Sub
#End Region
End Class

Public MustInherit Class TicketStatusBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal ticketStatusId As Integer)
        Me.New()
        Load(ticketStatusId)
    End Sub
#End Region
#Region " Properties "

    Private mTickets As List(Of Ticket)
    Public Overridable ReadOnly Property Tickets() As List(Of Ticket)
        Get
            If mTickets Is Nothing Then
                mTickets = New List(Of Ticket)
                Dim req As New AirArena.Data.DataRequest("Find_Ticket")
                'req.Parameters.add("@SomethingId", SomethingId)
                Dim dt As DataTable = DataCon.GetDataTable(req)
                For Each dr As DataRow In dt.Rows
                    mTickets.Add(New Ticket(CInt(dr("TicketId"))))
                Next dr
            End If
            Return mTickets
        End Get
    End Property


    Private mTicketMessages As List(Of TicketMessage)
    Public Overridable ReadOnly Property TicketMessages() As List(Of TicketMessage)
        Get
            If mTicketMessages Is Nothing Then
                mTicketMessages = New List(Of TicketMessage)
                Dim req As New AirArena.Data.DataRequest("Find_TicketMessage")
                'req.Parameters.add("@SomethingId", SomethingId)
                Dim dt As DataTable = DataCon.GetDataTable(req)
                For Each dr As DataRow In dt.Rows
                    mTicketMessages.Add(New TicketMessage(CInt(dr("TicketMessageId"))))
                Next dr
            End If
            Return mTicketMessages
        End Get
    End Property

    Private mStatusId As Integer
    Public Overridable Property StatusId() As Integer
        Get
            Return mStatusId
        End Get
        Protected Set(ByVal value As Integer)
            mStatusId = value
        End Set
    End Property


    Private mName As String
    Public Overridable Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property


    Private mDisplayName As String
    Public Overridable Property DisplayName() As String
        Get
            Return mDisplayName
        End Get
        Set(ByVal value As String)
            mDisplayName = value
        End Set
    End Property

#End Region
#Region " Methods "
    Protected Overridable Sub Load(ByVal ticketStatusId As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_TicketStatus")
        req.Parameters.Add("@ticketStatusId", ticketStatusId)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            mStatusId = CType(dr("i_StatusCode"), Integer)
            mName = CType(dr("vc_StatusName"), String)
            'mDisplayName = CType(dr("DisplayName"), String)
        End If
    End Sub
    Public Overridable Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_TicketStatus")
        With req.Parameters
            .Add("@TicketStatusId", mStatusId)
            .Add("@Name", mName)
            .Add("@DisplayName", mDisplayName)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                'TODO: If this object represents a mulit-key table then remove this if/then condition
                Me.mStatusId = CType(dt.Rows(0)("TicketStatusId"), Integer)
            End If
        End With
    End Sub
#End Region
End Class

Public MustInherit Class TicketRecipientBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal ticketRecipientId As Integer)
        Me.New()
        Load(ticketRecipientId)
    End Sub
#End Region
#Region " Properties "

    Private mTicketRecipientId As Integer
    Public Overridable Property TicketRecipientId() As Integer
        Get
            Return mTicketRecipientId
        End Get
        Protected Set(ByVal value As Integer)
            mTicketRecipientId = value
        End Set
    End Property


    Private mTicketId As Integer
    Public Overridable Property TicketId() As Integer
        Get
            Return mTicketId
        End Get
        Set(ByVal value As Integer)
            mTicketId = value
        End Set
    End Property


    Private mAdminUserId As Integer
    Public Overridable Property AdminUserId() As Integer
        Get
            Return mAdminUserId
        End Get
        Set(ByVal value As Integer)
            mAdminUserId = value
        End Set
    End Property


    Private mCreationDT As Date
    Public Overridable Property CreationDT() As Date
        Get
            Return mCreationDT
        End Get
        Protected Set(ByVal value As Date)
            mCreationDT = value
        End Set
    End Property


    Private mUpdateDT As Date
    Public Overridable Property UpdateDT() As Date
        Get
            Return mUpdateDT
        End Get
        Protected Set(ByVal value As Date)
            mUpdateDT = value
        End Set
    End Property


    Private misActive As Boolean
    Public Overridable Property isActive() As Boolean
        Get
            Return misActive
        End Get
        Set(ByVal value As Boolean)
            misActive = value
        End Set
    End Property

#End Region
#Region " Methods "
    Protected Overridable Sub Load(ByVal ticketRecipientId As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_TicketRecipient")
        req.Parameters.Add("@ticketRecipientId", ticketRecipientId)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            mTicketRecipientId = CType(dr("TicketRecipientId"), Integer)
            mTicketId = CType(dr("TicketId"), Integer)
            mAdminUserId = CType(dr("AdminUserId"), Integer)
            mCreationDT = CType(dr("CreationDT"), Date)
            mUpdateDT = CType(dr("UpdateDT"), Date)
            misActive = CType(dr("isActive"), Boolean)
        End If
    End Sub
    Public Overridable Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_TicketRecipient")
        With req.Parameters
            .Add("@TicketRecipientId", mTicketRecipientId)
            .Add("@TicketId", mTicketId)
            .Add("@AdminUserId", mAdminUserId)
            .Add("@isActive", misActive)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                'TODO: If this object represents a mulit-key table then remove this if/then condition
                Me.mTicketRecipientId = CType(dt.Rows(0)("TicketRecipientId"), Integer)
            End If
        End With
    End Sub
#End Region
End Class