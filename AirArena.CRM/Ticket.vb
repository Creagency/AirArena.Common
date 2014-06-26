Imports System.Data
Imports System.Collections.Generic
Imports System.IO
Imports System.Configuration

Public Class Ticket
    Protected DataCon As AirArena.Data.DataConnector
#Region "Constructors"

    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub

    Public Sub New(ByVal code As Integer)
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

    Private m_SignupCode As Integer
    Public Property SignupCode() As Integer
        Get
            Return m_SignupCode
        End Get
        Set(ByVal value As Integer)
            m_SignupCode = value
            mSignup = New Marketing.Signup(SignupCode)
        End Set
    End Property

    Private mSignup As Marketing.Signup
    Public ReadOnly Property Signup() As Marketing.Signup
        Get
            If mSignup Is Nothing Then
                mSignup = New Marketing.Signup(SignupCode)
            End If
            Return mSignup
        End Get
    End Property

    Private m_Subject As String
    Public Property Subject() As String
        Get
            Return m_Subject
        End Get
        Set(ByVal value As String)
            m_Subject = value
        End Set
    End Property

    Private m_CategoryCode As Integer
    Public Property CategoryCode() As Integer
        Get
            Return m_CategoryCode
        End Get
        Set(ByVal value As Integer)
            m_CategoryCode = value
            mCategory = New TicketCategory(CategoryCode)
        End Set
    End Property

    Private mCategory As TicketCategory
    Public ReadOnly Property Category() As TicketCategory
        Get
            If mCategory Is Nothing Then
                mCategory = New TicketCategory(CategoryCode)
            End If
            Return mCategory
        End Get
    End Property

    Private m_StatusCode As Integer
    ''' <summary>
    ''' ***If closing the Ticket, use Ticket.CloseTicket()
    ''' </summary>
    Public Property StatusCode() As Integer
        Get
            Return m_StatusCode
        End Get
        Set(ByVal value As Integer)
            m_StatusCode = value
            mStatus = New TicketStatus(value)
        End Set
    End Property

    Private mStatus As TicketStatus
    Public ReadOnly Property Status() As TicketStatus
        Get
            If mStatus Is Nothing Then
                mStatus = New TicketStatus(StatusCode)
            End If
            Return mStatus
        End Get
    End Property

    Private m_CustomerName As String
    Public Property CustomerName() As String
        Get
            Return m_CustomerName
        End Get
        Set(ByVal value As String)
            m_CustomerName = value
        End Set
    End Property

    Private m_Email As String
    Public Property Email() As String
        Get
            Return m_Email
        End Get
        Set(ByVal value As String)
            m_Email = value
        End Set
    End Property

    Private m_CountryCode As String
    Public Property CountryCode() As String
        Get
            Return m_CountryCode
        End Get
        Set(ByVal value As String)
            m_CountryCode = value
        End Set
    End Property

    Private m_AssigneeCode As Integer
    Public Property AssigneeCode() As Integer
        Get
            Return m_AssigneeCode
        End Get
        Set(ByVal value As Integer)
            m_AssigneeCode = value
        End Set
    End Property

    Private mAssignee As AdminUser
    Public ReadOnly Property Assignee() As AdminUser
        Get
            If mAssignee Is Nothing Then
                mAssignee = New AdminUser(CInt(m_AssigneeCode))
            End If
            Return mAssignee
        End Get
    End Property

    Private mTicketMessages As List(Of TicketMessage)
    Public ReadOnly Property TicketMessages() As List(Of TicketMessage)
        Get
            If mTicketMessages Is Nothing Then
                mTicketMessages = New List(Of TicketMessage)
                Dim req As New AirArena.Data.DataRequest("Find_TicketMessage")
                req.Parameters.Add("@TicketId", Code)
                Dim dt As DataTable = DataCon.GetDataTable(req)
                For Each dr As DataRow In dt.Rows
                    mTicketMessages.Add(New TicketMessage(CInt(dr("i_Code"))))
                Next dr
            End If
            Return mTicketMessages
        End Get
    End Property

    Private m_DomainCode As Integer
    Public Property DomainCode() As Integer
        Get
            Return m_DomainCode
        End Get
        Set(ByVal value As Integer)
            m_DomainCode = value
        End Set
    End Property

    Private mDomain As Marketing.Domain
    Public ReadOnly Property Domain() As Marketing.Domain
        Get
            If mDomain Is Nothing Then
                mDomain = New Marketing.Domain(CInt(m_DomainCode))
            End If
            Return mDomain
        End Get
    End Property

    Private mCloseDT As Date
    Public Property CloseDT() As Date
        Get
            Return mCloseDT
        End Get
        Set(ByVal value As Date)
            mCloseDT = value
        End Set
    End Property

    Private mLanguageId As Integer
    Public Property LanguageId() As Integer
        Get
            Return mLanguageId
        End Get
        Set(ByVal value As Integer)
            mLanguageId = value
        End Set
    End Property

    Private mCreationDT As Date
    Public Property CreationDT() As Date
        Get
            Return mCreationDT
        End Get
        Set(ByVal value As Date)
            mCreationDT = value
        End Set
    End Property

    Private mLastModifiedDT As Date
    Public ReadOnly Property LastModifiedDT() As Date
        Get
            If mLastModifiedDT = Nothing Then
                If TicketMessages.Count > 0 Then
                    mLastModifiedDT = TicketMessages(TicketMessages.Count - 1).DateModified
                Else
                    mLastModifiedDT = Me.CreationDT
                End If
            End If
                Return mLastModifiedDT
        End Get
    End Property
#End Region

#Region "Methods"

    Protected Sub Load(ByVal ticketCode As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_Ticket")
        req.Parameters.Add("@ticketId", ticketCode)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            Code = CType(dr("i_Code"), Integer)
            SignupCode = CInt(IIf(IsDBNull(dr("i_SignupCode")), Nothing, dr("i_SignupCode")))
            Subject = CType(dr("vc_Subject"), String)
            CategoryCode = CType(dr("i_CategoryCode"), Integer)
            StatusCode = CType(dr("i_StatusCode"), Integer)
            CustomerName = CStr(IIf(IsDBNull(dr("vc_Name")), Nothing, dr("vc_Name")))
            Email = CStr(IIf(IsDBNull(dr("vc_Email")), Nothing, dr("vc_Email")))
            CountryCode = CStr(IIf(IsDBNull(dr("vc_CountryCode")), Nothing, dr("vc_CountryCode")))
            AssigneeCode = CInt(IIf(IsDBNull(dr("i_AssigneeCode")), Nothing, dr("i_AssigneeCode")))
            DomainCode = CInt(IIf(IsDBNull(dr("i_DomainCode")), Nothing, dr("i_DomainCode")))
            mLanguageId = CType(dr("LanguageId"), Integer)
            CloseDT = CType(IIf(IsDBNull(dr("dt_DateClosed")), Nothing, dr("dt_DateClosed")), Date)
            mCreationDT = CType(dr("dt_DateCreated"), Date)
        End If
    End Sub

    Public Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_Ticket")
        With req.Parameters
            .Add("@TicketId", m_Code)
            .Add("@SignupCode", m_SignupCode)
            .Add("@Subject", m_Subject)
            .Add("@StatusId", m_StatusCode)
            .Add("@CategoryId", m_CategoryCode)
            .Add("@CustomerName", m_CustomerName)
            .Add("@Email", m_Email)
            .Add("@CountryCode", m_CountryCode)
            .Add("@AssigneeCode", m_AssigneeCode)
            .Add("@DomainCode", IIf(m_DomainCode = 0, Nothing, m_DomainCode))
            .Add("@CloseDT", IIf(CloseDT = Nothing, Nothing, CloseDT))
            .Add("@LanguageId", mLanguageId)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                Me.m_Code = CType(dt.Rows(0)("i_Code"), Integer)
            End If
        End With
    End Sub

    Public Sub CloseTicket()
        Me.CloseDT = Date.UtcNow
        Me.StatusCode = TicketStatus.Status.Closed
    End Sub

    Public Sub AddMessage(ByVal authorCode As Integer, ByVal messageContents As String, ByVal visibleToCustomer As Boolean,
                          Optional ByVal et As EmailTemplate.TemplateName = Nothing)

        Dim m As New TicketMessage
        With m
            .TicketCode = m_Code
            .isPhoneMessage = False
            .DisplayStatus = visibleToCustomer
            .AuthorCode = authorCode
            .Message = messageContents
            .Save()
        End With

        If et <> Nothing Then
            NotifyCustomerOfTicket(et, m)
        End If

    End Sub

    Public Sub NotifyCustomerOfTicket(ByVal et As EmailTemplate.TemplateName, ByVal m As TicketMessage)

        'If Me.SignupCode = 0 Then
        '    Dim etl As New EmailTemplateLanguage(et, Me.LanguageId)

        '    etl.Body = etl.Body.Replace("%FirstName%", Me.CustomerName)
        '    etl.Body = etl.Body.Replace("%BrandingName%", Me.Domain.Branding.Name)
        '    etl.Body = etl.Body.Replace("%Domain%", Me.Domain.Name)
        '    etl.Body = etl.Body.Replace("%domain%", Me.Domain.Name)
        '    etl.Body = etl.Body.Replace("%Article%", PCase(Me.Domain.Branding.Article))

        '    etl.Body = etl.Body.Replace("%TicketCode%", CStr(m.TicketCode))
        '    etl.Body = etl.Body.Replace("%TicketSubject%", m.Ticket.Subject)
        '    etl.Body = etl.Body.Replace("%TicketStatus%", m.Ticket.Status.DisplayName)
        '    etl.Body = etl.Body.Replace("%MessageContents%", m.Message.Replace(vbCrLf, "<br>"))

        '    etl.MailFrom = etl.MailFrom.Replace("%Domain%", Me.Domain.Name)
        '    etl.Subject = etl.Subject.Replace("%BrandingName%", Me.Domain.Branding.Name)
        '    etl.Subject = etl.Subject.Replace("%firstname%", Me.CustomerName)
        '    etl.Subject = etl.Subject.Replace("%TicketSubject%", m.Ticket.Subject)

        '    GMOMBL.Email.SendEmail(Me.Email, etl.Subject, etl.Body, etl.MailFrom, etl.MailFrom, "Ticket", Domain.Branding.Name)
        'Else
        '    Dim sign As Signup = Me.Signup
        '    Dim etl As New EmailTemplateLanguage(et, sign.LanguageId)

        '    etl.Body = etl.Body.Replace("%FirstName%", sign.FirstName)
        '    etl.Body = etl.Body.Replace("%BrandingName%", sign.Domain.Branding.Name)
        '    etl.Body = etl.Body.Replace("%GoogleLogin%", sign.GoogleLogin)
        '    etl.Body = etl.Body.Replace("%LoginName%", sign.LoginName)
        '    etl.Body = etl.Body.Replace("%Password%", sign.Password)
        '    etl.Body = etl.Body.Replace("%Domain%", sign.Domain.Name)
        '    etl.Body = etl.Body.Replace("%domain%", sign.Domain.Name)
        '    etl.Body = etl.Body.Replace("%Article%", PCase(sign.Domain.Branding.Article))

        '    etl.Body = etl.Body.Replace("%TicketCode%", CStr(m.TicketCode))
        '    etl.Body = etl.Body.Replace("%TicketSubject%", m.Ticket.Subject)
        '    etl.Body = etl.Body.Replace("%TicketStatus%", m.Ticket.Status.DisplayName)
        '    etl.Body = etl.Body.Replace("%MessageContents%", m.Message.Replace(vbCrLf, "<br>"))

        '    etl.MailFrom = etl.MailFrom.Replace("%Domain%", sign.Domain.Name)
        '    etl.Subject = etl.Subject.Replace("%BrandingName%", sign.Domain.Branding.Name)
        '    etl.Subject = etl.Subject.Replace("%firstname%", sign.FirstName)
        '    etl.Subject = etl.Subject.Replace("%TicketSubject%", m.Ticket.Subject)

        '    'etl.Subject = String.Format("Re: {0}", m.Ticket.Subject) ' replace with origianl ticket subject

        '    GMOMBL.Email.SendEmail(sign.Email, etl.Subject, etl.Body, etl.MailFrom, etl.MailFrom, "Ticket", Domain.Branding.Name)
        'End If

    End Sub

    Function PCase(ByVal strInput As String) As String
        Dim iPosition As Integer ' Our current position in the string (First character = 1)
        Dim iSpace As Integer   ' The position of the next space after our iPosition
        Dim strOutput As String = "" ' Our temporary string used to build the function's output

        iPosition = 1

        Do While InStr(iPosition, strInput, " ", CompareMethod.Text) <> 0
            iSpace = InStr(iPosition, strInput, " ", CompareMethod.Text)
            strOutput = strOutput & UCase(Mid(strInput, iPosition, 1))
            strOutput = strOutput & LCase(Mid(strInput, iPosition + 1, iSpace - iPosition))
            iPosition = iSpace + 1
        Loop

        strOutput = strOutput & UCase(Mid(strInput, iPosition, 1))
        strOutput = strOutput & LCase(Mid(strInput, iPosition + 1))

        PCase = strOutput
    End Function

#End Region

End Class