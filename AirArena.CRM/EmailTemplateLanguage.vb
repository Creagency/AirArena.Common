Imports System.Configuration.ConfigurationManager
Imports System.Text.RegularExpressions

Public Class EmailTemplateLanguage : Inherits EmailTemplateLanguageBase
    Public Sub New()
        MyBase.New()
    End Sub
    ''' <summary>
    ''' Get email template language based on emailTemplateLanguageId. DON'T USE THIS OVERLAOD IN CMS!!!
    ''' </summary>
    ''' <param name="emailTemplateLanguageId"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal emailTemplateLanguageId As Integer)
        MyBase.new(emailTemplateLanguageId)
        loadTrackingAndTestInfo()
    End Sub

    ''' <summary>
    ''' Get email template language by Id. Control if loading split test (if live), and if merge tags
    ''' are replaced on load
    ''' </summary>
    ''' <param name="emailTemplateLanguageId"></param>
    ''' <param name="ReplaceComponentsOnLoad"></param>
    ''' <param name="GetSplitTestVariations"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal emailTemplateLanguageId As Integer, ByVal ReplaceComponentsOnLoad As Boolean,
                   ByVal GetSplitTestVariations As Boolean)
        MyBase.new(emailTemplateLanguageId)
        loadTrackingAndTestInfo(ReplaceComponentsOnLoad, GetSplitTestVariations)
    End Sub
    ''' <summary>
    ''' Get email template language based on template name and languageId. DON'T USE THIS OVERLAOD IN CMS!!!
    ''' </summary>
    ''' <param name="emailTemplateId"></param>
    ''' <param name="languageId"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal emailTemplateId As EmailTemplate.TemplateName, ByVal languageId As Integer)
        MyBase.new()
        Load(emailTemplateId, languageId, True)
        loadTrackingAndTestInfo()
    End Sub
    ''' <summary>
    ''' Use this constructor to show the email content without replacing components (In CMS)
    ''' <br>This constuctor also prevent any live split test to load (if False is passed)</br>
    ''' </summary>
    ''' <param name="emailTemplateId"></param>
    ''' <param name="languageId"></param>
    ''' <param name="ReplaceComponentsOnLoad"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal emailTemplateId As EmailTemplate.TemplateName, ByVal languageId As Integer, _
    ByVal ReplaceComponentsOnLoad As Boolean)
        MyBase.new()
        Load(emailTemplateId, languageId, ReplaceComponentsOnLoad)
        loadTrackingAndTestInfo(ReplaceComponentsOnLoad, True)
    End Sub

    Public Sub New(ByVal emailTemplateId As EmailTemplate.TemplateName, ByVal languageId As Integer, _
    ByVal ReplaceComponentsOnLoad As Boolean, ByVal GetSplitTestVariations As Boolean)
        MyBase.new()
        Load(emailTemplateId, languageId, ReplaceComponentsOnLoad)
        loadTrackingAndTestInfo(ReplaceComponentsOnLoad, GetSplitTestVariations)
    End Sub

#Region "Properties"
    Enum MailFormat
        HTML
        PlainText
    End Enum


    Private ReadOnly Property UseNonSecurePixel() As Boolean
        Get
            Dim mUseNonSecurePixel As Boolean = False
            Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings("UseNonSecurePixel"), mUseNonSecurePixel)
            Return mUseNonSecurePixel
        End Get
    End Property

    ''' <summary>
    ''' If both email template and config are set to use tracking - then return true.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetTrackingPixel() As Boolean
        Dim trackingEnabledInConfig As Boolean = False
        Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings("TrackEmailOpens"), trackingEnabledInConfig)
        If trackingEnabledInConfig AndAlso TrackingEnabled Then
            Return True
        Else
            Return False
        End If
    End Function

    Private mEmailTrackingPixel As String = String.Empty
    Public Property EmailTrackingPixel() As String
        Get
            If String.IsNullOrEmpty(mEmailTrackingPixel) Then
                If SetTrackingPixel() Then
                    If Not HasComponents Then ' Set hard coded tracking pixel only if this template doesn't use components.
                        Dim HttpProtocol As String = "https"
                        If UseNonSecurePixel Then
                            HttpProtocol = "http"
                        End If
                        mEmailTrackingPixel = vbCrLf & "<br/><img src=""" & HttpProtocol & "://%SecureSite%/EmailsTracker.ashx?code=%Code%&etlid=" & Me.EmailTemplateLanguageId & """ height=""1"" width=""1"" />"
                    End If
                End If
            End If
            Return mEmailTrackingPixel
        End Get
        Set(ByVal value As String)
            If Not String.IsNullOrEmpty(value) Then
                If SetTrackingPixel() AndAlso HasComponents Then
                    'If mHasComponents Then
                    mEmailTrackingPixel = value
                    If UseNonSecurePixel Then
                        mEmailTrackingPixel = mEmailTrackingPixel.Replace("https://", "http://")
                    End If
                Else
                    ' TODO : decide what else.
                    'End If
                End If
            End If
        End Set
    End Property


    Private mReplaceComponentsOnLoad As Boolean = True
    ''' <summary>
    ''' When viewing the template in CMS we don’t want to replace components (like links) on load.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ReplaceComponentsOnLoad() As Boolean
        Get
            Return mReplaceComponentsOnLoad
        End Get
        Set(ByVal value As Boolean)
            mReplaceComponentsOnLoad = value
        End Set
    End Property

    Private _etlIsSplitTest As Boolean = False

    Public ReadOnly Property EmailTemplateLanguageIsSplitTest() As Boolean
        Get
            Return EmailTest.EmailSplitTest.IsEmailLanaguageTemplateSplitTest(Me.EmailTemplateLanguageId)
        End Get
    End Property


    Private _randomSelectedTestVariation As EmailTest.EmailSplitTestContent = Nothing
    Public Property RandomSelectedTestVariation() As EmailTest.EmailSplitTestContent
        Get
            Return _randomSelectedTestVariation
        End Get
        Set(ByVal value As EmailTest.EmailSplitTestContent)
            _randomSelectedTestVariation = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Function ReplaceEmailTemplateVariables(ByVal str As String, ByVal signup As Marketing.Signup, Optional ByVal m As TicketMessage = Nothing) As String

        'If Me.CampaignGroupId <> Nothing Then
        '    Dim domainCampaignGroup As New DomainCampaignGroup(Me.CampaignGroupId, signup.Domain.Code)
        '    If domainCampaignGroup.CampaignCode <> Nothing Then
        '        str = Regex.Replace(str, "%ConvCid%", domainCampaignGroup.CampaignCode.ToString(), RegexOptions.IgnoreCase)
        '    End If
        'End If

        'str = Regex.Replace(str, "%FirstName%", signup.FirstName, RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%LastName%", signup.LastName, RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%BrandingName%", signup.Domain.Branding.Name, RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%GoogleLogin%", signup.GoogleLogin, RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%Password%", signup.Password, RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%Domain%", signup.Domain.Name, RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%LoginName%", signup.LoginName, RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%Email%", signup.Email, RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%BusName%", signup.BusName, RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%SecureSite%", signup.Domain.SecureSite, RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%Code%", CStr(signup.Code), RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%URL%", signup.BusWebAddress, RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%ConvCid%", CStr(signup.Domain.EmailConversionCID), RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%Country%", signup.Country, RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%TrialPeriodInDays%", CStr(signup.ConversionCampaign.TrialPeriodInDays), RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%phone%", signup.Domain.SupportPhone, RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%Cid%", CStr(signup.Campaign.Code), RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%TrialPricePoint%", signup.Campaign.SignupProductCurrency.Price.ToString("0"), RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%PricePoint%", signup.Campaign.OngoingProductCurrency.Price.ToString("0"), RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%CurrencySymbol%", signup.Domain.CurrencySymbol, RegexOptions.IgnoreCase)
        'str = Regex.Replace(str, "%etlId%", Me.EmailTemplateLanguageId.ToString(), RegexOptions.IgnoreCase)

        'Dim AuthCap As Decimal = signup.Campaign.OngoingProductCurrency.Price + signup.Campaign.SignupProductCurrency.Price
        'str = Regex.Replace(str, "%authcap%", AuthCap.ToString("0"), RegexOptions.IgnoreCase)

        'Dim bonusValue As String = "$100"
        'If signup.Campaign.Country.CountryCode = "UK" Then
        '    bonusValue = "£50"
        'ElseIf signup.Campaign.Country.CountryCode = "EU" Then
        '    bonusValue = "€50"
        'End If

        'str = Regex.Replace(str, "%ADWORDSCREDIT%", bonusValue, RegexOptions.IgnoreCase)

        'If m IsNot Nothing Then
        '    str = Regex.Replace(str, "%TicketCode%", CStr(m.TicketCode), RegexOptions.IgnoreCase)
        '    str = Regex.Replace(str, "%TicketSubject%", m.Ticket.Subject, RegexOptions.IgnoreCase)
        '    str = Regex.Replace(str, "%TicketStatus%", m.Ticket.Status.DisplayName, RegexOptions.IgnoreCase)
        '    str = Regex.Replace(str, "%MessageContents%", m.Message, RegexOptions.IgnoreCase)
        'End If

        Return str
    End Function

    Protected Sub loadTrackingAndTestInfo()
        loadTrackingAndTestInfo(True, True)
    End Sub

    Protected Sub loadTrackingAndTestInfo(ByVal ReplaceComponentsOnLoad As Boolean, ByVal LoadTestInfoIfActvie As Boolean)
        Dim etcSettings As New EmailTemplateComponentsSettings()
        Dim etlComponents As EmailTemplateComponents = Nothing
        If EmailTemplateLanguageIsSplitTest() AndAlso LoadTestInfoIfActvie Then ' Don't load test info in CMS
            Dim emailSplitTest As New EmailTest.EmailSplitTest()
            emailSplitTest = EmailTest.EmailSplitTest.GetSplitTestFromEmailLanaguageTemplate(Me.EmailTemplateLanguageId)
            If emailSplitTest.EmailSplitTestId > 0 Then
                emailSplitTest.RandomSelectedTestVariation = emailSplitTest.LoadSplitTestContent(CType(Me, EmailTemplateLanguage), emailSplitTest)
                Me.RandomSelectedTestVariation = emailSplitTest.RandomSelectedTestVariation
            End If

            If Me.OpensTrackingEnabled Then
                etcSettings.GetSplitTestOpensTrackingPixelComponent = EmailTemplateComponent.ComponentType.TrackingPixelSplitTest
            End If

            If Me.ClicksConversionsTrackingEnabled Then
                etcSettings.GetSplitTestCCPageUrlComponent =
                    EmailTemplateComponent.ComponentType.CCPageUrlSplitTest
            Else
                etcSettings.GetRegularCCPageUrlComponent = EmailTemplateComponent.ComponentType.CCPageUrl
            End If
            'etlComponents = New EmailTemplateComponents(etcSettings,
            etlComponents = New EmailTemplateComponents(etcSettings, True)
            ReplaceEmailTemplateLanguageComponents(etlComponents, emailSplitTest)
        Else
            If Me.OpensTrackingEnabled Then
                etcSettings.GetOpensTrackingPixelComponent = EmailTemplateComponent.ComponentType.TrackingPixel
            End If

            If Me.ClicksConversionsTrackingEnabled Then
                etcSettings.GetClicksConversionsTrackingCCPageUrlComponent =
                    EmailTemplateComponent.ComponentType.CCPageUrlTrackingClicksConversions
            Else
                etcSettings.GetRegularCCPageUrlComponent = EmailTemplateComponent.ComponentType.CCPageUrl
            End If

            'etlComponents = New EmailTemplateComponents(Me.EmailTemplateLanguageId, etcSettings)
            etlComponents = New EmailTemplateComponents(etcSettings)
            'ReplaceEmailTemplateLanguageComponents(etlComponents, LoadTestInfoIfActvie, Nothing)
            ReplaceEmailTemplateLanguageComponents(etlComponents, ReplaceComponentsOnLoad, Nothing)
        End If
        'ReplaceEmailTemplateLanguageComponents(etlComponents)
        'End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="etlComponents"></param>
    ''' <param name="ReplaceComponentsOnLoad">When Viewing in CMS is False</param>
    ''' <remarks></remarks>
    Protected Sub ReplaceEmailTemplateLanguageComponents(ByVal etlComponents As EmailTemplateComponents, _
    ByVal ReplaceComponentsOnLoad As Boolean, ByVal emailSplitTest As EmailTest.EmailSplitTest)
        Try
            For Each comp As EmailTemplateComponent In etlComponents.Components()
                Select Case comp.EmailTemplateComponentType
                    Case EmailTemplateComponent.ComponentType.CCPageUrl, EmailTemplateComponent.ComponentType.CCPageUrlTrackingClicksConversions
                        comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%etlId%", CStr(Me.EmailTemplateLanguageId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                        If ReplaceComponentsOnLoad Then
                            Me.Body = System.Text.RegularExpressions.Regex.Replace(Me.Body, comp.ComponentMergeTag, comp.Component, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                        End If
                    Case EmailTemplateComponent.ComponentType.TrackingPixel
                        comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%etlId%", CStr(Me.EmailTemplateLanguageId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                        Me.EmailTrackingPixel = comp.Component
                    Case EmailTemplateComponent.ComponentType.CCPageUrlSplitTest
                        If Not emailSplitTest Is Nothing AndAlso emailSplitTest.EmailSplitTestId > 0 Then
                            comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%etlId%", CStr(Me.EmailTemplateLanguageId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                            comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%estId%", CStr(emailSplitTest.EmailSplitTestId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                            comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%estvarId%", CStr(emailSplitTest.RandomSelectedTestVariation.VariationId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                        End If
                        If ReplaceComponentsOnLoad Then
                            Me.Body = System.Text.RegularExpressions.Regex.Replace(Me.Body, comp.ComponentMergeTag, comp.Component, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                        End If
                    Case EmailTemplateComponent.ComponentType.TrackingPixelSplitTest
                        If Not emailSplitTest Is Nothing AndAlso emailSplitTest.EmailSplitTestId > 0 Then
                            comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%etlId%", CStr(Me.EmailTemplateLanguageId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                            comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%estId%", CStr(emailSplitTest.EmailSplitTestId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                            comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%estvarId%", CStr(emailSplitTest.RandomSelectedTestVariation.VariationId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                        End If
                        Me.EmailTrackingPixel = comp.Component
                End Select
            Next

        Catch ex As Exception

        End Try


    End Sub
    Protected Sub ReplaceEmailTemplateLanguageComponents(ByVal etlComponents As EmailTemplateComponents)
        ReplaceEmailTemplateLanguageComponents(etlComponents, True, Nothing)
    End Sub

    Protected Sub ReplaceEmailTemplateLanguageComponents(ByVal etlComponents As EmailTemplateComponents, ByVal emailSplitTest As EmailTest.EmailSplitTest)
        ReplaceEmailTemplateLanguageComponents(etlComponents, True, emailSplitTest)
    End Sub

    Public Sub Send()


    End Sub

#End Region
End Class
