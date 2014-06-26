Imports System.Data

Public Class TicketMessage
    Protected DataCon As AirArena.Data.DataConnector
#Region "Constructors"

    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub

    'Public Sub New(ByVal signupCode As Integer, ByVal subject As String, ByVal categoryCode As Integer, ByVal statusCode As Integer, ByVal assigneeCode As Integer, ByVal message As String)
    '    Load(signupCode, subject, categoryCode, statusCode, assigneeCode, message)
    'End Sub

    Public Sub New(ByVal code As Integer)
        'm_Code = code
        Me.New()
        Load(code)
    End Sub

#End Region

#Region "Properties"

    Private m_Code As Integer
    Public Property Code() As Integer
        Get
            Return m_Code
        End Get
        Set(ByVal value As Integer)
            m_Code = value
        End Set
    End Property

    Private m_TicketCode As Integer
    Public Property TicketCode() As Integer
        Get
            Return m_TicketCode
        End Get
        Set(ByVal value As Integer)
            m_TicketCode = value
            mTicketIsDirty = True
        End Set
    End Property

    Private mTicketIsDirty As Boolean = True
    Private mTicket As Ticket
    Public ReadOnly Property Ticket() As Ticket
        Get
            If mTicket Is Nothing OrElse mTicketIsDirty Then
                mTicket = New Ticket(m_TicketCode)
                mTicketIsDirty = False
            End If
            Return mTicket
        End Get
    End Property

    Private m_AuthorCode As Integer
    Public Property AuthorCode() As Integer
        Get
            Return m_AuthorCode
        End Get
        Set(ByVal value As Integer)
            m_AuthorCode = value
        End Set
    End Property

    Private mAuthor As TicketAuthor
    Public Property Author() As TicketAuthor
        Get
            If mAuthor Is Nothing Then
                mAuthor = New TicketAuthor(m_AuthorCode)
            End If
            Return mAuthor
        End Get
        Set(ByVal value As TicketAuthor)
            mAuthor = value
        End Set
    End Property

    Private m_Message As String
    Public Property Message() As String
        Get
            Return m_Message
        End Get
        Set(ByVal value As String)
            m_Message = value
        End Set
    End Property

    Private m_DateModified As Date
    Public Property DateModified() As Date
        Get
            Return m_DateModified
        End Get
        Set(ByVal value As Date)
            m_DateModified = value
        End Set
    End Property

    Private mDisplayStatus As Boolean
    Public Property DisplayStatus() As Boolean
        Get
            Return mDisplayStatus
        End Get
        Set(ByVal value As Boolean)
            mDisplayStatus = value
        End Set
    End Property

    Private misPhoneMessage As Boolean
    Public Property isPhoneMessage() As Boolean
        Get
            Return misPhoneMessage
        End Get
        Set(ByVal value As Boolean)
            misPhoneMessage = value
        End Set
    End Property

    Private mPhoneMessage As TicketPhoneMessage
    Public Property PhoneMessage() As TicketPhoneMessage
        Get
            If mPhoneMessage Is Nothing And isPhoneMessage Then
                mPhoneMessage = New TicketPhoneMessage(Code)
            End If
            Return mPhoneMessage
        End Get
        Set(ByVal value As TicketPhoneMessage)
            mPhoneMessage = value
        End Set
    End Property

    Private mAdminUserId As Integer
    Public Property AdminUserid() As Integer
        Get
            Return mAdminUserId
        End Get
        Set(ByVal value As Integer)
            mAdminUserId = value
        End Set
    End Property

    Private mAdminUser As AdminUser
    Public Property AdminUser() As AdminUser
        Get
            If mAdminUser Is Nothing Then
                mAdminUser = New AdminUser(AdminUserid)
            End If
            Return mAdminUser
        End Get
        Set(ByVal value As AdminUser)
            mAdminUser = value
        End Set
    End Property

#End Region

#Region "Methods"

    Protected Overridable Sub Load(ByVal ticketMessageCode As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_TicketMessage")
        req.Parameters.Add("@i_TicketMessageCode", ticketMessageCode)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            Code = CType(dr("i_Code"), Integer)
            TicketCode = CType(dr("i_TicketCode"), Integer)
            Message = CType(dr("vc_Message"), String)
            DateModified = CType(dr("dt_DateModified"), Date)
            AuthorCode = CType(dr("i_AuthorCode"), Integer)
            DisplayStatus = CType(IIf(IsDBNull(dr("i_DisplayStatus")), True, dr("i_DisplayStatus")), Boolean)
            isPhoneMessage = CType(dr("isPhoneMessage"), Boolean)
        End If
    End Sub

    Public Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_TicketMessage")
        With req.Parameters
            .Add("@i_TicketMessageCode", Code)
            .Add("@i_TicketCode", TicketCode)
            .Add("@vc_Message", Message)
            .Add("@i_Authorcode", AuthorCode)
            .Add("@DisplayStatus", DisplayStatus)
            .Add("@isPhoneMessage", isPhoneMessage)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                Me.Code = CType(dt.Rows(0)("i_Code"), Integer)
            End If
        End With
    End Sub

    Public Sub Resend()
        'Dim mailFrom As String = System.Configuration.ConfigurationManager.AppSettings("EmailNoReply") + Ticket.Signup.Domain.Name
        'GMOMBL.Email.SendEmail(Me.Ticket.Signup.Email, Me.Ticket.Subject, Me.Message, mailFrom)
    End Sub

#End Region

End Class