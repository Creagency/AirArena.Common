Imports System.Collections.Generic
Imports System.Data
Imports Microsoft.VisualBasic



Public MustInherit Class LanguageBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal languageId As Integer)
        Me.New()
        Load(languageId)
    End Sub
#End Region
#Region " Properties "

    Private mEmailTemplateLanguages As List(Of EmailTemplateLanguage)
    Public Overridable ReadOnly Property EmailTemplateLanguages() As List(Of EmailTemplateLanguage)
        Get
            If mEmailTemplateLanguages Is Nothing Then
                mEmailTemplateLanguages = New List(Of EmailTemplateLanguage)
                Dim req As New AirArena.Data.DataRequest("Find_EmailTemplateLanguage")
                'req.Parameters.add("@SomethingId", SomethingId)
                Dim dt As DataTable = DataCon.GetDataTable(req)
                For Each dr As DataRow In dt.Rows
                    mEmailTemplateLanguages.Add(New EmailTemplateLanguage(CInt(dr("EmailTemplateLanguageId"))))
                Next dr
            End If
            Return mEmailTemplateLanguages
        End Get
    End Property

    Private mLanguageId As Integer
    Public Overridable Property LanguageId() As Integer
        Get
            Return mLanguageId
        End Get
        Protected Set(ByVal value As Integer)
            mLanguageId = value
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


    Private mCode As String
    Public Overridable Property Code() As String
        Get
            Return mCode
        End Get
        Set(ByVal value As String)
            mCode = value
        End Set
    End Property

