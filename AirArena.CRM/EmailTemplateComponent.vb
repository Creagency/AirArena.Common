Public Class EmailTemplateComponent

#Region " Constructors "
    Public Sub New()
    End Sub
#End Region

#Region " Properties "

    Private mCode As Integer
    Public Overridable Property Code() As Integer
        Get
            Return mCode
        End Get
        Set(ByVal value As Integer)
            mCode = value
        End Set
    End Property


    Private mEmailTemplateLanguageId As Integer
    Public Overridable Property EmailTemplateLanguageId() As Integer
        Get
            Return mEmailTemplateLanguageId
        End Get
        Set(ByVal value As Integer)
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

    'Private mCCPageUrl As String
    'Public Overridable Property CCPageUrl() As String
    '    Get
    '        Return mCCPageUrl
    '    End Get
    '    Set(ByVal value As String)
    '        mCCPageUrl = value
    '    End Set
    'End Property

    Private mComponent As String
    Public Overridable Property Component() As String
        Get
            Return mComponent
        End Get
        Set(ByVal value As String)
            mComponent = value
        End Set
    End Property

    Private mComponentType As ComponentType
    Public Overridable Property EmailTemplateComponentType() As ComponentType
        Get
            Return mComponentType
        End Get
        Set(ByVal value As ComponentType)
            mComponentType = value
        End Set
    End Property

    Private mComponentMergeTag As String
    Public Overridable Property ComponentMergeTag() As String
        Get
            Return mComponentMergeTag
        End Get
        Set(ByVal value As String)
            mComponentMergeTag = value
        End Set
    End Property

    Public Enum ComponentType
        CCPageUrl = 1
        TrackingPixel = 2
        CCPageUrlTrackingClicksConversions = 3
        CCPageUrlSplitTest = 4
        CCPageUrlMultiVariantTest = 5
        TrackingPixelSplitTest
    End Enum
#End Region

End Class