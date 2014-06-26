Imports System.Collections

Public Class SignupNewsletter
    Inherits SignupNewsletterBase

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal signupCode As Integer)
        MyBase.New(signupCode)
    End Sub

    Public Function IsSignupUnsubFromEmail(ByVal etl As EmailTemplateLanguage) As Boolean
        Dim _isSignupUnsubFromEmail As Boolean = False
        If SignupCode > 0 AndAlso MyBase._newsletterSignup.Count > 0 Then
            For Each sn As SignupNewsletter In MyBase._newsletterSignup
                If sn.IsUnsub Then
                    Select Case sn.NewsletterType
                        Case NewsletterTypeName.MarketingEmails
                            If etl.EmailTemplate.EmailType = NewsletterTypeName.MarketingEmails Then
                                _isSignupUnsubFromEmail = True
                            End If
                        Case NewsletterTypeName.AccountEmails ' this path would be canceled
                            If etl.EmailTemplate.EmailType = NewsletterTypeName.AccountEmails Then
                                _isSignupUnsubFromEmail = True
                            End If
                            'Case NewsletterTypeName.AdwordsKeywordsReport
                            '    If etl.EmailTemplate.Name.ToLower.Contains("adwords keywords report") Then
                            '        _isSignupUnsubFromEmail = True
                            '    End If
                    End Select
                End If

            Next
        End If
        Return _isSignupUnsubFromEmail
    End Function

    Public Function IsSignupUnsubFromEmailType(ByVal emailType As SignupNewsletterBase.NewsletterTypeName) As Boolean
        Dim _isSignupUnsubFromEmail As Boolean = False
        If SignupCode > 0 AndAlso MyBase._newsletterSignup.Count > 0 Then
            For Each sn As SignupNewsletter In MyBase._newsletterSignup
                If sn.IsUnsub Then
                    Select Case sn.NewsletterType
                        Case NewsletterTypeName.MarketingEmails
                            _isSignupUnsubFromEmail = True
                        Case NewsletterTypeName.AccountEmails ' this path would be canceled
                            _isSignupUnsubFromEmail = True
                        Case NewsletterTypeName.AdwordsKeywordsReport
                            _isSignupUnsubFromEmail = True
                    End Select
                End If

            Next
        End If
        Return _isSignupUnsubFromEmail
    End Function

    Public Function UnsubSignupFromEmail(ByVal emailType As SignupNewsletterBase.NewsletterTypeName) As Boolean
        If Me.SignupCode > 0 Then
            Return MyBase.UnsubFromEmail(emailType)
        End If
    End Function

    Public Function ResubSignupToEmail(ByVal emailType As SignupNewsletterBase.NewsletterTypeName) As Boolean
        If Me.SignupCode > 0 Then
            Return MyBase.ResubToEmail(emailType)
        End If
    End Function

    Public Shared Function GetEmailTypes() As Dictionary(Of Int32, String)
        Dim DataCon As New AirArena.Data.DataConnector
        Dim EmailTypes As New Dictionary(Of Int32, String)
        Dim req As New AirArena.Data.DataRequest("List_EmailTypes")
        Dim dt As DataTable = DataCon.GetDataTable(req)
        For Each dr As DataRow In dt.Rows
            EmailTypes.Add(CType(dr("EmailTypeId"), SignupNewsletter.NewsletterTypeName), CStr(dr("EmailTypeName")))
        Next dr
        Return EmailTypes
    End Function

    Public Shared Function GetSignupsByEmail(ByVal emailAddress As String) As List(Of Marketing.Signup)
        Dim DataCon As New AirArena.Data.DataConnector()
        Dim SignupsByEmail As New List(Of Marketing.Signup)
        Dim req As New AirArena.Data.DataRequest("GET_Signup")
        req.Parameters.Add("@vc_Email", emailAddress)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        For Each dr As DataRow In dt.Rows()
            Dim sign As New Marketing.Signup(dr)
            SignupsByEmail.Add(sign)
        Next
        Return SignupsByEmail
    End Function


End Class