#End Region
#Region " Methods "
    Protected Overridable Sub Load(ByVal languageId As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_Language")
        req.Parameters.Add("@languageId", languageId)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            mLanguageId = CType(dr("LanguageId"), Integer)
            mName = CType(dr("Name"), String)
            mCode = CType(dr("Code"), String)
        End If
    End Sub
    Public Overridable Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_Language")
        With req.Parameters
            .Add("@LanguageId", mLanguageId)
            .Add("@Name", mName)
            .Add("@Code", mCode)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                'TODO: If this object represents a mulit-key table then remove this if/then condition
                Me.mLanguageId = CType(dt.Rows(0)("LanguageId"), Integer)
            End If
        End With
    End Sub
#End Region
End Class

Public MustInherit Class EmailTemplateComponentsBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub

    ''' <summary>
    ''' Gets a collection of all components of this email template.
    ''' </summary>
    ''' <param name="emailTemplateLanguageId"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal emailTemplateLanguageId As Integer)
        Me.New()
        Load(emailTemplateLanguageId)
    End Sub

    Public Sub New(ByVal emailTemplateLanguageId As Integer,
                   ByVal emailTemplateComponentSettings As EmailTemplateComponentsSettings)
        Me.New()
        Load(emailTemplateLanguageId, emailTemplateComponentSettings)
    End Sub

    ''' <summary>
    ''' Use this constructor to get split test components (etl invariant)
    ''' </summary>
    ''' <param name="emailTemplateComponentSettings"></param>
    ''' <param name="getSplitTestComponents"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal emailTemplateComponentSettings As EmailTemplateComponentsSettings,
                    ByVal getSplitTestComponents As Boolean)
        Me.New()
        Load(emailTemplateComponentSettings, getSplitTestComponents)
    End Sub

    Public Sub New(ByVal emailTemplateComponentSettings As EmailTemplateComponentsSettings)
        Me.New()
        Load(emailTemplateComponentSettings)
    End Sub
#End Region
#Region " Properties "

    'Private mCode As Integer
    'Public Overridable ReadOnly Property Code() As Integer
    '    Get
    '        Return mCode
    '    End Get
    '    'Set(ByVal value As Integer)

    '    'End Set
    'End Property


    Private mEmailTemplateLanguageId As Integer
    Public Overridable Property EmailTemplateLanguageId() As Integer
        Get
            Return mEmailTemplateLanguageId
        End Get
        Protected Set(ByVal value As Integer)
            mEmailTemplateLanguageId = value
        End Set
    End Property

    Private mEmailTemplateLanguage As EmailTemplateLanguage
    Public Overridable ReadOnly Property EmailTemplateLanguage() As EmailTemplateLanguage
        Get
            If mEmailTemplateLanguage Is Nothing Then
                mEmailTemplateLanguage = New EmailTemplateLanguage(mEmailTemplateLanguageId)
            End If
            Return mEmailTemplateLanguage
        End Get
        'Set(ByVal value As EmailTemplateLanguage)

        'End Set
    End Property

    Private mComponents As List(Of EmailTemplateComponent)
    Public Overridable ReadOnly Property Components() As List(Of EmailTemplateComponent)
        Get
            If mComponents Is Nothing Then
                mComponents = New List(Of EmailTemplateComponent)
            End If
            Return mComponents
        End Get
    End Property

#End Region
#Region " Methods "
    Protected Overridable Sub Load(ByVal emailTemplateLanguageId As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_EmailTemplateComponents")
        req.Parameters.Add("@emailTemplateLanguageId", emailTemplateLanguageId)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            mComponents = New List(Of EmailTemplateComponent)
            For Each dr As DataRow In dt.Rows()
                Dim comp As New EmailTemplateComponent()
                With comp
                    .Code = CType(dr("i_code"), Integer)
                    .EmailTemplateLanguageId = CType(dr("EmailTemplateLanguageId"), Integer)
                    .Component = CType(dr("Component"), String)
                    .ComponentMergeTag = CType(dr("ComponentMergeTag"), String)
                    .EmailTemplateComponentType = CType(dr("ComponentType"), EmailTemplateComponent.ComponentType)
                End With
                mComponents.Add(comp)
            Next
            Me.mEmailTemplateLanguageId = mComponents.Item(0).EmailTemplateLanguageId
        End If
    End Sub

    ''' <summary>
    ''' stored proc not yet implemented !!!!
    ''' </summary>
    ''' <param name="emailTemplateLanguageId"></param>
    ''' <param name="componentType"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub Load(ByVal emailTemplateLanguageId As Integer, ByVal componentType As EmailTemplateComponent.ComponentType)
        Dim req As New AirArena.Data.DataRequest("Get_EmailTemplateComponents")
        req.Parameters.Add("@emailTemplateLanguageId", emailTemplateLanguageId)
        req.Parameters.Add("@componentType", componentType)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            mComponents = New List(Of EmailTemplateComponent)
            For Each dr As DataRow In dt.Rows()
                Dim comp As New EmailTemplateComponent()
                With comp
                    .Code = CType(dr("i_code"), Integer)
                    .EmailTemplateLanguageId = CType(dr("EmailTemplateLanguageId"), Integer)
                    .Component = CType(dr("Component"), String)
                    .ComponentMergeTag = CType(dr("ComponentMergeTag"), String)
                    .EmailTemplateComponentType = CType(dr("ComponentType"), EmailTemplateComponent.ComponentType)
                End With
                mComponents.Add(comp)
            Next
            Me.mEmailTemplateLanguageId = mComponents.Item(0).EmailTemplateLanguageId
        End If
    End Sub

    Protected Overridable Sub Load(ByVal emailTemplateLanguageId As Integer, ByVal emailTemplateComponentSettings As EmailTemplateComponentsSettings)
        Dim req As New AirArena.Data.DataRequest("Get_EmailTemplateComponents")
        req.Parameters.Add("@emailTemplateLanguageId", emailTemplateLanguageId)
        emailTemplateComponentSettings.GetAllComponents = False
        req.Parameters.Add("@GetAllComponents", emailTemplateComponentSettings.GetAllComponents)
        req.Parameters.Add("@CCPageUrl", emailTemplateComponentSettings.GetRegularCCPageUrlComponent)
        req.Parameters.Add("@TrackingPixel", emailTemplateComponentSettings.GetOpensTrackingPixelComponent)
        req.Parameters.Add("@CCPageUrlTrackingClicksConversions", emailTemplateComponentSettings.GetClicksConversionsTrackingCCPageUrlComponent)
        req.Parameters.Add("@CCPageUrlSplitTest", emailTemplateComponentSettings.GetSplitTestCCPageUrlComponent)
        req.Parameters.Add("@CCPageUrlMultiVariantTest", emailTemplateComponentSettings.GetMultiVariantTestCCPageUrlComponent)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            mComponents = New List(Of EmailTemplateComponent)
            For Each dr As DataRow In dt.Rows()
                Dim comp As New EmailTemplateComponent()
                With comp
                    .Code = CType(dr("i_code"), Integer)
                    .EmailTemplateLanguageId = CType(dr("EmailTemplateLanguageId"), Integer)
                    .Component = CType(dr("Component"), String)
                    .ComponentMergeTag = CType(dr("ComponentMergeTag"), String)
                    .EmailTemplateComponentType = CType(dr("ComponentType"), EmailTemplateComponent.ComponentType)
                End With
                mComponents.Add(comp)
            Next
            Me.mEmailTemplateLanguageId = mComponents.Item(0).EmailTemplateLanguageId
        End If
    End Sub

    ''' <summary>
    ''' Use this overload to get split test components (etl invariant)
    ''' </summary>
    ''' <param name="emailTemplateComponentSettings"></param>
    ''' <param name="getSplitTestComponents"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub Load(ByVal emailTemplateComponentSettings As EmailTemplateComponentsSettings, ByVal getSplitTestComponents As Boolean)
        Dim req As New AirArena.Data.DataRequest("Get_EmailTemplateSplitTestComponents")
        'req.Parameters.Add("@emailTemplateLanguageId", emailTemplateLanguageId)
        emailTemplateComponentSettings.GetAllComponents = False
        'req.Parameters.Add("@GetAllComponents", emailTemplateComponentSettings.GetAllComponents)
        req.Parameters.Add("@CCPageUrl", emailTemplateComponentSettings.GetRegularCCPageUrlComponent)
        'req.Parameters.Add("@TrackingPixel", emailTemplateComponentSettings.GetOpensTrackingPixelComponent)
        req.Parameters.Add("@TrackingPixelSplitTest", emailTemplateComponentSettings.GetSplitTestOpensTrackingPixelComponent)
        req.Parameters.Add("@CCPageUrlSplitTest", emailTemplateComponentSettings.GetSplitTestCCPageUrlComponent)
        'req.Parameters.Add("@CCPageUrlMultiVariantTest", emailTemplateComponentSettings.GetMultiVariantTestCCPageUrlComponent)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            mComponents = New List(Of EmailTemplateComponent)
            For Each dr As DataRow In dt.Rows()
                Dim comp As New EmailTemplateComponent()
                With comp
                    .Code = CType(dr("i_code"), Integer)
                    '    .EmailTemplateLanguageId = CType(dr("EmailTemplateLanguageId"), Integer)
                    .Component = CType(dr("Component"), String)
                    .ComponentMergeTag = CType(dr("ComponentMergeTag"), String)
                    .EmailTemplateComponentType = CType(dr("ComponentType"), EmailTemplateComponent.ComponentType)
                End With
                mComponents.Add(comp)
            Next
            '  Me.mEmailTemplateLanguageId = mComponents.Item(0).EmailTemplateLanguageId
        End If
    End Sub

    Protected Overridable Sub Load(ByVal emailTemplateComponentSettings As EmailTemplateComponentsSettings)
        Dim req As New AirArena.Data.DataRequest("Get_EmailTemplateComponentsAnyEtl")
        emailTemplateComponentSettings.GetAllComponents = False
        req.Parameters.Add("@GetAllComponents", emailTemplateComponentSettings.GetAllComponents)
        req.Parameters.Add("@CCPageUrl", emailTemplateComponentSettings.GetRegularCCPageUrlComponent)
        req.Parameters.Add("@TrackingPixel", emailTemplateComponentSettings.GetOpensTrackingPixelComponent)
        req.Parameters.Add("@CCPageUrlTrackingClicksConversions", emailTemplateComponentSettings.GetClicksConversionsTrackingCCPageUrlComponent)
        req.Parameters.Add("@CCPageUrlSplitTest", emailTemplateComponentSettings.GetSplitTestCCPageUrlComponent)
        req.Parameters.Add("@CCPageUrlMultiVariantTest", emailTemplateComponentSettings.GetMultiVariantTestCCPageUrlComponent)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            mComponents = New List(Of EmailTemplateComponent)
            For Each dr As DataRow In dt.Rows()
                Dim comp As New EmailTemplateComponent()
                With comp
                    .Code = CType(dr("i_code"), Integer)
                    '.EmailTemplateLanguageId = CType(dr("EmailTemplateLanguageId"), Integer)
                    .Component = CType(dr("Component"), String)
                    .ComponentMergeTag = CType(dr("ComponentMergeTag"), String)
                    .EmailTemplateComponentType = CType(dr("ComponentType"), EmailTemplateComponent.ComponentType)
                End With
                mComponents.Add(comp)
            Next
            'Me.mEmailTemplateLanguageId = mComponents.Item(0).EmailTemplateLanguageId
        End If
    End Sub
#End Region

End Class

Public MustInherit Class EmailTemplateLanguageBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal emailTemplateLanguageId As Integer)
        Me.New()
        Load(emailTemplateLanguageId)
    End Sub
#End Region
#Region " Properties "

    Private mEmailTemplateLanguageId As Integer
    Public Overridable Property EmailTemplateLanguageId() As Integer
        Get
            Return mEmailTemplateLanguageId
        End Get
        Protected Set(ByVal value As Integer)
            mEmailTemplateLanguageId = value
        End Set
    End Property


    Private mEmailTemplateId As Integer
    Public Overridable Property EmailTemplateId() As Integer
        Get
            Return mEmailTemplateId
        End Get
        Set(ByVal value As Integer)
            mEmailTemplateId = value
        End Set
    End Property
    Private mEmailTemplate As EmailTemplate
    Public Overridable ReadOnly Property EmailTemplate() As EmailTemplate
        Get
            If mEmailTemplate Is Nothing Then
                mEmailTemplate = New EmailTemplate(mEmailTemplateId)
            End If
            Return mEmailTemplate
        End Get
    End Property


    Private mLanguageId As Integer
    Public Overridable Property LanguageId() As Integer
        Get
            Return mLanguageId
        End Get
        Set(ByVal value As Integer)
            mLanguageId = value
        End Set
    End Property
    Private mLanguage As Language
    Public Overridable ReadOnly Property Language() As Language
        Get
            If mLanguage Is Nothing Then
                mLanguage = New Language(mLanguageId)
            End If
            Return mLanguage
        End Get
    End Property


    Private mSubject As String
    Public Overridable Property Subject() As String
        Get
            Return mSubject
        End Get
        Set(ByVal value As String)
            mSubject = value
        End Set
    End Property


    Private mBody As String
    Public Overridable Property Body() As String
        Get
            Return mBody
        End Get
        Set(ByVal value As String)
            mBody = value
        End Set
    End Property


    Private mMailFrom As String
    Public Overridable Property MailFrom() As String
        Get
            Return mMailFrom
        End Get
        Set(ByVal value As String)
            mMailFrom = value
        End Set
    End Property


    Private mMailFromName As String
    Public Overridable Property MailFromName() As String
        Get
            Return mMailFromName
        End Get
        Set(ByVal value As String)
            mMailFromName = value
        End Set
    End Property

    Private mFormat As Integer
    Public Overridable Property Format() As Integer
        Get
            Return mFormat
        End Get
        Set(ByVal value As Integer)
            mFormat = value
        End Set
    End Property
    ''' <summary>
    ''' Might be deprecated
    ''' </summary>
    ''' <remarks></remarks>
    Private mTrackingEnabled As Boolean
    Public Overridable Property TrackingEnabled() As Boolean
        Get
            Return mTrackingEnabled
        End Get
        Set(ByVal value As Boolean)
            mTrackingEnabled = value
        End Set
    End Property

    Private mSendTrackingEnabled As Boolean
    Public Overridable Property SendTrackingEnabled() As Boolean
        Get
            Return mSendTrackingEnabled
        End Get
        Set(ByVal value As Boolean)
            mSendTrackingEnabled = value
        End Set
    End Property

    Private mOpensTrackingEnabled As Boolean
    Public Overridable Property OpensTrackingEnabled() As Boolean
        Get
            Return mOpensTrackingEnabled
        End Get
        Set(ByVal value As Boolean)
            mOpensTrackingEnabled = value
        End Set
    End Property

    Private mClicksConversionsTrackingEnabled As Boolean
    Public Overridable Property ClicksConversionsTrackingEnabled() As Boolean
        Get
            Return mClicksConversionsTrackingEnabled
        End Get
        Set(ByVal value As Boolean)
            mClicksConversionsTrackingEnabled = value
        End Set
    End Property

    Private mHasComponents As Boolean
    Public Overridable Property HasComponents() As Boolean
        Get
            Return mHasComponents
        End Get
        Set(ByVal value As Boolean)
            mHasComponents = value
        End Set
    End Property

    Private mCampaignGroupId As Integer
    Public Overridable Property CampaignGroupId() As Integer
        Get
            Return mCampaignGroupId
        End Get
        Set(ByVal value As Integer)
            mCampaignGroupId = value
        End Set
    End Property

    'Private ReadOnly Property UseNonSecurePixel() As Boolean
    '    Get
    '        Dim mUseNonSecurePixel As Boolean = False
    '        Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings("UseNonSecurePixel"), mUseNonSecurePixel)
    '        Return mUseNonSecurePixel
    '    End Get
    'End Property

    ' ''' <summary>
    ' ''' If both email template and config are set to use tracking - then return true.
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function SetTrackingPixel() As Boolean
    '    Dim trackingEnabledInConfig As Boolean = False
    '    Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings("TrackEmailOpens"), trackingEnabledInConfig)
    '    If trackingEnabledInConfig AndAlso TrackingEnabled Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function

    'Private mEmailTrackingPixel As String = String.Empty
    'Public Property EmailTrackingPixel() As String
    '    Get
    '        If String.IsNullOrEmpty(mEmailTrackingPixel) Then
    '            If SetTrackingPixel() Then
    '                If Not mHasComponents Then ' Set hard coded tracking pixel only if this template doesn't use components.
    '                    Dim HttpProtocol As String = "https"
    '                    If UseNonSecurePixel Then
    '                        HttpProtocol = "http"
    '                    End If
    '                    mEmailTrackingPixel = vbCrLf & "<br/><img src=""" & HttpProtocol & "://%SecureSite%/EmailsTracker.ashx?code=%Code%&etlid=" & Me.EmailTemplateLanguageId & """ height=""1"" width=""1"" />"
    '                End If
    '            End If
    '        End If
    '        Return mEmailTrackingPixel
    '    End Get
    '    Set(ByVal value As String)
    '        If Not String.IsNullOrEmpty(value) Then
    '            If SetTrackingPixel() AndAlso mHasComponents Then
    '                'If mHasComponents Then
    '                mEmailTrackingPixel = value
    '                If UseNonSecurePixel Then
    '                    mEmailTrackingPixel = mEmailTrackingPixel.Replace("https://", "http://")
    '                End If
    '            Else
    '                ' TODO : decide what else.
    '                'End If
    '            End If
    '        End If
    '    End Set
    'End Property


    'Private mReplaceComponentsOnLoad As Boolean = True
    ' ''' <summary>
    ' ''' When viewing the template in CMS we don’t want to replace components (like links) on load.
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property ReplaceComponentsOnLoad() As Boolean
    '    Get
    '        Return mReplaceComponentsOnLoad
    '    End Get
    '    Set(ByVal value As Boolean)
    '        mReplaceComponentsOnLoad = value
    '    End Set
    'End Property

    'Private _etlIsSplitTest As Boolean = False

    'Public ReadOnly Property EmailTemplateLanguageIsSplitTest() As Boolean
    '    Get
    '        Return GMOMBL.EmailTest.EmailSplitTest.IsEmailLanaguageTemplateSplitTest(Me.EmailTemplateLanguageId)
    '    End Get
    'End Property


    'Private _randomSelectedTestVariation As EmailTest.EmailSplitTestContent = Nothing
    'Public Property RandomSelectedTestVariation() As EmailTest.EmailSplitTestContent
    '    Get
    '        Return _randomSelectedTestVariation
    '    End Get
    '    Set(ByVal value As EmailTest.EmailSplitTestContent)
    '        _randomSelectedTestVariation = value
    '    End Set
    'End Property

#End Region
#Region " Methods "
    Protected Overridable Sub Load(ByVal emailTemplateLanguageId As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_EmailTemplateLanguage")
        req.Parameters.Add("@emailTemplateLanguageId", emailTemplateLanguageId)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            mEmailTemplateLanguageId = CType(dr("EmailTemplateLanguageId"), Integer)
            mEmailTemplateId = CType(dr("EmailTemplateId"), Integer)
            mLanguageId = CType(dr("LanguageId"), Integer)
            mSubject = CType(dr("Subject"), String)
            mBody = CType(dr("Body"), String)
            mMailFrom = CType(dr("MailFrom"), String)
            mMailFromName = CType(dr("MailFromName"), String)
            mFormat = CType(dr("Format"), Integer)
            mTrackingEnabled = CType(dr("TrackingEnabled"), Boolean)
            mSendTrackingEnabled = CType(dr("SendTrackingEnabled"), Boolean)
            mOpensTrackingEnabled = CType(dr("OpensTrackingEnabled"), Boolean)
            mClicksConversionsTrackingEnabled = CType(dr("ClicksConversionsTrackingEnabled"), Boolean)
            mHasComponents = CType(dr("HasComponents"), Boolean)
            mCampaignGroupId = CType(IIf(IsDBNull(dr("CampaignGroupId")), Nothing, dr("CampaignGroupId")), Integer)

            '    Dim etcSettings As New EmailTemplateComponentsSettings()
            '    Dim etlComponents As EmailTemplateComponents = Nothing
            '    If EmailTemplateLanguageIsSplitTest() Then
            '        Dim emailSplitTest As New GMOMBL.EmailTest.EmailSplitTest()
            '        emailSplitTest = GMOMBL.EmailTest.EmailSplitTest.GetSplitTestFromEmailLanaguageTemplate(Me.EmailTemplateLanguageId)
            '        If emailSplitTest.EmailSplitTestId > 0 Then
            '            emailSplitTest.RandomSelectedTestVariation = emailSplitTest.LoadSplitTestContent(CType(Me, EmailTemplateLanguage), emailSplitTest)
            '            Me.RandomSelectedTestVariation = emailSplitTest.RandomSelectedTestVariation
            '        End If

            '        If Me.OpensTrackingEnabled Then
            '            etcSettings.GetSplitTestOpensTrackingPixelComponent = EmailTemplateComponent.ComponentType.TrackingPixelSplitTest
            '        End If

            '        If Me.ClicksConversionsTrackingEnabled Then
            '            etcSettings.GetSplitTestCCPageUrlComponent =
            '                EmailTemplateComponent.ComponentType.CCPageUrlSplitTest
            '        Else
            '            etcSettings.GetRegularCCPageUrlComponent = EmailTemplateComponent.ComponentType.CCPageUrl
            '        End If
            '        'etlComponents = New EmailTemplateComponents(etcSettings,
            '        etlComponents = New EmailTemplateComponents(etcSettings, True)
            '        ReplaceEmailTemplateLanguageComponents(etlComponents, emailSplitTest)
            '    Else
            '        If Me.OpensTrackingEnabled Then
            '            etcSettings.GetOpensTrackingPixelComponent = EmailTemplateComponent.ComponentType.TrackingPixel
            '        End If

            '        If Me.ClicksConversionsTrackingEnabled Then
            '            etcSettings.GetClicksConversionsTrackingCCPageUrlComponent =
            '                EmailTemplateComponent.ComponentType.CCPageUrlTrackingClicksConversions
            '        Else
            '            etcSettings.GetRegularCCPageUrlComponent = EmailTemplateComponent.ComponentType.CCPageUrl
            '        End If

            '        etlComponents = New EmailTemplateComponents(Me.EmailTemplateLanguageId, etcSettings)
            '        ReplaceEmailTemplateLanguageComponents(etlComponents)
            '    End If
            '    'ReplaceEmailTemplateLanguageComponents(etlComponents)
        End If
    End Sub

    Protected Overridable Sub Load(ByVal emailTemplateId As EmailTemplate.TemplateName, ByVal languageId As Integer, _
    ByVal ReplaceComponentsOnLoad As Boolean)
        Dim req As New AirArena.Data.DataRequest("Find_EmailTemplateLanguage")
        req.Parameters.Add("@emailTemplateId", CInt(emailTemplateId))
        req.Parameters.Add("@LanguageId", languageId)
        Dim dt As DataTable = DataCon.GetDataTable(req)

        If dt.Rows.Count = 0 Then 'No EmailTemplateLanguage available, try English
            req = New AirArena.Data.DataRequest("Find_EmailTemplateLanguage")
            req.Parameters.Add("@emailTemplateId", CInt(emailTemplateId))
            req.Parameters.Add("@LanguageId", 1)
            dt = DataCon.GetDataTable(req)
        End If

        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            Me.EmailTemplateLanguageId = CType(dr("EmailTemplateLanguageId"), Integer)
            Me.EmailTemplateId = CType(dr("EmailTemplateId"), Integer)
            Me.LanguageId = CType(dr("LanguageId"), Integer)
            Me.Subject = CType(dr("Subject"), String)
            Me.Body = CType(dr("Body"), String)
            Me.MailFrom = CType(dr("MailFrom"), String)
            Me.MailFromName = CType(dr("MailFromName"), String)
            Me.Format = CType(dr("Format"), Integer)
            Me.TrackingEnabled = CType(dr("TrackingEnabled"), Boolean)
            Me.SendTrackingEnabled = CType(dr("SendTrackingEnabled"), Boolean)
            Me.OpensTrackingEnabled = CType(dr("OpensTrackingEnabled"), Boolean)
            Me.ClicksConversionsTrackingEnabled = CType(dr("ClicksConversionsTrackingEnabled"), Boolean)
            Me.HasComponents = CType(dr("HasComponents"), Boolean)
            Me.CampaignGroupId = CInt(IIf(IsDBNull(dr("CampaignGroupId")), Nothing, dr("CampaignGroupId")))
            'If Me.TrackingEnabled Then
            'Dim etcSettings As New EmailTemplateComponentsSettings()

            'If Me.OpensTrackingEnabled Then
            '    etcSettings.GetOpensTrackingPixelComponent = EmailTemplateComponent.ComponentType.TrackingPixel
            'End If

            'If Me.ClicksConversionsTrackingEnabled Then
            '    etcSettings.GetClicksConversionsTrackingCCPageUrlComponent =
            '        EmailTemplateComponent.ComponentType.CCPageUrlTrackingClicksConversions
            'Else
            '    etcSettings.GetRegularCCPageUrlComponent = EmailTemplateComponent.ComponentType.CCPageUrl
            'End If

            'Dim etlComponents As New EmailTemplateComponents(Me.EmailTemplateLanguageId, etcSettings)
            'MyBase.ReplaceEmailTemplateLanguageComponents(etlComponents, ReplaceComponentsOnLoad, Nothing)
            'loadTrackingAndTestInfo()
        End If


    End Sub
    Public Overridable Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_EmailTemplateLanguage")
        With req.Parameters
            .Add("@EmailTemplateLanguageId", mEmailTemplateLanguageId)
            .Add("@EmailTemplateId", mEmailTemplateId)
            .Add("@LanguageId", mLanguageId)
            .Add("@Subject", mSubject)
            .Add("@Body", mBody)
            .Add("@MailFrom", mMailFrom)
            .Add("@MailFromName", mMailFromName)
            .Add("@Format", mFormat)
            .Add("@TrackingEnabled", mTrackingEnabled)
            .Add("@HasComponents", mHasComponents)
            .Add("@SendTrackingEnabled", mSendTrackingEnabled)
            .Add("@OpensTrackingEnabled", mOpensTrackingEnabled)
            .Add("@ClicksConversionsTrackingEnabled", mClicksConversionsTrackingEnabled)
            If mCampaignGroupId <> 0 Then .Add("@CampaignGroupId", mCampaignGroupId)

            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                'TODO: If this object represents a mulit-key table then remove this if/then condition
                Me.mEmailTemplateLanguageId = CType(dt.Rows(0)("EmailTemplateLanguageId"), Integer)
            End If
        End With
    End Sub
    ' ''' <summary>
    ' ''' 
    ' ''' </summary>
    ' ''' <param name="etlComponents"></param>
    ' ''' <param name="ReplaceComponentsOnLoad">When Viewing in CMS is False</param>
    ' ''' <remarks></remarks>
    'Protected Sub ReplaceEmailTemplateLanguageComponents(ByVal etlComponents As EmailTemplateComponents, _
    'ByVal ReplaceComponentsOnLoad As Boolean, ByVal emailSplitTest As EmailTest.EmailSplitTest)
    '    For Each comp As EmailTemplateComponent In etlComponents.Components()
    '        Select Case comp.EmailTemplateComponentType
    '            Case EmailTemplateComponent.ComponentType.CCPageUrl, EmailTemplateComponent.ComponentType.CCPageUrlTrackingClicksConversions
    '                comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%etlId%", CStr(Me.EmailTemplateLanguageId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    '                If ReplaceComponentsOnLoad Then
    '                    Me.Body = System.Text.RegularExpressions.Regex.Replace(Me.Body, comp.ComponentMergeTag, comp.Component, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    '                End If
    '            Case EmailTemplateComponent.ComponentType.TrackingPixel
    '                comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%etlId%", CStr(Me.EmailTemplateLanguageId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    '                Me.EmailTrackingPixel = comp.Component
    '            Case EmailTemplateComponent.ComponentType.CCPageUrlSplitTest
    '                If Not emailSplitTest Is Nothing AndAlso emailSplitTest.EmailSplitTestId > 0 Then
    '                    comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%etlId%", CStr(Me.EmailTemplateLanguageId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    '                    comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%estId%", CStr(emailSplitTest.EmailSplitTestId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    '                    comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%estvarId%", CStr(emailSplitTest.RandomSelectedTestVariation.VariationId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    '                End If
    '                If ReplaceComponentsOnLoad Then
    '                    Me.Body = System.Text.RegularExpressions.Regex.Replace(Me.Body, comp.ComponentMergeTag, comp.Component, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    '                End If
    '            Case EmailTemplateComponent.ComponentType.TrackingPixelSplitTest
    '                If Not emailSplitTest Is Nothing AndAlso emailSplitTest.EmailSplitTestId > 0 Then
    '                    comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%etlId%", CStr(Me.EmailTemplateLanguageId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    '                    comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%estId%", CStr(emailSplitTest.EmailSplitTestId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    '                    comp.Component = System.Text.RegularExpressions.Regex.Replace(comp.Component, "%estvarId%", CStr(emailSplitTest.RandomSelectedTestVariation.VariationId), System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    '                End If
    '                Me.EmailTrackingPixel = comp.Component
    '        End Select
    '    Next


    'End Sub
    'Protected Sub ReplaceEmailTemplateLanguageComponents(ByVal etlComponents As EmailTemplateComponents)
    '    ReplaceEmailTemplateLanguageComponents(etlComponents, True, Nothing)
    'End Sub

    'Protected Sub ReplaceEmailTemplateLanguageComponents(ByVal etlComponents As EmailTemplateComponents, ByVal emailSplitTest As EmailTest.EmailSplitTest)
    '    ReplaceEmailTemplateLanguageComponents(etlComponents, True, emailSplitTest)
    'End Sub

#End Region
End Class

Public MustInherit Class EmailTemplateBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal emailTemplateId As Integer)
        Me.New()
        Load(emailTemplateId)
    End Sub
#End Region
#Region " Properties "

    Private mEmailTemplateLanguages As List(Of EmailTemplateLanguage)
    Public Overridable ReadOnly Property EmailTemplateLanguages() As List(Of EmailTemplateLanguage)
        Get
            If mEmailTemplateLanguages Is Nothing Then
                mEmailTemplateLanguages = New List(Of EmailTemplateLanguage)
                Dim req As New AirArena.Data.DataRequest("Find_EmailTemplateLanguage")
                'req.Parameters.add("@SomethingId", SomethingId)
                Dim dt As DataTable = DataCon.GetDataTable(req)
                For Each dr As DataRow In dt.Rows
                    mEmailTemplateLanguages.Add(New EmailTemplateLanguage(CInt(dr("EmailTemplateLanguageId"))))
                Next dr
            End If
            Return mEmailTemplateLanguages
        End Get
    End Property

    Private mEmailTemplateId As Integer
    Public Overridable Property EmailTemplateId() As Integer
        Get
            Return mEmailTemplateId
        End Get
        Protected Set(ByVal value As Integer)
            mEmailTemplateId = value
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

    'Default is Account emails
    Public Property EmailType() As SignupNewsletter.NewsletterTypeName = SignupNewsletterBase.NewsletterTypeName.AccountEmails

#End Region
#Region " Methods "
    Protected Overridable Sub Load(ByVal emailTemplateId As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_EmailTemplate")
        req.Parameters.Add("@emailTemplateId", emailTemplateId)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow
            dr = dt.Rows(0)
            mEmailTemplateId = CType(dr("EmailTemplateId"), Integer)
            mName = CType(dr("Name"), String)
            EmailType = CType(dr("EmailType"), SignupNewsletter.NewsletterTypeName)
        End If
    End Sub
    Public Overridable Sub Save()
        Dim req As New AirArena.Data.DataRequest("AU_EmailTemplate")
        With req.Parameters
            .Add("@EmailTemplateId", mEmailTemplateId)
            .Add("@Name", mName)
            .Add("@EmailType", EmailType)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                'TODO: If this object represents a mulit-key table then remove this if/then condition
                Me.mEmailTemplateId = CType(dt.Rows(0)("EmailTemplateId"), Integer)
            End If
        End With
    End Sub
#End Region
End Class

Public MustInherit Class EmailTrackingBase
    Protected DataCon As AirArena.Data.DataConnector
#Region " Properties "
    Private mCode As Integer
    Private mSignupCode As Integer
    Private mEmailTemplateLanguageId As Integer
    Private mEmailTemplateLanguage As EmailTemplateLanguage

    Public Overridable Property EmailTemplateLanguageId() As Integer
        Get
            Return mEmailTemplateLanguageId
        End Get
        Set(ByVal value As Integer)
            mEmailTemplateLanguageId = value
        End Set
    End Property

    Public Overridable ReadOnly Property EmailTemplateLanguage() As EmailTemplateLanguage
        Get
            If mEmailTemplateLanguage Is Nothing Then
                mEmailTemplateLanguage = New EmailTemplateLanguage(mEmailTemplateLanguageId)
            End If
            Return mEmailTemplateLanguage
        End Get
    End Property

    Public Overridable Property Code() As Integer
        Get
            Return mCode
        End Get
        Set(ByVal value As Integer)
            mCode = value
        End Set
    End Property

    Public Overridable Property SignupCode() As Integer
        Get
            Return mSignupCode
        End Get
        Set(ByVal value As Integer)
            mSignupCode = value
        End Set
    End Property
#End Region
#Region "Methods"
    'All email tracking classes (Opens, Clicks and Conversion would implement this method)
    Public MustOverride Sub Save()

#End Region
End Class




Public MustInherit Class SignupNewsletterBase
    Protected DataCon As AirArena.Data.DataConnector

#Region " Constructors "
    Public Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub
    Public Sub New(ByVal SignupCode As Integer)
        Me.New()
        Load(SignupCode)
    End Sub
#End Region

#Region " Properties "

    Protected _newsletterSignup As New List(Of SignupNewsletter)

    Private _isUnsub As Boolean = False
    Public Property IsUnsub As Boolean
        Get
            Return _isUnsub
        End Get
        Set(ByVal value As Boolean)
            _isUnsub = value
        End Set
    End Property

    Private _signupCode As Integer = 0
    Public Property SignupCode() As Integer
        Get
            Return _signupCode
        End Get
        Set(ByVal value As Integer)
            _signupCode = value
        End Set
    End Property

    Private _newsletterType As NewsletterTypeName
    Public Property NewsletterType As NewsletterTypeName
        Get
            Return _newsletterType
        End Get
        Set(ByVal value As NewsletterTypeName)
            _newsletterType = value
        End Set
    End Property

    Private _newsletterName As String
    Public Property NewsletterName As String
        Get
            Return _newsletterName
        End Get
        Set(ByVal value As String)
            _newsletterName = value
        End Set
    End Property

    Public Property EmailTemplateLanguageId() As Int32 = 0



    Public Enum NewsletterTypeName
        MarketingEmails = 1
        AccountEmails = 2
        UnpaidEmails = 3
        AccountActivateReminder
        AdwordsKeywordsReport
    End Enum

#End Region

#Region " Methods "
    Protected Overridable Sub Load(ByVal SignupCode As Integer)
        Dim req As New AirArena.Data.DataRequest("Get_NewsletterSignup")
        req.Parameters.Add("@SignupCode", SignupCode)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                Dim newsletterSignup As New SignupNewsletter()
                With newsletterSignup
                    ._signupCode = CType(dr("SignupCode"), Integer)
                    ._newsletterType = CType(CType(dr("NewsletterTypeId"), Integer), NewsletterTypeName)
                    ._newsletterName = CStr(dr("NewsletterName"))
                    ._isUnsub = CType(dr("IsUnsub"), Boolean)
                End With
                _newsletterSignup.Add(newsletterSignup)
            Next
            _signupCode = _newsletterSignup.Item(0).SignupCode
        Else
            _signupCode = SignupCode ' Asign to make the class usable for checking sub/unsub for email types
        End If
    End Sub

    Protected Overridable Function UnsubFromEmail(ByVal emailType As NewsletterTypeName) As Boolean
        Return UnsubResubFromEmail(emailType, True)
    End Function

    Protected Overridable Function ResubToEmail(ByVal emailType As NewsletterTypeName) As Boolean
        'Dim SuccessfulUnsub As Boolean = False
        'Dim req As New AirArena.Data.DataRequest("AU_UnsubSignupFromEmail")
        'req.Parameters.Add("@SignupCode", Me._signupCode)
        'req.Parameters.Add("@NewsletterTypeId", emailType)
        'req.Parameters.Add("@IsUnsub", False)
        'If Me.EmailTemplateLanguageId > 0 Then
        '    req.Parameters.Add("@EmailTemplateLanguageId", Me.EmailTemplateLanguageId)
        'End If
        'Dim dt As DataTable = DataCon.GetDataTable(req)
        'If dt.Rows.Count > 0 Then
        '    SuccessfulUnsub = True
        'End If
        'Return SuccessfulUnsub
        Return UnsubResubFromEmail(emailType, False)
    End Function

    Private Function UnsubResubFromEmail(ByVal emailType As NewsletterTypeName, ByVal unsubAction As Boolean) As Boolean
        Dim SuccessfulUnsub As Boolean = False
        Dim req As New AirArena.Data.DataRequest("AU_UnsubSignupFromEmail")
        req.Parameters.Add("@SignupCode", Me._signupCode)
        req.Parameters.Add("@NewsletterTypeId", emailType)
        req.Parameters.Add("@IsUnsub", unsubAction)
        If Me.EmailTemplateLanguageId > 0 Then
            req.Parameters.Add("@EmailTemplateLanguageId", Me.EmailTemplateLanguageId)
        End If
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            SuccessfulUnsub = True
        End If
        Return SuccessfulUnsub
    End Function


    
#End Region

End Class


Public MustInherit Class EmailCampaignBase
    Protected DataCon As AirArena.Data.DataConnector

#Region " Constructors "
    Protected Sub New()
        DataCon = New AirArena.Data.DataConnector
    End Sub

    Protected Sub New(ByVal emailCampaignId As Integer)
        Me.New()
        Load(emailCampaignId)
    End Sub

    Protected Sub New(ByVal dr As DataRow)
        Me.New()
        Load(dr)
    End Sub
#End Region

#Region " Properties "
    Private mEmailCampaignId As Integer

    Public ReadOnly Property EmailCampaignId() As Integer
        Get
            Return mEmailCampaignId
        End Get
    End Property

    Public Property Name() As String
    Public Property Description() As String = String.Empty
    Public Property SignupStatusId() As Integer
    Public Property EmailTemplateLanguageId() As Integer
    ''' <summary>
    ''' List of all countries that signups from them would receive this campaign.
    ''' If empty – all countries 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CountryIDs() As String = String.Empty
    'Public Property LanguageId() As Nullable(Of Integer)
    Public Property LanguageId() As Integer
    Public Property CreationDate() As DateTime
    Public Property ScheduleDT() As DateTime
    Public Property EmailCampaignStatus() As Integer
    Public Property RecipientsNumber() As Integer
    Public Property LastSignupCode() As Integer
    Public Property TimeZoneId() As String = String.Empty

#End Region

#Region " Methods "
    Protected Overridable Sub Load(ByVal emailCampaignId As Integer)
        Dim req As New AirArena.Data.DataRequest("Admin_GET_EmailCampaign")
        req.Parameters.Add("@EmailCampaignId", emailCampaignId)
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)

            Load(dr)

            'Me.mEmailCampaignId = CInt(dr("Id"))
            'Me.Name = CStr(dr("name"))
            'Me.Description = CStr(dr("Description"))
            'Me.CreationDate = CDate(dr("dt_Created"))
            'Me.EmailTemplateLanguageId = CInt(dr("EmailTemplateLanguageId"))
            'Me.SignupStatusId = CInt(dr("SignupStatusId"))
            'Me.CountryIDs = CStr(IIf(IsDBNull(dr("CountryIDs")), Nothing, dr("CountryIDs")))
            'Me.LanguageId = CInt(IIf(IsDBNull(dr("LanguageId")), 1, dr("LanguageId")))
            'Me.RecipientsNumber = CInt(dr("RecipientsNumber"))
            'Me.ScheduleDT = CDate(IIf(IsDBNull(dr("ScheduleDT")), Nothing, dr("ScheduleDT")))
            'Me.EmailCampaignStatus = CInt(dr("CampaignStatusId"))
        End If
      

    End Sub

    Protected Overridable Sub Load(ByVal dr As DataRow)
        Me.mEmailCampaignId = CInt(dr("Id"))
        Me.Name = CStr(dr("name"))
        Me.Description = CStr(dr("Description"))
        Me.CreationDate = CDate(dr("dt_Created"))
        Me.EmailTemplateLanguageId = CInt(dr("EmailTemplateLanguageId"))
        Me.SignupStatusId = CInt(dr("SignupStatusId"))
        Me.CountryIDs = CStr(IIf(IsDBNull(dr("CountryIDs")), Nothing, dr("CountryIDs")))
        Me.LanguageId = CInt(IIf(IsDBNull(dr("LanguageId")), 1, dr("LanguageId")))
        Me.RecipientsNumber = CInt(dr("RecipientsNumber"))
        Me.ScheduleDT = CDate(IIf(IsDBNull(dr("ScheduleDT")), Nothing, dr("ScheduleDT")))
        Me.EmailCampaignStatus = CInt(dr("CampaignStatusId"))
        Me.TimeZoneId = CStr(IIf(IsDBNull(dr("TimeZoneId")), String.Empty, dr("TimeZoneId")))
    End Sub

    Protected Sub Save()
        ' TO DO: implement this method + all stored procs
        Dim req As New AirArena.Data.DataRequest("AU_EmailCampaign")
        req.Parameters.Add("@EmailTemplateLanguageId", Me.EmailTemplateLanguageId)
        req.Parameters.Add("@SignupStatusId", Me.SignupStatusId)
        req.Parameters.Add("@Name", Me.Name)
        req.Parameters.Add("@Description", Me.Description)
        If Not String.IsNullOrEmpty(Me.CountryIDs) Then
            req.Parameters.Add("@CountryIDs", Me.CountryIDs)
        End If
        req.Parameters.Add("@LanguageId", Me.LanguageId)
        req.Parameters.Add("@RecipientsNumber", Me.RecipientsNumber)
        req.Parameters.Add("@ScheduleDT", Me.ScheduleDT)
        req.Parameters.Add("@CampaignStatusId", Me.EmailCampaignStatus)
        If Me.EmailCampaignId > 0 Then
            req.Parameters.Add("@EmailCampaignId", Me.EmailCampaignId)
        End If
        If Not String.IsNullOrEmpty(Me.TimeZoneId) Then
            req.Parameters.Add("@TimeZoneId", Me.TimeZoneId)
        End If
        Dim dt As DataTable = DataCon.GetDataTable(req)
        If dt.Rows.Count > 0 AndAlso Me.EmailCampaignId = 0 Then
            Me.mEmailCampaignId = CType(dt.Rows(0)("i_Code"), Integer)
        End If
    End Sub
#End Region


#Region " Shared methods "
    Protected Shared Function _getAllEmailCampaigns() As List(Of EmailCampaign)
        'Public Shared Function GetAllEmailCampaigns() As List(Of EmailCampaign)
        Dim DataCon As New AirArena.Data.DataConnector
        Dim AllCampaigns As New List(Of EmailCampaign)
        Dim req As New AirArena.Data.DataRequest("Admin_GET_EmailCampaign")
        Dim dt As DataTable = DataCon.GetDataTable(req)
        For Each dr As DataRow In dt.Rows
            Dim emailCampaign As New EmailCampaign(dr)
            'With emailCampaign
            '    .mEmailCampaignId = CInt(dr("Id"))
            '    .Name = CStr(dr("name"))
            '    .Description = CStr(dr("Description"))
            '    .CreationDate = CDate(dr("dt_Created"))
            '    .EmailTemplateLanguageId = CInt(dr("EmailTemplateLanguageId"))
            '    .SignupStatusId = CInt(dr("SignupStatusId"))
            '    .CountryIDs = CStr(dr("@CountryIDs"))
            'End With
            AllCampaigns.Add(emailCampaign)
        Next dr
        Return AllCampaigns
    End Function
#End Region
End Class

